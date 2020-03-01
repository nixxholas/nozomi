using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.ViewModels.Source;
using Nozomi.Preprocessing.Attributes;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Source
{
    public class SourceController : BaseApiController<SourceController>, ISourceController
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceService _sourceService;

        public SourceController(ILogger<SourceController> logger, ICurrencyEvent currencyEvent,
            ISourceEvent sourceEvent, ISourceService sourceService)
            : base(logger)
        {
            _currencyEvent = currencyEvent;
            _sourceEvent = sourceEvent;
            _sourceService = sourceService;
        }

        [HttpGet("{slug}")]
        [Throttle(Milliseconds = 1000)]
        public IActionResult CountByCurrency([FromRoute]string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return BadRequest("Invalid slug.");

            return Ok(_currencyEvent.SourceCount(slug));
        }

        [HttpGet]
        [Throttle(Milliseconds = 1000)]
        public IActionResult All()
        {
            return Ok(_sourceEvent.GetAll());
        }

        [Authorize]
        [HttpPost]
        [Throttle(Milliseconds = 1000)]
        public IActionResult Create(CreateSourceViewModel vm)
        {
            var sub = ((ClaimsIdentity) User.Identity)
                .Claims.SingleOrDefault(c => c.Type.Equals(JwtClaimTypes.Subject))?.Value;

            if (!string.IsNullOrWhiteSpace(sub))
            {
                _sourceService.Create(vm, sub);

                return Ok();
            }

            return BadRequest("Please re-authenticate again");
        }

//        [HttpGet]
//        public NozomiResult<ICollection<Data.Models.Currency.Source>> All()
//        {
//            return new NozomiResult<ICollection<Data.Models.Currency.Source>>(
//                _sourceEvent.GetAllActive(true).ToList());
//        }

        [HttpGet("{slug}")]
        [Throttle(Milliseconds = 1000)]
        public NozomiResult<ICollection<Data.Models.Currency.Source>> GetCurrencySources(string slug, int page = 0)
        {
            return new NozomiResult<ICollection<Data.Models.Currency.Source>>(
                _sourceEvent.GetCurrencySources(slug, page).ToList());
        }

        [HttpGet("{slug}")]
        [Throttle(Milliseconds = 1000)]
        public IActionResult ListByCurrency([FromRoute]string slug, [FromQuery]int page = 0, [FromQuery]int itemsPerPage = 50)
        {
            if (string.IsNullOrWhiteSpace(slug) || page < 0 || itemsPerPage < 1)
                return BadRequest("Invalid parameters.");
            
            return Ok(_currencyEvent.ListSources(slug, page, itemsPerPage));
        }
    }
}