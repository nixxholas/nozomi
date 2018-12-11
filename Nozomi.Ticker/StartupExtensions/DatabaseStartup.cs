using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data;

namespace Nozomi.Ticker.StartupExtensions
{
    public static class DatabaseStartup
    {
        public static void UseAutoDbMigration(this IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    // Auto Wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }
                    
                    context.Database.EnsureCreated();
                    
                    if (!context.Sources.Any())
                    {
                        context.AddRange(
                            new Source()
                            {
                                Abbreviation = "BFX",
                                Name = "Bitfinex",
                                APIDocsURL = "https://docs.bitfinex.com/docs/introduction"
                            },
                            new Source()
                            {
                                Abbreviation = "HAKO",
                                Name = "Coinhako",
                                APIDocsURL = "None"
                            },
                            new Source()
                            {
                                Abbreviation = "BNA",
                                Name = "Binance",
                                APIDocsURL = "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md"
                            },
                            new Source()
                            {
                                Abbreviation = "ECB",
                                Name = "European Central Bank",
                                APIDocsURL = "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html"
                            },
                            new Source()
                            {
                                Abbreviation = "AVG",
                                Name = "AlphaVantage",
                                APIDocsURL = "https://www.alphavantage.co/documentation/"
                            });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}