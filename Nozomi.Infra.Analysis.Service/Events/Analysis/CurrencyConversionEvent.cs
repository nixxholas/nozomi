using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
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
            
            // Obtain all components.
            var cPairs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null
                             && cp.MainCurrency.Equals(abbrv, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.AnalysedComponents
                    .Where(ac => ac.IsEnabled && ac.DeletedAt == null
                                 && ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice)))
                .ToList();

            // Obtain the uncomputed first
            foreach (var cPair in cPairs)
            {
                var counterCurrency = cPair.CounterCurrency.ToUpper();
                
                var avgPrice = cPair.AnalysedComponents.FirstOrDefault(ac =>
                    ac.ComponentType.Equals(AnalysedComponentType.CurrentAveragePrice));

                if (avgPrice != null && decimal.TryParse(avgPrice.Value, out var rate))
                {
                    if (res.ContainsKey(counterCurrency))
                    {
                        // Average it
                        res[counterCurrency] = (res[counterCurrency] + rate) / 2;
                    }
                    else
                    {
                        res.Add(counterCurrency, rate);
                    }
                }
            }

            return new NozomiResult<IDictionary<string, decimal>>(res);
        }
    }
}