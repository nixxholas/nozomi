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
            entityTypeBuilder.HasMany(c => c.CurrencyRequests).WithOne(cr => cr.Currency)
                .HasForeignKey(cr => cr.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Currencies_CurrencyRequests_Constraint");

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
                    Id = 1518,
                    CurrencyTypeId = 2,
                    Name = "Maker",
                    Abbreviation = "MKR",
                    Slug = "MKR"
                },
                new Currency
                {
                    Id = 2566,
                    CurrencyTypeId = 2,
                    Name = "Ontology",
                    Abbreviation = "ONT",
                    Slug = "ONT"
                },
                new Currency
                {
                    Id = 1437,
                    CurrencyTypeId = 2,
                    Name = "Zcash",
                    Abbreviation = "ZEC",
                    Slug = "ZEC"
                },
                new Currency
                {
                    Id = 1697,
                    CurrencyTypeId = 2,
                    Name = "Basic Attention Token",
                    Abbreviation = "BAT",
                    Slug = "BAT"
                },
                new Currency
                {
                    Id = 3635,
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
                },
                new Currency
                {
                    Id = 1903,
                    CurrencyTypeId = 2,
                    Name = "HyperCash",
                    Abbreviation = "HC",
                    Slug = "HC"
                },
                new Currency
                {
                    Id = 2132,
                    CurrencyTypeId = 2,
                    Name = "Power Ledger",
                    Abbreviation = "POWR",
                    Slug = "POWR"
                },
                new Currency
                {
                    Id = 118,
                    CurrencyTypeId = 2,
                    Name = "ReddCoin",
                    Abbreviation = "RDD",
                    Slug = "RDD"
                },
                new Currency
                {
                    Id = 2403,
                    CurrencyTypeId = 2,
                    Name = "MOAC",
                    Abbreviation = "MOAC",
                    Slug = "MOAC"
                },
                new Currency
                {
                    Id = 2897,
                    CurrencyTypeId = 2,
                    Name = "Clipper Coin",
                    Abbreviation = "CCCX",
                    Slug = "CCCX"
                },
                new Currency
                {
                    Id = 2545,
                    CurrencyTypeId = 2,
                    Name = "Arcblock",
                    Abbreviation = "ABT",
                    Slug = "ABT"
                },
                new Currency
                {
                    Id = 3871,
                    CurrencyTypeId = 2,
                    Name = "Newton",
                    Abbreviation = "NEW",
                    Slug = "NEW"
                },
                new Currency
                {
                    Id = 2213,
                    CurrencyTypeId = 2,
                    Name = "QASH",
                    Abbreviation = "QASH",
                    Slug = "QASH"
                },
                new Currency
                {
                    Id = 2631,
                    CurrencyTypeId = 2,
                    Name = "ODEM",
                    Abbreviation = "ODE",
                    Slug = "ODE"
                },
                new Currency
                {
                    Id = 2346,
                    CurrencyTypeId = 2,
                    Name = "WaykiChain",
                    Abbreviation = "WICC",
                    Slug = "WICC"
                },
                new Currency
                {
                    Id = 2606,
                    CurrencyTypeId = 2,
                    Name = "Wanchain",
                    Abbreviation = "WAN",
                    Slug = "WAN"
                },
                new Currency
                {
                    Id = 1727,
                    CurrencyTypeId = 2,
                    Name = "Bancor",
                    Abbreviation = "BNT",
                    Slug = "BNT"
                },
                new Currency
                {
                    Id = 3115,
                    CurrencyTypeId = 2,
                    Name = "Maximine Coin",
                    Abbreviation = "MXM",
                    Slug = "MXM"
                },
                new Currency
                {
                    Id = 2694,
                    CurrencyTypeId = 2,
                    Name = "Nexo",
                    Abbreviation = "NEXO",
                    Slug = "NEXO"
                },
                new Currency
                {
                    Id = 2496,
                    CurrencyTypeId = 2,
                    Name = "Polymath",
                    Abbreviation = "POLY",
                    Slug = "POLY"
                },
                new Currency
                {
                    Id = 1772,
                    CurrencyTypeId = 2,
                    Name = "Storj",
                    Abbreviation = "STORJ",
                    Slug = "STORJ"
                },
                new Currency
                {
                    Id = 2306,
                    CurrencyTypeId = 2,
                    Name = "Bread",
                    Abbreviation = "BRD",
                    Slug = "BRD"
                },
                new Currency
                {
                    Id = 2137,
                    CurrencyTypeId = 2,
                    Name = "Electroneum",
                    Abbreviation = "ETN",
                    Slug = "ETN"
                },
                new Currency
                {
                    Id = 1169,
                    CurrencyTypeId = 2,
                    Name = "PIVX",
                    Abbreviation = "PIVX",
                    Slug = "PIVX"
                },
                new Currency
                {
                    Id = 45,
                    CurrencyTypeId = 2,
                    Name = "CasinoCoin",
                    Abbreviation = "CSC",
                    Slug = "CSC"
                },
                new Currency
                {
                    Id = 2777,
                    CurrencyTypeId = 2,
                    Name = "IoTeX",
                    Abbreviation = "IOTX",
                    Slug = "IOTX"
                },
                new Currency
                {
                    Id = 1908,
                    CurrencyTypeId = 2,
                    Name = "Nebulas",
                    Abbreviation = "NAS",
                    Slug = "NAS"
                },
                new Currency
                {
                    Id = 1637,
                    CurrencyTypeId = 2,
                    Name = "iExec RLC",
                    Abbreviation = "RLC",
                    Slug = "RLC"
                },
                new Currency
                {
                    Id = 1710,
                    CurrencyTypeId = 2,
                    Name = "Veritaseum",
                    Abbreviation = "VERI",
                    Slug = "VERI"
                },
                new Currency
                {
                    Id = 1757,
                    CurrencyTypeId = 2,
                    Name = "FunFair",
                    Abbreviation = "FUN",
                    Slug = "FUN"
                },
                new Currency
                {
                    Id = 2829,
                    CurrencyTypeId = 2,
                    Name = "REPO",
                    Abbreviation = "REPO",
                    Slug = "REPO"
                },
                new Currency
                {
                    Id = 3814,
                    CurrencyTypeId = 2,
                    Name = "Celer Network",
                    Abbreviation = "CELR",
                    Slug = "CELR"
                },
                new Currency
                {
                    Id = 2307,
                    CurrencyTypeId = 2,
                    Name = "Bibox Token",
                    Abbreviation = "BIX",
                    Slug = "BIX"
                },
                new Currency
                {
                    Id = 541,
                    CurrencyTypeId = 2,
                    Name = "Syscoin",
                    Abbreviation = "SYS",
                    Slug = "SYS"
                },
                new Currency
                {
                    Id = 3415,
                    CurrencyTypeId = 2,
                    Name = "Buggyra Coin Zero",
                    Abbreviation = "BCZERO",
                    Slug = "BCZERO"
                },
                new Currency
                {
                    Id = 2044,
                    CurrencyTypeId = 2,
                    Name = "Enigma",
                    Abbreviation = "ENG",
                    Slug = "ENG"
                },
                new Currency
                {
                    Id = 2989,
                    CurrencyTypeId = 2,
                    Name = "STASIS EURS",
                    Abbreviation = "EURS",
                    Slug = "EURS"
                },
                new Currency
                {
                    Id = 3828,
                    CurrencyTypeId = 2,
                    Name = "Japan Content Token",
                    Abbreviation = "JCT",
                    Slug = "JCT"
                },
                new Currency
                {
                    Id = 3218,
                    CurrencyTypeId = 2,
                    Name = "Energi",
                    Abbreviation = "NRG",
                    Slug = "NRG"
                },
                new Currency
                {
                    Id = 258,
                    CurrencyTypeId = 2,
                    Name = "Groestlcoin",
                    Abbreviation = "GRS",
                    Slug = "GRS"
                },
                new Currency
                {
                    Id = 1826,
                    CurrencyTypeId = 2,
                    Name = "Particl",
                    Abbreviation = "PART",
                    Slug = "PART"
                },
                new Currency
                {
                    Id = 3788,
                    CurrencyTypeId = 2,
                    Name = "NEXT",
                    Abbreviation = "NET",
                    Slug = "NET"
                },
                new Currency
                {
                    Id = 2570,
                    CurrencyTypeId = 2,
                    Name = "TomoChain",
                    Abbreviation = "TOMO",
                    Slug = "TOMO"
                },
                new Currency
                {
                    Id = 66,
                    CurrencyTypeId = 2,
                    Name = "Nxt",
                    Abbreviation = "NXT",
                    Slug = "NXT"
                },
                new Currency
                {
                    Id = 2952,
                    CurrencyTypeId = 2,
                    Name = "Gold Bits Coin",
                    Abbreviation = "GBC",
                    Slug = "GBC"
                },
                new Currency
                {
                    Id = 3657,
                    CurrencyTypeId = 2,
                    Name = "Lambda",
                    Abbreviation = "LAMB",
                    Slug = "LAMB"
                },
                new Currency
                {
                    Id = 2453,
                    CurrencyTypeId = 2,
                    Name = "EDUCare",
                    Abbreviation = "EKT",
                    Slug = "EKT"
                },
                new Currency
                {
                    Id = 2320,
                    CurrencyTypeId = 2,
                    Name = "UTRUST",
                    Abbreviation = "UTK",
                    Slug = "UTK"
                },
                new Currency
                {
                    Id = 2840,
                    CurrencyTypeId = 2,
                    Name = "QuarkChain",
                    Abbreviation = "QKC",
                    Slug = "QKC"
                },
                new Currency
                {
                    Id = 2112,
                    CurrencyTypeId = 2,
                    Name = "Red Pulse Phoenix",
                    Abbreviation = "PHX",
                    Slug = "PHX"
                },
                new Currency
                {
                    Id = 3701,
                    CurrencyTypeId = 2,
                    Name = "RIF Token",
                    Abbreviation = "RIF",
                    Slug = "RIF"
                },
                new Currency
                {
                    Id = 1785,
                    CurrencyTypeId = 2,
                    Name = "Gas",
                    Abbreviation = "GAS",
                    Slug = "GAS"
                },
                new Currency
                {
                    Id = 1659,
                    CurrencyTypeId = 2,
                    Name = "Gnosis",
                    Abbreviation = "GNO",
                    Slug = "GNO"
                },
                new Currency
                {
                    Id = 1816,
                    CurrencyTypeId = 2,
                    Name = "Civic",
                    Abbreviation = "CVC",
                    Slug = "CVC"
                },
                new Currency
                {
                    Id = 2381,
                    CurrencyTypeId = 2,
                    Name = "Spectre.ai Dividend Token",
                    Abbreviation = "SXDT",
                    Slug = "SXDT"
                },
                new Currency
                {
                    Id = 1993,
                    CurrencyTypeId = 2,
                    Name = "Kin",
                    Abbreviation = "KIN",
                    Slug = "KIN"
                },
                new Currency
                {
                    Id = 3662,
                    CurrencyTypeId = 2,
                    Name = "HedgeTrade",
                    Abbreviation = "HEDG",
                    Slug = "HEDG"
                },
                new Currency
                {
                    Id = 3513,
                    CurrencyTypeId = 2,
                    Name = "Fantom",
                    Abbreviation = "FTM",
                    Slug = "FTM"
                },
                new Currency
                {
                    Id = 2638,
                    CurrencyTypeId = 2,
                    Name = "Cortex",
                    Abbreviation = "CTXC",
                    Slug = "CTXC"
                },
                new Currency
                {
                    Id = 3126,
                    CurrencyTypeId = 2,
                    Name = "ProximaX",
                    Abbreviation = "XPX",
                    Slug = "XPX"
                },
                new Currency
                {
                    Id = 3344,
                    CurrencyTypeId = 2,
                    Name = "Ecoreal Estate",
                    Abbreviation = "ECOREAL",
                    Slug = "ECOREAL"
                },
                new Currency
                {
                    Id = 2608,
                    CurrencyTypeId = 2,
                    Name = "Mithril",
                    Abbreviation = "MITH",
                    Slug = "MITH"
                },
                new Currency
                {
                    Id = 2444,
                    CurrencyTypeId = 2,
                    Name = "CRYPTO20",
                    Abbreviation = "C20",
                    Slug = "C20"
                },
                new Currency
                {
                    Id = 1758,
                    CurrencyTypeId = 2,
                    Name = "TenX",
                    Abbreviation = "PAY",
                    Slug = "PAY"
                },
                new Currency
                {
                    Id = 3178,
                    CurrencyTypeId = 2,
                    Name = "Linkey",
                    Abbreviation = "LKY",
                    Slug = "LKY"
                },
                new Currency
                {
                    Id = 2896,
                    CurrencyTypeId = 2,
                    Name = "Mainframe",
                    Abbreviation = "MFT",
                    Slug = "MFT"
                },
                new Currency
                {
                    Id = 2845,
                    CurrencyTypeId = 2,
                    Name = "MediBloc [ERC20]",
                    Abbreviation = "MEDX",
                    Slug = "MEDX"
                },
                new Currency
                {
                    Id = 2043,
                    CurrencyTypeId = 2,
                    Name = "Cindicator",
                    Abbreviation = "CND",
                    Slug = "CND"
                },
                new Currency
                {
                    Id = 1492,
                    CurrencyTypeId = 2,
                    Name = "Obyte",
                    Abbreviation = "GBYTE",
                    Slug = "GBYTE"
                },
                new Currency
                {
                    Id = 3863,
                    CurrencyTypeId = 2,
                    Name = "UGAS",
                    Abbreviation = "UGAS",
                    Slug = "UGAS"
                },
                new Currency
                {
                    Id = 3756,
                    CurrencyTypeId = 2,
                    Name = "#MetaHash",
                    Abbreviation = "MHC",
                    Slug = "MHC"
                },
                new Currency
                {
                    Id = 2530,
                    CurrencyTypeId = 2,
                    Name = "Fusion",
                    Abbreviation = "FSN",
                    Slug = "FSN"
                },
                new Currency
                {
                    Id = 2424,
                    CurrencyTypeId = 2,
                    Name = "SingularityNET",
                    Abbreviation = "AGI",
                    Slug = "AGI"
                },
                new Currency
                {
                    Id = 3709,
                    CurrencyTypeId = 2,
                    Name = "Grin",
                    Abbreviation = "GRIN",
                    Slug = "GRIN"
                },
                new Currency
                {
                    Id = 2599,
                    CurrencyTypeId = 2,
                    Name = "Noah Coin",
                    Abbreviation = "NOAH",
                    Slug = "NOAH"
                },
                new Currency
                {
                    Id = 2057,
                    CurrencyTypeId = 2,
                    Name = "Eidoo",
                    Abbreviation = "EDO",
                    Slug = "EDO"
                },
                new Currency
                {
                    Id = 2885,
                    CurrencyTypeId = 2,
                    Name = "Egretia",
                    Abbreviation = "EGT",
                    Slug = "EGT"
                },
                new Currency
                {
                    Id = 2772,
                    CurrencyTypeId = 2,
                    Name = "Digitex Futures",
                    Abbreviation = "DGTX",
                    Slug = "DGTX"
                },
                new Currency
                {
                    Id = 3617,
                    CurrencyTypeId = 2,
                    Name = "ILCoin",
                    Abbreviation = "ILC",
                    Slug = "ILC"
                },
                new Currency
                {
                    Id = 3695,
                    CurrencyTypeId = 2,
                    Name = "Hyperion",
                    Abbreviation = "HYN",
                    Slug = "HYN"
                },
                new Currency
                {
                    Id = 3418,
                    CurrencyTypeId = 2,
                    Name = "Metadium",
                    Abbreviation = "META",
                    Slug = "META"
                },
                new Currency
                {
                    Id = 2298,
                    CurrencyTypeId = 2,
                    Name = "Dynamic Trading Rights",
                    Abbreviation = "DTR",
                    Slug = "DTR"
                },
                new Currency
                {
                    Id = 99,
                    CurrencyTypeId = 2,
                    Name = "Vertcoin",
                    Abbreviation = "VTC",
                    Slug = "VTC"
                },
                new Currency
                {
                    Id = 1619,
                    CurrencyTypeId = 2,
                    Name = "Skycoin",
                    Abbreviation = "SKY",
                    Slug = "SKY"
                },
                new Currency
                {
                    Id = 3325,
                    CurrencyTypeId = 2,
                    Name = "Robotina",
                    Abbreviation = "ROX",
                    Slug = "ROX"
                },
                new Currency
                {
                    Id = 3847,
                    CurrencyTypeId = 2,
                    Name = "Contents Protocol",
                    Abbreviation = "CPT",
                    Slug = "CPT"
                },
                new Currency
                {
                    Id = 201,
                    CurrencyTypeId = 2,
                    Name = "Einsteinium",
                    Abbreviation = "EMC2",
                    Slug = "EMC2"
                },
                new Currency
                {
                    Id = 2539,
                    CurrencyTypeId = 2,
                    Name = "Ren",
                    Abbreviation = "REN",
                    Slug = "REN"
                },
                new Currency
                {
                    Id = 2992,
                    CurrencyTypeId = 2,
                    Name = "Apollo Currency",
                    Abbreviation = "APL",
                    Slug = "APL"
                },
                new Currency
                {
                    Id = 2955,
                    CurrencyTypeId = 2,
                    Name = "Cosmo Coin",
                    Abbreviation = "COSM",
                    Slug = "COSM"
                },
                new Currency
                {
                    Id = 3155,
                    CurrencyTypeId = 2,
                    Name = "Quant",
                    Abbreviation = "QNT",
                    Slug = "QNT"
                },
                new Currency
                {
                    Id = 2605,
                    CurrencyTypeId = 2,
                    Name = "BnkToTheFuture",
                    Abbreviation = "BFT",
                    Slug = "BFT"
                },
                new Currency
                {
                    Id = 2251,
                    CurrencyTypeId = 2,
                    Name = "IoT Chain",
                    Abbreviation = "ITC",
                    Slug = "ITC"
                },
                new Currency
                {
                    Id = 3085,
                    CurrencyTypeId = 2,
                    Name = "INO COIN",
                    Abbreviation = "INO",
                    Slug = "INO"
                },
                new Currency
                {
                    Id = 3306,
                    CurrencyTypeId = 2,
                    Name = "Gemini Dollar",
                    Abbreviation = "GUSD",
                    Slug = "GUSD"
                },
                new Currency
                {
                    Id = 2780,
                    CurrencyTypeId = 2,
                    Name = "NKN",
                    Abbreviation = "NKN",
                    Slug = "NKN"
                },
                new Currency
                {
                    Id = 2394,
                    CurrencyTypeId = 2,
                    Name = "Telcoin",
                    Abbreviation = "TEL",
                    Slug = "TEL"
                },
                new Currency
                {
                    Id = 789,
                    CurrencyTypeId = 2,
                    Name = "Nexus",
                    Abbreviation = "NXS",
                    Slug = "NXS"
                },
                new Currency
                {
                    Id = 1788,
                    CurrencyTypeId = 2,
                    Name = "Metal",
                    Abbreviation = "MTL",
                    Slug = "MTL"
                },
                new Currency
                {
                    Id = 2289,
                    CurrencyTypeId = 2,
                    Name = "Gifto",
                    Abbreviation = "GTO",
                    Slug = "GTO"
                },
                new Currency
                {
                    Id = 1680,
                    CurrencyTypeId = 2,
                    Name = "Aragon",
                    Abbreviation = "ANT",
                    Slug = "ANT"
                },
                new Currency
                {
                    Id = 2335,
                    CurrencyTypeId = 2,
                    Name = "Lightning Bitcoin",
                    Abbreviation = "LBTC",
                    Slug = "LBTC"
                },
                new Currency
                {
                    Id = 3637,
                    CurrencyTypeId = 2,
                    Name = "Aergo",
                    Abbreviation = "AERGO",
                    Slug = "AERGO"
                },
                new Currency
                {
                    Id = 2760,
                    CurrencyTypeId = 2,
                    Name = "Cred",
                    Abbreviation = "LBA",
                    Slug = "LBA"
                },
                new Currency
                {
                    Id = 1712,
                    CurrencyTypeId = 2,
                    Name = "Quantum Resistant Ledger",
                    Abbreviation = "QRL",
                    Slug = "QRL"
                },
                new Currency
                {
                    Id = 2627,
                    CurrencyTypeId = 2,
                    Name = "TokenPay",
                    Abbreviation = "TPAY",
                    Slug = "TPAY"
                },
                new Currency
                {
                    Id = 2297,
                    CurrencyTypeId = 2,
                    Name = "Storm",
                    Abbreviation = "STORM",
                    Slug = "STORM"
                },
                new Currency
                {
                    Id = 1937,
                    CurrencyTypeId = 2,
                    Name = "Po.et",
                    Abbreviation = "POE",
                    Slug = "POE"
                },
                new Currency
                {
                    Id = 2267,
                    CurrencyTypeId = 2,
                    Name = "Tael",
                    Abbreviation = "WABI",
                    Slug = "WABI"
                },
                new Currency
                {
                    Id = 3609,
                    CurrencyTypeId = 2,
                    Name = "CWV Chain",
                    Abbreviation = "CWV",
                    Slug = "CWV"
                },
                new Currency
                {
                    Id = 215,
                    CurrencyTypeId = 2,
                    Name = "Rubycoin",
                    Abbreviation = "RBY",
                    Slug = "RBY"
                },
                new Currency
                {
                    Id = 3783,
                    CurrencyTypeId = 2,
                    Name = "Ankr Network",
                    Abbreviation = "ANKR",
                    Slug = "ANKR"
                },
                new Currency
                {
                    Id = 1954,
                    CurrencyTypeId = 2,
                    Name = "Moeda Loyalty Points",
                    Abbreviation = "MDA",
                    Slug = "MDA"
                },
                new Currency
                {
                    Id = 3154,
                    CurrencyTypeId = 2,
                    Name = "Davinci Coin",
                    Abbreviation = "DAC",
                    Slug = "DAC"
                },
                new Currency
                {
                    Id = 2243,
                    CurrencyTypeId = 2,
                    Name = "Dragonchain",
                    Abbreviation = "DRGN",
                    Slug = "DRGN"
                },
                new Currency
                {
                    Id = 2071,
                    CurrencyTypeId = 2,
                    Name = "Request",
                    Abbreviation = "REQ",
                    Slug = "REQ"
                },
                new Currency
                {
                    Id = 2939,
                    CurrencyTypeId = 2,
                    Name = "SCRL",
                    Abbreviation = "SCRL",
                    Slug = "SCRL"
                },
                new Currency
                {
                    Id = 1955,
                    CurrencyTypeId = 2,
                    Name = "Neblio",
                    Abbreviation = "NEBL",
                    Slug = "NEBL"
                },
                new Currency
                {
                    Id = 3826,
                    CurrencyTypeId = 2,
                    Name = "TOP",
                    Abbreviation = "TOP",
                    Slug = "TOP"
                },
                new Currency
                {
                    Id = 707,
                    CurrencyTypeId = 2,
                    Name = "Blocknet",
                    Abbreviation = "BLOCK",
                    Slug = "BLOCK"
                },
                new Currency
                {
                    Id = 2458,
                    CurrencyTypeId = 2,
                    Name = "Odyssey",
                    Abbreviation = "OCN",
                    Slug = "OCN"
                },
                new Currency
                {
                    Id = 2433,
                    CurrencyTypeId = 2,
                    Name = "IPChain",
                    Abbreviation = "IPC",
                    Slug = "IPC"
                },
                new Currency
                {
                    Id = 2930,
                    CurrencyTypeId = 2,
                    Name = "Everipedia",
                    Abbreviation = "IQ",
                    Slug = "IQ"
                },
                new Currency
                {
                    Id = 2471,
                    CurrencyTypeId = 2,
                    Name = "Smartlands",
                    Abbreviation = "SLT",
                    Slug = "SLT"
                },
                new Currency
                {
                    Id = 2276,
                    CurrencyTypeId = 2,
                    Name = "Ignis",
                    Abbreviation = "IGNIS",
                    Slug = "IGNIS"
                },
                new Currency
                {
                    Id = 2934,
                    CurrencyTypeId = 2,
                    Name = "BitKan",
                    Abbreviation = "KAN",
                    Slug = "KAN"
                },
                new Currency
                {
                    Id = 1779,
                    CurrencyTypeId = 2,
                    Name = "Wagerr",
                    Abbreviation = "WGR",
                    Slug = "WGR"
                },
                new Currency
                {
                    Id = 3175,
                    CurrencyTypeId = 2,
                    Name = "TTC Protocol",
                    Abbreviation = "TTC",
                    Slug = "TTC"
                },
                new Currency
                {
                    Id = 2034,
                    CurrencyTypeId = 2,
                    Name = "Everex",
                    Abbreviation = "EVX",
                    Slug = "EVX"
                },
                new Currency
                {
                    Id = 3345,
                    CurrencyTypeId = 2,
                    Name = "DAPS Token",
                    Abbreviation = "DAPS",
                    Slug = "DAPS"
                },
                new Currency
                {
                    Id = 2591,
                    CurrencyTypeId = 2,
                    Name = "Dropil",
                    Abbreviation = "DROP",
                    Slug = "DROP"
                },
                new Currency
                {
                    Id = 1918,
                    CurrencyTypeId = 2,
                    Name = "Achain",
                    Abbreviation = "ACT",
                    Slug = "ACT"
                },
                new Currency
                {
                    Id = 2843,
                    CurrencyTypeId = 2,
                    Name = "Ether Zero",
                    Abbreviation = "ETZ",
                    Slug = "ETZ"
                },
                new Currency
                {
                    Id = 3600,
                    CurrencyTypeId = 2,
                    Name = "Humanscape",
                    Abbreviation = "HUM",
                    Slug = "HUM"
                },
                new Currency
                {
                    Id = 2861,
                    CurrencyTypeId = 2,
                    Name = "GoChain",
                    Abbreviation = "GO",
                    Slug = "GO"
                },
                new Currency
                {
                    Id = 2345,
                    CurrencyTypeId = 2,
                    Name = "High Performance Blockchain",
                    Abbreviation = "HPB",
                    Slug = "HPB"
                },
                new Currency
                {
                    Id = 3737,
                    CurrencyTypeId = 2,
                    Name = "BTU Protocol",
                    Abbreviation = "BTU",
                    Slug = "BTU"
                },
                new Currency
                {
                    Id = 3083,
                    CurrencyTypeId = 2,
                    Name = "LINA",
                    Abbreviation = "LINA",
                    Slug = "LINA"
                },
                new Currency
                {
                    Id = 3366,
                    CurrencyTypeId = 2,
                    Name = "SafeInsure",
                    Abbreviation = "SINS",
                    Slug = "SINS"
                },
                new Currency
                {
                    Id = 2296,
                    CurrencyTypeId = 2,
                    Name = "OST",
                    Abbreviation = "OST",
                    Slug = "OST"
                },
                new Currency
                {
                    Id = 2474,
                    CurrencyTypeId = 2,
                    Name = "Matrix AI Network",
                    Abbreviation = "MAN",
                    Slug = "MAN"
                },
                new Currency
                {
                    Id = 2835,
                    CurrencyTypeId = 2,
                    Name = "Endor Protocol",
                    Abbreviation = "EDR",
                    Slug = "EDR"
                },
                new Currency
                {
                    Id = 1876,
                    CurrencyTypeId = 2,
                    Name = "Dentacoin",
                    Abbreviation = "DCN",
                    Slug = "DCN"
                },
                new Currency
                {
                    Id = 1654,
                    CurrencyTypeId = 2,
                    Name = "Bitcore",
                    Abbreviation = "BTX",
                    Slug = "BTX"
                },
                new Currency
                {
                    Id = 2161,
                    CurrencyTypeId = 2,
                    Name = "Raiden Network Token",
                    Abbreviation = "RDN",
                    Slug = "RDN"
                },
                new Currency
                {
                    Id = 3020,
                    CurrencyTypeId = 2,
                    Name = "BHPCoin",
                    Abbreviation = "BHP",
                    Slug = "BHP"
                },
                new Currency
                {
                    Id = 2143,
                    CurrencyTypeId = 2,
                    Name = "Streamr DATAcoin",
                    Abbreviation = "DATA",
                    Slug = "DATA"
                },
                new Currency
                {
                    Id = 2476,
                    CurrencyTypeId = 2,
                    Name = "Ruff",
                    Abbreviation = "RUFF",
                    Slug = "RUFF"
                },
                new Currency
                {
                    Id = 2096,
                    CurrencyTypeId = 2,
                    Name = "Ripio Credit Network",
                    Abbreviation = "RCN",
                    Slug = "RCN"
                },
                new Currency
                {
                    Id = 2212,
                    CurrencyTypeId = 2,
                    Name = "Quantstamp",
                    Abbreviation = "QSP",
                    Slug = "QSP"
                },
                new Currency
                {
                    Id = 3773,
                    CurrencyTypeId = 2,
                    Name = "Fetch",
                    Abbreviation = "FET",
                    Slug = "FET"
                },
                new Currency
                {
                    Id = 1853,
                    CurrencyTypeId = 2,
                    Name = "OAX",
                    Abbreviation = "OAX",
                    Slug = "OAX"
                },
                new Currency
                {
                    Id = 64,
                    CurrencyTypeId = 2,
                    Name = "FLO",
                    Abbreviation = "FLO",
                    Slug = "FLO"
                },
                new Currency
                {
                    Id = 470,
                    CurrencyTypeId = 2,
                    Name = "Viacoin",
                    Abbreviation = "VIA",
                    Slug = "VIA"
                },
                new Currency
                {
                    Id = 2586,
                    CurrencyTypeId = 2,
                    Name = "Synthetix Network Token",
                    Abbreviation = "SNX",
                    Slug = "SNX"
                },
                new Currency
                {
                    Id = 2313,
                    CurrencyTypeId = 2,
                    Name = "SIRIN LABS Token",
                    Abbreviation = "SRN",
                    Slug = "SRN"
                },
                new Currency
                {
                    Id = 2400,
                    CurrencyTypeId = 2,
                    Name = "OneRoot Network",
                    Abbreviation = "RNT",
                    Slug = "RNT"
                },
                new Currency
                {
                    Id = 2379,
                    CurrencyTypeId = 2,
                    Name = "Kcash",
                    Abbreviation = "KCASH",
                    Slug = "KCASH"
                },
                new Currency
                {
                    Id = 377,
                    CurrencyTypeId = 2,
                    Name = "NavCoin",
                    Abbreviation = "NAV",
                    Slug = "NAV"
                },
                new Currency
                {
                    Id = 2066,
                    CurrencyTypeId = 2,
                    Name = "Everus",
                    Abbreviation = "EVR",
                    Slug = "EVR"
                },
                new Currency
                {
                    Id = 2446,
                    CurrencyTypeId = 2,
                    Name = "DATA",
                    Abbreviation = "DTA",
                    Slug = "DTA"
                },
                new Currency
                {
                    Id = 2274,
                    CurrencyTypeId = 2,
                    Name = "MediShares",
                    Abbreviation = "MDS",
                    Slug = "MDS"
                },
                new Currency
                {
                    Id = 1817,
                    CurrencyTypeId = 2,
                    Name = "Ethos",
                    Abbreviation = "ETHOS",
                    Slug = "ETHOS"
                },
                new Currency
                {
                    Id = 2235,
                    CurrencyTypeId = 2,
                    Name = "Time New Bank",
                    Abbreviation = "TNB",
                    Slug = "TNB"
                },
                new Currency
                {
                    Id = 2765,
                    CurrencyTypeId = 2,
                    Name = "XYO",
                    Abbreviation = "XYO",
                    Slug = "XYO"
                },
                new Currency
                {
                    Id = 2505,
                    CurrencyTypeId = 2,
                    Name = "Bluzelle",
                    Abbreviation = "BLZ",
                    Slug = "BLZ"
                },
                new Currency
                {
                    Id = 1660,
                    CurrencyTypeId = 2,
                    Name = "TokenCard",
                    Abbreviation = "TKN",
                    Slug = "TKN"
                },
                new Currency
                {
                    Id = 1923,
                    CurrencyTypeId = 2,
                    Name = "Tierion",
                    Abbreviation = "TNT",
                    Slug = "TNT"
                },
                new Currency
                {
                    Id = 2918,
                    CurrencyTypeId = 2,
                    Name = "Bit-Z Token",
                    Abbreviation = "BZ",
                    Slug = "BZ"
                },
                new Currency
                {
                    Id = 2447,
                    CurrencyTypeId = 2,
                    Name = "Crypterium",
                    Abbreviation = "CRPT",
                    Slug = "CRPT"
                },
                new Currency
                {
                    Id = 2538,
                    CurrencyTypeId = 2,
                    Name = "Nectar",
                    Abbreviation = "NEC",
                    Slug = "NEC"
                },
                new Currency
                {
                    Id = 3164,
                    CurrencyTypeId = 2,
                    Name = "PumaPay",
                    Abbreviation = "PMA",
                    Slug = "PMA"
                },
                new Currency
                {
                    Id = 1828,
                    CurrencyTypeId = 2,
                    Name = "SmartCash",
                    Abbreviation = "SMART",
                    Slug = "SMART"
                },
                new Currency
                {
                    Id = 1732,
                    CurrencyTypeId = 2,
                    Name = "Numeraire",
                    Abbreviation = "NMR",
                    Slug = "NMR"
                },
                new Currency
                {
                    Id = 3631,
                    CurrencyTypeId = 2,
                    Name = "FOAM",
                    Abbreviation = "FOAM",
                    Slug = "FOAM"
                },
                new Currency
                {
                    Id = 2507,
                    CurrencyTypeId = 2,
                    Name = "THEKEY",
                    Abbreviation = "TKY",
                    Slug = "TKY"
                },
                new Currency
                {
                    Id = 2559,
                    CurrencyTypeId = 2,
                    Name = "Cube",
                    Abbreviation = "AUTO",
                    Slug = "AUTO"
                },
                new Currency
                {
                    Id = 2661,
                    CurrencyTypeId = 2,
                    Name = "Tripio",
                    Abbreviation = "TRIO",
                    Slug = "TRIO"
                },
                new Currency
                {
                    Id = 1768,
                    CurrencyTypeId = 2,
                    Name = "AdEx",
                    Abbreviation = "ADX",
                    Slug = "ADX"
                },
                new Currency
                {
                    Id = 1711,
                    CurrencyTypeId = 2,
                    Name = "Electra",
                    Abbreviation = "ECA",
                    Slug = "ECA"
                },
                new Currency
                {
                    Id = 2503,
                    CurrencyTypeId = 2,
                    Name = "DMarket",
                    Abbreviation = "DMT",
                    Slug = "DMT"
                },
                new Currency
                {
                    Id = 2873,
                    CurrencyTypeId = 2,
                    Name = "Metronome",
                    Abbreviation = "MET",
                    Slug = "MET"
                },
                new Currency
                {
                    Id = 1974,
                    CurrencyTypeId = 2,
                    Name = "Propy",
                    Abbreviation = "PRO",
                    Slug = "PRO"
                },
                new Currency
                {
                    Id = 2544,
                    CurrencyTypeId = 2,
                    Name = "Nucleus Vision",
                    Abbreviation = "NCASH",
                    Slug = "NCASH"
                },
                new Currency
                {
                    Id = 1447,
                    CurrencyTypeId = 2,
                    Name = "ZClassic",
                    Abbreviation = "ZCL",
                    Slug = "ZCL"
                },
                new Currency
                {
                    Id = 588,
                    CurrencyTypeId = 2,
                    Name = "Ubiq",
                    Abbreviation = "UBQ",
                    Slug = "UBQ"
                },
                new Currency
                {
                    Id = 3139,
                    CurrencyTypeId = 2,
                    Name = "DxChain Token",
                    Abbreviation = "DX",
                    Slug = "DX"
                },
                new Currency
                {
                    Id = 1609,
                    CurrencyTypeId = 2,
                    Name = "Asch",
                    Abbreviation = "XAS",
                    Slug = "XAS"
                },
                new Currency
                {
                    Id = 2316,
                    CurrencyTypeId = 2,
                    Name = "DeepBrain Chain",
                    Abbreviation = "DBC",
                    Slug = "DBC"
                },
                new Currency
                {
                    Id = 2021,
                    CurrencyTypeId = 2,
                    Name = "RChain",
                    Abbreviation = "RHOC",
                    Slug = "RHOC"
                },
                new Currency
                {
                    Id = 2369,
                    CurrencyTypeId = 2,
                    Name = "Insolar",
                    Abbreviation = "INS",
                    Slug = "INS"
                },
                new Currency
                {
                    Id = 1856,
                    CurrencyTypeId = 2,
                    Name = "district0x",
                    Abbreviation = "DNT",
                    Slug = "DNT"
                },
                new Currency
                {
                    Id = 406,
                    CurrencyTypeId = 2,
                    Name = "Boolberry",
                    Abbreviation = "BBR",
                    Slug = "BBR"
                },
                new Currency
                {
                    Id = 405,
                    CurrencyTypeId = 2,
                    Name = "DigitalNote",
                    Abbreviation = "XDN",
                    Slug = "XDN"
                },
                new Currency
                {
                    Id = 3066,
                    CurrencyTypeId = 2,
                    Name = "BitCapitalVendor",
                    Abbreviation = "BCV",
                    Slug = "BCV"
                },
                new Currency
                {
                    Id = 2223,
                    CurrencyTypeId = 2,
                    Name = "BLOCKv",
                    Abbreviation = "VEE",
                    Slug = "VEE"
                },
                new Currency
                {
                    Id = 558,
                    CurrencyTypeId = 2,
                    Name = "Emercoin",
                    Abbreviation = "EMC",
                    Slug = "EMC"
                },
                new Currency
                {
                    Id = 2341,
                    CurrencyTypeId = 2,
                    Name = "SwftCoin",
                    Abbreviation = "SWFTC",
                    Slug = "SWFTC"
                },
                new Currency
                {
                    Id = 2556,
                    CurrencyTypeId = 2,
                    Name = "Credits",
                    Abbreviation = "CS",
                    Slug = "CS"
                },
                new Currency
                {
                    Id = 2239,
                    CurrencyTypeId = 2,
                    Name = "ETHLend",
                    Abbreviation = "LEND",
                    Slug = "LEND"
                },
                new Currency
                {
                    Id = 624,
                    CurrencyTypeId = 2,
                    Name = "bitCNY",
                    Abbreviation = "BITCNY",
                    Slug = "BITCNY"
                },
                new Currency
                {
                    Id = 1949,
                    CurrencyTypeId = 2,
                    Name = "Agrello",
                    Abbreviation = "DLT",
                    Slug = "DLT"
                },
                new Currency
                {
                    Id = 1681,
                    CurrencyTypeId = 2,
                    Name = "PRIZM",
                    Abbreviation = "PZM",
                    Slug = "PZM"
                },
                new Currency
                {
                    Id = 2321,
                    CurrencyTypeId = 2,
                    Name = "QLC Chain",
                    Abbreviation = "QLC",
                    Slug = "QLC"
                },
                new Currency
                {
                    Id = 1814,
                    CurrencyTypeId = 2,
                    Name = "Linda",
                    Abbreviation = "LINDA",
                    Slug = "LINDA"
                },
                new Currency
                {
                    Id = 1409,
                    CurrencyTypeId = 2,
                    Name = "SingularDTV",
                    Abbreviation = "SNGLS",
                    Slug = "SNGLS"
                },
                new Currency
                {
                    Id = 2673,
                    CurrencyTypeId = 2,
                    Name = "Own",
                    Abbreviation = "CHX",
                    Slug = "CHX"
                },
                new Currency
                {
                    Id = 2398,
                    CurrencyTypeId = 2,
                    Name = "Selfkey",
                    Abbreviation = "KEY",
                    Slug = "KEY"
                },
                new Currency
                {
                    Id = 3648,
                    CurrencyTypeId = 2,
                    Name = "CoinUs",
                    Abbreviation = "CNUS",
                    Slug = "CNUS"
                },
                new Currency
                {
                    Id = 37,
                    CurrencyTypeId = 2,
                    Name = "Peercoin",
                    Abbreviation = "PPC",
                    Slug = "PPC"
                },
                new Currency
                {
                    Id = 3686,
                    CurrencyTypeId = 2,
                    Name = "Content Value Network",
                    Abbreviation = "CVNT",
                    Slug = "CVNT"
                },
                new Currency
                {
                    Id = 2526,
                    CurrencyTypeId = 2,
                    Name = "Envion",
                    Abbreviation = "EVN",
                    Slug = "EVN"
                },
                new Currency
                {
                    Id = 2953,
                    CurrencyTypeId = 2,
                    Name = "Blue Whale EXchange",
                    Abbreviation = "BWX",
                    Slug = "BWX"
                },
                new Currency
                {
                    Id = 2204,
                    CurrencyTypeId = 2,
                    Name = "B2BX",
                    Abbreviation = "B2B",
                    Slug = "B2B"
                },
                new Currency
                {
                    Id = 1834,
                    CurrencyTypeId = 2,
                    Name = "Pillar",
                    Abbreviation = "PLR",
                    Slug = "PLR"
                },
                new Currency
                {
                    Id = 2554,
                    CurrencyTypeId = 2,
                    Name = "Lympo",
                    Abbreviation = "LYM",
                    Slug = "LYM"
                },
                new Currency
                {
                    Id = 1723,
                    CurrencyTypeId = 2,
                    Name = "SONM",
                    Abbreviation = "SNM",
                    Slug = "SNM"
                },
                new Currency
                {
                    Id = 1592,
                    CurrencyTypeId = 2,
                    Name = "TaaS",
                    Abbreviation = "TAAS",
                    Slug = "TAAS"
                },
                new Currency
                {
                    Id = 2576,
                    CurrencyTypeId = 2,
                    Name = "Tokenomy",
                    Abbreviation = "TEN",
                    Slug = "TEN"
                },
                new Currency
                {
                    Id = 3475,
                    CurrencyTypeId = 2,
                    Name = "BOX Token",
                    Abbreviation = "BOX",
                    Slug = "BOX"
                },
                new Currency
                {
                    Id = 2473,
                    CurrencyTypeId = 2,
                    Name = "All Sports",
                    Abbreviation = "SOC",
                    Slug = "SOC"
                },
                new Currency
                {
                    Id = 723,
                    CurrencyTypeId = 2,
                    Name = "BitBay",
                    Abbreviation = "BAY",
                    Slug = "BAY"
                },
                new Currency
                {
                    Id = 2153,
                    CurrencyTypeId = 2,
                    Name = "Aeron",
                    Abbreviation = "ARN",
                    Slug = "ARN"
                },
                new Currency
                {
                    Id = 2991,
                    CurrencyTypeId = 2,
                    Name = "NIX",
                    Abbreviation = "NIX",
                    Slug = "NIX"
                },
                new Currency
                {
                    Id = 3316,
                    CurrencyTypeId = 2,
                    Name = "smARTOFGIVING",
                    Abbreviation = "AOG",
                    Slug = "AOG"
                },
                new Currency
                {
                    Id = 2344,
                    CurrencyTypeId = 2,
                    Name = "AppCoins",
                    Abbreviation = "APPC",
                    Slug = "APPC"
                },
                new Currency
                {
                    Id = 1996,
                    CurrencyTypeId = 2,
                    Name = "SALT",
                    Abbreviation = "SALT",
                    Slug = "SALT"
                },
                new Currency
                {
                    Id = 2826,
                    CurrencyTypeId = 2,
                    Name = "Zipper",
                    Abbreviation = "ZIP",
                    Slug = "ZIP"
                },
                new Currency
                {
                    Id = 1726,
                    CurrencyTypeId = 2,
                    Name = "ZrCoin",
                    Abbreviation = "ZRC",
                    Slug = "ZRC"
                },
                new Currency
                {
                    Id = 254,
                    CurrencyTypeId = 2,
                    Name = "Gulden",
                    Abbreviation = "NLG",
                    Slug = "NLG"
                },
                new Currency
                {
                    Id = 1312,
                    CurrencyTypeId = 2,
                    Name = "Steem Dollars",
                    Abbreviation = "SBD",
                    Slug = "SBD"
                },
                new Currency
                {
                    Id = 3664,
                    CurrencyTypeId = 2,
                    Name = "AgaveCoin",
                    Abbreviation = "AGVC",
                    Slug = "AGVC"
                },
                new Currency
                {
                    Id = 2633,
                    CurrencyTypeId = 2,
                    Name = "Stakenet",
                    Abbreviation = "XSN",
                    Slug = "XSN"
                },
                new Currency
                {
                    Id = 2553,
                    CurrencyTypeId = 2,
                    Name = "Refereum",
                    Abbreviation = "RFR",
                    Slug = "RFR"
                },
                new Currency
                {
                    Id = 2642,
                    CurrencyTypeId = 2,
                    Name = "CyberVein",
                    Abbreviation = "CVT",
                    Slug = "CVT"
                },
                new Currency
                {
                    Id = 2467,
                    CurrencyTypeId = 2,
                    Name = "OriginTrail",
                    Abbreviation = "TRAC",
                    Slug = "TRAC"
                },
                new Currency
                {
                    Id = 2019,
                    CurrencyTypeId = 2,
                    Name = "Viberate",
                    Abbreviation = "VIB",
                    Slug = "VIB"
                },
                new Currency
                {
                    Id = 2287,
                    CurrencyTypeId = 2,
                    Name = "LockTrip",
                    Abbreviation = "LOC",
                    Slug = "LOC"
                },
                new Currency
                {
                    Id = 3702,
                    CurrencyTypeId = 2,
                    Name = "Beam",
                    Abbreviation = "BEAM",
                    Slug = "BEAM"
                },
                new Currency
                {
                    Id = 1984,
                    CurrencyTypeId = 2,
                    Name = "Substratum",
                    Abbreviation = "SUB",
                    Slug = "SUB"
                },
                new Currency
                {
                    Id = 2336,
                    CurrencyTypeId = 2,
                    Name = "Game.com",
                    Abbreviation = "GTC",
                    Slug = "GTC"
                },
                new Currency
                {
                    Id = 2876,
                    CurrencyTypeId = 2,
                    Name = "Ternio",
                    Abbreviation = "TERN",
                    Slug = "TERN"
                },
                new Currency
                {
                    Id = 3911,
                    CurrencyTypeId = 2,
                    Name = "Ocean Protocol",
                    Abbreviation = "OCEAN",
                    Slug = "OCEAN"
                },
                new Currency
                {
                    Id = 1947,
                    CurrencyTypeId = 2,
                    Name = "Monetha",
                    Abbreviation = "MTH",
                    Slug = "MTH"
                },
                new Currency
                {
                    Id = 2303,
                    CurrencyTypeId = 2,
                    Name = "MediBloc [QRC20]",
                    Abbreviation = "MED",
                    Slug = "MED"
                },
                new Currency
                {
                    Id = 1552,
                    CurrencyTypeId = 2,
                    Name = "Melon",
                    Abbreviation = "MLN",
                    Slug = "MLN"
                },
                new Currency
                {
                    Id = 2120,
                    CurrencyTypeId = 2,
                    Name = "Etherparty",
                    Abbreviation = "FUEL",
                    Slug = "FUEL"
                },
                new Currency
                {
                    Id = 2061,
                    CurrencyTypeId = 2,
                    Name = "BlockMason Credit Protocol",
                    Abbreviation = "BCPT",
                    Slug = "BCPT"
                },
                new Currency
                {
                    Id = 1026,
                    CurrencyTypeId = 2,
                    Name = "Aeon",
                    Abbreviation = "AEON",
                    Slug = "AEON"
                },
                new Currency
                {
                    Id = 2318,
                    CurrencyTypeId = 2,
                    Name = "Neumark",
                    Abbreviation = "NEU",
                    Slug = "NEU"
                },
                new Currency
                {
                    Id = 2548,
                    CurrencyTypeId = 2,
                    Name = "POA Network",
                    Abbreviation = "POA",
                    Slug = "POA"
                },
                new Currency
                {
                    Id = 2058,
                    CurrencyTypeId = 2,
                    Name = "AirSwap",
                    Abbreviation = "AST",
                    Slug = "AST"
                },
                new Currency
                {
                    Id = 3655,
                    CurrencyTypeId = 2,
                    Name = "Fiii",
                    Abbreviation = "FIII",
                    Slug = "FIII"
                },
                new Currency
                {
                    Id = 3307,
                    CurrencyTypeId = 2,
                    Name = "Spendcoin",
                    Abbreviation = "SPND",
                    Slug = "SPND"
                },
                new Currency
                {
                    Id = 3928,
                    CurrencyTypeId = 2,
                    Name = "IDEX",
                    Abbreviation = "IDEX",
                    Slug = "IDEX"
                },
                new Currency
                {
                    Id = 3710,
                    CurrencyTypeId = 2,
                    Name = "SDChain",
                    Abbreviation = "SDA",
                    Slug = "SDA"
                },
                new Currency
                {
                    Id = 3632,
                    CurrencyTypeId = 2,
                    Name = "Opacity",
                    Abbreviation = "OPQ",
                    Slug = "OPQ"
                },
                new Currency
                {
                    Id = 2506,
                    CurrencyTypeId = 2,
                    Name = "Swarm",
                    Abbreviation = "SWM",
                    Slug = "SWM"
                },
                new Currency
                {
                    Id = 2600,
                    CurrencyTypeId = 2,
                    Name = "LGO Exchange",
                    Abbreviation = "LGO",
                    Slug = "LGO"
                },
                new Currency
                {
                    Id = 1841,
                    CurrencyTypeId = 2,
                    Name = "Primalbase Token",
                    Abbreviation = "PBT",
                    Slug = "PBT"
                },
                new Currency
                {
                    Id = 2901,
                    CurrencyTypeId = 2,
                    Name = "FansTime",
                    Abbreviation = "FTI",
                    Slug = "FTI"
                },
                new Currency
                {
                    Id = 2511,
                    CurrencyTypeId = 2,
                    Name = "WePower",
                    Abbreviation = "WPR",
                    Slug = "WPR"
                },
                new Currency
                {
                    Id = 2644,
                    CurrencyTypeId = 2,
                    Name = "eosDAC",
                    Abbreviation = "EOSDAC",
                    Slug = "EOSDAC"
                },
                new Currency
                {
                    Id = 3840,
                    CurrencyTypeId = 2,
                    Name = "1irstcoin",
                    Abbreviation = "FST",
                    Slug = "FST"
                },
                new Currency
                {
                    Id = 2735,
                    CurrencyTypeId = 2,
                    Name = "Content Neutrality Network",
                    Abbreviation = "CNN",
                    Slug = "CNN"
                },
                new Currency
                {
                    Id = 1864,
                    CurrencyTypeId = 2,
                    Name = "Blox",
                    Abbreviation = "CDT",
                    Slug = "CDT"
                },
                new Currency
                {
                    Id = 1475,
                    CurrencyTypeId = 2,
                    Name = "Incent",
                    Abbreviation = "INCNT",
                    Slug = "INCNT"
                },
                new Currency
                {
                    Id = 2561,
                    CurrencyTypeId = 2,
                    Name = "BitTube",
                    Abbreviation = "TUBE",
                    Slug = "TUBE"
                },
                new Currency
                {
                    Id = 3683,
                    CurrencyTypeId = 2,
                    Name = "Aencoin",
                    Abbreviation = "AEN",
                    Slug = "AEN"
                },
                new Currency
                {
                    Id = 2726,
                    CurrencyTypeId = 2,
                    Name = "DAOstack",
                    Abbreviation = "GEN",
                    Slug = "GEN"
                },
                new Currency
                {
                    Id = 1298,
                    CurrencyTypeId = 2,
                    Name = "LBRY Credits",
                    Abbreviation = "LBC",
                    Slug = "LBC"
                },
                new Currency
                {
                    Id = 2036,
                    CurrencyTypeId = 2,
                    Name = "PayPie",
                    Abbreviation = "PPP",
                    Slug = "PPP"
                },
                new Currency
                {
                    Id = 2675,
                    CurrencyTypeId = 2,
                    Name = "Dock",
                    Abbreviation = "DOCK",
                    Slug = "DOCK"
                },
                new Currency
                {
                    Id = 2081,
                    CurrencyTypeId = 2,
                    Name = "Ambrosus",
                    Abbreviation = "AMB",
                    Slug = "AMB"
                },
                new Currency
                {
                    Id = 2399,
                    CurrencyTypeId = 2,
                    Name = "Internet Node Token",
                    Abbreviation = "INT",
                    Slug = "INT"
                },
                new Currency
                {
                    Id = 2698,
                    CurrencyTypeId = 2,
                    Name = "Hydro",
                    Abbreviation = "HYDRO",
                    Slug = "HYDRO"
                },
                new Currency
                {
                    Id = 2604,
                    CurrencyTypeId = 2,
                    Name = "BitGreen",
                    Abbreviation = "BITG",
                    Slug = "BITG"
                },
                new Currency
                {
                    Id = 3853,
                    CurrencyTypeId = 2,
                    Name = "MultiVAC",
                    Abbreviation = "MTV",
                    Slug = "MTV"
                },
                new Currency
                {
                    Id = 3642,
                    CurrencyTypeId = 2,
                    Name = "Trade Token X",
                    Abbreviation = "TIOX",
                    Slug = "TIOX"
                },
                new Currency
                {
                    Id = 2533,
                    CurrencyTypeId = 2,
                    Name = "Restart Energy MWAT",
                    Abbreviation = "MWAT",
                    Slug = "MWAT"
                },
                new Currency
                {
                    Id = 1899,
                    CurrencyTypeId = 2,
                    Name = "YOYOW",
                    Abbreviation = "YOYOW",
                    Slug = "YOYOW"
                },
                new Currency
                {
                    Id = 2095,
                    CurrencyTypeId = 2,
                    Name = "BOScoin",
                    Abbreviation = "BOS",
                    Slug = "BOS"
                },
                new Currency
                {
                    Id = 914,
                    CurrencyTypeId = 2,
                    Name = "Sphere",
                    Abbreviation = "SPHR",
                    Slug = "SPHR"
                },
                new Currency
                {
                    Id = 1715,
                    CurrencyTypeId = 2,
                    Name = "MobileGo",
                    Abbreviation = "MGO",
                    Slug = "MGO"
                },
                new Currency
                {
                    Id = 1473,
                    CurrencyTypeId = 2,
                    Name = "Pascal Coin",
                    Abbreviation = "PASC",
                    Slug = "PASC"
                },
                new Currency
                {
                    Id = 3142,
                    CurrencyTypeId = 2,
                    Name = "BaaSid",
                    Abbreviation = "BAAS",
                    Slug = "BAAS"
                },
                new Currency
                {
                    Id = 2602,
                    CurrencyTypeId = 2,
                    Name = "NaPoleonX",
                    Abbreviation = "NPX",
                    Slug = "NPX"
                },
                new Currency
                {
                    Id = 1022,
                    CurrencyTypeId = 2,
                    Name = "LEOcoin",
                    Abbreviation = "LEO",
                    Slug = "LEO"
                },
                new Currency
                {
                    Id = 1930,
                    CurrencyTypeId = 2,
                    Name = "Primas",
                    Abbreviation = "PST",
                    Slug = "PST"
                },
                new Currency
                {
                    Id = 3618,
                    CurrencyTypeId = 2,
                    Name = "STACS",
                    Abbreviation = "STACS",
                    Slug = "STACS"
                },
                new Currency
                {
                    Id = 2916,
                    CurrencyTypeId = 2,
                    Name = "Nimiq",
                    Abbreviation = "NIM",
                    Slug = "NIM"
                },
                new Currency
                {
                    Id = 2529,
                    CurrencyTypeId = 2,
                    Name = "Cashaa",
                    Abbreviation = "CAS",
                    Slug = "CAS"
                },
                new Currency
                {
                    Id = 2498,
                    CurrencyTypeId = 2,
                    Name = "Jibrel Network",
                    Abbreviation = "JNT",
                    Slug = "JNT"
                },
                new Currency
                {
                    Id = 2482,
                    CurrencyTypeId = 2,
                    Name = "CPChain",
                    Abbreviation = "CPC",
                    Slug = "CPC"
                },
                new Currency
                {
                    Id = 42,
                    CurrencyTypeId = 2,
                    Name = "Primecoin",
                    Abbreviation = "XPM",
                    Slug = "XPM"
                },
                new Currency
                {
                    Id = 2064,
                    CurrencyTypeId = 2,
                    Name = "Maecenas",
                    Abbreviation = "ART",
                    Slug = "ART"
                },
                new Currency
                {
                    Id = 2429,
                    CurrencyTypeId = 2,
                    Name = "Mobius",
                    Abbreviation = "MOBI",
                    Slug = "MOBI"
                },
                new Currency
                {
                    Id = 2693,
                    CurrencyTypeId = 2,
                    Name = "Loopring [NEO]",
                    Abbreviation = "LRN",
                    Slug = "LRN"
                },
                new Currency
                {
                    Id = 1527,
                    CurrencyTypeId = 2,
                    Name = "Waves Community Token",
                    Abbreviation = "WCT",
                    Slug = "WCT"
                },
                new Currency
                {
                    Id = 3719,
                    CurrencyTypeId = 2,
                    Name = "StableUSD",
                    Abbreviation = "USDS",
                    Slug = "USDS"
                },
                new Currency
                {
                    Id = 3040,
                    CurrencyTypeId = 2,
                    Name = "CanonChain",
                    Abbreviation = "CZR",
                    Slug = "CZR"
                },
                new Currency
                {
                    Id = 2245,
                    CurrencyTypeId = 2,
                    Name = "Presearch",
                    Abbreviation = "PRE",
                    Slug = "PRE"
                },
                new Currency
                {
                    Id = 2175,
                    CurrencyTypeId = 2,
                    Name = "DecentBet",
                    Abbreviation = "DBET",
                    Slug = "DBET"
                },
                new Currency
                {
                    Id = 170,
                    CurrencyTypeId = 2,
                    Name = "BlackCoin",
                    Abbreviation = "BLK",
                    Slug = "BLK"
                },
                new Currency
                {
                    Id = 576,
                    CurrencyTypeId = 2,
                    Name = "GameCredits",
                    Abbreviation = "GAME",
                    Slug = "GAME"
                },
                new Currency
                {
                    Id = 2540,
                    CurrencyTypeId = 2,
                    Name = "Litecoin Cash",
                    Abbreviation = "LCC",
                    Slug = "LCC"
                },
                new Currency
                {
                    Id = 448,
                    CurrencyTypeId = 2,
                    Name = "Stealth",
                    Abbreviation = "XST",
                    Slug = "XST"
                },
                new Currency
                {
                    Id = 1590,
                    CurrencyTypeId = 2,
                    Name = "Mercury",
                    Abbreviation = "MER",
                    Slug = "MER"
                },
                new Currency
                {
                    Id = 1050,
                    CurrencyTypeId = 2,
                    Name = "Shift",
                    Abbreviation = "SHIFT",
                    Slug = "SHIFT"
                },
                new Currency
                {
                    Id = 573,
                    CurrencyTypeId = 2,
                    Name = "Burst",
                    Abbreviation = "BURST",
                    Slug = "BURST"
                },
                new Currency
                {
                    Id = 2291,
                    CurrencyTypeId = 2,
                    Name = "Genaro Network",
                    Abbreviation = "GNX",
                    Slug = "GNX"
                },
                new Currency
                {
                    Id = 2691,
                    CurrencyTypeId = 2,
                    Name = "Penta",
                    Abbreviation = "PNT",
                    Slug = "PNT"
                },
                new Currency
                {
                    Id = 40,
                    CurrencyTypeId = 2,
                    Name = "Namecoin",
                    Abbreviation = "NMC",
                    Slug = "NMC"
                },
                new Currency
                {
                    Id = 2945,
                    CurrencyTypeId = 2,
                    Name = "ContentBox",
                    Abbreviation = "BOX",
                    Slug = "BOX"
                },
                new Currency
                {
                    Id = 1658,
                    CurrencyTypeId = 2,
                    Name = "Lunyr",
                    Abbreviation = "LUN",
                    Slug = "LUN"
                },
                new Currency
                {
                    Id = 2711,
                    CurrencyTypeId = 2,
                    Name = "doc.com Token",
                    Abbreviation = "MTC",
                    Slug = "MTC"
                },
                new Currency
                {
                    Id = 2748,
                    CurrencyTypeId = 2,
                    Name = "Loki",
                    Abbreviation = "LOKI",
                    Slug = "LOKI"
                },
                new Currency
                {
                    Id = 2958,
                    CurrencyTypeId = 2,
                    Name = "TurtleCoin",
                    Abbreviation = "TRTL",
                    Slug = "TRTL"
                },
                new Currency
                {
                    Id = 2524,
                    CurrencyTypeId = 2,
                    Name = "Universa",
                    Abbreviation = "UTNP",
                    Slug = "UTNP"
                },
                new Currency
                {
                    Id = 2375,
                    CurrencyTypeId = 2,
                    Name = "QunQun",
                    Abbreviation = "QUN",
                    Slug = "QUN"
                },
                new Currency
                {
                    Id = 2838,
                    CurrencyTypeId = 2,
                    Name = "PCHAIN",
                    Abbreviation = "PI",
                    Slug = "PI"
                },
                new Currency
                {
                    Id = 2481,
                    CurrencyTypeId = 2,
                    Name = "Zeepin",
                    Abbreviation = "ZPT",
                    Slug = "ZPT"
                },
                new Currency
                {
                    Id = 2480,
                    CurrencyTypeId = 2,
                    Name = "HalalChain",
                    Abbreviation = "HLC",
                    Slug = "HLC"
                },
                new Currency
                {
                    Id = 2776,
                    CurrencyTypeId = 2,
                    Name = "Travala.com",
                    Abbreviation = "AVA",
                    Slug = "AVA"
                },
                new Currency
                {
                    Id = 2763,
                    CurrencyTypeId = 2,
                    Name = "Morpheus.Network",
                    Abbreviation = "MRPH",
                    Slug = "MRPH"
                },
                new Currency
                {
                    Id = 2650,
                    CurrencyTypeId = 2,
                    Name = "CommerceBlock",
                    Abbreviation = "CBT",
                    Slug = "CBT"
                },
                new Currency
                {
                    Id = 2546,
                    CurrencyTypeId = 2,
                    Name = "Remme",
                    Abbreviation = "REM",
                    Slug = "REM"
                },
                new Currency
                {
                    Id = 3242,
                    CurrencyTypeId = 2,
                    Name = "Beetle Coin",
                    Abbreviation = "BEET",
                    Slug = "BEET"
                },
                new Currency
                {
                    Id = 1636,
                    CurrencyTypeId = 2,
                    Name = "XTRABYTES",
                    Abbreviation = "XBY",
                    Slug = "XBY"
                },
                new Currency
                {
                    Id = 2392,
                    CurrencyTypeId = 2,
                    Name = "Bottos",
                    Abbreviation = "BTO",
                    Slug = "BTO"
                },
                new Currency
                {
                    Id = 3441,
                    CurrencyTypeId = 2,
                    Name = "Divi",
                    Abbreviation = "DIVI",
                    Slug = "DIVI"
                },
                new Currency
                {
                    Id = 3029,
                    CurrencyTypeId = 2,
                    Name = "ZelCash",
                    Abbreviation = "ZEL",
                    Slug = "ZEL"
                },
                new Currency
                {
                    Id = 3843,
                    CurrencyTypeId = 2,
                    Name = "BOLT",
                    Abbreviation = "BOLT",
                    Slug = "BOLT"
                },
                new Currency
                {
                    Id = 3260,
                    CurrencyTypeId = 2,
                    Name = "AMO Coin",
                    Abbreviation = "AMO",
                    Slug = "AMO"
                },
                new Currency
                {
                    Id = 1478,
                    CurrencyTypeId = 2,
                    Name = "DECENT",
                    Abbreviation = "DCT",
                    Slug = "DCT"
                },
                new Currency
                {
                    Id = 2209,
                    CurrencyTypeId = 2,
                    Name = "Ink",
                    Abbreviation = "INK",
                    Slug = "INK"
                },
                new Currency
                {
                    Id = 1775,
                    CurrencyTypeId = 2,
                    Name = "adToken",
                    Abbreviation = "ADT",
                    Slug = "ADT"
                },
                new Currency
                {
                    Id = 3515,
                    CurrencyTypeId = 2,
                    Name = "DEX",
                    Abbreviation = "DEX",
                    Slug = "DEX"
                },
                new Currency
                {
                    Id = 3838,
                    CurrencyTypeId = 2,
                    Name = "Esportbits",
                    Abbreviation = "HLT",
                    Slug = "HLT"
                },
                new Currency
                {
                    Id = 2866,
                    CurrencyTypeId = 2,
                    Name = "Sentinel Protocol",
                    Abbreviation = "UPP",
                    Slug = "UPP"
                },
                new Currency
                {
                    Id = 3650,
                    CurrencyTypeId = 2,
                    Name = "COVA",
                    Abbreviation = "COVA",
                    Slug = "COVA"
                },
                new Currency
                {
                    Id = 3842,
                    CurrencyTypeId = 2,
                    Name = "Caspian",
                    Abbreviation = "CSP",
                    Slug = "CSP"
                },
                new Currency
                {
                    Id = 2348,
                    CurrencyTypeId = 2,
                    Name = "Measurable Data Token",
                    Abbreviation = "MDT",
                    Slug = "MDT"
                },
                new Currency
                {
                    Id = 1786,
                    CurrencyTypeId = 2,
                    Name = "SunContract",
                    Abbreviation = "SNC",
                    Slug = "SNC"
                },
                new Currency
                {
                    Id = 3628,
                    CurrencyTypeId = 2,
                    Name = "Machine Xchange Coin",
                    Abbreviation = "MXC",
                    Slug = "MXC"
                },
                new Currency
                {
                    Id = 2665,
                    CurrencyTypeId = 2,
                    Name = "Dero",
                    Abbreviation = "DERO",
                    Slug = "DERO"
                },
                new Currency
                {
                    Id = 1976,
                    CurrencyTypeId = 2,
                    Name = "Blackmoon",
                    Abbreviation = "BMC",
                    Slug = "BMC"
                },
                new Currency
                {
                    Id = 1881,
                    CurrencyTypeId = 2,
                    Name = "DeepOnion",
                    Abbreviation = "ONION",
                    Slug = "ONION"
                },
                new Currency
                {
                    Id = 2847,
                    CurrencyTypeId = 2,
                    Name = "Abyss Token",
                    Abbreviation = "ABYSS",
                    Slug = "ABYSS"
                },
                new Currency
                {
                    Id = 3694,
                    CurrencyTypeId = 2,
                    Name = "INMAX",
                    Abbreviation = "INX",
                    Slug = "INX"
                },
                new Currency
                {
                    Id = 34,
                    CurrencyTypeId = 2,
                    Name = "Feathercoin",
                    Abbreviation = "FTC",
                    Slug = "FTC"
                },
                new Currency
                {
                    Id = 1172,
                    CurrencyTypeId = 2,
                    Name = "Safex Token",
                    Abbreviation = "SFT",
                    Slug = "SFT"
                },
                new Currency
                {
                    Id = 1405,
                    CurrencyTypeId = 2,
                    Name = "Pepe Cash",
                    Abbreviation = "PEPECASH",
                    Slug = "PEPECASH"
                },
                new Currency
                {
                    Id = 2830,
                    CurrencyTypeId = 2,
                    Name = "Seele",
                    Abbreviation = "SEELE",
                    Slug = "SEELE"
                },
                new Currency
                {
                    Id = 2472,
                    CurrencyTypeId = 2,
                    Name = "Fortuna",
                    Abbreviation = "FOTA",
                    Slug = "FOTA"
                },
                new Currency
                {
                    Id = 1403,
                    CurrencyTypeId = 2,
                    Name = "FirstBlood",
                    Abbreviation = "1ST",
                    Slug = "1ST"
                },
                new Currency
                {
                    Id = 3623,
                    CurrencyTypeId = 2,
                    Name = "Online",
                    Abbreviation = "OIO",
                    Slug = "OIO"
                },
                new Currency
                {
                    Id = 2359,
                    CurrencyTypeId = 2,
                    Name = "Polis",
                    Abbreviation = "POLIS",
                    Slug = "POLIS"
                },
                new Currency
                {
                    Id = 1989,
                    CurrencyTypeId = 2,
                    Name = "COSS",
                    Abbreviation = "COSS",
                    Slug = "COSS"
                },
                new Currency
                {
                    Id = 3070,
                    CurrencyTypeId = 2,
                    Name = "Litex",
                    Abbreviation = "LXT",
                    Slug = "LXT"
                },
                new Currency
                {
                    Id = 3156,
                    CurrencyTypeId = 2,
                    Name = "Airbloc",
                    Abbreviation = "ABL",
                    Slug = "ABL"
                },
                new Currency
                {
                    Id = 2342,
                    CurrencyTypeId = 2,
                    Name = "Covesting",
                    Abbreviation = "COV",
                    Slug = "COV"
                },
                new Currency
                {
                    Id = 3913,
                    CurrencyTypeId = 2,
                    Name = "Titan Coin",
                    Abbreviation = "TTN",
                    Slug = "TTN"
                },
                new Currency
                {
                    Id = 2685,
                    CurrencyTypeId = 2,
                    Name = "Zebi",
                    Abbreviation = "ZCO",
                    Slug = "ZCO"
                },
                new Currency
                {
                    Id = 2757,
                    CurrencyTypeId = 2,
                    Name = "Callisto Network",
                    Abbreviation = "CLO",
                    Slug = "CLO"
                },
                new Currency
                {
                    Id = 2737,
                    CurrencyTypeId = 2,
                    Name = "Global Social Chain",
                    Abbreviation = "GSC",
                    Slug = "GSC"
                },
                new Currency
                {
                    Id = 3376,
                    CurrencyTypeId = 2,
                    Name = "Darico Ecosystem Coin",
                    Abbreviation = "DEC",
                    Slug = "DEC"
                },
                new Currency
                {
                    Id = 2468,
                    CurrencyTypeId = 2,
                    Name = "LinkEye",
                    Abbreviation = "LET",
                    Slug = "LET"
                },
                new Currency
                {
                    Id = 3716,
                    CurrencyTypeId = 2,
                    Name = "Amoveo",
                    Abbreviation = "VEO",
                    Slug = "VEO"
                },
                new Currency
                {
                    Id = 2862,
                    CurrencyTypeId = 2,
                    Name = "Smartshare",
                    Abbreviation = "SSP",
                    Slug = "SSP"
                },
                new Currency
                {
                    Id = 2158,
                    CurrencyTypeId = 2,
                    Name = "Phore",
                    Abbreviation = "PHR",
                    Slug = "PHR"
                },
                new Currency
                {
                    Id = 2380,
                    CurrencyTypeId = 2,
                    Name = "ATN",
                    Abbreviation = "ATN",
                    Slug = "ATN"
                },
                new Currency
                {
                    Id = 3815,
                    CurrencyTypeId = 2,
                    Name = "Eterbase Coin",
                    Abbreviation = "XBASE",
                    Slug = "XBASE"
                },
                new Currency
                {
                    Id = 1677,
                    CurrencyTypeId = 2,
                    Name = "Etheroll",
                    Abbreviation = "DICE",
                    Slug = "DICE"
                },
                new Currency
                {
                    Id = 3944,
                    CurrencyTypeId = 2,
                    Name = "Artfinity",
                    Abbreviation = "AT",
                    Slug = "AT"
                },
                new Currency
                {
                    Id = 2535,
                    CurrencyTypeId = 2,
                    Name = "DADI",
                    Abbreviation = "DADI",
                    Slug = "DADI"
                },
                new Currency
                {
                    Id = 2933,
                    CurrencyTypeId = 2,
                    Name = "BitMart Token",
                    Abbreviation = "BMX",
                    Slug = "BMX"
                },
                new Currency
                {
                    Id = 3612,
                    CurrencyTypeId = 2,
                    Name = "Business Credit Alliance Chain",
                    Abbreviation = "BCAC",
                    Slug = "BCAC"
                },
                new Currency
                {
                    Id = 2094,
                    CurrencyTypeId = 2,
                    Name = "Paragon",
                    Abbreviation = "PRG",
                    Slug = "PRG"
                },
                new Currency
                {
                    Id = 2001,
                    CurrencyTypeId = 2,
                    Name = "ColossusXT",
                    Abbreviation = "COLX",
                    Slug = "COLX"
                },
                new Currency
                {
                    Id = 2892,
                    CurrencyTypeId = 2,
                    Name = "Wowbit",
                    Abbreviation = "WWB",
                    Slug = "WWB"
                },
                new Currency
                {
                    Id = 2427,
                    CurrencyTypeId = 2,
                    Name = "ChatCoin",
                    Abbreviation = "CHAT",
                    Slug = "CHAT"
                },
                new Currency
                {
                    Id = 2669,
                    CurrencyTypeId = 2,
                    Name = "MARK.SPACE",
                    Abbreviation = "MRK",
                    Slug = "MRK"
                },
                new Currency
                {
                    Id = 819,
                    CurrencyTypeId = 2,
                    Name = "Bean Cash",
                    Abbreviation = "BITB",
                    Slug = "BITB"
                },
                new Currency
                {
                    Id = 2882,
                    CurrencyTypeId = 2,
                    Name = "0Chain",
                    Abbreviation = "ZCN",
                    Slug = "ZCN"
                },
                new Currency
                {
                    Id = 3281,
                    CurrencyTypeId = 2,
                    Name = "Quanta Utility Token",
                    Abbreviation = "QNTU",
                    Slug = "QNTU"
                },
                new Currency
                {
                    Id = 3813,
                    CurrencyTypeId = 2,
                    Name = "PTON",
                    Abbreviation = "PTON",
                    Slug = "PTON"
                },
                new Currency
                {
                    Id = 2666,
                    CurrencyTypeId = 2,
                    Name = "Effect.AI",
                    Abbreviation = "EFX",
                    Slug = "EFX"
                },
                new Currency
                {
                    Id = 1883,
                    CurrencyTypeId = 2,
                    Name = "Adshares",
                    Abbreviation = "ADS",
                    Slug = "ADS"
                },
                new Currency
                {
                    Id = 2982,
                    CurrencyTypeId = 2,
                    Name = "MVL",
                    Abbreviation = "MVL",
                    Slug = "MVL"
                },
                new Currency
                {
                    Id = 1784,
                    CurrencyTypeId = 2,
                    Name = "Polybius",
                    Abbreviation = "PLBT",
                    Slug = "PLBT"
                },
                new Currency
                {
                    Id = 2162,
                    CurrencyTypeId = 2,
                    Name = "Delphy",
                    Abbreviation = "DPY",
                    Slug = "DPY"
                },
                new Currency
                {
                    Id = 2499,
                    CurrencyTypeId = 2,
                    Name = "SwissBorg",
                    Abbreviation = "CHSB",
                    Slug = "CHSB"
                },
                new Currency
                {
                    Id = 623,
                    CurrencyTypeId = 2,
                    Name = "bitUSD",
                    Abbreviation = "BITUSD",
                    Slug = "BITUSD"
                },
                new Currency
                {
                    Id = 1500,
                    CurrencyTypeId = 2,
                    Name = "Wings",
                    Abbreviation = "WINGS",
                    Slug = "WINGS"
                },
                new Currency
                {
                    Id = 3722,
                    CurrencyTypeId = 2,
                    Name = "TEMCO",
                    Abbreviation = "TEMCO",
                    Slug = "TEMCO"
                },
                new Currency
                {
                    Id = 2739,
                    CurrencyTypeId = 2,
                    Name = "Digix Gold Token",
                    Abbreviation = "DGX",
                    Slug = "DGX"
                },
                new Currency
                {
                    Id = 2938,
                    CurrencyTypeId = 2,
                    Name = "Hashgard",
                    Abbreviation = "GARD",
                    Slug = "GARD"
                },
                new Currency
                {
                    Id = 400,
                    CurrencyTypeId = 2,
                    Name = "Kore",
                    Abbreviation = "KORE",
                    Slug = "KORE"
                },
                new Currency
                {
                    Id = 3200,
                    CurrencyTypeId = 2,
                    Name = "Nasdacoin",
                    Abbreviation = "NSD",
                    Slug = "NSD"
                },
                new Currency
                {
                    Id = 2430,
                    CurrencyTypeId = 2,
                    Name = "Hydro Protocol",
                    Abbreviation = "HOT",
                    Slug = "HOT"
                },
                new Currency
                {
                    Id = 3301,
                    CurrencyTypeId = 2,
                    Name = "Invictus Hyperion Fund",
                    Abbreviation = "IHF",
                    Slug = "IHF"
                },
                new Currency
                {
                    Id = 3371,
                    CurrencyTypeId = 2,
                    Name = "MIR COIN",
                    Abbreviation = "MIR",
                    Slug = "MIR"
                },
                new Currency
                {
                    Id = 3186,
                    CurrencyTypeId = 2,
                    Name = "Zen Protocol",
                    Abbreviation = "ZP",
                    Slug = "ZP"
                },
                new Currency
                {
                    Id = 2327,
                    CurrencyTypeId = 2,
                    Name = "Olympus Labs",
                    Abbreviation = "MOT",
                    Slug = "MOT"
                },
                new Currency
                {
                    Id = 1531,
                    CurrencyTypeId = 2,
                    Name = "Global Cryptocurrency",
                    Abbreviation = "GCC",
                    Slug = "GCC"
                },
                new Currency
                {
                    Id = 2709,
                    CurrencyTypeId = 2,
                    Name = "Morpheus Labs",
                    Abbreviation = "MITX",
                    Slug = "MITX"
                },
                new Currency
                {
                    Id = 3714,
                    CurrencyTypeId = 2,
                    Name = "LTO Network",
                    Abbreviation = "LTO",
                    Slug = "LTO"
                },
                new Currency
                {
                    Id = 3461,
                    CurrencyTypeId = 2,
                    Name = "PlayCoin [ERC20]",
                    Abbreviation = "PLY",
                    Slug = "PLY"
                },
                new Currency
                {
                    Id = 1719,
                    CurrencyTypeId = 2,
                    Name = "Peerplays",
                    Abbreviation = "PPY",
                    Slug = "PPY"
                },
                new Currency
                {
                    Id = 3785,
                    CurrencyTypeId = 2,
                    Name = "AIDUS TOKEN",
                    Abbreviation = "AID",
                    Slug = "AID"
                },
                new Currency
                {
                    Id = 1154,
                    CurrencyTypeId = 2,
                    Name = "Radium",
                    Abbreviation = "RADS",
                    Slug = "RADS"
                },
                new Currency
                {
                    Id = 3233,
                    CurrencyTypeId = 2,
                    Name = "Ulord",
                    Abbreviation = "UT",
                    Slug = "UT"
                },
                new Currency
                {
                    Id = 2641,
                    CurrencyTypeId = 2,
                    Name = "Apex",
                    Abbreviation = "CPX",
                    Slug = "CPX"
                },
                new Currency
                {
                    Id = 2402,
                    CurrencyTypeId = 2,
                    Name = "Sense",
                    Abbreviation = "SENSE",
                    Slug = "SENSE"
                },
                new Currency
                {
                    Id = 77,
                    CurrencyTypeId = 2,
                    Name = "Diamond",
                    Abbreviation = "DMD",
                    Slug = "DMD"
                },
                new Currency
                {
                    Id = 3337,
                    CurrencyTypeId = 2,
                    Name = "QChi",
                    Abbreviation = "QCH",
                    Slug = "QCH"
                },
                new Currency
                {
                    Id = 2828,
                    CurrencyTypeId = 2,
                    Name = "SPINDLE",
                    Abbreviation = "SPD",
                    Slug = "SPD"
                },
                new Currency
                {
                    Id = 3015,
                    CurrencyTypeId = 2,
                    Name = "Brickblock",
                    Abbreviation = "BBK",
                    Slug = "BBK"
                },
                new Currency
                {
                    Id = 2354,
                    CurrencyTypeId = 2,
                    Name = "GET Protocol",
                    Abbreviation = "GET",
                    Slug = "GET-protocol"
                },
                new Currency
                {
                    Id = 2645,
                    CurrencyTypeId = 2,
                    Name = "U Network",
                    Abbreviation = "UUU",
                    Slug = "UUU"
                },
                new Currency
                {
                    Id = 2017,
                    CurrencyTypeId = 2,
                    Name = "KickCoin",
                    Abbreviation = "KICK",
                    Slug = "KICK"
                },
                new Currency
                {
                    Id = 2689,
                    CurrencyTypeId = 2,
                    Name = "Rublix",
                    Abbreviation = "RBLX",
                    Slug = "RBLX"
                },
                new Currency
                {
                    Id = 90,
                    CurrencyTypeId = 2,
                    Name = "Dimecoin",
                    Abbreviation = "DIME",
                    Slug = "DIME"
                },
                new Currency
                {
                    Id = 3713,
                    CurrencyTypeId = 2,
                    Name = "Wibson",
                    Abbreviation = "WIB",
                    Slug = "WIB"
                },
                new Currency
                {
                    Id = 3754,
                    CurrencyTypeId = 2,
                    Name = "EveryCoin ",
                    Abbreviation = "EVY",
                    Slug = "EVY"
                },
                new Currency
                {
                    Id = 3297,
                    CurrencyTypeId = 2,
                    Name = "Gene Source Code Chain",
                    Abbreviation = "GENE",
                    Slug = "GENE"
                },
                new Currency
                {
                    Id = 2315,
                    CurrencyTypeId = 2,
                    Name = "HTMLCOIN",
                    Abbreviation = "HTML",
                    Slug = "HTML"
                },
                new Currency
                {
                    Id = 3666,
                    CurrencyTypeId = 2,
                    Name = "Ultiledger",
                    Abbreviation = "ULT",
                    Slug = "ULT"
                },
                new Currency
                {
                    Id = 699,
                    CurrencyTypeId = 2,
                    Name = "NuShares",
                    Abbreviation = "NSR",
                    Slug = "NSR"
                },
                new Currency
                {
                    Id = 1107,
                    CurrencyTypeId = 2,
                    Name = "PACcoin",
                    Abbreviation = "$PAC",
                    Slug = "$PAC"
                },
                new Currency
                {
                    Id = 3622,
                    CurrencyTypeId = 2,
                    Name = "nOS",
                    Abbreviation = "NOS",
                    Slug = "NOS"
                },
                new Currency
                {
                    Id = 2572,
                    CurrencyTypeId = 2,
                    Name = "BABB",
                    Abbreviation = "BAX",
                    Slug = "BAX"
                },
                new Currency
                {
                    Id = 3327,
                    CurrencyTypeId = 2,
                    Name = "SIX",
                    Abbreviation = "SIX",
                    Slug = "SIX"
                },
                new Currency
                {
                    Id = 3138,
                    CurrencyTypeId = 2,
                    Name = "Noku",
                    Abbreviation = "NOKU",
                    Slug = "NOKU"
                },
                new Currency
                {
                    Id = 2219,
                    CurrencyTypeId = 2,
                    Name = "SpankChain",
                    Abbreviation = "SPANK",
                    Slug = "SPANK"
                },
                new Currency
                {
                    Id = 3727,
                    CurrencyTypeId = 2,
                    Name = "Flowchain",
                    Abbreviation = "FLC",
                    Slug = "FLC"
                },
                new Currency
                {
                    Id = 2410,
                    CurrencyTypeId = 2,
                    Name = "SpaceChain",
                    Abbreviation = "SPC",
                    Slug = "SPC"
                },
                new Currency
                {
                    Id = 3065,
                    CurrencyTypeId = 2,
                    Name = "ICE ROCK MINING",
                    Abbreviation = "ROCK2",
                    Slug = "ROCK2"
                },
                new Currency
                {
                    Id = 2859,
                    CurrencyTypeId = 2,
                    Name = "XMax",
                    Abbreviation = "XMX",
                    Slug = "XMX"
                },
                new Currency
                {
                    Id = 2149,
                    CurrencyTypeId = 2,
                    Name = "Unikoin Gold",
                    Abbreviation = "UKG",
                    Slug = "UKG"
                },
                new Currency
                {
                    Id = 2972,
                    CurrencyTypeId = 2,
                    Name = "ZPER",
                    Abbreviation = "ZPR",
                    Slug = "ZPR"
                },
                new Currency
                {
                    Id = 182,
                    CurrencyTypeId = 2,
                    Name = "Myriad",
                    Abbreviation = "XMY",
                    Slug = "XMY"
                },
                new Currency
                {
                    Id = 3634,
                    CurrencyTypeId = 2,
                    Name = "Kambria",
                    Abbreviation = "KAT",
                    Slug = "KAT"
                },
                new Currency
                {
                    Id = 495,
                    CurrencyTypeId = 2,
                    Name = "I/O Coin",
                    Abbreviation = "IOC",
                    Slug = "IOC"
                },
                new Currency
                {
                    Id = 1998,
                    CurrencyTypeId = 2,
                    Name = "Ormeus Coin",
                    Abbreviation = "ORME",
                    Slug = "ORME"
                },
                new Currency
                {
                    Id = 3590,
                    CurrencyTypeId = 2,
                    Name = "CrypticCoin",
                    Abbreviation = "CRYP",
                    Slug = "CRYP"
                },
                new Currency
                {
                    Id = 3279,
                    CurrencyTypeId = 2,
                    Name = "Rotharium",
                    Abbreviation = "RTH",
                    Slug = "RTH"
                },
                new Currency
                {
                    Id = 2337,
                    CurrencyTypeId = 2,
                    Name = "Lamden",
                    Abbreviation = "TAU",
                    Slug = "TAU"
                },
                new Currency
                {
                    Id = 2841,
                    CurrencyTypeId = 2,
                    Name = "LoyalCoin",
                    Abbreviation = "LYL",
                    Slug = "LYL"
                },
                new Currency
                {
                    Id = 362,
                    CurrencyTypeId = 2,
                    Name = "CloakCoin",
                    Abbreviation = "CLOAK",
                    Slug = "CLOAK"
                },
                new Currency
                {
                    Id = 1771,
                    CurrencyTypeId = 2,
                    Name = "DAO.Casino",
                    Abbreviation = "BET",
                    Slug = "BET"
                },
                new Currency
                {
                    Id = 3227,
                    CurrencyTypeId = 2,
                    Name = "Traceability Chain",
                    Abbreviation = "TAC",
                    Slug = "TAC"
                },
                new Currency
                {
                    Id = 2305,
                    CurrencyTypeId = 2,
                    Name = "NAGA",
                    Abbreviation = "NGC",
                    Slug = "NGC"
                },
                new Currency
                {
                    Id = 2578,
                    CurrencyTypeId = 2,
                    Name = "TE-FOOD",
                    Abbreviation = "TFD",
                    Slug = "TFD"
                },
                new Currency
                {
                    Id = 2450,
                    CurrencyTypeId = 2,
                    Name = "carVertical",
                    Abbreviation = "CV",
                    Slug = "CV"
                },
                new Currency
                {
                    Id = 2686,
                    CurrencyTypeId = 2,
                    Name = "Lendingblock",
                    Abbreviation = "LND",
                    Slug = "LND"
                },
                new Currency
                {
                    Id = 720,
                    CurrencyTypeId = 2,
                    Name = "Crown",
                    Abbreviation = "CRW",
                    Slug = "CRW"
                },
                new Currency
                {
                    Id = 2630,
                    CurrencyTypeId = 2,
                    Name = "PolySwarm",
                    Abbreviation = "NCT",
                    Slug = "NCT"
                },
                new Currency
                {
                    Id = 2667,
                    CurrencyTypeId = 2,
                    Name = "FintruX Network",
                    Abbreviation = "FTX",
                    Slug = "FTX"
                },
                new Currency
                {
                    Id = 3305,
                    CurrencyTypeId = 2,
                    Name = "Eden",
                    Abbreviation = "EDN",
                    Slug = "EDN"
                },
                new Currency
                {
                    Id = 3850,
                    CurrencyTypeId = 2,
                    Name = "OTOCASH",
                    Abbreviation = "OTO",
                    Slug = "OTO"
                },
                new Currency
                {
                    Id = 2387,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Atom",
                    Abbreviation = "BCA",
                    Slug = "BCA"
                },
                new Currency
                {
                    Id = 3199,
                    CurrencyTypeId = 2,
                    Name = "Cashbery Coin",
                    Abbreviation = "CBC",
                    Slug = "CBC"
                },
                new Currency
                {
                    Id = 3811,
                    CurrencyTypeId = 2,
                    Name = "IntelliShare",
                    Abbreviation = "INE",
                    Slug = "INE"
                },
                new Currency
                {
                    Id = 122,
                    CurrencyTypeId = 2,
                    Name = "PotCoin",
                    Abbreviation = "POT",
                    Slug = "POT"
                },
                new Currency
                {
                    Id = 3082,
                    CurrencyTypeId = 2,
                    Name = "VINchain",
                    Abbreviation = "VIN",
                    Slug = "VIN"
                },
                new Currency
                {
                    Id = 3867,
                    CurrencyTypeId = 2,
                    Name = "NeoWorld Cash",
                    Abbreviation = "NASH",
                    Slug = "NASH"
                },
                new Currency
                {
                    Id = 2340,
                    CurrencyTypeId = 2,
                    Name = "Bloom",
                    Abbreviation = "BLT",
                    Slug = "BLT"
                },
                new Currency
                {
                    Id = 2659,
                    CurrencyTypeId = 2,
                    Name = "Dignity",
                    Abbreviation = "DIG",
                    Slug = "DIG"
                },
                new Currency
                {
                    Id = 3698,
                    CurrencyTypeId = 2,
                    Name = "Observer",
                    Abbreviation = "OBSR",
                    Slug = "OBSR"
                },
                new Currency
                {
                    Id = 2569,
                    CurrencyTypeId = 2,
                    Name = "CoinPoker",
                    Abbreviation = "CHP",
                    Slug = "CHP"
                },
                new Currency
                {
                    Id = 2437,
                    CurrencyTypeId = 2,
                    Name = "YEE",
                    Abbreviation = "YEE",
                    Slug = "YEE"
                },
                new Currency
                {
                    Id = 3830,
                    CurrencyTypeId = 2,
                    Name = "Veil",
                    Abbreviation = "VEIL",
                    Slug = "VEIL"
                },
                new Currency
                {
                    Id = 323,
                    CurrencyTypeId = 2,
                    Name = "VeriCoin",
                    Abbreviation = "VRC",
                    Slug = "VRC"
                },
                new Currency
                {
                    Id = 2610,
                    CurrencyTypeId = 2,
                    Name = "Peculium",
                    Abbreviation = "PCL",
                    Slug = "PCL"
                },
                new Currency
                {
                    Id = 2070,
                    CurrencyTypeId = 2,
                    Name = "DomRaider",
                    Abbreviation = "DRT",
                    Slug = "DRT"
                },
                new Currency
                {
                    Id = 2758,
                    CurrencyTypeId = 2,
                    Name = "Unibright",
                    Abbreviation = "UBT",
                    Slug = "UBT"
                },
                new Currency
                {
                    Id = 833,
                    CurrencyTypeId = 2,
                    Name = "GridCoin",
                    Abbreviation = "GRC",
                    Slug = "GRC"
                },
                new Currency
                {
                    Id = 3748,
                    CurrencyTypeId = 2,
                    Name = "Hxro",
                    Abbreviation = "HXRO",
                    Slug = "HXRO"
                },
                new Currency
                {
                    Id = 1950,
                    CurrencyTypeId = 2,
                    Name = "Hiveterminal Token",
                    Abbreviation = "HVN",
                    Slug = "HVN"
                },
                new Currency
                {
                    Id = 3081,
                    CurrencyTypeId = 2,
                    Name = "Omnitude",
                    Abbreviation = "ECOM",
                    Slug = "ECOM"
                },
                new Currency
                {
                    Id = 3649,
                    CurrencyTypeId = 2,
                    Name = "Plus-Coin",
                    Abbreviation = "NPLC",
                    Slug = "NPLC"
                },
                new Currency
                {
                    Id = 2998,
                    CurrencyTypeId = 2,
                    Name = "Vexanium",
                    Abbreviation = "VEX",
                    Slug = "VEX"
                },
                new Currency
                {
                    Id = 2060,
                    CurrencyTypeId = 2,
                    Name = "Change",
                    Abbreviation = "CAG",
                    Slug = "CAG"
                },
                new Currency
                {
                    Id = 3598,
                    CurrencyTypeId = 2,
                    Name = "Optimal Shelf Availability Token",
                    Abbreviation = "OSA",
                    Slug = "OSA"
                },
                new Currency
                {
                    Id = 1810,
                    CurrencyTypeId = 2,
                    Name = "CVCoin",
                    Abbreviation = "CVN",
                    Slug = "CVN"
                },
                new Currency
                {
                    Id = 2696,
                    CurrencyTypeId = 2,
                    Name = "DAEX",
                    Abbreviation = "DAX",
                    Slug = "DAX"
                },
                new Currency
                {
                    Id = 2725,
                    CurrencyTypeId = 2,
                    Name = "Skrumble Network",
                    Abbreviation = "SKM",
                    Slug = "SKM"
                },
                new Currency
                {
                    Id = 3768,
                    CurrencyTypeId = 2,
                    Name = "PIBBLE",
                    Abbreviation = "PIB",
                    Slug = "PIB"
                },
                new Currency
                {
                    Id = 3920,
                    CurrencyTypeId = 2,
                    Name = "Diamond Platform Token",
                    Abbreviation = "DPT",
                    Slug = "DPT"
                },
                new Currency
                {
                    Id = 2902,
                    CurrencyTypeId = 2,
                    Name = "POPCHAIN",
                    Abbreviation = "PCH",
                    Slug = "PCH"
                },
                new Currency
                {
                    Id = 2662,
                    CurrencyTypeId = 2,
                    Name = "Haven Protocol",
                    Abbreviation = "XHV",
                    Slug = "XHV"
                },
                new Currency
                {
                    Id = 1905,
                    CurrencyTypeId = 2,
                    Name = "TrueFlip",
                    Abbreviation = "TFL",
                    Slug = "TFL"
                },
                new Currency
                {
                    Id = 2827,
                    CurrencyTypeId = 2,
                    Name = "Phantasma",
                    Abbreviation = "SOUL",
                    Slug = "SOUL"
                },
                new Currency
                {
                    Id = 3824,
                    CurrencyTypeId = 2,
                    Name = "Vanta Network",
                    Abbreviation = "VNT",
                    Slug = "VNT"
                },
                new Currency
                {
                    Id = 3782,
                    CurrencyTypeId = 2,
                    Name = "ONOToken",
                    Abbreviation = "ONOT",
                    Slug = "ONOT"
                },
                new Currency
                {
                    Id = 3663,
                    CurrencyTypeId = 2,
                    Name = "Footballcoin",
                    Abbreviation = "XFC",
                    Slug = "XFC"
                },
                new Currency
                {
                    Id = 760,
                    CurrencyTypeId = 2,
                    Name = "OKCash",
                    Abbreviation = "OK",
                    Slug = "OK"
                },
                new Currency
                {
                    Id = 27,
                    CurrencyTypeId = 2,
                    Name = "GoldCoin",
                    Abbreviation = "GLC",
                    Slug = "GLC"
                },
                new Currency
                {
                    Id = 920,
                    CurrencyTypeId = 2,
                    Name = "SounDAC",
                    Abbreviation = "XSD",
                    Slug = "XSD"
                },
                new Currency
                {
                    Id = 1044,
                    CurrencyTypeId = 2,
                    Name = "Global Currency Reserve",
                    Abbreviation = "GCR",
                    Slug = "GCR"
                },
                new Currency
                {
                    Id = 895,
                    CurrencyTypeId = 2,
                    Name = "Xaurum",
                    Abbreviation = "XAUR",
                    Slug = "XAUR"
                },
                new Currency
                {
                    Id = 3023,
                    CurrencyTypeId = 2,
                    Name = "Semux",
                    Abbreviation = "SEM",
                    Slug = "SEM"
                },
                new Currency
                {
                    Id = 333,
                    CurrencyTypeId = 2,
                    Name = "Curecoin",
                    Abbreviation = "CURE",
                    Slug = "CURE"
                },
                new Currency
                {
                    Id = 2957,
                    CurrencyTypeId = 2,
                    Name = "Olive",
                    Abbreviation = "OLE",
                    Slug = "OLE"
                },
                new Currency
                {
                    Id = 2621,
                    CurrencyTypeId = 2,
                    Name = "Consensus",
                    Abbreviation = "SEN",
                    Slug = "SEN"
                },
                new Currency
                {
                    Id = 2309,
                    CurrencyTypeId = 2,
                    Name = "SophiaTX",
                    Abbreviation = "SPHTX",
                    Slug = "SPHTX"
                },
                new Currency
                {
                    Id = 3014,
                    CurrencyTypeId = 2,
                    Name = "RightMesh",
                    Abbreviation = "RMESH",
                    Slug = "RMESH"
                },
                new Currency
                {
                    Id = 3581,
                    CurrencyTypeId = 2,
                    Name = "Kleros",
                    Abbreviation = "PNK",
                    Slug = "PNK"
                },
                new Currency
                {
                    Id = 2643,
                    CurrencyTypeId = 2,
                    Name = "Sentinel",
                    Abbreviation = "SENT",
                    Slug = "SENT"
                },
                new Currency
                {
                    Id = 2477,
                    CurrencyTypeId = 2,
                    Name = "Nework",
                    Abbreviation = "NKC",
                    Slug = "NKC"
                },
                new Currency
                {
                    Id = 2571,
                    CurrencyTypeId = 2,
                    Name = "Graft",
                    Abbreviation = "GRFT",
                    Slug = "GRFT"
                },
                new Currency
                {
                    Id = 2913,
                    CurrencyTypeId = 2,
                    Name = "DaTa eXchange",
                    Abbreviation = "DTX",
                    Slug = "DTX"
                },
                new Currency
                {
                    Id = 2462,
                    CurrencyTypeId = 2,
                    Name = "AidCoin",
                    Abbreviation = "AID",
                    Slug = "AID"
                },
                new Currency
                {
                    Id = 1556,
                    CurrencyTypeId = 2,
                    Name = "Chronobank",
                    Abbreviation = "TIME",
                    Slug = "TIME"
                },
                new Currency
                {
                    Id = 2497,
                    CurrencyTypeId = 2,
                    Name = "Medicalchain",
                    Abbreviation = "MTN",
                    Slug = "MTN"
                },
                new Currency
                {
                    Id = 3514,
                    CurrencyTypeId = 2,
                    Name = "SUQA",
                    Abbreviation = "SUQA",
                    Slug = "SUQA"
                },
                new Currency
                {
                    Id = 1737,
                    CurrencyTypeId = 2,
                    Name = "XEL",
                    Abbreviation = "XEL",
                    Slug = "XEL"
                },
                new Currency
                {
                    Id = 2688,
                    CurrencyTypeId = 2,
                    Name = "Vipstar Coin",
                    Abbreviation = "VIPS",
                    Slug = "VIPS"
                },
                new Currency
                {
                    Id = 2536,
                    CurrencyTypeId = 2,
                    Name = "Neurotoken",
                    Abbreviation = "NTK",
                    Slug = "NTK"
                },
                new Currency
                {
                    Id = 1382,
                    CurrencyTypeId = 2,
                    Name = "NoLimitCoin",
                    Abbreviation = "NLC2",
                    Slug = "NLC2"
                },
                new Currency
                {
                    Id = 3177,
                    CurrencyTypeId = 2,
                    Name = "Seal Network",
                    Abbreviation = "SEAL",
                    Slug = "SEAL"
                },
                new Currency
                {
                    Id = 3052,
                    CurrencyTypeId = 2,
                    Name = "Eligma Token",
                    Abbreviation = "ELI",
                    Slug = "ELI"
                },
                new Currency
                {
                    Id = 584,
                    CurrencyTypeId = 2,
                    Name = "NativeCoin",
                    Abbreviation = "N8V",
                    Slug = "N8V"
                },
                new Currency
                {
                    Id = 2620,
                    CurrencyTypeId = 2,
                    Name = "Switcheo",
                    Abbreviation = "SWTH",
                    Slug = "SWTH"
                },
                new Currency
                {
                    Id = 3474,
                    CurrencyTypeId = 2,
                    Name = "YGGDRASH",
                    Abbreviation = "YEED",
                    Slug = "YEED"
                },
                new Currency
                {
                    Id = 2312,
                    CurrencyTypeId = 2,
                    Name = "DIMCOIN",
                    Abbreviation = "DIM",
                    Slug = "DIM"
                },
                new Currency
                {
                    Id = 233,
                    CurrencyTypeId = 2,
                    Name = "SolarCoin",
                    Abbreviation = "SLR",
                    Slug = "SLR"
                },
                new Currency
                {
                    Id = 1751,
                    CurrencyTypeId = 2,
                    Name = "ATC Coin",
                    Abbreviation = "ATCC",
                    Slug = "ATCC"
                },
                new Currency
                {
                    Id = 132,
                    CurrencyTypeId = 2,
                    Name = "Counterparty",
                    Abbreviation = "XCP",
                    Slug = "XCP"
                },
                new Currency
                {
                    Id = 2363,
                    CurrencyTypeId = 2,
                    Name = "Zap",
                    Abbreviation = "ZAP",
                    Slug = "ZAP"
                },
                new Currency
                {
                    Id = 3712,
                    CurrencyTypeId = 2,
                    Name = "Cloudbric",
                    Abbreviation = "CLB",
                    Slug = "CLB"
                },
                new Currency
                {
                    Id = 1755,
                    CurrencyTypeId = 2,
                    Name = "Flash",
                    Abbreviation = "FLASH",
                    Slug = "FLASH"
                },
                new Currency
                {
                    Id = 2597,
                    CurrencyTypeId = 2,
                    Name = "UpToken",
                    Abbreviation = "UP",
                    Slug = "UP"
                },
                new Currency
                {
                    Id = 706,
                    CurrencyTypeId = 2,
                    Name = "MonetaryUnit",
                    Abbreviation = "MUE",
                    Slug = "MUE"
                },
                new Currency
                {
                    Id = 2484,
                    CurrencyTypeId = 2,
                    Name = "Hi Mutual Society",
                    Abbreviation = "HMC",
                    Slug = "HMC"
                },
                new Currency
                {
                    Id = 2047,
                    CurrencyTypeId = 2,
                    Name = "Zeusshield",
                    Abbreviation = "ZSC",
                    Slug = "ZSC"
                },
                new Currency
                {
                    Id = 2714,
                    CurrencyTypeId = 2,
                    Name = "Nexty",
                    Abbreviation = "NTY",
                    Slug = "NTY"
                },
                new Currency
                {
                    Id = 3729,
                    CurrencyTypeId = 2,
                    Name = "WOLLO",
                    Abbreviation = "WLO",
                    Slug = "WLO"
                },
                new Currency
                {
                    Id = 2762,
                    CurrencyTypeId = 2,
                    Name = "Open Platform",
                    Abbreviation = "OPEN",
                    Slug = "OPEN"
                },
                new Currency
                {
                    Id = 2184,
                    CurrencyTypeId = 2,
                    Name = "Privatix",
                    Abbreviation = "PRIX",
                    Slug = "PRIX"
                },
                new Currency
                {
                    Id = 3243,
                    CurrencyTypeId = 2,
                    Name = "Moneytoken",
                    Abbreviation = "IMT",
                    Slug = "IMT"
                },
                new Currency
                {
                    Id = 2908,
                    CurrencyTypeId = 2,
                    Name = "HashCoin",
                    Abbreviation = "HSC",
                    Slug = "HSC"
                },
                new Currency
                {
                    Id = 1281,
                    CurrencyTypeId = 2,
                    Name = "ION",
                    Abbreviation = "ION",
                    Slug = "ION"
                },
                new Currency
                {
                    Id = 2078,
                    CurrencyTypeId = 2,
                    Name = "LIFE",
                    Abbreviation = "LIFE",
                    Slug = "LIFE"
                },
                new Currency
                {
                    Id = 3636,
                    CurrencyTypeId = 2,
                    Name = "Lisk Machine Learning",
                    Abbreviation = "LML",
                    Slug = "LML"
                },
                new Currency
                {
                    Id = 2493,
                    CurrencyTypeId = 2,
                    Name = "STK",
                    Abbreviation = "STK",
                    Slug = "STK"
                },
                new Currency
                {
                    Id = 1990,
                    CurrencyTypeId = 2,
                    Name = "BitDice",
                    Abbreviation = "CSNO",
                    Slug = "CSNO"
                },
                new Currency
                {
                    Id = 3432,
                    CurrencyTypeId = 2,
                    Name = "Rapids",
                    Abbreviation = "RPD",
                    Slug = "RPD"
                },
                new Currency
                {
                    Id = 2723,
                    CurrencyTypeId = 2,
                    Name = "FuzeX",
                    Abbreviation = "FXT",
                    Slug = "FXT"
                },
                new Currency
                {
                    Id = 1208,
                    CurrencyTypeId = 2,
                    Name = "RevolutionVR",
                    Abbreviation = "RVR",
                    Slug = "RVR"
                },
                new Currency
                {
                    Id = 2357,
                    CurrencyTypeId = 2,
                    Name = "AI Doctor",
                    Abbreviation = "AIDOC",
                    Slug = "AIDOC"
                },
                new Currency
                {
                    Id = 3314,
                    CurrencyTypeId = 2,
                    Name = "Blockparty (BOXX Token)",
                    Abbreviation = "BOXX",
                    Slug = "BOXX"
                },
                new Currency
                {
                    Id = 2728,
                    CurrencyTypeId = 2,
                    Name = "Winding Tree",
                    Abbreviation = "LIF",
                    Slug = "LIF"
                },
                new Currency
                {
                    Id = 2856,
                    CurrencyTypeId = 2,
                    Name = "CEEK VR",
                    Abbreviation = "CEEK",
                    Slug = "CEEK"
                },
                new Currency
                {
                    Id = 3833,
                    CurrencyTypeId = 2,
                    Name = "HYPNOXYS",
                    Abbreviation = "HYPX",
                    Slug = "HYPX"
                },
                new Currency
                {
                    Id = 2438,
                    CurrencyTypeId = 2,
                    Name = "Acute Angle Cloud",
                    Abbreviation = "AAC",
                    Slug = "AAC"
                },
                new Currency
                {
                    Id = 2176,
                    CurrencyTypeId = 2,
                    Name = "Decision Token",
                    Abbreviation = "HST",
                    Slug = "HST"
                },
                new Currency
                {
                    Id = 2389,
                    CurrencyTypeId = 2,
                    Name = "ugChain",
                    Abbreviation = "UGC",
                    Slug = "UGC"
                },
                new Currency
                {
                    Id = 2587,
                    CurrencyTypeId = 2,
                    Name = "Fluz Fluz",
                    Abbreviation = "FLUZ",
                    Slug = "FLUZ"
                },
                new Currency
                {
                    Id = 2283,
                    CurrencyTypeId = 2,
                    Name = "Datum",
                    Abbreviation = "DAT",
                    Slug = "DAT"
                },
                new Currency
                {
                    Id = 3757,
                    CurrencyTypeId = 2,
                    Name = "GMB",
                    Abbreviation = "GMB",
                    Slug = "GMB"
                },
                new Currency
                {
                    Id = 3520,
                    CurrencyTypeId = 2,
                    Name = "VisionX",
                    Abbreviation = "VNX",
                    Slug = "VNX"
                },
                new Currency
                {
                    Id = 3389,
                    CurrencyTypeId = 2,
                    Name = "Tolar",
                    Abbreviation = "TOL",
                    Slug = "TOL"
                },
                new Currency
                {
                    Id = 3315,
                    CurrencyTypeId = 2,
                    Name = "Playgroundz",
                    Abbreviation = "IOG",
                    Slug = "IOG"
                },
                new Currency
                {
                    Id = 1367,
                    CurrencyTypeId = 2,
                    Name = "Experience Points",
                    Abbreviation = "XP",
                    Slug = "XP"
                },
                new Currency
                {
                    Id = 2880,
                    CurrencyTypeId = 2,
                    Name = "Rate3",
                    Abbreviation = "RTE",
                    Slug = "RTE"
                },
                new Currency
                {
                    Id = 3438,
                    CurrencyTypeId = 2,
                    Name = "Oxycoin",
                    Abbreviation = "OXY",
                    Slug = "OXY"
                },
                new Currency
                {
                    Id = 3022,
                    CurrencyTypeId = 2,
                    Name = "ZMINE",
                    Abbreviation = "ZMN",
                    Slug = "ZMN"
                },
                new Currency
                {
                    Id = 2527,
                    CurrencyTypeId = 2,
                    Name = "SureRemit",
                    Abbreviation = "RMT",
                    Slug = "RMT"
                },
                new Currency
                {
                    Id = 3435,
                    CurrencyTypeId = 2,
                    Name = "Snetwork",
                    Abbreviation = "SNET",
                    Slug = "SNET"
                },
                new Currency
                {
                    Id = 2595,
                    CurrencyTypeId = 2,
                    Name = "NANJCOIN",
                    Abbreviation = "NANJ",
                    Slug = "NANJ"
                },
                new Currency
                {
                    Id = 3711,
                    CurrencyTypeId = 2,
                    Name = "Plair",
                    Abbreviation = "PLA",
                    Slug = "PLA"
                },
                new Currency
                {
                    Id = 2382,
                    CurrencyTypeId = 2,
                    Name = "Spectre.ai Utility Token",
                    Abbreviation = "SXUT",
                    Slug = "SXUT"
                },
                new Currency
                {
                    Id = 2022,
                    CurrencyTypeId = 2,
                    Name = "Internxt",
                    Abbreviation = "INXT",
                    Slug = "INXT"
                },
                new Currency
                {
                    Id = 2579,
                    CurrencyTypeId = 2,
                    Name = "ShipChain",
                    Abbreviation = "SHIP",
                    Slug = "SHIP"
                },
                new Currency
                {
                    Id = 2773,
                    CurrencyTypeId = 2,
                    Name = "GINcoin",
                    Abbreviation = "GIN",
                    Slug = "GIN"
                },
                new Currency
                {
                    Id = 36,
                    CurrencyTypeId = 2,
                    Name = "Novacoin",
                    Abbreviation = "NVC",
                    Slug = "NVC"
                },
                new Currency
                {
                    Id = 2248,
                    CurrencyTypeId = 2,
                    Name = "Cappasity",
                    Abbreviation = "CAPP",
                    Slug = "CAPP"
                },
                new Currency
                {
                    Id = 1587,
                    CurrencyTypeId = 2,
                    Name = "Dynamic",
                    Abbreviation = "DYN",
                    Slug = "DYN"
                },
                new Currency
                {
                    Id = 3816,
                    CurrencyTypeId = 2,
                    Name = "Verasity",
                    Abbreviation = "VRA",
                    Slug = "VRA"
                },
                new Currency
                {
                    Id = 3703,
                    CurrencyTypeId = 2,
                    Name = "ADAMANT Messenger",
                    Abbreviation = "ADM",
                    Slug = "ADM"
                },
                new Currency
                {
                    Id = 2107,
                    CurrencyTypeId = 2,
                    Name = "LUXCoin",
                    Abbreviation = "LUX",
                    Slug = "LUX"
                },
                new Currency
                {
                    Id = 2510,
                    CurrencyTypeId = 2,
                    Name = "Datawallet",
                    Abbreviation = "DXT",
                    Slug = "DXT"
                },
                new Currency
                {
                    Id = 2964,
                    CurrencyTypeId = 2,
                    Name = "ValueCyberToken",
                    Abbreviation = "VCT",
                    Slug = "VCT"
                },
                new Currency
                {
                    Id = 1070,
                    CurrencyTypeId = 2,
                    Name = "Expanse",
                    Abbreviation = "EXP",
                    Slug = "EXP"
                },
                new Currency
                {
                    Id = 3058,
                    CurrencyTypeId = 2,
                    Name = "eSDChain",
                    Abbreviation = "SDA",
                    Slug = "SDA"
                },
                new Currency
                {
                    Id = 3735,
                    CurrencyTypeId = 2,
                    Name = "VegaWallet Token",
                    Abbreviation = "VGW",
                    Slug = "VGW"
                },
                new Currency
                {
                    Id = 3845,
                    CurrencyTypeId = 2,
                    Name = "V-ID",
                    Abbreviation = "VIDT",
                    Slug = "VIDT"
                },
                new Currency
                {
                    Id = 1577,
                    CurrencyTypeId = 2,
                    Name = "Musicoin",
                    Abbreviation = "MUSIC",
                    Slug = "MUSIC"
                },
                new Currency
                {
                    Id = 3884,
                    CurrencyTypeId = 2,
                    Name = "Function X",
                    Abbreviation = "FX",
                    Slug = "FX"
                },
                new Currency
                {
                    Id = 2466,
                    CurrencyTypeId = 2,
                    Name = "aXpire",
                    Abbreviation = "AXPR",
                    Slug = "AXPR"
                },
                new Currency
                {
                    Id = 2868,
                    CurrencyTypeId = 2,
                    Name = "Constellation",
                    Abbreviation = "DAG",
                    Slug = "DAG"
                },
                new Currency
                {
                    Id = 2634,
                    CurrencyTypeId = 2,
                    Name = "XinFin Network",
                    Abbreviation = "XDCE",
                    Slug = "XDCE"
                },
                new Currency
                {
                    Id = 1669,
                    CurrencyTypeId = 2,
                    Name = "Humaniq",
                    Abbreviation = "HMQ",
                    Slug = "HMQ"
                },
                new Currency
                {
                    Id = 2764,
                    CurrencyTypeId = 2,
                    Name = "Silent Notary",
                    Abbreviation = "SNTR",
                    Slug = "SNTR"
                },
                new Currency
                {
                    Id = 2891,
                    CurrencyTypeId = 2,
                    Name = "Cardstack",
                    Abbreviation = "CARD",
                    Slug = "CARD"
                },
                new Currency
                {
                    Id = 2718,
                    CurrencyTypeId = 2,
                    Name = "PAL Network",
                    Abbreviation = "PAL",
                    Slug = "PAL"
                },
                new Currency
                {
                    Id = 2869,
                    CurrencyTypeId = 2,
                    Name = "Merculet",
                    Abbreviation = "MVP",
                    Slug = "MVP"
                },
                new Currency
                {
                    Id = 3388,
                    CurrencyTypeId = 2,
                    Name = "FREE Coin",
                    Abbreviation = "FREE",
                    Slug = "FREE"
                }, new Currency
                {
                    Id = 2443,
                    CurrencyTypeId = 2,
                    Name = "Trinity Network Credit",
                    Abbreviation = "TNC",
                    Slug = "TNC"
                }, new Currency
                {
                    Id = 3016,
                    CurrencyTypeId = 2,
                    Name = "NeuroChain",
                    Abbreviation = "NCC",
                    Slug = "NCC"
                }, new Currency
                {
                    Id = 2692,
                    CurrencyTypeId = 2,
                    Name = "Nebula AI",
                    Abbreviation = "NBAI",
                    Slug = "NBAI"
                }, new Currency
                {
                    Id = 3466,
                    CurrencyTypeId = 2,
                    Name = "Insureum",
                    Abbreviation = "ISR",
                    Slug = "ISR"
                }, new Currency
                {
                    Id = 3352,
                    CurrencyTypeId = 2,
                    Name = "MidasProtocol",
                    Abbreviation = "MAS",
                    Slug = "MAS"
                }, new Currency
                {
                    Id = 2626,
                    CurrencyTypeId = 2,
                    Name = "Friendz",
                    Abbreviation = "FDZ",
                    Slug = "FDZ"
                }, new Currency
                {
                    Id = 2390,
                    CurrencyTypeId = 2,
                    Name = "BANKEX",
                    Abbreviation = "BKX",
                    Slug = "BKX"
                }, new Currency
                {
                    Id = 3280,
                    CurrencyTypeId = 2,
                    Name = "RealTract",
                    Abbreviation = "RET",
                    Slug = "RET"
                }, new Currency
                {
                    Id = 870,
                    CurrencyTypeId = 2,
                    Name = "Pura",
                    Abbreviation = "PURA",
                    Slug = "PURA"
                }, new Currency
                {
                    Id = 2040,
                    CurrencyTypeId = 2,
                    Name = "ALIS",
                    Abbreviation = "ALIS",
                    Slug = "ALIS"
                }, new Currency
                {
                    Id = 2439,
                    CurrencyTypeId = 2,
                    Name = "SelfSell",
                    Abbreviation = "SSC",
                    Slug = "SSC"
                }, new Currency
                {
                    Id = 3499,
                    CurrencyTypeId = 2,
                    Name = "Liquidity Network",
                    Abbreviation = "LQD",
                    Slug = "LQD"
                }, new Currency
                {
                    Id = 3750,
                    CurrencyTypeId = 2,
                    Name = "eXPerience Chain",
                    Abbreviation = "XPC",
                    Slug = "XPC"
                }, new Currency
                {
                    Id = 2391,
                    CurrencyTypeId = 2,
                    Name = "EchoLink",
                    Abbreviation = "EKO",
                    Slug = "EKO"
                }, new Currency
                {
                    Id = 2575,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Private",
                    Abbreviation = "BTCP",
                    Slug = "BTCP"
                }, new Currency
                {
                    Id = 2314,
                    CurrencyTypeId = 2,
                    Name = "Cryptopay",
                    Abbreviation = "CPAY",
                    Slug = "CPAY"
                }, new Currency
                {
                    Id = 3140,
                    CurrencyTypeId = 2,
                    Name = "Ubex",
                    Abbreviation = "UBEX",
                    Slug = "UBEX"
                }, new Currency
                {
                    Id = 1638,
                    CurrencyTypeId = 2,
                    Name = "WeTrust",
                    Abbreviation = "TRST",
                    Slug = "TRST"
                }, new Currency
                {
                    Id = 2088,
                    CurrencyTypeId = 2,
                    Name = "EXRNchain",
                    Abbreviation = "EXRN",
                    Slug = "EXRN"
                }, new Currency
                {
                    Id = 2702,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Interest",
                    Abbreviation = "BCI",
                    Slug = "BCI"
                }, new Currency
                {
                    Id = 366,
                    CurrencyTypeId = 2,
                    Name = "BitSend",
                    Abbreviation = "BSD",
                    Slug = "BSD"
                }, new Currency
                {
                    Id = 3055,
                    CurrencyTypeId = 2,
                    Name = "EBCoin",
                    Abbreviation = "EBC",
                    Slug = "EBC"
                }, new Currency
                {
                    Id = 2191,
                    CurrencyTypeId = 2,
                    Name = "Paypex",
                    Abbreviation = "PAYX",
                    Slug = "PAYX"
                }, new Currency
                {
                    Id = 3616,
                    CurrencyTypeId = 2,
                    Name = "Blockchain Certified Data Token",
                    Abbreviation = "BCDT",
                    Slug = "BCDT"
                }, new Currency
                {
                    Id = 3210,
                    CurrencyTypeId = 2,
                    Name = "MIB Coin",
                    Abbreviation = "MIB",
                    Slug = "MIB"
                }, new Currency
                {
                    Id = 2343,
                    CurrencyTypeId = 2,
                    Name = "CanYaCoin",
                    Abbreviation = "CAN",
                    Slug = "CAN"
                }, new Currency
                {
                    Id = 3870,
                    CurrencyTypeId = 2,
                    Name = "Lition",
                    Abbreviation = "LIT",
                    Slug = "LIT"
                }, new Currency
                {
                    Id = 2573,
                    CurrencyTypeId = 2,
                    Name = "Electrify.Asia",
                    Abbreviation = "ELEC",
                    Slug = "ELEC"
                }, new Currency
                {
                    Id = 3762,
                    CurrencyTypeId = 2,
                    Name = "1SG",
                    Abbreviation = "1SG",
                    Slug = "1SG"
                }, new Currency
                {
                    Id = 3404,
                    CurrencyTypeId = 2,
                    Name = "Wixlar",
                    Abbreviation = "WIX",
                    Slug = "WIX"
                }, new Currency
                {
                    Id = 1616,
                    CurrencyTypeId = 2,
                    Name = "Matchpool",
                    Abbreviation = "GUP",
                    Slug = "GUP"
                }, new Currency
                {
                    Id = 2558,
                    CurrencyTypeId = 2,
                    Name = "Insights Network",
                    Abbreviation = "INSTAR",
                    Slug = "INSTAR"
                }, new Currency
                {
                    Id = 1125,
                    CurrencyTypeId = 2,
                    Name = "HyperSpace",
                    Abbreviation = "AMP",
                    Slug = "AMP"
                }, new Currency
                {
                    Id = 184,
                    CurrencyTypeId = 2,
                    Name = "DNotes",
                    Abbreviation = "NOTE",
                    Slug = "NOTE"
                }, new Currency
                {
                    Id = 1838,
                    CurrencyTypeId = 2,
                    Name = "OracleChain",
                    Abbreviation = "OCT",
                    Slug = "OCT"
                }, new Currency
                {
                    Id = 2178,
                    CurrencyTypeId = 2,
                    Name = "Upfiring",
                    Abbreviation = "UFR",
                    Slug = "UFR"
                }, new Currency
                {
                    Id = 3419,
                    CurrencyTypeId = 2,
                    Name = "Quasarcoin",
                    Abbreviation = "QAC",
                    Slug = "QAC"
                }, new Currency
                {
                    Id = 2855,
                    CurrencyTypeId = 2,
                    Name = "CashBet Coin",
                    Abbreviation = "CBC",
                    Slug = "CBC"
                }, new Currency
                {
                    Id = 3357,
                    CurrencyTypeId = 2,
                    Name = "Digital Asset Guarantee Token",
                    Abbreviation = "DAGT",
                    Slug = "DAGT"
                }, new Currency
                {
                    Id = 3638,
                    CurrencyTypeId = 2,
                    Name = "Skychain",
                    Abbreviation = "SKCH",
                    Slug = "SKCH"
                }, new Currency
                {
                    Id = 2699,
                    CurrencyTypeId = 2,
                    Name = "Sharder",
                    Abbreviation = "SS",
                    Slug = "SS"
                }, new Currency
                {
                    Id = 2459,
                    CurrencyTypeId = 2,
                    Name = "indaHash",
                    Abbreviation = "IDH",
                    Slug = "IDH"
                }, new Currency
                {
                    Id = 2657,
                    CurrencyTypeId = 2,
                    Name = "BrahmaOS",
                    Abbreviation = "BRM",
                    Slug = "BRM"
                }, new Currency
                {
                    Id = 1032,
                    CurrencyTypeId = 2,
                    Name = "TransferCoin",
                    Abbreviation = "TX",
                    Slug = "TX"
                }, new Currency
                {
                    Id = 2541,
                    CurrencyTypeId = 2,
                    Name = "Storiqa",
                    Abbreviation = "STQ",
                    Slug = "STQ"
                }, new Currency
                {
                    Id = 3651,
                    CurrencyTypeId = 2,
                    Name = "Next.exchange",
                    Abbreviation = "NEXT",
                    Slug = "NEXT"
                }, new Currency
                {
                    Id = 3658,
                    CurrencyTypeId = 2,
                    Name = "Fountain",
                    Abbreviation = "FTN",
                    Slug = "FTN"
                }, new Currency
                {
                    Id = 3917,
                    CurrencyTypeId = 2,
                    Name = "Sentivate",
                    Abbreviation = "SNTVT",
                    Slug = "SNTVT"
                }, new Currency
                {
                    Id = 2242,
                    CurrencyTypeId = 2,
                    Name = "Qbao",
                    Abbreviation = "QBT",
                    Slug = "QBT"
                }, new Currency
                {
                    Id = 2927,
                    CurrencyTypeId = 2,
                    Name = "sUSD",
                    Abbreviation = "SUSD",
                    Slug = "SUSD"
                }, new Currency
                {
                    Id = 2275,
                    CurrencyTypeId = 2,
                    Name = "ProChain",
                    Abbreviation = "PRA",
                    Slug = "PRA"
                }, new Currency
                {
                    Id = 2562,
                    CurrencyTypeId = 2,
                    Name = "Education Ecosystem",
                    Abbreviation = "LEDU",
                    Slug = "LEDU"
                }, new Currency
                {
                    Id = 3775,
                    CurrencyTypeId = 2,
                    Name = "win.win",
                    Abbreviation = "TWINS",
                    Slug = "TWINS"
                }, new Currency
                {
                    Id = 1063,
                    CurrencyTypeId = 2,
                    Name = "BitCrystals",
                    Abbreviation = "BCY",
                    Slug = "BCY"
                }, new Currency
                {
                    Id = 2136,
                    CurrencyTypeId = 2,
                    Name = "ATLANT",
                    Abbreviation = "ATL",
                    Slug = "ATL"
                }, new Currency
                {
                    Id = 823,
                    CurrencyTypeId = 2,
                    Name = "GeoCoin",
                    Abbreviation = "GEO",
                    Slug = "GEO"
                }, new Currency
                {
                    Id = 3691,
                    CurrencyTypeId = 2,
                    Name = "Kuai Token",
                    Abbreviation = "KT",
                    Slug = "KT"
                }, new Currency
                {
                    Id = 3120,
                    CurrencyTypeId = 2,
                    Name = "OWNDATA",
                    Abbreviation = "OWN",
                    Slug = "OWN"
                }, new Currency
                {
                    Id = 2537,
                    CurrencyTypeId = 2,
                    Name = "Gems ",
                    Abbreviation = "GEM",
                    Slug = "GEM"
                }, new Currency
                {
                    Id = 2105,
                    CurrencyTypeId = 2,
                    Name = "Pirl",
                    Abbreviation = "PIRL",
                    Slug = "PIRL"
                }, new Currency
                {
                    Id = 1999,
                    CurrencyTypeId = 2,
                    Name = "Kolion",
                    Abbreviation = "KLN",
                    Slug = "KLN"
                }, new Currency
                {
                    Id = 2512,
                    CurrencyTypeId = 2,
                    Name = "UNIVERSAL CASH",
                    Abbreviation = "UCASH",
                    Slug = "UCASH"
                }, new Currency
                {
                    Id = 2629,
                    CurrencyTypeId = 2,
                    Name = "Torque",
                    Abbreviation = "XTC",
                    Slug = "XTC"
                }, new Currency
                {
                    Id = 1294,
                    CurrencyTypeId = 2,
                    Name = "Rise",
                    Abbreviation = "RISE",
                    Slug = "RISE"
                }, new Currency
                {
                    Id = 3168,
                    CurrencyTypeId = 2,
                    Name = "FarmaTrust",
                    Abbreviation = "FTT",
                    Slug = "FTT"
                }, new Currency
                {
                    Id = 3096,
                    CurrencyTypeId = 2,
                    Name = "Pundi X NEM",
                    Abbreviation = "NPXSXEM",
                    Slug = "NPXSXEM"
                }, new Currency
                {
                    Id = 1861,
                    CurrencyTypeId = 2,
                    Name = "Stox",
                    Abbreviation = "STX",
                    Slug = "STX"
                }, new Currency
                {
                    Id = 2325,
                    CurrencyTypeId = 2,
                    Name = "Matryx",
                    Abbreviation = "MTX",
                    Slug = "MTX"
                }, new Currency
                {
                    Id = 1739,
                    CurrencyTypeId = 2,
                    Name = "Miners' Reward Token",
                    Abbreviation = "MRT",
                    Slug = "MRT"
                }, new Currency
                {
                    Id = 3821,
                    CurrencyTypeId = 2,
                    Name = "Qredit",
                    Abbreviation = "XQR",
                    Slug = "XQR"
                }, new Currency
                {
                    Id = 2231,
                    CurrencyTypeId = 2,
                    Name = "Flixxo",
                    Abbreviation = "FLIXX",
                    Slug = "FLIXX"
                }, new Currency
                {
                    Id = 3334,
                    CurrencyTypeId = 2,
                    Name = "X-CASH",
                    Abbreviation = "XCASH",
                    Slug = "XCASH"
                }, new Currency
                {
                    Id = 1082,
                    CurrencyTypeId = 2,
                    Name = "SIBCoin",
                    Abbreviation = "SIB",
                    Slug = "SIB"
                }, new Currency
                {
                    Id = 3679,
                    CurrencyTypeId = 2,
                    Name = "SnapCoin",
                    Abbreviation = "SNPC",
                    Slug = "SNPC"
                }, new Currency
                {
                    Id = 2310,
                    CurrencyTypeId = 2,
                    Name = "Bounty0x",
                    Abbreviation = "BNTY",
                    Slug = "BNTY"
                }, new Currency
                {
                    Id = 2360,
                    CurrencyTypeId = 2,
                    Name = "Hacken",
                    Abbreviation = "HKN",
                    Slug = "HKN"
                }, new Currency
                {
                    Id = 2215,
                    CurrencyTypeId = 2,
                    Name = "Energo",
                    Abbreviation = "TSL",
                    Slug = "TSL"
                }, new Currency
                {
                    Id = 1562,
                    CurrencyTypeId = 2,
                    Name = "Swarm City",
                    Abbreviation = "SWT",
                    Slug = "SWT"
                }, new Currency
                {
                    Id = 1371,
                    CurrencyTypeId = 2,
                    Name = "B3Coin",
                    Abbreviation = "KB3",
                    Slug = "KB3"
                }, new Currency
                {
                    Id = 83,
                    CurrencyTypeId = 2,
                    Name = "Omni",
                    Abbreviation = "OMNI",
                    Slug = "OMNI"
                }, new Currency
                {
                    Id = 3854,
                    CurrencyTypeId = 2,
                    Name = "Unification",
                    Abbreviation = "UND",
                    Slug = "UND"
                }, new Currency
                {
                    Id = 3010,
                    CurrencyTypeId = 2,
                    Name = "Coinsuper Ecosystem Network",
                    Abbreviation = "CEN",
                    Slug = "CEN"
                }, new Currency
                {
                    Id = 2525,
                    CurrencyTypeId = 2,
                    Name = "Alphacat",
                    Abbreviation = "ACAT",
                    Slug = "ACAT"
                }, new Currency
                {
                    Id = 3738,
                    CurrencyTypeId = 2,
                    Name = "Decentralized Crypto Token",
                    Abbreviation = "DCTO",
                    Slug = "DCTO"
                }, new Currency
                {
                    Id = 2949,
                    CurrencyTypeId = 2,
                    Name = "Kryll",
                    Abbreviation = "KRL",
                    Slug = "KRL"
                }, new Currency
                {
                    Id = 1708,
                    CurrencyTypeId = 2,
                    Name = "Patientory",
                    Abbreviation = "PTOY",
                    Slug = "PTOY"
                }, new Currency
                {
                    Id = 3506,
                    CurrencyTypeId = 2,
                    Name = "IONChain",
                    Abbreviation = "IONC",
                    Slug = "IONC"
                }, new Currency
                {
                    Id = 2921,
                    CurrencyTypeId = 2,
                    Name = "OneLedger",
                    Abbreviation = "OLT",
                    Slug = "OLT"
                }, new Currency
                {
                    Id = 3195,
                    CurrencyTypeId = 2,
                    Name = "Credit Tag Chain",
                    Abbreviation = "CTC",
                    Slug = "CTC"
                }, new Currency
                {
                    Id = 2478,
                    CurrencyTypeId = 2,
                    Name = "CoinFi",
                    Abbreviation = "COFI",
                    Slug = "COFI"
                }, new Currency
                {
                    Id = 2500,
                    CurrencyTypeId = 2,
                    Name = "Zilla",
                    Abbreviation = "ZLA",
                    Slug = "ZLA"
                }, new Currency
                {
                    Id = 1464,
                    CurrencyTypeId = 2,
                    Name = "Internet of People",
                    Abbreviation = "IOP",
                    Slug = "IOP"
                }, new Currency
                {
                    Id = 2567,
                    CurrencyTypeId = 2,
                    Name = "DATx",
                    Abbreviation = "DATX",
                    Slug = "DATX"
                }, new Currency
                {
                    Id = 3104,
                    CurrencyTypeId = 2,
                    Name = "Giant",
                    Abbreviation = "GIC",
                    Slug = "GIC"
                }, new Currency
                {
                    Id = 2564,
                    CurrencyTypeId = 2,
                    Name = "HOQU",
                    Abbreviation = "HQX",
                    Slug = "HQX"
                }, new Currency
                {
                    Id = 2110,
                    CurrencyTypeId = 2,
                    Name = "Dovu",
                    Abbreviation = "DOV",
                    Slug = "DOV"
                }, new Currency
                {
                    Id = 1948,
                    CurrencyTypeId = 2,
                    Name = "Aventus",
                    Abbreviation = "AVT",
                    Slug = "AVT"
                }, new Currency
                {
                    Id = 2979,
                    CurrencyTypeId = 2,
                    Name = "Linfinity",
                    Abbreviation = "LFC",
                    Slug = "LFC"
                }, new Currency
                {
                    Id = 2076,
                    CurrencyTypeId = 2,
                    Name = "Blue Protocol",
                    Abbreviation = "BLUE",
                    Slug = "BLUE"
                }, new Currency
                {
                    Id = 2549,
                    CurrencyTypeId = 2,
                    Name = "Ink Protocol",
                    Abbreviation = "XNK",
                    Slug = "XNK"
                }, new Currency
                {
                    Id = 2970,
                    CurrencyTypeId = 2,
                    Name = "LocalCoinSwap",
                    Abbreviation = "LCS",
                    Slug = "LCS"
                }, new Currency
                {
                    Id = 2771,
                    CurrencyTypeId = 2,
                    Name = "RED",
                    Abbreviation = "RED",
                    Slug = "RED"
                }, new Currency
                {
                    Id = 2844,
                    CurrencyTypeId = 2,
                    Name = "Shivom",
                    Abbreviation = "OMX",
                    Slug = "OMX"
                }, new Currency
                {
                    Id = 2592,
                    CurrencyTypeId = 2,
                    Name = "Banca",
                    Abbreviation = "BANCA",
                    Slug = "BANCA"
                }, new Currency
                {
                    Id = 2850,
                    CurrencyTypeId = 2,
                    Name = "TRAXIA",
                    Abbreviation = "TMT",
                    Slug = "TMT"
                }, new Currency
                {
                    Id = 1380,
                    CurrencyTypeId = 2,
                    Name = "LoMoCoin",
                    Abbreviation = "LMC",
                    Slug = "LMC"
                }, new Currency
                {
                    Id = 2490,
                    CurrencyTypeId = 2,
                    Name = "CargoX",
                    Abbreviation = "CXO",
                    Slug = "CXO"
                }, new Currency
                {
                    Id = 3079,
                    CurrencyTypeId = 2,
                    Name = "X8X Token",
                    Abbreviation = "X8X",
                    Slug = "X8X"
                }, new Currency
                {
                    Id = 3639,
                    CurrencyTypeId = 2,
                    Name = "PlayGame",
                    Abbreviation = "PXG",
                    Slug = "PXG"
                }, new Currency
                {
                    Id = 416,
                    CurrencyTypeId = 2,
                    Name = "HempCoin",
                    Abbreviation = "THC",
                    Slug = "THC"
                }, new Currency
                {
                    Id = 3859,
                    CurrencyTypeId = 2,
                    Name = "Paytomat",
                    Abbreviation = "PTI",
                    Slug = "PTI"
                }, new Currency
                {
                    Id = 2742,
                    CurrencyTypeId = 2,
                    Name = "Sakura Bloom",
                    Abbreviation = "SKB",
                    Slug = "SKB"
                }, new Currency
                {
                    Id = 217,
                    CurrencyTypeId = 2,
                    Name = "Bela",
                    Abbreviation = "BELA",
                    Slug = "BELA"
                }, new Currency
                {
                    Id = 2893,
                    CurrencyTypeId = 2,
                    Name = "On.Live",
                    Abbreviation = "ONL",
                    Slug = "ONL"
                }, new Currency
                {
                    Id = 2426,
                    CurrencyTypeId = 2,
                    Name = "ShareX",
                    Abbreviation = "SEXC",
                    Slug = "SEXC"
                }, new Currency
                {
                    Id = 3194,
                    CurrencyTypeId = 2,
                    Name = "DPRating",
                    Abbreviation = "RATING",
                    Slug = "RATING"
                }, new Currency
                {
                    Id = 1392,
                    CurrencyTypeId = 2,
                    Name = "Pluton",
                    Abbreviation = "PLU",
                    Slug = "PLU"
                }, new Currency
                {
                    Id = 2624,
                    CurrencyTypeId = 2,
                    Name = "Sentinel Chain",
                    Abbreviation = "SENC",
                    Slug = "SENC"
                }, new Currency
                {
                    Id = 2479,
                    CurrencyTypeId = 2,
                    Name = "Equal",
                    Abbreviation = "EQL",
                    Slug = "EQL"
                }, new Currency
                {
                    Id = 3625,
                    CurrencyTypeId = 2,
                    Name = "QuadrantProtocol",
                    Abbreviation = "EQUAD",
                    Slug = "EQUAD"
                }, new Currency
                {
                    Id = 2837,
                    CurrencyTypeId = 2,
                    Name = "0xBitcoin",
                    Abbreviation = "0xBTC",
                    Slug = "0xBTC"
                }, new Currency
                {
                    Id = 2906,
                    CurrencyTypeId = 2,
                    Name = "Essentia",
                    Abbreviation = "ESS",
                    Slug = "ESS"
                }, new Currency
                {
                    Id = 3215,
                    CurrencyTypeId = 2,
                    Name = "Gentarium",
                    Abbreviation = "GTM",
                    Slug = "GTM"
                }, new Currency
                {
                    Id = 374,
                    CurrencyTypeId = 2,
                    Name = "ArtByte",
                    Abbreviation = "ABY",
                    Slug = "ABY"
                }, new Currency
                {
                    Id = 2865,
                    CurrencyTypeId = 2,
                    Name = "Trittium",
                    Abbreviation = "TRTT",
                    Slug = "TRTT"
                }, new Currency
                {
                    Id = 3786,
                    CurrencyTypeId = 2,
                    Name = "Lunes",
                    Abbreviation = "LUNES",
                    Slug = "LUNES"
                }, new Currency
                {
                    Id = 1628,
                    CurrencyTypeId = 2,
                    Name = "Happycoin",
                    Abbreviation = "HPC",
                    Slug = "HPC"
                }, new Currency
                {
                    Id = 3669,
                    CurrencyTypeId = 2,
                    Name = "Winco",
                    Abbreviation = "WCO",
                    Slug = "WCO"
                }, new Currency
                {
                    Id = 2139,
                    CurrencyTypeId = 2,
                    Name = "MinexCoin",
                    Abbreviation = "MNX",
                    Slug = "MNX"
                }, new Currency
                {
                    Id = 606,
                    CurrencyTypeId = 2,
                    Name = "FoldingCoin",
                    Abbreviation = "FLDC",
                    Slug = "FLDC"
                }, new Currency
                {
                    Id = 2151,
                    CurrencyTypeId = 2,
                    Name = "Autonio",
                    Abbreviation = "NIO",
                    Slug = "NIO"
                }, new Currency
                {
                    Id = 1606,
                    CurrencyTypeId = 2,
                    Name = "Solaris",
                    Abbreviation = "XLR",
                    Slug = "XLR"
                }, new Currency
                {
                    Id = 626,
                    CurrencyTypeId = 2,
                    Name = "NuBits",
                    Abbreviation = "USNBT",
                    Slug = "USNBT"
                }, new Currency
                {
                    Id = 2144,
                    CurrencyTypeId = 2,
                    Name = "SHIELD",
                    Abbreviation = "XSH",
                    Slug = "XSH"
                }, new Currency
                {
                    Id = 87,
                    CurrencyTypeId = 2,
                    Name = "FedoraCoin",
                    Abbreviation = "TIPS",
                    Slug = "TIPS"
                }, new Currency
                {
                    Id = 3095,
                    CurrencyTypeId = 2,
                    Name = "Niobium Coin",
                    Abbreviation = "NBC",
                    Slug = "NBC"
                }, new Currency
                {
                    Id = 2551,
                    CurrencyTypeId = 2,
                    Name = "Bezop",
                    Abbreviation = "BEZ",
                    Slug = "BEZ"
                }, new Currency
                {
                    Id = 1106,
                    CurrencyTypeId = 2,
                    Name = "StrongHands",
                    Abbreviation = "SHND",
                    Slug = "SHND"
                }, new Currency
                {
                    Id = 3760,
                    CurrencyTypeId = 2,
                    Name = "Scanetchain",
                    Abbreviation = "SWC",
                    Slug = "SWC"
                }, new Currency
                {
                    Id = 2041,
                    CurrencyTypeId = 2,
                    Name = "BitcoinZ",
                    Abbreviation = "BTCZ",
                    Slug = "BTCZ"
                }, new Currency
                {
                    Id = 2516,
                    CurrencyTypeId = 2,
                    Name = "MktCoin",
                    Abbreviation = "MLM",
                    Slug = "MLM"
                }, new Currency
                {
                    Id = 3779,
                    CurrencyTypeId = 2,
                    Name = "CoTrader",
                    Abbreviation = "COT",
                    Slug = "COT"
                }, new Currency
                {
                    Id = 3809,
                    CurrencyTypeId = 2,
                    Name = "DOS Network",
                    Abbreviation = "DOS",
                    Slug = "DOS"
                }, new Currency
                {
                    Id = 2936,
                    CurrencyTypeId = 2,
                    Name = "MTC Mesh Network",
                    Abbreviation = "MTC",
                    Slug = "MTC"
                }, new Currency
                {
                    Id = 2929,
                    CurrencyTypeId = 2,
                    Name = "Truegame",
                    Abbreviation = "TGAME",
                    Slug = "TGAME"
                }, new Currency
                {
                    Id = 2501,
                    CurrencyTypeId = 2,
                    Name = "adbank",
                    Abbreviation = "ADB",
                    Slug = "ADB"
                }, new Currency
                {
                    Id = 2249,
                    CurrencyTypeId = 2,
                    Name = "Eroscoin",
                    Abbreviation = "ERO",
                    Slug = "ERO"
                }, new Currency
                {
                    Id = 2407,
                    CurrencyTypeId = 2,
                    Name = "AICHAIN",
                    Abbreviation = "AIT",
                    Slug = "AIT"
                }, new Currency
                {
                    Id = 3340,
                    CurrencyTypeId = 2,
                    Name = "Mallcoin",
                    Abbreviation = "MLC",
                    Slug = "MLC"
                }, new Currency
                {
                    Id = 2279,
                    CurrencyTypeId = 2,
                    Name = "Playkey",
                    Abbreviation = "PKT",
                    Slug = "PKT"
                }, new Currency
                {
                    Id = 2731,
                    CurrencyTypeId = 2,
                    Name = "Utrum",
                    Abbreviation = "OOT",
                    Slug = "OOT"
                }, new Currency
                {
                    Id = 2707,
                    CurrencyTypeId = 2,
                    Name = "FLIP",
                    Abbreviation = "FLP",
                    Slug = "FLP"
                }, new Currency
                {
                    Id = 2898,
                    CurrencyTypeId = 2,
                    Name = "GoNetwork",
                    Abbreviation = "GOT",
                    Slug = "GOT"
                }, new Currency
                {
                    Id = 2006,
                    CurrencyTypeId = 2,
                    Name = "Cobinhood",
                    Abbreviation = "COB",
                    Slug = "COB"
                }, new Currency
                {
                    Id = 2678,
                    CurrencyTypeId = 2,
                    Name = "TraDove B2BCoin",
                    Abbreviation = "BBC",
                    Slug = "BBC"
                }, new Currency
                {
                    Id = 2674,
                    CurrencyTypeId = 2,
                    Name = "Masari",
                    Abbreviation = "MSR",
                    Slug = "MSR"
                }, new Currency
                {
                    Id = 3234,
                    CurrencyTypeId = 2,
                    Name = "Xriba",
                    Abbreviation = "XRA",
                    Slug = "XRA"
                }, new Currency
                {
                    Id = 2708,
                    CurrencyTypeId = 2,
                    Name = "Crowd Machine",
                    Abbreviation = "CMCT",
                    Slug = "CMCT"
                }, new Currency
                {
                    Id = 2240,
                    CurrencyTypeId = 2,
                    Name = "SoMee.Social",
                    Abbreviation = "ONG",
                    Slug = "ONG"
                }, new Currency
                {
                    Id = 3894,
                    CurrencyTypeId = 2,
                    Name = "Crypto Sports",
                    Abbreviation = "CSPN",
                    Slug = "CSPN"
                }, new Currency
                {
                    Id = 2185,
                    CurrencyTypeId = 2,
                    Name = "Lethean",
                    Abbreviation = "LTHN",
                    Slug = "LTHN"
                }, new Currency
                {
                    Id = 2594,
                    CurrencyTypeId = 2,
                    Name = "LatiumX",
                    Abbreviation = "LATX",
                    Slug = "LATX"
                }, new Currency
                {
                    Id = 1399,
                    CurrencyTypeId = 2,
                    Name = "Sequence",
                    Abbreviation = "SEQ",
                    Slug = "SEQ"
                }, new Currency
                {
                    Id = 2421,
                    CurrencyTypeId = 2,
                    Name = "VouchForMe",
                    Abbreviation = "IPL",
                    Slug = "IPL"
                }, new Currency
                {
                    Id = 1991,
                    CurrencyTypeId = 2,
                    Name = "Rivetz",
                    Abbreviation = "RVT",
                    Slug = "RVT"
                }, new Currency
                {
                    Id = 1769,
                    CurrencyTypeId = 2,
                    Name = "Denarius",
                    Abbreviation = "D",
                    Slug = "D"
                }, new Currency
                {
                    Id = 3113,
                    CurrencyTypeId = 2,
                    Name = "InterCrone",
                    Abbreviation = "ICR",
                    Slug = "ICR"
                }, new Currency
                {
                    Id = 1191,
                    CurrencyTypeId = 2,
                    Name = "Memetic / PepeCoin",
                    Abbreviation = "MEME",
                    Slug = "MEME"
                }, new Currency
                {
                    Id = 293,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Plus",
                    Abbreviation = "XBC",
                    Slug = "XBC"
                }, new Currency
                {
                    Id = 2508,
                    CurrencyTypeId = 2,
                    Name = "DCORP Utility",
                    Abbreviation = "DRPU",
                    Slug = "DRPU"
                }, new Currency
                {
                    Id = 3141,
                    CurrencyTypeId = 2,
                    Name = "Blockpass",
                    Abbreviation = "PASS",
                    Slug = "PASS"
                }, new Currency
                {
                    Id = 3080,
                    CurrencyTypeId = 2,
                    Name = "Commercium",
                    Abbreviation = "CMM",
                    Slug = "CMM"
                }, new Currency
                {
                    Id = 2528,
                    CurrencyTypeId = 2,
                    Name = "Dether",
                    Abbreviation = "DTH",
                    Slug = "DTH"
                }, new Currency
                {
                    Id = 2912,
                    CurrencyTypeId = 2,
                    Name = "SnowGem",
                    Abbreviation = "XSG",
                    Slug = "XSG"
                }, new Currency
                {
                    Id = 2236,
                    CurrencyTypeId = 2,
                    Name = "MyWish",
                    Abbreviation = "WISH",
                    Slug = "WISH"
                }, new Currency
                {
                    Id = 2775,
                    CurrencyTypeId = 2,
                    Name = "Faceter",
                    Abbreviation = "FACE",
                    Slug = "FACE"
                }, new Currency
                {
                    Id = 633,
                    CurrencyTypeId = 2,
                    Name = "ExclusiveCoin",
                    Abbreviation = "EXCL",
                    Slug = "EXCL"
                }, new Currency
                {
                    Id = 2649,
                    CurrencyTypeId = 2,
                    Name = "DeviantCoin",
                    Abbreviation = "DEV",
                    Slug = "DEV"
                }, new Currency
                {
                    Id = 3323,
                    CurrencyTypeId = 2,
                    Name = "PAYCENT",
                    Abbreviation = "PYN",
                    Slug = "PYN"
                }, new Currency
                {
                    Id = 313,
                    CurrencyTypeId = 2,
                    Name = "PinkCoin",
                    Abbreviation = "PINK",
                    Slug = "PINK"
                }, new Currency
                {
                    Id = 2418,
                    CurrencyTypeId = 2,
                    Name = "Maverick Chain",
                    Abbreviation = "MVC",
                    Slug = "MVC"
                }, new Currency
                {
                    Id = 788,
                    CurrencyTypeId = 2,
                    Name = "Circuits of Value",
                    Abbreviation = "COVAL",
                    Slug = "COVAL"
                }, new Currency
                {
                    Id = 3360,
                    CurrencyTypeId = 2,
                    Name = "Eristica",
                    Abbreviation = "ERT",
                    Slug = "ERT"
                }, new Currency
                {
                    Id = 1340,
                    CurrencyTypeId = 2,
                    Name = "Karbo",
                    Abbreviation = "KRB",
                    Slug = "KRB"
                }, new Currency
                {
                    Id = 2582,
                    CurrencyTypeId = 2,
                    Name = "LALA World",
                    Abbreviation = "LALA",
                    Slug = "LALA"
                }, new Currency
                {
                    Id = 3220,
                    CurrencyTypeId = 2,
                    Name = "DAV Coin",
                    Abbreviation = "DAV",
                    Slug = "DAV"
                }, new Currency
                {
                    Id = 3118,
                    CurrencyTypeId = 2,
                    Name = "Graviocoin",
                    Abbreviation = "GIO",
                    Slug = "GIO"
                }, new Currency
                {
                    Id = 2127,
                    CurrencyTypeId = 2,
                    Name = "eBitcoin",
                    Abbreviation = "EBTC",
                    Slug = "EBTC"
                }, new Currency
                {
                    Id = 2002,
                    CurrencyTypeId = 2,
                    Name = "TrezarCoin",
                    Abbreviation = "TZC",
                    Slug = "TZC"
                }, new Currency
                {
                    Id = 3469,
                    CurrencyTypeId = 2,
                    Name = "TrueDeck",
                    Abbreviation = "TDP",
                    Slug = "TDP"
                }, new Currency
                {
                    Id = 2547,
                    CurrencyTypeId = 2,
                    Name = "Experty",
                    Abbreviation = "EXY",
                    Slug = "EXY"
                }, new Currency
                {
                    Id = 1873,
                    CurrencyTypeId = 2,
                    Name = "Blocktix",
                    Abbreviation = "TIX",
                    Slug = "TIX"
                }, new Currency
                {
                    Id = 3237,
                    CurrencyTypeId = 2,
                    Name = "Timicoin",
                    Abbreviation = "TMC",
                    Slug = "TMC"
                }, new Currency
                {
                    Id = 2724,
                    CurrencyTypeId = 2,
                    Name = "Zippie",
                    Abbreviation = "ZIPT",
                    Slug = "ZIPT"
                }, new Currency
                {
                    Id = 3877,
                    CurrencyTypeId = 2,
                    Name = "WebDollar",
                    Abbreviation = "WEBD",
                    Slug = "WEBD"
                }, new Currency
                {
                    Id = 1762,
                    CurrencyTypeId = 2,
                    Name = "Ergo",
                    Abbreviation = "EFYT",
                    Slug = "EFYT"
                }, new Currency
                {
                    Id = 3455,
                    CurrencyTypeId = 2,
                    Name = "DEEX",
                    Abbreviation = "DEEX",
                    Slug = "DEEX"
                }, new Currency
                {
                    Id = 3365,
                    CurrencyTypeId = 2,
                    Name = "VeriSafe",
                    Abbreviation = "VSF",
                    Slug = "VSF"
                }, new Currency
                {
                    Id = 1002,
                    CurrencyTypeId = 2,
                    Name = "Sprouts",
                    Abbreviation = "SPRTS",
                    Slug = "SPRTS"
                }, new Currency
                {
                    Id = 2273,
                    CurrencyTypeId = 2,
                    Name = "Uquid Coin",
                    Abbreviation = "UQC",
                    Slug = "UQC"
                }, new Currency
                {
                    Id = 3302,
                    CurrencyTypeId = 2,
                    Name = "UChain",
                    Abbreviation = "UCN",
                    Slug = "UCN"
                }, new Currency
                {
                    Id = 1304,
                    CurrencyTypeId = 2,
                    Name = "Syndicate",
                    Abbreviation = "SYNX",
                    Slug = "SYNX"
                }, new Currency
                {
                    Id = 3240,
                    CurrencyTypeId = 2,
                    Name = "Ethersocial",
                    Abbreviation = "ESN",
                    Slug = "ESN"
                }, new Currency
                {
                    Id = 2676,
                    CurrencyTypeId = 2,
                    Name = "PHI Token",
                    Abbreviation = "PHI",
                    Slug = "PHI"
                }, new Currency
                {
                    Id = 1156,
                    CurrencyTypeId = 2,
                    Name = "Yocoin",
                    Abbreviation = "YOC",
                    Slug = "YOC"
                }, new Currency
                {
                    Id = 2985,
                    CurrencyTypeId = 2,
                    Name = "ARBITRAGE",
                    Abbreviation = "ARB",
                    Slug = "ARB"
                }, new Currency
                {
                    Id = 3148,
                    CurrencyTypeId = 2,
                    Name = "MetaMorph",
                    Abbreviation = "METM",
                    Slug = "METM"
                }, new Currency
                {
                    Id = 3028,
                    CurrencyTypeId = 2,
                    Name = "Formosa Financial",
                    Abbreviation = "FMF",
                    Slug = "FMF"
                }, new Currency
                {
                    Id = 2495,
                    CurrencyTypeId = 2,
                    Name = "PARETO Rewards",
                    Abbreviation = "PARETO",
                    Slug = "PARETO"
                }, new Currency
                {
                    Id = 322,
                    CurrencyTypeId = 2,
                    Name = "Energycoin",
                    Abbreviation = "ENRG",
                    Slug = "ENRG"
                }, new Currency
                {
                    Id = 3017,
                    CurrencyTypeId = 2,
                    Name = "Coinvest",
                    Abbreviation = "COIN",
                    Slug = "COIN"
                }, new Currency
                {
                    Id = 2863,
                    CurrencyTypeId = 2,
                    Name = "HOLD",
                    Abbreviation = "HOLD",
                    Slug = "HOLD"
                }, new Currency
                {
                    Id = 3909,
                    CurrencyTypeId = 2,
                    Name = "SPIDER VPS",
                    Abbreviation = "SPDR",
                    Slug = "SPDR"
                }, new Currency
                {
                    Id = 3094,
                    CurrencyTypeId = 2,
                    Name = "Scorum Coins",
                    Abbreviation = "SCR",
                    Slug = "SCR"
                }, new Currency
                {
                    Id = 3689,
                    CurrencyTypeId = 2,
                    Name = "Mocrow",
                    Abbreviation = "MCW",
                    Slug = "MCW"
                }, new Currency
                {
                    Id = 3264,
                    CurrencyTypeId = 2,
                    Name = "Digital Insurance Token",
                    Abbreviation = "DIT",
                    Slug = "DIT"
                }, new Currency
                {
                    Id = 2272,
                    CurrencyTypeId = 2,
                    Name = "Soma",
                    Abbreviation = "SCT",
                    Slug = "SCT"
                }, new Currency
                {
                    Id = 1916,
                    CurrencyTypeId = 2,
                    Name = "BiblePay",
                    Abbreviation = "BBP",
                    Slug = "BBP"
                }, new Currency
                {
                    Id = 2199,
                    CurrencyTypeId = 2,
                    Name = "ALQO",
                    Abbreviation = "XLQ",
                    Slug = "XLQ"
                }, new Currency
                {
                    Id = 3423,
                    CurrencyTypeId = 2,
                    Name = "Sharpay",
                    Abbreviation = "S",
                    Slug = "S"
                }, new Currency
                {
                    Id = 2966,
                    CurrencyTypeId = 2,
                    Name = "Fox Trading",
                    Abbreviation = "FOXT",
                    Slug = "FOXT"
                }, new Currency
                {
                    Id = 1845,
                    CurrencyTypeId = 2,
                    Name = "IXT",
                    Abbreviation = "IXT",
                    Slug = "IXT"
                }, new Currency
                {
                    Id = 2614,
                    CurrencyTypeId = 2,
                    Name = "BlitzPredict",
                    Abbreviation = "XBP",
                    Slug = "XBP"
                }, new Currency
                {
                    Id = 1226,
                    CurrencyTypeId = 2,
                    Name = "Qwark",
                    Abbreviation = "QWARK",
                    Slug = "QWARK"
                }, new Currency
                {
                    Id = 3171,
                    CurrencyTypeId = 2,
                    Name = "HeartBout",
                    Abbreviation = "HB",
                    Slug = "HB"
                }, new Currency
                {
                    Id = 1970,
                    CurrencyTypeId = 2,
                    Name = "ATBCoin",
                    Abbreviation = "ATB",
                    Slug = "ATB"
                }, new Currency
                {
                    Id = 3024,
                    CurrencyTypeId = 2,
                    Name = "Arionum",
                    Abbreviation = "ARO",
                    Slug = "ARO"
                }, new Currency
                {
                    Id = 3402,
                    CurrencyTypeId = 2,
                    Name = "Ifoods Chain",
                    Abbreviation = "IFOOD",
                    Slug = "IFOOD"
                }, new Currency
                {
                    Id = 3765,
                    CurrencyTypeId = 2,
                    Name = "Serve",
                    Abbreviation = "SERV",
                    Slug = "SERV"
                }, new Currency
                {
                    Id = 1008,
                    CurrencyTypeId = 2,
                    Name = "Capricoin",
                    Abbreviation = "CPC",
                    Slug = "CPC"
                }, new Currency
                {
                    Id = 3753,
                    CurrencyTypeId = 2,
                    Name = "PlatonCoin",
                    Abbreviation = "PLTC",
                    Slug = "PLTC"
                }, new Currency
                {
                    Id = 2976,
                    CurrencyTypeId = 2,
                    Name = "Ryo Currency",
                    Abbreviation = "RYO",
                    Slug = "RYO"
                }, new Currency
                {
                    Id = 3879,
                    CurrencyTypeId = 2,
                    Name = "ESBC",
                    Abbreviation = "ESBC",
                    Slug = "ESBC"
                }, new Currency
                {
                    Id = 1578,
                    CurrencyTypeId = 2,
                    Name = "Zero",
                    Abbreviation = "ZER",
                    Slug = "ZER"
                }, new Currency
                {
                    Id = 3230,
                    CurrencyTypeId = 2,
                    Name = "VULCANO",
                    Abbreviation = "VULC",
                    Slug = "VULC"
                }, new Currency
                {
                    Id = 3732,
                    CurrencyTypeId = 2,
                    Name = "Conceal",
                    Abbreviation = "CCX",
                    Slug = "CCX"
                }, new Currency
                {
                    Id = 3244,
                    CurrencyTypeId = 2,
                    Name = "Mindexcoin",
                    Abbreviation = "MIC",
                    Slug = "MIC"
                }, new Currency
                {
                    Id = 2879,
                    CurrencyTypeId = 2,
                    Name = "Origin Sport",
                    Abbreviation = "ORS",
                    Slug = "ORS"
                }, new Currency
                {
                    Id = 3245,
                    CurrencyTypeId = 2,
                    Name = "Ubcoin Market",
                    Abbreviation = "UBC",
                    Slug = "UBC"
                }, new Currency
                {
                    Id = 1894,
                    CurrencyTypeId = 2,
                    Name = "The ChampCoin",
                    Abbreviation = "TCC",
                    Slug = "TCC"
                }, new Currency
                {
                    Id = 2278,
                    CurrencyTypeId = 2,
                    Name = "HollyWoodCoin",
                    Abbreviation = "HWC",
                    Slug = "HWC"
                }, new Currency
                {
                    Id = 3166,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Incognito",
                    Abbreviation = "XBI",
                    Slug = "XBI"
                }, new Currency
                {
                    Id = 1869,
                    CurrencyTypeId = 2,
                    Name = "Mao Zedong",
                    Abbreviation = "MAO",
                    Slug = "MAO"
                }, new Currency
                {
                    Id = 3674,
                    CurrencyTypeId = 2,
                    Name = "Globatalent",
                    Abbreviation = "GBT",
                    Slug = "GBT"
                }, new Currency
                {
                    Id = 3901,
                    CurrencyTypeId = 2,
                    Name = "KuboCoin",
                    Abbreviation = "KUBO",
                    Slug = "KUBO"
                }, new Currency
                {
                    Id = 3752,
                    CurrencyTypeId = 2,
                    Name = "uPlexa",
                    Abbreviation = "UPX",
                    Slug = "UPX"
                }, new Currency
                {
                    Id = 2414,
                    CurrencyTypeId = 2,
                    Name = "RealChain",
                    Abbreviation = "RCT",
                    Slug = "RCT"
                }, new Currency
                {
                    Id = 2663,
                    CurrencyTypeId = 2,
                    Name = "StarCoin",
                    Abbreviation = "KST",
                    Slug = "KST"
                }, new Currency
                {
                    Id = 3805,
                    CurrencyTypeId = 2,
                    Name = "BoatPilot Token",
                    Abbreviation = "NAVY",
                    Slug = "NAVY"
                }, new Currency
                {
                    Id = 2680,
                    CurrencyTypeId = 2,
                    Name = "HBZ coin",
                    Abbreviation = "HBZ",
                    Slug = "HBZ"
                }, new Currency
                {
                    Id = 2920,
                    CurrencyTypeId = 2,
                    Name = "0xcert",
                    Abbreviation = "ZXC",
                    Slug = "ZXC"
                }, new Currency
                {
                    Id = 2988,
                    CurrencyTypeId = 2,
                    Name = "Pigeoncoin",
                    Abbreviation = "PGN",
                    Slug = "PGN"
                }, new Currency
                {
                    Id = 3362,
                    CurrencyTypeId = 2,
                    Name = "Auxilium",
                    Abbreviation = "AUX",
                    Slug = "AUX"
                }, new Currency
                {
                    Id = 3373,
                    CurrencyTypeId = 2,
                    Name = "Bethereum",
                    Abbreviation = "BETHER",
                    Slug = "BETHER"
                }, new Currency
                {
                    Id = 3596,
                    CurrencyTypeId = 2,
                    Name = "Nerva",
                    Abbreviation = "XNV",
                    Slug = "XNV"
                }, new Currency
                {
                    Id = 3011,
                    CurrencyTypeId = 2,
                    Name = "BitScreener Token",
                    Abbreviation = "BITX",
                    Slug = "BITX"
                }, new Currency
                {
                    Id = 2922,
                    CurrencyTypeId = 2,
                    Name = "Atonomi",
                    Abbreviation = "ATMI",
                    Slug = "ATMI"
                }, new Currency
                {
                    Id = 3597,
                    CurrencyTypeId = 2,
                    Name = "InterValue",
                    Abbreviation = "INVE",
                    Slug = "INVE"
                }, new Currency
                {
                    Id = 1745,
                    CurrencyTypeId = 2,
                    Name = "Dinastycoin",
                    Abbreviation = "DCY",
                    Slug = "DCY"
                }, new Currency
                {
                    Id = 1216,
                    CurrencyTypeId = 2,
                    Name = "EDRCoin",
                    Abbreviation = "EDRC",
                    Slug = "EDRC"
                }, new Currency
                {
                    Id = 2584,
                    CurrencyTypeId = 2,
                    Name = "Debitum",
                    Abbreviation = "DEB",
                    Slug = "DEB"
                }, new Currency
                {
                    Id = 3440,
                    CurrencyTypeId = 2,
                    Name = "AirWire",
                    Abbreviation = "WIRE",
                    Slug = "WIRE"
                }, new Currency
                {
                    Id = 3158,
                    CurrencyTypeId = 2,
                    Name = "ZCore",
                    Abbreviation = "ZCR",
                    Slug = "ZCR"
                }, new Currency
                {
                    Id = 3758,
                    CurrencyTypeId = 2,
                    Name = "Max Property Group",
                    Abbreviation = "MPG",
                    Slug = "MPG"
                }, new Currency
                {
                    Id = 3411,
                    CurrencyTypeId = 2,
                    Name = "Welltrado",
                    Abbreviation = "WTL",
                    Slug = "WTL"
                }, new Currency
                {
                    Id = 3643,
                    CurrencyTypeId = 2,
                    Name = "TENA",
                    Abbreviation = "TENA",
                    Slug = "TENA"
                }, new Currency
                {
                    Id = 2413,
                    CurrencyTypeId = 2,
                    Name = "Ethouse",
                    Abbreviation = "HORSE",
                    Slug = "HORSE"
                }, new Currency
                {
                    Id = 2557,
                    CurrencyTypeId = 2,
                    Name = "Bee Token",
                    Abbreviation = "BEE",
                    Slug = "BEE"
                }, new Currency
                {
                    Id = 2568,
                    CurrencyTypeId = 2,
                    Name = "JET8",
                    Abbreviation = "J8T",
                    Slug = "J8T"
                }, new Currency
                {
                    Id = 2759,
                    CurrencyTypeId = 2,
                    Name = "Patron",
                    Abbreviation = "PAT",
                    Slug = "PAT"
                }, new Currency
                {
                    Id = 2956,
                    CurrencyTypeId = 2,
                    Name = "Narrative",
                    Abbreviation = "NRVE",
                    Slug = "NRVE"
                }, new Currency
                {
                    Id = 3661,
                    CurrencyTypeId = 2,
                    Name = "Stronghold Token",
                    Abbreviation = "SHX",
                    Slug = "SHX"
                }, new Currency
                {
                    Id = 3613,
                    CurrencyTypeId = 2,
                    Name = "Dash Green",
                    Abbreviation = "DASHG",
                    Slug = "DASHG"
                }, new Currency
                {
                    Id = 3947,
                    CurrencyTypeId = 2,
                    Name = "HashNet BitEco",
                    Abbreviation = "HNB",
                    Slug = "HNB"
                }, new Currency
                {
                    Id = 3774,
                    CurrencyTypeId = 2,
                    Name = "Maincoin",
                    Abbreviation = "MNC",
                    Slug = "MNC"
                }, new Currency
                {
                    Id = 2601,
                    CurrencyTypeId = 2,
                    Name = "1World",
                    Abbreviation = "1WO",
                    Slug = "1WO"
                }, new Currency
                {
                    Id = 2142,
                    CurrencyTypeId = 2,
                    Name = "FORCE",
                    Abbreviation = "FOR",
                    Slug = "FOR"
                }, new Currency
                {
                    Id = 2653,
                    CurrencyTypeId = 2,
                    Name = "Auctus",
                    Abbreviation = "AUC",
                    Slug = "AUC"
                }, new Currency
                {
                    Id = 2330,
                    CurrencyTypeId = 2,
                    Name = "Pylon Network",
                    Abbreviation = "PYLNT",
                    Slug = "PYLNT"
                }, new Currency
                {
                    Id = 3876,
                    CurrencyTypeId = 2,
                    Name = "EnterCoin",
                    Abbreviation = "ENTRC",
                    Slug = "ENTRC"
                }, new Currency
                {
                    Id = 2200,
                    CurrencyTypeId = 2,
                    Name = "GoByte",
                    Abbreviation = "GBX",
                    Slug = "GBX"
                }, new Currency
                {
                    Id = 1387,
                    CurrencyTypeId = 2,
                    Name = "VeriumReserve",
                    Abbreviation = "VRM",
                    Slug = "VRM"
                }, new Currency
                {
                    Id = 2295,
                    CurrencyTypeId = 2,
                    Name = "Starbase",
                    Abbreviation = "STAR",
                    Slug = "STAR"
                }, new Currency
                {
                    Id = 2465,
                    CurrencyTypeId = 2,
                    Name = "Blockport",
                    Abbreviation = "BPT",
                    Slug = "BPT"
                }, new Currency
                {
                    Id = 2311,
                    CurrencyTypeId = 2,
                    Name = "ACE (TokenStars)",
                    Abbreviation = "ACE",
                    Slug = "ACE"
                }, new Currency
                {
                    Id = 2961,
                    CurrencyTypeId = 2,
                    Name = "Relex",
                    Abbreviation = "RLX",
                    Slug = "RLX"
                }, new Currency
                {
                    Id = 1694,
                    CurrencyTypeId = 2,
                    Name = "Sumokoin",
                    Abbreviation = "SUMO",
                    Slug = "SUMO"
                }, new Currency
                {
                    Id = 2258,
                    CurrencyTypeId = 2,
                    Name = "Snovian.Space",
                    Abbreviation = "SNOV",
                    Slug = "SNOV"
                }, new Currency
                {
                    Id = 3818,
                    CurrencyTypeId = 2,
                    Name = "GoPower",
                    Abbreviation = "GPT",
                    Slug = "GPT"
                }, new Currency
                {
                    Id = 2836,
                    CurrencyTypeId = 2,
                    Name = "Bigbom",
                    Abbreviation = "BBO",
                    Slug = "BBO"
                }, new Currency
                {
                    Id = 1588,
                    CurrencyTypeId = 2,
                    Name = "Tokes",
                    Abbreviation = "TKS",
                    Slug = "TKS"
                }, new Currency
                {
                    Id = 1249,
                    CurrencyTypeId = 2,
                    Name = "Elcoin",
                    Abbreviation = "EL",
                    Slug = "EL"
                }, new Currency
                {
                    Id = 3372,
                    CurrencyTypeId = 2,
                    Name = "Repme",
                    Abbreviation = "RPM",
                    Slug = "RPM"
                }, new Currency
                {
                    Id = 1902,
                    CurrencyTypeId = 2,
                    Name = "MyBit",
                    Abbreviation = "MYB",
                    Slug = "MYB"
                }, new Currency
                {
                    Id = 2884,
                    CurrencyTypeId = 2,
                    Name = "FSBT API Token",
                    Abbreviation = "FSBT",
                    Slug = "FSBT"
                }, new Currency
                {
                    Id = 3179,
                    CurrencyTypeId = 2,
                    Name = "Arbidex",
                    Abbreviation = "ABX",
                    Slug = "ABX"
                }, new Currency
                {
                    Id = 3128,
                    CurrencyTypeId = 2,
                    Name = "SiaCashCoin",
                    Abbreviation = "SCC",
                    Slug = "sia-cash-coin"
                }, new Currency
                {
                    Id = 3119,
                    CurrencyTypeId = 2,
                    Name = "Alchemint Standards",
                    Abbreviation = "SDS",
                    Slug = "SDS"
                }, new Currency
                {
                    Id = 3106,
                    CurrencyTypeId = 2,
                    Name = "PKG Token",
                    Abbreviation = "PKG",
                    Slug = "PKG"
                }, new Currency
                {
                    Id = 3837,
                    CurrencyTypeId = 2,
                    Name = "MFCoin",
                    Abbreviation = "MFC",
                    Slug = "MFC"
                }, new Currency
                {
                    Id = 2089,
                    CurrencyTypeId = 2,
                    Name = "ClearPoll",
                    Abbreviation = "POLL",
                    Slug = "POLL"
                }, new Currency
                {
                    Id = 3071,
                    CurrencyTypeId = 2,
                    Name = "EUNO",
                    Abbreviation = "EUNO",
                    Slug = "EUNO"
                }, new Currency
                {
                    Id = 2271,
                    CurrencyTypeId = 2,
                    Name = "Verify",
                    Abbreviation = "CRED",
                    Slug = "CRED"
                }, new Currency
                {
                    Id = 1704,
                    CurrencyTypeId = 2,
                    Name = "eBoost",
                    Abbreviation = "EBST",
                    Slug = "EBST"
                }, new Currency
                {
                    Id = 2611,
                    CurrencyTypeId = 2,
                    Name = "Spectiv",
                    Abbreviation = "SIG",
                    Slug = "SIG"
                }, new Currency
                {
                    Id = 56,
                    CurrencyTypeId = 2,
                    Name = "Zetacoin",
                    Abbreviation = "ZET",
                    Slug = "ZET"
                }, new Currency
                {
                    Id = 2323,
                    CurrencyTypeId = 2,
                    Name = "HEROcoin",
                    Abbreviation = "PLAY",
                    Slug = "PLAY"
                }, new Currency
                {
                    Id = 3101,
                    CurrencyTypeId = 2,
                    Name = "OptiToken",
                    Abbreviation = "OPTI",
                    Slug = "OPTI"
                }, new Currency
                {
                    Id = 2237,
                    CurrencyTypeId = 2,
                    Name = "EventChain",
                    Abbreviation = "EVC",
                    Slug = "EVC"
                }, new Currency
                {
                    Id = 2968,
                    CurrencyTypeId = 2,
                    Name = "Bridge Protocol",
                    Abbreviation = "BRDG",
                    Slug = "BRDG"
                }, new Currency
                {
                    Id = 2269,
                    CurrencyTypeId = 2,
                    Name = "WandX",
                    Abbreviation = "WAND",
                    Slug = "WAND"
                }, new Currency
                {
                    Id = 977,
                    CurrencyTypeId = 2,
                    Name = "GravityCoin",
                    Abbreviation = "GXX",
                    Slug = "GXX"
                }, new Currency
                {
                    Id = 2754,
                    CurrencyTypeId = 2,
                    Name = "HeroNode",
                    Abbreviation = "HER",
                    Slug = "HER"
                }, new Currency
                {
                    Id = 2963,
                    CurrencyTypeId = 2,
                    Name = "View",
                    Abbreviation = "VIEW",
                    Slug = "VIEW"
                }, new Currency
                {
                    Id = 2658,
                    CurrencyTypeId = 2,
                    Name = "SyncFab",
                    Abbreviation = "MFG",
                    Slug = "MFG"
                }, new Currency
                {
                    Id = 3247,
                    CurrencyTypeId = 2,
                    Name = "Fire Lotto",
                    Abbreviation = "FLOT",
                    Slug = "FLOT"
                }, new Currency
                {
                    Id = 2774,
                    CurrencyTypeId = 2,
                    Name = "Invacio",
                    Abbreviation = "INV",
                    Slug = "INV"
                }, new Currency
                {
                    Id = 2646,
                    CurrencyTypeId = 2,
                    Name = "AdHive",
                    Abbreviation = "ADH",
                    Slug = "ADH"
                }, new Currency
                {
                    Id = 2504,
                    CurrencyTypeId = 2,
                    Name = "Iungo",
                    Abbreviation = "ING",
                    Slug = "ING"
                }, new Currency
                {
                    Id = 2946,
                    CurrencyTypeId = 2,
                    Name = "Proton Token",
                    Abbreviation = "PTT",
                    Slug = "PTT"
                }, new Currency
                {
                    Id = 2656,
                    CurrencyTypeId = 2,
                    Name = "Daneel",
                    Abbreviation = "DAN",
                    Slug = "DAN"
                }, new Currency
                {
                    Id = 3942,
                    CurrencyTypeId = 2,
                    Name = "Qwertycoin",
                    Abbreviation = "QWC",
                    Slug = "QWC"
                }, new Currency
                {
                    Id = 3672,
                    CurrencyTypeId = 2,
                    Name = "DogeCash",
                    Abbreviation = "DOGEC",
                    Slug = "DOGEC"
                }, new Currency
                {
                    Id = 2445,
                    CurrencyTypeId = 2,
                    Name = "Block Array",
                    Abbreviation = "ARY",
                    Slug = "ARY"
                }, new Currency
                {
                    Id = 3793,
                    CurrencyTypeId = 2,
                    Name = "Galilel",
                    Abbreviation = "GALI",
                    Slug = "GALI"
                }, new Currency
                {
                    Id = 2126,
                    CurrencyTypeId = 2,
                    Name = "FlypMe",
                    Abbreviation = "FYP",
                    Slug = "FYP"
                }, new Currency
                {
                    Id = 2615,
                    CurrencyTypeId = 2,
                    Name = "Blocklancer",
                    Abbreviation = "LNC",
                    Slug = "LNC"
                }, new Currency
                {
                    Id = 3203,
                    CurrencyTypeId = 2,
                    Name = "Lobstex",
                    Abbreviation = "LOBS",
                    Slug = "LOBS"
                }, new Currency
                {
                    Id = 2870,
                    CurrencyTypeId = 2,
                    Name = "FantasyGold",
                    Abbreviation = "FGC",
                    Slug = "FGC"
                }, new Currency
                {
                    Id = 3627,
                    CurrencyTypeId = 2,
                    Name = "Block-Logic",
                    Abbreviation = "BLTG",
                    Slug = "BLTG"
                }, new Currency
                {
                    Id = 3278,
                    CurrencyTypeId = 2,
                    Name = "PENG",
                    Abbreviation = "PENG",
                    Slug = "PENG"
                }, new Currency
                {
                    Id = 2990,
                    CurrencyTypeId = 2,
                    Name = "EXMR",
                    Abbreviation = "EXMR",
                    Slug = "EXMR"
                }, new Currency
                {
                    Id = 2940,
                    CurrencyTypeId = 2,
                    Name = "Sp8de",
                    Abbreviation = "SPX",
                    Slug = "SPX"
                }, new Currency
                {
                    Id = 3619,
                    CurrencyTypeId = 2,
                    Name = "BEAT",
                    Abbreviation = "BEAT",
                    Slug = "BEAT"
                }, new Currency
                {
                    Id = 3615,
                    CurrencyTypeId = 2,
                    Name = "HyperQuant",
                    Abbreviation = "HQT",
                    Slug = "HQT"
                }, new Currency
                {
                    Id = 2705,
                    CurrencyTypeId = 2,
                    Name = "Amon",
                    Abbreviation = "AMN",
                    Slug = "AMN"
                }, new Currency
                {
                    Id = 2733,
                    CurrencyTypeId = 2,
                    Name = "Freyrchain",
                    Abbreviation = "FREC",
                    Slug = "FREC"
                }, new Currency
                {
                    Id = 2509,
                    CurrencyTypeId = 2,
                    Name = "EtherSportz",
                    Abbreviation = "ESZ",
                    Slug = "ESZ"
                }, new Currency
                {
                    Id = 2367,
                    CurrencyTypeId = 2,
                    Name = "Aigang",
                    Abbreviation = "AIX",
                    Slug = "AIX"
                }, new Currency
                {
                    Id = 3878,
                    CurrencyTypeId = 2,
                    Name = "Swap",
                    Abbreviation = "XWP",
                    Slug = "XWP"
                }, new Currency
                {
                    Id = 2679,
                    CurrencyTypeId = 2,
                    Name = "Decentralized Machine Learning",
                    Abbreviation = "DML",
                    Slug = "DML"
                }, new Currency
                {
                    Id = 1933,
                    CurrencyTypeId = 2,
                    Name = "Suretly",
                    Abbreviation = "SUR",
                    Slug = "SUR"
                }, new Currency
                {
                    Id = 2660,
                    CurrencyTypeId = 2,
                    Name = "Aditus",
                    Abbreviation = "ADI",
                    Slug = "ADI"
                }, new Currency
                {
                    Id = 2889,
                    CurrencyTypeId = 2,
                    Name = "Bob's Repair",
                    Abbreviation = "BOB",
                    Slug = "BOB"
                }, new Currency
                {
                    Id = 3390,
                    CurrencyTypeId = 2,
                    Name = "Quantis Network",
                    Abbreviation = "QUAN",
                    Slug = "QUAN"
                }, new Currency
                {
                    Id = 145,
                    CurrencyTypeId = 2,
                    Name = "DopeCoin",
                    Abbreviation = "DOPE",
                    Slug = "DOPE"
                }, new Currency
                {
                    Id = 3285,
                    CurrencyTypeId = 2,
                    Name = "Birake",
                    Abbreviation = "BIR",
                    Slug = "BIR"
                }, new Currency
                {
                    Id = 2684,
                    CurrencyTypeId = 2,
                    Name = "Aphelion",
                    Abbreviation = "APH",
                    Slug = "APH"
                }, new Currency
                {
                    Id = 3018,
                    CurrencyTypeId = 2,
                    Name = "Kalkulus",
                    Abbreviation = "KLKS",
                    Slug = "KLKS"
                }, new Currency
                {
                    Id = 2729,
                    CurrencyTypeId = 2,
                    Name = "TEAM (TokenStars)",
                    Abbreviation = "TEAM",
                    Slug = "TEAM"
                }, new Currency
                {
                    Id = 2849,
                    CurrencyTypeId = 2,
                    Name = "Hurify",
                    Abbreviation = "HUR",
                    Slug = "HUR"
                }, new Currency
                {
                    Id = 2948,
                    CurrencyTypeId = 2,
                    Name = "Jury.Online Token",
                    Abbreviation = "JOT",
                    Slug = "JOT"
                }, new Currency
                {
                    Id = 3953,
                    CurrencyTypeId = 2,
                    Name = "Evedo",
                    Abbreviation = "EVED",
                    Slug = "EVED"
                }, new Currency
                {
                    Id = 3132,
                    CurrencyTypeId = 2,
                    Name = "EtherGem",
                    Abbreviation = "EGEM",
                    Slug = "EGEM"
                }, new Currency
                {
                    Id = 3482,
                    CurrencyTypeId = 2,
                    Name = "Teloscoin",
                    Abbreviation = "TELOS",
                    Slug = "TELOS"
                }, new Currency
                {
                    Id = 3604,
                    CurrencyTypeId = 2,
                    Name = "SkyHub Coin",
                    Abbreviation = "SHB",
                    Slug = "SHB"
                }, new Currency
                {
                    Id = 2923,
                    CurrencyTypeId = 2,
                    Name = "XMCT",
                    Abbreviation = "XMCT",
                    Slug = "XMCT"
                }, new Currency
                {
                    Id = 3248,
                    CurrencyTypeId = 2,
                    Name = "AiLink Token",
                    Abbreviation = "ALI",
                    Slug = "ALI"
                }, new Currency
                {
                    Id = 3505,
                    CurrencyTypeId = 2,
                    Name = "Typerium",
                    Abbreviation = "TYPE",
                    Slug = "TYPE"
                }, new Currency
                {
                    Id = 2744,
                    CurrencyTypeId = 2,
                    Name = "NPER",
                    Abbreviation = "NPER",
                    Slug = "NPER"
                }, new Currency
                {
                    Id = 3339,
                    CurrencyTypeId = 2,
                    Name = "Puregold Token",
                    Abbreviation = "PGTS",
                    Slug = "PGTS"
                }, new Currency
                {
                    Id = 3112,
                    CurrencyTypeId = 2,
                    Name = "Bitnation",
                    Abbreviation = "XPAT",
                    Slug = "XPAT"
                }, new Currency
                {
                    Id = 3742,
                    CurrencyTypeId = 2,
                    Name = "Chimpion",
                    Abbreviation = "BNANA",
                    Slug = "BNANA"
                }, new Currency
                {
                    Id = 3209,
                    CurrencyTypeId = 2,
                    Name = "4NEW",
                    Abbreviation = "KWATT",
                    Slug = "KWATT"
                }, new Currency
                {
                    Id = 3868,
                    CurrencyTypeId = 2,
                    Name = "SignatureChain ",
                    Abbreviation = "SICA",
                    Slug = "SICA"
                }, new Currency
                {
                    Id = 3027,
                    CurrencyTypeId = 2,
                    Name = "Webcoin",
                    Abbreviation = "WEB",
                    Slug = "WEB"
                }, new Currency
                {
                    Id = 551,
                    CurrencyTypeId = 2,
                    Name = "NeosCoin",
                    Abbreviation = "NEOS",
                    Slug = "NEOS"
                }, new Currency
                {
                    Id = 3149,
                    CurrencyTypeId = 2,
                    Name = "NetKoin",
                    Abbreviation = "NTK",
                    Slug = "NTK"
                }, new Currency
                {
                    Id = 2270,
                    CurrencyTypeId = 2,
                    Name = "SportyCo",
                    Abbreviation = "SPF",
                    Slug = "SPF"
                }, new Currency
                {
                    Id = 2356,
                    CurrencyTypeId = 2,
                    Name = "CFun",
                    Abbreviation = "CFUN",
                    Slug = "CFUN"
                }, new Currency
                {
                    Id = 3792,
                    CurrencyTypeId = 2,
                    Name = "ARAW",
                    Abbreviation = "ARAW",
                    Slug = "ARAW"
                }, new Currency
                {
                    Id = 1465,
                    CurrencyTypeId = 2,
                    Name = "Veros",
                    Abbreviation = "VRS",
                    Slug = "VRS"
                }, new Currency
                {
                    Id = 3763,
                    CurrencyTypeId = 2,
                    Name = "ODUWA",
                    Abbreviation = "OWC",
                    Slug = "OWC"
                }, new Currency
                {
                    Id = 3256,
                    CurrencyTypeId = 2,
                    Name = "Bitether",
                    Abbreviation = "BTR",
                    Slug = "BTR"
                }, new Currency
                {
                    Id = 2944,
                    CurrencyTypeId = 2,
                    Name = "Elysian",
                    Abbreviation = "ELY",
                    Slug = "ELY"
                }, new Currency
                {
                    Id = 2984,
                    CurrencyTypeId = 2,
                    Name = "Newton Coin Project",
                    Abbreviation = "NCP",
                    Slug = "NCP"
                }, new Currency
                {
                    Id = 3121,
                    CurrencyTypeId = 2,
                    Name = "IGToken",
                    Abbreviation = "IG",
                    Slug = "IG"
                }, new Currency
                {
                    Id = 730,
                    CurrencyTypeId = 2,
                    Name = "GCN Coin",
                    Abbreviation = "GCN",
                    Slug = "GCN"
                }, new Currency
                {
                    Id = 3097,
                    CurrencyTypeId = 2,
                    Name = "XOVBank",
                    Abbreviation = "XOV",
                    Slug = "XOV"
                }, new Currency
                {
                    Id = 2720,
                    CurrencyTypeId = 2,
                    Name = "Parkgene",
                    Abbreviation = "GENE",
                    Slug = "GENE"
                }, new Currency
                {
                    Id = 3823,
                    CurrencyTypeId = 2,
                    Name = "OLXA",
                    Abbreviation = "OLXA",
                    Slug = "OLXA"
                }, new Currency
                {
                    Id = 3386,
                    CurrencyTypeId = 2,
                    Name = "Actinium",
                    Abbreviation = "ACM",
                    Slug = "ACM"
                }, new Currency
                {
                    Id = 1252,
                    CurrencyTypeId = 2,
                    Name = "2GIVE",
                    Abbreviation = "2GIVE",
                    Slug = "2GIVE"
                }, new Currency
                {
                    Id = 2165,
                    CurrencyTypeId = 2,
                    Name = "ERC20",
                    Abbreviation = "ERC20",
                    Slug = "ERC20"
                }, new Currency
                {
                    Id = 3383,
                    CurrencyTypeId = 2,
                    Name = "Knekted",
                    Abbreviation = "KNT",
                    Slug = "KNT"
                }, new Currency
                {
                    Id = 3497,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Zero",
                    Abbreviation = "BZX",
                    Slug = "BZX"
                }, new Currency
                {
                    Id = 2717,
                    CurrencyTypeId = 2,
                    Name = "BoutsPro",
                    Abbreviation = "BOUTS",
                    Slug = "BOUTS"
                }, new Currency
                {
                    Id = 3808,
                    CurrencyTypeId = 2,
                    Name = "Cointorox",
                    Abbreviation = "OROX",
                    Slug = "OROX"
                }, new Currency
                {
                    Id = 1787,
                    CurrencyTypeId = 2,
                    Name = "Jetcoin",
                    Abbreviation = "JET",
                    Slug = "JET"
                }, new Currency
                {
                    Id = 2825,
                    CurrencyTypeId = 2,
                    Name = "Naviaddress",
                    Abbreviation = "NAVI",
                    Slug = "NAVI"
                }, new Currency
                {
                    Id = 3452,
                    CurrencyTypeId = 2,
                    Name = "Ether-1",
                    Abbreviation = "ETHO",
                    Slug = "ETHO"
                }, new Currency
                {
                    Id = 2704,
                    CurrencyTypeId = 2,
                    Name = "Transcodium",
                    Abbreviation = "TNS",
                    Slug = "TNS"
                }, new Currency
                {
                    Id = 2172,
                    CurrencyTypeId = 2,
                    Name = "Emphy",
                    Abbreviation = "EPY",
                    Slug = "EPY"
                }, new Currency
                {
                    Id = 916,
                    CurrencyTypeId = 2,
                    Name = "MedicCoin",
                    Abbreviation = "MEDIC",
                    Slug = "MEDIC"
                }, new Currency
                {
                    Id = 1803,
                    CurrencyTypeId = 2,
                    Name = "PeepCoin",
                    Abbreviation = "PCN",
                    Slug = "PCN"
                }, new Currency
                {
                    Id = 3589,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Meta",
                    Abbreviation = "ETHM",
                    Slug = "ETHM"
                }, new Currency
                {
                    Id = 1736,
                    CurrencyTypeId = 2,
                    Name = "Unify",
                    Abbreviation = "UNIFY",
                    Slug = "UNIFY"
                }, new Currency
                {
                    Id = 2452,
                    CurrencyTypeId = 2,
                    Name = "Tokenbox",
                    Abbreviation = "TBX",
                    Slug = "TBX"
                }, new Currency
                {
                    Id = 2747,
                    CurrencyTypeId = 2,
                    Name = "BlockMesh",
                    Abbreviation = "BMH",
                    Slug = "BMH"
                }, new Currency
                {
                    Id = 3856,
                    CurrencyTypeId = 2,
                    Name = "SF Capital",
                    Abbreviation = "SFCP",
                    Slug = "SFCP"
                }, new Currency
                {
                    Id = 3449,
                    CurrencyTypeId = 2,
                    Name = "MMOCoin",
                    Abbreviation = "MMO",
                    Slug = "MMO"
                }, new Currency
                {
                    Id = 2256,
                    CurrencyTypeId = 2,
                    Name = "Bonpay",
                    Abbreviation = "BON",
                    Slug = "BON"
                }, new Currency
                {
                    Id = 2244,
                    CurrencyTypeId = 2,
                    Name = "Payfair",
                    Abbreviation = "PFR",
                    Slug = "PFR"
                }, new Currency
                {
                    Id = 1650,
                    CurrencyTypeId = 2,
                    Name = "ProCurrency",
                    Abbreviation = "PROC",
                    Slug = "PROC"
                }, new Currency
                {
                    Id = 2286,
                    CurrencyTypeId = 2,
                    Name = "MicroMoney",
                    Abbreviation = "AMM",
                    Slug = "AMM"
                }, new Currency
                {
                    Id = 3869,
                    CurrencyTypeId = 2,
                    Name = "Alpha Token",
                    Abbreviation = "A",
                    Slug = "A"
                }, new Currency
                {
                    Id = 2198,
                    CurrencyTypeId = 2,
                    Name = "Viuly",
                    Abbreviation = "VIU",
                    Slug = "VIU"
                }, new Currency
                {
                    Id = 3741,
                    CurrencyTypeId = 2,
                    Name = "EurocoinToken",
                    Abbreviation = "ECTE",
                    Slug = "ECTE"
                }, new Currency
                {
                    Id = 1830,
                    CurrencyTypeId = 2,
                    Name = "SkinCoin",
                    Abbreviation = "SKIN",
                    Slug = "SKIN"
                }, new Currency
                {
                    Id = 2419,
                    CurrencyTypeId = 2,
                    Name = "Profile Utility Token",
                    Abbreviation = "PUT",
                    Slug = "PUT"
                }, new Currency
                {
                    Id = 3708,
                    CurrencyTypeId = 2,
                    Name = "Exosis",
                    Abbreviation = "EXO",
                    Slug = "EXO"
                }, new Currency
                {
                    Id = 2042,
                    CurrencyTypeId = 2,
                    Name = "HelloGold",
                    Abbreviation = "HGT",
                    Slug = "HGT"
                }, new Currency
                {
                    Id = 3523,
                    CurrencyTypeId = 2,
                    Name = "SnodeCoin",
                    Abbreviation = "SND",
                    Slug = "SND"
                }, new Currency
                {
                    Id = 2745,
                    CurrencyTypeId = 2,
                    Name = "Joint Ventures",
                    Abbreviation = "JOINT",
                    Slug = "JOINT"
                }, new Currency
                {
                    Id = 2974,
                    CurrencyTypeId = 2,
                    Name = "Lightpaycoin",
                    Abbreviation = "LPC",
                    Slug = "LPC"
                }, new Currency
                {
                    Id = 1678,
                    CurrencyTypeId = 2,
                    Name = "InsaneCoin",
                    Abbreviation = "INSN",
                    Slug = "INSN"
                }, new Currency
                {
                    Id = 2752,
                    CurrencyTypeId = 2,
                    Name = "Datarius Credit",
                    Abbreviation = "DTRC",
                    Slug = "DTRC"
                }, new Currency
                {
                    Id = 2942,
                    CurrencyTypeId = 2,
                    Name = "Aegeus",
                    Abbreviation = "AEG",
                    Slug = "AEG"
                }, new Currency
                {
                    Id = 3068,
                    CurrencyTypeId = 2,
                    Name = "BitcoiNote",
                    Abbreviation = "BTCN",
                    Slug = "BTCN"
                }, new Currency
                {
                    Id = 3086,
                    CurrencyTypeId = 2,
                    Name = "Kora Network Token",
                    Abbreviation = "KNT",
                    Slug = "korapay"
                }, new Currency
                {
                    Id = 2931,
                    CurrencyTypeId = 2,
                    Name = "Engagement Token",
                    Abbreviation = "ENGT",
                    Slug = "ENGT"
                }, new Currency
                {
                    Id = 945,
                    CurrencyTypeId = 2,
                    Name = "Bata",
                    Abbreviation = "BTA",
                    Slug = "BTA"
                }, new Currency
                {
                    Id = 2565,
                    CurrencyTypeId = 2,
                    Name = "StarterCoin",
                    Abbreviation = "STAC",
                    Slug = "STAC"
                }, new Currency
                {
                    Id = 3021,
                    CurrencyTypeId = 2,
                    Name = "InternationalCryptoX",
                    Abbreviation = "INCX",
                    Slug = "INCX"
                }, new Currency
                {
                    Id = 3588,
                    CurrencyTypeId = 2,
                    Name = "Absolute",
                    Abbreviation = "ABS",
                    Slug = "ABS"
                }, new Currency
                {
                    Id = 3433,
                    CurrencyTypeId = 2,
                    Name = "EUNOMIA",
                    Abbreviation = "ENTS",
                    Slug = "ENTS"
                }, new Currency
                {
                    Id = 3001,
                    CurrencyTypeId = 2,
                    Name = "KWHCoin",
                    Abbreviation = "KWH",
                    Slug = "KWH"
                }, new Currency
                {
                    Id = 3312,
                    CurrencyTypeId = 2,
                    Name = "Evimeria",
                    Abbreviation = "EVI",
                    Slug = "EVI"
                }, new Currency
                {
                    Id = 3798,
                    CurrencyTypeId = 2,
                    Name = "Xuez",
                    Abbreviation = "XUEZ",
                    Slug = "XUEZ"
                }, new Currency
                {
                    Id = 3048,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Turbo Koin",
                    Abbreviation = "BTK",
                    Slug = "BTK"
                }, new Currency
                {
                    Id = 1153,
                    CurrencyTypeId = 2,
                    Name = "Creditbit",
                    Abbreviation = "CRB",
                    Slug = "CRB"
                }, new Currency
                {
                    Id = 3685,
                    CurrencyTypeId = 2,
                    Name = "BTC Lite",
                    Abbreviation = "BTCL",
                    Slug = "BTCL"
                }, new Currency
                {
                    Id = 3777,
                    CurrencyTypeId = 2,
                    Name = "Spectrum",
                    Abbreviation = "SPT",
                    Slug = "SPT"
                }, new Currency
                {
                    Id = 3629,
                    CurrencyTypeId = 2,
                    Name = "LRM Coin",
                    Abbreviation = "LRM",
                    Slug = "LRM"
                }, new Currency
                {
                    Id = 2977,
                    CurrencyTypeId = 2,
                    Name = "BitRewards",
                    Abbreviation = "BIT",
                    Slug = "BIT"
                }, new Currency
                {
                    Id = 2721,
                    CurrencyTypeId = 2,
                    Name = "APR Coin",
                    Abbreviation = "APR",
                    Slug = "APR"
                }, new Currency
                {
                    Id = 3451,
                    CurrencyTypeId = 2,
                    Name = "BLOC.MONEY",
                    Abbreviation = "BLOC",
                    Slug = "BLOC"
                }, new Currency
                {
                    Id = 1683,
                    CurrencyTypeId = 2,
                    Name = "RouletteToken",
                    Abbreviation = "RLT",
                    Slug = "RLT"
                }, new Currency
                {
                    Id = 3646,
                    CurrencyTypeId = 2,
                    Name = "Herbalist Token",
                    Abbreviation = "HERB",
                    Slug = "HERB"
                }, new Currency
                {
                    Id = 3189,
                    CurrencyTypeId = 2,
                    Name = "Mainstream For The Underground",
                    Abbreviation = "MFTU",
                    Slug = "MFTU"
                }, new Currency
                {
                    Id = 3687,
                    CurrencyTypeId = 2,
                    Name = "BitBall",
                    Abbreviation = "BTB",
                    Slug = "BTB"
                }, new Currency
                {
                    Id = 3413,
                    CurrencyTypeId = 2,
                    Name = "nDEX",
                    Abbreviation = "NDX",
                    Slug = "NDX"
                }, new Currency
                {
                    Id = 3599,
                    CurrencyTypeId = 2,
                    Name = "EtherInc",
                    Abbreviation = "ETI",
                    Slug = "ETI"
                }, new Currency
                {
                    Id = 3919,
                    CurrencyTypeId = 2,
                    Name = "Doge Token",
                    Abbreviation = "DOGET",
                    Slug = "DOGET"
                }, new Currency
                {
                    Id = 3678,
                    CurrencyTypeId = 2,
                    Name = "ICOBay",
                    Abbreviation = "IBT",
                    Slug = "IBT"
                }, new Currency
                {
                    Id = 3359,
                    CurrencyTypeId = 2,
                    Name = "WITChain",
                    Abbreviation = "WIT",
                    Slug = "WIT"
                }, new Currency
                {
                    Id = 3746,
                    CurrencyTypeId = 2,
                    Name = "electrumdark",
                    Abbreviation = "ELD",
                    Slug = "ELD"
                }, new Currency
                {
                    Id = 2932,
                    CurrencyTypeId = 2,
                    Name = "No BS Crypto",
                    Abbreviation = "NOBS",
                    Slug = "NOBS"
                }, new Currency
                {
                    Id = 3796,
                    CurrencyTypeId = 2,
                    Name = "MESG",
                    Abbreviation = "MESG",
                    Slug = "MESG"
                }, new Currency
                {
                    Id = 2475,
                    CurrencyTypeId = 2,
                    Name = "Garlicoin",
                    Abbreviation = "GRLC",
                    Slug = "GRLC"
                }, new Currency
                {
                    Id = 3181,
                    CurrencyTypeId = 2,
                    Name = "ShowHand",
                    Abbreviation = "HAND",
                    Slug = "HAND"
                }, new Currency
                {
                    Id = 3398,
                    CurrencyTypeId = 2,
                    Name = "SCRIV NETWORK",
                    Abbreviation = "SCRIV",
                    Slug = "SCRIV"
                }, new Currency
                {
                    Id = 3730,
                    CurrencyTypeId = 2,
                    Name = "The Currency Analytics",
                    Abbreviation = "TCAT",
                    Slug = "TCAT"
                }, new Currency
                {
                    Id = 3771,
                    CurrencyTypeId = 2,
                    Name = "EthereumX",
                    Abbreviation = "ETX",
                    Slug = "ETX"
                }, new Currency
                {
                    Id = 3056,
                    CurrencyTypeId = 2,
                    Name = "Thore Cash",
                    Abbreviation = "TCH",
                    Slug = "TCH"
                }, new Currency
                {
                    Id = 2489,
                    CurrencyTypeId = 2,
                    Name = "BitWhite",
                    Abbreviation = "BTW",
                    Slug = "BTW"
                }, new Currency
                {
                    Id = 3935,
                    CurrencyTypeId = 2,
                    Name = "Sparkpoint",
                    Abbreviation = "SRK",
                    Slug = "SRK"
                }, new Currency
                {
                    Id = 1474,
                    CurrencyTypeId = 2,
                    Name = "Eternity",
                    Abbreviation = "ENT",
                    Slug = "ENT"
                }, new Currency
                {
                    Id = 3810,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Gold Project",
                    Abbreviation = "ETGP",
                    Slug = "ETGP"
                }, new Currency
                {
                    Id = 2448,
                    CurrencyTypeId = 2,
                    Name = "SparksPay",
                    Abbreviation = "SPK",
                    Slug = "SPK"
                }, new Currency
                {
                    Id = 3889,
                    CurrencyTypeId = 2,
                    Name = "Natmin Pure Escrow",
                    Abbreviation = "NAT",
                    Slug = "NAT"
                }, new Currency
                {
                    Id = 3151,
                    CurrencyTypeId = 2,
                    Name = "Akroma",
                    Abbreviation = "AKA",
                    Slug = "AKA"
                }, new Currency
                {
                    Id = 3446,
                    CurrencyTypeId = 2,
                    Name = "Zenswap Network Token",
                    Abbreviation = "ZNT",
                    Slug = "ZNT"
                }, new Currency
                {
                    Id = 2583,
                    CurrencyTypeId = 2,
                    Name = "Octoin Coin",
                    Abbreviation = "OCC",
                    Slug = "OCC"
                }, new Currency
                {
                    Id = 3338,
                    CurrencyTypeId = 2,
                    Name = "PAXEX",
                    Abbreviation = "PAXEX",
                    Slug = "PAXEX"
                }, new Currency
                {
                    Id = 3509,
                    CurrencyTypeId = 2,
                    Name = "Provoco Token",
                    Abbreviation = "VOCO",
                    Slug = "VOCO"
                }, new Currency
                {
                    Id = 3476,
                    CurrencyTypeId = 2,
                    Name = "Italian Lira",
                    Abbreviation = "ITL",
                    Slug = "ITL"
                }, new Currency
                {
                    Id = 3184,
                    CurrencyTypeId = 2,
                    Name = "Gold Poker",
                    Abbreviation = "GPKR",
                    Slug = "GPKR"
                }, new Currency
                {
                    Id = 2635,
                    CurrencyTypeId = 2,
                    Name = "TokenDesk",
                    Abbreviation = "TDS",
                    Slug = "TDS"
                }, new Currency
                {
                    Id = 3739,
                    CurrencyTypeId = 2,
                    Name = "Constant",
                    Abbreviation = "CONST",
                    Slug = "CONST"
                }, new Currency
                {
                    Id = 3270,
                    CurrencyTypeId = 2,
                    Name = "e-Chat",
                    Abbreviation = "ECHT",
                    Slug = "ECHT"
                }, new Currency
                {
                    Id = 2147,
                    CurrencyTypeId = 2,
                    Name = "ELTCOIN",
                    Abbreviation = "ELTCOIN",
                    Slug = "ELTCOIN"
                }, new Currency
                {
                    Id = 3459,
                    CurrencyTypeId = 2,
                    Name = "GoHelpFund",
                    Abbreviation = "HELP",
                    Slug = "HELP"
                }, new Currency
                {
                    Id = 3501,
                    CurrencyTypeId = 2,
                    Name = "CryptoSoul",
                    Abbreviation = "SOUL",
                    Slug = "SOUL"
                }, new Currency
                {
                    Id = 2386,
                    CurrencyTypeId = 2,
                    Name = "KZ Cash",
                    Abbreviation = "KZC",
                    Slug = "KZC"
                }, new Currency
                {
                    Id = 3484,
                    CurrencyTypeId = 2,
                    Name = "Waletoken",
                    Abbreviation = "WTN",
                    Slug = "WTN"
                }, new Currency
                {
                    Id = 2461,
                    CurrencyTypeId = 2,
                    Name = "Peerguess",
                    Abbreviation = "GUESS",
                    Slug = "GUESS"
                }, new Currency
                {
                    Id = 3271,
                    CurrencyTypeId = 2,
                    Name = "Ether Kingdoms Token",
                    Abbreviation = "IMP",
                    Slug = "IMP"
                }, new Currency
                {
                    Id = 3460,
                    CurrencyTypeId = 2,
                    Name = "Bitcoinus",
                    Abbreviation = "BITS",
                    Slug = "BITS"
                }, new Currency
                {
                    Id = 3510,
                    CurrencyTypeId = 2,
                    Name = "Traid",
                    Abbreviation = "TRAID",
                    Slug = "TRAID"
                }, new Currency
                {
                    Id = 3787,
                    CurrencyTypeId = 2,
                    Name = "InnovativeBioresearchClassic",
                    Abbreviation = "INNBCL",
                    Slug = "INNBCL"
                }, new Currency
                {
                    Id = 2148,
                    CurrencyTypeId = 2,
                    Name = "Desire",
                    Abbreviation = "DSR",
                    Slug = "DSR"
                }, new Currency
                {
                    Id = 3610,
                    CurrencyTypeId = 2,
                    Name = "Micromines",
                    Abbreviation = "MICRO",
                    Slug = "MICRO"
                }, new Currency
                {
                    Id = 3172,
                    CurrencyTypeId = 2,
                    Name = "LogisCoin",
                    Abbreviation = "LGS",
                    Slug = "LGS"
                }, new Currency
                {
                    Id = 3841,
                    CurrencyTypeId = 2,
                    Name = "StellarPay",
                    Abbreviation = "XLB",
                    Slug = "XLB"
                }, new Currency
                {
                    Id = 2420,
                    CurrencyTypeId = 2,
                    Name = "Nitro",
                    Abbreviation = "NOX",
                    Slug = "NOX"
                }, new Currency
                {
                    Id = 3454,
                    CurrencyTypeId = 2,
                    Name = "Decentralized Asset Trading Platform",
                    Abbreviation = "DATP",
                    Slug = "DATP"
                }, new Currency
                {
                    Id = 3429,
                    CurrencyTypeId = 2,
                    Name = "Cyber Movie Chain",
                    Abbreviation = "CMCT",
                    Slug = "CMCT"
                }, new Currency
                {
                    Id = 3255,
                    CurrencyTypeId = 2,
                    Name = "CyberMusic",
                    Abbreviation = "CYMT",
                    Slug = "CYMT"
                }, new Currency
                {
                    Id = 3659,
                    CurrencyTypeId = 2,
                    Name = "QUINADS",
                    Abbreviation = "QUIN",
                    Slug = "QUIN"
                }, new Currency
                {
                    Id = 3468,
                    CurrencyTypeId = 2,
                    Name = "Fivebalance",
                    Abbreviation = "FBN",
                    Slug = "FBN"
                }, new Currency
                {
                    Id = 3131,
                    CurrencyTypeId = 2,
                    Name = "Thingschain",
                    Abbreviation = "TIC",
                    Slug = "TIC"
                }, new Currency
                {
                    Id = 2965,
                    CurrencyTypeId = 2,
                    Name = "VikkyToken",
                    Abbreviation = "VIKKY",
                    Slug = "VIKKY"
                }, new Currency
                {
                    Id = 3770,
                    CurrencyTypeId = 2,
                    Name = "CustomContractNetwork",
                    Abbreviation = "CCN",
                    Slug = "CCN"
                }, new Currency
                {
                    Id = 1897,
                    CurrencyTypeId = 2,
                    Name = "Bolenum",
                    Abbreviation = "BLN",
                    Slug = "BLN"
                }, new Currency
                {
                    Id = 3583,
                    CurrencyTypeId = 2,
                    Name = "Posscoin",
                    Abbreviation = "POSS",
                    Slug = "POSS"
                }, new Currency
                {
                    Id = 3216,
                    CurrencyTypeId = 2,
                    Name = "DeltaChain",
                    Abbreviation = "DELTA",
                    Slug = "DELTA"
                }, new Currency
                {
                    Id = 3521,
                    CurrencyTypeId = 2,
                    Name = "PAWS Fund",
                    Abbreviation = "PAWS",
                    Slug = "PAWS"
                }, new Currency
                {
                    Id = 3265,
                    CurrencyTypeId = 2,
                    Name = "Havy",
                    Abbreviation = "HAVY",
                    Slug = "HAVY"
                }, new Currency
                {
                    Id = 3397,
                    CurrencyTypeId = 2,
                    Name = "Neural Protocol",
                    Abbreviation = "NRP",
                    Slug = "NRP"
                }, new Currency
                {
                    Id = 2960,
                    CurrencyTypeId = 2,
                    Name = "Tourist Token",
                    Abbreviation = "TOTO",
                    Slug = "TOTO"
                }, new Currency
                {
                    Id = 3162,
                    CurrencyTypeId = 2,
                    Name = "YoloCash",
                    Abbreviation = "YLC",
                    Slug = "YLC"
                }, new Currency
                {
                    Id = 3219,
                    CurrencyTypeId = 2,
                    Name = "FUTURAX",
                    Abbreviation = "FTXT",
                    Slug = "FTXT"
                }, new Currency
                {
                    Id = 3254,
                    CurrencyTypeId = 2,
                    Name = "Mirai",
                    Abbreviation = "MRI",
                    Slug = "MRI"
                }, new Currency
                {
                    Id = 3512,
                    CurrencyTypeId = 2,
                    Name = "Alpha Coin",
                    Abbreviation = "APC",
                    Slug = "APC"
                }, new Currency
                {
                    Id = 3374,
                    CurrencyTypeId = 2,
                    Name = "Ragnarok",
                    Abbreviation = "RAGNA",
                    Slug = "RAGNA"
                }, new Currency
                {
                    Id = 3740,
                    CurrencyTypeId = 2,
                    Name = "Blacer Coin",
                    Abbreviation = "BLCR",
                    Slug = "BLCR"
                }, new Currency
                {
                    Id = 3769,
                    CurrencyTypeId = 2,
                    Name = "HashBX ",
                    Abbreviation = "HBX",
                    Slug = "HBX"
                }, new Currency
                {
                    Id = 1515,
                    CurrencyTypeId = 2,
                    Name = "iBank",
                    Abbreviation = "IBANK",
                    Slug = "IBANK"
                }, new Currency
                {
                    Id = 3317,
                    CurrencyTypeId = 2,
                    Name = "Cryptrust",
                    Abbreviation = "CTRT",
                    Slug = "CTRT"
                }, new Currency
                {
                    Id = 3263,
                    CurrencyTypeId = 2,
                    Name = "Dinero",
                    Abbreviation = "DIN",
                    Slug = "DIN"
                }, new Currency
                {
                    Id = 3222,
                    CurrencyTypeId = 2,
                    Name = "Bionic",
                    Abbreviation = "BNC",
                    Slug = "BNC"
                }, new Currency
                {
                    Id = 3444,
                    CurrencyTypeId = 2,
                    Name = "KUN",
                    Abbreviation = "KUN",
                    Slug = "KUN"
                }, new Currency
                {
                    Id = 3807,
                    CurrencyTypeId = 2,
                    Name = "LitecoinToken",
                    Abbreviation = "LTK",
                    Slug = "LTK"
                }, new Currency
                {
                    Id = 3804,
                    CurrencyTypeId = 2,
                    Name = "SpectrumNetwork",
                    Abbreviation = "SPEC",
                    Slug = "SPEC"
                }, new Currency
                {
                    Id = 3931,
                    CurrencyTypeId = 2,
                    Name = "Elementeum",
                    Abbreviation = "ELET",
                    Slug = "ELET"
                }, new Currency
                {
                    Id = 3424,
                    CurrencyTypeId = 2,
                    Name = "QYNO",
                    Abbreviation = "QNO",
                    Slug = "QNO"
                }, new Currency
                {
                    Id = 3886,
                    CurrencyTypeId = 2,
                    Name = "ICOCalendar.Today",
                    Abbreviation = "ICT",
                    Slug = "ICT"
                }, new Currency
                {
                    Id = 3749,
                    CurrencyTypeId = 2,
                    Name = "IceChain",
                    Abbreviation = "ICHX",
                    Slug = "ICHX"
                }, new Currency
                {
                    Id = 3287,
                    CurrencyTypeId = 2,
                    Name = "Abulaba",
                    Abbreviation = "AAA",
                    Slug = "AAA"
                }, new Currency
                {
                    Id = 3580,
                    CurrencyTypeId = 2,
                    Name = "Crystal Token",
                    Abbreviation = "CYL",
                    Slug = "CYL"
                }, new Currency
                {
                    Id = 1832,
                    CurrencyTypeId = 2,
                    Name = "HarmonyCoin",
                    Abbreviation = "HMC",
                    Slug = "HMC"
                }, new Currency
                {
                    Id = 67,
                    CurrencyTypeId = 2,
                    Name = "Unobtanium",
                    Abbreviation = "UNO",
                    Slug = "UNO"
                }, new Currency
                {
                    Id = 2585,
                    CurrencyTypeId = 2,
                    Name = "Centrality",
                    Abbreviation = "CENNZ",
                    Slug = "CENNZ"
                }, new Currency
                {
                    Id = 1408,
                    CurrencyTypeId = 2,
                    Name = "Iconomi",
                    Abbreviation = "ICN",
                    Slug = "ICN"
                }, new Currency
                {
                    Id = 2304,
                    CurrencyTypeId = 2,
                    Name = "DEW",
                    Abbreviation = "DEW",
                    Slug = "DEW"
                }, new Currency
                {
                    Id = 2371,
                    CurrencyTypeId = 2,
                    Name = "United Traders Token",
                    Abbreviation = "UTT",
                    Slug = "UTT"
                }, new Currency
                {
                    Id = 3225,
                    CurrencyTypeId = 2,
                    Name = "BitNewChain",
                    Abbreviation = "BTN",
                    Slug = "BTN"
                }, new Currency
                {
                    Id = 3354,
                    CurrencyTypeId = 2,
                    Name = "TRONCLASSIC",
                    Abbreviation = "TRXC",
                    Slug = "TRXC"
                }, new Currency
                {
                    Id = 1782,
                    CurrencyTypeId = 2,
                    Name = "Ecobit",
                    Abbreviation = "ECOB",
                    Slug = "ECOB"
                }, new Currency
                {
                    Id = 3072,
                    CurrencyTypeId = 2,
                    Name = "MassGrid",
                    Abbreviation = "MGD",
                    Slug = "MGD"
                }, new Currency
                {
                    Id = 2134,
                    CurrencyTypeId = 2,
                    Name = "Grid+",
                    Abbreviation = "GRID",
                    Slug = "GRID"
                }, new Currency
                {
                    Id = 212,
                    CurrencyTypeId = 2,
                    Name = "ECC",
                    Abbreviation = "ECC",
                    Slug = "ECC"
                }, new Currency
                {
                    Id = 2732,
                    CurrencyTypeId = 2,
                    Name = "Aston",
                    Abbreviation = "ATX",
                    Slug = "ATX"
                }, new Currency
                {
                    Id = 1454,
                    CurrencyTypeId = 2,
                    Name = "Lykke",
                    Abbreviation = "LKK",
                    Slug = "LKK"
                }, new Currency
                {
                    Id = 1963,
                    CurrencyTypeId = 2,
                    Name = "Credo",
                    Abbreviation = "CREDO",
                    Slug = "CREDO"
                }, new Currency
                {
                    Id = 141,
                    CurrencyTypeId = 2,
                    Name = "MintCoin",
                    Abbreviation = "MINT",
                    Slug = "MINT"
                }, new Currency
                {
                    Id = 3608,
                    CurrencyTypeId = 2,
                    Name = "Howdoo",
                    Abbreviation = "UDOO",
                    Slug = "UDOO"
                }, new Currency
                {
                    Id = 224,
                    CurrencyTypeId = 2,
                    Name = "FairCoin",
                    Abbreviation = "FAIR",
                    Slug = "FAIR"
                }, new Currency
                {
                    Id = 3054,
                    CurrencyTypeId = 2,
                    Name = "DACSEE",
                    Abbreviation = "DACS",
                    Slug = "DACS"
                }, new Currency
                {
                    Id = 89,
                    CurrencyTypeId = 2,
                    Name = "Mooncoin",
                    Abbreviation = "MOON",
                    Slug = "MOON"
                }, new Currency
                {
                    Id = 2384,
                    CurrencyTypeId = 2,
                    Name = "Vezt",
                    Abbreviation = "VZT",
                    Slug = "VZT"
                }, new Currency
                {
                    Id = 1244,
                    CurrencyTypeId = 2,
                    Name = "HiCoin",
                    Abbreviation = "XHI",
                    Slug = "XHI"
                }, new Currency
                {
                    Id = 161,
                    CurrencyTypeId = 2,
                    Name = "Pandacoin",
                    Abbreviation = "PND",
                    Slug = "PND"
                }, new Currency
                {
                    Id = 3407,
                    CurrencyTypeId = 2,
                    Name = "Ondori",
                    Abbreviation = "RSTR",
                    Slug = "RSTR"
                }, new Currency
                {
                    Id = 666,
                    CurrencyTypeId = 2,
                    Name = "Aurum Coin",
                    Abbreviation = "AU",
                    Slug = "AU"
                }, new Currency
                {
                    Id = 3605,
                    CurrencyTypeId = 2,
                    Name = "Vites",
                    Abbreviation = "VITES",
                    Slug = "VITES"
                }, new Currency
                {
                    Id = 3585,
                    CurrencyTypeId = 2,
                    Name = "WeShow Token",
                    Abbreviation = "WET",
                    Slug = "WET"
                }, new Currency
                {
                    Id = 2867,
                    CurrencyTypeId = 2,
                    Name = "Bittwatt",
                    Abbreviation = "BWT",
                    Slug = "BWT"
                }, new Currency
                {
                    Id = 2607,
                    CurrencyTypeId = 2,
                    Name = "AMLT",
                    Abbreviation = "AMLT",
                    Slug = "AMLT"
                }, new Currency
                {
                    Id = 2924,
                    CurrencyTypeId = 2,
                    Name = "FNKOS",
                    Abbreviation = "FNKOS",
                    Slug = "FNKOS"
                }, new Currency
                {
                    Id = 2722,
                    CurrencyTypeId = 2,
                    Name = "AC3",
                    Abbreviation = "AC3",
                    Slug = "AC3"
                }, new Currency
                {
                    Id = 3799,
                    CurrencyTypeId = 2,
                    Name = "SafeCoin",
                    Abbreviation = "SAFE",
                    Slug = "SAFE"
                }, new Currency
                {
                    Id = 2881,
                    CurrencyTypeId = 2,
                    Name = "Distributed Credit Chain",
                    Abbreviation = "DCC",
                    Slug = "DCC"
                }, new Currency
                {
                    Id = 2875,
                    CurrencyTypeId = 2,
                    Name = "ALAX",
                    Abbreviation = "ALX",
                    Slug = "ALX"
                }, new Currency
                {
                    Id = 1721,
                    CurrencyTypeId = 2,
                    Name = "Mysterium",
                    Abbreviation = "MYST",
                    Slug = "MYST"
                }, new Currency
                {
                    Id = 1109,
                    CurrencyTypeId = 2,
                    Name = "Elite",
                    Abbreviation = "1337",
                    Slug = "1337"
                }, new Currency
                {
                    Id = 298,
                    CurrencyTypeId = 2,
                    Name = "NewYorkCoin",
                    Abbreviation = "NYC",
                    Slug = "NYC"
                }, new Currency
                {
                    Id = 3092,
                    CurrencyTypeId = 2,
                    Name = "Nuggets",
                    Abbreviation = "NUG",
                    Slug = "NUG"
                }, new Currency
                {
                    Id = 2993,
                    CurrencyTypeId = 2,
                    Name = "HorusPay",
                    Abbreviation = "HORUS",
                    Slug = "HORUS"
                }, new Currency
                {
                    Id = 1238,
                    CurrencyTypeId = 2,
                    Name = "Espers",
                    Abbreviation = "ESP",
                    Slug = "ESP"
                }, new Currency
                {
                    Id = 1819,
                    CurrencyTypeId = 2,
                    Name = "Starta",
                    Abbreviation = "STA",
                    Slug = "STA"
                }, new Currency
                {
                    Id = 2211,
                    CurrencyTypeId = 2,
                    Name = "Bodhi",
                    Abbreviation = "BOT",
                    Slug = "BOT"
                }, new Currency
                {
                    Id = 3728,
                    CurrencyTypeId = 2,
                    Name = "Halo Platform",
                    Abbreviation = "HALO",
                    Slug = "HALO"
                }, new Currency
                {
                    Id = 3584,
                    CurrencyTypeId = 2,
                    Name = "TV-TWO",
                    Abbreviation = "TTV",
                    Slug = "TTV"
                }, new Currency
                {
                    Id = 2954,
                    CurrencyTypeId = 2,
                    Name = "wys Token",
                    Abbreviation = "WYS",
                    Slug = "WYS"
                }, new Currency
                {
                    Id = 2877,
                    CurrencyTypeId = 2,
                    Name = "Bodhi [ETH]",
                    Abbreviation = "BOE",
                    Slug = "BOE"
                }, new Currency
                {
                    Id = 2909,
                    CurrencyTypeId = 2,
                    Name = "LikeCoin",
                    Abbreviation = "LIKE",
                    Slug = "LIKE"
                }, new Currency
                {
                    Id = 260,
                    CurrencyTypeId = 2,
                    Name = "PetroDollar",
                    Abbreviation = "XPD",
                    Slug = "XPD"
                }, new Currency
                {
                    Id = 2543,
                    CurrencyTypeId = 2,
                    Name = "COPYTRACK",
                    Abbreviation = "CPY",
                    Slug = "CPY"
                }, new Currency
                {
                    Id = 2031,
                    CurrencyTypeId = 2,
                    Name = "Hubii Network",
                    Abbreviation = "HBT",
                    Slug = "HBT"
                }, new Currency
                {
                    Id = 1595,
                    CurrencyTypeId = 2,
                    Name = "Soarcoin",
                    Abbreviation = "SOAR",
                    Slug = "SOAR"
                }, new Currency
                {
                    Id = 2377,
                    CurrencyTypeId = 2,
                    Name = "Leverj",
                    Abbreviation = "LEV",
                    Slug = "LEV"
                }, new Currency
                {
                    Id = 2208,
                    CurrencyTypeId = 2,
                    Name = "EncrypGen",
                    Abbreviation = "DNA",
                    Slug = "DNA"
                }, new Currency
                {
                    Id = 3426,
                    CurrencyTypeId = 2,
                    Name = "Incodium",
                    Abbreviation = "INCO",
                    Slug = "INCO"
                }, new Currency
                {
                    Id = 1611,
                    CurrencyTypeId = 2,
                    Name = "DubaiCoin",
                    Abbreviation = "DBIX",
                    Slug = "DBIX"
                }, new Currency
                {
                    Id = 2687,
                    CurrencyTypeId = 2,
                    Name = "Proxeus",
                    Abbreviation = "XES",
                    Slug = "XES"
                }, new Currency
                {
                    Id = 3336,
                    CurrencyTypeId = 2,
                    Name = "IQeon",
                    Abbreviation = "IQN",
                    Slug = "IQN"
                }, new Currency
                {
                    Id = 1375,
                    CurrencyTypeId = 2,
                    Name = "Golfcoin",
                    Abbreviation = "GOLF",
                    Slug = "GOLF"
                }, new Currency
                {
                    Id = 1968,
                    CurrencyTypeId = 2,
                    Name = "XPA",
                    Abbreviation = "XPA",
                    Slug = "XPA"
                }, new Currency
                {
                    Id = 2899,
                    CurrencyTypeId = 2,
                    Name = "Thrive Token",
                    Abbreviation = "THRT",
                    Slug = "THRT"
                }, new Currency
                {
                    Id = 3089,
                    CurrencyTypeId = 2,
                    Name = "AVINOC",
                    Abbreviation = "AVINOC",
                    Slug = "AVINOC"
                }, new Currency
                {
                    Id = 3274,
                    CurrencyTypeId = 2,
                    Name = "Carboneum [C8] Token",
                    Abbreviation = "C8",
                    Slug = "C8"
                }, new Currency
                {
                    Id = 2166,
                    CurrencyTypeId = 2,
                    Name = "Ties.DB",
                    Abbreviation = "TIE",
                    Slug = "TIE"
                }, new Currency
                {
                    Id = 2030,
                    CurrencyTypeId = 2,
                    Name = "REAL",
                    Abbreviation = "REAL",
                    Slug = "REAL"
                }, new Currency
                {
                    Id = 1503,
                    CurrencyTypeId = 2,
                    Name = "Darcrus",
                    Abbreviation = "DAR",
                    Slug = "DAR"
                }, new Currency
                {
                    Id = 2260,
                    CurrencyTypeId = 2,
                    Name = "Bulwark",
                    Abbreviation = "BWK",
                    Slug = "BWK"
                }, new Currency
                {
                    Id = 1308,
                    CurrencyTypeId = 2,
                    Name = "HEAT",
                    Abbreviation = "HEAT",
                    Slug = "HEAT"
                }, new Currency
                {
                    Id = 3471,
                    CurrencyTypeId = 2,
                    Name = "RoBET",
                    Abbreviation = "ROBET",
                    Slug = "ROBET"
                }, new Currency
                {
                    Id = 53,
                    CurrencyTypeId = 2,
                    Name = "Quark",
                    Abbreviation = "QRK",
                    Slug = "QRK"
                }, new Currency
                {
                    Id = 80,
                    CurrencyTypeId = 2,
                    Name = "Orbitcoin",
                    Abbreviation = "ORB",
                    Slug = "ORB"
                }, new Currency
                {
                    Id = 3519,
                    CurrencyTypeId = 2,
                    Name = "Breezecoin",
                    Abbreviation = "BRZC",
                    Slug = "BRZC"
                }, new Currency
                {
                    Id = 2374,
                    CurrencyTypeId = 2,
                    Name = "BitDegree",
                    Abbreviation = "BDG",
                    Slug = "BDG"
                }, new Currency
                {
                    Id = 168,
                    CurrencyTypeId = 2,
                    Name = "Uniform Fiscal Object",
                    Abbreviation = "UFO",
                    Slug = "UFO"
                }, new Currency
                {
                    Id = 2104,
                    CurrencyTypeId = 2,
                    Name = "iEthereum",
                    Abbreviation = "IETH",
                    Slug = "IETH"
                }, new Currency
                {
                    Id = 93,
                    CurrencyTypeId = 2,
                    Name = "42-coin",
                    Abbreviation = "42",
                    Slug = "42"
                }, new Currency
                {
                    Id = 1967,
                    CurrencyTypeId = 2,
                    Name = "Indorse Token",
                    Abbreviation = "IND",
                    Slug = "IND"
                }, new Currency
                {
                    Id = 2378,
                    CurrencyTypeId = 2,
                    Name = "Karma",
                    Abbreviation = "KRM",
                    Slug = "KRM"
                },
                new Currency
                {
                    Id = 39,
                    CurrencyTypeId = 2,
                    Name = "Terracoin",
                    Abbreviation = "TRC",
                    Slug = "TRC"
                },
                new Currency
                {
                    Id = 234,
                    CurrencyTypeId = 2,
                    Name = "e-Gulden",
                    Abbreviation = "EFL",
                    Slug = "EFL"
                },
                new Currency
                {
                    Id = 3161,
                    CurrencyTypeId = 2,
                    Name = "savedroid",
                    Abbreviation = "SVD",
                    Slug = "SVD"
                }, new Currency
                {
                    Id = 1988,
                    CurrencyTypeId = 2,
                    Name = "Lampix",
                    Abbreviation = "PIX",
                    Slug = "PIX"
                }, new Currency
                {
                    Id = 3492,
                    CurrencyTypeId = 2,
                    Name = "Vetri",
                    Abbreviation = "VLD",
                    Slug = "VLD"
                }, new Currency
                {
                    Id = 2589,
                    CurrencyTypeId = 2,
                    Name = "Guaranteed Ethurance Token Extra",
                    Abbreviation = "GETX",
                    Slug = "GETX"
                }, new Currency
                {
                    Id = 128,
                    CurrencyTypeId = 2,
                    Name = "Maxcoin",
                    Abbreviation = "MAX",
                    Slug = "MAX"
                }, new Currency
                {
                    Id = 3406,
                    CurrencyTypeId = 2,
                    Name = "Block-Chain.com",
                    Abbreviation = "BC",
                    Slug = "BC"
                }, new Currency
                {
                    Id = 2368,
                    CurrencyTypeId = 2,
                    Name = "REBL",
                    Abbreviation = "REBL",
                    Slug = "REBL"
                }, new Currency
                {
                    Id = 1686,
                    CurrencyTypeId = 2,
                    Name = "EquiTrader",
                    Abbreviation = "EQT",
                    Slug = "EQT"
                }, new Currency
                {
                    Id = 2872,
                    CurrencyTypeId = 2,
                    Name = "EnergiToken",
                    Abbreviation = "ETK",
                    Slug = "ETK"
                }, new Currency
                {
                    Id = 2050,
                    CurrencyTypeId = 2,
                    Name = "Swisscoin",
                    Abbreviation = "SIC",
                    Slug = "SIC"
                }, new Currency
                {
                    Id = 1019,
                    CurrencyTypeId = 2,
                    Name = "Manna",
                    Abbreviation = "MANNA",
                    Slug = "MANNA"
                }, new Currency
                {
                    Id = 3592,
                    CurrencyTypeId = 2,
                    Name = "Coin Lion",
                    Abbreviation = "LION",
                    Slug = "LION"
                }, new Currency
                {
                    Id = 3487,
                    CurrencyTypeId = 2,
                    Name = "Pedity",
                    Abbreviation = "PEDI",
                    Slug = "PEDI"
                }, new Currency
                {
                    Id = 3706,
                    CurrencyTypeId = 2,
                    Name = "ALBOS",
                    Abbreviation = "ALB",
                    Slug = "ALB"
                }, new Currency
                {
                    Id = 2012,
                    CurrencyTypeId = 2,
                    Name = "Voise",
                    Abbreviation = "VOISE",
                    Slug = "VOISE"
                }, new Currency
                {
                    Id = 2333,
                    CurrencyTypeId = 2,
                    Name = "FidentiaX",
                    Abbreviation = "FDX",
                    Slug = "FDX"
                }, new Currency
                {
                    Id = 3611,
                    CurrencyTypeId = 2,
                    Name = "Noir",
                    Abbreviation = "NOR",
                    Slug = "NOR"
                }, new Currency
                {
                    Id = 2854,
                    CurrencyTypeId = 2,
                    Name = "PikcioChain",
                    Abbreviation = "PKC",
                    Slug = "PKC"
                }, new Currency
                {
                    Id = 2513,
                    CurrencyTypeId = 2,
                    Name = "GoldMint",
                    Abbreviation = "MNTP",
                    Slug = "MNTP"
                }, new Currency
                {
                    Id = 1480,
                    CurrencyTypeId = 2,
                    Name = "Golos",
                    Abbreviation = "GOLOS",
                    Slug = "GOLOS"
                }, new Currency
                {
                    Id = 799,
                    CurrencyTypeId = 2,
                    Name = "SmileyCoin",
                    Abbreviation = "SMLY",
                    Slug = "SMLY"
                }, new Currency
                {
                    Id = 2701,
                    CurrencyTypeId = 2,
                    Name = "TrustNote",
                    Abbreviation = "TTT",
                    Slug = "TTT"
                }, new Currency
                {
                    Id = 2833,
                    CurrencyTypeId = 2,
                    Name = "Ivy",
                    Abbreviation = "IVY",
                    Slug = "IVY"
                }, new Currency
                {
                    Id = 2753,
                    CurrencyTypeId = 2,
                    Name = "Colu Local Network",
                    Abbreviation = "CLN",
                    Slug = "CLN"
                }, new Currency
                {
                    Id = 1510,
                    CurrencyTypeId = 2,
                    Name = "CryptoCarbon",
                    Abbreviation = "CCRB",
                    Slug = "CCRB"
                }, new Currency
                {
                    Id = 2352,
                    CurrencyTypeId = 2,
                    Name = "Coinlancer",
                    Abbreviation = "CL",
                    Slug = "CL"
                }, new Currency
                {
                    Id = 1962,
                    CurrencyTypeId = 2,
                    Name = "BuzzCoin",
                    Abbreviation = "BUZZ",
                    Slug = "BUZZ"
                }, new Currency
                {
                    Id = 205,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Scrypt",
                    Abbreviation = "BTCS",
                    Slug = "BTCS"
                }, new Currency
                {
                    Id = 148,
                    CurrencyTypeId = 2,
                    Name = "Auroracoin",
                    Abbreviation = "AUR",
                    Slug = "AUR"
                }, new Currency
                {
                    Id = 2425,
                    CurrencyTypeId = 2,
                    Name = "Global Awards Token",
                    Abbreviation = "GAT",
                    Slug = "GAT"
                }, new Currency
                {
                    Id = 29,
                    CurrencyTypeId = 2,
                    Name = "WorldCoin",
                    Abbreviation = "WDC",
                    Slug = "WDC"
                }, new Currency
                {
                    Id = 1931,
                    CurrencyTypeId = 2,
                    Name = "Opus",
                    Abbreviation = "OPT",
                    Slug = "OPT"
                }, new Currency
                {
                    Id = 1123,
                    CurrencyTypeId = 2,
                    Name = "OBITS",
                    Abbreviation = "OBITS",
                    Slug = "OBITS"
                }, new Currency
                {
                    Id = 2555,
                    CurrencyTypeId = 2,
                    Name = "Sether",
                    Abbreviation = "SETH",
                    Slug = "SETH"
                }, new Currency
                {
                    Id = 1852,
                    CurrencyTypeId = 2,
                    Name = "KekCoin",
                    Abbreviation = "KEK",
                    Slug = "KEK"
                }, new Currency
                {
                    Id = 360,
                    CurrencyTypeId = 2,
                    Name = "Motocoin",
                    Abbreviation = "MOTO",
                    Slug = "MOTO"
                }, new Currency
                {
                    Id = 1917,
                    CurrencyTypeId = 2,
                    Name = "bitqy",
                    Abbreviation = "BQ",
                    Slug = "BQ"
                }, new Currency
                {
                    Id = 506,
                    CurrencyTypeId = 2,
                    Name = "CannabisCoin",
                    Abbreviation = "CANN",
                    Slug = "CANN"
                }, new Currency
                {
                    Id = 2422,
                    CurrencyTypeId = 2,
                    Name = "IDEX Membership",
                    Abbreviation = "IDXM",
                    Slug = "IDXM"
                }, new Currency
                {
                    Id = 3291,
                    CurrencyTypeId = 2,
                    Name = "Bettex Coin",
                    Abbreviation = "BTXC",
                    Slug = "BTXC"
                }, new Currency
                {
                    Id = 948,
                    CurrencyTypeId = 2,
                    Name = "AudioCoin",
                    Abbreviation = "ADC",
                    Slug = "ADC"
                }, new Currency
                {
                    Id = 1995,
                    CurrencyTypeId = 2,
                    Name = "Target Coin",
                    Abbreviation = "TGT",
                    Slug = "TGT"
                }, new Currency
                {
                    Id = 3003,
                    CurrencyTypeId = 2,
                    Name = "White Standard",
                    Abbreviation = "WSD",
                    Slug = "WSD"
                }, new Currency
                {
                    Id = 525,
                    CurrencyTypeId = 2,
                    Name = "HyperStake",
                    Abbreviation = "HYP",
                    Slug = "HYP"
                }, new Currency
                {
                    Id = 1466,
                    CurrencyTypeId = 2,
                    Name = "Hush",
                    Abbreviation = "HUSH",
                    Slug = "HUSH"
                }, new Currency
                {
                    Id = 1915,
                    CurrencyTypeId = 2,
                    Name = "AdCoin",
                    Abbreviation = "ACC",
                    Slug = "ACC"
                }, new Currency
                {
                    Id = 2436,
                    CurrencyTypeId = 2,
                    Name = "RefToken",
                    Abbreviation = "REF",
                    Slug = "REF"
                }, new Currency
                {
                    Id = 502,
                    CurrencyTypeId = 2,
                    Name = "Carboncoin",
                    Abbreviation = "CARBON",
                    Slug = "CARBON"
                }, new Currency
                {
                    Id = 2521,
                    CurrencyTypeId = 2,
                    Name = "BioCoin",
                    Abbreviation = "BIO",
                    Slug = "BIO"
                }, new Currency
                {
                    Id = 199,
                    CurrencyTypeId = 2,
                    Name = "Skeincoin",
                    Abbreviation = "SKC",
                    Slug = "SKC"
                }, new Currency
                {
                    Id = 3465,
                    CurrencyTypeId = 2,
                    Name = "Alt.Estate token",
                    Abbreviation = "ALT",
                    Slug = "ALT"
                }, new Currency
                {
                    Id = 1699,
                    CurrencyTypeId = 2,
                    Name = "Ethbits",
                    Abbreviation = "ETBS",
                    Slug = "ETBS"
                }, new Currency
                {
                    Id = 2703,
                    CurrencyTypeId = 2,
                    Name = "BetterBetting",
                    Abbreviation = "BETR",
                    Slug = "BETR"
                }, new Currency
                {
                    Id = 3755,
                    CurrencyTypeId = 2,
                    Name = "Moneynet",
                    Abbreviation = "MNC",
                    Slug = "MNC"
                }, new Currency
                {
                    Id = 501,
                    CurrencyTypeId = 2,
                    Name = "Cryptonite",
                    Abbreviation = "XCN",
                    Slug = "XCN"
                }, new Currency
                {
                    Id = 629,
                    CurrencyTypeId = 2,
                    Name = "Magi",
                    Abbreviation = "XMG",
                    Slug = "XMG"
                }, new Currency
                {
                    Id = 2980,
                    CurrencyTypeId = 2,
                    Name = "WABnetwork",
                    Abbreviation = "WAB",
                    Slug = "WAB"
                }, new Currency
                {
                    Id = 2598,
                    CurrencyTypeId = 2,
                    Name = "Banyan Network",
                    Abbreviation = "BBN",
                    Slug = "BBN"
                }, new Currency
                {
                    Id = 3078,
                    CurrencyTypeId = 2,
                    Name = "Kind Ads Token",
                    Abbreviation = "KIND",
                    Slug = "KIND"
                }, new Currency
                {
                    Id = 2672,
                    CurrencyTypeId = 2,
                    Name = "SRCOIN",
                    Abbreviation = "SRCOIN",
                    Slug = "SRCOIN"
                }, new Currency
                {
                    Id = 32,
                    CurrencyTypeId = 2,
                    Name = "Freicoin",
                    Abbreviation = "FRC",
                    Slug = "FRC"
                }, new Currency
                {
                    Id = 853,
                    CurrencyTypeId = 2,
                    Name = "LiteDoge",
                    Abbreviation = "LDOGE",
                    Slug = "LDOGE"
                }, new Currency
                {
                    Id = 3241,
                    CurrencyTypeId = 2,
                    Name = "Knoxstertoken",
                    Abbreviation = "FKX",
                    Slug = "FKX"
                }, new Currency
                {
                    Id = 2255,
                    CurrencyTypeId = 2,
                    Name = "Social Send",
                    Abbreviation = "SEND",
                    Slug = "SEND"
                }, new Currency
                {
                    Id = 2926,
                    CurrencyTypeId = 2,
                    Name = "PRASM",
                    Abbreviation = "PSM",
                    Slug = "PSM"
                }, new Currency
                {
                    Id = 2015,
                    CurrencyTypeId = 2,
                    Name = "ATMChain",
                    Abbreviation = "ATM",
                    Slug = "ATM"
                }, new Currency
                {
                    Id = 175,
                    CurrencyTypeId = 2,
                    Name = "Photon",
                    Abbreviation = "PHO",
                    Slug = "PHO"
                }, new Currency
                {
                    Id = 2491,
                    CurrencyTypeId = 2,
                    Name = "Travelflex",
                    Abbreviation = "TRF",
                    Slug = "TRF"
                }, new Currency
                {
                    Id = 1882,
                    CurrencyTypeId = 2,
                    Name = "BlockCAT",
                    Abbreviation = "CAT",
                    Slug = "CAT"
                }, new Currency
                {
                    Id = 2628,
                    CurrencyTypeId = 2,
                    Name = "Rentberry",
                    Abbreviation = "BERRY",
                    Slug = "BERRY"
                }, new Currency
                {
                    Id = 30,
                    CurrencyTypeId = 2,
                    Name = "BitBar",
                    Abbreviation = "BTB",
                    Slug = "BTB"
                }, new Currency
                {
                    Id = 3700,
                    CurrencyTypeId = 2,
                    Name = "Centauri",
                    Abbreviation = "CTX",
                    Slug = "CTX"
                }, new Currency
                {
                    Id = 1961,
                    CurrencyTypeId = 2,
                    Name = "imbrex",
                    Abbreviation = "REX",
                    Slug = "REX"
                }, new Currency
                {
                    Id = 1969,
                    CurrencyTypeId = 2,
                    Name = "Sociall",
                    Abbreviation = "SCL",
                    Slug = "SCL"
                }, new Currency
                {
                    Id = 990,
                    CurrencyTypeId = 2,
                    Name = "Bitzeny",
                    Abbreviation = "ZNY",
                    Slug = "ZNY"
                }, new Currency
                {
                    Id = 275,
                    CurrencyTypeId = 2,
                    Name = "PopularCoin",
                    Abbreviation = "POP",
                    Slug = "POP"
                }, new Currency
                {
                    Id = 2625,
                    CurrencyTypeId = 2,
                    Name = "VIT",
                    Abbreviation = "VIT",
                    Slug = "VIT"
                }, new Currency
                {
                    Id = 2334,
                    CurrencyTypeId = 2,
                    Name = "BitClave",
                    Abbreviation = "CAT",
                    Slug = "CAT"
                }, new Currency
                {
                    Id = 38,
                    CurrencyTypeId = 2,
                    Name = "Megacoin",
                    Abbreviation = "MEC",
                    Slug = "MEC"
                }, new Currency
                {
                    Id = 3206,
                    CurrencyTypeId = 2,
                    Name = "Mithril Ore",
                    Abbreviation = "MORE",
                    Slug = "MORE"
                }, new Currency
                {
                    Id = 1495,
                    CurrencyTypeId = 2,
                    Name = "PoSW Coin",
                    Abbreviation = "POSW",
                    Slug = "POSW"
                }, new Currency
                {
                    Id = 3467,
                    CurrencyTypeId = 2,
                    Name = "Helium",
                    Abbreviation = "HLM",
                    Slug = "HLM"
                }, new Currency
                {
                    Id = 1833,
                    CurrencyTypeId = 2,
                    Name = "ToaCoin",
                    Abbreviation = "TOA",
                    Slug = "TOA"
                }, new Currency
                {
                    Id = 31,
                    CurrencyTypeId = 2,
                    Name = "Ixcoin",
                    Abbreviation = "IXC",
                    Slug = "IXC"
                }, new Currency
                {
                    Id = 2617,
                    CurrencyTypeId = 2,
                    Name = "IP Exchange",
                    Abbreviation = "IPSX",
                    Slug = "IPSX"
                }, new Currency
                {
                    Id = 2372,
                    CurrencyTypeId = 2,
                    Name = "CDX Network",
                    Abbreviation = "CDX",
                    Slug = "CDX"
                }, new Currency
                {
                    Id = 3047,
                    CurrencyTypeId = 2,
                    Name = "UltraNote Coin",
                    Abbreviation = "XUN",
                    Slug = "XUN"
                }, new Currency
                {
                    Id = 3335,
                    CurrencyTypeId = 2,
                    Name = "Shard",
                    Abbreviation = "SHARD",
                    Slug = "SHARD"
                }, new Currency
                {
                    Id = 1878,
                    CurrencyTypeId = 2,
                    Name = "Shadow Token",
                    Abbreviation = "SHDW",
                    Slug = "SHDW"
                }, new Currency
                {
                    Id = 3751,
                    CurrencyTypeId = 2,
                    Name = "Stakinglab",
                    Abbreviation = "LABX",
                    Slug = "LABX"
                }, new Currency
                {
                    Id = 151,
                    CurrencyTypeId = 2,
                    Name = "Pesetacoin",
                    Abbreviation = "PTC",
                    Slug = "PTC"
                }, new Currency
                {
                    Id = 1141,
                    CurrencyTypeId = 2,
                    Name = "Moin",
                    Abbreviation = "MOIN",
                    Slug = "MOIN"
                }, new Currency
                {
                    Id = 3370,
                    CurrencyTypeId = 2,
                    Name = "Nerves",
                    Abbreviation = "NER",
                    Slug = "NER"
                }, new Currency
                {
                    Id = 43,
                    CurrencyTypeId = 2,
                    Name = "Anoncoin",
                    Abbreviation = "ANC",
                    Slug = "ANC"
                }, new Currency
                {
                    Id = 2616,
                    CurrencyTypeId = 2,
                    Name = "Stipend",
                    Abbreviation = "SPD",
                    Slug = "SPD"
                }, new Currency
                {
                    Id = 2523,
                    CurrencyTypeId = 2,
                    Name = "Tigereum",
                    Abbreviation = "TIG",
                    Slug = "TIG"
                }, new Currency
                {
                    Id = 35,
                    CurrencyTypeId = 2,
                    Name = "Argentum",
                    Abbreviation = "ARG",
                    Slug = "ARG"
                }, new Currency
                {
                    Id = 3180,
                    CurrencyTypeId = 2,
                    Name = "Compound Coin",
                    Abbreviation = "COMP",
                    Slug = "COMP"
                }, new Currency
                {
                    Id = 1605,
                    CurrencyTypeId = 2,
                    Name = "Universe",
                    Abbreviation = "UNI",
                    Slug = "UNI"
                }, new Currency
                {
                    Id = 3361,
                    CurrencyTypeId = 2,
                    Name = "Webchain",
                    Abbreviation = "WEB",
                    Slug = "WEB"
                }, new Currency
                {
                    Id = 3933,
                    CurrencyTypeId = 2,
                    Name = "SwiftCash",
                    Abbreviation = "SWIFT",
                    Slug = "SWIFT"
                }, new Currency
                {
                    Id = 1279,
                    CurrencyTypeId = 2,
                    Name = "PWR Coin",
                    Abbreviation = "PWR",
                    Slug = "PWR"
                }, new Currency
                {
                    Id = 594,
                    CurrencyTypeId = 2,
                    Name = "BunnyCoin",
                    Abbreviation = "BUN",
                    Slug = "BUN"
                }, new Currency
                {
                    Id = 1195,
                    CurrencyTypeId = 2,
                    Name = "HOdlcoin",
                    Abbreviation = "HODL",
                    Slug = "HODL"
                }, new Currency
                {
                    Id = 3633,
                    CurrencyTypeId = 2,
                    Name = "BitGuild PLAT",
                    Abbreviation = "PLAT",
                    Slug = "PLAT"
                }, new Currency
                {
                    Id = 3045,
                    CurrencyTypeId = 2,
                    Name = "OPCoinX",
                    Abbreviation = "OPCX",
                    Slug = "OPCX"
                }, new Currency
                {
                    Id = 894,
                    CurrencyTypeId = 2,
                    Name = "Neutron",
                    Abbreviation = "NTRN",
                    Slug = "NTRN"
                }, new Currency
                {
                    Id = 1629,
                    CurrencyTypeId = 2,
                    Name = "Zennies",
                    Abbreviation = "ZENI",
                    Slug = "ZENI"
                }, new Currency
                {
                    Id = 120,
                    CurrencyTypeId = 2,
                    Name = "Nyancoin",
                    Abbreviation = "NYAN",
                    Slug = "NYAN"
                }, new Currency
                {
                    Id = 1066,
                    CurrencyTypeId = 2,
                    Name = "Pakcoin",
                    Abbreviation = "PAK",
                    Slug = "PAK"
                }, new Currency
                {
                    Id = 290,
                    CurrencyTypeId = 2,
                    Name = "BlueCoin",
                    Abbreviation = "BLU",
                    Slug = "BLU"
                }, new Currency
                {
                    Id = 1522,
                    CurrencyTypeId = 2,
                    Name = "FirstCoin",
                    Abbreviation = "FRST",
                    Slug = "FRST"
                }, new Currency
                {
                    Id = 2622,
                    CurrencyTypeId = 2,
                    Name = "ClearCoin",
                    Abbreviation = "XCLR",
                    Slug = "XCLR"
                }, new Currency
                {
                    Id = 644,
                    CurrencyTypeId = 2,
                    Name = "GlobalBoost-Y",
                    Abbreviation = "BSTY",
                    Slug = "BSTY"
                }, new Currency
                {
                    Id = 389,
                    CurrencyTypeId = 2,
                    Name = "Startcoin",
                    Abbreviation = "START",
                    Slug = "START"
                }, new Currency
                {
                    Id = 2005,
                    CurrencyTypeId = 2,
                    Name = "Obsidian",
                    Abbreviation = "ODN",
                    Slug = "ODN"
                }, new Currency
                {
                    Id = 1175,
                    CurrencyTypeId = 2,
                    Name = "Rubies",
                    Abbreviation = "RBIES",
                    Slug = "RBIES"
                }, new Currency
                {
                    Id = 2612,
                    CurrencyTypeId = 2,
                    Name = "BitRent",
                    Abbreviation = "RNTB",
                    Slug = "RNTB"
                }, new Currency
                {
                    Id = 2288,
                    CurrencyTypeId = 2,
                    Name = "Worldcore",
                    Abbreviation = "WRC",
                    Slug = "WRC"
                }, new Currency
                {
                    Id = 2664,
                    CurrencyTypeId = 2,
                    Name = "CryCash",
                    Abbreviation = "CRC",
                    Slug = "CRC"
                }, new Currency
                {
                    Id = 3621,
                    CurrencyTypeId = 2,
                    Name = "BitNautic Token",
                    Abbreviation = "BTNT",
                    Slug = "BTNT"
                }, new Currency
                {
                    Id = 2695,
                    CurrencyTypeId = 2,
                    Name = "VeriME",
                    Abbreviation = "VME",
                    Slug = "VME"
                }, new Currency
                {
                    Id = 1951,
                    CurrencyTypeId = 2,
                    Name = "Vsync",
                    Abbreviation = "VSX",
                    Slug = "VSX"
                }, new Currency
                {
                    Id = 3006,
                    CurrencyTypeId = 2,
                    Name = "Niobio Cash",
                    Abbreviation = "NBR",
                    Slug = "NBR"
                }, new Currency
                {
                    Id = 1582,
                    CurrencyTypeId = 2,
                    Name = "Netko",
                    Abbreviation = "NETKO",
                    Slug = "NETKO"
                }, new Currency
                {
                    Id = 719,
                    CurrencyTypeId = 2,
                    Name = "TittieCoin",
                    Abbreviation = "TIT",
                    Slug = "TIT"
                }, new Currency
                {
                    Id = 911,
                    CurrencyTypeId = 2,
                    Name = "Advanced Internet Blocks",
                    Abbreviation = "AIB",
                    Slug = "AIB"
                }, new Currency
                {
                    Id = 3480,
                    CurrencyTypeId = 2,
                    Name = "StrongHands Masternode",
                    Abbreviation = "SHMN",
                    Slug = "SHMN"
                }, new Currency
                {
                    Id = 181,
                    CurrencyTypeId = 2,
                    Name = "Zeitcoin",
                    Abbreviation = "ZEIT",
                    Slug = "ZEIT"
                }, new Currency
                {
                    Id = 654,
                    CurrencyTypeId = 2,
                    Name = "DigitalPrice",
                    Abbreviation = "DP",
                    Slug = "DP"
                }, new Currency
                {
                    Id = 1247,
                    CurrencyTypeId = 2,
                    Name = "AquariusCoin",
                    Abbreviation = "ARCO",
                    Slug = "ARCO"
                }, new Currency
                {
                    Id = 2411,
                    CurrencyTypeId = 2,
                    Name = "Galactrum",
                    Abbreviation = "ORE",
                    Slug = "ORE"
                }, new Currency
                {
                    Id = 638,
                    CurrencyTypeId = 2,
                    Name = "Trollcoin",
                    Abbreviation = "TROLL",
                    Slug = "TROLL"
                }, new Currency
                {
                    Id = 3603,
                    CurrencyTypeId = 2,
                    Name = "Menlo One",
                    Abbreviation = "ONE",
                    Slug = "ONE"
                }, new Currency
                {
                    Id = 2683,
                    CurrencyTypeId = 2,
                    Name = "TrakInvest",
                    Abbreviation = "TRAK",
                    Slug = "TRAK"
                }, new Currency
                {
                    Id = 3436,
                    CurrencyTypeId = 2,
                    Name = "SIMDAQ",
                    Abbreviation = "SMQ",
                    Slug = "SMQ"
                }, new Currency
                {
                    Id = 1004,
                    CurrencyTypeId = 2,
                    Name = "Helleniccoin",
                    Abbreviation = "HNC",
                    Slug = "HNC"
                }, new Currency
                {
                    Id = 61,
                    CurrencyTypeId = 2,
                    Name = "TagCoin",
                    Abbreviation = "TAG",
                    Slug = "TAG"
                }, new Currency
                {
                    Id = 2768,
                    CurrencyTypeId = 2,
                    Name = "Fabric Token",
                    Abbreviation = "FT",
                    Slug = "FT"
                }, new Currency
                {
                    Id = 3422,
                    CurrencyTypeId = 2,
                    Name = "SHPING",
                    Abbreviation = "SHPING",
                    Slug = "SHPING"
                }, new Currency
                {
                    Id = 2056,
                    CurrencyTypeId = 2,
                    Name = "PiplCoin",
                    Abbreviation = "PIPL",
                    Slug = "PIPL"
                }, new Currency
                {
                    Id = 2065,
                    CurrencyTypeId = 2,
                    Name = "XGOX",
                    Abbreviation = "XGOX",
                    Slug = "XGOX"
                }, new Currency
                {
                    Id = 3511,
                    CurrencyTypeId = 2,
                    Name = "Bitibu Coin",
                    Abbreviation = "BTB",
                    Slug = "BTB"
                }, new Currency
                {
                    Id = 3348,
                    CurrencyTypeId = 2,
                    Name = "MNPCoin",
                    Abbreviation = "MNP",
                    Slug = "MNP"
                }, new Currency
                {
                    Id = 2230,
                    CurrencyTypeId = 2,
                    Name = "Monkey Project",
                    Abbreviation = "MONK",
                    Slug = "MONK"
                }, new Currency
                {
                    Id = 3488,
                    CurrencyTypeId = 2,
                    Name = "Gravity",
                    Abbreviation = "GZRO",
                    Slug = "GZRO"
                }, new Currency
                {
                    Id = 625,
                    CurrencyTypeId = 2,
                    Name = "bitBTC",
                    Abbreviation = "BITBTC",
                    Slug = "BITBTC"
                }, new Currency
                {
                    Id = 1731,
                    CurrencyTypeId = 2,
                    Name = "GlobalToken",
                    Abbreviation = "GLT",
                    Slug = "GLT"
                }, new Currency
                {
                    Id = 3387,
                    CurrencyTypeId = 2,
                    Name = "BLAST",
                    Abbreviation = "BLAST",
                    Slug = "BLAST"
                }, new Currency
                {
                    Id = 2218,
                    CurrencyTypeId = 2,
                    Name = "Magnet",
                    Abbreviation = "MAG",
                    Slug = "MAG"
                }, new Currency
                {
                    Id = 1799,
                    CurrencyTypeId = 2,
                    Name = "Rupee",
                    Abbreviation = "RUP",
                    Slug = "RUP"
                }, new Currency
                {
                    Id = 1257,
                    CurrencyTypeId = 2,
                    Name = "LanaCoin",
                    Abbreviation = "LANA",
                    Slug = "LANA"
                }, new Currency
                {
                    Id = 3472,
                    CurrencyTypeId = 2,
                    Name = "JSECOIN",
                    Abbreviation = "JSE",
                    Slug = "JSE"
                }, new Currency
                {
                    Id = 1185,
                    CurrencyTypeId = 2,
                    Name = "TrumpCoin",
                    Abbreviation = "TRUMP",
                    Slug = "TRUMP"
                }, new Currency
                {
                    Id = 2464,
                    CurrencyTypeId = 2,
                    Name = "Devery",
                    Abbreviation = "EVE",
                    Slug = "EVE"
                }, new Currency
                {
                    Id = 1434,
                    CurrencyTypeId = 2,
                    Name = "Advanced Technology Coin",
                    Abbreviation = "ARC",
                    Slug = "ARC"
                }, new Currency
                {
                    Id = 1439,
                    CurrencyTypeId = 2,
                    Name = "AllSafe",
                    Abbreviation = "ASAFE",
                    Slug = "ASAFE"
                }, new Currency
                {
                    Id = 3262,
                    CurrencyTypeId = 2,
                    Name = "CYCLEAN",
                    Abbreviation = "CCL",
                    Slug = "CCL"
                }, new Currency
                {
                    Id = 2531,
                    CurrencyTypeId = 2,
                    Name = "W3Coin",
                    Abbreviation = "W3C",
                    Slug = "W3C"
                }, new Currency
                {
                    Id = 2910,
                    CurrencyTypeId = 2,
                    Name = "Crowdholding",
                    Abbreviation = "YUP",
                    Slug = "YUP"
                }, new Currency
                {
                    Id = 2749,
                    CurrencyTypeId = 2,
                    Name = "Signals Network",
                    Abbreviation = "SGN",
                    Slug = "SGN"
                }, new Currency
                {
                    Id = 2779,
                    CurrencyTypeId = 2,
                    Name = "Level Up Coin",
                    Abbreviation = "LUC",
                    Slug = "LUC"
                }, new Currency
                {
                    Id = 276,
                    CurrencyTypeId = 2,
                    Name = "Bitstar",
                    Abbreviation = "BITS",
                    Slug = "BITS"
                }, new Currency
                {
                    Id = 1777,
                    CurrencyTypeId = 2,
                    Name = "CryptoPing",
                    Abbreviation = "PING",
                    Slug = "PING"
                }, new Currency
                {
                    Id = 3479,
                    CurrencyTypeId = 2,
                    Name = "MODEL-X-coin",
                    Abbreviation = "MODX",
                    Slug = "MODX"
                }, new Currency
                {
                    Id = 960,
                    CurrencyTypeId = 2,
                    Name = "FujiCoin",
                    Abbreviation = "FJC",
                    Slug = "FJC"
                }, new Currency
                {
                    Id = 3431,
                    CurrencyTypeId = 2,
                    Name = "Iconiq Lab Token",
                    Abbreviation = "ICNQ",
                    Slug = "ICNQ"
                }, new Currency
                {
                    Id = 3159,
                    CurrencyTypeId = 2,
                    Name = "Apollon",
                    Abbreviation = "XAP",
                    Slug = "XAP"
                }, new Currency
                {
                    Id = 1725,
                    CurrencyTypeId = 2,
                    Name = "Adelphoi",
                    Abbreviation = "ADL",
                    Slug = "ADL"
                }, new Currency
                {
                    Id = 1148,
                    CurrencyTypeId = 2,
                    Name = "EverGreenCoin",
                    Abbreviation = "EGC",
                    Slug = "EGC"
                }, new Currency
                {
                    Id = 764,
                    CurrencyTypeId = 2,
                    Name = "PayCoin",
                    Abbreviation = "XPY",
                    Slug = "XPY"
                }, new Currency
                {
                    Id = 50,
                    CurrencyTypeId = 2,
                    Name = "Emerald Crypto",
                    Abbreviation = "EMD",
                    Slug = "EMD"
                }, new Currency
                {
                    Id = 2332,
                    CurrencyTypeId = 2,
                    Name = "STRAKS",
                    Abbreviation = "STAK",
                    Slug = "STAK"
                }, new Currency
                {
                    Id = 1754,
                    CurrencyTypeId = 2,
                    Name = "Bitradio",
                    Abbreviation = "BRO",
                    Slug = "BRO"
                }, new Currency
                {
                    Id = 813,
                    CurrencyTypeId = 2,
                    Name = "bitSilver",
                    Abbreviation = "BITSILVER",
                    Slug = "BITSILVER"
                }, new Currency
                {
                    Id = 2520,
                    CurrencyTypeId = 2,
                    Name = "Jesus Coin",
                    Abbreviation = "JC",
                    Slug = "JC"
                }, new Currency
                {
                    Id = 2518,
                    CurrencyTypeId = 2,
                    Name = "LOCIcoin",
                    Abbreviation = "LOCI",
                    Slug = "LOCI"
                }, new Currency
                {
                    Id = 1603,
                    CurrencyTypeId = 2,
                    Name = "Databits",
                    Abbreviation = "DTB",
                    Slug = "DTB"
                }, new Currency
                {
                    Id = 1251,
                    CurrencyTypeId = 2,
                    Name = "SixEleven",
                    Abbreviation = "611",
                    Slug = "611"
                }, new Currency
                {
                    Id = 3495,
                    CurrencyTypeId = 2,
                    Name = "ModulTrade",
                    Abbreviation = "MTRC",
                    Slug = "MTRC"
                }, new Currency
                {
                    Id = 815,
                    CurrencyTypeId = 2,
                    Name = "Kobocoin",
                    Abbreviation = "KOBO",
                    Slug = "KOBO"
                }, new Currency
                {
                    Id = 1702,
                    CurrencyTypeId = 2,
                    Name = "Version",
                    Abbreviation = "V",
                    Slug = "V"
                }, new Currency
                {
                    Id = 1790,
                    CurrencyTypeId = 2,
                    Name = "WomenCoin",
                    Abbreviation = "WOMEN",
                    Slug = "WOMEN"
                }, new Currency
                {
                    Id = 951,
                    CurrencyTypeId = 2,
                    Name = "Synergy",
                    Abbreviation = "SNRG",
                    Slug = "SNRG"
                }, new Currency
                {
                    Id = 1481,
                    CurrencyTypeId = 2,
                    Name = "Nexium",
                    Abbreviation = "NXC",
                    Slug = "NXC"
                }, new Currency
                {
                    Id = 1643,
                    CurrencyTypeId = 2,
                    Name = "WavesGo",
                    Abbreviation = "WGO",
                    Slug = "WGO"
                }, new Currency
                {
                    Id = 3043,
                    CurrencyTypeId = 2,
                    Name = "PitisCoin",
                    Abbreviation = "PTS",
                    Slug = "PTS"
                }, new Currency
                {
                    Id = 1985,
                    CurrencyTypeId = 2,
                    Name = "Chronologic",
                    Abbreviation = "DAY",
                    Slug = "DAY"
                }, new Currency
                {
                    Id = 1724,
                    CurrencyTypeId = 2,
                    Name = "Linx",
                    Abbreviation = "LINX",
                    Slug = "LINX"
                }, new Currency
                {
                    Id = 2103,
                    CurrencyTypeId = 2,
                    Name = "Intelligent Trading Foundation",
                    Abbreviation = "ITT",
                    Slug = "ITT"
                }, new Currency
                {
                    Id = 3091,
                    CurrencyTypeId = 2,
                    Name = "Sapien",
                    Abbreviation = "SPN",
                    Slug = "SPN"
                }, new Currency
                {
                    Id = 1381,
                    CurrencyTypeId = 2,
                    Name = "Bitcloud",
                    Abbreviation = "BTDX",
                    Slug = "BTDX"
                }, new Currency
                {
                    Id = 2935,
                    CurrencyTypeId = 2,
                    Name = "CDMCOIN",
                    Abbreviation = "CDM",
                    Slug = "CDM"
                }, new Currency
                {
                    Id = 1722,
                    CurrencyTypeId = 2,
                    Name = "More Coin",
                    Abbreviation = "MORE",
                    Slug = "MORE"
                }, new Currency
                {
                    Id = 72,
                    CurrencyTypeId = 2,
                    Name = "Deutsche eMark",
                    Abbreviation = "DEM",
                    Slug = "DEM"
                }, new Currency
                {
                    Id = 1752,
                    CurrencyTypeId = 2,
                    Name = "Goodomy",
                    Abbreviation = "GOOD",
                    Slug = "GOOD"
                }, new Currency
                {
                    Id = 2196,
                    CurrencyTypeId = 2,
                    Name = "Sugar Exchange",
                    Abbreviation = "SGR",
                    Slug = "SGR"
                }, new Currency
                {
                    Id = 597,
                    CurrencyTypeId = 2,
                    Name = "Opal",
                    Abbreviation = "OPAL",
                    Slug = "OPAL"
                }, new Currency
                {
                    Id = 1334,
                    CurrencyTypeId = 2,
                    Name = "Elementrem",
                    Abbreviation = "ELE",
                    Slug = "ELE"
                }, new Currency
                {
                    Id = 778,
                    CurrencyTypeId = 2,
                    Name = "bitGold",
                    Abbreviation = "BITGOLD",
                    Slug = "BITGOLD"
                }, new Currency
                {
                    Id = 954,
                    CurrencyTypeId = 2,
                    Name = "bitEUR",
                    Abbreviation = "BITEUR",
                    Slug = "BITEUR"
                }, new Currency
                {
                    Id = 1504,
                    CurrencyTypeId = 2,
                    Name = "InflationCoin",
                    Abbreviation = "IFLT",
                    Slug = "IFLT"
                }, new Currency
                {
                    Id = 702,
                    CurrencyTypeId = 2,
                    Name = "SpreadCoin",
                    Abbreviation = "SPR",
                    Slug = "SPR"
                }, new Currency
                {
                    Id = 1297,
                    CurrencyTypeId = 2,
                    Name = "ChessCoin",
                    Abbreviation = "CHESS",
                    Slug = "CHESS"
                }, new Currency
                {
                    Id = 3690,
                    CurrencyTypeId = 2,
                    Name = "Bulleon",
                    Abbreviation = "BUL",
                    Slug = "BUL"
                }, new Currency
                {
                    Id = 869,
                    CurrencyTypeId = 2,
                    Name = "Crave",
                    Abbreviation = "CRAVE",
                    Slug = "CRAVE"
                }, new Currency
                {
                    Id = 703,
                    CurrencyTypeId = 2,
                    Name = "Rimbit",
                    Abbreviation = "RBT",
                    Slug = "RBT"
                }, new Currency
                {
                    Id = 33,
                    CurrencyTypeId = 2,
                    Name = "Mincoin",
                    Abbreviation = "MNC",
                    Slug = "MNC"
                }, new Currency
                {
                    Id = 2160,
                    CurrencyTypeId = 2,
                    Name = "Innova",
                    Abbreviation = "INN",
                    Slug = "INN"
                }, new Currency
                {
                    Id = 3124,
                    CurrencyTypeId = 2,
                    Name = "Dragonglass",
                    Abbreviation = "DGS",
                    Slug = "DGS"
                }, new Currency
                {
                    Id = 2063,
                    CurrencyTypeId = 2,
                    Name = "Tracto",
                    Abbreviation = "TRCT",
                    Slug = "TRCT"
                }, new Currency
                {
                    Id = 3439,
                    CurrencyTypeId = 2,
                    Name = "iDealCash",
                    Abbreviation = "DEAL",
                    Slug = "DEAL"
                }, new Currency
                {
                    Id = 2514,
                    CurrencyTypeId = 2,
                    Name = "Shekel",
                    Abbreviation = "JEW",
                    Slug = "JEW"
                }, new Currency
                {
                    Id = 2848,
                    CurrencyTypeId = 2,
                    Name = "Paymon",
                    Abbreviation = "PMNT",
                    Slug = "PMNT"
                }, new Currency
                {
                    Id = 1980,
                    CurrencyTypeId = 2,
                    Name = "Elixir",
                    Abbreviation = "ELIX",
                    Slug = "ELIX"
                }, new Currency
                {
                    Id = 3399,
                    CurrencyTypeId = 2,
                    Name = "Wispr",
                    Abbreviation = "WSP",
                    Slug = "WSP"
                }, new Currency
                {
                    Id = 3457,
                    CurrencyTypeId = 2,
                    Name = "Iridium",
                    Abbreviation = "IRD",
                    Slug = "IRD"
                }, new Currency
                {
                    Id = 3311,
                    CurrencyTypeId = 2,
                    Name = "Castle",
                    Abbreviation = "CSTL",
                    Slug = "CSTL"
                }, new Currency
                {
                    Id = 921,
                    CurrencyTypeId = 2,
                    Name = "Universal Currency",
                    Abbreviation = "UNIT",
                    Slug = "UNIT"
                }, new Currency
                {
                    Id = 3331,
                    CurrencyTypeId = 2,
                    Name = "CrowdWiz",
                    Abbreviation = "WIZ",
                    Slug = "WIZ"
                }, new Currency
                {
                    Id = 3764,
                    CurrencyTypeId = 2,
                    Name = "Save Environment Token",
                    Abbreviation = "SET",
                    Slug = "SET"
                }, new Currency
                {
                    Id = 2580,
                    CurrencyTypeId = 2,
                    Name = "Leadcoin",
                    Abbreviation = "LDC",
                    Slug = "LDC"
                }, new Currency
                {
                    Id = 2681,
                    CurrencyTypeId = 2,
                    Name = "Origami",
                    Abbreviation = "ORI",
                    Slug = "ORI"
                }, new Currency
                {
                    Id = 2395,
                    CurrencyTypeId = 2,
                    Name = "Ignition",
                    Abbreviation = "IC",
                    Slug = "IC"
                }, new Currency
                {
                    Id = 1959,
                    CurrencyTypeId = 2,
                    Name = "Oceanlab",
                    Abbreviation = "OCL",
                    Slug = "OCL"
                }, new Currency
                {
                    Id = 1671,
                    CurrencyTypeId = 2,
                    Name = "iTicoin",
                    Abbreviation = "ITI",
                    Slug = "ITI"
                }, new Currency
                {
                    Id = 3087,
                    CurrencyTypeId = 2,
                    Name = "CROAT",
                    Abbreviation = "CROAT",
                    Slug = "CROAT"
                }, new Currency
                {
                    Id = 121,
                    CurrencyTypeId = 2,
                    Name = "UltraCoin",
                    Abbreviation = "UTC",
                    Slug = "UTC"
                }, new Currency
                {
                    Id = 2542,
                    CurrencyTypeId = 2,
                    Name = "Tidex Token",
                    Abbreviation = "TDX",
                    Slug = "TDX"
                }, new Currency
                {
                    Id = 3268,
                    CurrencyTypeId = 2,
                    Name = "DarexTravel",
                    Abbreviation = "DART",
                    Slug = "DART"
                }, new Currency
                {
                    Id = 3122,
                    CurrencyTypeId = 2,
                    Name = "Help The Homeless Coin",
                    Abbreviation = "HTH",
                    Slug = "HTH"
                }, new Currency
                {
                    Id = 3665,
                    CurrencyTypeId = 2,
                    Name = "Impleum",
                    Abbreviation = "IMPL",
                    Slug = "IMPL"
                }, new Currency
                {
                    Id = 3412,
                    CurrencyTypeId = 2,
                    Name = "Simmitri",
                    Abbreviation = "SIM",
                    Slug = "SIM"
                }, new Currency
                {
                    Id = 331,
                    CurrencyTypeId = 2,
                    Name = "Litecoin Plus",
                    Abbreviation = "LCP",
                    Slug = "LCP"
                }, new Currency
                {
                    Id = 3688,
                    CurrencyTypeId = 2,
                    Name = "MoX",
                    Abbreviation = "MOX",
                    Slug = "MOX"
                }, new Currency
                {
                    Id = 978,
                    CurrencyTypeId = 2,
                    Name = "Ratecoin",
                    Abbreviation = "XRA",
                    Slug = "XRA"
                }, new Currency
                {
                    Id = 3463,
                    CurrencyTypeId = 2,
                    Name = "RPICoin",
                    Abbreviation = "RPI",
                    Slug = "RPI"
                }, new Currency
                {
                    Id = 2404,
                    CurrencyTypeId = 2,
                    Name = "TOKYO",
                    Abbreviation = "TOKC",
                    Slug = "TOKC"
                }, new Currency
                {
                    Id = 3013,
                    CurrencyTypeId = 2,
                    Name = "PRiVCY",
                    Abbreviation = "PRIV",
                    Slug = "PRIV"
                }, new Currency
                {
                    Id = 2007,
                    CurrencyTypeId = 2,
                    Name = "Regalcoin",
                    Abbreviation = "REC",
                    Slug = "REC"
                }, new Currency
                {
                    Id = 3093,
                    CurrencyTypeId = 2,
                    Name = "BBSCoin",
                    Abbreviation = "BBS",
                    Slug = "BBS"
                }, new Currency
                {
                    Id = 2122,
                    CurrencyTypeId = 2,
                    Name = "Ellaism",
                    Abbreviation = "ELLA",
                    Slug = "ELLA"
                }, new Currency
                {
                    Id = 1842,
                    CurrencyTypeId = 2,
                    Name = "CampusCoin",
                    Abbreviation = "CC",
                    Slug = "CC"
                }, new Currency
                {
                    Id = 2180,
                    CurrencyTypeId = 2,
                    Name = "bitJob",
                    Abbreviation = "STU",
                    Slug = "STU"
                }, new Currency
                {
                    Id = 3882,
                    CurrencyTypeId = 2,
                    Name = "Arqma",
                    Abbreviation = "ARQ",
                    Slug = "ARQ"
                }, new Currency
                {
                    Id = 3146,
                    CurrencyTypeId = 2,
                    Name = "CyberFM",
                    Abbreviation = "CYFM",
                    Slug = "CYFM"
                }, new Currency
                {
                    Id = 3214,
                    CurrencyTypeId = 2,
                    Name = "Soniq",
                    Abbreviation = "SONIQ",
                    Slug = "SONIQ"
                }, new Currency
                {
                    Id = 2690,
                    CurrencyTypeId = 2,
                    Name = "Biotron",
                    Abbreviation = "BTRN",
                    Slug = "BTRN"
                }, new Currency
                {
                    Id = 2048,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Cash",
                    Abbreviation = "ECASH",
                    Slug = "ECASH"
                }, new Currency
                {
                    Id = 1468,
                    CurrencyTypeId = 2,
                    Name = "Kurrent",
                    Abbreviation = "KURT",
                    Slug = "KURT"
                }, new Currency
                {
                    Id = 3800,
                    CurrencyTypeId = 2,
                    Name = "FidexToken",
                    Abbreviation = "FEX",
                    Slug = "FEX"
                }, new Currency
                {
                    Id = 28,
                    CurrencyTypeId = 2,
                    Name = "Digitalcoin",
                    Abbreviation = "DGC",
                    Slug = "DGC"
                }, new Currency
                {
                    Id = 1266,
                    CurrencyTypeId = 2,
                    Name = "MarteXcoin",
                    Abbreviation = "MXT",
                    Slug = "MXT"
                }, new Currency
                {
                    Id = 3207,
                    CurrencyTypeId = 2,
                    Name = "Social Activity Token",
                    Abbreviation = "SAT",
                    Slug = "SAT"
                }, new Currency
                {
                    Id = 1390,
                    CurrencyTypeId = 2,
                    Name = "Jin Coin",
                    Abbreviation = "JIN",
                    Slug = "JIN"
                }, new Currency
                {
                    Id = 3409,
                    CurrencyTypeId = 2,
                    Name = "Etheera",
                    Abbreviation = "ETA",
                    Slug = "ETA"
                }, new Currency
                {
                    Id = 1276,
                    CurrencyTypeId = 2,
                    Name = "ICO OpenLedger",
                    Abbreviation = "ICOO",
                    Slug = "ICOO"
                }, new Currency
                {
                    Id = 3133,
                    CurrencyTypeId = 2,
                    Name = "Arepacoin",
                    Abbreviation = "AREPA",
                    Slug = "AREPA"
                }, new Currency
                {
                    Id = 2890,
                    CurrencyTypeId = 2,
                    Name = "KanadeCoin",
                    Abbreviation = "KNDC",
                    Slug = "KNDC"
                }, new Currency
                {
                    Id = 1662,
                    CurrencyTypeId = 2,
                    Name = "Condensate",
                    Abbreviation = "RAIN",
                    Slug = "RAIN"
                }, new Currency
                {
                    Id = 1483,
                    CurrencyTypeId = 2,
                    Name = "vSlice",
                    Abbreviation = "VSL",
                    Slug = "VSL"
                }, new Currency
                {
                    Id = 350,
                    CurrencyTypeId = 2,
                    Name = "BoostCoin",
                    Abbreviation = "BOST",
                    Slug = "BOST"
                }, new Currency
                {
                    Id = 1511,
                    CurrencyTypeId = 2,
                    Name = "PureVidz",
                    Abbreviation = "VIDZ",
                    Slug = "VIDZ"
                }, new Currency
                {
                    Id = 1306,
                    CurrencyTypeId = 2,
                    Name = "Cryptojacks",
                    Abbreviation = "CJ",
                    Slug = "CJ"
                }, new Currency
                {
                    Id = 1085,
                    CurrencyTypeId = 2,
                    Name = "Swing",
                    Abbreviation = "SWING",
                    Slug = "SWING"
                }, new Currency
                {
                    Id = 3031,
                    CurrencyTypeId = 2,
                    Name = "Orbis Token",
                    Abbreviation = "OBT",
                    Slug = "OBT"
                }, new Currency
                {
                    Id = 3008,
                    CurrencyTypeId = 2,
                    Name = "Vivid Coin",
                    Abbreviation = "VIVID",
                    Slug = "VIVID"
                }, new Currency
                {
                    Id = 295,
                    CurrencyTypeId = 2,
                    Name = "BTCtalkcoin",
                    Abbreviation = "TALK",
                    Slug = "TALK"
                }, new Currency
                {
                    Id = 3443,
                    CurrencyTypeId = 2,
                    Name = "HUZU",
                    Abbreviation = "HUZU",
                    Slug = "HUZU"
                }, new Currency
                {
                    Id = 3223,
                    CurrencyTypeId = 2,
                    Name = "DOWCOIN",
                    Abbreviation = "DOW",
                    Slug = "DOW"
                }, new Currency
                {
                    Id = 3125,
                    CurrencyTypeId = 2,
                    Name = "XDNA",
                    Abbreviation = "XDNA",
                    Slug = "XDNA"
                }, new Currency
                {
                    Id = 426,
                    CurrencyTypeId = 2,
                    Name = "BritCoin",
                    Abbreviation = "BRIT",
                    Slug = "BRIT"
                }, new Currency
                {
                    Id = 113,
                    CurrencyTypeId = 2,
                    Name = "SmartCoin",
                    Abbreviation = "SMC",
                    Slug = "SMC"
                }, new Currency
                {
                    Id = 2770,
                    CurrencyTypeId = 2,
                    Name = "Cazcoin",
                    Abbreviation = "CAZ",
                    Slug = "CAZ"
                }, new Currency
                {
                    Id = 130,
                    CurrencyTypeId = 2,
                    Name = "HunterCoin",
                    Abbreviation = "HUC",
                    Slug = "HUC"
                }, new Currency
                {
                    Id = 1165,
                    CurrencyTypeId = 2,
                    Name = "Evil Coin",
                    Abbreviation = "EVIL",
                    Slug = "EVIL"
                }, new Currency
                {
                    Id = 1981,
                    CurrencyTypeId = 2,
                    Name = "Billionaire Token",
                    Abbreviation = "XBL",
                    Slug = "XBL"
                }, new Currency
                {
                    Id = 2486,
                    CurrencyTypeId = 2,
                    Name = "Speed Mining Service",
                    Abbreviation = "SMS",
                    Slug = "SMS"
                }, new Currency
                {
                    Id = 2355,
                    CurrencyTypeId = 2,
                    Name = "OP Coin",
                    Abbreviation = "OPC",
                    Slug = "OPC"
                }, new Currency
                {
                    Id = 341,
                    CurrencyTypeId = 2,
                    Name = "SuperCoin",
                    Abbreviation = "SUPER",
                    Slug = "SUPER"
                }, new Currency
                {
                    Id = 2751,
                    CurrencyTypeId = 2,
                    Name = "FundRequest",
                    Abbreviation = "FND",
                    Slug = "FND"
                }, new Currency
                {
                    Id = 3246,
                    CurrencyTypeId = 2,
                    Name = "Thunderstake",
                    Abbreviation = "TSC",
                    Slug = "TSC"
                }, new Currency
                {
                    Id = 1747,
                    CurrencyTypeId = 2,
                    Name = "Onix",
                    Abbreviation = "ONX",
                    Slug = "ONX"
                }, new Currency
                {
                    Id = 1285,
                    CurrencyTypeId = 2,
                    Name = "GoldBlocks",
                    Abbreviation = "GB",
                    Slug = "GB"
                }, new Currency
                {
                    Id = 1956,
                    CurrencyTypeId = 2,
                    Name = "VIVO",
                    Abbreviation = "VIVO",
                    Slug = "VIVO"
                }, new Currency
                {
                    Id = 1673,
                    CurrencyTypeId = 2,
                    Name = "Minereum",
                    Abbreviation = "MNE",
                    Slug = "MNE"
                }, new Currency
                {
                    Id = 2069,
                    CurrencyTypeId = 2,
                    Name = "Open Trading Network",
                    Abbreviation = "OTN",
                    Slug = "OTN"
                }, new Currency
                {
                    Id = 3414,
                    CurrencyTypeId = 2,
                    Name = "ZeusNetwork",
                    Abbreviation = "ZEUS",
                    Slug = "ZEUS"
                }, new Currency
                {
                    Id = 2648,
                    CurrencyTypeId = 2,
                    Name = "Bitsum",
                    Abbreviation = "BSM",
                    Slug = "BSM"
                }, new Currency
                {
                    Id = 3165,
                    CurrencyTypeId = 2,
                    Name = "Arion",
                    Abbreviation = "ARION",
                    Slug = "ARION"
                }, new Currency
                {
                    Id = 1836,
                    CurrencyTypeId = 2,
                    Name = "Signatum",
                    Abbreviation = "SIGT",
                    Slug = "SIGT"
                }, new Currency
                {
                    Id = 2093,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Red",
                    Abbreviation = "BTCRED",
                    Slug = "BTCRED"
                }, new Currency
                {
                    Id = 3517,
                    CurrencyTypeId = 2,
                    Name = "SONDER",
                    Abbreviation = "SNR",
                    Slug = "SNR"
                }, new Currency
                {
                    Id = 3377,
                    CurrencyTypeId = 2,
                    Name = "GenesisX",
                    Abbreviation = "XGS",
                    Slug = "XGS"
                }, new Currency
                {
                    Id = 3046,
                    CurrencyTypeId = 2,
                    Name = "Blocknode",
                    Abbreviation = "BND",
                    Slug = "BND"
                }, new Currency
                {
                    Id = 2415,
                    CurrencyTypeId = 2,
                    Name = "ArbitrageCT",
                    Abbreviation = "ARCT",
                    Slug = "ARCT"
                }, new Currency
                {
                    Id = 1763,
                    CurrencyTypeId = 2,
                    Name = "BriaCoin",
                    Abbreviation = "BRIA",
                    Slug = "BRIA"
                }, new Currency
                {
                    Id = 1273,
                    CurrencyTypeId = 2,
                    Name = "Citadel",
                    Abbreviation = "CTL",
                    Slug = "CTL"
                }, new Currency
                {
                    Id = 3266,
                    CurrencyTypeId = 2,
                    Name = "Carebit",
                    Abbreviation = "CARE",
                    Slug = "CARE"
                }, new Currency
                {
                    Id = 988,
                    CurrencyTypeId = 2,
                    Name = "IrishCoin",
                    Abbreviation = "IRL",
                    Slug = "IRL"
                }, new Currency
                {
                    Id = 1922,
                    CurrencyTypeId = 2,
                    Name = "Monoeci",
                    Abbreviation = "XMCC",
                    Slug = "XMCC"
                }, new Currency
                {
                    Id = 2973,
                    CurrencyTypeId = 2,
                    Name = "empowr coin",
                    Abbreviation = "EMPR",
                    Slug = "EMPR"
                }, new Currency
                {
                    Id = 1053,
                    CurrencyTypeId = 2,
                    Name = "Bolivarcoin",
                    Abbreviation = "BOLI",
                    Slug = "BOLI"
                }, new Currency
                {
                    Id = 2619,
                    CurrencyTypeId = 2,
                    Name = "BitStation",
                    Abbreviation = "BSTN",
                    Slug = "BSTN"
                }, new Currency
                {
                    Id = 3493,
                    CurrencyTypeId = 2,
                    Name = "SAKECOIN",
                    Abbreviation = "SAKE",
                    Slug = "SAKE"
                }, new Currency
                {
                    Id = 3289,
                    CurrencyTypeId = 2,
                    Name = "BitCoen",
                    Abbreviation = "BEN",
                    Slug = "BEN"
                }, new Currency
                {
                    Id = 1223,
                    CurrencyTypeId = 2,
                    Name = "BERNcash",
                    Abbreviation = "BERN",
                    Slug = "BERN"
                }, new Currency
                {
                    Id = 3447,
                    CurrencyTypeId = 2,
                    Name = "Atheios",
                    Abbreviation = "ATH",
                    Slug = "ATH"
                }, new Currency
                {
                    Id = 1888,
                    CurrencyTypeId = 2,
                    Name = "InvestFeed",
                    Abbreviation = "IFT",
                    Slug = "IFT"
                }, new Currency
                {
                    Id = 1607,
                    CurrencyTypeId = 2,
                    Name = "Impact",
                    Abbreviation = "IMX",
                    Slug = "IMX"
                }, new Currency
                {
                    Id = 3293,
                    CurrencyTypeId = 2,
                    Name = "Olympic",
                    Abbreviation = "OLMP",
                    Slug = "OLMP"
                }, new Currency
                {
                    Id = 3395,
                    CurrencyTypeId = 2,
                    Name = "SteepCoin",
                    Abbreviation = "STEEP",
                    Slug = "STEEP"
                }, new Currency
                {
                    Id = 316,
                    CurrencyTypeId = 2,
                    Name = "Dreamcoin",
                    Abbreviation = "DRM",
                    Slug = "DRM"
                }, new Currency
                {
                    Id = 2290,
                    CurrencyTypeId = 2,
                    Name = "YENTEN",
                    Abbreviation = "YTN",
                    Slug = "YTN"
                }, new Currency
                {
                    Id = 125,
                    CurrencyTypeId = 2,
                    Name = "Blakecoin",
                    Abbreviation = "BLC",
                    Slug = "BLC"
                }, new Currency
                {
                    Id = 1033,
                    CurrencyTypeId = 2,
                    Name = "GuccioneCoin",
                    Abbreviation = "GCC",
                    Slug = "GCC"
                }, new Currency
                {
                    Id = 2883,
                    CurrencyTypeId = 2,
                    Name = "ZINC",
                    Abbreviation = "ZINC",
                    Slug = "ZINC"
                }, new Currency
                {
                    Id = 2996,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin W Spectrum",
                    Abbreviation = "BWS",
                    Slug = "BWS"
                }, new Currency
                {
                    Id = 3350,
                    CurrencyTypeId = 2,
                    Name = "Dach Coin",
                    Abbreviation = "DACHX",
                    Slug = "DACHX"
                }, new Currency
                {
                    Id = 3652,
                    CurrencyTypeId = 2,
                    Name = "bitcoin2network",
                    Abbreviation = "B2N",
                    Slug = "B2N"
                }, new Currency
                {
                    Id = 1136,
                    CurrencyTypeId = 2,
                    Name = "Adzcoin",
                    Abbreviation = "ADZ",
                    Slug = "ADZ"
                }, new Currency
                {
                    Id = 3051,
                    CurrencyTypeId = 2,
                    Name = "Bitblocks",
                    Abbreviation = "BBK",
                    Slug = "BBK"
                }, new Currency
                {
                    Id = 837,
                    CurrencyTypeId = 2,
                    Name = "X-Coin",
                    Abbreviation = "XCO",
                    Slug = "XCO"
                }, new Currency
                {
                    Id = 1617,
                    CurrencyTypeId = 2,
                    Name = "Ultimate Secure Cash",
                    Abbreviation = "USC",
                    Slug = "USC"
                }, new Currency
                {
                    Id = 1035,
                    CurrencyTypeId = 2,
                    Name = "AmsterdamCoin",
                    Abbreviation = "AMS",
                    Slug = "AMS"
                }, new Currency
                {
                    Id = 3332,
                    CurrencyTypeId = 2,
                    Name = "Gossipcoin",
                    Abbreviation = "GOSS",
                    Slug = "GOSS"
                }, new Currency
                {
                    Id = 1550,
                    CurrencyTypeId = 2,
                    Name = "Master Swiscoin",
                    Abbreviation = "MSCN",
                    Slug = "MSCN"
                }, new Currency
                {
                    Id = 1877,
                    CurrencyTypeId = 2,
                    Name = "Rupaya",
                    Abbreviation = "RUPX",
                    Slug = "RUPX"
                }, new Currency
                {
                    Id = 1113,
                    CurrencyTypeId = 2,
                    Name = "SecretCoin",
                    Abbreviation = "SCRT",
                    Slug = "SCRT"
                }, new Currency
                {
                    Id = 2221,
                    CurrencyTypeId = 2,
                    Name = "VoteCoin",
                    Abbreviation = "VOT",
                    Slug = "VOT"
                }, new Currency
                {
                    Id = 1250,
                    CurrencyTypeId = 2,
                    Name = "Zurcoin",
                    Abbreviation = "ZUR",
                    Slug = "ZUR"
                }, new Currency
                {
                    Id = 2668,
                    CurrencyTypeId = 2,
                    Name = "Earth Token",
                    Abbreviation = "EARTH",
                    Slug = "EARTH"
                }, new Currency
                {
                    Id = 2651,
                    CurrencyTypeId = 2,
                    Name = "GreenMed",
                    Abbreviation = "GRMD",
                    Slug = "GRMD"
                }, new Currency
                {
                    Id = 3503,
                    CurrencyTypeId = 2,
                    Name = "CommunityGeneration",
                    Abbreviation = "CGEN",
                    Slug = "CGEN"
                }, new Currency
                {
                    Id = 3250,
                    CurrencyTypeId = 2,
                    Name = "Zoomba",
                    Abbreviation = "ZBA",
                    Slug = "ZBA"
                }, new Currency
                {
                    Id = 3009,
                    CurrencyTypeId = 2,
                    Name = "Pure",
                    Abbreviation = "PUREX",
                    Slug = "PUREX"
                }, new Currency
                {
                    Id = 3668,
                    CurrencyTypeId = 2,
                    Name = "ProxyNode",
                    Abbreviation = "PRX",
                    Slug = "PRX"
                }, new Currency
                {
                    Id = 3403,
                    CurrencyTypeId = 2,
                    Name = "EagleX",
                    Abbreviation = "EGX",
                    Slug = "EGX"
                }, new Currency
                {
                    Id = 1248,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin 21",
                    Abbreviation = "XBTC21",
                    Slug = "XBTC21"
                }, new Currency
                {
                    Id = 3353,
                    CurrencyTypeId = 2,
                    Name = "WELL",
                    Abbreviation = "WELL",
                    Slug = "WELL"
                }, new Currency
                {
                    Id = 3416,
                    CurrencyTypeId = 2,
                    Name = "Digiwage",
                    Abbreviation = "WAGE",
                    Slug = "WAGE"
                }, new Currency
                {
                    Id = 2978,
                    CurrencyTypeId = 2,
                    Name = "AceD",
                    Abbreviation = "ACED",
                    Slug = "ACED"
                }, new Currency
                {
                    Id = 3321,
                    CurrencyTypeId = 2,
                    Name = "BunnyToken",
                    Abbreviation = "BUNNY",
                    Slug = "BUNNY"
                }, new Currency
                {
                    Id = 3656,
                    CurrencyTypeId = 2,
                    Name = "Beacon",
                    Abbreviation = "BECN",
                    Slug = "BECN"
                }, new Currency
                {
                    Id = 278,
                    CurrencyTypeId = 2,
                    Name = "Quebecoin",
                    Abbreviation = "QBC",
                    Slug = "QBC"
                }, new Currency
                {
                    Id = 536,
                    CurrencyTypeId = 2,
                    Name = "Joincoin",
                    Abbreviation = "J",
                    Slug = "J"
                }, new Currency
                {
                    Id = 3341,
                    CurrencyTypeId = 2,
                    Name = "Phonecoin",
                    Abbreviation = "PHON",
                    Slug = "PHON"
                }, new Currency
                {
                    Id = 3778,
                    CurrencyTypeId = 2,
                    Name = "EVOS",
                    Abbreviation = "EVOS",
                    Slug = "EVOS"
                }, new Currency
                {
                    Id = 1846,
                    CurrencyTypeId = 2,
                    Name = "GeyserCoin",
                    Abbreviation = "GSR",
                    Slug = "GSR"
                }, new Currency
                {
                    Id = 3267,
                    CurrencyTypeId = 2,
                    Name = "Project Coin",
                    Abbreviation = "PRJ",
                    Slug = "PRJ"
                }, new Currency
                {
                    Id = 2051,
                    CurrencyTypeId = 2,
                    Name = "Authorship",
                    Abbreviation = "ATS",
                    Slug = "ATS"
                }, new Currency
                {
                    Id = 367,
                    CurrencyTypeId = 2,
                    Name = "Coin2.1",
                    Abbreviation = "C2",
                    Slug = "C2"
                }, new Currency
                {
                    Id = 3050,
                    CurrencyTypeId = 2,
                    Name = "Dystem",
                    Abbreviation = "DTEM",
                    Slug = "DTEM"
                }, new Currency
                {
                    Id = 2715,
                    CurrencyTypeId = 2,
                    Name = "ConnectJob",
                    Abbreviation = "CJT",
                    Slug = "CJT"
                }, new Currency
                {
                    Id = 3273,
                    CurrencyTypeId = 2,
                    Name = "IQ.cash",
                    Abbreviation = "IQ",
                    Slug = "IQ"
                }, new Currency
                {
                    Id = 1850,
                    CurrencyTypeId = 2,
                    Name = "Cream",
                    Abbreviation = "CRM",
                    Slug = "CRM"
                }, new Currency
                {
                    Id = 1496,
                    CurrencyTypeId = 2,
                    Name = "Luna Coin",
                    Abbreviation = "LUNA",
                    Slug = "LUNA"
                }, new Currency
                {
                    Id = 1218,
                    CurrencyTypeId = 2,
                    Name = "PostCoin",
                    Abbreviation = "POST",
                    Slug = "POST"
                }, new Currency
                {
                    Id = 3393,
                    CurrencyTypeId = 2,
                    Name = "MASTERNET",
                    Abbreviation = "MASH",
                    Slug = "MASH"
                }, new Currency
                {
                    Id = 3324,
                    CurrencyTypeId = 2,
                    Name = "PluraCoin",
                    Abbreviation = "PLURA",
                    Slug = "PLURA"
                }, new Currency
                {
                    Id = 3464,
                    CurrencyTypeId = 2,
                    Name = "Cheesecoin",
                    Abbreviation = "CHEESE",
                    Slug = "CHEESE"
                }, new Currency
                {
                    Id = 1793,
                    CurrencyTypeId = 2,
                    Name = "Bitdeal",
                    Abbreviation = "BDL",
                    Slug = "BDL"
                }, new Currency
                {
                    Id = 3645,
                    CurrencyTypeId = 2,
                    Name = "Shivers",
                    Abbreviation = "SHVR",
                    Slug = "SHVR"
                }, new Currency
                {
                    Id = 2241,
                    CurrencyTypeId = 2,
                    Name = "Ccore",
                    Abbreviation = "CCO",
                    Slug = "CCO"
                }, new Currency
                {
                    Id = 2032,
                    CurrencyTypeId = 2,
                    Name = "Crystal Clear ",
                    Abbreviation = "CCT",
                    Slug = "CCT"
                }, new Currency
                {
                    Id = 2074,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Gold",
                    Abbreviation = "ETG",
                    Slug = "ETG"
                }, new Currency
                {
                    Id = 3624,
                    CurrencyTypeId = 2,
                    Name = "Zealium",
                    Abbreviation = "NZL",
                    Slug = "NZL"
                }, new Currency
                {
                    Id = 1539,
                    CurrencyTypeId = 2,
                    Name = "Elysium",
                    Abbreviation = "ELS",
                    Slug = "ELS"
                }, new Currency
                {
                    Id = 1506,
                    CurrencyTypeId = 2,
                    Name = "Safe Trade Coin",
                    Abbreviation = "XSTC",
                    Slug = "XSTC"
                }, new Currency
                {
                    Id = 3489,
                    CurrencyTypeId = 2,
                    Name = "Escroco Emerald",
                    Abbreviation = "ESCE",
                    Slug = "ESCE"
                }, new Currency
                {
                    Id = 1687,
                    CurrencyTypeId = 2,
                    Name = "Digital Money Bits",
                    Abbreviation = "DMB",
                    Slug = "DMB"
                }, new Currency
                {
                    Id = 1254,
                    CurrencyTypeId = 2,
                    Name = "PlatinumBAR",
                    Abbreviation = "XPTX",
                    Slug = "XPTX"
                }, new Currency
                {
                    Id = 601,
                    CurrencyTypeId = 2,
                    Name = "Acoin",
                    Abbreviation = "ACOIN",
                    Slug = "ACOIN"
                }, new Currency
                {
                    Id = 1890,
                    CurrencyTypeId = 2,
                    Name = "Etheriya",
                    Abbreviation = "RIYA",
                    Slug = "RIYA"
                }, new Currency
                {
                    Id = 3294,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Adult",
                    Abbreviation = "BTAD",
                    Slug = "BTAD"
                }, new Currency
                {
                    Id = 1200,
                    CurrencyTypeId = 2,
                    Name = "NevaCoin",
                    Abbreviation = "NEVA",
                    Slug = "NEVA"
                }, new Currency
                {
                    Id = 3201,
                    CurrencyTypeId = 2,
                    Name = "Printex",
                    Abbreviation = "PRTX",
                    Slug = "PRTX"
                }, new Currency
                {
                    Id = 898,
                    CurrencyTypeId = 2,
                    Name = "Californium",
                    Abbreviation = "CF",
                    Slug = "CF"
                }, new Currency
                {
                    Id = 3062,
                    CurrencyTypeId = 2,
                    Name = "GambleCoin",
                    Abbreviation = "GMCN",
                    Slug = "GMCN"
                }, new Currency
                {
                    Id = 1212,
                    CurrencyTypeId = 2,
                    Name = "MojoCoin",
                    Abbreviation = "MOJO",
                    Slug = "MOJO"
                }, new Currency
                {
                    Id = 2983,
                    CurrencyTypeId = 2,
                    Name = "AdultChain",
                    Abbreviation = "XXX",
                    Slug = "XXX"
                }, new Currency
                {
                    Id = 1241,
                    CurrencyTypeId = 2,
                    Name = "FuzzBalls",
                    Abbreviation = "FUZZ",
                    Slug = "FUZZ"
                }, new Currency
                {
                    Id = 3355,
                    CurrencyTypeId = 2,
                    Name = "Azart",
                    Abbreviation = "AZART",
                    Slug = "AZART"
                }, new Currency
                {
                    Id = 1717,
                    CurrencyTypeId = 2,
                    Name = "Neuro",
                    Abbreviation = "NRO",
                    Slug = "NRO"
                }, new Currency
                {
                    Id = 3192,
                    CurrencyTypeId = 2,
                    Name = "CatoCoin",
                    Abbreviation = "CATO",
                    Slug = "CATO"
                }, new Currency
                {
                    Id = 934,
                    CurrencyTypeId = 2,
                    Name = "ParkByte",
                    Abbreviation = "PKB",
                    Slug = "PKB"
                }, new Currency
                {
                    Id = 57,
                    CurrencyTypeId = 2,
                    Name = "SecureCoin",
                    Abbreviation = "SRC",
                    Slug = "SRC"
                }, new Currency
                {
                    Id = 3088,
                    CurrencyTypeId = 2,
                    Name = "BitCoin One",
                    Abbreviation = "BTCONE",
                    Slug = "BTCONE"
                }, new Currency
                {
                    Id = 1198,
                    CurrencyTypeId = 2,
                    Name = "BigUp",
                    Abbreviation = "BIGUP",
                    Slug = "BIGUP"
                }, new Currency
                {
                    Id = 1395,
                    CurrencyTypeId = 2,
                    Name = "Dollarcoin",
                    Abbreviation = "DLC",
                    Slug = "DLC"
                }, new Currency
                {
                    Id = 1353,
                    CurrencyTypeId = 2,
                    Name = "TajCoin",
                    Abbreviation = "TAJ",
                    Slug = "TAJ"
                }, new Currency
                {
                    Id = 1514,
                    CurrencyTypeId = 2,
                    Name = "ICOBID",
                    Abbreviation = "ICOB",
                    Slug = "ICOB"
                }, new Currency
                {
                    Id = 1534,
                    CurrencyTypeId = 2,
                    Name = "BOAT",
                    Abbreviation = "BOAT",
                    Slug = "BOAT"
                }, new Currency
                {
                    Id = 2205,
                    CurrencyTypeId = 2,
                    Name = "Phantomx",
                    Abbreviation = "PNX",
                    Slug = "PNX"
                }, new Currency
                {
                    Id = 3333,
                    CurrencyTypeId = 2,
                    Name = "Sola Token",
                    Abbreviation = "SOL",
                    Slug = "SOL"
                }, new Currency
                {
                    Id = 1535,
                    CurrencyTypeId = 2,
                    Name = "Eryllium",
                    Abbreviation = "ERY",
                    Slug = "ERY"
                }, new Currency
                {
                    Id = 3228,
                    CurrencyTypeId = 2,
                    Name = "Cryptosolartech",
                    Abbreviation = "CST",
                    Slug = "CST"
                }, new Currency
                {
                    Id = 3129,
                    CurrencyTypeId = 2,
                    Name = "Nyerium",
                    Abbreviation = "NYEX",
                    Slug = "NYEX"
                }, new Currency
                {
                    Id = 3349,
                    CurrencyTypeId = 2,
                    Name = "PyrexCoin",
                    Abbreviation = "PYX",
                    Slug = "PYX"
                }, new Currency
                {
                    Id = 520,
                    CurrencyTypeId = 2,
                    Name = "Virtacoin",
                    Abbreviation = "VTA",
                    Slug = "VTA"
                }, new Currency
                {
                    Id = 1038,
                    CurrencyTypeId = 2,
                    Name = "Eurocoin",
                    Abbreviation = "EUC",
                    Slug = "EUC"
                }, new Currency
                {
                    Id = 3644,
                    CurrencyTypeId = 2,
                    Name = "TravelNote",
                    Abbreviation = "TVNT",
                    Slug = "TVNT"
                }, new Currency
                {
                    Id = 1581,
                    CurrencyTypeId = 2,
                    Name = "Honey",
                    Abbreviation = "HONEY",
                    Slug = "HONEY"
                }, new Currency
                {
                    Id = 2168,
                    CurrencyTypeId = 2,
                    Name = "Grimcoin",
                    Abbreviation = "GRIM",
                    Slug = "GRIM"
                }, new Currency
                {
                    Id = 2140,
                    CurrencyTypeId = 2,
                    Name = "SONO",
                    Abbreviation = "SONO",
                    Slug = "SONO"
                }, new Currency
                {
                    Id = 1282,
                    CurrencyTypeId = 2,
                    Name = "High Voltage",
                    Abbreviation = "HVCO",
                    Slug = "HVCO"
                }, new Currency
                {
                    Id = 3030,
                    CurrencyTypeId = 2,
                    Name = "BrokerNekoNetwork",
                    Abbreviation = "BNN",
                    Slug = "BNN"
                }, new Currency
                {
                    Id = 1206,
                    CurrencyTypeId = 2,
                    Name = "BumbaCoin",
                    Abbreviation = "BUMBA",
                    Slug = "BUMBA"
                }, new Currency
                {
                    Id = 3421,
                    CurrencyTypeId = 2,
                    Name = "PrimeStone",
                    Abbreviation = "PSC",
                    Slug = "PSC"
                }, new Currency
                {
                    Id = 1651,
                    CurrencyTypeId = 2,
                    Name = "SpeedCash",
                    Abbreviation = "SCS",
                    Slug = "SCS"
                }, new Currency
                {
                    Id = 1155,
                    CurrencyTypeId = 2,
                    Name = "Litecred",
                    Abbreviation = "LTCR",
                    Slug = "LTCR"
                }, new Currency
                {
                    Id = 3682,
                    CurrencyTypeId = 2,
                    Name = "Italo",
                    Abbreviation = "XTA",
                    Slug = "XTA"
                }, new Currency
                {
                    Id = 3442,
                    CurrencyTypeId = 2,
                    Name = "INDINODE",
                    Abbreviation = "XIND",
                    Slug = "XIND"
                }, new Currency
                {
                    Id = 1209,
                    CurrencyTypeId = 2,
                    Name = "PosEx",
                    Abbreviation = "PEX",
                    Slug = "PEX"
                }, new Currency
                {
                    Id = 1597,
                    CurrencyTypeId = 2,
                    Name = "Bankcoin",
                    Abbreviation = "B@",
                    Slug = "B@"
                }, new Currency
                {
                    Id = 2025,
                    CurrencyTypeId = 2,
                    Name = "FLiK",
                    Abbreviation = "FLIK",
                    Slug = "FLIK"
                }, new Currency
                {
                    Id = 1926,
                    CurrencyTypeId = 2,
                    Name = "BROTHER",
                    Abbreviation = "BRAT",
                    Slug = "BRAT"
                }, new Currency
                {
                    Id = 159,
                    CurrencyTypeId = 2,
                    Name = "Cashcoin",
                    Abbreviation = "CASH",
                    Slug = "CASH"
                }, new Currency
                {
                    Id = 1389,
                    CurrencyTypeId = 2,
                    Name = "Zayedcoin",
                    Abbreviation = "ZYD",
                    Slug = "ZYD"
                }, new Currency
                {
                    Id = 1396,
                    CurrencyTypeId = 2,
                    Name = "MustangCoin",
                    Abbreviation = "MST",
                    Slug = "MST"
                }, new Currency
                {
                    Id = 69,
                    CurrencyTypeId = 2,
                    Name = "Datacoin",
                    Abbreviation = "DTC",
                    Slug = "DTC"
                }, new Currency
                {
                    Id = 3174,
                    CurrencyTypeId = 2,
                    Name = "Fintab",
                    Abbreviation = "FNTB",
                    Slug = "FNTB"
                }, new Currency
                {
                    Id = 3508,
                    CurrencyTypeId = 2,
                    Name = "BZLCOIN",
                    Abbreviation = "BZL",
                    Slug = "BZL"
                }, new Currency
                {
                    Id = 1889,
                    CurrencyTypeId = 2,
                    Name = "CoinonatX",
                    Abbreviation = "XCXT",
                    Slug = "XCXT"
                }, new Currency
                {
                    Id = 2905,
                    CurrencyTypeId = 2,
                    Name = "Qurito",
                    Abbreviation = "QURO",
                    Slug = "QURO"
                }, new Currency
                {
                    Id = 2257,
                    CurrencyTypeId = 2,
                    Name = "Nekonium",
                    Abbreviation = "NUKO",
                    Slug = "NUKO"
                }, new Currency
                {
                    Id = 2460,
                    CurrencyTypeId = 2,
                    Name = "Qbic",
                    Abbreviation = "QBIC",
                    Slug = "QBIC"
                }, new Currency
                {
                    Id = 1912,
                    CurrencyTypeId = 2,
                    Name = "Dalecoin",
                    Abbreviation = "DALC",
                    Slug = "DALC"
                }, new Currency
                {
                    Id = 1194,
                    CurrencyTypeId = 2,
                    Name = "Independent Money System",
                    Abbreviation = "IMS",
                    Slug = "IMS"
                }, new Currency
                {
                    Id = 993,
                    CurrencyTypeId = 2,
                    Name = "BowsCoin",
                    Abbreviation = "BSC",
                    Slug = "BSC"
                }, new Currency
                {
                    Id = 3410,
                    CurrencyTypeId = 2,
                    Name = "Bitspace",
                    Abbreviation = "BSX",
                    Slug = "BSX"
                }, new Currency
                {
                    Id = 1429,
                    CurrencyTypeId = 2,
                    Name = "Levocoin",
                    Abbreviation = "LEVO",
                    Slug = "LEVO"
                }, new Currency
                {
                    Id = 1576,
                    CurrencyTypeId = 2,
                    Name = "MiloCoin",
                    Abbreviation = "MILO",
                    Slug = "MILO"
                }, new Currency
                {
                    Id = 3491,
                    CurrencyTypeId = 2,
                    Name = "EZOOW",
                    Abbreviation = "EZW",
                    Slug = "EZW"
                }, new Currency
                {
                    Id = 1674,
                    CurrencyTypeId = 2,
                    Name = "Cannation",
                    Abbreviation = "CNNC",
                    Slug = "CNNC"
                }, new Currency
                {
                    Id = 1693,
                    CurrencyTypeId = 2,
                    Name = "Theresa May Coin",
                    Abbreviation = "MAY",
                    Slug = "MAY"
                }, new Currency
                {
                    Id = 2284,
                    CurrencyTypeId = 2,
                    Name = "Trident Group",
                    Abbreviation = "TRDT",
                    Slug = "TRDT"
                }, new Currency
                {
                    Id = 1935,
                    CurrencyTypeId = 2,
                    Name = "LiteCoin Ultra",
                    Abbreviation = "LTCU",
                    Slug = "LTCU"
                }, new Currency
                {
                    Id = 3313,
                    CurrencyTypeId = 2,
                    Name = "CryptoFlow",
                    Abbreviation = "CFL",
                    Slug = "CFL"
                }, new Currency
                {
                    Id = 1748,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Planet",
                    Abbreviation = "BTPL",
                    Slug = "BTPL"
                }, new Currency
                {
                    Id = 656,
                    CurrencyTypeId = 2,
                    Name = "Prime-XI",
                    Abbreviation = "PXI",
                    Slug = "PXI"
                }, new Currency
                {
                    Id = 3197,
                    CurrencyTypeId = 2,
                    Name = "Graphcoin",
                    Abbreviation = "GRPH",
                    Slug = "GRPH"
                }, new Currency
                {
                    Id = 1528,
                    CurrencyTypeId = 2,
                    Name = "Iconic",
                    Abbreviation = "ICON",
                    Slug = "ICON"
                }, new Currency
                {
                    Id = 1368,
                    CurrencyTypeId = 2,
                    Name = "Veltor",
                    Abbreviation = "VLT",
                    Slug = "VLT"
                }, new Currency
                {
                    Id = 1291,
                    CurrencyTypeId = 2,
                    Name = "Comet",
                    Abbreviation = "CMT",
                    Slug = "CMT"
                }, new Currency
                {
                    Id = 1509,
                    CurrencyTypeId = 2,
                    Name = "BenjiRolls",
                    Abbreviation = "BENJI",
                    Slug = "BENJI"
                }, new Currency
                {
                    Id = 2100,
                    CurrencyTypeId = 2,
                    Name = "JavaScript Token",
                    Abbreviation = "JS",
                    Slug = "JS"
                }, new Currency
                {
                    Id = 1546,
                    CurrencyTypeId = 2,
                    Name = "Centurion",
                    Abbreviation = "CNT",
                    Slug = "CNT"
                }, new Currency
                {
                    Id = 1052,
                    CurrencyTypeId = 2,
                    Name = "VectorAI",
                    Abbreviation = "VEC2",
                    Slug = "VEC2"
                }, new Currency
                {
                    Id = 1774,
                    CurrencyTypeId = 2,
                    Name = "SocialCoin",
                    Abbreviation = "SOCC",
                    Slug = "SOCC"
                }, new Currency
                {
                    Id = 1210,
                    CurrencyTypeId = 2,
                    Name = "Cabbage",
                    Abbreviation = "CAB",
                    Slug = "CAB"
                }, new Currency
                {
                    Id = 1716,
                    CurrencyTypeId = 2,
                    Name = "Ammo Reloaded",
                    Abbreviation = "AMMO",
                    Slug = "AMMO"
                }, new Currency
                {
                    Id = 263,
                    CurrencyTypeId = 2,
                    Name = "PLNcoin",
                    Abbreviation = "PLNC",
                    Slug = "PLNC"
                }, new Currency
                {
                    Id = 3163,
                    CurrencyTypeId = 2,
                    Name = "Mero",
                    Abbreviation = "MERO",
                    Slug = "MERO"
                }, new Currency
                {
                    Id = 2769,
                    CurrencyTypeId = 2,
                    Name = "Rhenium",
                    Abbreviation = "XRH",
                    Slug = "XRH"
                }, new Currency
                {
                    Id = 3427,
                    CurrencyTypeId = 2,
                    Name = "Agrolot",
                    Abbreviation = "AGLT",
                    Slug = "AGLT"
                }, new Currency
                {
                    Id = 1559,
                    CurrencyTypeId = 2,
                    Name = "Renos",
                    Abbreviation = "RNS",
                    Slug = "RNS"
                }, new Currency
                {
                    Id = 1657,
                    CurrencyTypeId = 2,
                    Name = "Bitvolt",
                    Abbreviation = "VOLT",
                    Slug = "VOLT"
                }, new Currency
                {
                    Id = 3298,
                    CurrencyTypeId = 2,
                    Name = "UralsCoin",
                    Abbreviation = "URALS",
                    Slug = "URALS"
                }, new Currency
                {
                    Id = 857,
                    CurrencyTypeId = 2,
                    Name = "SongCoin",
                    Abbreviation = "SONG",
                    Slug = "SONG"
                }, new Currency
                {
                    Id = 2131,
                    CurrencyTypeId = 2,
                    Name = "iBTC",
                    Abbreviation = "IBTC",
                    Slug = "IBTC"
                }, new Currency
                {
                    Id = 831,
                    CurrencyTypeId = 2,
                    Name = "Wild Beast Block",
                    Abbreviation = "WBB",
                    Slug = "WBB"
                }, new Currency
                {
                    Id = 3308,
                    CurrencyTypeId = 2,
                    Name = "Xchange",
                    Abbreviation = "XCG",
                    Slug = "XCG"
                }, new Currency
                {
                    Id = 1824,
                    CurrencyTypeId = 2,
                    Name = "BitCoal",
                    Abbreviation = "COAL",
                    Slug = "COAL"
                }, new Currency
                {
                    Id = 1825,
                    CurrencyTypeId = 2,
                    Name = "LiteBitcoin",
                    Abbreviation = "LBTC",
                    Slug = "LBTC"
                }, new Currency
                {
                    Id = 3235,
                    CurrencyTypeId = 2,
                    Name = "FolmCoin",
                    Abbreviation = "FLM",
                    Slug = "FLM"
                }, new Currency
                {
                    Id = 3578,
                    CurrencyTypeId = 2,
                    Name = "CoinToGo",
                    Abbreviation = "2GO",
                    Slug = "2GO"
                }, new Currency
                {
                    Id = 3677,
                    CurrencyTypeId = 2,
                    Name = "ROIyal Coin",
                    Abbreviation = "ROCO",
                    Slug = "ROCO"
                }, new Currency
                {
                    Id = 3394,
                    CurrencyTypeId = 2,
                    Name = "Ourcoin",
                    Abbreviation = "OUR",
                    Slug = "OUR"
                }, new Currency
                {
                    Id = 2214,
                    CurrencyTypeId = 2,
                    Name = "ZoZoCoin",
                    Abbreviation = "ZZC",
                    Slug = "ZZC"
                }, new Currency
                {
                    Id = 986,
                    CurrencyTypeId = 2,
                    Name = "CrevaCoin",
                    Abbreviation = "CREVA",
                    Slug = "CREVA"
                }, new Currency
                {
                    Id = 2115,
                    CurrencyTypeId = 2,
                    Name = "PlayerCoin",
                    Abbreviation = "PLACO",
                    Slug = "PLACO"
                }, new Currency
                {
                    Id = 2581,
                    CurrencyTypeId = 2,
                    Name = "Sharpe Platform Token",
                    Abbreviation = "SHP",
                    Slug = "SHP"
                }, new Currency
                {
                    Id = 1259,
                    CurrencyTypeId = 2,
                    Name = "PonziCoin",
                    Abbreviation = "PONZI",
                    Slug = "PONZI"
                }, new Currency
                {
                    Id = 1632,
                    CurrencyTypeId = 2,
                    Name = "Concoin",
                    Abbreviation = "CONX",
                    Slug = "CONX"
                }, new Currency
                {
                    Id = 1523,
                    CurrencyTypeId = 2,
                    Name = "Magnum",
                    Abbreviation = "MGM",
                    Slug = "MGM"
                }, new Currency
                {
                    Id = 3232,
                    CurrencyTypeId = 2,
                    Name = "Staker",
                    Abbreviation = "STR",
                    Slug = "STR"
                }, new Currency
                {
                    Id = 1691,
                    CurrencyTypeId = 2,
                    Name = "Project-X",
                    Abbreviation = "NANOX",
                    Slug = "NANOX"
                }, new Currency
                {
                    Id = 994,
                    CurrencyTypeId = 2,
                    Name = "AnarchistsPrime",
                    Abbreviation = "ACP",
                    Slug = "ACP"
                }, new Currency
                {
                    Id = 1630,
                    CurrencyTypeId = 2,
                    Name = "Coinonat",
                    Abbreviation = "CXT",
                    Slug = "CXT"
                }, new Currency
                {
                    Id = 3481,
                    CurrencyTypeId = 2,
                    Name = "Peony",
                    Abbreviation = "PNY",
                    Slug = "PNY"
                }, new Currency
                {
                    Id = 1090,
                    CurrencyTypeId = 2,
                    Name = "Save and Gain",
                    Abbreviation = "SANDG",
                    Slug = "SANDG"
                }, new Currency
                {
                    Id = 3384,
                    CurrencyTypeId = 2,
                    Name = "Benz",
                    Abbreviation = "BENZ",
                    Slug = "BENZ"
                }, new Currency
                {
                    Id = 1994,
                    CurrencyTypeId = 2,
                    Name = "Interzone",
                    Abbreviation = "ITZ",
                    Slug = "ITZ"
                }, new Currency
                {
                    Id = 3025,
                    CurrencyTypeId = 2,
                    Name = "ACRE",
                    Abbreviation = "ACRE",
                    Slug = "ACRE"
                }, new Currency
                {
                    Id = 3494,
                    CurrencyTypeId = 2,
                    Name = "Rocketcoin",
                    Abbreviation = "ROCK",
                    Slug = "ROCK"
                }, new Currency
                {
                    Id = 3486,
                    CurrencyTypeId = 2,
                    Name = "dietbitcoin",
                    Abbreviation = "DDX",
                    Slug = "DDX"
                }, new Currency
                {
                    Id = 1558,
                    CurrencyTypeId = 2,
                    Name = "Argus",
                    Abbreviation = "ARGUS",
                    Slug = "ARGUS"
                }, new Currency
                {
                    Id = 3524,
                    CurrencyTypeId = 2,
                    Name = "MFIT COIN",
                    Abbreviation = "MFIT",
                    Slug = "MFIT"
                }, new Currency
                {
                    Id = 2045,
                    CurrencyTypeId = 2,
                    Name = "Coimatic 3.0",
                    Abbreviation = "CTIC3",
                    Slug = "CTIC3"
                }, new Currency
                {
                    Id = 3391,
                    CurrencyTypeId = 2,
                    Name = "SmartFox",
                    Abbreviation = "FOX",
                    Slug = "FOX"
                }, new Currency
                {
                    Id = 3453,
                    CurrencyTypeId = 2,
                    Name = "CJs",
                    Abbreviation = "CJS",
                    Slug = "CJS"
                }, new Currency
                {
                    Id = 3401,
                    CurrencyTypeId = 2,
                    Name = "SHADE Token",
                    Abbreviation = "SHADE",
                    Slug = "SHADE"
                }, new Currency
                {
                    Id = 2253,
                    CurrencyTypeId = 2,
                    Name = "Jiyo [OLD]",
                    Abbreviation = "JIYO",
                    Slug = "JIYO"
                }, new Currency
                {
                    Id = 2596,
                    CurrencyTypeId = 2,
                    Name = "CK USD",
                    Abbreviation = "CKUSD",
                    Slug = "CKUSD"
                }, new Currency
                {
                    Id = 3766,
                    CurrencyTypeId = 2,
                    Name = "Fatcoin",
                    Abbreviation = "FAT",
                    Slug = "FAT"
                }, new Currency
                {
                    Id = 3897,
                    CurrencyTypeId = 2,
                    Name = "OKB",
                    Abbreviation = "OKB",
                    Slug = "OKB"
                }, new Currency
                {
                    Id = 3351,
                    CurrencyTypeId = 2,
                    Name = "ZB",
                    Abbreviation = "ZB",
                    Slug = "ZB"
                }, new Currency
                {
                    Id = 3930,
                    CurrencyTypeId = 2,
                    Name = "Thunder Token",
                    Abbreviation = "TT",
                    Slug = "TT"
                }, new Currency
                {
                    Id = 3806,
                    CurrencyTypeId = 2,
                    Name = "TigerCash",
                    Abbreviation = "TCH",
                    Slug = "TCH"
                }, new Currency
                {
                    Id = 3860,
                    CurrencyTypeId = 2,
                    Name = "Blockcloud",
                    Abbreviation = "BLOC",
                    Slug = "BLOC"
                }, new Currency
                {
                    Id = 3964,
                    CurrencyTypeId = 2,
                    Name = "Reserve Rights",
                    Abbreviation = "RSR",
                    Slug = "RSR"
                }, new Currency
                {
                    Id = 3891,
                    CurrencyTypeId = 2,
                    Name = "V-Dimension",
                    Abbreviation = "VOLLAR",
                    Slug = "VOLLAR"
                }, new Currency
                {
                    Id = 2280,
                    CurrencyTypeId = 2,
                    Name = "Filecoin [Futures]",
                    Abbreviation = "FIL",
                    Slug = "FIL"
                }, new Currency
                {
                    Id = 3217,
                    CurrencyTypeId = 2,
                    Name = "Ontology Gas",
                    Abbreviation = "ONG",
                    Slug = "ONG"
                }, new Currency
                {
                    Id = 2907,
                    CurrencyTypeId = 2,
                    Name = "Karatgold Coin",
                    Abbreviation = "KBC",
                    Slug = "KBC"
                }, new Currency
                {
                    Id = 1706,
                    CurrencyTypeId = 2,
                    Name = "Aidos Kuneen",
                    Abbreviation = "ADK",
                    Slug = "ADK"
                }, new Currency
                {
                    Id = 3957,
                    CurrencyTypeId = 2,
                    Name = "UNUS SED LEO",
                    Abbreviation = "LEO",
                    Slug = "LEO"
                }, new Currency
                {
                    Id = 3252,
                    CurrencyTypeId = 2,
                    Name = "ShineChain",
                    Abbreviation = "SHE",
                    Slug = "SHE"
                }, new Currency
                {
                    Id = 3914,
                    CurrencyTypeId = 2,
                    Name = "GlitzKoin",
                    Abbreviation = "GTN",
                    Slug = "GTN"
                }, new Currency
                {
                    Id = 2878,
                    CurrencyTypeId = 2,
                    Name = "DigiFinexToken",
                    Abbreviation = "DFT",
                    Slug = "DFT"
                }, new Currency
                {
                    Id = 3295,
                    CurrencyTypeId = 2,
                    Name = "BUMO",
                    Abbreviation = "BU",
                    Slug = "BU"
                }, new Currency
                {
                    Id = 2997,
                    CurrencyTypeId = 2,
                    Name = "DIPNET",
                    Abbreviation = "DPN",
                    Slug = "DPN"
                }, new Currency
                {
                    Id = 2895,
                    CurrencyTypeId = 2,
                    Name = "Coni",
                    Abbreviation = "CONI",
                    Slug = "CONI"
                }, new Currency
                {
                    Id = 3673,
                    CurrencyTypeId = 2,
                    Name = "BitMax Token",
                    Abbreviation = "BTMX",
                    Slug = "BTMX"
                }, new Currency
                {
                    Id = 2441,
                    CurrencyTypeId = 2,
                    Name = "Molecular Future",
                    Abbreviation = "MOF",
                    Slug = "MOF"
                }, new Currency
                {
                    Id = 3188,
                    CurrencyTypeId = 2,
                    Name = "SuperEdge",
                    Abbreviation = "ECT",
                    Slug = "ECT"
                }, new Currency
                {
                    Id = 3236,
                    CurrencyTypeId = 2,
                    Name = "WinToken",
                    Abbreviation = "WIN",
                    Slug = "WIN"
                }, new Currency
                {
                    Id = 3924,
                    CurrencyTypeId = 2,
                    Name = "DREP",
                    Abbreviation = "DREP",
                    Slug = "DREP"
                }, new Currency
                {
                    Id = 3789,
                    CurrencyTypeId = 2,
                    Name = "Boltt Coin ",
                    Abbreviation = "BOLTT",
                    Slug = "BOLTT"
                }, new Currency
                {
                    Id = 2712,
                    CurrencyTypeId = 2,
                    Name = "MyToken",
                    Abbreviation = "MT",
                    Slug = "MT"
                }, new Currency
                {
                    Id = 3883,
                    CurrencyTypeId = 2,
                    Name = "QuickX Protocol",
                    Abbreviation = "QCX",
                    Slug = "QCX"
                }, new Currency
                {
                    Id = 3704,
                    CurrencyTypeId = 2,
                    Name = "V Systems",
                    Abbreviation = "VSYS",
                    Slug = "VSYS"
                }, new Currency
                {
                    Id = 2947,
                    CurrencyTypeId = 2,
                    Name = "SoPay",
                    Abbreviation = "SOP",
                    Slug = "SOP"
                }, new Currency
                {
                    Id = 3283,
                    CurrencyTypeId = 2,
                    Name = "FOIN",
                    Abbreviation = "FOIN",
                    Slug = "FOIN"
                }, new Currency
                {
                    Id = 3962,
                    CurrencyTypeId = 2,
                    Name = "Vodi X",
                    Abbreviation = "VDX",
                    Slug = "VDX"
                }, new Currency
                {
                    Id = 3653,
                    CurrencyTypeId = 2,
                    Name = "Baer Chain",
                    Abbreviation = "BRC",
                    Slug = "BRC"
                }, new Currency
                {
                    Id = 2941,
                    CurrencyTypeId = 2,
                    Name = "CoinEx Token",
                    Abbreviation = "CET",
                    Slug = "CET"
                }, new Currency
                {
                    Id = 3829,
                    CurrencyTypeId = 2,
                    Name = "Nash Exchange",
                    Abbreviation = "NEX",
                    Slug = "NEX"
                }, new Currency
                {
                    Id = 3296,
                    CurrencyTypeId = 2,
                    Name = "MINDOL",
                    Abbreviation = "MIN",
                    Slug = "MIN"
                }, new Currency
                {
                    Id = 2282,
                    CurrencyTypeId = 2,
                    Name = "Super Bitcoin",
                    Abbreviation = "SBTC",
                    Slug = "SBTC"
                }, new Currency
                {
                    Id = 3053,
                    CurrencyTypeId = 2,
                    Name = "YOU COIN",
                    Abbreviation = "YOU",
                    Slug = "YOU"
                }, new Currency
                {
                    Id = 3937,
                    CurrencyTypeId = 2,
                    Name = "NNB Token",
                    Abbreviation = "NNB",
                    Slug = "NNB"
                }, new Currency
                {
                    Id = 2432,
                    CurrencyTypeId = 2,
                    Name = "StarChain",
                    Abbreviation = "STC",
                    Slug = "STC"
                }, new Currency
                {
                    Id = 3721,
                    CurrencyTypeId = 2,
                    Name = "Huobi Pool Token",
                    Abbreviation = "HPT",
                    Slug = "HPT"
                }, new Currency
                {
                    Id = 3182,
                    CurrencyTypeId = 2,
                    Name = "HitChain",
                    Abbreviation = "HIT",
                    Slug = "HIT"
                }, new Currency
                {
                    Id = 3874,
                    CurrencyTypeId = 2,
                    Name = "IRISnet",
                    Abbreviation = "IRIS",
                    Slug = "IRIS"
                }, new Currency
                {
                    Id = 3846,
                    CurrencyTypeId = 2,
                    Name = "VeriBlock",
                    Abbreviation = "VBK",
                    Slug = "VBK"
                }, new Currency
                {
                    Id = 3832,
                    CurrencyTypeId = 2,
                    Name = "Big Bang Game Coin",
                    Abbreviation = "BBGC",
                    Slug = "BBGC"
                }, new Currency
                {
                    Id = 3012,
                    CurrencyTypeId = 2,
                    Name = "VeThor Token",
                    Abbreviation = "VTHO",
                    Slug = "VTHO"
                }, new Currency
                {
                    Id = 3485,
                    CurrencyTypeId = 2,
                    Name = "Game Stars",
                    Abbreviation = "GST",
                    Slug = "GST"
                }, new Currency
                {
                    Id = 3934,
                    CurrencyTypeId = 2,
                    Name = "CNNS",
                    Abbreviation = "CNNS",
                    Slug = "CNNS"
                }, new Currency
                {
                    Id = 2740,
                    CurrencyTypeId = 2,
                    Name = "Influence Chain",
                    Abbreviation = "INC",
                    Slug = "INC"
                }, new Currency
                {
                    Id = 2454,
                    CurrencyTypeId = 2,
                    Name = "UnlimitedIP",
                    Abbreviation = "UIP",
                    Slug = "UIP"
                }, new Currency
                {
                    Id = 3196,
                    CurrencyTypeId = 2,
                    Name = "KNOW",
                    Abbreviation = "KNOW",
                    Slug = "KNOW"
                }, new Currency
                {
                    Id = 2376,
                    CurrencyTypeId = 2,
                    Name = "TopChain",
                    Abbreviation = "TOPC",
                    Slug = "TOPC"
                }, new Currency
                {
                    Id = 2456,
                    CurrencyTypeId = 2,
                    Name = "OFCOIN",
                    Abbreviation = "OF",
                    Slug = "OF"
                }, new Currency
                {
                    Id = 3620,
                    CurrencyTypeId = 2,
                    Name = "Atlas Protocol",
                    Abbreviation = "ATP",
                    Slug = "ATP"
                }, new Currency
                {
                    Id = 3791,
                    CurrencyTypeId = 2,
                    Name = "Jewel",
                    Abbreviation = "JWL",
                    Slug = "JWL"
                }, new Currency
                {
                    Id = 3347,
                    CurrencyTypeId = 2,
                    Name = "CARAT",
                    Abbreviation = "CARAT",
                    Slug = "CARAT"
                }, new Currency
                {
                    Id = 2981,
                    CurrencyTypeId = 2,
                    Name = "Consentium",
                    Abbreviation = "CSM",
                    Slug = "CSM"
                }, new Currency
                {
                    Id = 2435,
                    CurrencyTypeId = 2,
                    Name = "LightChain",
                    Abbreviation = "LIGHT",
                    Slug = "LIGHT"
                }, new Currency
                {
                    Id = 3875,
                    CurrencyTypeId = 2,
                    Name = "Valor Token",
                    Abbreviation = "VALOR",
                    Slug = "VALOR"
                }, new Currency
                {
                    Id = 2987,
                    CurrencyTypeId = 2,
                    Name = "ThingsOperatingSystem",
                    Abbreviation = "TOS",
                    Slug = "TOS"
                }, new Currency
                {
                    Id = 3880,
                    CurrencyTypeId = 2,
                    Name = "OceanEx Token",
                    Abbreviation = "OCE",
                    Slug = "OCE"
                }, new Currency
                {
                    Id = 2741,
                    CurrencyTypeId = 2,
                    Name = "Intelligent Investment Chain",
                    Abbreviation = "IIC",
                    Slug = "IIC"
                }, new Currency
                {
                    Id = 2846,
                    CurrencyTypeId = 2,
                    Name = "FuturoCoin",
                    Abbreviation = "FTO",
                    Slug = "FTO"
                }, new Currency
                {
                    Id = 3734,
                    CurrencyTypeId = 2,
                    Name = "ELA Coin",
                    Abbreviation = "ELAC",
                    Slug = "ELAC"
                }, new Currency
                {
                    Id = 3795,
                    CurrencyTypeId = 2,
                    Name = "ZEON",
                    Abbreviation = "ZEON",
                    Slug = "ZEON"
                }, new Currency
                {
                    Id = 2396,
                    CurrencyTypeId = 2,
                    Name = "WETH",
                    Abbreviation = "WETH",
                    Slug = "WETH"
                }, new Currency
                {
                    Id = 3060,
                    CurrencyTypeId = 2,
                    Name = "Yuan Chain Coin",
                    Abbreviation = "YCC",
                    Slug = "YCC"
                }, new Currency
                {
                    Id = 3888,
                    CurrencyTypeId = 2,
                    Name = "bitCEO",
                    Abbreviation = "BCEO",
                    Slug = "BCEO"
                }, new Currency
                {
                    Id = 3660,
                    CurrencyTypeId = 2,
                    Name = "USDCoin",
                    Abbreviation = "USC",
                    Slug = "USC"
                }, new Currency
                {
                    Id = 3797,
                    CurrencyTypeId = 2,
                    Name = "Bitex Global XBX Coin",
                    Abbreviation = "XBX",
                    Slug = "XBX"
                }, new Currency
                {
                    Id = 2928,
                    CurrencyTypeId = 2,
                    Name = "PlayCoin [QRC20]",
                    Abbreviation = "PLY",
                    Slug = "PLY"
                }, new Currency
                {
                    Id = 3259,
                    CurrencyTypeId = 2,
                    Name = "YouLive Coin",
                    Abbreviation = "UC",
                    Slug = "UC"
                }, new Currency
                {
                    Id = 3938,
                    CurrencyTypeId = 2,
                    Name = "Muzika",
                    Abbreviation = "MZK",
                    Slug = "MZK"
                }, new Currency
                {
                    Id = 2091,
                    CurrencyTypeId = 2,
                    Name = "Exchange Union",
                    Abbreviation = "XUC",
                    Slug = "XUC"
                }, new Currency
                {
                    Id = 2871,
                    CurrencyTypeId = 2,
                    Name = "Ubique Chain Of Things",
                    Abbreviation = "UCT",
                    Slug = "UCT"
                }, new Currency
                {
                    Id = 3707,
                    CurrencyTypeId = 2,
                    Name = "T.OS",
                    Abbreviation = "TOSC",
                    Slug = "TOSC"
                }, new Currency
                {
                    Id = 2969,
                    CurrencyTypeId = 2,
                    Name = "Globalvillage Ecosystem",
                    Abbreviation = "GVE",
                    Slug = "GVE"
                }, new Currency
                {
                    Id = 3872,
                    CurrencyTypeId = 2,
                    Name = "Bilaxy Token",
                    Abbreviation = "BIA",
                    Slug = "BIA"
                }, new Currency
                {
                    Id = 3873,
                    CurrencyTypeId = 2,
                    Name = "botXcoin",
                    Abbreviation = "BOTX",
                    Slug = "BOTX"
                }, new Currency
                {
                    Id = 3858,
                    CurrencyTypeId = 2,
                    Name = "FNB Protocol",
                    Abbreviation = "FNB",
                    Slug = "FNB"
                }, new Currency
                {
                    Id = 3803,
                    CurrencyTypeId = 2,
                    Name = "Diruna",
                    Abbreviation = "DRA",
                    Slug = "DRA"
                }, new Currency
                {
                    Id = 2361,
                    CurrencyTypeId = 2,
                    Name = "Show",
                    Abbreviation = "SHOW",
                    Slug = "SHOW"
                }, new Currency
                {
                    Id = 3970,
                    CurrencyTypeId = 2,
                    Name = "Trias",
                    Abbreviation = "TRY",
                    Slug = "TRY"
                }, new Currency
                {
                    Id = 3061,
                    CurrencyTypeId = 2,
                    Name = "Promotion Coin",
                    Abbreviation = "PC",
                    Slug = "PC"
                }, new Currency
                {
                    Id = 2950,
                    CurrencyTypeId = 2,
                    Name = "LemoChain",
                    Abbreviation = "LEMO",
                    Slug = "LEMO"
                }, new Currency
                {
                    Id = 3898,
                    CurrencyTypeId = 2,
                    Name = "AXE",
                    Abbreviation = "AXE",
                    Slug = "AXE"
                }, new Currency
                {
                    Id = 2734,
                    CurrencyTypeId = 2,
                    Name = "EduCoin",
                    Abbreviation = "EDU",
                    Slug = "EDU"
                }, new Currency
                {
                    Id = 3929,
                    CurrencyTypeId = 2,
                    Name = "BQT",
                    Abbreviation = "BQTX",
                    Slug = "BQTX"
                }, new Currency
                {
                    Id = 2434,
                    CurrencyTypeId = 2,
                    Name = "Maggie",
                    Abbreviation = "MAG",
                    Slug = "MAG"
                }, new Currency
                {
                    Id = 3595,
                    CurrencyTypeId = 2,
                    Name = "PalletOne",
                    Abbreviation = "PTN",
                    Slug = "PTN"
                }, new Currency
                {
                    Id = 2852,
                    CurrencyTypeId = 2,
                    Name = "Engine",
                    Abbreviation = "EGCC",
                    Slug = "EGCC"
                }, new Currency
                {
                    Id = 2914,
                    CurrencyTypeId = 2,
                    Name = "BeeKan",
                    Abbreviation = "BKBT",
                    Slug = "BKBT"
                }, new Currency
                {
                    Id = 3258,
                    CurrencyTypeId = 2,
                    Name = "BitUP Token",
                    Abbreviation = "BUT",
                    Slug = "BUT"
                }, new Currency
                {
                    Id = 3320,
                    CurrencyTypeId = 2,
                    Name = "TCOIN",
                    Abbreviation = "TCN",
                    Slug = "TCN"
                }, new Currency
                {
                    Id = 3922,
                    CurrencyTypeId = 2,
                    Name = "Tarush",
                    Abbreviation = "TAS",
                    Slug = "TAS"
                }, new Currency
                {
                    Id = 2247,
                    CurrencyTypeId = 2,
                    Name = "BlockCDN",
                    Abbreviation = "BCDN",
                    Slug = "BCDN"
                }, new Currency
                {
                    Id = 3004,
                    CurrencyTypeId = 2,
                    Name = "Volt",
                    Abbreviation = "ACDC",
                    Slug = "ACDC"
                }, new Currency
                {
                    Id = 3825,
                    CurrencyTypeId = 2,
                    Name = "PUBLYTO Token",
                    Abbreviation = "PUB",
                    Slug = "PUB"
                }, new Currency
                {
                    Id = 3958,
                    CurrencyTypeId = 2,
                    Name = "RedFOX Labs",
                    Abbreviation = "RFOX",
                    Slug = "RFOX"
                }, new Currency
                {
                    Id = 3831,
                    CurrencyTypeId = 2,
                    Name = "Safe Haven",
                    Abbreviation = "SHA",
                    Slug = "SHA"
                }, new Currency
                {
                    Id = 3950,
                    CurrencyTypeId = 2,
                    Name = "Netrum",
                    Abbreviation = "NTR",
                    Slug = "NTR"
                }, new Currency
                {
                    Id = 3153,
                    CurrencyTypeId = 2,
                    Name = "Twinkle",
                    Abbreviation = "TKT",
                    Slug = "TKT"
                }, new Currency
                {
                    Id = 3090,
                    CurrencyTypeId = 2,
                    Name = "Wiki Token",
                    Abbreviation = "WIKI",
                    Slug = "WIKI"
                }, new Currency
                {
                    Id = 2986,
                    CurrencyTypeId = 2,
                    Name = "DACC",
                    Abbreviation = "DACC",
                    Slug = "DACC"
                }, new Currency
                {
                    Id = 2962,
                    CurrencyTypeId = 2,
                    Name = "CHEX",
                    Abbreviation = "CHEX",
                    Slug = "CHEX"
                }, new Currency
                {
                    Id = 3626,
                    CurrencyTypeId = 2,
                    Name = "RSK Smart Bitcoin",
                    Abbreviation = "RBTC",
                    Slug = "RBTC"
                }, new Currency
                {
                    Id = 2408,
                    CurrencyTypeId = 2,
                    Name = "Qube",
                    Abbreviation = "QUBE",
                    Slug = "QUBE"
                }, new Currency
                {
                    Id = 3286,
                    CurrencyTypeId = 2,
                    Name = "MEX",
                    Abbreviation = "MEX",
                    Slug = "MEX"
                }, new Currency
                {
                    Id = 2713,
                    CurrencyTypeId = 2,
                    Name = "KEY",
                    Abbreviation = "KEY",
                    Slug = "KEY"
                }, new Currency
                {
                    Id = 3967,
                    CurrencyTypeId = 2,
                    Name = "Eva Cash",
                    Abbreviation = "EVC",
                    Slug = "EVC"
                }, new Currency
                {
                    Id = 3780,
                    CurrencyTypeId = 2,
                    Name = "Sparkle",
                    Abbreviation = "SPRKL",
                    Slug = "SPRKL"
                }, new Currency
                {
                    Id = 3918,
                    CurrencyTypeId = 2,
                    Name = "Safe",
                    Abbreviation = "SAFE",
                    Slug = "SAFE"
                }, new Currency
                {
                    Id = 2736,
                    CurrencyTypeId = 2,
                    Name = "InsurChain",
                    Abbreviation = "INSUR",
                    Slug = "INSUR"
                }, new Currency
                {
                    Id = 2293,
                    CurrencyTypeId = 2,
                    Name = "United Bitcoin",
                    Abbreviation = "UBTC",
                    Slug = "UBTC"
                }, new Currency
                {
                    Id = 2281,
                    CurrencyTypeId = 2,
                    Name = "BitcoinX",
                    Abbreviation = "BCX",
                    Slug = "BCX"
                }, new Currency
                {
                    Id = 3839,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Rhodium",
                    Abbreviation = "XRC",
                    Slug = "XRC"
                }, new Currency
                {
                    Id = 3834,
                    CurrencyTypeId = 2,
                    Name = "Dexter G",
                    Abbreviation = "DXG",
                    Slug = "DXG"
                }, new Currency
                {
                    Id = 3522,
                    CurrencyTypeId = 2,
                    Name = "MESSE TOKEN",
                    Abbreviation = "MESSE",
                    Slug = "MESSE"
                }, new Currency
                {
                    Id = 2440,
                    CurrencyTypeId = 2,
                    Name = "Read",
                    Abbreviation = "READ",
                    Slug = "READ"
                }, new Currency
                {
                    Id = 3844,
                    CurrencyTypeId = 2,
                    Name = "Xtock",
                    Abbreviation = "XTX",
                    Slug = "XTX"
                }, new Currency
                {
                    Id = 3857,
                    CurrencyTypeId = 2,
                    Name = "GoldenFever",
                    Abbreviation = "GFR",
                    Slug = "GFR"
                }, new Currency
                {
                    Id = 3736,
                    CurrencyTypeId = 2,
                    Name = "Marginless",
                    Abbreviation = "MRS",
                    Slug = "MRS"
                }, new Currency
                {
                    Id = 3277,
                    CurrencyTypeId = 2,
                    Name = "vSportCoin",
                    Abbreviation = "VSC",
                    Slug = "VSC"
                }, new Currency
                {
                    Id = 3249,
                    CurrencyTypeId = 2,
                    Name = "Usechain Token",
                    Abbreviation = "USE",
                    Slug = "USE"
                }, new Currency
                {
                    Id = 2903,
                    CurrencyTypeId = 2,
                    Name = "SEER",
                    Abbreviation = "SEER",
                    Slug = "SEER"
                }, new Currency
                {
                    Id = 3717,
                    CurrencyTypeId = 2,
                    Name = "Wrapped Bitcoin",
                    Abbreviation = "WBTC",
                    Slug = "WBTC"
                }, new Currency
                {
                    Id = 2919,
                    CurrencyTypeId = 2,
                    Name = "DWS",
                    Abbreviation = "DWS",
                    Slug = "DWS"
                }, new Currency
                {
                    Id = 3231,
                    CurrencyTypeId = 2,
                    Name = "Blockchain Quotations Index Token",
                    Abbreviation = "BQT",
                    Slug = "BQT"
                }, new Currency
                {
                    Id = 3948,
                    CurrencyTypeId = 2,
                    Name = "TERA",
                    Abbreviation = "TERA",
                    Slug = "TERA"
                }, new Currency
                {
                    Id = 2406,
                    CurrencyTypeId = 2,
                    Name = "InvestDigital",
                    Abbreviation = "IDT",
                    Slug = "IDT"
                }, new Currency
                {
                    Id = 3582,
                    CurrencyTypeId = 2,
                    Name = "MediBit",
                    Abbreviation = "MEDIBIT",
                    Slug = "MEDIBIT"
                }, new Currency
                {
                    Id = 3925,
                    CurrencyTypeId = 2,
                    Name = "Tratok",
                    Abbreviation = "TRAT",
                    Slug = "TRAT"
                }, new Currency
                {
                    Id = 3640,
                    CurrencyTypeId = 2,
                    Name = "Livepeer",
                    Abbreviation = "LPT",
                    Slug = "LPT"
                }, new Currency
                {
                    Id = 3720,
                    CurrencyTypeId = 2,
                    Name = "SDUSD",
                    Abbreviation = "SDUSD",
                    Slug = "SDUSD"
                }, new Currency
                {
                    Id = 3002,
                    CurrencyTypeId = 2,
                    Name = "Master Contract Token",
                    Abbreviation = "MCT",
                    Slug = "MCT"
                }, new Currency
                {
                    Id = 3074,
                    CurrencyTypeId = 2,
                    Name = "Experience Token",
                    Abbreviation = "EXT",
                    Slug = "EXT"
                }, new Currency
                {
                    Id = 3744,
                    CurrencyTypeId = 2,
                    Name = "WEBN token",
                    Abbreviation = "WEBN",
                    Slug = "WEBN"
                }, new Currency
                {
                    Id = 3790,
                    CurrencyTypeId = 2,
                    Name = "RoboCalls",
                    Abbreviation = "RC20",
                    Slug = "RC20"
                }, new Currency
                {
                    Id = 3912,
                    CurrencyTypeId = 2,
                    Name = "W Green Pay",
                    Abbreviation = "WGP",
                    Slug = "WGP"
                }, new Currency
                {
                    Id = 2670,
                    CurrencyTypeId = 2,
                    Name = "Pixie Coin",
                    Abbreviation = "PXC",
                    Slug = "PXC"
                }, new Currency
                {
                    Id = 3123,
                    CurrencyTypeId = 2,
                    Name = "GSENetwork",
                    Abbreviation = "GSE",
                    Slug = "GSE"
                }, new Currency
                {
                    Id = 1037,
                    CurrencyTypeId = 2,
                    Name = "Agoras Tokens",
                    Abbreviation = "AGRS",
                    Slug = "AGRS"
                }, new Currency
                {
                    Id = 3105,
                    CurrencyTypeId = 2,
                    Name = "Atlantis Blue Digital Token",
                    Abbreviation = "ABDT",
                    Slug = "ABDT"
                }, new Currency
                {
                    Id = 3855,
                    CurrencyTypeId = 2,
                    Name = "Locus Chain",
                    Abbreviation = "LOCUS",
                    Slug = "LOCUS"
                }, new Currency
                {
                    Id = 2858,
                    CurrencyTypeId = 2,
                    Name = "Couchain",
                    Abbreviation = "COU",
                    Slug = "COU"
                }, new Currency
                {
                    Id = 3885,
                    CurrencyTypeId = 2,
                    Name = "WPP TOKEN",
                    Abbreviation = "WPP",
                    Slug = "WPP"
                }, new Currency
                {
                    Id = 3670,
                    CurrencyTypeId = 2,
                    Name = "ROMToken",
                    Abbreviation = "ROM",
                    Slug = "ROM"
                }, new Currency
                {
                    Id = 3951,
                    CurrencyTypeId = 2,
                    Name = "Pirate Chain",
                    Abbreviation = "ARRR",
                    Slug = "ARRR"
                }, new Currency
                {
                    Id = 3198,
                    CurrencyTypeId = 2,
                    Name = "KingXChain",
                    Abbreviation = "KXC",
                    Slug = "KXC"
                }, new Currency
                {
                    Id = 3910,
                    CurrencyTypeId = 2,
                    Name = "pEOS",
                    Abbreviation = "PEOS",
                    Slug = "PEOS"
                }, new Currency
                {
                    Id = 3292,
                    CurrencyTypeId = 2,
                    Name = "CariNet",
                    Abbreviation = "CIT",
                    Slug = "CIT"
                }, new Currency
                {
                    Id = 3448,
                    CurrencyTypeId = 2,
                    Name = "Commerce Data Connection",
                    Abbreviation = "CDC",
                    Slug = "CDC"
                }, new Currency
                {
                    Id = 3848,
                    CurrencyTypeId = 2,
                    Name = "OOOBTC TOKEN",
                    Abbreviation = "OBX",
                    Slug = "OBX"
                }, new Currency
                {
                    Id = 3257,
                    CurrencyTypeId = 2,
                    Name = "GazeCoin",
                    Abbreviation = "GZE",
                    Slug = "GZE"
                }, new Currency
                {
                    Id = 3127,
                    CurrencyTypeId = 2,
                    Name = "Themis",
                    Abbreviation = "GET",
                    Slug = "themis"
                }, new Currency
                {
                    Id = 3693,
                    CurrencyTypeId = 2,
                    Name = "M2O",
                    Abbreviation = "M2O",
                    Slug = "M2O"
                }, new Currency
                {
                    Id = 2999,
                    CurrencyTypeId = 2,
                    Name = "Hdac",
                    Abbreviation = "HDAC",
                    Slug = "HDAC"
                }, new Currency
                {
                    Id = 2700,
                    CurrencyTypeId = 2,
                    Name = "Celsius",
                    Abbreviation = "CEL",
                    Slug = "CEL"
                }, new Currency
                {
                    Id = 3328,
                    CurrencyTypeId = 2,
                    Name = "CMITCOIN",
                    Abbreviation = "CMIT",
                    Slug = "CMIT"
                }, new Currency
                {
                    Id = 3940,
                    CurrencyTypeId = 2,
                    Name = "P2P Global Network",
                    Abbreviation = "P2PX",
                    Slug = "P2PX"
                }, new Currency
                {
                    Id = 2383,
                    CurrencyTypeId = 2,
                    Name = "Jingtum Tech",
                    Abbreviation = "SWTC",
                    Slug = "SWTC"
                }, new Currency
                {
                    Id = 3926,
                    CurrencyTypeId = 2,
                    Name = "Stellar Gold",
                    Abbreviation = "XLMG",
                    Slug = "XLMG"
                }, new Currency
                {
                    Id = 3819,
                    CurrencyTypeId = 2,
                    Name = "GAMB",
                    Abbreviation = "GMB",
                    Slug = "GAMB"
                }, new Currency
                {
                    Id = 3923,
                    CurrencyTypeId = 2,
                    Name = "VENJOCOIN",
                    Abbreviation = "VJC",
                    Slug = "VJC"
                }, new Currency
                {
                    Id = 2593,
                    CurrencyTypeId = 2,
                    Name = "Dragon Coins",
                    Abbreviation = "DRG",
                    Slug = "DRG"
                }, new Currency
                {
                    Id = 3356,
                    CurrencyTypeId = 2,
                    Name = "The Midas Touch Gold",
                    Abbreviation = "TMTG",
                    Slug = "TMTG"
                }, new Currency
                {
                    Id = 3861,
                    CurrencyTypeId = 2,
                    Name = "Infinitus Token",
                    Abbreviation = "INF",
                    Slug = "INF"
                }, new Currency
                {
                    Id = 2894,
                    CurrencyTypeId = 2,
                    Name = "OTCBTC Token",
                    Abbreviation = "OTB",
                    Slug = "OTB"
                }, new Currency
                {
                    Id = 3667,
                    CurrencyTypeId = 2,
                    Name = "Atomic Wallet Coin",
                    Abbreviation = "AWC",
                    Slug = "AWC"
                }, new Currency
                {
                    Id = 3681,
                    CurrencyTypeId = 2,
                    Name = "CENTERCOIN",
                    Abbreviation = "CENT",
                    Slug = "CENT"
                }, new Currency
                {
                    Id = 3968,
                    CurrencyTypeId = 2,
                    Name = "Elitium",
                    Abbreviation = "EUM",
                    Slug = "EUM"
                }, new Currency
                {
                    Id = 3076,
                    CurrencyTypeId = 2,
                    Name = "Endorsit",
                    Abbreviation = "EDS",
                    Slug = "EDS"
                }, new Currency
                {
                    Id = 2370,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin God",
                    Abbreviation = "GOD",
                    Slug = "GOD"
                }, new Currency
                {
                    Id = 918,
                    CurrencyTypeId = 2,
                    Name = "Bubble",
                    Abbreviation = "BUB",
                    Slug = "BUB"
                }, new Currency
                {
                    Id = 3820,
                    CurrencyTypeId = 2,
                    Name = "BuckHathCoin",
                    Abbreviation = "BHIG",
                    Slug = "BHIG"
                }, new Currency
                {
                    Id = 3941,
                    CurrencyTypeId = 2,
                    Name = "Fast Access Blockchain",
                    Abbreviation = "FAB",
                    Slug = "FAB"
                }, new Currency
                {
                    Id = 2618,
                    CurrencyTypeId = 2,
                    Name = "StockChain",
                    Abbreviation = "SCC",
                    Slug = "stockchain"
                }, new Currency
                {
                    Id = 3881,
                    CurrencyTypeId = 2,
                    Name = "BitStash",
                    Abbreviation = "STASH",
                    Slug = "STASH"
                }, new Currency
                {
                    Id = 3896,
                    CurrencyTypeId = 2,
                    Name = "HoryouToken",
                    Abbreviation = "HYT",
                    Slug = "HYT"
                }, new Currency
                {
                    Id = 1135,
                    CurrencyTypeId = 2,
                    Name = "ClubCoin",
                    Abbreviation = "CLUB",
                    Slug = "CLUB"
                }, new Currency
                {
                    Id = 2262,
                    CurrencyTypeId = 2,
                    Name = "COMSA [ETH]",
                    Abbreviation = "CMS",
                    Slug = "CMS"
                }, new Currency
                {
                    Id = 3193,
                    CurrencyTypeId = 2,
                    Name = "RRCoin",
                    Abbreviation = "RRC",
                    Slug = "RRC"
                }, new Currency
                {
                    Id = 3745,
                    CurrencyTypeId = 2,
                    Name = "BiNGO.Fun",
                    Abbreviation = "777",
                    Slug = "777"
                }, new Currency
                {
                    Id = 2888,
                    CurrencyTypeId = 2,
                    Name = "CarBlock",
                    Abbreviation = "CAR",
                    Slug = "CAR"
                }, new Currency
                {
                    Id = 3955,
                    CurrencyTypeId = 2,
                    Name = "Rentoo",
                    Abbreviation = "RENTOO",
                    Slug = "RENTOO"
                }, new Currency
                {
                    Id = 3759,
                    CurrencyTypeId = 2,
                    Name = "Jinbi Token",
                    Abbreviation = "JNB",
                    Slug = "JNB"
                }, new Currency
                {
                    Id = 2655,
                    CurrencyTypeId = 2,
                    Name = "Monero Classic",
                    Abbreviation = "XMC",
                    Slug = "XMC"
                }, new Currency
                {
                    Id = 3117,
                    CurrencyTypeId = 2,
                    Name = "Social Lending Token",
                    Abbreviation = "SLT",
                    Slug = "social-lending-token"
                }, new Currency
                {
                    Id = 3956,
                    CurrencyTypeId = 2,
                    Name = "BOMB",
                    Abbreviation = "BOMB",
                    Slug = "BOMB"
                }, new Currency
                {
                    Id = 3907,
                    CurrencyTypeId = 2,
                    Name = "BitCash",
                    Abbreviation = "BITC",
                    Slug = "BITC"
                }, new Currency
                {
                    Id = 3507,
                    CurrencyTypeId = 2,
                    Name = "MicroBitcoin",
                    Abbreviation = "MBC",
                    Slug = "MBC"
                }, new Currency
                {
                    Id = 3318,
                    CurrencyTypeId = 2,
                    Name = "Countinghouse",
                    Abbreviation = "CHT",
                    Slug = "CHT"
                }, new Currency
                {
                    Id = 2483,
                    CurrencyTypeId = 2,
                    Name = "OceanChain",
                    Abbreviation = "OC",
                    Slug = "OC"
                }, new Currency
                {
                    Id = 3477,
                    CurrencyTypeId = 2,
                    Name = "Asura Coin",
                    Abbreviation = "ASA",
                    Slug = "ASA"
                }, new Currency
                {
                    Id = 3776,
                    CurrencyTypeId = 2,
                    Name = "GoldFund",
                    Abbreviation = "GFUN",
                    Slug = "GFUN"
                }, new Currency
                {
                    Id = 3110,
                    CurrencyTypeId = 2,
                    Name = "NewsToken",
                    Abbreviation = "NEWOS",
                    Slug = "NEWOS"
                }, new Currency
                {
                    Id = 3417,
                    CurrencyTypeId = 2,
                    Name = "Future1coin",
                    Abbreviation = "F1C",
                    Slug = "F1C"
                }, new Currency
                {
                    Id = 2266,
                    CurrencyTypeId = 2,
                    Name = "COMSA [XEM]",
                    Abbreviation = "CMS",
                    Slug = "CMS-XEM"
                }, new Currency
                {
                    Id = 3369,
                    CurrencyTypeId = 2,
                    Name = "Kuende",
                    Abbreviation = "KUE",
                    Slug = "KUE"
                }, new Currency
                {
                    Id = 3936,
                    CurrencyTypeId = 2,
                    Name = "GNY",
                    Abbreviation = "GNY",
                    Slug = "GNY"
                }, new Currency
                {
                    Id = 1487,
                    CurrencyTypeId = 2,
                    Name = "Pabyosi Coin (Special)",
                    Abbreviation = "PCS",
                    Slug = "PCS"
                }, new Currency
                {
                    Id = 3895,
                    CurrencyTypeId = 2,
                    Name = "Matrexcoin",
                    Abbreviation = "MAC",
                    Slug = "MAC"
                }, new Currency
                {
                    Id = 3579,
                    CurrencyTypeId = 2,
                    Name = "APOT",
                    Abbreviation = "APOT",
                    Slug = "APOT"
                }, new Currency
                {
                    Id = 3007,
                    CurrencyTypeId = 2,
                    Name = "Haracoin",
                    Abbreviation = "HRC",
                    Slug = "HRC"
                }, new Currency
                {
                    Id = 3630,
                    CurrencyTypeId = 2,
                    Name = "Hercules",
                    Abbreviation = "HERC",
                    Slug = "HERC"
                }, new Currency
                {
                    Id = 3767,
                    CurrencyTypeId = 2,
                    Name = "1X2 COIN",
                    Abbreviation = "1X2",
                    Slug = "1X2"
                }, new Currency
                {
                    Id = 2842,
                    CurrencyTypeId = 2,
                    Name = "Bankera",
                    Abbreviation = "BNK",
                    Slug = "BNK"
                }, new Currency
                {
                    Id = 3380,
                    CurrencyTypeId = 2,
                    Name = "WIZBL",
                    Abbreviation = "WBL",
                    Slug = "WBL"
                }, new Currency
                {
                    Id = 3026,
                    CurrencyTypeId = 2,
                    Name = "Carlive Chain",
                    Abbreviation = "IOV",
                    Slug = "IOV"
                }, new Currency
                {
                    Id = 3253,
                    CurrencyTypeId = 2,
                    Name = "eosBLACK",
                    Abbreviation = "BLACK",
                    Slug = "BLACK"
                }, new Currency
                {
                    Id = 2485,
                    CurrencyTypeId = 2,
                    Name = "Candy",
                    Abbreviation = "CANDY",
                    Slug = "CANDY"
                }, new Currency
                {
                    Id = 2008,
                    CurrencyTypeId = 2,
                    Name = "MSD",
                    Abbreviation = "MSD",
                    Slug = "MSD"
                }, new Currency
                {
                    Id = 3456,
                    CurrencyTypeId = 2,
                    Name = "PlusOneCoin",
                    Abbreviation = "PLUS1",
                    Slug = "PLUS1"
                }, new Currency
                {
                    Id = 3743,
                    CurrencyTypeId = 2,
                    Name = "QUSD",
                    Abbreviation = "QUSD",
                    Slug = "QUSD"
                }, new Currency
                {
                    Id = 3893,
                    CurrencyTypeId = 2,
                    Name = "NOW Token",
                    Abbreviation = "NOW",
                    Slug = "NOW"
                }, new Currency
                {
                    Id = 549,
                    CurrencyTypeId = 2,
                    Name = "Storjcoin X",
                    Abbreviation = "SJCX",
                    Slug = "SJCX"
                }, new Currency
                {
                    Id = 3135,
                    CurrencyTypeId = 2,
                    Name = "CEDEX Coin",
                    Abbreviation = "CEDEX",
                    Slug = "CEDEX"
                }, new Currency
                {
                    Id = 3696,
                    CurrencyTypeId = 2,
                    Name = "SpectrumCash",
                    Abbreviation = "XSM",
                    Slug = "XSM"
                }, new Currency
                {
                    Id = 3865,
                    CurrencyTypeId = 2,
                    Name = "BIZKEY",
                    Abbreviation = "BZKY",
                    Slug = "BZKY"
                }, new Currency
                {
                    Id = 1921,
                    CurrencyTypeId = 2,
                    Name = "SIGMAcoin",
                    Abbreviation = "SIGMA",
                    Slug = "SIGMA"
                }, new Currency
                {
                    Id = 3908,
                    CurrencyTypeId = 2,
                    Name = "Decimated",
                    Abbreviation = "DIO",
                    Slug = "DIO"
                }, new Currency
                {
                    Id = 2654,
                    CurrencyTypeId = 2,
                    Name = "Budbo",
                    Abbreviation = "BUBO",
                    Slug = "BUBO"
                }, new Currency
                {
                    Id = 3290,
                    CurrencyTypeId = 2,
                    Name = "Elliot Coin",
                    Abbreviation = "ELLI",
                    Slug = "ELLI"
                }, new Currency
                {
                    Id = 3067,
                    CurrencyTypeId = 2,
                    Name = "XTRD",
                    Abbreviation = "XTRD",
                    Slug = "XTRD"
                }, new Currency
                {
                    Id = 2522,
                    CurrencyTypeId = 2,
                    Name = "Superior Coin",
                    Abbreviation = "SUP",
                    Slug = "SUP"
                }, new Currency
                {
                    Id = 1809,
                    CurrencyTypeId = 2,
                    Name = "TerraNova",
                    Abbreviation = "TER",
                    Slug = "TER"
                }, new Currency
                {
                    Id = 3326,
                    CurrencyTypeId = 2,
                    Name = "Crypto Harbor Exchange",
                    Abbreviation = "CHE",
                    Slug = "CHE"
                }, new Currency
                {
                    Id = 3939,
                    CurrencyTypeId = 2,
                    Name = "Tronipay",
                    Abbreviation = "TRP",
                    Slug = "TRP"
                }, new Currency
                {
                    Id = 3902,
                    CurrencyTypeId = 2,
                    Name = "MoneroV ",
                    Abbreviation = "XMV",
                    Slug = "XMV"
                }, new Currency
                {
                    Id = 3921,
                    CurrencyTypeId = 2,
                    Name = "Hilux",
                    Abbreviation = "HLX",
                    Slug = "HLX"
                }, new Currency
                {
                    Id = 58,
                    CurrencyTypeId = 2,
                    Name = "Sexcoin",
                    Abbreviation = "SXC",
                    Slug = "SXC"
                }, new Currency
                {
                    Id = 3963,
                    CurrencyTypeId = 2,
                    Name = "DreamTeam",
                    Abbreviation = "DREAM",
                    Slug = "DREAM"
                }, new Currency
                {
                    Id = 2192,
                    CurrencyTypeId = 2,
                    Name = "GOLD Reward Token",
                    Abbreviation = "GRX",
                    Slug = "GRX"
                }, new Currency
                {
                    Id = 2329,
                    CurrencyTypeId = 2,
                    Name = "Hyper Pay",
                    Abbreviation = "HPY",
                    Slug = "HPY"
                }, new Currency
                {
                    Id = 3343,
                    CurrencyTypeId = 2,
                    Name = "ANON",
                    Abbreviation = "ANON",
                    Slug = "ANON"
                }, new Currency
                {
                    Id = 3697,
                    CurrencyTypeId = 2,
                    Name = "Gamblica",
                    Abbreviation = "GMBC",
                    Slug = "GMBC"
                }, new Currency
                {
                    Id = 3726,
                    CurrencyTypeId = 2,
                    Name = "Bidooh DOOH Token",
                    Abbreviation = "DOOH",
                    Slug = "DOOH"
                }, new Currency
                {
                    Id = 2046,
                    CurrencyTypeId = 2,
                    Name = "Bastonet",
                    Abbreviation = "BSN",
                    Slug = "BSN"
                }, new Currency
                {
                    Id = 3208,
                    CurrencyTypeId = 2,
                    Name = "YUKI",
                    Abbreviation = "YUKI",
                    Slug = "YUKI"
                }, new Currency
                {
                    Id = 3069,
                    CurrencyTypeId = 2,
                    Name = "NAM COIN",
                    Abbreviation = "NAM",
                    Slug = "NAM"
                }, new Currency
                {
                    Id = 2609,
                    CurrencyTypeId = 2,
                    Name = "Lendroid Support Token",
                    Abbreviation = "LST",
                    Slug = "LST"
                }, new Currency
                {
                    Id = 3490,
                    CurrencyTypeId = 2,
                    Name = "Valuto",
                    Abbreviation = "VLU",
                    Slug = "VLU"
                }, new Currency
                {
                    Id = 3363,
                    CurrencyTypeId = 2,
                    Name = "WXCOINS",
                    Abbreviation = "WXC",
                    Slug = "WXC"
                }, new Currency
                {
                    Id = 41,
                    CurrencyTypeId = 2,
                    Name = "Infinitecoin",
                    Abbreviation = "IFC",
                    Slug = "IFC"
                }, new Currency
                {
                    Id = 2755,
                    CurrencyTypeId = 2,
                    Name = "Hero",
                    Abbreviation = "HERO",
                    Slug = "HERO"
                }, new Currency
                {
                    Id = 3943,
                    CurrencyTypeId = 2,
                    Name = "NEOX",
                    Abbreviation = "NEOX",
                    Slug = "NEOX"
                }, new Currency
                {
                    Id = 3169,
                    CurrencyTypeId = 2,
                    Name = "Hybrid Block",
                    Abbreviation = "HYB",
                    Slug = "HYB"
                }, new Currency
                {
                    Id = 3176,
                    CurrencyTypeId = 2,
                    Name = "Alttex",
                    Abbreviation = "ALTX",
                    Slug = "ALTX"
                }, new Currency
                {
                    Id = 3802,
                    CurrencyTypeId = 2,
                    Name = "Cryptoinvest",
                    Abbreviation = "CTT",
                    Slug = "CTT"
                }, new Currency
                {
                    Id = 3272,
                    CurrencyTypeId = 2,
                    Name = "Coin2Play",
                    Abbreviation = "C2P",
                    Slug = "C2P"
                }, new Currency
                {
                    Id = 3671,
                    CurrencyTypeId = 2,
                    Name = "Almeela",
                    Abbreviation = "KZE",
                    Slug = "KZE"
                }, new Currency
                {
                    Id = 1863,
                    CurrencyTypeId = 2,
                    Name = "Minex",
                    Abbreviation = "MINEX",
                    Slug = "MINEX"
                }, new Currency
                {
                    Id = 2201,
                    CurrencyTypeId = 2,
                    Name = "WINCOIN",
                    Abbreviation = "WC",
                    Slug = "WC"
                }, new Currency
                {
                    Id = 3276,
                    CurrencyTypeId = 2,
                    Name = "SaveNode",
                    Abbreviation = "SNO",
                    Slug = "SNO"
                }, new Currency
                {
                    Id = 3382,
                    CurrencyTypeId = 2,
                    Name = "CARDbuyers",
                    Abbreviation = "BCARD",
                    Slug = "BCARD"
                }, new Currency
                {
                    Id = 3425,
                    CurrencyTypeId = 2,
                    Name = "EmaratCoin",
                    Abbreviation = "AEC",
                    Slug = "AEC"
                }, new Currency
                {
                    Id = 3304,
                    CurrencyTypeId = 2,
                    Name = "MobilinkToken",
                    Abbreviation = "MOLK",
                    Slug = "MOLK"
                }, new Currency
                {
                    Id = 2072,
                    CurrencyTypeId = 2,
                    Name = "SegWit2x",
                    Abbreviation = "B2X",
                    Slug = "B2X"
                }, new Currency
                {
                    Id = 3160,
                    CurrencyTypeId = 2,
                    Name = "Infinipay",
                    Abbreviation = "IFP",
                    Slug = "IFP"
                }, new Currency
                {
                    Id = 3381,
                    CurrencyTypeId = 2,
                    Name = "Civitas",
                    Abbreviation = "CIV",
                    Slug = "CIV"
                }, new Currency
                {
                    Id = 2119,
                    CurrencyTypeId = 2,
                    Name = "BTCMoon",
                    Abbreviation = "BTCM",
                    Slug = "BTCM"
                }, new Currency
                {
                    Id = 3073,
                    CurrencyTypeId = 2,
                    Name = "Esports Token",
                    Abbreviation = "EST",
                    Slug = "EST"
                }, new Currency
                {
                    Id = 3212,
                    CurrencyTypeId = 2,
                    Name = "CottonCoin",
                    Abbreviation = "COTN",
                    Slug = "COTN"
                }, new Currency
                {
                    Id = 3059,
                    CurrencyTypeId = 2,
                    Name = "EscrowCoin",
                    Abbreviation = "ESCO",
                    Slug = "ESCO"
                }, new Currency
                {
                    Id = 3400,
                    CurrencyTypeId = 2,
                    Name = "X12 Coin",
                    Abbreviation = "X12",
                    Slug = "X12"
                }, new Currency
                {
                    Id = 572,
                    CurrencyTypeId = 2,
                    Name = "RabbitCoin",
                    Abbreviation = "RBBT",
                    Slug = "RBBT"
                }, new Currency
                {
                    Id = 3483,
                    CurrencyTypeId = 2,
                    Name = "OmenCoin",
                    Abbreviation = "OMEN",
                    Slug = "OMEN"
                }, new Currency
                {
                    Id = 3516,
                    CurrencyTypeId = 2,
                    Name = "DarkPayCoin",
                    Abbreviation = "DKPC",
                    Slug = "DKPC"
                }, new Currency
                {
                    Id = 2350,
                    CurrencyTypeId = 2,
                    Name = "GameChain System",
                    Abbreviation = "GCS",
                    Slug = "GCS"
                }, new Currency
                {
                    Id = 733,
                    CurrencyTypeId = 2,
                    Name = "Quotient",
                    Abbreviation = "XQN",
                    Slug = "XQN"
                }, new Currency
                {
                    Id = 1542,
                    CurrencyTypeId = 2,
                    Name = "Golos Gold",
                    Abbreviation = "GBG",
                    Slug = "GBG"
                }, new Currency
                {
                    Id = 3111,
                    CurrencyTypeId = 2,
                    Name = "PayDay Coin",
                    Abbreviation = "PDX",
                    Slug = "PDX"
                }, new Currency
                {
                    Id = 1351,
                    CurrencyTypeId = 2,
                    Name = "Aces",
                    Abbreviation = "ACES",
                    Slug = "ACES"
                }, new Currency
                {
                    Id = 3504,
                    CurrencyTypeId = 2,
                    Name = "HondaisCoin",
                    Abbreviation = "HNDC",
                    Slug = "HNDC"
                }, new Currency
                {
                    Id = 3269,
                    CurrencyTypeId = 2,
                    Name = "Crypto Improvement Fund",
                    Abbreviation = "CIF",
                    Slug = "CIF"
                }, new Currency
                {
                    Id = 3450,
                    CurrencyTypeId = 2,
                    Name = "ShopZcoin",
                    Abbreviation = "SZC",
                    Slug = "SZC"
                }, new Currency
                {
                    Id = 1843,
                    CurrencyTypeId = 2,
                    Name = "EmberCoin",
                    Abbreviation = "EMB",
                    Slug = "EMB"
                }, new Currency
                {
                    Id = 1020,
                    CurrencyTypeId = 2,
                    Name = "Axiom",
                    Abbreviation = "AXIOM",
                    Slug = "AXIOM"
                }, new Currency
                {
                    Id = 2517,
                    CurrencyTypeId = 2,
                    Name = "Animation Vision Cash",
                    Abbreviation = "AVH",
                    Slug = "AVH"
                }, new Currency
                {
                    Id = 1398,
                    CurrencyTypeId = 2,
                    Name = "PROUD Money",
                    Abbreviation = "PROUD",
                    Slug = "PROUD"
                }, new Currency
                {
                    Id = 3039,
                    CurrencyTypeId = 2,
                    Name = "Excaliburcoin",
                    Abbreviation = "EXC",
                    Slug = "EXC"
                }, new Currency
                {
                    Id = 3420,
                    CurrencyTypeId = 2,
                    Name = "Cobrabytes",
                    Abbreviation = "COBRA",
                    Slug = "COBRA"
                }, new Currency
                {
                    Id = 1971,
                    CurrencyTypeId = 2,
                    Name = "iQuant",
                    Abbreviation = "IQT",
                    Slug = "IQT"
                }, new Currency
                {
                    Id = 1497,
                    CurrencyTypeId = 2,
                    Name = "Fargocoin",
                    Abbreviation = "FRGC",
                    Slug = "FRGC"
                }, new Currency
                {
                    Id = 1146,
                    CurrencyTypeId = 2,
                    Name = "AvatarCoin",
                    Abbreviation = "AV",
                    Slug = "AV"
                }, new Currency
                {
                    Id = 1164,
                    CurrencyTypeId = 2,
                    Name = "Francs",
                    Abbreviation = "FRN",
                    Slug = "FRN"
                }, new Currency
                {
                    Id = 1393,
                    CurrencyTypeId = 2,
                    Name = "Tellurion",
                    Abbreviation = "TELL",
                    Slug = "TELL"
                }, new Currency
                {
                    Id = 1436,
                    CurrencyTypeId = 2,
                    Name = "DynamicCoin",
                    Abbreviation = "DMC",
                    Slug = "DMC"
                }, new Currency
                {
                    Id = 1623,
                    CurrencyTypeId = 2,
                    Name = "BlazerCoin",
                    Abbreviation = "BLAZR",
                    Slug = "BLAZR"
                }, new Currency
                {
                    Id = 1679,
                    CurrencyTypeId = 2,
                    Name = "Halloween Coin",
                    Abbreviation = "HALLO",
                    Slug = "HALLO"
                }, new Currency
                {
                    Id = 1849,
                    CurrencyTypeId = 2,
                    Name = "Birds",
                    Abbreviation = "BIRDS",
                    Slug = "BIRDS"
                }, new Currency
                {
                    Id = 1851,
                    CurrencyTypeId = 2,
                    Name = "ERA",
                    Abbreviation = "ERA",
                    Slug = "ERA"
                }, new Currency
                {
                    Id = 1865,
                    CurrencyTypeId = 2,
                    Name = "Wink",
                    Abbreviation = "WINK",
                    Slug = "WINK"
                }, new Currency
                {
                    Id = 2067,
                    CurrencyTypeId = 2,
                    Name = "Dutch Coin",
                    Abbreviation = "DUTCH",
                    Slug = "DUTCH"
                }, new Currency
                {
                    Id = 2077,
                    CurrencyTypeId = 2,
                    Name = "Runners",
                    Abbreviation = "RUNNERS",
                    Slug = "RUNNERS"
                }, new Currency
                {
                    Id = 2101,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Lite",
                    Abbreviation = "ELITE",
                    Slug = "ELITE"
                }, new Currency
                {
                    Id = 2488,
                    CurrencyTypeId = 2,
                    Name = "ValueChain",
                    Abbreviation = "VLC",
                    Slug = "VLC"
                }, new Currency
                {
                    Id = 2647,
                    CurrencyTypeId = 2,
                    Name = "SnipCoin",
                    Abbreviation = "SNIP",
                    Slug = "SNIP"
                }, new Currency
                {
                    Id = 2671,
                    CurrencyTypeId = 2,
                    Name = "Cropcoin",
                    Abbreviation = "CROP",
                    Slug = "CROP"
                }, new Currency
                {
                    Id = 2834,
                    CurrencyTypeId = 2,
                    Name = "ContractNet",
                    Abbreviation = "CNET",
                    Slug = "CNET"
                }, new Currency
                {
                    Id = 2857,
                    CurrencyTypeId = 2,
                    Name = "SalPay",
                    Abbreviation = "SAL",
                    Slug = "SAL"
                }, new Currency
                {
                    Id = 2943,
                    CurrencyTypeId = 2,
                    Name = "Rocket Pool",
                    Abbreviation = "RPL",
                    Slug = "RPL"
                }, new Currency
                {
                    Id = 2959,
                    CurrencyTypeId = 2,
                    Name = "WeToken",
                    Abbreviation = "WT",
                    Slug = "WT"
                }, new Currency
                {
                    Id = 3100,
                    CurrencyTypeId = 2,
                    Name = "Ultra Salescloud",
                    Abbreviation = "UST",
                    Slug = "UST"
                }, new Currency
                {
                    Id = 3107,
                    CurrencyTypeId = 2,
                    Name = "BingoCoin",
                    Abbreviation = "BOC",
                    Slug = "BOC"
                }, new Currency
                {
                    Id = 3109,
                    CurrencyTypeId = 2,
                    Name = "Ordocoin",
                    Abbreviation = "RDC",
                    Slug = "RDC"
                }, new Currency
                {
                    Id = 3143,
                    CurrencyTypeId = 2,
                    Name = "Pecunio",
                    Abbreviation = "PCO",
                    Slug = "PCO"
                }, new Currency
                {
                    Id = 3152,
                    CurrencyTypeId = 2,
                    Name = "Obitan Chain",
                    Abbreviation = "OBTC",
                    Slug = "OBTC"
                }, new Currency
                {
                    Id = 3157,
                    CurrencyTypeId = 2,
                    Name = "Smart Application Chain",
                    Abbreviation = "SAC",
                    Slug = "SAC"
                }, new Currency
                {
                    Id = 3288,
                    CurrencyTypeId = 2,
                    Name = "Digital Asset Exchange Token",
                    Abbreviation = "DAXT",
                    Slug = "DAXT"
                }, new Currency
                {
                    Id = 3309,
                    CurrencyTypeId = 2,
                    Name = "Concierge Coin",
                    Abbreviation = "CCC",
                    Slug = "CCC"
                }, new Currency
                {
                    Id = 3310,
                    CurrencyTypeId = 2,
                    Name = "ALLCOIN",
                    Abbreviation = "ALC",
                    Slug = "ALC"
                }, new Currency
                {
                    Id = 3358,
                    CurrencyTypeId = 2,
                    Name = "Helper Search Token",
                    Abbreviation = "HSN",
                    Slug = "HSN"
                }, new Currency
                {
                    Id = 3378,
                    CurrencyTypeId = 2,
                    Name = "Labh Coin",
                    Abbreviation = "LABH",
                    Slug = "LABH"
                }, new Currency
                {
                    Id = 3385,
                    CurrencyTypeId = 2,
                    Name = "GIGA",
                    Abbreviation = "XG",
                    Slug = "XG"
                }, new Currency
                {
                    Id = 3405,
                    CurrencyTypeId = 2,
                    Name = "Pandemia",
                    Abbreviation = "PNDM",
                    Slug = "PNDM"
                }, new Currency
                {
                    Id = 3458,
                    CurrencyTypeId = 2,
                    Name = "ZTCoin",
                    Abbreviation = "ZT",
                    Slug = "ZT"
                }, new Currency
                {
                    Id = 3470,
                    CurrencyTypeId = 2,
                    Name = "Dragon Token",
                    Abbreviation = "DT",
                    Slug = "DT"
                }, new Currency
                {
                    Id = 3498,
                    CurrencyTypeId = 2,
                    Name = "CapdaxToken",
                    Abbreviation = "XCD",
                    Slug = "XCD"
                }, new Currency
                {
                    Id = 3500,
                    CurrencyTypeId = 2,
                    Name = "Delizia",
                    Abbreviation = "DELIZ",
                    Slug = "DELIZ"
                }, new Currency
                {
                    Id = 3587,
                    CurrencyTypeId = 2,
                    Name = "Bgogo Token",
                    Abbreviation = "BGG",
                    Slug = "BGG"
                }, new Currency
                {
                    Id = 3692,
                    CurrencyTypeId = 2,
                    Name = "TOKOK",
                    Abbreviation = "TOK",
                    Slug = "TOK"
                }, new Currency
                {
                    Id = 3864,
                    CurrencyTypeId = 2,
                    Name = "UTEMIS",
                    Abbreviation = "UTS",
                    Slug = "UTS"
                }, new Currency
                {
                    Id = 3866,
                    CurrencyTypeId = 2,
                    Name = "CONUN",
                    Abbreviation = "CON",
                    Slug = "CON"
                }, new Currency
                {
                    Id = 3900,
                    CurrencyTypeId = 2,
                    Name = "Hellenic Node",
                    Abbreviation = "HN",
                    Slug = "HN"
                }, new Currency
                {
                    Id = 3961,
                    CurrencyTypeId = 2,
                    Name = "BZEdge",
                    Abbreviation = "BZE",
                    Slug = "BZE"
                });
        }
    }
}