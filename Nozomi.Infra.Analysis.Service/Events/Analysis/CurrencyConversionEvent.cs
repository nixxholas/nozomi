using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
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
            // Setups
            var res = new Dictionary<string, decimal>();
            var uncomputedRes = new Dictionary<string, ICollection<decimal>>();
            
            // Obtain all components.
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

            // Obtain the uncomputed first
            foreach (var reqComp in reqComps)
            {
                var reqCompType = reqComp.ComponentType.GetDescription();
                
                if (res.ContainsKey(reqCompType))
                {
                    uncomputedRes[reqCompType].Add(decimal.Parse(reqComp.RequestComponentDatum.Value));
                }
                else
                {
                    uncomputedRes.Add(reqCompType, new List<decimal>
                    {
                        decimal.Parse(reqComp.RequestComponentDatum.Value)
                    });
                }
            }
            
            // Aggregate it
            foreach (var kvp in uncomputedRes)
            {
                // Check
                if (kvp.Value != null && kvp.Value.Count > 0)
                {
                    if (!res.ContainsKey(kvp.Key)) // Extra checks
                    {
                        res.Add(kvp.Key, kvp.Value.Average());
                    }
                }
            }

            return new NozomiResult<IDictionary<string, decimal>>(res);
        }
    }
}