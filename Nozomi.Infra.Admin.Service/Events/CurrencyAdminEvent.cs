using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.PartialCurrencyPair;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;

namespace Nozomi.Infra.Admin.Service.Events
{
    public class CurrencyAdminEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, ICurrencyAdminEvent
    {
        private readonly ICurrencyCurrencyPairAdminEvent _currencyCurrencyPairAdminEvent;
        
        public CurrencyAdminEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ICurrencyCurrencyPairAdminEvent currencyCurrencyPairAdminEvent) 
            : base(logger, unitOfWork)
        {
            _currencyCurrencyPairAdminEvent = currencyCurrencyPairAdminEvent;
        }

        public AbbrvUniqueCurrencyResponse GetCurrencyByAbbreviation(string abbreviation)
        {
            // First obtain all 'ABBRV' objects first, 
            var currency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase))
                .Include(c => c.CurrencyType)
                .Include(c => c.AnalysedComponents)
                .Include(c => c.CurrencySource)
                .Include(c => c.CurrencyPairSourceCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                .Include(c => c.CurrencyCurrencyPairs)
                .ThenInclude(pcp => pcp.CurrencyPair)
                .ThenInclude(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.AnalysedComponents)
                .Include(c => c.CurrencyCurrencyPairs)
                .ThenInclude(pcp => pcp.CurrencyPair)
                .ThenInclude(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .Include(c => c.CurrencyRequests)
                .ThenInclude(cr => cr.RequestComponents)
                .ToList();

            if (currency.Count > 0)
            {
                var result = new AbbrvUniqueCurrencyResponse(currency);
                
                return result;
            }

            return null;
        }
    }
}