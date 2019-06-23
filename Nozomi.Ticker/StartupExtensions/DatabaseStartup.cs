using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                        //context.Database.EnsureDeleted();

                        context.Database.EnsureCreated();
                    }
                    else if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                    if (!context.CurrencyTypes.Any())
                    {
                        context.CurrencyTypes.AddRange(
                            new CurrencyType()
                            {
                                TypeShortForm = "FIAT",
                                Name = "FIAT Cash"
                            },
                            new CurrencyType()
                            {
                                TypeShortForm = "CRYPTO",
                                Name = "Cryptocurrency"
                            });
                    }

                    if (!context.Currencies.Any() && context.CurrencyTypes.Any())
                    {
                        context.Currencies.AddRange(
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("FIAT",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "USD",
                                Slug = "USD",
                                Name = "United States Dollar"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("FIAT",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "EUR",
                                Slug = "EUR",
                                Name = "Euro"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "ETH",
                                Slug = "ETH",
                                Name = "Ethereum",
                                Denominations = 18,
                                DenominationName = "Wei"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "KNC",
                                Slug = "KNC",
                                Name = "Kyber Network Crystal",
                                Denominations = 18,
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "BTC",
                                Slug = "BTC",
                                Name = "Bitcoin",
                                Denominations = 8,
                                DenominationName = "Sat"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "BCN",
                                Slug = "BCN",
                                Name = "Bytecoin"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "BTS",
                                Slug = "BTS",
                                Name = "BitShares"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "USDT",
                                Slug = "USDT",
                                Name = "Tether"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("FIAT",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "SGD",
                                Slug = "SGD",
                                Name = "Singapore Dollar"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Abbreviation = "LTC",
                                Slug = "LTC",
                                Name = "Litecoin"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "XRP",
                                Abbreviation = "XRP",
                                Slug = "XRP"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Bitcoin Cash",
                                Abbreviation = "BCH",
                                Slug = "BCH"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "EOS",
                                Abbreviation = "EOS",
                                Slug = "EOS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Binance Coin",
                                Abbreviation = "BNB",
                                Slug = "BNB"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Stellar",
                                Abbreviation = "XLM",
                                Slug = "XLM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Cardano",
                                Abbreviation = "ADA",
                                Slug = "ADA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "TRON",
                                Abbreviation = "TRX",
                                Slug = "TRX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Bitcoin SV",
                                Abbreviation = "BSV",
                                Slug = "BSV"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Monero",
                                Abbreviation = "XMR",
                                Slug = "XMR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Dash",
                                Abbreviation = "DASH",
                                Slug = "DASH"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "IOTA",
                                Abbreviation = "MIOTA",
                                Slug = "MIOTA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Tezos",
                                Abbreviation = "XTZ",
                                Slug = "XTZ"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Cosmos",
                                Abbreviation = "ATOM",
                                Slug = "ATOM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Ethereum Classic",
                                Abbreviation = "ETC",
                                Slug = "ETC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "NEM",
                                Abbreviation = "XEM",
                                Slug = "XEM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "NEO",
                                Abbreviation = "NEO",
                                Slug = "NEO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Maker",
                                Abbreviation = "MKR",
                                Slug = "MKR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Ontology",
                                Abbreviation = "ONT",
                                Slug = "ONT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Zcash",
                                Abbreviation = "ZEC",
                                Slug = "ZEC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Basic Attention Token",
                                Abbreviation = "BAT",
                                Slug = "BAT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Crypto.com Chain",
                                Abbreviation = "CRO",
                                Slug = "CRO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Bitcoin Gold",
                                Abbreviation = "BTG",
                                Slug = "BTG"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "VeChain",
                                Abbreviation = "VET",
                                Slug = "VET"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Chainlink",
                                Abbreviation = "LINK",
                                Slug = "LINK"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "USD Coin",
                                Abbreviation = "USDC",
                                Slug = "USDC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Dogecoin",
                                Abbreviation = "DOGE",
                                Slug = "DOGE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Aurora",
                                Abbreviation = "AOA",
                                Slug = "AOA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "OmiseGO",
                                Abbreviation = "OMG",
                                Slug = "OMG"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Qtum",
                                Abbreviation = "QTUM",
                                Slug = "QTUM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Decred",
                                Abbreviation = "DCR",
                                Slug = "DCR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Waves",
                                Abbreviation = "WAVES",
                                Slug = "WAVES"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "BitTorrent",
                                Abbreviation = "BTT",
                                Slug = "BTT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Holo",
                                Abbreviation = "HOT",
                                Slug = "HOT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "TrueUSD",
                                Abbreviation = "TUSD",
                                Slug = "TUSD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Lisk",
                                Abbreviation = "LSK",
                                Slug = "LSK"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Nano",
                                Abbreviation = "NANO",
                                Slug = "NANO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Augur",
                                Abbreviation = "REP",
                                Slug = "REP"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Bitcoin Diamond",
                                Abbreviation = "BCD",
                                Slug = "BCD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "0x",
                                Abbreviation = "ZRX",
                                Slug = "ZRX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Ravencoin",
                                Abbreviation = "RVN",
                                Slug = "RVN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "DigiByte",
                                Abbreviation = "DGB",
                                Slug = "DGB"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "ICON",
                                Abbreviation = "ICX",
                                Slug = "ICX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Verge",
                                Abbreviation = "XVG",
                                Slug = "XVG"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Paxos Standard Token",
                                Abbreviation = "PAX",
                                Slug = "PAX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Pundi X",
                                Abbreviation = "NPXS",
                                Slug = "NPXS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Zilliqa",
                                Abbreviation = "ZIL",
                                Slug = "ZIL"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Huobi Token",
                                Abbreviation = "HT",
                                Slug = "HT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Aeternity",
                                Abbreviation = "AE",
                                Slug = "AE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "IOST",
                                Abbreviation = "IOST",
                                Slug = "IOST"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Siacoin",
                                Abbreviation = "SC",
                                Slug = "SC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "ABBC Coin",
                                Abbreviation = "ABBC",
                                Slug = "ABBC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Komodo",
                                Abbreviation = "KMD",
                                Slug = "KMD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Enjin Coin",
                                Abbreviation = "ENJ",
                                Slug = "ENJ"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Steem",
                                Abbreviation = "STEEM",
                                Slug = "STEEM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Bytom",
                                Abbreviation = "BTM",
                                Slug = "BTM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Qubitica",
                                Abbreviation = "QBIT",
                                Slug = "QBIT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "THETA",
                                Abbreviation = "THETA",
                                Slug = "THETA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Stratis",
                                Abbreviation = "STRAT",
                                Slug = "STRAT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "ThoreCoin",
                                Abbreviation = "THR",
                                Slug = "THR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "MaidSafeCoin",
                                Abbreviation = "MAID",
                                Slug = "MAID"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Dent",
                                Abbreviation = "DENT",
                                Slug = "DENT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Insight Chain",
                                Abbreviation = "INB",
                                Slug = "INB"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "KuCoin Shares",
                                Abbreviation = "KCS",
                                Slug = "KCS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "SOLVE",
                                Abbreviation = "SOLVE",
                                Slug = "SOLVE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Waltonchain",
                                Abbreviation = "WTC",
                                Slug = "WTC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Golem",
                                Abbreviation = "GNT",
                                Slug = "GNT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "aelf",
                                Abbreviation = "ELF",
                                Slug = "ELF"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Status",
                                Abbreviation = "SNT",
                                Slug = "SNT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Crypto.com",
                                Abbreviation = "MCO",
                                Slug = "MCO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Mixin",
                                Abbreviation = "XIN",
                                Slug = "XIN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Cryptonex",
                                Abbreviation = "CNX",
                                Slug = "CNX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Theta Fuel",
                                Abbreviation = "TFUEL",
                                Slug = "TFUEL"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Ardor",
                                Abbreviation = "ARDR",
                                Slug = "ARDR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "VestChain",
                                Abbreviation = "VEST",
                                Slug = "VEST"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Dai",
                                Abbreviation = "DAI",
                                Slug = "DAI"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Factom",
                                Abbreviation = "FCT",
                                Slug = "FCT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Project Pai",
                                Abbreviation = "PAI",
                                Slug = "PAI"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "WAX",
                                Abbreviation = "WAX",
                                Slug = "WAX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "TrueChain",
                                Abbreviation = "TRUE",
                                Slug = "TRUE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Ark",
                                Abbreviation = "ARK",
                                Slug = "ARK"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Horizen",
                                Abbreviation = "ZEN",
                                Slug = "ZEN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "DigixDAO",
                                Abbreviation = "DGD",
                                Slug = "DGD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Clams",
                                Abbreviation = "CLAM",
                                Slug = "CLAM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "MonaCoin",
                                Abbreviation = "MONA",
                                Slug = "MONA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "GXChain",
                                Abbreviation = "GXC",
                                Slug = "GXC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Decentraland",
                                Abbreviation = "MANA",
                                Slug = "MANA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Orbs",
                                Abbreviation = "ORBS",
                                Slug = "ORBS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Aion",
                                Abbreviation = "AION",
                                Slug = "AION"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Metaverse ETP",
                                Abbreviation = "ETP",
                                Slug = "ETP"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Elastos",
                                Abbreviation = "ELA",
                                Slug = "ELA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Loopring",
                                Abbreviation = "LRC",
                                Slug = "LRC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Loom Network",
                                Abbreviation = "LOOM",
                                Slug = "LOOM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Santiment Network Token",
                                Abbreviation = "SAN",
                                Slug = "SAN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Revain",
                                Abbreviation = "R",
                                Slug = "R"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "NULS",
                                Abbreviation = "NULS",
                                Slug = "NULS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Populous",
                                Abbreviation = "PPT",
                                Slug = "PPT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Matic Network",
                                Abbreviation = "MATIC",
                                Slug = "MATIC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "LATOKEN",
                                Abbreviation = "LA",
                                Slug = "LA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = context.CurrencyTypes
                                    .Where(ct => ct.TypeShortForm.Equals("CRYPTO",
                                        StringComparison.InvariantCultureIgnoreCase))
                                    .Select(ct => ct.Id)
                                    .SingleOrDefault(),
                                Name = "Zcoin",
                                Abbreviation = "XZC",
                                Slug = "XZC"
                            });
                    }

                    if (!context.Sources.Any())
                    {
                        context.Sources.AddRange(
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
                                APIDocsURL =
                                    "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md"
                            },
                            new Source()
                            { 
                                Abbreviation = "ECB",
                                Name = "European Central Bank",
                                APIDocsURL =
                                    "https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html"
                            },
                            new Source()
                            {
                                Abbreviation = "AVG",
                                Name = "AlphaVantage",
                                APIDocsURL = "https://www.alphavantage.co/documentation/"
                            },
                            new Source
                            {
                                Abbreviation = "POLO",
                                Name = "Poloniex",
                                APIDocsURL = "https://docs.poloniex.com/#public-http-api-methods"
                            });
                    }
                    
                    if (!context.CurrencyPairs.Any() && context.Sources.Any())
                    {
                        var bfx = context.Sources
                            .SingleOrDefault(s =>
                                s.Abbreviation.Equals("BFX", StringComparison.InvariantCultureIgnoreCase));
                        var hako = context.Sources
                            .SingleOrDefault(s =>
                                s.Abbreviation.Equals("HAKO", StringComparison.InvariantCultureIgnoreCase));
                        var bna = context.Sources
                            .SingleOrDefault(s =>
                                s.Abbreviation.Equals("BNA", StringComparison.InvariantCultureIgnoreCase));
                        var ecb = context.Sources
                            .SingleOrDefault(s =>
                                s.Abbreviation.Equals("ECB", StringComparison.InvariantCultureIgnoreCase));
                        var avg = context.Sources
                            .SingleOrDefault(s =>
                                s.Abbreviation.Equals("AVG", StringComparison.InvariantCultureIgnoreCase));
                        var polo = context.Sources
                            .SingleOrDefault(s =>
                                s.Abbreviation.Equals("POLO", StringComparison.InvariantCultureIgnoreCase));

                        Debug.Assert(bfx != null, nameof(bfx) + " != null");
                        
                        context.CurrencyPairs.AddRange(
                            new CurrencyPair()
                            {
                                CurrencyPairType = CurrencyPairType.TRADEABLE,
                                APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                                DefaultComponent = "0",
                                SourceId = bfx.Id,
                                MainCurrencyAbbrv = "ETH",
                                CounterCurrencyAbbrv = "USD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.TRADEABLE,
                                APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD",
                                DefaultComponent = "0",
                                SourceId = bfx.Id,
                                MainCurrencyAbbrv = "KNC",
                                CounterCurrencyAbbrv = "USD"
                            },
                            new CurrencyPair()
                            {
                                CurrencyPairType = CurrencyPairType.TRADEABLE,
                                APIUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                                DefaultComponent = "Cube",
                                SourceId = ecb.Id,
                                MainCurrencyAbbrv = "EUR",
                                CounterCurrencyAbbrv = "USD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.TRADEABLE,
                                APIUrl = "https://www.alphavantage.co/query",
                                DefaultComponent = "Realtime Currency Exchange Rate/5. Exchange Rate",
                                SourceId = avg.Id,
                                MainCurrencyAbbrv = "EUR",
                                CounterCurrencyAbbrv = "USD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://poloniex.com/public?command=returnTicker",
                                DefaultComponent = "BTC_BCN/lowestAsk",
                                SourceId = polo.Id,
                                MainCurrencyAbbrv = "BTC",
                                CounterCurrencyAbbrv = "BCN"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://poloniex.com/public?command=returnTicker",
                                DefaultComponent = "BTC_BTS/lowestAsk",
                                SourceId = polo.Id,
                                MainCurrencyAbbrv = "BTC",
                                CounterCurrencyAbbrv = "BTS"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.TRADEABLE,
                                APIUrl = "https://api.bitfinex.com/v1/pubticker/etheur",
                                DefaultComponent = "0",
                                SourceId = bfx.Id,
                                MainCurrencyAbbrv = "ETH",
                                CounterCurrencyAbbrv = "EUR"
                            },
                            new CurrencyPair()
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://poloniex.com/public?command=returnTicker",
                                DefaultComponent = "USDT_BTC/lowestAsk",
                                SourceId = polo.Id,
                                MainCurrencyAbbrv = "BTC",
                                CounterCurrencyAbbrv = "USDT"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                DefaultComponent = "b",
                                SourceId = bna.Id,
                                MainCurrencyAbbrv = "ETH",
                                CounterCurrencyAbbrv = "BTC"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "wss://stream.binance.com:9443/stream?streams=!ticker@arr",
                                DefaultComponent = "b",
                                SourceId = bna.Id,
                                MainCurrencyAbbrv = "KNC",
                                CounterCurrencyAbbrv = "ETH"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://www.coinhako.com/api/v1/price/currency/BTCSGD",
                                DefaultComponent = "data/buy_price",
                                SourceId = hako.Id,
                                MainCurrencyAbbrv = "BTC",
                                CounterCurrencyAbbrv = "SGD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://www.coinhako.com/api/v1/price/currency/BTCUSD",
                                DefaultComponent = "data/buy_price",
                                SourceId = hako.Id,
                                MainCurrencyAbbrv = "BTC",
                                CounterCurrencyAbbrv = "USD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://www.coinhako.com/api/v1/price/currency/ETHSGD",
                                DefaultComponent = "data/buy_price",
                                SourceId = hako.Id,
                                MainCurrencyAbbrv = "ETH",
                                CounterCurrencyAbbrv = "SGD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://www.coinhako.com/api/v1/price/currency/ETHUSD",
                                DefaultComponent = "data/buy_price",
                                SourceId = hako.Id,
                                MainCurrencyAbbrv = "ETH",
                                CounterCurrencyAbbrv = "USD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://www.coinhako.com/api/v1/price/currency/LTCSGD",
                                DefaultComponent = "data/buy_price",
                                SourceId = hako.Id,
                                MainCurrencyAbbrv = "LTC",
                                CounterCurrencyAbbrv = "SGD"
                            },
                            new CurrencyPair
                            {
                                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                                APIUrl = "https://www.coinhako.com/api/v1/price/currency/LTCUSD",
                                DefaultComponent = "data/buy_price",
                                SourceId = hako.Id,
                                MainCurrencyAbbrv = "LTC",
                                CounterCurrencyAbbrv = "USD"
                            });
                    }
                }
            }
        }
    }
}