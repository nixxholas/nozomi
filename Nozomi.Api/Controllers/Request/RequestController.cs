using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Infra.Api.Limiter.Attributes;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Api.Controllers.Request
{
    [Produces("application/json")]
    public class RequestController : BaseApiController<RequestController>, IRequestController
    {
        private readonly INozomiRedisEvent _nozomiRedisEvent;
        private readonly IRequestEvent _requestEvent;
        private readonly IRequestService _requestService;

        public RequestController(ILogger<RequestController> logger,
            INozomiRedisEvent nozomiRedisEvent, IRequestEvent requestEvent, IRequestService requestService) 
            : base(logger)
        {
            _nozomiRedisEvent = nozomiRedisEvent;
            _requestEvent = requestEvent;
            _requestService = requestService;
        }

        [TokenBucket(Name = "Request/Get", Weight = 1)]
        [HttpGet("{guid}")]
        [SwaggerResponseExample((int) HttpStatusCode.OK, typeof(RequestViewModel.RequestViewModelExample))]
        public IActionResult Get(string guid)
        {
            if (Guid.TryParse(guid, out var parsedGuid))
            {
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
                    return Ok(_requestEvent.GetByGuid(parsedGuid, 
                        _nozomiRedisEvent.GetValue(apiKey).ToString()));

                _logger.LogWarning($"{_controllerName} Get: User managed to bypass the token bucket " +
                                   "attribute without an API key!");
                return new InternalServerErrorObjectResult("Not sure how you got here, but no.");
            }

            return BadRequest("Invalid Guid.");
        }

        [TokenBucket(Name = "Request/Create", Weight = 3)]
        [HttpPost]
        public IActionResult Create(CreateRequestViewModel vm)
        {
            if (vm.IsValid() && HttpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
            {                    
                var userId = _nozomiRedisEvent.GetValue(apiKey, RedisDatabases.ApiKeyUser);

                if (userId.HasValue)
                {
                    _requestService.Create(vm, userId.ToString());

                    return Ok("Request successfully created!");
                }
            }

            return BadRequest("Invalid payload.");
        }
    }
}