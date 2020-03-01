using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.CurrencyType;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Preprocessing.Statics;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.CurrencyType
{
    public class CurrencyTypeController : BaseApiController<CurrencyTypeController>, ICurrencyTypeController
    {
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        private readonly ICurrencyTypeService _currencyTypeService;

        public CurrencyTypeController(ILogger<CurrencyTypeController> logger,
            IAnalysedComponentEvent analysedComponentEvent, ICurrencyTypeEvent currencyTypeEvent,
            ICurrencyTypeService currencyTypeService)
            : base(logger)
        {
            _analysedComponentEvent = analysedComponentEvent;
            _currencyTypeEvent = currencyTypeEvent;
            _currencyTypeService = currencyTypeService;
        }

        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult All([FromQuery]int index = 0, [FromQuery]int itemsPerPage = 200)
        {
            return Ok(_currencyTypeEvent.All());
        }

        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPost]
        [Throttle(Milliseconds = 1000)]
        public IActionResult Create([FromBody]CreateCurrencyTypeViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _currencyTypeService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }

        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult ListAll([FromQuery]int page = 0, [FromQuery]int itemsPerPage = 50, [FromQuery]bool orderAscending = true, 
            [FromQuery]string orderingParam = "TypeShortForm")
        {
            return Ok(_currencyTypeEvent.ListAll(page, itemsPerPage, orderAscending, orderingParam));
        }
        
        [Authorize(Roles = NozomiPermissions.AllowAllStaffRoles)]
        [HttpPut]
        [Throttle(Milliseconds = 1000)]
        public IActionResult Update([FromBody]UpdateCurrencyTypeViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _currencyTypeService.Update(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }
    }
}
