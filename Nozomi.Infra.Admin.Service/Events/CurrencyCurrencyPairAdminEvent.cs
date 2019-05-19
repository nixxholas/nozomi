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
    public class CurrencyCurrencyPairAdminEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, ICurrencyCurrencyPairAdminEvent
    {
        public CurrencyCurrencyPairAdminEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyPairSourceCurrency> GetCounterCurrenciesByAbbreviation(string mainAbbreviation)
        {
            return _unitOfWork.GetRepository<CurrencyPairSourceCurrency>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cpsc => cpsc.CurrencySource)
                .ThenInclude(cs => cs.Currency)
                .Include(ccp => ccp.CurrencyPair)
                .Where(ccp => ccp.CurrencyPair.MainCurrency.Equals(mainAbbreviation)
                && !ccp.CurrencySource.Currency.Abbreviation.Equals(mainAbbreviation, 
                    StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}