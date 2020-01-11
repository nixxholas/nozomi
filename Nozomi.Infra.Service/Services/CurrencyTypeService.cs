using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencyType;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyTypeService : BaseService<CurrencyTypeService, NozomiDbContext>, ICurrencyTypeService
    {
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        
        public CurrencyTypeService(ILogger<CurrencyTypeService> logger, 
            ICurrencyTypeEvent currencyTypeEvent, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _currencyTypeEvent = currencyTypeEvent;
        }

        public CurrencyTypeService(IHttpContextAccessor contextAccessor, ILogger<CurrencyTypeService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void Create(CreateCurrencyTypeViewModel vm, string userId = null)
        {
            if (vm.IsValid() && !_currencyTypeEvent.Exists(vm.TypeShortForm))
            {
                var currencyType = new CurrencyType(vm.TypeShortForm, vm.Name);
                
                _unitOfWork.GetRepository<CurrencyType>().Add(currencyType);
                _unitOfWork.Commit(userId);

                return;
            }
            
            throw new ArgumentOutOfRangeException("Invalid payload or the currency type already exists.");
        }

        public void Update(UpdateCurrencyTypeViewModel vm, string userId = null)
        {
            throw new System.NotImplementedException();
        }
    }
}