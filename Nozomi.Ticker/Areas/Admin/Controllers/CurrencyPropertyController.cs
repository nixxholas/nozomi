using System.Threading.Tasks;
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
        public IActionResult Create([FromBody]CreateCurrencyProperty currencyProperty)
        {
            if (ModelState.IsValid)
            {
                var res = _currencyPropertyService.Create(new Data.Models.Currency.CurrencyProperty
                {
                    Type = currencyProperty.Type,
                    Value = currencyProperty.Value,
                    CurrencyId = currencyProperty.CurrencyId,
                    IsEnabled = currencyProperty.IsEnabled
                });

                return res > 0 ? (IActionResult) Ok(new NozomiResult<string>(NozomiResultType.Success,
                    "Currency Property successfully created!")) 
                    : BadRequest(new NozomiResult<string>(NozomiResultType.Failed,
                    "Invalid payload."));
            }
            
            return BadRequest(new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload."));
        }    

        [HttpPut]
        public IActionResult Update([FromBody] UpdateCurrencyProperty currencyProperty)
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
                
                return res ? (IActionResult) Ok(new NozomiResult<string>(NozomiResultType.Success, 
                        "Property successfully updated!"))
                : BadRequest(new NozomiResult<string>(NozomiResultType.Failed, 
                    "Please make sure you're modifying a valid property."));
            }
            
            return BadRequest(new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload"));
        }

        [HttpDelete("{currencyPropertyId}")]
        public IActionResult Delete(long currencyPropertyId)
        {
            var res = _currencyPropertyService.Delete(currencyPropertyId);
            return res ? (IActionResult) Ok(new NozomiResult<string>(NozomiResultType.Success,"Property successfully deleted!")) 
                : NotFound(new NozomiResult<string>(NozomiResultType.Failed, "Deletion unsuccessful."));
        }
    }
}