using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.CurrencySource;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.CurrencySource
{
    public class CurrencySourceController : BaseApiController<CurrencySourceController>, ICurrencySourceController
    {
        private readonly ICurrencySourceService _currencySourceService;
        
        public CurrencySourceController(ILogger<CurrencySourceController> logger,
            ICurrencySourceService currencySourceService) 
            : base(logger)
        {
            _currencySourceService = currencySourceService;
        }

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        public IActionResult Create([FromBody]CreateCurrencySourceViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _currencySourceService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }
    }
}