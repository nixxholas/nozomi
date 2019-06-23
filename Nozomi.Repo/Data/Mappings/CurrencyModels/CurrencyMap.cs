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

            entityTypeBuilder.HasIndex(c => c.Slug).IsUnique().HasName("Currency_Index_Slug");
            entityTypeBuilder.Property(c => c.Slug).IsRequired();

            entityTypeBuilder.Property(c => c.Abbreviation).IsRequired();
            entityTypeBuilder.Property(c => c.LogoPath).IsRequired().HasDefaultValue("assets/svg/icons/question.svg");
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
            entityTypeBuilder.HasMany(c => c.CurrencySources).WithOne(cs => cs.Currency)
                .HasForeignKey(cs => cs.CurrencyId).OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Currency_CurrencySources_Constraint");
            entityTypeBuilder.HasMany(c => c.Requests).WithOne(r => r.Currency)
                .HasForeignKey(r => r.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Currencies_CurrencyRequests_Constraint")
                .IsRequired(false);

            entityTypeBuilder.HasData(new Currency
                {
                    Id = 1,
                    CurrencyTypeId = 1,
                    Abbreviation = "USD",
                    Slug = "USD",
                    Name = "United States Dollar"
                },
                new Currency
                {
                    Id = 2,
                    CurrencyTypeId = 1,
                    Abbreviation = "EUR",
                    Slug = "EUR",
                    Name = "Euro"
                },
                new Currency
                {
                    Id = 3,
                    CurrencyTypeId = 2,
                    Abbreviation = "ETH",
                    Slug = "ETH",
                    Name = "Ethereum",
                    Denominations = 18,
                    DenominationName = "Wei"
                },
                new Currency
                {
                    Id = 4,
                    CurrencyTypeId = 2,
                    Abbreviation = "KNC",
                    Slug = "KNC",
                    Name = "Kyber Network Crystal",
                    Denominations = 18,
                },
                new Currency
                {
                    Id = 5,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTC",
                    Slug = "BTC",
                    Name = "Bitcoin",
                    Denominations = 8,
                    DenominationName = "Sat"
                },
                new Currency
                {
                    Id = 6,
                    CurrencyTypeId = 2,
                    Abbreviation = "BCN",
                    Slug = "BCN",
                    Name = "Bytecoin"
                },
                new Currency
                {
                    Id = 7,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTS",
                    Slug = "BTS",
                    Name = "BitShares"
                },
                new Currency
                {
                    Id = 8,
                    CurrencyTypeId = 2,
                    Abbreviation = "USDT",
                    Slug = "USDT",
                    Name = "Tether"
                },
                new Currency
                {
                    Id = 9,
                    CurrencyTypeId = 1,
                    Abbreviation = "SGD",
                    Slug = "SGD",
                    Name = "Singapore Dollar"
                },
                new Currency
                {
                    Id = 10,
                    CurrencyTypeId = 2,
                    Abbreviation = "LTC",
                    Slug = "LTC",
                    Name = "Litecoin"
                },
                new Currency
                {
                    Id = 11,
                    CurrencyTypeId = 2,
                    Name = "XRP",
                    Abbreviation = "XRP",
                    Slug = "XRP"
                },
                new Currency
                {
                    Id = 12,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Cash",
                    Abbreviation = "BCH",
                    Slug = "BCH"
                },
                new Currency
                {
                    Id = 13,
                    CurrencyTypeId = 2,
                    Name = "EOS",
                    Abbreviation = "EOS",
                    Slug = "EOS"
                },
                new Currency
                {
                    Id = 14,
                    CurrencyTypeId = 2,
                    Name = "Binance Coin",
                    Abbreviation = "BNB",
                    Slug = "BNB"
                },
                new Currency
                {
                    Id = 15,
                    CurrencyTypeId = 2,
                    Name = "Stellar",
                    Abbreviation = "XLM",
                    Slug = "XLM"
                },
                new Currency
                {
                    Id = 16,
                    CurrencyTypeId = 2,
                    Name = "Cardano",
                    Abbreviation = "ADA",
                    Slug = "ADA"
                },
                new Currency
                {
                    Id = 17,
                    CurrencyTypeId = 2,
                    Name = "TRON",
                    Abbreviation = "TRX",
                    Slug = "TRX"
                },
                new Currency
                {
                    Id = 18,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin SV",
                    Abbreviation = "BSV",
                    Slug = "BSV"
                },
                new Currency
                {
                    Id = 19,
                    CurrencyTypeId = 2,
                    Name = "Monero",
                    Abbreviation = "XMR",
                    Slug = "XMR"
                },
                new Currency
                {
                    Id = 20,
                    CurrencyTypeId = 2,
                    Name = "Dash",
                    Abbreviation = "DASH",
                    Slug = "DASH"
                },
                new Currency
                {
                    Id = 21,
                    CurrencyTypeId = 2,
                    Name = "IOTA",
                    Abbreviation = "MIOTA",
                    Slug = "MIOTA"
                },
                new Currency
                {
                    Id = 22,
                    CurrencyTypeId = 2,
                    Name = "Tezos",
                    Abbreviation = "XTZ",
                    Slug = "XTZ"
                },
                new Currency
                {
                    Id = 23,
                    CurrencyTypeId = 2,
                    Name = "Cosmos",
                    Abbreviation = "ATOM",
                    Slug = "ATOM"
                },
                new Currency
                {
                    Id = 24,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Classic",
                    Abbreviation = "ETC",
                    Slug = "ETC"
                },
                new Currency
                {
                    Id = 25,
                    CurrencyTypeId = 2,
                    Name = "NEM",
                    Abbreviation = "XEM",
                    Slug = "XEM"
                },
                new Currency
                {
                    Id = 26,
                    CurrencyTypeId = 2,
                    Name = "NEO",
                    Abbreviation = "NEO",
                    Slug = "NEO"
                },
                new Currency
                {
                    Id = 27,
                    CurrencyTypeId = 2,
                    Name = "Maker",
                    Abbreviation = "MKR",
                    Slug = "MKR"
                },
                new Currency
                {
                    Id = 28,
                    CurrencyTypeId = 2,
                    Name = "Ontology",
                    Abbreviation = "ONT",
                    Slug = "ONT"
                },
                new Currency
                {
                    Id = 29,
                    CurrencyTypeId = 2,
                    Name = "Zcash",
                    Abbreviation = "ZEC",
                    Slug = "ZEC"
                },
                new Currency
                {
                    Id = 30,
                    CurrencyTypeId = 2,
                    Name = "Basic Attention Token",
                    Abbreviation = "BAT",
                    Slug = "BAT"
                },
                new Currency
                {
                    Id = 31,
                    CurrencyTypeId = 2,
                    Name = "Crypto.com Chain",
                    Abbreviation = "CRO",
                    Slug = "CRO"
                },
                new Currency
                {
                    Id = 2083,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Gold",
                    Abbreviation = "BTG",
                    Slug = "BTG"
                },
                new Currency
                {
                    Id = 3077,
                    CurrencyTypeId = 2,
                    Name = "VeChain",
                    Abbreviation = "VET",
                    Slug = "VET"
                },
                new Currency
                {
                    Id = 1975,
                    CurrencyTypeId = 2,
                    Name = "Chainlink",
                    Abbreviation = "LINK",
                    Slug = "LINK"
                },
                new Currency
                {
                    Id = 3408,
                    CurrencyTypeId = 2,
                    Name = "USD Coin",
                    Abbreviation = "USDC",
                    Slug = "USDC"
                },
                new Currency
                {
                    Id = 74,
                    CurrencyTypeId = 2,
                    Name = "Dogecoin",
                    Abbreviation = "DOGE",
                    Slug = "DOGE"
                },
                new Currency
                {
                    Id = 2874,
                    CurrencyTypeId = 2,
                    Name = "Aurora",
                    Abbreviation = "AOA",
                    Slug = "AOA"
                },
                new Currency
                {
                    Id = 1808,
                    CurrencyTypeId = 2,
                    Name = "OmiseGO",
                    Abbreviation = "OMG",
                    Slug = "OMG"
                },
                new Currency
                {
                    Id = 1684,
                    CurrencyTypeId = 2,
                    Name = "Qtum",
                    Abbreviation = "QTUM",
                    Slug = "QTUM"
                },
                new Currency
                {
                    Id = 1168,
                    CurrencyTypeId = 2,
                    Name = "Decred",
                    Abbreviation = "DCR",
                    Slug = "DCR"
                },
                new Currency
                {
                    Id = 1274,
                    CurrencyTypeId = 2,
                    Name = "Waves",
                    Abbreviation = "WAVES",
                    Slug = "WAVES"
                },
                new Currency
                {
                    Id = 3718,
                    CurrencyTypeId = 2,
                    Name = "BitTorrent",
                    Abbreviation = "BTT",
                    Slug = "BTT"
                },
                new Currency
                {
                    Id = 2682,
                    CurrencyTypeId = 2,
                    Name = "Holo",
                    Abbreviation = "HOT",
                    Slug = "HOT"
                },
                new Currency
                {
                    Id = 2563,
                    CurrencyTypeId = 2,
                    Name = "TrueUSD",
                    Abbreviation = "TUSD",
                    Slug = "TUSD"
                },
                new Currency
                {
                    Id = 1214,
                    CurrencyTypeId = 2,
                    Name = "Lisk",
                    Abbreviation = "LSK",
                    Slug = "LSK"
                },
                new Currency
                {
                    Id = 1567,
                    CurrencyTypeId = 2,
                    Name = "Nano",
                    Abbreviation = "NANO",
                    Slug = "NANO"
                },
                new Currency
                {
                    Id = 1104,
                    CurrencyTypeId = 2,
                    Name = "Augur",
                    Abbreviation = "REP",
                    Slug = "REP"
                },
                new Currency
                {
                    Id = 2222,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Diamond",
                    Abbreviation = "BCD",
                    Slug = "BCD"
                },
                new Currency
                {
                    Id = 1896,
                    CurrencyTypeId = 2,
                    Name = "0x",
                    Abbreviation = "ZRX",
                    Slug = "ZRX"
                },
                new Currency
                {
                    Id = 2577,
                    CurrencyTypeId = 2,
                    Name = "Ravencoin",
                    Abbreviation = "RVN",
                    Slug = "RVN"
                },
                new Currency
                {
                    Id = 109,
                    CurrencyTypeId = 2,
                    Name = "DigiByte",
                    Abbreviation = "DGB",
                    Slug = "DGB"
                },
                new Currency
                {
                    Id = 2099,
                    CurrencyTypeId = 2,
                    Name = "ICON",
                    Abbreviation = "ICX",
                    Slug = "ICX"
                },
                new Currency
                {
                    Id = 693,
                    CurrencyTypeId = 2,
                    Name = "Verge",
                    Abbreviation = "XVG",
                    Slug = "XVG"
                },
                new Currency
                {
                    Id = 3330,
                    CurrencyTypeId = 2,
                    Name = "Paxos Standard Token",
                    Abbreviation = "PAX",
                    Slug = "PAX"
                },
                new Currency
                {
                    Id = 2603,
                    CurrencyTypeId = 2,
                    Name = "Pundi X",
                    Abbreviation = "NPXS",
                    Slug = "NPXS"
                },
                new Currency
                {
                    Id = 2469,
                    CurrencyTypeId = 2,
                    Name = "Zilliqa",
                    Abbreviation = "ZIL",
                    Slug = "ZIL"
                },
                new Currency
                {
                    Id = 2502,
                    CurrencyTypeId = 2,
                    Name = "Huobi Token",
                    Abbreviation = "HT",
                    Slug = "HT"
                },
                new Currency
                {
                    Id = 1700,
                    CurrencyTypeId = 2,
                    Name = "Aeternity",
                    Abbreviation = "AE",
                    Slug = "AE"
                },
                new Currency
                {
                    Id = 2405,
                    CurrencyTypeId = 2,
                    Name = "IOST",
                    Abbreviation = "IOST",
                    Slug = "IOST"
                },
                new Currency
                {
                    Id = 1042,
                    CurrencyTypeId = 2,
                    Name = "Siacoin",
                    Abbreviation = "SC",
                    Slug = "SC"
                },
                new Currency
                {
                    Id = 3437,
                    CurrencyTypeId = 2,
                    Name = "ABBC Coin",
                    Abbreviation = "ABBC",
                    Slug = "ABBC"
                },
                new Currency
                {
                    Id = 1521,
                    CurrencyTypeId = 2,
                    Name = "Komodo",
                    Abbreviation = "KMD",
                    Slug = "KMD"
                },
                new Currency
                {
                    Id = 2130,
                    CurrencyTypeId = 2,
                    Name = "Enjin Coin",
                    Abbreviation = "ENJ",
                    Slug = "ENJ"
                },
                new Currency
                {
                    Id = 1230,
                    CurrencyTypeId = 2,
                    Name = "Steem",
                    Abbreviation = "STEEM",
                    Slug = "STEEM"
                },
                new Currency
                {
                    Id = 1866,
                    CurrencyTypeId = 2,
                    Name = "Bytom",
                    Abbreviation = "BTM",
                    Slug = "BTM"
                },
                new Currency
                {
                    Id = 3224,
                    CurrencyTypeId = 2,
                    Name = "Qubitica",
                    Abbreviation = "QBIT",
                    Slug = "QBIT"
                },
                new Currency
                {
                    Id = 2416,
                    CurrencyTypeId = 2,
                    Name = "THETA",
                    Abbreviation = "THETA",
                    Slug = "THETA"
                },
                new Currency
                {
                    Id = 1343,
                    CurrencyTypeId = 2,
                    Name = "Stratis",
                    Abbreviation = "STRAT",
                    Slug = "STRAT"
                },
                new Currency
                {
                    Id = 3144,
                    CurrencyTypeId = 2,
                    Name = "ThoreCoin",
                    Abbreviation = "THR",
                    Slug = "THR"
                },
                new Currency
                {
                    Id = 291,
                    CurrencyTypeId = 2,
                    Name = "MaidSafeCoin",
                    Abbreviation = "MAID",
                    Slug = "MAID"
                },
                new Currency
                {
                    Id = 1886,
                    CurrencyTypeId = 2,
                    Name = "Dent",
                    Abbreviation = "DENT",
                    Slug = "DENT"
                },
                new Currency
                {
                    Id = 3116,
                    CurrencyTypeId = 2,
                    Name = "Insight Chain",
                    Abbreviation = "INB",
                    Slug = "INB"
                },
                new Currency
                {
                    Id = 2087,
                    CurrencyTypeId = 2,
                    Name = "KuCoin Shares",
                    Abbreviation = "KCS",
                    Slug = "KCS"
                },
                new Currency
                {
                    Id = 3724,
                    CurrencyTypeId = 2,
                    Name = "SOLVE",
                    Abbreviation = "SOLVE",
                    Slug = "SOLVE"
                },
                new Currency
                {
                    Id = 1925,
                    CurrencyTypeId = 2,
                    Name = "Waltonchain",
                    Abbreviation = "WTC",
                    Slug = "WTC"
                },
                new Currency
                {
                    Id = 1455,
                    CurrencyTypeId = 2,
                    Name = "Golem",
                    Abbreviation = "GNT",
                    Slug = "GNT"
                },
                new Currency
                {
                    Id = 2299,
                    CurrencyTypeId = 2,
                    Name = "aelf",
                    Abbreviation = "ELF",
                    Slug = "ELF"
                },
                new Currency
                {
                    Id = 1759,
                    CurrencyTypeId = 2,
                    Name = "Status",
                    Abbreviation = "SNT",
                    Slug = "SNT"
                },
                new Currency
                {
                    Id = 1776,
                    CurrencyTypeId = 2,
                    Name = "Crypto.com",
                    Abbreviation = "MCO",
                    Slug = "MCO"
                },
                new Currency
                {
                    Id = 2349,
                    CurrencyTypeId = 2,
                    Name = "Mixin",
                    Abbreviation = "XIN",
                    Slug = "XIN"
                },
                new Currency
                {
                    Id = 2027,
                    CurrencyTypeId = 2,
                    Name = "Cryptonex",
                    Abbreviation = "CNX",
                    Slug = "CNX"
                },
                new Currency
                {
                    Id = 3822,
                    CurrencyTypeId = 2,
                    Name = "Theta Fuel",
                    Abbreviation = "TFUEL",
                    Slug = "TFUEL"
                },
                new Currency
                {
                    Id = 1320,
                    CurrencyTypeId = 2,
                    Name = "Ardor",
                    Abbreviation = "ARDR",
                    Slug = "ARDR"
                },
                new Currency
                {
                    Id = 3607,
                    CurrencyTypeId = 2,
                    Name = "VestChain",
                    Abbreviation = "VEST",
                    Slug = "VEST"
                },
                new Currency
                {
                    Id = 2308,
                    CurrencyTypeId = 2,
                    Name = "Dai",
                    Abbreviation = "DAI",
                    Slug = "DAI"
                },
                new Currency
                {
                    Id = 1087,
                    CurrencyTypeId = 2,
                    Name = "Factom",
                    Abbreviation = "FCT",
                    Slug = "FCT"
                },
                new Currency
                {
                    Id = 2900,
                    CurrencyTypeId = 2,
                    Name = "Project Pai",
                    Abbreviation = "PAI",
                    Slug = "PAI"
                },
                new Currency
                {
                    Id = 2300,
                    CurrencyTypeId = 2,
                    Name = "WAX",
                    Abbreviation = "WAX",
                    Slug = "WAX"
                },
                new Currency
                {
                    Id = 2457,
                    CurrencyTypeId = 2,
                    Name = "TrueChain",
                    Abbreviation = "TRUE",
                    Slug = "TRUE"
                },
                new Currency
                {
                    Id = 1586,
                    CurrencyTypeId = 2,
                    Name = "Ark",
                    Abbreviation = "ARK",
                    Slug = "ARK"
                },
                new Currency
                {
                    Id = 1698,
                    CurrencyTypeId = 2,
                    Name = "Horizen",
                    Abbreviation = "ZEN",
                    Slug = "ZEN"
                },
                new Currency
                {
                    Id = 1229,
                    CurrencyTypeId = 2,
                    Name = "DigixDAO",
                    Abbreviation = "DGD",
                    Slug = "DGD"
                },
                new Currency
                {
                    Id = 460,
                    CurrencyTypeId = 2,
                    Name = "Clams",
                    Abbreviation = "CLAM",
                    Slug = "CLAM"
                },
                new Currency
                {
                    Id = 213,
                    CurrencyTypeId = 2,
                    Name = "MonaCoin",
                    Abbreviation = "MONA",
                    Slug = "MONA"
                },
                new Currency
                {
                    Id = 1750,
                    CurrencyTypeId = 2,
                    Name = "GXChain",
                    Abbreviation = "GXC",
                    Slug = "GXC"
                },
                new Currency
                {
                    Id = 1966,
                    CurrencyTypeId = 2,
                    Name = "Decentraland",
                    Abbreviation = "MANA",
                    Slug = "MANA"
                },
                new Currency
                {
                    Id = 3835,
                    CurrencyTypeId = 2,
                    Name = "Orbs",
                    Abbreviation = "ORBS",
                    Slug = "ORBS"
                },
                new Currency
                {
                    Id = 2062,
                    CurrencyTypeId = 2,
                    Name = "Aion",
                    Abbreviation = "AION",
                    Slug = "AION"
                },
                new Currency
                {
                    Id = 1703,
                    CurrencyTypeId = 2,
                    Name = "Metaverse ETP",
                    Abbreviation = "ETP",
                    Slug = "ETP"
                },
                new Currency
                {
                    Id = 2492,
                    CurrencyTypeId = 2,
                    Name = "Elastos",
                    Abbreviation = "ELA",
                    Slug = "ELA"
                },
                new Currency
                {
                    Id = 1934,
                    CurrencyTypeId = 2,
                    Name = "Loopring",
                    Abbreviation = "LRC",
                    Slug = "LRC"
                },
                new Currency
                {
                    Id = 2588,
                    CurrencyTypeId = 2,
                    Name = "Loom Network",
                    Abbreviation = "LOOM",
                    Slug = "LOOM"
                },
                new Currency
                {
                    Id = 1807,
                    CurrencyTypeId = 2,
                    Name = "Santiment Network Token",
                    Abbreviation = "SAN",
                    Slug = "SAN"
                },
                new Currency
                {
                    Id = 2135,
                    CurrencyTypeId = 2,
                    Name = "Revain",
                    Abbreviation = "R",
                    Slug = "R"
                },
                new Currency
                {
                    Id = 2092,
                    CurrencyTypeId = 2,
                    Name = "NULS",
                    Abbreviation = "NULS",
                    Slug = "NULS"
                },
                new Currency
                {
                    Id = 1789,
                    CurrencyTypeId = 2,
                    Name = "Populous",
                    Abbreviation = "PPT",
                    Slug = "PPT"
                },
                new Currency
                {
                    Id = 3890,
                    CurrencyTypeId = 2,
                    Name = "Matic Network",
                    Abbreviation = "MATIC",
                    Slug = "MATIC"
                },
                new Currency
                {
                    Id = 2090,
                    CurrencyTypeId = 2,
                    Name = "LATOKEN",
                    Abbreviation = "LA",
                    Slug = "LA"
                },
                new Currency
                {
                    Id = 1414,
                    CurrencyTypeId = 2,
                    Name = "Zcoin",
                    Abbreviation = "XZC",
                    Slug = "XZC"
                });
        }
    }
}