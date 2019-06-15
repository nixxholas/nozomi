using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyProperty;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Service.Identity.Managers;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyProperty
{
    [ApiController]
    public class CurrencyPropertyController : BaseController<CurrencyPropertyController>, ICurrencyPropertyController
    {
        private readonly ICurrencyPropertyService _currencyPropertyService;
        
        public CurrencyPropertyController(ILogger<CurrencyPropertyController> logger, NozomiUserManager nozomiUserManager,
            ICurrencyPropertyService currencyPropertyService) 
            : base(logger, nozomiUserManager)
        {
            _currencyPropertyService = currencyPropertyService;
        }

        [HttpPost]
        public NozomiResult<string> Create(CreateCurrencyProperty currencyProperty)
        {
            if (currencyProperty.IsValid())
            {
                var res = _currencyPropertyService.Create(new Data.Models.Currency.CurrencyProperty
                {
                    Type = currencyProperty.Type,
                    Value = currencyProperty.Value,
                    CurrencyId = currencyProperty.CurrencyId
                });
                
                return new NozomiResult<string>(res > 0 ? NozomiResultType.Success : NozomiResultType.Failed,
                    res.ToString());
            }
            
            return new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload.");
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