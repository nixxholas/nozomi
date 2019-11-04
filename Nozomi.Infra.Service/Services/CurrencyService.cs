using System;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyService : BaseService<CurrencyService, NozomiDbContext>, ICurrencyService
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        
        public CurrencyService(ILogger<CurrencyService> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ICurrencyEvent currencyEvent, ICurrencyTypeEvent currencyTypeEvent) 
            : base(logger, unitOfWork)
        {
            _currencyEvent = currencyEvent;
            _currencyTypeEvent = currencyTypeEvent;
        }

        public void Create(CreateCurrencyViewModel vm, string userId)
        {
            if (vm.IsValid() && string.IsNullOrWhiteSpace(userId))
            {
                if (_currencyEvent.Exists(vm.Slug))
                    return;
                
                // Obtain the currency type
                var currencyType = _currencyTypeEvent.Get(vm.CurrencyTypeGuid.ToString());

                if (currencyType == null)
                    throw new Exception("Currency type not found."); // TODO: Custom exception
                
                // Time to create
                var currency = new Currency(currencyType.Id, vm.LogoPath, vm.Abbreviation, vm.Slug, vm.Name, 
                    vm.Description, vm.Denominations, vm.DenominationName);
                
                _unitOfWork.GetRepository<Currency>().Add(currency);
                _unitOfWork.Commit(userId);
                return;
            }
            
            throw new ArgumentException("Invalid payload.");
        }
    }
}