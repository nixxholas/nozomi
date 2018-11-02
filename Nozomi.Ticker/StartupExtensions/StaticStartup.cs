using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.ResponseModels;
using Nozomi.Repo.Data;
using Nozomi.Service;
using Nozomi.Ticker.Areas;
using Swashbuckle.AspNetCore.Swagger;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class StaticStartup
    {
        public static void ConfigureStatics(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    var currencyPairs = context.CurrencyPairs
                        .AsNoTracking()
                        .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                        .Include(cp => cp.CurrencySource)
                        .Include(cp => cp.PartialCurrencyPairs)
                            .ThenInclude(pcp => pcp.Currency)
                        .Include(cp => cp.CurrencyPairRequests)
                        .Where(cp => cp.CurrencyPairRequests.Any(cpr => cpr.DeletedAt == null && cpr.IsEnabled
                                     && cpr.RequestComponents.Any(rc => rc.DeletedAt == null && rc.IsEnabled))
                        && cp.PartialCurrencyPairs.Count == 2);

                    foreach (var cp in currencyPairs)
                    {
                        var tickerSymbol = string.Join("", cp.PartialCurrencyPairs
                            .OrderByDescending(pcp => pcp.IsMain)
                            .Select(pcp => pcp.Currency.Abbrv));
                            
                        // Populate the CurrencySourceSymbolDictionary
                        NozomiServiceConstants.CurrencySourceSymbolDictionary.Add(
                            new Tuple<long, string>(cp.CurrencySourceId, tickerSymbol), 
                            cp.Id);
                        
                        // Populate the TickerSymbolDictionary
                        
                        // Check if the ticker exists in the collection
                        if (NozomiServiceConstants.TickerSymbolDictionary.ContainsKey(tickerSymbol))
                        {
                            var longList = NozomiServiceConstants.TickerSymbolDictionary[tickerSymbol];

                            // Make sure the list already has it
                            if (!longList.Contains(cp.Id))
                            {
                                longList.AddLast(cp.Id);
                            }
                        }
                        else // nope, let's add it in
                        {
                            NozomiServiceConstants.TickerSymbolDictionary.Add(tickerSymbol,
                                new LinkedList<long>());

                            NozomiServiceConstants.TickerSymbolDictionary[tickerSymbol].AddLast(cp.Id);
                        }
                    }

                    var dtrList = context.CurrencyPairRequests
                        .AsNoTracking()
                        .Where(r => r.DeletedAt == null && r.IsEnabled)
                        .Include(r => r.RequestComponents)
                            .ThenInclude(rc => rc.RequestComponentDatum)
                        .Include(r => r.CurrencyPair)
                            .ThenInclude(cp => cp.CurrencySource)
                        .Where(r => r.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null
                                                                  && rc.RequestComponentDatum != null))
                        .Select(cpr => new DiscoverabeTickerResponse()
                        {
                            CurrencyPairId = cpr.CurrencyPairId,
                            Exchange = cpr.CurrencyPair.CurrencySource.Name,
                            ExchangeAbbrv = cpr.CurrencyPair.CurrencySource.Abbreviation,
                            LastUpdated = cpr.RequestComponents.First().RequestComponentDatum.ModifiedAt,
                            Properties = cpr.RequestComponents.Select(rc => 
                                new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                    rc.RequestComponentDatum.Value)).ToList()
                        });

                    foreach (var dtr in dtrList)
                    {
                        if (!NozomiServiceConstants.CurrencyPairDictionary.ContainsKey(dtr.CurrencyPairId))
                        {
                            // Add it in, value can be automatically casted down.
                            NozomiServiceConstants.CurrencyPairDictionary.Add(dtr.CurrencyPairId, dtr);
                        }
                    }
                }
            }
        }
    }
}