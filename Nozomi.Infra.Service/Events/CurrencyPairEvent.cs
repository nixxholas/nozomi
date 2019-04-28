using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyPairEvent : BaseEvent<CurrencyPairEvent, NozomiDbContext>, ICurrencyPairEvent
    {
        public CurrencyPairEvent(ILogger<CurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyPair> GetAllByCounterCurrency(string counterCurrencyAbbrv = CoreConstants.GenericCounterCurrency)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                .Where(pcp => pcp.CurrencyPairCurrencies.FirstOrDefault(ccp => ccp.Currency.Abbrv
                        .Equals(ccp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase)).Currency.Abbrv
                    .Contains(counterCurrencyAbbrv, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}