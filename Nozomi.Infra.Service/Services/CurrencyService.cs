using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Statics;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyService : BaseService<CurrencyService, NozomiDbContext>, ICurrencyService
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        
        public CurrencyService(ILogger<CurrencyService> logger, NozomiDbContext context,
            IHttpContextAccessor contextAccessor, ICurrencyEvent currencyEvent, ICurrencyTypeEvent currencyTypeEvent) 
            : base(contextAccessor, logger, context)
        {
            _currencyEvent = currencyEvent;
            _currencyTypeEvent = currencyTypeEvent;
        }

        public void Create(CreateCurrencyViewModel vm, string userId)
        {
            if (vm.IsValid() && !string.IsNullOrWhiteSpace(userId))
            {
                if (_currencyEvent.Exists(vm.Slug))
                    return;

                // Obtain the currency type
                var currencyType = _currencyTypeEvent.Get(vm.CurrencyTypeGuid);

                if (currencyType == null)
                    throw new Exception("Currency type not found."); // TODO: Custom exception
                
                // Time to create
                var currency = new Currency(currencyType.Id, vm.LogoPath, vm.Abbreviation, vm.Slug, vm.Name, 
                    vm.Description, vm.Denominations, vm.DenominationName);
                
                _context.Currencies.Add(currency);
                _context.SaveChanges(userId);
                return;
            }
            
            throw new ArgumentException("Invalid payload.");
        }

        public void Edit(ModifyCurrencyViewModel vm, string userId)
        {
            if (vm.IsValid() && !string.IsNullOrWhiteSpace(userId))
            {
                Currency currency;
                
                // Also check if the user owns this currency first
                if (vm.Id != null && vm.Id > 0) // ID-based currency obtaining
                {
                    currency = _currencyEvent.Get((long) vm.Id);
                } else if (!string.IsNullOrWhiteSpace(vm.Slug) && !string.IsNullOrEmpty(vm.Slug))
                { // Slug-based currency obtaining
                    currency = _currencyEvent.GetBySlug(vm.Slug);
                }
                else
                {
                    throw new Exception("Currency not found.");
                }

                var userRoles = CurrentAccessor().HttpContext.User.Claims
                    .Where(c => c.Type.Equals(JwtClaimTypes.Role) || c.Type.Equals(ClaimTypes.Role))
                    .ToList();
                
                // Null Checks
                if (currency == null ||
                    // Or if the slug is null or if the ID < 0, which counts it as invalid.
                    string.IsNullOrEmpty(currency.Slug) || currency.Id <= 0)
                    throw new InvalidOperationException("Invalid currency.");
                
                // Role Checks
                // If the user in not a staff,
                if (!userRoles.Any(r => NozomiPermissions.AllowAllStaffRoles.Contains(r.Value))
                    // And if the creator is the machine or if the user accessing this API is not the creator,
                    && (currency.CreatedById == null || !currency.CreatedById.Equals(userId)))
                    throw new AccessViolationException("You do not have permissions to modify this currency.");
                
                // Obtain the currency type
                var currencyType = _currencyTypeEvent.Get(vm.CurrencyTypeGuid);

                if (currencyType == null)
                    throw new Exception("Currency type not found."); // TODO: Custom exception

                // Time to create
                var updatedCurrency = _currencyEvent.Get(currency.Id, true);
                
                updatedCurrency.CurrencyTypeId = currencyType.Id;
                updatedCurrency.LogoPath = vm.LogoPath;
                updatedCurrency.Abbreviation = vm.Abbreviation;
                updatedCurrency.Slug = vm.Slug;
                updatedCurrency.Name = vm.Name;
                updatedCurrency.Description = vm.Description;
                updatedCurrency.Denominations = vm.Denominations;
                updatedCurrency.DenominationName = vm.DenominationName;
                
                _context.Currencies.Update(updatedCurrency);
                _context.SaveChanges(userId);
                return;
            }
            
            throw new ArgumentException("Invalid payload.");
        }
    }
}