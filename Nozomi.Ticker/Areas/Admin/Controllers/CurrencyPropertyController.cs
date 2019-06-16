using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyProperty;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Ticker.Controllers;

namespace Nozomi.Ticker.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Owner, Administrator, Staff")]
    public class CurrencyPropertyController : AreaBaseViewController<CurrencyPropertyController>
    {
        private readonly ICurrencyPropertyService _currencyPropertyService;
        
        public CurrencyPropertyController(ILogger<CurrencyPropertyController> logger, NozomiSignInManager signInManager,
            NozomiUserManager userManager, ICurrencyPropertyService currencyPropertyService) 
            : base(logger, signInManager, userManager)
        {
            _currencyPropertyService = currencyPropertyService;
        }

        [HttpPost]
        public NozomiResult<string> Create([FromBody]CreateCurrencyProperty currencyProperty)
        {
            if (currencyProperty.IsValid())
            {
                var res = _currencyPropertyService.Create(new Data.Models.Currency.CurrencyProperty
                {
                    Type = currencyProperty.Type,
                    Value = currencyProperty.Value,
                    CurrencyId = currencyProperty.CurrencyId,
                    IsEnabled = currencyProperty.IsEnabled
                });
                
                return new NozomiResult<string>(res > 0 ? NozomiResultType.Success : NozomiResultType.Failed,
                    res.ToString());
            }
            
            return new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload.");
        }    

        [HttpPut]
        public NozomiResult<string> Update([FromBody] UpdateCurrencyProperty currencyProperty)
        {
            if (ModelState.IsValid)
            {
                var res = _currencyPropertyService.Update(new CurrencyProperty
                {
                    Id = currencyProperty.Id,
                    Type = currencyProperty.Type,
                    Value = currencyProperty.Value,
                    IsEnabled = currencyProperty.IsEnabled
                });
                
                return new NozomiResult<string>();
            }
            
            return new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload");
        }

        [HttpDelete("{currencyPropertyId}")]
        public NozomiResult<string> Delete(long currencyPropertyId)
        {
            var res = _currencyPropertyService.Delete(currencyPropertyId);
            return new NozomiResult<string>(res ? NozomiResultType.Success : NozomiResultType.Failed,
                res ? "Property successfully deleted!" : "Deletion unsuccessful.");
        }
    }
}