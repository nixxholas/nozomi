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
        private readonly Interfaces.ICurrencyPairSourceCurrencyAdminEvent _currencyPairSourceCurrencyAdminEvent;
        
        public CurrencyAdminEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            Interfaces.ICurrencyPairSourceCurrencyAdminEvent currencyPairSourceCurrencyAdminEvent) 
            : base(logger, unitOfWork)
        {
            _currencyPairSourceCurrencyAdminEvent = currencyPairSourceCurrencyAdminEvent;
        }

        public Currency GetCurrencyByAbbreviation(string abbreviation)
        { 
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase))
                .Include(c => c.CurrencyType)
                .Include(c => c.AnalysedComponents)
                .Include(c => c.CurrencySources)
                .ThenInclude(cs => cs.Source)
                .Include(c => c.CurrencySources)
                .ThenInclude(cs => cs.Source)
                .ThenInclude(s => s.CurrencyPairs)
                .Include(c => c.Requests)
                .ThenInclude(cr => cr.RequestComponents)
                .SingleOrDefault();
        }
    }
}