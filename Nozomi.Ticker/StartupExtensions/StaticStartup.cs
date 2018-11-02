using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                        NozomiServiceConstants.CurrencyPairDictionary.Add(
                            new Tuple<long, string>(cp.CurrencySourceId, string.Join("", cp.PartialCurrencyPairs
                                    .OrderByDescending(pcp => pcp.IsMain)
                                    .Select(pcp => pcp.Currency.Abbrv))), 
                            cp.CurrencyPairRequests.First().RequestComponents.Select(rc => 
                                rc.Id.ToString(CultureInfo.InvariantCulture)).ToArray());
                    }
                }
            }
        }
    }
}