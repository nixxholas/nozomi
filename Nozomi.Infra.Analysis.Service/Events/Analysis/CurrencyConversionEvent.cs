using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis
{
    public class CurrencyConversionEvent : BaseEvent<CurrencyConversionEvent, NozomiDbContext>, 
        ICurrencyConversionEvent
    {
        public CurrencyConversionEvent(ILogger<CurrencyConversionEvent> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public NozomiResult<IDictionary<string, decimal>> ObtainConversionRates(string abbrv)
        {
            var reqComps = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp => cp.PartialCurrencyPairs
                    .Any(pcp => !pcp.IsMain && pcp.Currency.Abbrv.Equals(abbrv,
                                    StringComparison.InvariantCultureIgnoreCase)))
                .Include(cp => cp.CurrencyPairRequests
                    .Where(cpr => cpr.IsEnabled && cpr.DeletedAt == null))
                .ThenInclude(cpr => cpr.RequestComponents
                    .Where(rc => rc.IsEnabled && rc.DeletedAt == null))
                .ThenInclude(rc => rc.RequestComponentDatum)
                .SelectMany(cp => cp.CurrencyPairRequests)
                .SelectMany(cpr => cpr.RequestComponents)
                .ToList();
                
            throw new System.NotImplementedException();
        }
    }
}