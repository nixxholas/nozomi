using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events;

namespace Nozomi.Infra.Admin.Service.Events
{
    public class CurrencyPairSourceCurrencyAdminEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, Interfaces.ICurrencyPairSourceCurrencyAdminEvent
    {
        public CurrencyPairSourceCurrencyAdminEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<Currency> GetCounterCurrenciesByAbbreviation(string mainAbbreviation)
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.MainCurrencyAbbrv.Equals(mainAbbreviation, StringComparison.InvariantCultureIgnoreCase))
                .Include(c => c.CounterCurrency)
                .Select(cp => cp.CounterCurrency)
                .ToList();
        }
    }
}