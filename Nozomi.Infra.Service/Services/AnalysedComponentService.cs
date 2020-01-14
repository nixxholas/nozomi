using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class AnalysedComponentService : BaseService<AnalysedComponentService, NozomiDbContext>, 
        IAnalysedComponentService
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        
        public AnalysedComponentService(ILogger<AnalysedComponentService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, IAnalysedComponentEvent analysedComponentEvent,
            ICurrencyEvent currencyEvent, ICurrencyPairEvent currencyPairEvent, ICurrencyTypeEvent currencyTypeEvent) 
            : base(logger, unitOfWork)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _currencyEvent = currencyEvent;
            _currencyPairEvent = currencyPairEvent;
            _currencyTypeEvent = currencyTypeEvent;
        }

        public void Create(CreateAnalysedComponentViewModel vm, string userId)
        {
            if (vm.IsValid() && !_analysedComponentEvent.Exists(vm.Type, 0, vm.CurrencySlug, 
                    vm.CurrencyPairGuid, vm.CurrencyTypeShortForm))
            {
                long cId = 0, cpId = 0, ctId = 0;
                
                // Look for the FKs
                if (!string.IsNullOrEmpty(vm.CurrencySlug))
                {
                    var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                    if (currency != null)
                        cId = currency.Id;
                    else
                        throw new KeyNotFoundException("Unable to find the appropriate currency.");
                }
                else if (Guid.TryParse(vm.CurrencyPairGuid, out var cpGuid))
                {
                    var currencyPair = _currencyPairEvent.Get(cpGuid);

                    if (currencyPair != null)
                        cpId = currencyPair.Id;
                    else
                        throw new KeyNotFoundException("Unable to find the appropriate currency pair.");
                } else if (!string.IsNullOrEmpty(vm.CurrencyTypeShortForm))
                {
                    var currencyType = _currencyTypeEvent.Get(vm.CurrencyTypeShortForm);

                    if (currencyType != null)
                        cId = currencyType.Id;
                    else
                        throw new KeyNotFoundException("Unable to find the appropriate currency.");
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid identifier for this Analysed Component.");
                }
                
                var analysedComponent = new AnalysedComponent(vm.Type, vm.Delay, vm.UiFormatting, vm.IsDenominated,
                    vm.StoreHistoricals, cId, cpId, ctId);
                
                _unitOfWork.GetRepository<AnalysedComponent>().Add(analysedComponent);
                _unitOfWork.Commit(userId);
                return;
            }
        
            throw new InvalidOperationException("Invalid model data.");
        }

        public void Update(UpdateAnalysedComponentViewModel vm, string userId = null)
        {
            if (vm.IsValid())
            {
                var analysedComponent = _analysedComponentEvent.Pop(vm.Guid);

                if (analysedComponent != null)
                {
                    // Update
                    if (vm.Type != analysedComponent.ComponentType)
                        analysedComponent.ComponentType = vm.Type;

                    if (vm.Delay != analysedComponent.Delay)
                        analysedComponent.Delay = vm.Delay;

                    if (vm.IsDenominated != analysedComponent.IsDenominated)
                        analysedComponent.IsDenominated = vm.IsDenominated;

                    if (vm.StoreHistoricals != analysedComponent.StoreHistoricals)
                        analysedComponent.StoreHistoricals = vm.StoreHistoricals;

                    if (!string.IsNullOrEmpty(vm.CurrencySlug))
                    {
                        // Obtain the currency in question
                        var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                        // Ensure that the component is not currently binded to the aforementioned currency.
                        if (currency != null && !analysedComponent.CurrencyId.Equals(currency.Id))
                            analysedComponent.CurrencyId = currency.Id;
                    }

                    if (Guid.TryParse(vm.CurrencyPairGuid, out var cpGuid))
                    {
                        // Obtain the currency in question
                        var currencyPair = _currencyPairEvent.Get(vm.CurrencyPairGuid);

                        // Ensure that the component is not currently binded to the aforementioned currency.
                        if (currencyPair != null && !analysedComponent.CurrencyPairId.Equals(currencyPair.Id))
                            analysedComponent.CurrencyPairId = currencyPair.Id;
                    }

                    if (!string.IsNullOrEmpty(vm.CurrencyTypeShortForm))
                    {
                        // Obtain the currency in question
                        var currencyType = _currencyTypeEvent.Get(vm.CurrencyTypeShortForm);

                        // Ensure that the component is not currently binded to the aforementioned currency.
                        if (currencyType != null && !analysedComponent.CurrencyTypeId.Equals(currencyType.Id))
                            analysedComponent.CurrencyTypeId = currencyType.Id;
                    }
                    
                    _unitOfWork.GetRepository<AnalysedComponent>().Update(analysedComponent);
                    _unitOfWork.Commit(userId);

                    _logger.LogInformation($"{_serviceName}: Successfully updated AnalysedComponent -> " +
                                           $"{analysedComponent.Guid}");
                    return; // Done
                }
            }
            
            throw new InvalidDataException("Invalid payload!!!");
        }
    }
}