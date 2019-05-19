using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyCurrencyPairEvent : BaseEvent<CurrencyCurrencyPairEvent, NozomiDbContext>, ICurrencyCurrencyPairEvent
    {
        public CurrencyCurrencyPairEvent(ILogger<CurrencyCurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyPairSourceCurrency> ObtainCounterCurrencyPairs(ICollection<CurrencyPairSourceCurrency> mainCCPs)
        {
            if (mainCCPs != null && mainCCPs.Count > 0)
            {
                return _unitOfWork.GetRepository<CurrencyPairSourceCurrency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Include(ccp => ccp.Currency)
                    .Include(ccp => ccp.CurrencyPair)
                    .Where(ccp => ccp.CurrencyPair.CounterCurrency
                                      .Equals(ccp.Currency.Abbreviation))
                    .ToList();
            }

            return null;
        }
    }
}