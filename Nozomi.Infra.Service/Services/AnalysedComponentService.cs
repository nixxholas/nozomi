using System;
using System.Collections.Generic;
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
    }
}