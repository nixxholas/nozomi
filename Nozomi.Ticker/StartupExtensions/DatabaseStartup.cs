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
                        //context.Database.EnsureDeleted();

                        context.Database.EnsureCreated();
                    }
                    else if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                    if (!context.Currencies.Any())
                    {
                        context.Currencies.AddRange(
                            new Currency
                            {
                                CurrencyTypeId = 1,
                                Abbreviation = "USD",
                                Slug = "USD",
                                Name = "United States Dollar"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 1,
                                Abbreviation = "EUR",
                                Slug = "EUR",
                                Name = "Euro"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "ETH",
                                Slug = "ETH",
                                Name = "Ethereum",
                                Denominations = 18,
                                DenominationName = "Wei"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "KNC",
                                Slug = "KNC",
                                Name = "Kyber Network Crystal",
                                Denominations = 18,
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "BTC",
                                Slug = "BTC",
                                Name = "Bitcoin",
                                Denominations = 8,
                                DenominationName = "Sat"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "BCN",
                                Slug = "BCN",
                                Name = "Bytecoin"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "BTS",
                                Slug = "BTS",
                                Name = "BitShares"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "USDT",
                                Slug = "USDT",
                                Name = "Tether"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 1,
                                Abbreviation = "SGD",
                                Slug = "SGD",
                                Name = "Singapore Dollar"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Abbreviation = "LTC",
                                Slug = "LTC",
                                Name = "Litecoin"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "XRP",
                                Abbreviation = "XRP",
                                Slug = "XRP"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Bitcoin Cash",
                                Abbreviation = "BCH",
                                Slug = "BCH"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "EOS",
                                Abbreviation = "EOS",
                                Slug = "EOS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Binance Coin",
                                Abbreviation = "BNB",
                                Slug = "BNB"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Stellar",
                                Abbreviation = "XLM",
                                Slug = "XLM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Cardano",
                                Abbreviation = "ADA",
                                Slug = "ADA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "TRON",
                                Abbreviation = "TRX",
                                Slug = "TRX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Bitcoin SV",
                                Abbreviation = "BSV",
                                Slug = "BSV"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Monero",
                                Abbreviation = "XMR",
                                Slug = "XMR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Dash",
                                Abbreviation = "DASH",
                                Slug = "DASH"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "IOTA",
                                Abbreviation = "MIOTA",
                                Slug = "MIOTA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Tezos",
                                Abbreviation = "XTZ",
                                Slug = "XTZ"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Cosmos",
                                Abbreviation = "ATOM",
                                Slug = "ATOM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Ethereum Classic",
                                Abbreviation = "ETC",
                                Slug = "ETC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "NEM",
                                Abbreviation = "XEM",
                                Slug = "XEM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "NEO",
                                Abbreviation = "NEO",
                                Slug = "NEO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Maker",
                                Abbreviation = "MKR",
                                Slug = "MKR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Ontology",
                                Abbreviation = "ONT",
                                Slug = "ONT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Zcash",
                                Abbreviation = "ZEC",
                                Slug = "ZEC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Basic Attention Token",
                                Abbreviation = "BAT",
                                Slug = "BAT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Crypto.com Chain",
                                Abbreviation = "CRO",
                                Slug = "CRO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Bitcoin Gold",
                                Abbreviation = "BTG",
                                Slug = "BTG"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "VeChain",
                                Abbreviation = "VET",
                                Slug = "VET"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Chainlink",
                                Abbreviation = "LINK",
                                Slug = "LINK"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "USD Coin",
                                Abbreviation = "USDC",
                                Slug = "USDC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Dogecoin",
                                Abbreviation = "DOGE",
                                Slug = "DOGE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Aurora",
                                Abbreviation = "AOA",
                                Slug = "AOA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "OmiseGO",
                                Abbreviation = "OMG",
                                Slug = "OMG"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Qtum",
                                Abbreviation = "QTUM",
                                Slug = "QTUM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Decred",
                                Abbreviation = "DCR",
                                Slug = "DCR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Waves",
                                Abbreviation = "WAVES",
                                Slug = "WAVES"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "BitTorrent",
                                Abbreviation = "BTT",
                                Slug = "BTT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Holo",
                                Abbreviation = "HOT",
                                Slug = "HOT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "TrueUSD",
                                Abbreviation = "TUSD",
                                Slug = "TUSD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Lisk",
                                Abbreviation = "LSK",
                                Slug = "LSK"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Nano",
                                Abbreviation = "NANO",
                                Slug = "NANO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Augur",
                                Abbreviation = "REP",
                                Slug = "REP"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Bitcoin Diamond",
                                Abbreviation = "BCD",
                                Slug = "BCD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "0x",
                                Abbreviation = "ZRX",
                                Slug = "ZRX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Ravencoin",
                                Abbreviation = "RVN",
                                Slug = "RVN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "DigiByte",
                                Abbreviation = "DGB",
                                Slug = "DGB"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "ICON",
                                Abbreviation = "ICX",
                                Slug = "ICX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Verge",
                                Abbreviation = "XVG",
                                Slug = "XVG"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Paxos Standard Token",
                                Abbreviation = "PAX",
                                Slug = "PAX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Pundi X",
                                Abbreviation = "NPXS",
                                Slug = "NPXS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Zilliqa",
                                Abbreviation = "ZIL",
                                Slug = "ZIL"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Huobi Token",
                                Abbreviation = "HT",
                                Slug = "HT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Aeternity",
                                Abbreviation = "AE",
                                Slug = "AE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "IOST",
                                Abbreviation = "IOST",
                                Slug = "IOST"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Siacoin",
                                Abbreviation = "SC",
                                Slug = "SC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "ABBC Coin",
                                Abbreviation = "ABBC",
                                Slug = "ABBC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Komodo",
                                Abbreviation = "KMD",
                                Slug = "KMD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Enjin Coin",
                                Abbreviation = "ENJ",
                                Slug = "ENJ"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Steem",
                                Abbreviation = "STEEM",
                                Slug = "STEEM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Bytom",
                                Abbreviation = "BTM",
                                Slug = "BTM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Qubitica",
                                Abbreviation = "QBIT",
                                Slug = "QBIT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "THETA",
                                Abbreviation = "THETA",
                                Slug = "THETA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Stratis",
                                Abbreviation = "STRAT",
                                Slug = "STRAT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "ThoreCoin",
                                Abbreviation = "THR",
                                Slug = "THR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "MaidSafeCoin",
                                Abbreviation = "MAID",
                                Slug = "MAID"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Dent",
                                Abbreviation = "DENT",
                                Slug = "DENT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Insight Chain",
                                Abbreviation = "INB",
                                Slug = "INB"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "KuCoin Shares",
                                Abbreviation = "KCS",
                                Slug = "KCS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "SOLVE",
                                Abbreviation = "SOLVE",
                                Slug = "SOLVE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Waltonchain",
                                Abbreviation = "WTC",
                                Slug = "WTC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Golem",
                                Abbreviation = "GNT",
                                Slug = "GNT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "aelf",
                                Abbreviation = "ELF",
                                Slug = "ELF"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Status",
                                Abbreviation = "SNT",
                                Slug = "SNT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Crypto.com",
                                Abbreviation = "MCO",
                                Slug = "MCO"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Mixin",
                                Abbreviation = "XIN",
                                Slug = "XIN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Cryptonex",
                                Abbreviation = "CNX",
                                Slug = "CNX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Theta Fuel",
                                Abbreviation = "TFUEL",
                                Slug = "TFUEL"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Ardor",
                                Abbreviation = "ARDR",
                                Slug = "ARDR"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "VestChain",
                                Abbreviation = "VEST",
                                Slug = "VEST"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Dai",
                                Abbreviation = "DAI",
                                Slug = "DAI"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Factom",
                                Abbreviation = "FCT",
                                Slug = "FCT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Project Pai",
                                Abbreviation = "PAI",
                                Slug = "PAI"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "WAX",
                                Abbreviation = "WAX",
                                Slug = "WAX"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "TrueChain",
                                Abbreviation = "TRUE",
                                Slug = "TRUE"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Ark",
                                Abbreviation = "ARK",
                                Slug = "ARK"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Horizen",
                                Abbreviation = "ZEN",
                                Slug = "ZEN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "DigixDAO",
                                Abbreviation = "DGD",
                                Slug = "DGD"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Clams",
                                Abbreviation = "CLAM",
                                Slug = "CLAM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "MonaCoin",
                                Abbreviation = "MONA",
                                Slug = "MONA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "GXChain",
                                Abbreviation = "GXC",
                                Slug = "GXC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Decentraland",
                                Abbreviation = "MANA",
                                Slug = "MANA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Orbs",
                                Abbreviation = "ORBS",
                                Slug = "ORBS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Aion",
                                Abbreviation = "AION",
                                Slug = "AION"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Metaverse ETP",
                                Abbreviation = "ETP",
                                Slug = "ETP"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Elastos",
                                Abbreviation = "ELA",
                                Slug = "ELA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Loopring",
                                Abbreviation = "LRC",
                                Slug = "LRC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Loom Network",
                                Abbreviation = "LOOM",
                                Slug = "LOOM"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Santiment Network Token",
                                Abbreviation = "SAN",
                                Slug = "SAN"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Revain",
                                Abbreviation = "R",
                                Slug = "R"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "NULS",
                                Abbreviation = "NULS",
                                Slug = "NULS"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Populous",
                                Abbreviation = "PPT",
                                Slug = "PPT"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Matic Network",
                                Abbreviation = "MATIC",
                                Slug = "MATIC"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "LATOKEN",
                                Abbreviation = "LA",
                                Slug = "LA"
                            },
                            new Currency
                            {
                                CurrencyTypeId = 2,
                                Name = "Zcoin",
                                Abbreviation = "XZC",
                                Slug = "XZC"
                            });
                    }
                }
            }
        }
    }
}