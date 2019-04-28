using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Analysis.Base.Domain.Responses.Hub.Asset;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Analysis.Service.Events.Analysis.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Analysis.Service.Events.Analysis
{
    public class AnalysedResponseEvent : BaseEvent<AnalysedResponseEvent, NozomiDbContext>, 
        IAnalysedResponseEvent
    {
        public AnalysedResponseEvent(ILogger<AnalysedResponseEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<AssetResponse> GetAllAssetResponses(long index = 0)
        {
            var res = new List<AssetResponse>();

            var cPairs = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.IsEnabled && cp.DeletedAt == null)
                .Include(cp => cp.CurrencyPairRequests
                    .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled))
                .ThenInclude(cpr => cpr.AnalysedComponents
                    .Where(ac => ac.DeletedAt == null && ac.IsEnabled))
                .Where(cp => cp.CurrencyPairRequests.Any() 
                             && cp.CurrencyPairRequests.Any(cpr => cpr.AnalysedComponents.Any()))
                .Include(cp => cp.CurrencyPairCurrencies)
                .ThenInclude(pcp => pcp.Currency)
                .Where(cp => cp.CurrencyPairCurrencies.All(pcp => 
                    pcp.Currency.IsEnabled && pcp.Currency.DeletedAt == null));
            
            // TODO: OPTIMIZE.
            if (cPairs.Any())
            {
                // Iterate the pairs, because some pairs are identical.
                foreach (var cPair in cPairs)
                {
                    var mainCurrency = cPair.CurrencyPairCurrencies
                        .FirstOrDefault(ccp => ccp.Currency.Abbrv
                            .Equals(ccp.CurrencyPair.MainCurrency, StringComparison.InvariantCultureIgnoreCase))?.Currency;

                    // Null checks
                    if (mainCurrency != null)
                    {
                        // Does the currency already exist?
                        if (res.Any(item => item.Abbrv.Equals(mainCurrency.Abbrv,
                            StringComparison.InvariantCultureIgnoreCase)))
                        {
                            // Yes, populate the props.
                            var currency = res.SingleOrDefault(item => item.Abbrv.Equals(mainCurrency.Abbrv,
                                StringComparison.InvariantCultureIgnoreCase));
                            
                            // Null checks
                            if (currency != null)
                            {
                                if (currency.Properties == null)
                                    currency.Properties = new Dictionary<string, string>();
                                
                                // Iterate the props
                                foreach (var currPairReq in cPair.CurrencyPairRequests
                                    .Where(cpr => cpr.AnalysedComponents.Any()))
                                {
                                    foreach (var component in currPairReq.AnalysedComponents)
                                    {
                                        if (currency.Properties.ContainsKey(component.ComponentType.GetDescription()))
                                        {
                                            // is the value a numerical?
                                            if (decimal.TryParse(currency.Properties[component.ComponentType.GetDescription()]
                                                , out var num) && decimal.TryParse(component.Value, out var num1))
                                            {
                                                // Yup, aggregate
                                                currency.Properties[component.ComponentType.GetDescription()]
                                                    = ((num + num1) / 2).ToString(CultureInfo.InvariantCulture);
                                            }
                                            else
                                            {
                                                // Don't do anything for now.
                                            }
                                        }
                                        else
                                        {
                                            currency.Properties.Add(component.ComponentType.GetDescription(),
                                                component.Value);
                                        }
                                    }
                                }
                            }
                        } else 
                        // No, populate the currency.
                        {
                            var newCurr = new AssetResponse
                            {
                                Abbrv = mainCurrency.Abbrv,
                                Name = mainCurrency.Name,
                                Properties = new Dictionary<string, string>()
                            };
                            
                            // Populate the props
                            foreach (var currPairReq in cPair.CurrencyPairRequests)
                            {
                                foreach (var component in currPairReq.AnalysedComponents)
                                {
                                    if (newCurr.Properties.ContainsKey(component.ComponentType.GetDescription()))
                                    {
                                        // is the value a numerical?
                                        if (decimal.TryParse(newCurr.Properties[component.ComponentType.GetDescription()]
                                                , out var num) && decimal.TryParse(component.Value, out var num1))
                                        {
                                            // Yup, aggregate
                                            newCurr.Properties[component.ComponentType.GetDescription()]
                                                = ((num + num1) / 2).ToString(CultureInfo.InvariantCulture);
                                        }
                                        else
                                        {
                                            // Don't do anything for now.
                                        }
                                    }
                                    else
                                    {
                                        newCurr.Properties.Add(component.ComponentType.GetDescription(),
                                            component.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return res;
        }
    }
}