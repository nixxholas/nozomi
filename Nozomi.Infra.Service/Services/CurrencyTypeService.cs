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
            ICurrencyTypeEvent currencyTypeEvent, IUnitOfWork<NozomiDbContext> context) : base(logger, context)
        {
            _currencyTypeEvent = currencyTypeEvent;
        }

        public void Create(CreateCurrencyTypeViewModel vm, string userId = null)
        {
            if (vm.IsValid() && !_currencyTypeEvent.Exists(vm.TypeShortForm))
            {
                var currencyType = new CurrencyType(vm.TypeShortForm, vm.Name);
                
                _context.GetRepository<CurrencyType>().Add(currencyType);
                _context.Commit(userId);

                return;
            }
            
            throw new ArgumentOutOfRangeException("Invalid payload or the currency type already exists.");
        }

        public void Update(UpdateCurrencyTypeViewModel vm, string userId = null)
        {
            if (vm.IsValid() && _currencyTypeEvent.Exists(vm.Guid))
            {
                var currencyType = _currencyTypeEvent.Pop(vm.Guid);

                if (vm.IsEnabled != null)
                    currencyType.IsEnabled = (bool) vm.IsEnabled;

                if (vm.Delete)
                {
                    currencyType.DeletedAt = DateTime.UtcNow;
                    currencyType.DeletedById = userId;
                    
                    _context.GetRepository<CurrencyType>().Update(currencyType);
                    _context.Commit(userId);

                    return; // Do not execute further since we're done
                }                    

                if (!string.IsNullOrEmpty(vm.TypeShortForm)
                    && !_currencyTypeEvent.Exists(vm.TypeShortForm))
                    currencyType.TypeShortForm = vm.TypeShortForm;

                if (!string.IsNullOrEmpty(vm.Name))
                    currencyType.Name = vm.Name;
                
                _context.GetRepository<CurrencyType>().Update(currencyType);
                _context.Commit(userId);

                return; // Do not execute further since we're done
            }
            
            throw new ArgumentNullException("Invalid payload, the currency type does not exist.");
        }
    }
}