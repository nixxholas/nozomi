using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Repo.Data;
using Nozomi.Repo.Identity.Data;
using Nozomi.Service.Identity.Events.Interfaces;
using Nozomi.Service.Identity.Managers;
using Nozomi.Service.Identity.Services.Interfaces;

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
                var stripeService = serviceScope.ServiceProvider.GetService<IStripeService>();
                stripeService.ConfigureStripePlans();

                var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();

                using (var context = serviceScope.ServiceProvider.GetService<NozomiAuthContext>())
                {
                    // Auto wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }

                    context.Database.Migrate();

                    // Seed roles
                    using (var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>())
                    {
                        // Iterating Enumerator values.
                        // https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c
                        var roles = Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>();

                        foreach (var role in roles)
                        {
                            var roleStr = role.GetDescription();

                            if (roleManager.FindByNameAsync(roleStr).Result == null)
                            {
                                var newRole = new Role
                                {
                                    Name = roleStr
                                };

                                var res = roleManager.CreateAsync(newRole).Result;

                                if (!res.Succeeded)
                                {
                                    logger.LogCritical($"Error seeding role {newRole.Name}.");
                                }
                            }
                        }
                    }

                    // Seed users
                    using (var userManager = serviceScope.ServiceProvider.GetService<NozomiUserManager>())
                    {
                        // Seed big brother
                        if (userManager.FindByEmailAsync("nixholas@outlook.com").Result == null)
                        {
                            var boss = new User
                            {
                                UserName = "nixholas",
                                NormalizedUserName = "NIXHOLAS",
                                NormalizedEmail = "NIXHOLAS@OUTLOOK.COM",
                                Email = "nixholas@outlook.com",
                                StripeCustomerId = "cus_ELCsKKBzzjNc2I",
                                EmailConfirmed = true
                            };

                            var res = userManager.CreateAsync(boss, "P@ssw0rd").Result;

                            if (!res.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss!!!");
                            }

                            var roleRes = userManager.AddToRoleAsync(boss, RoleEnum.Owner.GetDescription()).Result;

                            if (!roleRes.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss role!!!");
                            }
                        }

                        if (userManager.FindByEmailAsync("nicholas@counter.network").Result == null)
                        {
                            var boss = new User
                            {
                                UserName = "nicholas",
                                NormalizedUserName = "NICHOLAS",
                                NormalizedEmail = "NICHOLAS@COUNTER.NETWORK",
                                Email = "nicholas@counter.network",
                                StripeCustomerId = "cus_ELCsKKBzzjNc2I",
                                EmailConfirmed = true
                            };

                            var res = userManager.CreateAsync(boss, "P@ssw0rd").Result;

                            if (!res.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss!!!");
                            }

                            var roleRes = userManager.AddToRoleAsync(boss, RoleEnum.Owner.GetDescription()).Result;

                            if (!roleRes.Succeeded)
                            {
                                logger.LogCritical($"Error seeding da boss role!!!");
                            }
                        }
                    }
                }

                using (var context = serviceScope.ServiceProvider.GetService<NozomiDbContext>())
                {
                    // Auto Wipe
                    if (env.IsDevelopment())
                    {
                        context.Database.EnsureDeleted();
                    }

                    context.Database.Migrate();

                    if (context.Sources.Any())
                    {
                        var bfxSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("BFX"));
                        var bnaSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("BNA"));
                        var ecbSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("ECB"));
                        var avgSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("AVG"));
                        var poloSource = context.Sources.SingleOrDefault(s => s.Abbreviation.Equals("POLO"));

                        if (context.Currencies.Any())
                        {
                            var usdBfx = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("USD") &&
                                    c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                            var eurBfx = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("EUR") &&
                                    c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                            var ethBfx = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("ETH") &&
                                    c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                            var kncBfx = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("KNC") &&
                                    c.CurrencySource.Abbreviation.Equals(bfxSource.Abbreviation));
                            var kncBna = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("KNC") &&
                                    c.CurrencySource.Abbreviation.Equals(bnaSource.Abbreviation));
                            var ethBna = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("ETH") &&
                                    c.CurrencySource.Abbreviation.Equals(bnaSource.Abbreviation));
                            var btcBna = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BTC") &&
                                    c.CurrencySource.Abbreviation.Equals(bnaSource.Abbreviation));
                            var eurECB = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("EUR") &&
                                    c.CurrencySource.Abbreviation.Equals(ecbSource.Abbreviation));
                            var usdECB = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("USD") &&
                                    c.CurrencySource.Abbreviation.Equals(ecbSource.Abbreviation));
                            var eurAVG = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("EUR") &&
                                    c.CurrencySource.Abbreviation.Equals(avgSource.Abbreviation));
                            var usdAVG = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("USD") &&
                                    c.CurrencySource.Abbreviation.Equals(avgSource.Abbreviation));
                            var btcPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BTC") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));
                            var bcnPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BCN") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));
                            var btsPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("BTS") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));
                            var usdtPOLO = context.Currencies.Include(c => c.CurrencySource)
                                .SingleOrDefault(c =>
                                    c.Abbrv.Equals("USDT") &&
                                    c.CurrencySource.Abbreviation.Equals(poloSource.Abbreviation));

                            if (!context.CurrencyPairs.Any())
                            {
                                if (!context.WebsocketRequests.Any())
                                {
                                    // Binance's Websocket-based ticker data stream
                                    var binanceETHBTCWSR = new WebsocketRequest
                                    {
                                        CurrencyPair = new CurrencyPair
                                        {
                                            CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                            APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                            DefaultComponent = "b",
                                            CurrencySourceId = 3,
                                            CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                            {
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = ethBna.Id
                                                },
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = btcBna.Id
                                                }
                                            },
                                            MainCurrency = ethBna.Abbrv,
                                            CounterCurrency = btcBna.Abbrv
                                        },
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.WebSocket,
                                        ResponseType = ResponseType.Json,
                                        DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                        Delay = 0,
                                        WebsocketCommands = new List<WebsocketCommand>(),
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.VOLUME,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "v"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "a"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask_Size,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "A"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "b"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid_Size,
                                                Identifier = "data/s=>ETHBTC",
                                                QueryComponent = "B"
                                            }
                                        }
                                    };

                                    context.WebsocketRequests.Add(binanceETHBTCWSR);

                                    // Binance's Websocket-based ticker data stream
                                    var binanceKNCETHWSR = new WebsocketRequest
                                    {
                                        CurrencyPair = new CurrencyPair
                                        {
                                            CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                            APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                            DefaultComponent = "b",
                                            CurrencySourceId = 3,
                                            CurrencyPairCurrencies = new List<CurrencyCurrencyPair>
                                            {
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = kncBna.Id
                                                },
                                                new CurrencyCurrencyPair
                                                {
                                                    CurrencyId = ethBna.Id
                                                }
                                            },
                                            MainCurrency = kncBna.Abbrv,
                                            CounterCurrency = ethBna.Abbrv
                                        },
                                        Guid = Guid.NewGuid(),
                                        RequestType = RequestType.WebSocket,
                                        ResponseType = ResponseType.Json,
                                        DataPath = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                        Delay = 0,
                                        WebsocketCommands = new List<WebsocketCommand>(),
                                        RequestComponents = new List<RequestComponent>()
                                        {
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.VOLUME,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "v"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "a"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Ask_Size,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "A"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "b"
                                            },
                                            new RequestComponent
                                            {
                                                ComponentType = ComponentType.Bid_Size,
                                                Identifier = "data/s=>KNCETH",
                                                QueryComponent = "B"
                                            }
                                        }
                                    };

                                    context.WebsocketRequests.Add(binanceKNCETHWSR);

                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}