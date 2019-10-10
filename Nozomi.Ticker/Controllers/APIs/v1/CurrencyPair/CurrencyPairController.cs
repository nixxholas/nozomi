﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Hubs;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyPair
{
    [ApiController]
    public class CurrencyPairController : BaseController<CurrencyPairController>, ICurrencyPairController
    {
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyPairService _currencyPairService;
        private readonly ITickerEvent _tickerEvent;
        private readonly IHubContext<NozomiStreamHub> _tickerHubContext;

        public CurrencyPairController(IHubContext<NozomiStreamHub> tickerHubContext, UserManager<User> userManager,
            ICurrencyPairEvent currencyPairEvent, ITickerEvent tickerEvent,
            ICurrencyPairService currencyPairService,
            ILogger<CurrencyPairController> logger)
            : base(logger, userManager)
        {
            _tickerHubContext = tickerHubContext;
            _currencyPairEvent = currencyPairEvent;
            _tickerEvent = tickerEvent;
            _currencyPairService = currencyPairService;
        }

        [Authorize]
        [HttpPost]
        public async Task<NozomiResult<string>> Create([FromBody] CreateCurrencyPair currencyPair)
        {
//            if (_currencyPairService.Create(currencyPair, 0 /* 0 for now */))
//            {
//                return new NozomiResult<string>()
//                {
//                    ResultType = NozomiResultType.Success,
//                    Data = "Currency pair successfully created!"
//                };
//            }

            return new NozomiResult<string>()
            {
                ResultType = NozomiResultType.Failed,
                Data = "An error has occurred"
            };
        }

        [HttpGet("{id}")]
        public Task Get(long id)
        {
            return _tickerEvent.GetById(id);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{abbrv}")]
        public NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>> Ticker(string abbrv)
        {
            return new NozomiResult<ICollection<Data.Models.Currency.CurrencyPair>>(
                _currencyPairEvent.GetAllByTickerPairAbbreviation(abbrv, true));
        }
    }
}