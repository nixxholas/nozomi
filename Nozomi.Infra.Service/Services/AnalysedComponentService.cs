using System;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class AnalysedComponentService : BaseService<AnalysedComponentService, NozomiDbContext>, 
        IAnalysedComponentService
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        
        public AnalysedComponentService(ILogger<AnalysedComponentService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, IAnalysedComponentEvent analysedComponentEvent) 
            : base(logger, unitOfWork)
        {
            _analysedComponentEvent = analysedComponentEvent;
        }

        public void Create(CreateAnalysedComponentViewModel vm, string userId)
        {
            if (vm.IsValid() && _analysedComponentEvent.Exists(vm.Type, vm.CurrencyId, vm.CurrencyPairId, 
                    vm.CurrencyTypeId))
            {
                var analysedComponent = new AnalysedComponent(vm.Type, vm.Delay, vm.UiFormatting, vm.IsDenominated,
                    vm.StoreHistoricals, vm.CurrencyId, vm.CurrencyPairId, vm.CurrencyTypeId);
                
                _unitOfWork.GetRepository<AnalysedComponent>().Add(analysedComponent);
                _unitOfWork.Commit(userId);
            }
        
            throw new InvalidOperationException("Invalid model data.");
        }
    }
}