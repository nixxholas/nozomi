using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Events
{
    public class CurrencyPairSourceCurrencyAdminEvent : BaseEvent<CurrencyPairSourceCurrencyAdminEvent, NozomiDbContext>
        , Interfaces.ICurrencyPairSourceCurrencyAdminEvent
    {
        public CurrencyPairSourceCurrencyAdminEvent(ILogger<CurrencyPairSourceCurrencyAdminEvent> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<Currency> GetCounterCurrenciesByAbbreviation(string mainAbbreviation)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.MainTicker.Equals(mainAbbreviation, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                .SelectMany(cp => cp.Source.SourceCurrencies
                    .Where(sc => sc.Currency.Abbreviation.Equals(cp.CounterTicker))
                    .Select(sc => sc.Currency))
                .ToList();
        }
    }
}