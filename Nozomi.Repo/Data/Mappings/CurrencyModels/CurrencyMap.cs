using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyMap : BaseMap<Currency>
    {
        public CurrencyMap(EntityTypeBuilder<Currency> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(c => c.Id).HasName("Currency_PK_Id");
            entityTypeBuilder.Property(c => c.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(c => c.Abbrv).IsRequired();
            entityTypeBuilder.HasIndex(c => new {c.Abbrv, c.CurrencySourceId})
                .HasName("Currency_Index_Abbrv_CurrencySourceId").IsUnique();
            entityTypeBuilder.Property(c => c.Denominations).HasDefaultValue(0);
            entityTypeBuilder.Property(c => c.DenominationName).IsRequired(false);
            entityTypeBuilder.Property(c => c.Name).IsRequired();

            entityTypeBuilder.HasMany(c => c.AnalysedComponents).WithOne(ac => ac.Currency)
                .HasForeignKey(c => c.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Currency_AnalysedComponents_Constraint")
                .IsRequired(false);
            entityTypeBuilder.HasOne(c => c.CurrencyType).WithMany(ct => ct.Currencies)
                .HasForeignKey(c => c.CurrencyTypeId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Currencies_CurrencyType_Constraint");
            entityTypeBuilder.HasOne(c => c.CurrencySource).WithMany(cs => cs.Currencies)
                .HasForeignKey(c => c.CurrencySourceId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Currencies_CurrencySource_Constraint");
            entityTypeBuilder.HasMany(c => c.CurrencyRequests).WithOne(cr => cr.Currency)
                .HasForeignKey(cr => cr.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Currencies_CurrencyRequests_Constraint");
            entityTypeBuilder.HasMany(c => c.CurrencyCurrencyPairs).WithOne(pcp => pcp.Currency)
                .HasForeignKey(pcp => pcp.CurrencyId)
                .HasConstraintName("Currency_PartialCurrencyPairs_Constraint");

            entityTypeBuilder.HasData(new Currency
                {
                    Id = 1,
                    CurrencyTypeId = 1,
                    Abbrv = "USD",
                    Name = "United States Dollar",
                    CurrencySourceId = 1,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 2,
                    CurrencyTypeId = 1,
                    Abbrv = "EUR",
                    Name = "Euro",
                    CurrencySourceId = 1,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 3,
                    CurrencyTypeId = 2,
                    Abbrv = "ETH",
                    Name = "Ethereum",
                    CurrencySourceId = 1,
                    WalletTypeId = 1, // As per CNWallet
                    Denominations = 18,
                    DenominationName = "Wei",
//                                        // Calculates mCap ONLY for this exact Currency pair on this exchange.
//                                        AnalysedComponents = new List<AnalysedComponent>
//                                        {
//                                            new AnalysedComponent
//                                            {
//                                                ComponentType = AnalysedComponentType.MarketCap,
//                                                Delay = 1000,
//                                                UIFormatting = "$ 0 a",
//                                                CreatedAt = DateTime.UtcNow,
//                                                ModifiedAt = DateTime.UtcNow,
//                                                IsDenominated = true,
//                                                DeletedAt = null
//                                            }
//                                        },
//                                        CurrencyRequests = new List<CurrencyRequest>
//                                        {
//                                            new CurrencyRequest
//                                            {
//                                                Guid = Guid.NewGuid(),
//                                                RequestType = RequestType.HttpGet,
//                                                DataPath = "https://api.etherscan.io/api",
//                                                Delay = 5000,
//                                                RequestComponents = new List<RequestComponent>
//                                                {
//                                                    new RequestComponent
//                                                    {
//                                                        ComponentType = ComponentType.Circulating_Supply,
//                                                        IsDenominated = true,
//                                                        QueryComponent = "result",
//                                                        CreatedAt = DateTime.UtcNow,
//                                                        ModifiedAt = DateTime.UtcNow,
//                                                        DeletedAt = null
//                                                    }
//                                                },
//                                                RequestProperties = new List<RequestProperty>
//                                                {
//                                                    new RequestProperty
//                                                    {
//                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                                        Key = "module",
//                                                        Value = "stats",
//                                                    },
//                                                    new RequestProperty
//                                                    {
//                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                                        Key = "action",
//                                                        Value = "ethsupply",
//                                                    },
//                                                    new RequestProperty
//                                                    {
//                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                                        Key = "apikey",
//                                                        Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
//                                                    }
//                                                }
//                                            }
//                                        }
                },
                new Currency
                {
                    Id = 4,
                    CurrencyTypeId = 2,
                    Abbrv = "KNC",
                    Name = "Kyber Network Crystal",
                    CurrencySourceId = 1,
                    Denominations = 18,
                    WalletTypeId = 4, // As per CNWallet
//                                        // Calculates mCap ONLY for this exact Currency pair on this exchange.
//                                        AnalysedComponents = new List<AnalysedComponent>
//                                        {
//                                            new AnalysedComponent
//                                            {
//                                                ComponentType = AnalysedComponentType.MarketCap,
//                                                Delay = 1000,
//                                                CreatedAt = DateTime.UtcNow,
//                                                ModifiedAt = DateTime.UtcNow,
//                                                IsDenominated = true,
//                                                DeletedAt = null
//                                            }
//                                        },
//                                        CurrencyRequests = new List<CurrencyRequest>
//                                        {
//                                            new CurrencyRequest
//                                            {
//                                                Guid = Guid.NewGuid(),
//                                                RequestType = RequestType.HttpGet,
//                                                DataPath = "https://api.etherscan.io/api",
//                                                Delay = 5000,
//                                                RequestComponents = new List<RequestComponent>
//                                                {
//                                                    new RequestComponent
//                                                    {
//                                                        ComponentType = ComponentType.Circulating_Supply,
//                                                        IsDenominated = true,
//                                                        QueryComponent = "result",
//                                                        CreatedAt = DateTime.UtcNow,
//                                                        ModifiedAt = DateTime.UtcNow,
//                                                        DeletedAt = null
//                                                    }
//                                                },
//                                                RequestProperties = new List<RequestProperty>
//                                                {
//                                                    new RequestProperty
//                                                    {
//                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                                        Key = "module",
//                                                        Value = "stats",
//                                                    },
//                                                    new RequestProperty
//                                                    {
//                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                                        Key = "action",
//                                                        Value = "tokensupply",
//                                                    },
//                                                    new RequestProperty
//                                                    {
//                                                        RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                                        Key = "apikey",
//                                                        Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
//                                                    }
//                                                }
//                                            },
//                                        }
                },
                new Currency
                {
                    Id = 5,
                    CurrencyTypeId = 2,
                    Abbrv = "KNC",
                    Name = "Kyber Network Crystal",
                    CurrencySourceId = 3,
                    WalletTypeId = 4 // As per CNWallet
                },
                new Currency
                {
                    Id = 6,
                    CurrencyTypeId = 2,
                    Abbrv = "ETH",
                    Name = "Ethereum",
                    CurrencySourceId = 3,
                    WalletTypeId = 1, // As per CNWallet
                    Denominations = 18,
                    DenominationName = "Wei",
                },
                new Currency
                {
                    Id = 7,
                    CurrencyTypeId = 2,
                    Abbrv = "BTC",
                    Name = "Bitcoin",
                    CurrencySourceId = 3,
                    WalletTypeId = 0, // As per CNWallet
                    Denominations = 8,
                    DenominationName = "Sat"
                },
                new Currency
                {
                    Id = 8,
                    CurrencyTypeId = 1,
                    Abbrv = "EUR",
                    Name = "Euro",
                    CurrencySourceId = 4,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 9,
                    CurrencyTypeId = 1,
                    Abbrv = "USD",
                    Name = "United States Dollar",
                    CurrencySourceId = 4,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 10,
                    CurrencyTypeId = 1,
                    Abbrv = "EUR",
                    Name = "Euro",
                    CurrencySourceId = 5,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 11,
                    CurrencyTypeId = 1,
                    Abbrv = "USD",
                    Name = "United States Dollar",
                    CurrencySourceId = 5,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 12,
                    CurrencyTypeId = 2,
                    Abbrv = "BTC",
                    Name = "Bitcoin",
                    CurrencySourceId = 6,
                    WalletTypeId = 0,
                    Denominations = 8,
                    DenominationName = "Sat",
//                    CurrencyRequests = new List<CurrencyRequest>()
//                    {
//                        new CurrencyRequest
//                        {
//                            Guid = Guid.NewGuid(),
//                            RequestType = RequestType.HttpGet,
//                            DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
//                            Delay = 90000,
//                            RequestComponents = new List<RequestComponent>
//                            {
//                                new RequestComponent
//                                {
//                                    ComponentType = ComponentType.BlockCount,
//                                    QueryComponent = "info/blocks",
//                                    CreatedAt = DateTime.UtcNow,
//                                    ModifiedAt = DateTime.UtcNow,
//                                    DeletedAt = null
//                                },
//                                new RequestComponent
//                                {
//                                    ComponentType = ComponentType.Difficulty,
//                                    QueryComponent = "info/difficulty",
//                                    CreatedAt = DateTime.UtcNow,
//                                    ModifiedAt = DateTime.UtcNow,
//                                    DeletedAt = null
//                                }
//                            }
//                        },
//                        new CurrencyRequest
//                        {
//                            Guid = Guid.NewGuid(),
//                            RequestType = RequestType.HttpGet,
//                            DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
//                            Delay = 90000,
//                            RequestComponents = new List<RequestComponent>
//                            {
//                                new RequestComponent
//                                {
//                                    ComponentType = ComponentType.Circulating_Supply,
//                                    QueryComponent = "data/coin/circulatingSupply",
//                                    CreatedAt = DateTime.UtcNow,
//                                    ModifiedAt = DateTime.UtcNow,
//                                    DeletedAt = null
//                                }
//                            }
//                        }
//                    }
                },
                new Currency
                {
                    Id = 13,
                    CurrencyTypeId = 2,
                    Abbrv = "BCN",
                    Name = "Bytecoin",
                    CurrencySourceId = 6,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 14,
                    CurrencyTypeId = 2,
                    Abbrv = "BTS",
                    Name = "BitShares",
                    CurrencySourceId = 6,
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 15,
                    CurrencyTypeId = 1,
                    Abbrv = "USDT",
                    Name = "Tether USD",
                    CurrencySourceId = 6,
                    WalletTypeId = 0
                });
        }
    }
}
