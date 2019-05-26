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

            entityTypeBuilder.HasIndex(c => c.Abbreviation).IsUnique().HasName("Currency_Index_Abbreviation");
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
                    Name = "United States Dollar"
                },
                new Currency
                {
                    Id = 2,
                    CurrencyTypeId = 1,
                    Abbreviation = "EUR",
                    Name = "Euro"
                },
                new Currency
                {
                    Id = 3,
                    CurrencyTypeId = 2,
                    Abbreviation = "ETH",
                    Name = "Ethereum",
                    Denominations = 18,
                    DenominationName = "Wei"
                },
                new Currency
                {
                    Id = 4,
                    CurrencyTypeId = 2,
                    Abbreviation = "KNC",
                    Name = "Kyber Network Crystal",
                    Denominations = 18,
                },
                new Currency
                {
                    Id = 5,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTC",
                    Name = "Bitcoin",
                    Denominations = 8,
                    DenominationName = "Sat"
                },
                new Currency
                {
                    Id = 6,
                    CurrencyTypeId = 2,
                    Abbreviation = "BCN",
                    Name = "Bytecoin"
                },
                new Currency
                {
                    Id = 7,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTS",
                    Name = "BitShares"
                },
                new Currency
                {
                    Id = 8,
                    CurrencyTypeId = 2,
                    Abbreviation = "USDT",
                    Name = "Tether"
                },
                new Currency
                {
                    Id = 9,
                    CurrencyTypeId = 1,
                    Abbreviation = "SGD",
                    Name = "Singapore Dollar"
                },
                new Currency
                {
                    Id = 10,
                    CurrencyTypeId = 2,
                    Abbreviation = "LTC",
                    Name = "Litecoin"
                },
                new Currency
                {
                    Id = 11,
                    CurrencyTypeId = 2,
                    Name = "XRP",
                    Abbreviation = "XRP"
                },
                new Currency
                {
                    Id = 12,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Cash",
                    Abbreviation = "BCH"
                },
                new Currency
                {
                    Id = 13,
                    CurrencyTypeId = 2,
                    Name = "EOS",
                    Abbreviation = "EOS"
                },
                new Currency
                {
                    Id = 14,
                    CurrencyTypeId = 2,
                    Name = "Binance Coin",
                    Abbreviation = "BNB"
                },
                new Currency
                {
                    Id = 15,
                    CurrencyTypeId = 2,
                    Name = "Stellar",
                    Abbreviation = "XLM"
                },
                new Currency
                {
                    Id = 16,
                    CurrencyTypeId = 2,
                    Name = "Cardano",
                    Abbreviation = "ADA"
                },
                new Currency
                {
                    Id = 17,
                    CurrencyTypeId = 2,
                    Name = "TRON",
                    Abbreviation = "TRX"
                },
                new Currency
                {
                    Id = 18,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin SV",
                    Abbreviation = "BSV"
                },
                new Currency
                {
                    Id = 19,
                    CurrencyTypeId = 2,
                    Name = "Monero",
                    Abbreviation = "XMR"
                },
                new Currency
                {
                    Id = 20,
                    CurrencyTypeId = 2,
                    Name = "Dash",
                    Abbreviation = "DASH"
                },
                new Currency
                {
                    Id = 21,
                    CurrencyTypeId = 2,
                    Name = "IOTA",
                    Abbreviation = "MIOTA"
                },
                new Currency
                {
                    Id = 22,
                    CurrencyTypeId = 2,
                    Name = "Tezos",
                    Abbreviation = "XTZ"
                },
                new Currency
                {
                    Id = 23,
                    CurrencyTypeId = 2,
                    Name = "Cosmos",
                    Abbreviation = "ATOM"
                },
                new Currency
                {
                    Id = 24,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Classic",
                    Abbreviation = "ETC"
                },
                new Currency
                {
                    Id = 25,
                    CurrencyTypeId = 2,
                    Name = "NEM",
                    Abbreviation = "XEM"
                },
                new Currency
                {
                    Id = 26,
                    CurrencyTypeId = 2,
                    Name = "NEO",
                    Abbreviation = "NEO"
                },
                new Currency
                {
                    Id = 1518,
                    CurrencyTypeId = 2,
                    Name = "Maker",
                    Abbreviation = "MKR"
                },
                new Currency
                {
                    Id = 2566,
                    CurrencyTypeId = 2,
                    Name = "Ontology",
                    Abbreviation = "ONT"
                },
                new Currency
                {
                    Id = 1437,
                    CurrencyTypeId = 2,
                    Name = "Zcash",
                    Abbreviation = "ZEC"
                },
                new Currency
                {
                    Id = 1697,
                    CurrencyTypeId = 2,
                    Name = "Basic Attention Token",
                    Abbreviation = "BAT"
                },
                new Currency
                {
                    Id = 3635,
                    CurrencyTypeId = 2,
                    Name = "Crypto.com Chain",
                    Abbreviation = "CRO"
                },
                new Currency
                {
                    Id = 2083,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Gold",
                    Abbreviation = "BTG"
                },
                new Currency
                {
                    Id = 3077,
                    CurrencyTypeId = 2,
                    Name = "VeChain",
                    Abbreviation = "VET"
                },
                new Currency
                {
                    Id = 1975,
                    CurrencyTypeId = 2,
                    Name = "Chainlink",
                    Abbreviation = "LINK"
                },
                new Currency
                {
                    Id = 3408,
                    CurrencyTypeId = 2,
                    Name = "USD Coin",
                    Abbreviation = "USDC"
                },
                new Currency
                {
                    Id = 74,
                    CurrencyTypeId = 2,
                    Name = "Dogecoin",
                    Abbreviation = "DOGE"
                },
                new Currency
                {
                    Id = 2874,
                    CurrencyTypeId = 2,
                    Name = "Aurora",
                    Abbreviation = "AOA"
                },
                new Currency
                {
                    Id = 1808,
                    CurrencyTypeId = 2,
                    Name = "OmiseGO",
                    Abbreviation = "OMG"
                },
                new Currency
                {
                    Id = 1684,
                    CurrencyTypeId = 2,
                    Name = "Qtum",
                    Abbreviation = "QTUM"
                },
                new Currency
                {
                    Id = 1168,
                    CurrencyTypeId = 2,
                    Name = "Decred",
                    Abbreviation = "DCR"
                },
                new Currency
                {
                    Id = 1274,
                    CurrencyTypeId = 2,
                    Name = "Waves",
                    Abbreviation = "WAVES"
                },
                new Currency
                {
                    Id = 3718,
                    CurrencyTypeId = 2,
                    Name = "BitTorrent",
                    Abbreviation = "BTT"
                },
                new Currency
                {
                    Id = 2682,
                    CurrencyTypeId = 2,
                    Name = "Holo",
                    Abbreviation = "HOT"
                },
                new Currency
                {
                    Id = 2563,
                    CurrencyTypeId = 2,
                    Name = "TrueUSD",
                    Abbreviation = "TUSD"
                },
                new Currency
                {
                    Id = 1214,
                    CurrencyTypeId = 2,
                    Name = "Lisk",
                    Abbreviation = "LSK"
                },
                new Currency
                {
                    Id = 1567,
                    CurrencyTypeId = 2,
                    Name = "Nano",
                    Abbreviation = "NANO"
                },
                new Currency
                {
                    Id = 1104,
                    CurrencyTypeId = 2,
                    Name = "Augur",
                    Abbreviation = "REP"
                },
                new Currency
                {
                    Id = 2222,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Diamond",
                    Abbreviation = "BCD"
                },
                new Currency
                {
                    Id = 1896,
                    CurrencyTypeId = 2,
                    Name = "0x",
                    Abbreviation = "ZRX"
                },
                new Currency
                {
                    Id = 2577,
                    CurrencyTypeId = 2,
                    Name = "Ravencoin",
                    Abbreviation = "RVN"
                },
                new Currency
                {
                    Id = 109,
                    CurrencyTypeId = 2,
                    Name = "DigiByte",
                    Abbreviation = "DGB"
                },
                new Currency
                {
                    Id = 2099,
                    CurrencyTypeId = 2,
                    Name = "ICON",
                    Abbreviation = "ICX"
                },
                new Currency
                {
                    Id = 693,
                    CurrencyTypeId = 2,
                    Name = "Verge",
                    Abbreviation = "XVG"
                },
                new Currency
                {
                    Id = 3330,
                    CurrencyTypeId = 2,
                    Name = "Paxos Standard Token",
                    Abbreviation = "PAX"
                },
                new Currency
                {
                    Id = 2603,
                    CurrencyTypeId = 2,
                    Name = "Pundi X",
                    Abbreviation = "NPXS"
                },
                new Currency
                {
                    Id = 2469,
                    CurrencyTypeId = 2,
                    Name = "Zilliqa",
                    Abbreviation = "ZIL"
                },
                new Currency
                {
                    Id = 2502,
                    CurrencyTypeId = 2,
                    Name = "Huobi Token",
                    Abbreviation = "HT"
                },
                new Currency
                {
                    Id = 1700,
                    CurrencyTypeId = 2,
                    Name = "Aeternity",
                    Abbreviation = "AE"
                },
                new Currency
                {
                    Id = 2405,
                    CurrencyTypeId = 2,
                    Name = "IOST",
                    Abbreviation = "IOST"
                },
                new Currency
                {
                    Id = 1042,
                    CurrencyTypeId = 2,
                    Name = "Siacoin",
                    Abbreviation = "SC"
                },
                new Currency
                {
                    Id = 3437,
                    CurrencyTypeId = 2,
                    Name = "ABBC Coin",
                    Abbreviation = "ABBC"
                },
                new Currency
                {
                    Id = 1521,
                    CurrencyTypeId = 2,
                    Name = "Komodo",
                    Abbreviation = "KMD"
                },
                new Currency
                {
                    Id = 2130,
                    CurrencyTypeId = 2,
                    Name = "Enjin Coin",
                    Abbreviation = "ENJ"
                },
                new Currency
                {
                    Id = 1230,
                    CurrencyTypeId = 2,
                    Name = "Steem",
                    Abbreviation = "STEEM"
                },
                new Currency
                {
                    Id = 1866,
                    CurrencyTypeId = 2,
                    Name = "Bytom",
                    Abbreviation = "BTM"
                },
                new Currency
                {
                    Id = 3224,
                    CurrencyTypeId = 2,
                    Name = "Qubitica",
                    Abbreviation = "QBIT"
                },
                new Currency
                {
                    Id = 2416,
                    CurrencyTypeId = 2,
                    Name = "THETA",
                    Abbreviation = "THETA"
                },
                new Currency
                {
                    Id = 1343,
                    CurrencyTypeId = 2,
                    Name = "Stratis",
                    Abbreviation = "STRAT"
                },
                new Currency
                {
                    Id = 3144,
                    CurrencyTypeId = 2,
                    Name = "ThoreCoin",
                    Abbreviation = "THR"
                },
                new Currency
                {
                    Id = 291,
                    CurrencyTypeId = 2,
                    Name = "MaidSafeCoin",
                    Abbreviation = "MAID"
                },
                new Currency
                {
                    Id = 1886,
                    CurrencyTypeId = 2,
                    Name = "Dent",
                    Abbreviation = "DENT"
                },
                new Currency
                {
                    Id = 3116,
                    CurrencyTypeId = 2,
                    Name = "Insight Chain",
                    Abbreviation = "INB"
                },
                new Currency
                {
                    Id = 2087,
                    CurrencyTypeId = 2,
                    Name = "KuCoin Shares",
                    Abbreviation = "KCS"
                },
                new Currency
                {
                    Id = 3724,
                    CurrencyTypeId = 2,
                    Name = "SOLVE",
                    Abbreviation = "SOLVE"
                },
                new Currency
                {
                    Id = 1925,
                    CurrencyTypeId = 2,
                    Name = "Waltonchain",
                    Abbreviation = "WTC"
                },
                new Currency
                {
                    Id = 1455,
                    CurrencyTypeId = 2,
                    Name = "Golem",
                    Abbreviation = "GNT"
                },
                new Currency
                {
                    Id = 2299,
                    CurrencyTypeId = 2,
                    Name = "aelf",
                    Abbreviation = "ELF"
                },
                new Currency
                {
                    Id = 1759,
                    CurrencyTypeId = 2,
                    Name = "Status",
                    Abbreviation = "SNT"
                },
                new Currency
                {
                    Id = 1776,
                    CurrencyTypeId = 2,
                    Name = "Crypto.com",
                    Abbreviation = "MCO"
                },
                new Currency
                {
                    Id = 2349,
                    CurrencyTypeId = 2,
                    Name = "Mixin",
                    Abbreviation = "XIN"
                },
                new Currency
                {
                    Id = 2027,
                    CurrencyTypeId = 2,
                    Name = "Cryptonex",
                    Abbreviation = "CNX"
                },
                new Currency
                {
                    Id = 3822,
                    CurrencyTypeId = 2,
                    Name = "Theta Fuel",
                    Abbreviation = "TFUEL"
                },
                new Currency
                {
                    Id = 1320,
                    CurrencyTypeId = 2,
                    Name = "Ardor",
                    Abbreviation = "ARDR"
                },
                new Currency
                {
                    Id = 3607,
                    CurrencyTypeId = 2,
                    Name = "VestChain",
                    Abbreviation = "VEST"
                },
                new Currency
                {
                    Id = 2308,
                    CurrencyTypeId = 2,
                    Name = "Dai",
                    Abbreviation = "DAI"
                },
                new Currency
                {
                    Id = 1087,
                    CurrencyTypeId = 2,
                    Name = "Factom",
                    Abbreviation = "FCT"
                },
                new Currency
                {
                    Id = 2900,
                    CurrencyTypeId = 2,
                    Name = "Project Pai",
                    Abbreviation = "PAI"
                },
                new Currency
                {
                    Id = 2300,
                    CurrencyTypeId = 2,
                    Name = "WAX",
                    Abbreviation = "WAX"
                },
                new Currency
                {
                    Id = 2457,
                    CurrencyTypeId = 2,
                    Name = "TrueChain",
                    Abbreviation = "TRUE"
                },
                new Currency
                {
                    Id = 1586,
                    CurrencyTypeId = 2,
                    Name = "Ark",
                    Abbreviation = "ARK"
                },
                new Currency
                {
                    Id = 1698,
                    CurrencyTypeId = 2,
                    Name = "Horizen",
                    Abbreviation = "ZEN"
                },
                new Currency
                {
                    Id = 1229,
                    CurrencyTypeId = 2,
                    Name = "DigixDAO",
                    Abbreviation = "DGD"
                },
                new Currency
                {
                    Id = 460,
                    CurrencyTypeId = 2,
                    Name = "Clams",
                    Abbreviation = "CLAM"
                },
                new Currency
                {
                    Id = 213,
                    CurrencyTypeId = 2,
                    Name = "MonaCoin",
                    Abbreviation = "MONA"
                },
                new Currency
                {
                    Id = 1750,
                    CurrencyTypeId = 2,
                    Name = "GXChain",
                    Abbreviation = "GXC"
                },
                new Currency
                {
                    Id = 1966,
                    CurrencyTypeId = 2,
                    Name = "Decentraland",
                    Abbreviation = "MANA"
                },
                new Currency
                {
                    Id = 3835,
                    CurrencyTypeId = 2,
                    Name = "Orbs",
                    Abbreviation = "ORBS"
                },
                new Currency
                {
                    Id = 2062,
                    CurrencyTypeId = 2,
                    Name = "Aion",
                    Abbreviation = "AION"
                },
                new Currency
                {
                    Id = 1703,
                    CurrencyTypeId = 2,
                    Name = "Metaverse ETP",
                    Abbreviation = "ETP"
                },
                new Currency
                {
                    Id = 2492,
                    CurrencyTypeId = 2,
                    Name = "Elastos",
                    Abbreviation = "ELA"
                },
                new Currency
                {
                    Id = 1934,
                    CurrencyTypeId = 2,
                    Name = "Loopring",
                    Abbreviation = "LRC"
                },
                new Currency
                {
                    Id = 2588,
                    CurrencyTypeId = 2,
                    Name = "Loom Network",
                    Abbreviation = "LOOM"
                },
                new Currency
                {
                    Id = 1807,
                    CurrencyTypeId = 2,
                    Name = "Santiment Network Token",
                    Abbreviation = "SAN"
                },
                new Currency
                {
                    Id = 2135,
                    CurrencyTypeId = 2,
                    Name = "Revain",
                    Abbreviation = "R"
                },
                new Currency
                {
                    Id = 2092,
                    CurrencyTypeId = 2,
                    Name = "NULS",
                    Abbreviation = "NULS"
                },
                new Currency
                {
                    Id = 1789,
                    CurrencyTypeId = 2,
                    Name = "Populous",
                    Abbreviation = "PPT"
                },
                new Currency
                {
                    Id = 3890,
                    CurrencyTypeId = 2,
                    Name = "Matic Network",
                    Abbreviation = "MATIC"
                },
                new Currency
                {
                    Id = 2090,
                    CurrencyTypeId = 2,
                    Name = "LATOKEN",
                    Abbreviation = "LA"
                },
                new Currency
                {
                    Id = 1414,
                    CurrencyTypeId = 2,
                    Name = "Zcoin",
                    Abbreviation = "XZC"
                },
                new Currency
                {
                    Id = 1903,
                    CurrencyTypeId = 2,
                    Name = "HyperCash",
                    Abbreviation = "HC"
                },
                new Currency
                {
                    Id = 2132,
                    CurrencyTypeId = 2,
                    Name = "Power Ledger",
                    Abbreviation = "POWR"
                },
                new Currency
                {
                    Id = 118,
                    CurrencyTypeId = 2,
                    Name = "ReddCoin",
                    Abbreviation = "RDD"
                },
                new Currency
                {
                    Id = 2403,
                    CurrencyTypeId = 2,
                    Name = "MOAC",
                    Abbreviation = "MOAC"
                },
                new Currency
                {
                    Id = 2897,
                    CurrencyTypeId = 2,
                    Name = "Clipper Coin",
                    Abbreviation = "CCCX"
                },
                new Currency
                {
                    Id = 2545,
                    CurrencyTypeId = 2,
                    Name = "Arcblock",
                    Abbreviation = "ABT"
                },
                new Currency
                {
                    Id = 3871,
                    CurrencyTypeId = 2,
                    Name = "Newton",
                    Abbreviation = "NEW"
                },
                new Currency
                {
                    Id = 2213,
                    CurrencyTypeId = 2,
                    Name = "QASH",
                    Abbreviation = "QASH"
                },
                new Currency
                {
                    Id = 2631,
                    CurrencyTypeId = 2,
                    Name = "ODEM",
                    Abbreviation = "ODE"
                },
                new Currency
                {
                    Id = 2346,
                    CurrencyTypeId = 2,
                    Name = "WaykiChain",
                    Abbreviation = "WICC"
                },
                new Currency
                {
                    Id = 2606,
                    CurrencyTypeId = 2,
                    Name = "Wanchain",
                    Abbreviation = "WAN"
                },
                new Currency
                {
                    Id = 1727,
                    CurrencyTypeId = 2,
                    Name = "Bancor",
                    Abbreviation = "BNT"
                },
                new Currency
                {
                    Id = 3115,
                    CurrencyTypeId = 2,
                    Name = "Maximine Coin",
                    Abbreviation = "MXM"
                },
                new Currency
                {
                    Id = 2694,
                    CurrencyTypeId = 2,
                    Name = "Nexo",
                    Abbreviation = "NEXO"
                },
                new Currency
                {
                    Id = 2496,
                    CurrencyTypeId = 2,
                    Name = "Polymath",
                    Abbreviation = "POLY"
                },
                new Currency
                {
                    Id = 1772,
                    CurrencyTypeId = 2,
                    Name = "Storj",
                    Abbreviation = "STORJ"
                },
                new Currency
                {
                    Id = 2306,
                    CurrencyTypeId = 2,
                    Name = "Bread",
                    Abbreviation = "BRD"
                },
                new Currency
                {
                    Id = 2137,
                    CurrencyTypeId = 2,
                    Name = "Electroneum",
                    Abbreviation = "ETN"
                },
                new Currency
                {
                    Id = 1169,
                    CurrencyTypeId = 2,
                    Name = "PIVX",
                    Abbreviation = "PIVX"
                },
                new Currency
                {
                    Id = 45,
                    CurrencyTypeId = 2,
                    Name = "CasinoCoin",
                    Abbreviation = "CSC"
                },
                new Currency
                {
                    Id = 2777,
                    CurrencyTypeId = 2,
                    Name = "IoTeX",
                    Abbreviation = "IOTX"
                },
                new Currency
                {
                    Id = 1908,
                    CurrencyTypeId = 2,
                    Name = "Nebulas",
                    Abbreviation = "NAS"
                },
                new Currency
                {
                    Id = 1637,
                    CurrencyTypeId = 2,
                    Name = "iExec RLC",
                    Abbreviation = "RLC"
                },
                new Currency
                {
                    Id = 1710,
                    CurrencyTypeId = 2,
                    Name = "Veritaseum",
                    Abbreviation = "VERI"
                },
                new Currency
                {
                    Id = 1757,
                    CurrencyTypeId = 2,
                    Name = "FunFair",
                    Abbreviation = "FUN"
                },
                new Currency
                {
                    Id = 2829,
                    CurrencyTypeId = 2,
                    Name = "REPO",
                    Abbreviation = "REPO"
                },
                new Currency
                {
                    Id = 3814,
                    CurrencyTypeId = 2,
                    Name = "Celer Network",
                    Abbreviation = "CELR"
                },
                new Currency
                {
                    Id = 2307,
                    CurrencyTypeId = 2,
                    Name = "Bibox Token",
                    Abbreviation = "BIX"
                },
                new Currency
                {
                    Id = 541,
                    CurrencyTypeId = 2,
                    Name = "Syscoin",
                    Abbreviation = "SYS"
                },
                new Currency
                {
                    Id = 3415,
                    CurrencyTypeId = 2,
                    Name = "Buggyra Coin Zero",
                    Abbreviation = "BCZERO"
                },
                new Currency
                {
                    Id = 2044,
                    CurrencyTypeId = 2,
                    Name = "Enigma",
                    Abbreviation = "ENG"
                },
                new Currency
                {
                    Id = 2989,
                    CurrencyTypeId = 2,
                    Name = "STASIS EURS",
                    Abbreviation = "EURS"
                },
                new Currency
                {
                    Id = 3828,
                    CurrencyTypeId = 2,
                    Name = "Japan Content Token",
                    Abbreviation = "JCT"
                },
                new Currency
                {
                    Id = 3218,
                    CurrencyTypeId = 2,
                    Name = "Energi",
                    Abbreviation = "NRG"
                },
                new Currency
                {
                    Id = 258,
                    CurrencyTypeId = 2,
                    Name = "Groestlcoin",
                    Abbreviation = "GRS"
                },
                new Currency
                {
                    Id = 1826,
                    CurrencyTypeId = 2,
                    Name = "Particl",
                    Abbreviation = "PART"
                },
                new Currency
                {
                    Id = 3788,
                    CurrencyTypeId = 2,
                    Name = "NEXT",
                    Abbreviation = "NET"
                },
                new Currency
                {
                    Id = 2570,
                    CurrencyTypeId = 2,
                    Name = "TomoChain",
                    Abbreviation = "TOMO"
                },
                new Currency
                {
                    Id = 66,
                    CurrencyTypeId = 2,
                    Name = "Nxt",
                    Abbreviation = "NXT"
                },
                new Currency
                {
                    Id = 2952,
                    CurrencyTypeId = 2,
                    Name = "Gold Bits Coin",
                    Abbreviation = "GBC"
                },
                new Currency
                {
                    Id = 3657,
                    CurrencyTypeId = 2,
                    Name = "Lambda",
                    Abbreviation = "LAMB"
                },
                new Currency
                {
                    Id = 2246,
                    CurrencyTypeId = 2,
                    Name = "CyberMiles",
                    Abbreviation = "CMT"
                },
                new Currency
                {
                    Id = 2453,
                    CurrencyTypeId = 2,
                    Name = "EDUCare",
                    Abbreviation = "EKT"
                },
                new Currency
                {
                    Id = 2320,
                    CurrencyTypeId = 2,
                    Name = "UTRUST",
                    Abbreviation = "UTK"
                },
                new Currency
                {
                    Id = 2840,
                    CurrencyTypeId = 2,
                    Name = "QuarkChain",
                    Abbreviation = "QKC"
                },
                new Currency
                {
                    Id = 2112,
                    CurrencyTypeId = 2,
                    Name = "Red Pulse Phoenix",
                    Abbreviation = "PHX"
                },
                new Currency
                {
                    Id = 3701,
                    CurrencyTypeId = 2,
                    Name = "RIF Token",
                    Abbreviation = "RIF"
                },
                new Currency
                {
                    Id = 1785,
                    CurrencyTypeId = 2,
                    Name = "Gas",
                    Abbreviation = "GAS"
                },
                new Currency
                {
                    Id = 1659,
                    CurrencyTypeId = 2,
                    Name = "Gnosis",
                    Abbreviation = "GNO"
                },
                new Currency
                {
                    Id = 1816,
                    CurrencyTypeId = 2,
                    Name = "Civic",
                    Abbreviation = "CVC"
                },
                new Currency
                {
                    Id = 2381,
                    CurrencyTypeId = 2,
                    Name = "Spectre.ai Dividend Token",
                    Abbreviation = "SXDT"
                },
                new Currency
                {
                    Id = 1993,
                    CurrencyTypeId = 2,
                    Name = "Kin",
                    Abbreviation = "KIN"
                },
                new Currency
                {
                    Id = 3662,
                    CurrencyTypeId = 2,
                    Name = "HedgeTrade",
                    Abbreviation = "HEDG"
                },
                new Currency
                {
                    Id = 3513,
                    CurrencyTypeId = 2,
                    Name = "Fantom",
                    Abbreviation = "FTM"
                },
                new Currency
                {
                    Id = 2638,
                    CurrencyTypeId = 2,
                    Name = "Cortex",
                    Abbreviation = "CTXC"
                },
                new Currency
                {
                    Id = 3126,
                    CurrencyTypeId = 2,
                    Name = "ProximaX",
                    Abbreviation = "XPX"
                },
                new Currency
                {
                    Id = 3344,
                    CurrencyTypeId = 2,
                    Name = "Ecoreal Estate",
                    Abbreviation = "ECOREAL"
                },
                new Currency
                {
                    Id = 2608,
                    CurrencyTypeId = 2,
                    Name = "Mithril",
                    Abbreviation = "MITH"
                },
                new Currency
                {
                    Id = 2444,
                    CurrencyTypeId = 2,
                    Name = "CRYPTO20",
                    Abbreviation = "C20"
                },
                new Currency
                {
                    Id = 1758,
                    CurrencyTypeId = 2,
                    Name = "TenX",
                    Abbreviation = "PAY"
                },
                new Currency
                {
                    Id = 3178,
                    CurrencyTypeId = 2,
                    Name = "Linkey",
                    Abbreviation = "LKY"
                },
                new Currency
                {
                    Id = 2896,
                    CurrencyTypeId = 2,
                    Name = "Mainframe",
                    Abbreviation = "MFT"
                },
                new Currency
                {
                    Id = 2845,
                    CurrencyTypeId = 2,
                    Name = "MediBloc [ERC20]",
                    Abbreviation = "MEDX"
                },
                new Currency
                {
                    Id = 2043,
                    CurrencyTypeId = 2,
                    Name = "Cindicator",
                    Abbreviation = "CND"
                },
                new Currency
                {
                    Id = 1492,
                    CurrencyTypeId = 2,
                    Name = "Obyte",
                    Abbreviation = "GBYTE"
                },
                new Currency
                {
                    Id = 3863,
                    CurrencyTypeId = 2,
                    Name = "UGAS",
                    Abbreviation = "UGAS"
                },
                new Currency
                {
                    Id = 3756,
                    CurrencyTypeId = 2,
                    Name = "#MetaHash",
                    Abbreviation = "MHC"
                },
                new Currency
                {
                    Id = 2530,
                    CurrencyTypeId = 2,
                    Name = "Fusion",
                    Abbreviation = "FSN"
                },
                new Currency
                {
                    Id = 2424,
                    CurrencyTypeId = 2,
                    Name = "SingularityNET",
                    Abbreviation = "AGI"
                },
                new Currency
                {
                    Id = 3709,
                    CurrencyTypeId = 2,
                    Name = "Grin",
                    Abbreviation = "GRIN"
                },
                new Currency
                {
                    Id = 2599,
                    CurrencyTypeId = 2,
                    Name = "Noah Coin",
                    Abbreviation = "NOAH"
                },
                new Currency
                {
                    Id = 2057,
                    CurrencyTypeId = 2,
                    Name = "Eidoo",
                    Abbreviation = "EDO"
                },
                new Currency
                {
                    Id = 2885,
                    CurrencyTypeId = 2,
                    Name = "Egretia",
                    Abbreviation = "EGT"
                },
                new Currency
                {
                    Id = 2772,
                    CurrencyTypeId = 2,
                    Name = "Digitex Futures",
                    Abbreviation = "DGTX"
                },
                new Currency
                {
                    Id = 3617,
                    CurrencyTypeId = 2,
                    Name = "ILCoin",
                    Abbreviation = "ILC"
                },
                new Currency
                {
                    Id = 3695,
                    CurrencyTypeId = 2,
                    Name = "Hyperion",
                    Abbreviation = "HYN"
                },
                new Currency
                {
                    Id = 3418,
                    CurrencyTypeId = 2,
                    Name = "Metadium",
                    Abbreviation = "META"
                },
                new Currency
                {
                    Id = 2298,
                    CurrencyTypeId = 2,
                    Name = "Dynamic Trading Rights",
                    Abbreviation = "DTR"
                },
                new Currency
                {
                    Id = 99,
                    CurrencyTypeId = 2,
                    Name = "Vertcoin",
                    Abbreviation = "VTC"
                },
                new Currency
                {
                    Id = 1619,
                    CurrencyTypeId = 2,
                    Name = "Skycoin",
                    Abbreviation = "SKY"
                },
                new Currency
                {
                    Id = 3325,
                    CurrencyTypeId = 2,
                    Name = "Robotina",
                    Abbreviation = "ROX"
                },
                new Currency
                {
                    Id = 3847,
                    CurrencyTypeId = 2,
                    Name = "Contents Protocol",
                    Abbreviation = "CPT"
                },
                new Currency
                {
                    Id = 201,
                    CurrencyTypeId = 2,
                    Name = "Einsteinium",
                    Abbreviation = "EMC2"
                },
                new Currency
                {
                    Id = 2539,
                    CurrencyTypeId = 2,
                    Name = "Ren",
                    Abbreviation = "REN"
                },
                new Currency
                {
                    Id = 2992,
                    CurrencyTypeId = 2,
                    Name = "Apollo Currency",
                    Abbreviation = "APL"
                },
                new Currency
                {
                    Id = 2955,
                    CurrencyTypeId = 2,
                    Name = "Cosmo Coin",
                    Abbreviation = "COSM"
                },
                new Currency
                {
                    Id = 3155,
                    CurrencyTypeId = 2,
                    Name = "Quant",
                    Abbreviation = "QNT"
                },
                new Currency
                {
                    Id = 2605,
                    CurrencyTypeId = 2,
                    Name = "BnkToTheFuture",
                    Abbreviation = "BFT"
                },
                new Currency
                {
                    Id = 2251,
                    CurrencyTypeId = 2,
                    Name = "IoT Chain",
                    Abbreviation = "ITC"
                },
                new Currency
                {
                    Id = 3085,
                    CurrencyTypeId = 2,
                    Name = "INO COIN",
                    Abbreviation = "INO"
                },
                new Currency
                {
                    Id = 3306,
                    CurrencyTypeId = 2,
                    Name = "Gemini Dollar",
                    Abbreviation = "GUSD"
                },
                new Currency
                {
                    Id = 2780,
                    CurrencyTypeId = 2,
                    Name = "NKN",
                    Abbreviation = "NKN"
                },
                new Currency
                {
                    Id = 2394,
                    CurrencyTypeId = 2,
                    Name = "Telcoin",
                    Abbreviation = "TEL"
                },
                new Currency
                {
                    Id = 789,
                    CurrencyTypeId = 2,
                    Name = "Nexus",
                    Abbreviation = "NXS"
                },
                new Currency
                {
                    Id = 1788,
                    CurrencyTypeId = 2,
                    Name = "Metal",
                    Abbreviation = "MTL"
                },
                new Currency
                {
                    Id = 2289,
                    CurrencyTypeId = 2,
                    Name = "Gifto",
                    Abbreviation = "GTO"
                },
                new Currency
                {
                    Id = 1680,
                    CurrencyTypeId = 2,
                    Name = "Aragon",
                    Abbreviation = "ANT"
                },
                new Currency
                {
                    Id = 2335,
                    CurrencyTypeId = 2,
                    Name = "Lightning Bitcoin",
                    Abbreviation = "LBTC"
                },
                new Currency
                {
                    Id = 3637,
                    CurrencyTypeId = 2,
                    Name = "Aergo",
                    Abbreviation = "AERGO"
                },
                new Currency
                {
                    Id = 2760,
                    CurrencyTypeId = 2,
                    Name = "Cred",
                    Abbreviation = "LBA"
                },
                new Currency
                {
                    Id = 1712,
                    CurrencyTypeId = 2,
                    Name = "Quantum Resistant Ledger",
                    Abbreviation = "QRL"
                },
                new Currency
                {
                    Id = 2627,
                    CurrencyTypeId = 2,
                    Name = "TokenPay",
                    Abbreviation = "TPAY"
                },
                new Currency
                {
                    Id = 2297,
                    CurrencyTypeId = 2,
                    Name = "Storm",
                    Abbreviation = "STORM"
                },
                new Currency
                {
                    Id = 1937,
                    CurrencyTypeId = 2,
                    Name = "Po.et",
                    Abbreviation = "POE"
                },
                new Currency
                {
                    Id = 2267,
                    CurrencyTypeId = 2,
                    Name = "Tael",
                    Abbreviation = "WABI"
                },
                new Currency
                {
                    Id = 3609,
                    CurrencyTypeId = 2,
                    Name = "CWV Chain",
                    Abbreviation = "CWV"
                },
                new Currency
                {
                    Id = 215,
                    CurrencyTypeId = 2,
                    Name = "Rubycoin",
                    Abbreviation = "RBY"
                },
                new Currency
                {
                    Id = 3783,
                    CurrencyTypeId = 2,
                    Name = "Ankr Network",
                    Abbreviation = "ANKR"
                },
                new Currency
                {
                    Id = 1954,
                    CurrencyTypeId = 2,
                    Name = "Moeda Loyalty Points",
                    Abbreviation = "MDA"
                },
                new Currency
                {
                    Id = 3154,
                    CurrencyTypeId = 2,
                    Name = "Davinci Coin",
                    Abbreviation = "DAC"
                },
                new Currency
                {
                    Id = 2243,
                    CurrencyTypeId = 2,
                    Name = "Dragonchain",
                    Abbreviation = "DRGN"
                },
                new Currency
                {
                    Id = 2071,
                    CurrencyTypeId = 2,
                    Name = "Request",
                    Abbreviation = "REQ"
                },
                new Currency
                {
                    Id = 2939,
                    CurrencyTypeId = 2,
                    Name = "SCRL",
                    Abbreviation = "SCRL"
                },
                new Currency
                {
                    Id = 1955,
                    CurrencyTypeId = 2,
                    Name = "Neblio",
                    Abbreviation = "NEBL"
                },
                new Currency
                {
                    Id = 3826,
                    CurrencyTypeId = 2,
                    Name = "TOP",
                    Abbreviation = "TOP"
                },
                new Currency
                {
                    Id = 707,
                    CurrencyTypeId = 2,
                    Name = "Blocknet",
                    Abbreviation = "BLOCK"
                },
                new Currency
                {
                    Id = 2458,
                    CurrencyTypeId = 2,
                    Name = "Odyssey",
                    Abbreviation = "OCN"
                },
                new Currency
                {
                    Id = 2433,
                    CurrencyTypeId = 2,
                    Name = "IPChain",
                    Abbreviation = "IPC"
                },
                new Currency
                {
                    Id = 2930,
                    CurrencyTypeId = 2,
                    Name = "Everipedia",
                    Abbreviation = "IQ"
                },
                new Currency
                {
                    Id = 2471,
                    CurrencyTypeId = 2,
                    Name = "Smartlands",
                    Abbreviation = "SLT"
                },
                new Currency
                {
                    Id = 2276,
                    CurrencyTypeId = 2,
                    Name = "Ignis",
                    Abbreviation = "IGNIS"
                },
                new Currency
                {
                    Id = 2934,
                    CurrencyTypeId = 2,
                    Name = "BitKan",
                    Abbreviation = "KAN"
                },
                new Currency
                {
                    Id = 1779,
                    CurrencyTypeId = 2,
                    Name = "Wagerr",
                    Abbreviation = "WGR"
                },
                new Currency
                {
                    Id = 3175,
                    CurrencyTypeId = 2,
                    Name = "TTC Protocol",
                    Abbreviation = "TTC"
                },
                new Currency
                {
                    Id = 2034,
                    CurrencyTypeId = 2,
                    Name = "Everex",
                    Abbreviation = "EVX"
                },
                new Currency
                {
                    Id = 3345,
                    CurrencyTypeId = 2,
                    Name = "DAPS Token",
                    Abbreviation = "DAPS"
                },
                new Currency
                {
                    Id = 2591,
                    CurrencyTypeId = 2,
                    Name = "Dropil",
                    Abbreviation = "DROP"
                },
                new Currency
                {
                    Id = 1918,
                    CurrencyTypeId = 2,
                    Name = "Achain",
                    Abbreviation = "ACT"
                },
                new Currency
                {
                    Id = 2843,
                    CurrencyTypeId = 2,
                    Name = "Ether Zero",
                    Abbreviation = "ETZ"
                },
                new Currency
                {
                    Id = 3600,
                    CurrencyTypeId = 2,
                    Name = "Humanscape",
                    Abbreviation = "HUM"
                },
                new Currency
                {
                    Id = 2861,
                    CurrencyTypeId = 2,
                    Name = "GoChain",
                    Abbreviation = "GO"
                },
                new Currency
                {
                    Id = 2345,
                    CurrencyTypeId = 2,
                    Name = "High Performance Blockchain",
                    Abbreviation = "HPB"
                },
                new Currency
                {
                    Id = 3737,
                    CurrencyTypeId = 2,
                    Name = "BTU Protocol",
                    Abbreviation = "BTU"
                },
                new Currency
                {
                    Id = 3083,
                    CurrencyTypeId = 2,
                    Name = "LINA",
                    Abbreviation = "LINA"
                },
                new Currency
                {
                    Id = 3366,
                    CurrencyTypeId = 2,
                    Name = "SafeInsure",
                    Abbreviation = "SINS"
                },
                new Currency
                {
                    Id = 2296,
                    CurrencyTypeId = 2,
                    Name = "OST",
                    Abbreviation = "OST"
                },
                new Currency
                {
                    Id = 268,
                    CurrencyTypeId = 2,
                    Name = "WhiteCoin",
                    Abbreviation = "XWC"
                },
                new Currency
                {
                    Id = 2474,
                    CurrencyTypeId = 2,
                    Name = "Matrix AI Network",
                    Abbreviation = "MAN"
                },
                new Currency
                {
                    Id = 2835,
                    CurrencyTypeId = 2,
                    Name = "Endor Protocol",
                    Abbreviation = "EDR"
                },
                new Currency
                {
                    Id = 3364,
                    CurrencyTypeId = 2,
                    Name = "PLATINCOIN",
                    Abbreviation = "PLC"
                },
                new Currency
                {
                    Id = 1876,
                    CurrencyTypeId = 2,
                    Name = "Dentacoin",
                    Abbreviation = "DCN"
                },
                new Currency
                {
                    Id = 1654,
                    CurrencyTypeId = 2,
                    Name = "Bitcore",
                    Abbreviation = "BTX"
                },
                new Currency
                {
                    Id = 2161,
                    CurrencyTypeId = 2,
                    Name = "Raiden Network Token",
                    Abbreviation = "RDN"
                },
                new Currency
                {
                    Id = 3020,
                    CurrencyTypeId = 2,
                    Name = "BHPCoin",
                    Abbreviation = "BHP"
                },
                new Currency
                {
                    Id = 3772,
                    CurrencyTypeId = 2,
                    Name = "STEM CELL COIN",
                    Abbreviation = "SCC"
                },
                new Currency
                {
                    Id = 2915,
                    CurrencyTypeId = 2,
                    Name = "Moss Coin",
                    Abbreviation = "MOC"
                },
                new Currency
                {
                    Id = 2143,
                    CurrencyTypeId = 2,
                    Name = "Streamr DATAcoin",
                    Abbreviation = "DATA"
                },
                new Currency
                {
                    Id = 2937,
                    CurrencyTypeId = 2,
                    Name = "VITE",
                    Abbreviation = "VITE"
                },
                new Currency
                {
                    Id = 2476,
                    CurrencyTypeId = 2,
                    Name = "Ruff",
                    Abbreviation = "RUFF"
                },
                new Currency
                {
                    Id = 2096,
                    CurrencyTypeId = 2,
                    Name = "Ripio Credit Network",
                    Abbreviation = "RCN"
                },
                new Currency
                {
                    Id = 2212,
                    CurrencyTypeId = 2,
                    Name = "Quantstamp",
                    Abbreviation = "QSP"
                },
                new Currency
                {
                    Id = 2364,
                    CurrencyTypeId = 2,
                    Name = "TokenClub",
                    Abbreviation = "TCT"
                },
                new Currency
                {
                    Id = 3773,
                    CurrencyTypeId = 2,
                    Name = "Fetch",
                    Abbreviation = "FET"
                },
                new Currency
                {
                    Id = 1159,
                    CurrencyTypeId = 2,
                    Name = "SaluS",
                    Abbreviation = "SLS"
                },
                new Currency
                {
                    Id = 3147,
                    CurrencyTypeId = 2,
                    Name = "HYCON",
                    Abbreviation = "HYC"
                },
                new Currency
                {
                    Id = 3731,
                    CurrencyTypeId = 2,
                    Name = "PlayChip",
                    Abbreviation = "PLA"
                },
                new Currency
                {
                    Id = 2277,
                    CurrencyTypeId = 2,
                    Name = "SmartMesh",
                    Abbreviation = "SMT"
                },
                new Currency
                {
                    Id = 1853,
                    CurrencyTypeId = 2,
                    Name = "OAX",
                    Abbreviation = "OAX"
                },
                new Currency
                {
                    Id = 64,
                    CurrencyTypeId = 2,
                    Name = "FLO",
                    Abbreviation = "FLO"
                },
                new Currency
                {
                    Id = 3261,
                    CurrencyTypeId = 2,
                    Name = "EvenCoin",
                    Abbreviation = "EVN"
                },
                new Currency
                {
                    Id = 2181,
                    CurrencyTypeId = 2,
                    Name = "Genesis Vision",
                    Abbreviation = "GVT"
                },
                new Currency
                {
                    Id = 470,
                    CurrencyTypeId = 2,
                    Name = "Viacoin",
                    Abbreviation = "VIA"
                },
                new Currency
                {
                    Id = 3684,
                    CurrencyTypeId = 2,
                    Name = "Bitcoiin",
                    Abbreviation = "B2G"
                },
                new Currency
                {
                    Id = 2586,
                    CurrencyTypeId = 2,
                    Name = "Synthetix Network Token",
                    Abbreviation = "SNX"
                },
                new Currency
                {
                    Id = 2455,
                    CurrencyTypeId = 2,
                    Name = "PressOne",
                    Abbreviation = "PRS"
                },
                new Currency
                {
                    Id = 2313,
                    CurrencyTypeId = 2,
                    Name = "SIRIN LABS Token",
                    Abbreviation = "SRN"
                },
                new Currency
                {
                    Id = 2400,
                    CurrencyTypeId = 2,
                    Name = "OneRoot Network",
                    Abbreviation = "RNT"
                },
                new Currency
                {
                    Id = 2379,
                    CurrencyTypeId = 2,
                    Name = "Kcash",
                    Abbreviation = "KCASH"
                },
                new Currency
                {
                    Id = 377,
                    CurrencyTypeId = 2,
                    Name = "NavCoin",
                    Abbreviation = "NAV"
                },
                new Currency
                {
                    Id = 2066,
                    CurrencyTypeId = 2,
                    Name = "Everus",
                    Abbreviation = "EVR"
                },
                new Currency
                {
                    Id = 2552,
                    CurrencyTypeId = 2,
                    Name = "IHT Real Estate Protocol",
                    Abbreviation = "IHT"
                },
                new Currency
                {
                    Id = 2446,
                    CurrencyTypeId = 2,
                    Name = "DATA",
                    Abbreviation = "DTA"
                },
                new Currency
                {
                    Id = 2274,
                    CurrencyTypeId = 2,
                    Name = "MediShares",
                    Abbreviation = "MDS"
                },
                new Currency
                {
                    Id = 1817,
                    CurrencyTypeId = 2,
                    Name = "Ethos",
                    Abbreviation = "ETHOS"
                },
                new Currency
                {
                    Id = 2235,
                    CurrencyTypeId = 2,
                    Name = "Time New Bank",
                    Abbreviation = "TNB"
                },
                new Currency
                {
                    Id = 2765,
                    CurrencyTypeId = 2,
                    Name = "XYO",
                    Abbreviation = "XYO"
                },
                new Currency
                {
                    Id = 2505,
                    CurrencyTypeId = 2,
                    Name = "Bluzelle",
                    Abbreviation = "BLZ"
                },
                new Currency
                {
                    Id = 1660,
                    CurrencyTypeId = 2,
                    Name = "TokenCard",
                    Abbreviation = "TKN"
                },
                new Currency
                {
                    Id = 1923,
                    CurrencyTypeId = 2,
                    Name = "Tierion",
                    Abbreviation = "TNT"
                },
                new Currency
                {
                    Id = 3715,
                    CurrencyTypeId = 2,
                    Name = "Cajutel",
                    Abbreviation = "CAJ"
                },
                new Currency
                {
                    Id = 2918,
                    CurrencyTypeId = 2,
                    Name = "Bit-Z Token",
                    Abbreviation = "BZ"
                },
                new Currency
                {
                    Id = 2447,
                    CurrencyTypeId = 2,
                    Name = "Crypterium",
                    Abbreviation = "CRPT"
                },
                new Currency
                {
                    Id = 2538,
                    CurrencyTypeId = 2,
                    Name = "Nectar",
                    Abbreviation = "NEC"
                },
                new Currency
                {
                    Id = 1505,
                    CurrencyTypeId = 2,
                    Name = "Spectrecoin",
                    Abbreviation = "XSPEC"
                },
                new Currency
                {
                    Id = 3164,
                    CurrencyTypeId = 2,
                    Name = "PumaPay",
                    Abbreviation = "PMA"
                },
                new Currency
                {
                    Id = 1828,
                    CurrencyTypeId = 2,
                    Name = "SmartCash",
                    Abbreviation = "SMART"
                },
                new Currency
                {
                    Id = 1732,
                    CurrencyTypeId = 2,
                    Name = "Numeraire",
                    Abbreviation = "NMR"
                },
                new Currency
                {
                    Id = 3631,
                    CurrencyTypeId = 2,
                    Name = "FOAM",
                    Abbreviation = "FOAM"
                },
                new Currency
                {
                    Id = 2507,
                    CurrencyTypeId = 2,
                    Name = "THEKEY",
                    Abbreviation = "TKY"
                },
                new Currency
                {
                    Id = 2727,
                    CurrencyTypeId = 2,
                    Name = "Bezant",
                    Abbreviation = "BZNT"
                },
                new Currency
                {
                    Id = 2559,
                    CurrencyTypeId = 2,
                    Name = "Cube",
                    Abbreviation = "AUTO"
                },
                new Currency
                {
                    Id = 3733,
                    CurrencyTypeId = 2,
                    Name = "S4FE",
                    Abbreviation = "S4F"
                },
                new Currency
                {
                    Id = 2661,
                    CurrencyTypeId = 2,
                    Name = "Tripio",
                    Abbreviation = "TRIO"
                },
                new Currency
                {
                    Id = 1983,
                    CurrencyTypeId = 2,
                    Name = "VIBE",
                    Abbreviation = "VIBE"
                },
                new Currency
                {
                    Id = 3063,
                    CurrencyTypeId = 2,
                    Name = "Vitae",
                    Abbreviation = "VITAE"
                },
                new Currency
                {
                    Id = 2033,
                    CurrencyTypeId = 2,
                    Name = "BridgeCoin",
                    Abbreviation = "BCO"
                },
                new Currency
                {
                    Id = 3932,
                    CurrencyTypeId = 2,
                    Name = "Connect Coin",
                    Abbreviation = "XCON"
                },
                new Currency
                {
                    Id = 1358,
                    CurrencyTypeId = 2,
                    Name = "EDC Blockchain",
                    Abbreviation = "EDC"
                },
                new Currency
                {
                    Id = 1768,
                    CurrencyTypeId = 2,
                    Name = "AdEx",
                    Abbreviation = "ADX"
                },
                new Currency
                {
                    Id = 1711,
                    CurrencyTypeId = 2,
                    Name = "Electra",
                    Abbreviation = "ECA"
                },
                new Currency
                {
                    Id = 2503,
                    CurrencyTypeId = 2,
                    Name = "DMarket",
                    Abbreviation = "DMT"
                },
                new Currency
                {
                    Id = 2873,
                    CurrencyTypeId = 2,
                    Name = "Metronome",
                    Abbreviation = "MET"
                },
                new Currency
                {
                    Id = 1974,
                    CurrencyTypeId = 2,
                    Name = "Propy",
                    Abbreviation = "PRO"
                },
                new Currency
                {
                    Id = 2544,
                    CurrencyTypeId = 2,
                    Name = "Nucleus Vision",
                    Abbreviation = "NCASH"
                },
                new Currency
                {
                    Id = 3784,
                    CurrencyTypeId = 2,
                    Name = "OVCODE",
                    Abbreviation = "OVC"
                },
                new Currency
                {
                    Id = 1447,
                    CurrencyTypeId = 2,
                    Name = "ZClassic",
                    Abbreviation = "ZCL"
                },
                new Currency
                {
                    Id = 3251,
                    CurrencyTypeId = 2,
                    Name = "ParkinGo",
                    Abbreviation = "GOT"
                },
                new Currency
                {
                    Id = 1596,
                    CurrencyTypeId = 2,
                    Name = "Edgeless",
                    Abbreviation = "EDG"
                },
                new Currency
                {
                    Id = 2428,
                    CurrencyTypeId = 2,
                    Name = "Scry.info",
                    Abbreviation = "DDD"
                },
                new Currency
                {
                    Id = 2767,
                    CurrencyTypeId = 2,
                    Name = "APIS",
                    Abbreviation = "APIS"
                },
                new Currency
                {
                    Id = 588,
                    CurrencyTypeId = 2,
                    Name = "Ubiq",
                    Abbreviation = "UBQ"
                },
                new Currency
                {
                    Id = 3139,
                    CurrencyTypeId = 2,
                    Name = "DxChain Token",
                    Abbreviation = "DX"
                },
                new Currency
                {
                    Id = 1609,
                    CurrencyTypeId = 2,
                    Name = "Asch",
                    Abbreviation = "XAS"
                },
                new Currency
                {
                    Id = 2316,
                    CurrencyTypeId = 2,
                    Name = "DeepBrain Chain",
                    Abbreviation = "DBC"
                },
                new Currency
                {
                    Id = 2021,
                    CurrencyTypeId = 2,
                    Name = "RChain",
                    Abbreviation = "RHOC"
                },
                new Currency
                {
                    Id = 2369,
                    CurrencyTypeId = 2,
                    Name = "Insolar",
                    Abbreviation = "INS"
                },
                new Currency
                {
                    Id = 1856,
                    CurrencyTypeId = 2,
                    Name = "district0x",
                    Abbreviation = "DNT"
                },
                new Currency
                {
                    Id = 406,
                    CurrencyTypeId = 2,
                    Name = "Boolberry",
                    Abbreviation = "BBR"
                },
                new Currency
                {
                    Id = 405,
                    CurrencyTypeId = 2,
                    Name = "DigitalNote",
                    Abbreviation = "XDN"
                },
                new Currency
                {
                    Id = 3066,
                    CurrencyTypeId = 2,
                    Name = "BitCapitalVendor",
                    Abbreviation = "BCV"
                },
                new Currency
                {
                    Id = 2223,
                    CurrencyTypeId = 2,
                    Name = "BLOCKv",
                    Abbreviation = "VEE"
                },
                new Currency
                {
                    Id = 558,
                    CurrencyTypeId = 2,
                    Name = "Emercoin",
                    Abbreviation = "EMC"
                },
                new Currency
                {
                    Id = 2341,
                    CurrencyTypeId = 2,
                    Name = "SwftCoin",
                    Abbreviation = "SWFTC"
                },
                new Currency
                {
                    Id = 2556,
                    CurrencyTypeId = 2,
                    Name = "Credits",
                    Abbreviation = "CS"
                },
                new Currency
                {
                    Id = 2239,
                    CurrencyTypeId = 2,
                    Name = "ETHLend",
                    Abbreviation = "LEND"
                },
                new Currency
                {
                    Id = 624,
                    CurrencyTypeId = 2,
                    Name = "bitCNY",
                    Abbreviation = "BITCNY"
                },
                new Currency
                {
                    Id = 1949,
                    CurrencyTypeId = 2,
                    Name = "Agrello",
                    Abbreviation = "DLT"
                },
                new Currency
                {
                    Id = 1681,
                    CurrencyTypeId = 2,
                    Name = "PRIZM",
                    Abbreviation = "PZM"
                },
                new Currency
                {
                    Id = 2321,
                    CurrencyTypeId = 2,
                    Name = "QLC Chain",
                    Abbreviation = "QLC"
                },
                new Currency
                {
                    Id = 1814,
                    CurrencyTypeId = 2,
                    Name = "Linda",
                    Abbreviation = "LINDA"
                },
                new Currency
                {
                    Id = 1409,
                    CurrencyTypeId = 2,
                    Name = "SingularDTV",
                    Abbreviation = "SNGLS"
                },
                new Currency
                {
                    Id = 2673,
                    CurrencyTypeId = 2,
                    Name = "Own",
                    Abbreviation = "CHX"
                },
                new Currency
                {
                    Id = 2398,
                    CurrencyTypeId = 2,
                    Name = "Selfkey",
                    Abbreviation = "KEY"
                },
                new Currency
                {
                    Id = 3648,
                    CurrencyTypeId = 2,
                    Name = "CoinUs",
                    Abbreviation = "CNUS"
                },
                new Currency
                {
                    Id = 37,
                    CurrencyTypeId = 2,
                    Name = "Peercoin",
                    Abbreviation = "PPC"
                },
                new Currency
                {
                    Id = 3686,
                    CurrencyTypeId = 2,
                    Name = "Content Value Network",
                    Abbreviation = "CVNT"
                },
                new Currency
                {
                    Id = 2526,
                    CurrencyTypeId = 2,
                    Name = "Envion",
                    Abbreviation = "EVN"
                },
                new Currency
                {
                    Id = 2953,
                    CurrencyTypeId = 2,
                    Name = "Blue Whale EXchange",
                    Abbreviation = "BWX"
                },
                new Currency
                {
                    Id = 2204,
                    CurrencyTypeId = 2,
                    Name = "B2BX",
                    Abbreviation = "B2B"
                },
                new Currency
                {
                    Id = 1834,
                    CurrencyTypeId = 2,
                    Name = "Pillar",
                    Abbreviation = "PLR"
                },
                new Currency
                {
                    Id = 2554,
                    CurrencyTypeId = 2,
                    Name = "Lympo",
                    Abbreviation = "LYM"
                },
                new Currency
                {
                    Id = 1723,
                    CurrencyTypeId = 2,
                    Name = "SONM",
                    Abbreviation = "SNM"
                },
                new Currency
                {
                    Id = 1592,
                    CurrencyTypeId = 2,
                    Name = "TaaS",
                    Abbreviation = "TAAS"
                },
                new Currency
                {
                    Id = 2576,
                    CurrencyTypeId = 2,
                    Name = "Tokenomy",
                    Abbreviation = "TEN"
                },
                new Currency
                {
                    Id = 3475,
                    CurrencyTypeId = 2,
                    Name = "BOX Token",
                    Abbreviation = "BOX"
                },
                new Currency
                {
                    Id = 2473,
                    CurrencyTypeId = 2,
                    Name = "All Sports",
                    Abbreviation = "SOC"
                },
                new Currency
                {
                    Id = 723,
                    CurrencyTypeId = 2,
                    Name = "BitBay",
                    Abbreviation = "BAY"
                },
                new Currency
                {
                    Id = 2153,
                    CurrencyTypeId = 2,
                    Name = "Aeron",
                    Abbreviation = "ARN"
                },
                new Currency
                {
                    Id = 2991,
                    CurrencyTypeId = 2,
                    Name = "NIX",
                    Abbreviation = "NIX"
                },
                new Currency
                {
                    Id = 3316,
                    CurrencyTypeId = 2,
                    Name = "smARTOFGIVING",
                    Abbreviation = "AOG"
                },
                new Currency
                {
                    Id = 2344,
                    CurrencyTypeId = 2,
                    Name = "AppCoins",
                    Abbreviation = "APPC"
                },
                new Currency
                {
                    Id = 1996,
                    CurrencyTypeId = 2,
                    Name = "SALT",
                    Abbreviation = "SALT"
                },
                new Currency
                {
                    Id = 2826,
                    CurrencyTypeId = 2,
                    Name = "Zipper",
                    Abbreviation = "ZIP"
                },
                new Currency
                {
                    Id = 1726,
                    CurrencyTypeId = 2,
                    Name = "ZrCoin",
                    Abbreviation = "ZRC"
                },
                new Currency
                {
                    Id = 254,
                    CurrencyTypeId = 2,
                    Name = "Gulden",
                    Abbreviation = "NLG"
                },
                new Currency
                {
                    Id = 1312,
                    CurrencyTypeId = 2,
                    Name = "Steem Dollars",
                    Abbreviation = "SBD"
                },
                new Currency
                {
                    Id = 3664,
                    CurrencyTypeId = 2,
                    Name = "AgaveCoin",
                    Abbreviation = "AGVC"
                },
                new Currency
                {
                    Id = 2633,
                    CurrencyTypeId = 2,
                    Name = "Stakenet",
                    Abbreviation = "XSN"
                },
                new Currency
                {
                    Id = 2553,
                    CurrencyTypeId = 2,
                    Name = "Refereum",
                    Abbreviation = "RFR"
                },
                new Currency
                {
                    Id = 2642,
                    CurrencyTypeId = 2,
                    Name = "CyberVein",
                    Abbreviation = "CVT"
                },
                new Currency
                {
                    Id = 2467,
                    CurrencyTypeId = 2,
                    Name = "OriginTrail",
                    Abbreviation = "TRAC"
                },
                new Currency
                {
                    Id = 2019,
                    CurrencyTypeId = 2,
                    Name = "Viberate",
                    Abbreviation = "VIB"
                },
                new Currency
                {
                    Id = 2287,
                    CurrencyTypeId = 2,
                    Name = "LockTrip",
                    Abbreviation = "LOC"
                },
                new Currency
                {
                    Id = 3702,
                    CurrencyTypeId = 2,
                    Name = "Beam",
                    Abbreviation = "BEAM"
                },
                new Currency
                {
                    Id = 1984,
                    CurrencyTypeId = 2,
                    Name = "Substratum",
                    Abbreviation = "SUB"
                },
                new Currency
                {
                    Id = 2336,
                    CurrencyTypeId = 2,
                    Name = "Game.com",
                    Abbreviation = "GTC"
                },
                new Currency
                {
                    Id = 2876,
                    CurrencyTypeId = 2,
                    Name = "Ternio",
                    Abbreviation = "TERN"
                },
                new Currency
                {
                    Id = 3911,
                    CurrencyTypeId = 2,
                    Name = "Ocean Protocol",
                    Abbreviation = "OCEAN"
                },
                new Currency
                {
                    Id = 1947,
                    CurrencyTypeId = 2,
                    Name = "Monetha",
                    Abbreviation = "MTH"
                },
                new Currency
                {
                    Id = 2303,
                    CurrencyTypeId = 2,
                    Name = "MediBloc [QRC20]",
                    Abbreviation = "MED"
                },
                new Currency
                {
                    Id = 1552,
                    CurrencyTypeId = 2,
                    Name = "Melon",
                    Abbreviation = "MLN"
                },
                new Currency
                {
                    Id = 2120,
                    CurrencyTypeId = 2,
                    Name = "Etherparty",
                    Abbreviation = "FUEL"
                },
                new Currency
                {
                    Id = 2061,
                    CurrencyTypeId = 2,
                    Name = "BlockMason Credit Protocol",
                    Abbreviation = "BCPT"
                },
                new Currency
                {
                    Id = 1026,
                    CurrencyTypeId = 2,
                    Name = "Aeon",
                    Abbreviation = "AEON"
                },
                new Currency
                {
                    Id = 2318,
                    CurrencyTypeId = 2,
                    Name = "Neumark",
                    Abbreviation = "NEU"
                },
                new Currency
                {
                    Id = 2548,
                    CurrencyTypeId = 2,
                    Name = "POA Network",
                    Abbreviation = "POA"
                },
                new Currency
                {
                    Id = 2058,
                    CurrencyTypeId = 2,
                    Name = "AirSwap",
                    Abbreviation = "AST"
                },
                new Currency
                {
                    Id = 3655,
                    CurrencyTypeId = 2,
                    Name = "Fiii",
                    Abbreviation = "FIII"
                },
                new Currency
                {
                    Id = 3307,
                    CurrencyTypeId = 2,
                    Name = "Spendcoin",
                    Abbreviation = "SPND"
                },
                new Currency
                {
                    Id = 3928,
                    CurrencyTypeId = 2,
                    Name = "IDEX",
                    Abbreviation = "IDEX"
                },
                new Currency
                {
                    Id = 3710,
                    CurrencyTypeId = 2,
                    Name = "SDChain",
                    Abbreviation = "SDA"
                },
                new Currency
                {
                    Id = 3632,
                    CurrencyTypeId = 2,
                    Name = "Opacity",
                    Abbreviation = "OPQ"
                },
                new Currency
                {
                    Id = 2506,
                    CurrencyTypeId = 2,
                    Name = "Swarm",
                    Abbreviation = "SWM"
                },
                new Currency
                {
                    Id = 2600,
                    CurrencyTypeId = 2,
                    Name = "LGO Exchange",
                    Abbreviation = "LGO"
                },
                new Currency
                {
                    Id = 1841,
                    CurrencyTypeId = 2,
                    Name = "Primalbase Token",
                    Abbreviation = "PBT"
                },
                new Currency
                {
                    Id = 2901,
                    CurrencyTypeId = 2,
                    Name = "FansTime",
                    Abbreviation = "FTI"
                },
                new Currency
                {
                    Id = 2511,
                    CurrencyTypeId = 2,
                    Name = "WePower",
                    Abbreviation = "WPR"
                },
                new Currency
                {
                    Id = 2644,
                    CurrencyTypeId = 2,
                    Name = "eosDAC",
                    Abbreviation = "EOSDAC"
                },
                new Currency
                {
                    Id = 3840,
                    CurrencyTypeId = 2,
                    Name = "1irstcoin",
                    Abbreviation = "FST"
                },
                new Currency
                {
                    Id = 2735,
                    CurrencyTypeId = 2,
                    Name = "Content Neutrality Network",
                    Abbreviation = "CNN"
                },
                new Currency
                {
                    Id = 1864,
                    CurrencyTypeId = 2,
                    Name = "Blox",
                    Abbreviation = "CDT"
                },
                new Currency
                {
                    Id = 1475,
                    CurrencyTypeId = 2,
                    Name = "Incent",
                    Abbreviation = "INCNT"
                },
                new Currency
                {
                    Id = 2561,
                    CurrencyTypeId = 2,
                    Name = "BitTube",
                    Abbreviation = "TUBE"
                },
                new Currency
                {
                    Id = 3683,
                    CurrencyTypeId = 2,
                    Name = "Aencoin",
                    Abbreviation = "AEN"
                },
                new Currency
                {
                    Id = 2726,
                    CurrencyTypeId = 2,
                    Name = "DAOstack",
                    Abbreviation = "GEN"
                },
                new Currency
                {
                    Id = 1298,
                    CurrencyTypeId = 2,
                    Name = "LBRY Credits",
                    Abbreviation = "LBC"
                },
                new Currency
                {
                    Id = 2036,
                    CurrencyTypeId = 2,
                    Name = "PayPie",
                    Abbreviation = "PPP"
                },
                new Currency
                {
                    Id = 2675,
                    CurrencyTypeId = 2,
                    Name = "Dock",
                    Abbreviation = "DOCK"
                },
                new Currency
                {
                    Id = 2081,
                    CurrencyTypeId = 2,
                    Name = "Ambrosus",
                    Abbreviation = "AMB"
                },
                new Currency
                {
                    Id = 2399,
                    CurrencyTypeId = 2,
                    Name = "Internet Node Token",
                    Abbreviation = "INT"
                },
                new Currency
                {
                    Id = 2698,
                    CurrencyTypeId = 2,
                    Name = "Hydro",
                    Abbreviation = "HYDRO"
                },
                new Currency
                {
                    Id = 2604,
                    CurrencyTypeId = 2,
                    Name = "BitGreen",
                    Abbreviation = "BITG"
                },
                new Currency
                {
                    Id = 3853,
                    CurrencyTypeId = 2,
                    Name = "MultiVAC",
                    Abbreviation = "MTV"
                },
                new Currency
                {
                    Id = 3642,
                    CurrencyTypeId = 2,
                    Name = "Trade Token X",
                    Abbreviation = "TIOX"
                },
                new Currency
                {
                    Id = 2533,
                    CurrencyTypeId = 2,
                    Name = "Restart Energy MWAT",
                    Abbreviation = "MWAT"
                },
                new Currency
                {
                    Id = 1899,
                    CurrencyTypeId = 2,
                    Name = "YOYOW",
                    Abbreviation = "YOYOW"
                },
                new Currency
                {
                    Id = 2095,
                    CurrencyTypeId = 2,
                    Name = "BOScoin",
                    Abbreviation = "BOS"
                },
                new Currency
                {
                    Id = 914,
                    CurrencyTypeId = 2,
                    Name = "Sphere",
                    Abbreviation = "SPHR"
                },
                new Currency
                {
                    Id = 1715,
                    CurrencyTypeId = 2,
                    Name = "MobileGo",
                    Abbreviation = "MGO"
                },
                new Currency
                {
                    Id = 1473,
                    CurrencyTypeId = 2,
                    Name = "Pascal Coin",
                    Abbreviation = "PASC"
                },
                new Currency
                {
                    Id = 3142,
                    CurrencyTypeId = 2,
                    Name = "BaaSid",
                    Abbreviation = "BAAS"
                },
                new Currency
                {
                    Id = 2602,
                    CurrencyTypeId = 2,
                    Name = "NaPoleonX",
                    Abbreviation = "NPX"
                },
                new Currency
                {
                    Id = 1022,
                    CurrencyTypeId = 2,
                    Name = "LEOcoin",
                    Abbreviation = "LEO"
                },
                new Currency
                {
                    Id = 1930,
                    CurrencyTypeId = 2,
                    Name = "Primas",
                    Abbreviation = "PST"
                },
                new Currency
                {
                    Id = 3618,
                    CurrencyTypeId = 2,
                    Name = "STACS",
                    Abbreviation = "STACS"
                },
                new Currency
                {
                    Id = 2916,
                    CurrencyTypeId = 2,
                    Name = "Nimiq",
                    Abbreviation = "NIM"
                },
                new Currency
                {
                    Id = 2529,
                    CurrencyTypeId = 2,
                    Name = "Cashaa",
                    Abbreviation = "CAS"
                },
                new Currency
                {
                    Id = 2498,
                    CurrencyTypeId = 2,
                    Name = "Jibrel Network",
                    Abbreviation = "JNT"
                },
                new Currency
                {
                    Id = 2482,
                    CurrencyTypeId = 2,
                    Name = "CPChain",
                    Abbreviation = "CPC"
                },
                new Currency
                {
                    Id = 42,
                    CurrencyTypeId = 2,
                    Name = "Primecoin",
                    Abbreviation = "XPM"
                },
                new Currency
                {
                    Id = 2064,
                    CurrencyTypeId = 2,
                    Name = "Maecenas",
                    Abbreviation = "ART"
                },
                new Currency
                {
                    Id = 2429,
                    CurrencyTypeId = 2,
                    Name = "Mobius",
                    Abbreviation = "MOBI"
                },
                new Currency
                {
                    Id = 2693,
                    CurrencyTypeId = 2,
                    Name = "Loopring [NEO]",
                    Abbreviation = "LRN"
                },
                new Currency
                {
                    Id = 1527,
                    CurrencyTypeId = 2,
                    Name = "Waves Community Token",
                    Abbreviation = "WCT"
                },
                new Currency
                {
                    Id = 3719,
                    CurrencyTypeId = 2,
                    Name = "StableUSD",
                    Abbreviation = "USDS"
                },
                new Currency
                {
                    Id = 3040,
                    CurrencyTypeId = 2,
                    Name = "CanonChain",
                    Abbreviation = "CZR"
                },
                new Currency
                {
                    Id = 2245,
                    CurrencyTypeId = 2,
                    Name = "Presearch",
                    Abbreviation = "PRE"
                },
                new Currency
                {
                    Id = 2175,
                    CurrencyTypeId = 2,
                    Name = "DecentBet",
                    Abbreviation = "DBET"
                },
                new Currency
                {
                    Id = 170,
                    CurrencyTypeId = 2,
                    Name = "BlackCoin",
                    Abbreviation = "BLK"
                },
                new Currency
                {
                    Id = 576,
                    CurrencyTypeId = 2,
                    Name = "GameCredits",
                    Abbreviation = "GAME"
                },
                new Currency
                {
                    Id = 2540,
                    CurrencyTypeId = 2,
                    Name = "Litecoin Cash",
                    Abbreviation = "LCC"
                },
                new Currency
                {
                    Id = 448,
                    CurrencyTypeId = 2,
                    Name = "Stealth",
                    Abbreviation = "XST"
                },
                new Currency
                {
                    Id = 1590,
                    CurrencyTypeId = 2,
                    Name = "Mercury",
                    Abbreviation = "MER"
                },
                new Currency
                {
                    Id = 1050,
                    CurrencyTypeId = 2,
                    Name = "Shift",
                    Abbreviation = "SHIFT"
                },
                new Currency
                {
                    Id = 573,
                    CurrencyTypeId = 2,
                    Name = "Burst",
                    Abbreviation = "BURST"
                },
                new Currency
                {
                    Id = 2291,
                    CurrencyTypeId = 2,
                    Name = "Genaro Network",
                    Abbreviation = "GNX"
                },
                new Currency
                {
                    Id = 2691,
                    CurrencyTypeId = 2,
                    Name = "Penta",
                    Abbreviation = "PNT"
                },
                new Currency
                {
                    Id = 40,
                    CurrencyTypeId = 2,
                    Name = "Namecoin",
                    Abbreviation = "NMC"
                },
                new Currency
                {
                    Id = 2945,
                    CurrencyTypeId = 2,
                    Name = "ContentBox",
                    Abbreviation = "BOX"
                },
                new Currency
                {
                    Id = 1658,
                    CurrencyTypeId = 2,
                    Name = "Lunyr",
                    Abbreviation = "LUN"
                },
                new Currency
                {
                    Id = 2711,
                    CurrencyTypeId = 2,
                    Name = "doc.com Token",
                    Abbreviation = "MTC"
                },
                new Currency
                {
                    Id = 2748,
                    CurrencyTypeId = 2,
                    Name = "Loki",
                    Abbreviation = "LOKI"
                },
                new Currency
                {
                    Id = 2958,
                    CurrencyTypeId = 2,
                    Name = "TurtleCoin",
                    Abbreviation = "TRTL"
                },
                new Currency
                {
                    Id = 2524,
                    CurrencyTypeId = 2,
                    Name = "Universa",
                    Abbreviation = "UTNP"
                },
                new Currency
                {
                    Id = 2375,
                    CurrencyTypeId = 2,
                    Name = "QunQun",
                    Abbreviation = "QUN"
                },
                new Currency
                {
                    Id = 2838,
                    CurrencyTypeId = 2,
                    Name = "PCHAIN",
                    Abbreviation = "PI"
                },
                new Currency
                {
                    Id = 2481,
                    CurrencyTypeId = 2,
                    Name = "Zeepin",
                    Abbreviation = "ZPT"
                },
                new Currency
                {
                    Id = 2480,
                    CurrencyTypeId = 2,
                    Name = "HalalChain",
                    Abbreviation = "HLC"
                },
                new Currency
                {
                    Id = 2776,
                    CurrencyTypeId = 2,
                    Name = "Travala.com",
                    Abbreviation = "AVA"
                },
                new Currency
                {
                    Id = 2763,
                    CurrencyTypeId = 2,
                    Name = "Morpheus.Network",
                    Abbreviation = "MRPH"
                },
                new Currency
                {
                    Id = 2650,
                    CurrencyTypeId = 2,
                    Name = "CommerceBlock",
                    Abbreviation = "CBT"
                },
                new Currency
                {
                    Id = 2546,
                    CurrencyTypeId = 2,
                    Name = "Remme",
                    Abbreviation = "REM"
                },
                new Currency
                {
                    Id = 2766,
                    CurrencyTypeId = 2,
                    Name = "Cryptaur",
                    Abbreviation = "CPT"
                },
                new Currency
                {
                    Id = 3242,
                    CurrencyTypeId = 2,
                    Name = "Beetle Coin",
                    Abbreviation = "BEET"
                },
                new Currency
                {
                    Id = 1636,
                    CurrencyTypeId = 2,
                    Name = "XTRABYTES",
                    Abbreviation = "XBY"
                },
                new Currency
                {
                    Id = 2392,
                    CurrencyTypeId = 2,
                    Name = "Bottos",
                    Abbreviation = "BTO"
                },
                new Currency
                {
                    Id = 3441,
                    CurrencyTypeId = 2,
                    Name = "Divi",
                    Abbreviation = "DIVI"
                },
                new Currency
                {
                    Id = 3029,
                    CurrencyTypeId = 2,
                    Name = "ZelCash",
                    Abbreviation = "ZEL"
                },
                new Currency
                {
                    Id = 3843,
                    CurrencyTypeId = 2,
                    Name = "BOLT",
                    Abbreviation = "BOLT"
                },
                new Currency
                {
                    Id = 3260,
                    CurrencyTypeId = 2,
                    Name = "AMO Coin",
                    Abbreviation = "AMO"
                },
                new Currency
                {
                    Id = 1478,
                    CurrencyTypeId = 2,
                    Name = "DECENT",
                    Abbreviation = "DCT"
                },
                new Currency
                {
                    Id = 2209,
                    CurrencyTypeId = 2,
                    Name = "Ink",
                    Abbreviation = "INK"
                },
                new Currency
                {
                    Id = 1775,
                    CurrencyTypeId = 2,
                    Name = "adToken",
                    Abbreviation = "ADT"
                },
                new Currency
                {
                    Id = 3515,
                    CurrencyTypeId = 2,
                    Name = "DEX",
                    Abbreviation = "DEX"
                },
                new Currency
                {
                    Id = 3838,
                    CurrencyTypeId = 2,
                    Name = "Esportbits",
                    Abbreviation = "HLT"
                },
                new Currency
                {
                    Id = 2866,
                    CurrencyTypeId = 2,
                    Name = "Sentinel Protocol",
                    Abbreviation = "UPP"
                },
                new Currency
                {
                    Id = 3650,
                    CurrencyTypeId = 2,
                    Name = "COVA",
                    Abbreviation = "COVA"
                },
                new Currency
                {
                    Id = 3842,
                    CurrencyTypeId = 2,
                    Name = "Caspian",
                    Abbreviation = "CSP"
                },
                new Currency
                {
                    Id = 2348,
                    CurrencyTypeId = 2,
                    Name = "Measurable Data Token",
                    Abbreviation = "MDT"
                },
                new Currency
                {
                    Id = 1786,
                    CurrencyTypeId = 2,
                    Name = "SunContract",
                    Abbreviation = "SNC"
                },
                new Currency
                {
                    Id = 3628,
                    CurrencyTypeId = 2,
                    Name = "Machine Xchange Coin",
                    Abbreviation = "MXC"
                },
                new Currency
                {
                    Id = 2665,
                    CurrencyTypeId = 2,
                    Name = "Dero",
                    Abbreviation = "DERO"
                },
                new Currency
                {
                    Id = 1976,
                    CurrencyTypeId = 2,
                    Name = "Blackmoon",
                    Abbreviation = "BMC"
                },
                new Currency
                {
                    Id = 1881,
                    CurrencyTypeId = 2,
                    Name = "DeepOnion",
                    Abbreviation = "ONION"
                },
                new Currency
                {
                    Id = 2847,
                    CurrencyTypeId = 2,
                    Name = "Abyss Token",
                    Abbreviation = "ABYSS"
                },
                new Currency
                {
                    Id = 3694,
                    CurrencyTypeId = 2,
                    Name = "INMAX",
                    Abbreviation = "INX"
                },
                new Currency
                {
                    Id = 34,
                    CurrencyTypeId = 2,
                    Name = "Feathercoin",
                    Abbreviation = "FTC"
                },
                new Currency
                {
                    Id = 1172,
                    CurrencyTypeId = 2,
                    Name = "Safex Token",
                    Abbreviation = "SFT"
                },
                new Currency
                {
                    Id = 1405,
                    CurrencyTypeId = 2,
                    Name = "Pepe Cash",
                    Abbreviation = "PEPECASH"
                },
                new Currency
                {
                    Id = 2830,
                    CurrencyTypeId = 2,
                    Name = "Seele",
                    Abbreviation = "SEELE"
                },
                new Currency
                {
                    Id = 2472,
                    CurrencyTypeId = 2,
                    Name = "Fortuna",
                    Abbreviation = "FOTA"
                },
                new Currency
                {
                    Id = 1403,
                    CurrencyTypeId = 2,
                    Name = "FirstBlood",
                    Abbreviation = "1ST"
                },
                new Currency
                {
                    Id = 3623,
                    CurrencyTypeId = 2,
                    Name = "Online",
                    Abbreviation = "OIO"
                },
                new Currency
                {
                    Id = 2359,
                    CurrencyTypeId = 2,
                    Name = "Polis",
                    Abbreviation = "POLIS"
                },
                new Currency
                {
                    Id = 1989,
                    CurrencyTypeId = 2,
                    Name = "COSS",
                    Abbreviation = "COSS"
                },
                new Currency
                {
                    Id = 3070,
                    CurrencyTypeId = 2,
                    Name = "Litex",
                    Abbreviation = "LXT"
                },
                new Currency
                {
                    Id = 3156,
                    CurrencyTypeId = 2,
                    Name = "Airbloc",
                    Abbreviation = "ABL"
                },
                new Currency
                {
                    Id = 2342,
                    CurrencyTypeId = 2,
                    Name = "Covesting",
                    Abbreviation = "COV"
                },
                new Currency
                {
                    Id = 3913,
                    CurrencyTypeId = 2,
                    Name = "Titan Coin",
                    Abbreviation = "TTN"
                },
                new Currency
                {
                    Id = 2685,
                    CurrencyTypeId = 2,
                    Name = "Zebi",
                    Abbreviation = "ZCO"
                },
                new Currency
                {
                    Id = 2757,
                    CurrencyTypeId = 2,
                    Name = "Callisto Network",
                    Abbreviation = "CLO"
                },
                new Currency
                {
                    Id = 2737,
                    CurrencyTypeId = 2,
                    Name = "Global Social Chain",
                    Abbreviation = "GSC"
                },
                new Currency
                {
                    Id = 3376,
                    CurrencyTypeId = 2,
                    Name = "Darico Ecosystem Coin",
                    Abbreviation = "DEC"
                },
                new Currency
                {
                    Id = 2468,
                    CurrencyTypeId = 2,
                    Name = "LinkEye",
                    Abbreviation = "LET"
                },
                new Currency
                {
                    Id = 3716,
                    CurrencyTypeId = 2,
                    Name = "Amoveo",
                    Abbreviation = "VEO"
                },
                new Currency
                {
                    Id = 2862,
                    CurrencyTypeId = 2,
                    Name = "Smartshare",
                    Abbreviation = "SSP"
                },
                new Currency
                {
                    Id = 2158,
                    CurrencyTypeId = 2,
                    Name = "Phore",
                    Abbreviation = "PHR"
                },
                new Currency
                {
                    Id = 2380,
                    CurrencyTypeId = 2,
                    Name = "ATN",
                    Abbreviation = "ATN"
                },
                new Currency
                {
                    Id = 3815,
                    CurrencyTypeId = 2,
                    Name = "Eterbase Coin",
                    Abbreviation = "XBASE"
                },
                new Currency
                {
                    Id = 1677,
                    CurrencyTypeId = 2,
                    Name = "Etheroll",
                    Abbreviation = "DICE"
                },
                new Currency
                {
                    Id = 3944,
                    CurrencyTypeId = 2,
                    Name = "Artfinity",
                    Abbreviation = "AT"
                },
                new Currency
                {
                    Id = 2535,
                    CurrencyTypeId = 2,
                    Name = "DADI",
                    Abbreviation = "DADI"
                },
                new Currency
                {
                    Id = 2933,
                    CurrencyTypeId = 2,
                    Name = "BitMart Token",
                    Abbreviation = "BMX"
                },
                new Currency
                {
                    Id = 3612,
                    CurrencyTypeId = 2,
                    Name = "Business Credit Alliance Chain",
                    Abbreviation = "BCAC"
                },
                new Currency
                {
                    Id = 2094,
                    CurrencyTypeId = 2,
                    Name = "Paragon",
                    Abbreviation = "PRG"
                },
                new Currency
                {
                    Id = 2001,
                    CurrencyTypeId = 2,
                    Name = "ColossusXT",
                    Abbreviation = "COLX"
                },
                new Currency
                {
                    Id = 2892,
                    CurrencyTypeId = 2,
                    Name = "Wowbit",
                    Abbreviation = "WWB"
                },
                new Currency
                {
                    Id = 2427,
                    CurrencyTypeId = 2,
                    Name = "ChatCoin",
                    Abbreviation = "CHAT"
                },
                new Currency
                {
                    Id = 2669,
                    CurrencyTypeId = 2,
                    Name = "MARK.SPACE",
                    Abbreviation = "MRK"
                },
                new Currency
                {
                    Id = 819,
                    CurrencyTypeId = 2,
                    Name = "Bean Cash",
                    Abbreviation = "BITB"
                },
                new Currency
                {
                    Id = 2882,
                    CurrencyTypeId = 2,
                    Name = "0Chain",
                    Abbreviation = "ZCN"
                },
                new Currency
                {
                    Id = 3281,
                    CurrencyTypeId = 2,
                    Name = "Quanta Utility Token",
                    Abbreviation = "QNTU"
                },
                new Currency
                {
                    Id = 3813,
                    CurrencyTypeId = 2,
                    Name = "PTON",
                    Abbreviation = "PTON"
                },
                new Currency
                {
                    Id = 2666,
                    CurrencyTypeId = 2,
                    Name = "Effect.AI",
                    Abbreviation = "EFX"
                },
                new Currency
                {
                    Id = 1883,
                    CurrencyTypeId = 2,
                    Name = "Adshares",
                    Abbreviation = "ADS"
                },
                new Currency
                {
                    Id = 2982,
                    CurrencyTypeId = 2,
                    Name = "MVL",
                    Abbreviation = "MVL"
                },
                new Currency
                {
                    Id = 1784,
                    CurrencyTypeId = 2,
                    Name = "Polybius",
                    Abbreviation = "PLBT"
                },
                new Currency
                {
                    Id = 2162,
                    CurrencyTypeId = 2,
                    Name = "Delphy",
                    Abbreviation = "DPY"
                },
                new Currency
                {
                    Id = 2499,
                    CurrencyTypeId = 2,
                    Name = "SwissBorg",
                    Abbreviation = "CHSB"
                },
                new Currency
                {
                    Id = 623,
                    CurrencyTypeId = 2,
                    Name = "bitUSD",
                    Abbreviation = "BITUSD"
                },
                new Currency
                {
                    Id = 1500,
                    CurrencyTypeId = 2,
                    Name = "Wings",
                    Abbreviation = "WINGS"
                },
                new Currency
                {
                    Id = 3722,
                    CurrencyTypeId = 2,
                    Name = "TEMCO",
                    Abbreviation = "TEMCO"
                },
                new Currency
                {
                    Id = 2739,
                    CurrencyTypeId = 2,
                    Name = "Digix Gold Token",
                    Abbreviation = "DGX"
                },
                new Currency
                {
                    Id = 2938,
                    CurrencyTypeId = 2,
                    Name = "Hashgard",
                    Abbreviation = "GARD"
                },
                new Currency
                {
                    Id = 400,
                    CurrencyTypeId = 2,
                    Name = "Kore",
                    Abbreviation = "KORE"
                },
                new Currency
                {
                    Id = 3200,
                    CurrencyTypeId = 2,
                    Name = "Nasdacoin",
                    Abbreviation = "NSD"
                },
                new Currency
                {
                    Id = 2430,
                    CurrencyTypeId = 2,
                    Name = "Hydro Protocol",
                    Abbreviation = "HOT"
                },
                new Currency
                {
                    Id = 3301,
                    CurrencyTypeId = 2,
                    Name = "Invictus Hyperion Fund",
                    Abbreviation = "IHF"
                },
                new Currency
                {
                    Id = 3371,
                    CurrencyTypeId = 2,
                    Name = "MIR COIN",
                    Abbreviation = "MIR"
                },
                new Currency
                {
                    Id = 3186,
                    CurrencyTypeId = 2,
                    Name = "Zen Protocol",
                    Abbreviation = "ZP"
                },
                new Currency
                {
                    Id = 2327,
                    CurrencyTypeId = 2,
                    Name = "Olympus Labs",
                    Abbreviation = "MOT"
                },
                new Currency
                {
                    Id = 1531,
                    CurrencyTypeId = 2,
                    Name = "Global Cryptocurrency",
                    Abbreviation = "GCC"
                },
                new Currency
                {
                    Id = 2709,
                    CurrencyTypeId = 2,
                    Name = "Morpheus Labs",
                    Abbreviation = "MITX"
                },
                new Currency
                {
                    Id = 3714,
                    CurrencyTypeId = 2,
                    Name = "LTO Network",
                    Abbreviation = "LTO"
                },
                new Currency
                {
                    Id = 3461,
                    CurrencyTypeId = 2,
                    Name = "PlayCoin [ERC20]",
                    Abbreviation = "PLY"
                },
                new Currency
                {
                    Id = 1719,
                    CurrencyTypeId = 2,
                    Name = "Peerplays",
                    Abbreviation = "PPY"
                },
                new Currency
                {
                    Id = 3785,
                    CurrencyTypeId = 2,
                    Name = "AIDUS TOKEN",
                    Abbreviation = "AID"
                },
                new Currency
                {
                    Id = 1154,
                    CurrencyTypeId = 2,
                    Name = "Radium",
                    Abbreviation = "RADS"
                },
                new Currency
                {
                    Id = 3233,
                    CurrencyTypeId = 2,
                    Name = "Ulord",
                    Abbreviation = "UT"
                },
                new Currency
                {
                    Id = 2641,
                    CurrencyTypeId = 2,
                    Name = "Apex",
                    Abbreviation = "CPX"
                },
                new Currency
                {
                    Id = 2402,
                    CurrencyTypeId = 2,
                    Name = "Sense",
                    Abbreviation = "SENSE"
                },
                new Currency
                {
                    Id = 77,
                    CurrencyTypeId = 2,
                    Name = "Diamond",
                    Abbreviation = "DMD"
                },
                new Currency
                {
                    Id = 3337,
                    CurrencyTypeId = 2,
                    Name = "QChi",
                    Abbreviation = "QCH"
                },
                new Currency
                {
                    Id = 2828,
                    CurrencyTypeId = 2,
                    Name = "SPINDLE",
                    Abbreviation = "SPD"
                },
                new Currency
                {
                    Id = 3015,
                    CurrencyTypeId = 2,
                    Name = "Brickblock",
                    Abbreviation = "BBK"
                },
                new Currency
                {
                    Id = 2354,
                    CurrencyTypeId = 2,
                    Name = "GET Protocol",
                    Abbreviation = "GET"
                },
                new Currency
                {
                    Id = 2645,
                    CurrencyTypeId = 2,
                    Name = "U Network",
                    Abbreviation = "UUU"
                },
                new Currency
                {
                    Id = 2017,
                    CurrencyTypeId = 2,
                    Name = "KickCoin",
                    Abbreviation = "KICK"
                },
                new Currency
                {
                    Id = 2689,
                    CurrencyTypeId = 2,
                    Name = "Rublix",
                    Abbreviation = "RBLX"
                },
                new Currency
                {
                    Id = 90,
                    CurrencyTypeId = 2,
                    Name = "Dimecoin",
                    Abbreviation = "DIME"
                },
                new Currency
                {
                    Id = 3713,
                    CurrencyTypeId = 2,
                    Name = "Wibson",
                    Abbreviation = "WIB"
                },
                new Currency
                {
                    Id = 3754,
                    CurrencyTypeId = 2,
                    Name = "EveryCoin ",
                    Abbreviation = "EVY"
                },
                new Currency
                {
                    Id = 3297,
                    CurrencyTypeId = 2,
                    Name = "Gene Source Code Chain",
                    Abbreviation = "GENE"
                },
                new Currency
                {
                    Id = 2315,
                    CurrencyTypeId = 2,
                    Name = "HTMLCOIN",
                    Abbreviation = "HTML"
                },
                new Currency
                {
                    Id = 3666,
                    CurrencyTypeId = 2,
                    Name = "Ultiledger",
                    Abbreviation = "ULT"
                },
                new Currency
                {
                    Id = 699,
                    CurrencyTypeId = 2,
                    Name = "NuShares",
                    Abbreviation = "NSR"
                },
                new Currency
                {
                    Id = 1107,
                    CurrencyTypeId = 2,
                    Name = "PACcoin",
                    Abbreviation = "$PAC"
                },
                new Currency
                {
                    Id = 3622,
                    CurrencyTypeId = 2,
                    Name = "nOS",
                    Abbreviation = "NOS"
                },
                new Currency
                {
                    Id = 2572,
                    CurrencyTypeId = 2,
                    Name = "BABB",
                    Abbreviation = "BAX"
                },
                new Currency
                {
                    Id = 3327,
                    CurrencyTypeId = 2,
                    Name = "SIX",
                    Abbreviation = "SIX"
                },
                new Currency
                {
                    Id = 3138,
                    CurrencyTypeId = 2,
                    Name = "Noku",
                    Abbreviation = "NOKU"
                },
                new Currency
                {
                    Id = 2219,
                    CurrencyTypeId = 2,
                    Name = "SpankChain",
                    Abbreviation = "SPANK"
                },
                new Currency
                {
                    Id = 3727,
                    CurrencyTypeId = 2,
                    Name = "Flowchain",
                    Abbreviation = "FLC"
                },
                new Currency
                {
                    Id = 2410,
                    CurrencyTypeId = 2,
                    Name = "SpaceChain",
                    Abbreviation = "SPC"
                },
                new Currency
                {
                    Id = 3065,
                    CurrencyTypeId = 2,
                    Name = "ICE ROCK MINING",
                    Abbreviation = "ROCK2"
                },
                new Currency
                {
                    Id = 2859,
                    CurrencyTypeId = 2,
                    Name = "XMax",
                    Abbreviation = "XMX"
                },
                new Currency
                {
                    Id = 2149,
                    CurrencyTypeId = 2,
                    Name = "Unikoin Gold",
                    Abbreviation = "UKG"
                },
                new Currency
                {
                    Id = 2972,
                    CurrencyTypeId = 2,
                    Name = "ZPER",
                    Abbreviation = "ZPR"
                },
                new Currency
                {
                    Id = 182,
                    CurrencyTypeId = 2,
                    Name = "Myriad",
                    Abbreviation = "XMY"
                },
                new Currency
                {
                    Id = 3634,
                    CurrencyTypeId = 2,
                    Name = "Kambria",
                    Abbreviation = "KAT"
                },
                new Currency
                {
                    Id = 495,
                    CurrencyTypeId = 2,
                    Name = "I/O Coin",
                    Abbreviation = "IOC"
                },
                new Currency
                {
                    Id = 1998,
                    CurrencyTypeId = 2,
                    Name = "Ormeus Coin",
                    Abbreviation = "ORME"
                },
                new Currency
                {
                    Id = 3590,
                    CurrencyTypeId = 2,
                    Name = "CrypticCoin",
                    Abbreviation = "CRYP"
                },
                new Currency
                {
                    Id = 3279,
                    CurrencyTypeId = 2,
                    Name = "Rotharium",
                    Abbreviation = "RTH"
                },
                new Currency
                {
                    Id = 2337,
                    CurrencyTypeId = 2,
                    Name = "Lamden",
                    Abbreviation = "TAU"
                },
                new Currency
                {
                    Id = 2841,
                    CurrencyTypeId = 2,
                    Name = "LoyalCoin",
                    Abbreviation = "LYL"
                },
                new Currency
                {
                    Id = 362,
                    CurrencyTypeId = 2,
                    Name = "CloakCoin",
                    Abbreviation = "CLOAK"
                },
                new Currency
                {
                    Id = 1771,
                    CurrencyTypeId = 2,
                    Name = "DAO.Casino",
                    Abbreviation = "BET"
                },
                new Currency
                {
                    Id = 3227,
                    CurrencyTypeId = 2,
                    Name = "Traceability Chain",
                    Abbreviation = "TAC"
                },
                new Currency
                {
                    Id = 2305,
                    CurrencyTypeId = 2,
                    Name = "NAGA",
                    Abbreviation = "NGC"
                },
                new Currency
                {
                    Id = 2578,
                    CurrencyTypeId = 2,
                    Name = "TE-FOOD",
                    Abbreviation = "TFD"
                },
                new Currency
                {
                    Id = 2450,
                    CurrencyTypeId = 2,
                    Name = "carVertical",
                    Abbreviation = "CV"
                },
                new Currency
                {
                    Id = 2686,
                    CurrencyTypeId = 2,
                    Name = "Lendingblock",
                    Abbreviation = "LND"
                },
                new Currency
                {
                    Id = 720,
                    CurrencyTypeId = 2,
                    Name = "Crown",
                    Abbreviation = "CRW"
                },
                new Currency
                {
                    Id = 2630,
                    CurrencyTypeId = 2,
                    Name = "PolySwarm",
                    Abbreviation = "NCT"
                },
                new Currency
                {
                    Id = 2667,
                    CurrencyTypeId = 2,
                    Name = "FintruX Network",
                    Abbreviation = "FTX"
                },
                new Currency
                {
                    Id = 3305,
                    CurrencyTypeId = 2,
                    Name = "Eden",
                    Abbreviation = "EDN"
                },
                new Currency
                {
                    Id = 3850,
                    CurrencyTypeId = 2,
                    Name = "OTOCASH",
                    Abbreviation = "OTO"
                },
                new Currency
                {
                    Id = 2387,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Atom",
                    Abbreviation = "BCA"
                },
                new Currency
                {
                    Id = 3199,
                    CurrencyTypeId = 2,
                    Name = "Cashbery Coin",
                    Abbreviation = "CBC"
                },
                new Currency
                {
                    Id = 3811,
                    CurrencyTypeId = 2,
                    Name = "IntelliShare",
                    Abbreviation = "INE"
                },
                new Currency
                {
                    Id = 122,
                    CurrencyTypeId = 2,
                    Name = "PotCoin",
                    Abbreviation = "POT"
                },
                new Currency
                {
                    Id = 3082,
                    CurrencyTypeId = 2,
                    Name = "VINchain",
                    Abbreviation = "VIN"
                },
                new Currency
                {
                    Id = 3867,
                    CurrencyTypeId = 2,
                    Name = "NeoWorld Cash",
                    Abbreviation = "NASH"
                },
                new Currency
                {
                    Id = 2340,
                    CurrencyTypeId = 2,
                    Name = "Bloom",
                    Abbreviation = "BLT"
                },
                new Currency
                {
                    Id = 2659,
                    CurrencyTypeId = 2,
                    Name = "Dignity",
                    Abbreviation = "DIG"
                },
                new Currency
                {
                    Id = 3698,
                    CurrencyTypeId = 2,
                    Name = "Observer",
                    Abbreviation = "OBSR"
                },
                new Currency
                {
                    Id = 2569,
                    CurrencyTypeId = 2,
                    Name = "CoinPoker",
                    Abbreviation = "CHP"
                },
                new Currency
                {
                    Id = 2437,
                    CurrencyTypeId = 2,
                    Name = "YEE",
                    Abbreviation = "YEE"
                },
                new Currency
                {
                    Id = 3830,
                    CurrencyTypeId = 2,
                    Name = "Veil",
                    Abbreviation = "VEIL"
                },
                new Currency
                {
                    Id = 323,
                    CurrencyTypeId = 2,
                    Name = "VeriCoin",
                    Abbreviation = "VRC"
                },
                new Currency
                {
                    Id = 2610,
                    CurrencyTypeId = 2,
                    Name = "Peculium",
                    Abbreviation = "PCL"
                },
                new Currency
                {
                    Id = 2070,
                    CurrencyTypeId = 2,
                    Name = "DomRaider",
                    Abbreviation = "DRT"
                },
                new Currency
                {
                    Id = 2758,
                    CurrencyTypeId = 2,
                    Name = "Unibright",
                    Abbreviation = "UBT"
                },
                new Currency
                {
                    Id = 833,
                    CurrencyTypeId = 2,
                    Name = "GridCoin",
                    Abbreviation = "GRC"
                },
                new Currency
                {
                    Id = 3748,
                    CurrencyTypeId = 2,
                    Name = "Hxro",
                    Abbreviation = "HXRO"
                },
                new Currency
                {
                    Id = 1950,
                    CurrencyTypeId = 2,
                    Name = "Hiveterminal Token",
                    Abbreviation = "HVN"
                },
                new Currency
                {
                    Id = 3081,
                    CurrencyTypeId = 2,
                    Name = "Omnitude",
                    Abbreviation = "ECOM"
                },
                new Currency
                {
                    Id = 3649,
                    CurrencyTypeId = 2,
                    Name = "Plus-Coin",
                    Abbreviation = "NPLC"
                },
                new Currency
                {
                    Id = 2998,
                    CurrencyTypeId = 2,
                    Name = "Vexanium",
                    Abbreviation = "VEX"
                },
                new Currency
                {
                    Id = 2060,
                    CurrencyTypeId = 2,
                    Name = "Change",
                    Abbreviation = "CAG"
                },
                new Currency
                {
                    Id = 3598,
                    CurrencyTypeId = 2,
                    Name = "Optimal Shelf Availability Token",
                    Abbreviation = "OSA"
                },
                new Currency
                {
                    Id = 1810,
                    CurrencyTypeId = 2,
                    Name = "CVCoin",
                    Abbreviation = "CVN"
                },
                new Currency
                {
                    Id = 2696,
                    CurrencyTypeId = 2,
                    Name = "DAEX",
                    Abbreviation = "DAX"
                },
                new Currency
                {
                    Id = 2725,
                    CurrencyTypeId = 2,
                    Name = "Skrumble Network",
                    Abbreviation = "SKM"
                },
                new Currency
                {
                    Id = 3768,
                    CurrencyTypeId = 2,
                    Name = "PIBBLE",
                    Abbreviation = "PIB"
                },
                new Currency
                {
                    Id = 3920,
                    CurrencyTypeId = 2,
                    Name = "Diamond Platform Token",
                    Abbreviation = "DPT"
                },
                new Currency
                {
                    Id = 2902,
                    CurrencyTypeId = 2,
                    Name = "POPCHAIN",
                    Abbreviation = "PCH"
                },
                new Currency
                {
                    Id = 2662,
                    CurrencyTypeId = 2,
                    Name = "Haven Protocol",
                    Abbreviation = "XHV"
                },
                new Currency
                {
                    Id = 1905,
                    CurrencyTypeId = 2,
                    Name = "TrueFlip",
                    Abbreviation = "TFL"
                },
                new Currency
                {
                    Id = 2827,
                    CurrencyTypeId = 2,
                    Name = "Phantasma",
                    Abbreviation = "SOUL"
                },
                new Currency
                {
                    Id = 3824,
                    CurrencyTypeId = 2,
                    Name = "Vanta Network",
                    Abbreviation = "VNT"
                },
                new Currency
                {
                    Id = 3782,
                    CurrencyTypeId = 2,
                    Name = "ONOToken",
                    Abbreviation = "ONOT"
                },
                new Currency
                {
                    Id = 3663,
                    CurrencyTypeId = 2,
                    Name = "Footballcoin",
                    Abbreviation = "XFC"
                },
                new Currency
                {
                    Id = 760,
                    CurrencyTypeId = 2,
                    Name = "OKCash",
                    Abbreviation = "OK"
                },
                new Currency
                {
                    Id = 27,
                    CurrencyTypeId = 2,
                    Name = "GoldCoin",
                    Abbreviation = "GLC"
                },
                new Currency
                {
                    Id = 920,
                    CurrencyTypeId = 2,
                    Name = "SounDAC",
                    Abbreviation = "XSD"
                },
                new Currency
                {
                    Id = 1044,
                    CurrencyTypeId = 2,
                    Name = "Global Currency Reserve",
                    Abbreviation = "GCR"
                },
                new Currency
                {
                    Id = 895,
                    CurrencyTypeId = 2,
                    Name = "Xaurum",
                    Abbreviation = "XAUR"
                },
                new Currency
                {
                    Id = 3023,
                    CurrencyTypeId = 2,
                    Name = "Semux",
                    Abbreviation = "SEM"
                },
                new Currency
                {
                    Id = 333,
                    CurrencyTypeId = 2,
                    Name = "Curecoin",
                    Abbreviation = "CURE"
                },
                new Currency
                {
                    Id = 2957,
                    CurrencyTypeId = 2,
                    Name = "Olive",
                    Abbreviation = "OLE"
                },
                new Currency
                {
                    Id = 2621,
                    CurrencyTypeId = 2,
                    Name = "Consensus",
                    Abbreviation = "SEN"
                },
                new Currency
                {
                    Id = 2309,
                    CurrencyTypeId = 2,
                    Name = "SophiaTX",
                    Abbreviation = "SPHTX"
                },
                new Currency
                {
                    Id = 3014,
                    CurrencyTypeId = 2,
                    Name = "RightMesh",
                    Abbreviation = "RMESH"
                },
                new Currency
                {
                    Id = 3581,
                    CurrencyTypeId = 2,
                    Name = "Kleros",
                    Abbreviation = "PNK"
                },
                new Currency
                {
                    Id = 2643,
                    CurrencyTypeId = 2,
                    Name = "Sentinel",
                    Abbreviation = "SENT"
                },
                new Currency
                {
                    Id = 2477,
                    CurrencyTypeId = 2,
                    Name = "Nework",
                    Abbreviation = "NKC"
                },
                new Currency
                {
                    Id = 2571,
                    CurrencyTypeId = 2,
                    Name = "Graft",
                    Abbreviation = "GRFT"
                },
                new Currency
                {
                    Id = 2913,
                    CurrencyTypeId = 2,
                    Name = "DaTa eXchange",
                    Abbreviation = "DTX"
                },
                new Currency
                {
                    Id = 2462,
                    CurrencyTypeId = 2,
                    Name = "AidCoin",
                    Abbreviation = "AID"
                },
                new Currency
                {
                    Id = 1556,
                    CurrencyTypeId = 2,
                    Name = "Chronobank",
                    Abbreviation = "TIME"
                },
                new Currency
                {
                    Id = 2497,
                    CurrencyTypeId = 2,
                    Name = "Medicalchain",
                    Abbreviation = "MTN"
                },
                new Currency
                {
                    Id = 3514,
                    CurrencyTypeId = 2,
                    Name = "SUQA",
                    Abbreviation = "SUQA"
                },
                new Currency
                {
                    Id = 1737,
                    CurrencyTypeId = 2,
                    Name = "XEL",
                    Abbreviation = "XEL"
                },
                new Currency
                {
                    Id = 2688,
                    CurrencyTypeId = 2,
                    Name = "Vipstar Coin",
                    Abbreviation = "VIPS"
                },
                new Currency
                {
                    Id = 2536,
                    CurrencyTypeId = 2,
                    Name = "Neurotoken",
                    Abbreviation = "NTK"
                },
                new Currency
                {
                    Id = 1382,
                    CurrencyTypeId = 2,
                    Name = "NoLimitCoin",
                    Abbreviation = "NLC2"
                },
                new Currency
                {
                    Id = 3177,
                    CurrencyTypeId = 2,
                    Name = "Seal Network",
                    Abbreviation = "SEAL"
                },
                new Currency
                {
                    Id = 3052,
                    CurrencyTypeId = 2,
                    Name = "Eligma Token",
                    Abbreviation = "ELI"
                },
                new Currency
                {
                    Id = 584,
                    CurrencyTypeId = 2,
                    Name = "NativeCoin",
                    Abbreviation = "N8V"
                },
                new Currency
                {
                    Id = 2620,
                    CurrencyTypeId = 2,
                    Name = "Switcheo",
                    Abbreviation = "SWTH"
                },
                new Currency
                {
                    Id = 3474,
                    CurrencyTypeId = 2,
                    Name = "YGGDRASH",
                    Abbreviation = "YEED"
                },
                new Currency
                {
                    Id = 2312,
                    CurrencyTypeId = 2,
                    Name = "DIMCOIN",
                    Abbreviation = "DIM"
                },
                new Currency
                {
                    Id = 233,
                    CurrencyTypeId = 2,
                    Name = "SolarCoin",
                    Abbreviation = "SLR"
                },
                new Currency
                {
                    Id = 1751,
                    CurrencyTypeId = 2,
                    Name = "ATC Coin",
                    Abbreviation = "ATCC"
                },
                new Currency
                {
                    Id = 132,
                    CurrencyTypeId = 2,
                    Name = "Counterparty",
                    Abbreviation = "XCP"
                },
                new Currency
                {
                    Id = 2363,
                    CurrencyTypeId = 2,
                    Name = "Zap",
                    Abbreviation = "ZAP"
                },
                new Currency
                {
                    Id = 3712,
                    CurrencyTypeId = 2,
                    Name = "Cloudbric",
                    Abbreviation = "CLB"
                },
                new Currency
                {
                    Id = 1755,
                    CurrencyTypeId = 2,
                    Name = "Flash",
                    Abbreviation = "FLASH"
                },
                new Currency
                {
                    Id = 2597,
                    CurrencyTypeId = 2,
                    Name = "UpToken",
                    Abbreviation = "UP"
                },
                new Currency
                {
                    Id = 706,
                    CurrencyTypeId = 2,
                    Name = "MonetaryUnit",
                    Abbreviation = "MUE"
                },
                new Currency
                {
                    Id = 2484,
                    CurrencyTypeId = 2,
                    Name = "Hi Mutual Society",
                    Abbreviation = "HMC"
                },
                new Currency
                {
                    Id = 2047,
                    CurrencyTypeId = 2,
                    Name = "Zeusshield",
                    Abbreviation = "ZSC"
                },
                new Currency
                {
                    Id = 2714,
                    CurrencyTypeId = 2,
                    Name = "Nexty",
                    Abbreviation = "NTY"
                },
                new Currency
                {
                    Id = 3729,
                    CurrencyTypeId = 2,
                    Name = "WOLLO",
                    Abbreviation = "WLO"
                },
                new Currency
                {
                    Id = 2762,
                    CurrencyTypeId = 2,
                    Name = "Open Platform",
                    Abbreviation = "OPEN"
                },
                new Currency
                {
                    Id = 2184,
                    CurrencyTypeId = 2,
                    Name = "Privatix",
                    Abbreviation = "PRIX"
                },
                new Currency
                {
                    Id = 3243,
                    CurrencyTypeId = 2,
                    Name = "Moneytoken",
                    Abbreviation = "IMT"
                },
                new Currency
                {
                    Id = 2908,
                    CurrencyTypeId = 2,
                    Name = "HashCoin",
                    Abbreviation = "HSC"
                },
                new Currency
                {
                    Id = 1281,
                    CurrencyTypeId = 2,
                    Name = "ION",
                    Abbreviation = "ION"
                },
                new Currency
                {
                    Id = 2078,
                    CurrencyTypeId = 2,
                    Name = "LIFE",
                    Abbreviation = "LIFE"
                },
                new Currency
                {
                    Id = 3636,
                    CurrencyTypeId = 2,
                    Name = "Lisk Machine Learning",
                    Abbreviation = "LML"
                },
                new Currency
                {
                    Id = 2493,
                    CurrencyTypeId = 2,
                    Name = "STK",
                    Abbreviation = "STK"
                },
                new Currency
                {
                    Id = 1990,
                    CurrencyTypeId = 2,
                    Name = "BitDice",
                    Abbreviation = "CSNO"
                },
                new Currency
                {
                    Id = 3432,
                    CurrencyTypeId = 2,
                    Name = "Rapids",
                    Abbreviation = "RPD"
                },
                new Currency
                {
                    Id = 2723,
                    CurrencyTypeId = 2,
                    Name = "FuzeX",
                    Abbreviation = "FXT"
                },
                new Currency
                {
                    Id = 1208,
                    CurrencyTypeId = 2,
                    Name = "RevolutionVR",
                    Abbreviation = "RVR"
                },
                new Currency
                {
                    Id = 2357,
                    CurrencyTypeId = 2,
                    Name = "AI Doctor",
                    Abbreviation = "AIDOC"
                },
                new Currency
                {
                    Id = 3314,
                    CurrencyTypeId = 2,
                    Name = "Blockparty (BOXX Token)",
                    Abbreviation = "BOXX"
                },
                new Currency
                {
                    Id = 2728,
                    CurrencyTypeId = 2,
                    Name = "Winding Tree",
                    Abbreviation = "LIF"
                },
                new Currency
                {
                    Id = 2856,
                    CurrencyTypeId = 2,
                    Name = "CEEK VR",
                    Abbreviation = "CEEK"
                },
                new Currency
                {
                    Id = 3833,
                    CurrencyTypeId = 2,
                    Name = "HYPNOXYS",
                    Abbreviation = "HYPX"
                },
                new Currency
                {
                    Id = 2438,
                    CurrencyTypeId = 2,
                    Name = "Acute Angle Cloud",
                    Abbreviation = "AAC"
                },
                new Currency
                {
                    Id = 2176,
                    CurrencyTypeId = 2,
                    Name = "Decision Token",
                    Abbreviation = "HST"
                },
                new Currency
                {
                    Id = 2389,
                    CurrencyTypeId = 2,
                    Name = "ugChain",
                    Abbreviation = "UGC"
                },
                new Currency
                {
                    Id = 2587,
                    CurrencyTypeId = 2,
                    Name = "Fluz Fluz",
                    Abbreviation = "FLUZ"
                },
                new Currency
                {
                    Id = 2283,
                    CurrencyTypeId = 2,
                    Name = "Datum",
                    Abbreviation = "DAT"
                },
                new Currency
                {
                    Id = 3757,
                    CurrencyTypeId = 2,
                    Name = "GMB",
                    Abbreviation = "GMB"
                },
                new Currency
                {
                    Id = 3520,
                    CurrencyTypeId = 2,
                    Name = "VisionX",
                    Abbreviation = "VNX"
                },
                new Currency
                {
                    Id = 3389,
                    CurrencyTypeId = 2,
                    Name = "Tolar",
                    Abbreviation = "TOL"
                },
                new Currency
                {
                    Id = 3315,
                    CurrencyTypeId = 2,
                    Name = "Playgroundz",
                    Abbreviation = "IOG"
                },
                new Currency
                {
                    Id = 1367,
                    CurrencyTypeId = 2,
                    Name = "Experience Points",
                    Abbreviation = "XP"
                },
                new Currency
                {
                    Id = 2880,
                    CurrencyTypeId = 2,
                    Name = "Rate3",
                    Abbreviation = "RTE"
                },
                new Currency
                {
                    Id = 3438,
                    CurrencyTypeId = 2,
                    Name = "Oxycoin",
                    Abbreviation = "OXY"
                },
                new Currency
                {
                    Id = 3022,
                    CurrencyTypeId = 2,
                    Name = "ZMINE",
                    Abbreviation = "ZMN"
                },
                new Currency
                {
                    Id = 2527,
                    CurrencyTypeId = 2,
                    Name = "SureRemit",
                    Abbreviation = "RMT"
                },
                new Currency
                {
                    Id = 3435,
                    CurrencyTypeId = 2,
                    Name = "Snetwork",
                    Abbreviation = "SNET"
                },
                new Currency
                {
                    Id = 2595,
                    CurrencyTypeId = 2,
                    Name = "NANJCOIN",
                    Abbreviation = "NANJ"
                },
                new Currency
                {
                    Id = 3711,
                    CurrencyTypeId = 2,
                    Name = "Plair",
                    Abbreviation = "PLA"
                },
                new Currency
                {
                    Id = 2382,
                    CurrencyTypeId = 2,
                    Name = "Spectre.ai Utility Token",
                    Abbreviation = "SXUT"
                },
                new Currency
                {
                    Id = 2022,
                    CurrencyTypeId = 2,
                    Name = "Internxt",
                    Abbreviation = "INXT"
                },
                new Currency
                {
                    Id = 2579,
                    CurrencyTypeId = 2,
                    Name = "ShipChain",
                    Abbreviation = "SHIP"
                },
                new Currency
                {
                    Id = 3084,
                    CurrencyTypeId = 2,
                    Name = "Blocktrade Token",
                    Abbreviation = "BTT"
                },
                new Currency
                {
                    Id = 2773,
                    CurrencyTypeId = 2,
                    Name = "GINcoin",
                    Abbreviation = "GIN"
                },
                new Currency
                {
                    Id = 36,
                    CurrencyTypeId = 2,
                    Name = "Novacoin",
                    Abbreviation = "NVC"
                },
                new Currency
                {
                    Id = 2248,
                    CurrencyTypeId = 2,
                    Name = "Cappasity",
                    Abbreviation = "CAPP"
                },
                new Currency
                {
                    Id = 1587,
                    CurrencyTypeId = 2,
                    Name = "Dynamic",
                    Abbreviation = "DYN"
                },
                new Currency
                {
                    Id = 3816,
                    CurrencyTypeId = 2,
                    Name = "Verasity",
                    Abbreviation = "VRA"
                },
                new Currency
                {
                    Id = 3703,
                    CurrencyTypeId = 2,
                    Name = "ADAMANT Messenger",
                    Abbreviation = "ADM"
                },
                new Currency
                {
                    Id = 2107,
                    CurrencyTypeId = 2,
                    Name = "LUXCoin",
                    Abbreviation = "LUX"
                },
                new Currency
                {
                    Id = 2510,
                    CurrencyTypeId = 2,
                    Name = "Datawallet",
                    Abbreviation = "DXT"
                },
                new Currency
                {
                    Id = 2964,
                    CurrencyTypeId = 2,
                    Name = "ValueCyberToken",
                    Abbreviation = "VCT"
                },
                new Currency
                {
                    Id = 1070,
                    CurrencyTypeId = 2,
                    Name = "Expanse",
                    Abbreviation = "EXP"
                },
                new Currency
                {
                    Id = 3058,
                    CurrencyTypeId = 2,
                    Name = "eSDChain",
                    Abbreviation = "SDA"
                },
                new Currency
                {
                    Id = 3735,
                    CurrencyTypeId = 2,
                    Name = "VegaWallet Token",
                    Abbreviation = "VGW"
                },
                new Currency
                {
                    Id = 3845,
                    CurrencyTypeId = 2,
                    Name = "V-ID",
                    Abbreviation = "VIDT"
                },
                new Currency
                {
                    Id = 1577,
                    CurrencyTypeId = 2,
                    Name = "Musicoin",
                    Abbreviation = "MUSIC"
                },
                new Currency
                {
                    Id = 3884,
                    CurrencyTypeId = 2,
                    Name = "Function X",
                    Abbreviation = "FX"
                },
                new Currency
                {
                    Id = 2466,
                    CurrencyTypeId = 2,
                    Name = "aXpire",
                    Abbreviation = "AXPR"
                },
                new Currency
                {
                    Id = 2868,
                    CurrencyTypeId = 2,
                    Name = "Constellation",
                    Abbreviation = "DAG"
                },
                new Currency
                {
                    Id = 2634,
                    CurrencyTypeId = 2,
                    Name = "XinFin Network",
                    Abbreviation = "XDCE"
                },
                new Currency
                {
                    Id = 1669,
                    CurrencyTypeId = 2,
                    Name = "Humaniq",
                    Abbreviation = "HMQ"
                },
                new Currency
                {
                    Id = 2764,
                    CurrencyTypeId = 2,
                    Name = "Silent Notary",
                    Abbreviation = "SNTR"
                },
                new Currency
                {
                    Id = 2891,
                    CurrencyTypeId = 2,
                    Name = "Cardstack",
                    Abbreviation = "CARD"
                },
                new Currency
                {
                    Id = 2718,
                    CurrencyTypeId = 2,
                    Name = "PAL Network",
                    Abbreviation = "PAL"
                },
                new Currency
                {
                    Id = 2869,
                    CurrencyTypeId = 2,
                    Name = "Merculet",
                    Abbreviation = "MVP"
                },
                new Currency
                {
                    Id = 3388,
                    CurrencyTypeId = 2,
                    Name = "FREE Coin",
                    Abbreviation = "FREE"
                }, new Currency
                {
                    Id = 2443,
                    CurrencyTypeId = 2,
                    Name = "Trinity Network Credit",
                    Abbreviation = "TNC"
                }, new Currency
                {
                    Id = 3016,
                    CurrencyTypeId = 2,
                    Name = "NeuroChain",
                    Abbreviation = "NCC"
                }, new Currency
                {
                    Id = 2692,
                    CurrencyTypeId = 2,
                    Name = "Nebula AI",
                    Abbreviation = "NBAI"
                }, new Currency
                {
                    Id = 3466,
                    CurrencyTypeId = 2,
                    Name = "Insureum",
                    Abbreviation = "ISR"
                }, new Currency
                {
                    Id = 3352,
                    CurrencyTypeId = 2,
                    Name = "MidasProtocol",
                    Abbreviation = "MAS"
                }, new Currency
                {
                    Id = 2626,
                    CurrencyTypeId = 2,
                    Name = "Friendz",
                    Abbreviation = "FDZ"
                }, new Currency
                {
                    Id = 2390,
                    CurrencyTypeId = 2,
                    Name = "BANKEX",
                    Abbreviation = "BKX"
                }, new Currency
                {
                    Id = 3280,
                    CurrencyTypeId = 2,
                    Name = "RealTract",
                    Abbreviation = "RET"
                }, new Currency
                {
                    Id = 870,
                    CurrencyTypeId = 2,
                    Name = "Pura",
                    Abbreviation = "PURA"
                }, new Currency
                {
                    Id = 2040,
                    CurrencyTypeId = 2,
                    Name = "ALIS",
                    Abbreviation = "ALIS"
                }, new Currency
                {
                    Id = 2439,
                    CurrencyTypeId = 2,
                    Name = "SelfSell",
                    Abbreviation = "SSC"
                }, new Currency
                {
                    Id = 3499,
                    CurrencyTypeId = 2,
                    Name = "Liquidity Network",
                    Abbreviation = "LQD"
                }, new Currency
                {
                    Id = 3750,
                    CurrencyTypeId = 2,
                    Name = "eXPerience Chain",
                    Abbreviation = "XPC"
                }, new Currency
                {
                    Id = 2391,
                    CurrencyTypeId = 2,
                    Name = "EchoLink",
                    Abbreviation = "EKO"
                }, new Currency
                {
                    Id = 2575,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Private",
                    Abbreviation = "BTCP"
                }, new Currency
                {
                    Id = 2314,
                    CurrencyTypeId = 2,
                    Name = "Cryptopay",
                    Abbreviation = "CPAY"
                }, new Currency
                {
                    Id = 3140,
                    CurrencyTypeId = 2,
                    Name = "Ubex",
                    Abbreviation = "UBEX"
                }, new Currency
                {
                    Id = 1638,
                    CurrencyTypeId = 2,
                    Name = "WeTrust",
                    Abbreviation = "TRST"
                }, new Currency
                {
                    Id = 2088,
                    CurrencyTypeId = 2,
                    Name = "EXRNchain",
                    Abbreviation = "EXRN"
                }, new Currency
                {
                    Id = 2702,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Interest",
                    Abbreviation = "BCI"
                }, new Currency
                {
                    Id = 366,
                    CurrencyTypeId = 2,
                    Name = "BitSend",
                    Abbreviation = "BSD"
                }, new Currency
                {
                    Id = 3055,
                    CurrencyTypeId = 2,
                    Name = "EBCoin",
                    Abbreviation = "EBC"
                }, new Currency
                {
                    Id = 2191,
                    CurrencyTypeId = 2,
                    Name = "Paypex",
                    Abbreviation = "PAYX"
                }, new Currency
                {
                    Id = 3616,
                    CurrencyTypeId = 2,
                    Name = "Blockchain Certified Data Token",
                    Abbreviation = "BCDT"
                }, new Currency
                {
                    Id = 3210,
                    CurrencyTypeId = 2,
                    Name = "MIB Coin",
                    Abbreviation = "MIB"
                }, new Currency
                {
                    Id = 2343,
                    CurrencyTypeId = 2,
                    Name = "CanYaCoin",
                    Abbreviation = "CAN"
                }, new Currency
                {
                    Id = 3870,
                    CurrencyTypeId = 2,
                    Name = "Lition",
                    Abbreviation = "LIT"
                }, new Currency
                {
                    Id = 2573,
                    CurrencyTypeId = 2,
                    Name = "Electrify.Asia",
                    Abbreviation = "ELEC"
                }, new Currency
                {
                    Id = 3762,
                    CurrencyTypeId = 2,
                    Name = "1SG",
                    Abbreviation = "1SG"
                }, new Currency
                {
                    Id = 3404,
                    CurrencyTypeId = 2,
                    Name = "Wixlar",
                    Abbreviation = "WIX"
                }, new Currency
                {
                    Id = 1616,
                    CurrencyTypeId = 2,
                    Name = "Matchpool",
                    Abbreviation = "GUP"
                }, new Currency
                {
                    Id = 2558,
                    CurrencyTypeId = 2,
                    Name = "Insights Network",
                    Abbreviation = "INSTAR"
                }, new Currency
                {
                    Id = 1125,
                    CurrencyTypeId = 2,
                    Name = "HyperSpace",
                    Abbreviation = "AMP"
                }, new Currency
                {
                    Id = 184,
                    CurrencyTypeId = 2,
                    Name = "DNotes",
                    Abbreviation = "NOTE"
                }, new Currency
                {
                    Id = 1838,
                    CurrencyTypeId = 2,
                    Name = "OracleChain",
                    Abbreviation = "OCT"
                }, new Currency
                {
                    Id = 2178,
                    CurrencyTypeId = 2,
                    Name = "Upfiring",
                    Abbreviation = "UFR"
                }, new Currency
                {
                    Id = 3419,
                    CurrencyTypeId = 2,
                    Name = "Quasarcoin",
                    Abbreviation = "QAC"
                }, new Currency
                {
                    Id = 2855,
                    CurrencyTypeId = 2,
                    Name = "CashBet Coin",
                    Abbreviation = "CBC"
                }, new Currency
                {
                    Id = 3357,
                    CurrencyTypeId = 2,
                    Name = "Digital Asset Guarantee Token",
                    Abbreviation = "DAGT"
                }, new Currency
                {
                    Id = 3638,
                    CurrencyTypeId = 2,
                    Name = "Skychain",
                    Abbreviation = "SKCH"
                }, new Currency
                {
                    Id = 2699,
                    CurrencyTypeId = 2,
                    Name = "Sharder",
                    Abbreviation = "SS"
                }, new Currency
                {
                    Id = 2459,
                    CurrencyTypeId = 2,
                    Name = "indaHash",
                    Abbreviation = "IDH"
                }, new Currency
                {
                    Id = 2657,
                    CurrencyTypeId = 2,
                    Name = "BrahmaOS",
                    Abbreviation = "BRM"
                }, new Currency
                {
                    Id = 1032,
                    CurrencyTypeId = 2,
                    Name = "TransferCoin",
                    Abbreviation = "TX"
                }, new Currency
                {
                    Id = 2541,
                    CurrencyTypeId = 2,
                    Name = "Storiqa",
                    Abbreviation = "STQ"
                }, new Currency
                {
                    Id = 3651,
                    CurrencyTypeId = 2,
                    Name = "Next.exchange",
                    Abbreviation = "NEXT"
                }, new Currency
                {
                    Id = 3658,
                    CurrencyTypeId = 2,
                    Name = "Fountain",
                    Abbreviation = "FTN"
                }, new Currency
                {
                    Id = 3917,
                    CurrencyTypeId = 2,
                    Name = "Sentivate",
                    Abbreviation = "SNTVT"
                }, new Currency
                {
                    Id = 2242,
                    CurrencyTypeId = 2,
                    Name = "Qbao",
                    Abbreviation = "QBT"
                }, new Currency
                {
                    Id = 2927,
                    CurrencyTypeId = 2,
                    Name = "sUSD",
                    Abbreviation = "SUSD"
                }, new Currency
                {
                    Id = 2275,
                    CurrencyTypeId = 2,
                    Name = "ProChain",
                    Abbreviation = "PRA"
                }, new Currency
                {
                    Id = 2562,
                    CurrencyTypeId = 2,
                    Name = "Education Ecosystem",
                    Abbreviation = "LEDU"
                }, new Currency
                {
                    Id = 3775,
                    CurrencyTypeId = 2,
                    Name = "win.win",
                    Abbreviation = "TWINS"
                }, new Currency
                {
                    Id = 1063,
                    CurrencyTypeId = 2,
                    Name = "BitCrystals",
                    Abbreviation = "BCY"
                }, new Currency
                {
                    Id = 2136,
                    CurrencyTypeId = 2,
                    Name = "ATLANT",
                    Abbreviation = "ATL"
                }, new Currency
                {
                    Id = 823,
                    CurrencyTypeId = 2,
                    Name = "GeoCoin",
                    Abbreviation = "GEO"
                }, new Currency
                {
                    Id = 3691,
                    CurrencyTypeId = 2,
                    Name = "Kuai Token",
                    Abbreviation = "KT"
                }, new Currency
                {
                    Id = 3120,
                    CurrencyTypeId = 2,
                    Name = "OWNDATA",
                    Abbreviation = "OWN"
                }, new Currency
                {
                    Id = 2537,
                    CurrencyTypeId = 2,
                    Name = "Gems ",
                    Abbreviation = "GEM"
                }, new Currency
                {
                    Id = 2105,
                    CurrencyTypeId = 2,
                    Name = "Pirl",
                    Abbreviation = "PIRL"
                }, new Currency
                {
                    Id = 1999,
                    CurrencyTypeId = 2,
                    Name = "Kolion",
                    Abbreviation = "KLN"
                }, new Currency
                {
                    Id = 2512,
                    CurrencyTypeId = 2,
                    Name = "UNIVERSAL CASH",
                    Abbreviation = "UCASH"
                }, new Currency
                {
                    Id = 2629,
                    CurrencyTypeId = 2,
                    Name = "Torque",
                    Abbreviation = "XTC"
                }, new Currency
                {
                    Id = 1294,
                    CurrencyTypeId = 2,
                    Name = "Rise",
                    Abbreviation = "RISE"
                }, new Currency
                {
                    Id = 3168,
                    CurrencyTypeId = 2,
                    Name = "FarmaTrust",
                    Abbreviation = "FTT"
                }, new Currency
                {
                    Id = 3096,
                    CurrencyTypeId = 2,
                    Name = "Pundi X NEM",
                    Abbreviation = "NPXSXEM"
                }, new Currency
                {
                    Id = 1861,
                    CurrencyTypeId = 2,
                    Name = "Stox",
                    Abbreviation = "STX"
                }, new Currency
                {
                    Id = 2325,
                    CurrencyTypeId = 2,
                    Name = "Matryx",
                    Abbreviation = "MTX"
                }, new Currency
                {
                    Id = 1739,
                    CurrencyTypeId = 2,
                    Name = "Miners' Reward Token",
                    Abbreviation = "MRT"
                }, new Currency
                {
                    Id = 3821,
                    CurrencyTypeId = 2,
                    Name = "Qredit",
                    Abbreviation = "XQR"
                }, new Currency
                {
                    Id = 2231,
                    CurrencyTypeId = 2,
                    Name = "Flixxo",
                    Abbreviation = "FLIXX"
                }, new Currency
                {
                    Id = 3334,
                    CurrencyTypeId = 2,
                    Name = "X-CASH",
                    Abbreviation = "XCASH"
                }, new Currency
                {
                    Id = 1082,
                    CurrencyTypeId = 2,
                    Name = "SIBCoin",
                    Abbreviation = "SIB"
                }, new Currency
                {
                    Id = 3679,
                    CurrencyTypeId = 2,
                    Name = "SnapCoin",
                    Abbreviation = "SNPC"
                }, new Currency
                {
                    Id = 2310,
                    CurrencyTypeId = 2,
                    Name = "Bounty0x",
                    Abbreviation = "BNTY"
                }, new Currency
                {
                    Id = 2360,
                    CurrencyTypeId = 2,
                    Name = "Hacken",
                    Abbreviation = "HKN"
                }, new Currency
                {
                    Id = 2215,
                    CurrencyTypeId = 2,
                    Name = "Energo",
                    Abbreviation = "TSL"
                }, new Currency
                {
                    Id = 1562,
                    CurrencyTypeId = 2,
                    Name = "Swarm City",
                    Abbreviation = "SWT"
                }, new Currency
                {
                    Id = 1371,
                    CurrencyTypeId = 2,
                    Name = "B3Coin",
                    Abbreviation = "KB3"
                }, new Currency
                {
                    Id = 83,
                    CurrencyTypeId = 2,
                    Name = "Omni",
                    Abbreviation = "OMNI"
                }, new Currency
                {
                    Id = 3854,
                    CurrencyTypeId = 2,
                    Name = "Unification",
                    Abbreviation = "UND"
                }, new Currency
                {
                    Id = 3010,
                    CurrencyTypeId = 2,
                    Name = "Coinsuper Ecosystem Network",
                    Abbreviation = "CEN"
                }, new Currency
                {
                    Id = 2525,
                    CurrencyTypeId = 2,
                    Name = "Alphacat",
                    Abbreviation = "ACAT"
                }, new Currency
                {
                    Id = 3738,
                    CurrencyTypeId = 2,
                    Name = "Decentralized Crypto Token",
                    Abbreviation = "DCTO"
                }, new Currency
                {
                    Id = 2949,
                    CurrencyTypeId = 2,
                    Name = "Kryll",
                    Abbreviation = "KRL"
                }, new Currency
                {
                    Id = 1708,
                    CurrencyTypeId = 2,
                    Name = "Patientory",
                    Abbreviation = "PTOY"
                }, new Currency
                {
                    Id = 3506,
                    CurrencyTypeId = 2,
                    Name = "IONChain",
                    Abbreviation = "IONC"
                }, new Currency
                {
                    Id = 2921,
                    CurrencyTypeId = 2,
                    Name = "OneLedger",
                    Abbreviation = "OLT"
                }, new Currency
                {
                    Id = 3195,
                    CurrencyTypeId = 2,
                    Name = "Credit Tag Chain",
                    Abbreviation = "CTC"
                }, new Currency
                {
                    Id = 2478,
                    CurrencyTypeId = 2,
                    Name = "CoinFi",
                    Abbreviation = "COFI"
                }, new Currency
                {
                    Id = 2500,
                    CurrencyTypeId = 2,
                    Name = "Zilla",
                    Abbreviation = "ZLA"
                }, new Currency
                {
                    Id = 1464,
                    CurrencyTypeId = 2,
                    Name = "Internet of People",
                    Abbreviation = "IOP"
                }, new Currency
                {
                    Id = 2567,
                    CurrencyTypeId = 2,
                    Name = "DATx",
                    Abbreviation = "DATX"
                }, new Currency
                {
                    Id = 3104,
                    CurrencyTypeId = 2,
                    Name = "Giant",
                    Abbreviation = "GIC"
                }, new Currency
                {
                    Id = 2564,
                    CurrencyTypeId = 2,
                    Name = "HOQU",
                    Abbreviation = "HQX"
                }, new Currency
                {
                    Id = 2110,
                    CurrencyTypeId = 2,
                    Name = "Dovu",
                    Abbreviation = "DOV"
                }, new Currency
                {
                    Id = 1948,
                    CurrencyTypeId = 2,
                    Name = "Aventus",
                    Abbreviation = "AVT"
                }, new Currency
                {
                    Id = 2979,
                    CurrencyTypeId = 2,
                    Name = "Linfinity",
                    Abbreviation = "LFC"
                }, new Currency
                {
                    Id = 2076,
                    CurrencyTypeId = 2,
                    Name = "Blue Protocol",
                    Abbreviation = "BLUE"
                }, new Currency
                {
                    Id = 2549,
                    CurrencyTypeId = 2,
                    Name = "Ink Protocol",
                    Abbreviation = "XNK"
                }, new Currency
                {
                    Id = 2970,
                    CurrencyTypeId = 2,
                    Name = "LocalCoinSwap",
                    Abbreviation = "LCS"
                }, new Currency
                {
                    Id = 2771,
                    CurrencyTypeId = 2,
                    Name = "RED",
                    Abbreviation = "RED"
                }, new Currency
                {
                    Id = 2844,
                    CurrencyTypeId = 2,
                    Name = "Shivom",
                    Abbreviation = "OMX"
                }, new Currency
                {
                    Id = 2592,
                    CurrencyTypeId = 2,
                    Name = "Banca",
                    Abbreviation = "BANCA"
                }, new Currency
                {
                    Id = 2850,
                    CurrencyTypeId = 2,
                    Name = "TRAXIA",
                    Abbreviation = "TMT"
                }, new Currency
                {
                    Id = 1380,
                    CurrencyTypeId = 2,
                    Name = "LoMoCoin",
                    Abbreviation = "LMC"
                }, new Currency
                {
                    Id = 2490,
                    CurrencyTypeId = 2,
                    Name = "CargoX",
                    Abbreviation = "CXO"
                }, new Currency
                {
                    Id = 3079,
                    CurrencyTypeId = 2,
                    Name = "X8X Token",
                    Abbreviation = "X8X"
                }, new Currency
                {
                    Id = 3639,
                    CurrencyTypeId = 2,
                    Name = "PlayGame",
                    Abbreviation = "PXG"
                }, new Currency
                {
                    Id = 3862,
                    CurrencyTypeId = 2,
                    Name = "Scopuly",
                    Abbreviation = "SKY"
                }, new Currency
                {
                    Id = 416,
                    CurrencyTypeId = 2,
                    Name = "HempCoin",
                    Abbreviation = "THC"
                }, new Currency
                {
                    Id = 3859,
                    CurrencyTypeId = 2,
                    Name = "Paytomat",
                    Abbreviation = "PTI"
                }, new Currency
                {
                    Id = 2742,
                    CurrencyTypeId = 2,
                    Name = "Sakura Bloom",
                    Abbreviation = "SKB"
                }, new Currency
                {
                    Id = 217,
                    CurrencyTypeId = 2,
                    Name = "Bela",
                    Abbreviation = "BELA"
                }, new Currency
                {
                    Id = 2893,
                    CurrencyTypeId = 2,
                    Name = "On.Live",
                    Abbreviation = "ONL"
                }, new Currency
                {
                    Id = 2426,
                    CurrencyTypeId = 2,
                    Name = "ShareX",
                    Abbreviation = "SEXC"
                }, new Currency
                {
                    Id = 3194,
                    CurrencyTypeId = 2,
                    Name = "DPRating",
                    Abbreviation = "RATING"
                }, new Currency
                {
                    Id = 1392,
                    CurrencyTypeId = 2,
                    Name = "Pluton",
                    Abbreviation = "PLU"
                }, new Currency
                {
                    Id = 2624,
                    CurrencyTypeId = 2,
                    Name = "Sentinel Chain",
                    Abbreviation = "SENC"
                }, new Currency
                {
                    Id = 2479,
                    CurrencyTypeId = 2,
                    Name = "Equal",
                    Abbreviation = "EQL"
                }, new Currency
                {
                    Id = 3625,
                    CurrencyTypeId = 2,
                    Name = "QuadrantProtocol",
                    Abbreviation = "EQUAD"
                }, new Currency
                {
                    Id = 2837,
                    CurrencyTypeId = 2,
                    Name = "0xBitcoin",
                    Abbreviation = "0xBTC"
                }, new Currency
                {
                    Id = 2906,
                    CurrencyTypeId = 2,
                    Name = "Essentia",
                    Abbreviation = "ESS"
                }, new Currency
                {
                    Id = 3215,
                    CurrencyTypeId = 2,
                    Name = "Gentarium",
                    Abbreviation = "GTM"
                }, new Currency
                {
                    Id = 374,
                    CurrencyTypeId = 2,
                    Name = "ArtByte",
                    Abbreviation = "ABY"
                }, new Currency
                {
                    Id = 2865,
                    CurrencyTypeId = 2,
                    Name = "Trittium",
                    Abbreviation = "TRTT"
                }, new Currency
                {
                    Id = 3786,
                    CurrencyTypeId = 2,
                    Name = "Lunes",
                    Abbreviation = "LUNES"
                }, new Currency
                {
                    Id = 1628,
                    CurrencyTypeId = 2,
                    Name = "Happycoin",
                    Abbreviation = "HPC"
                }, new Currency
                {
                    Id = 3669,
                    CurrencyTypeId = 2,
                    Name = "Winco",
                    Abbreviation = "WCO"
                }, new Currency
                {
                    Id = 2139,
                    CurrencyTypeId = 2,
                    Name = "MinexCoin",
                    Abbreviation = "MNX"
                }, new Currency
                {
                    Id = 606,
                    CurrencyTypeId = 2,
                    Name = "FoldingCoin",
                    Abbreviation = "FLDC"
                }, new Currency
                {
                    Id = 2151,
                    CurrencyTypeId = 2,
                    Name = "Autonio",
                    Abbreviation = "NIO"
                }, new Currency
                {
                    Id = 1606,
                    CurrencyTypeId = 2,
                    Name = "Solaris",
                    Abbreviation = "XLR"
                }, new Currency
                {
                    Id = 626,
                    CurrencyTypeId = 2,
                    Name = "NuBits",
                    Abbreviation = "USNBT"
                }, new Currency
                {
                    Id = 2144,
                    CurrencyTypeId = 2,
                    Name = "SHIELD",
                    Abbreviation = "XSH"
                }, new Currency
                {
                    Id = 87,
                    CurrencyTypeId = 2,
                    Name = "FedoraCoin",
                    Abbreviation = "TIPS"
                }, new Currency
                {
                    Id = 3095,
                    CurrencyTypeId = 2,
                    Name = "Niobium Coin",
                    Abbreviation = "NBC"
                }, new Currency
                {
                    Id = 2551,
                    CurrencyTypeId = 2,
                    Name = "Bezop",
                    Abbreviation = "BEZ"
                }, new Currency
                {
                    Id = 1106,
                    CurrencyTypeId = 2,
                    Name = "StrongHands",
                    Abbreviation = "SHND"
                }, new Currency
                {
                    Id = 3760,
                    CurrencyTypeId = 2,
                    Name = "Scanetchain",
                    Abbreviation = "SWC"
                }, new Currency
                {
                    Id = 2041,
                    CurrencyTypeId = 2,
                    Name = "BitcoinZ",
                    Abbreviation = "BTCZ"
                }, new Currency
                {
                    Id = 2516,
                    CurrencyTypeId = 2,
                    Name = "MktCoin",
                    Abbreviation = "MLM"
                }, new Currency
                {
                    Id = 3779,
                    CurrencyTypeId = 2,
                    Name = "CoTrader",
                    Abbreviation = "COT"
                }, new Currency
                {
                    Id = 3809,
                    CurrencyTypeId = 2,
                    Name = "DOS Network",
                    Abbreviation = "DOS"
                }, new Currency
                {
                    Id = 2936,
                    CurrencyTypeId = 2,
                    Name = "MTC Mesh Network",
                    Abbreviation = "MTC"
                }, new Currency
                {
                    Id = 2929,
                    CurrencyTypeId = 2,
                    Name = "Truegame",
                    Abbreviation = "TGAME"
                }, new Currency
                {
                    Id = 2501,
                    CurrencyTypeId = 2,
                    Name = "adbank",
                    Abbreviation = "ADB"
                }, new Currency
                {
                    Id = 2249,
                    CurrencyTypeId = 2,
                    Name = "Eroscoin",
                    Abbreviation = "ERO"
                }, new Currency
                {
                    Id = 2407,
                    CurrencyTypeId = 2,
                    Name = "AICHAIN",
                    Abbreviation = "AIT"
                }, new Currency
                {
                    Id = 3340,
                    CurrencyTypeId = 2,
                    Name = "Mallcoin",
                    Abbreviation = "MLC"
                }, new Currency
                {
                    Id = 2279,
                    CurrencyTypeId = 2,
                    Name = "Playkey",
                    Abbreviation = "PKT"
                }, new Currency
                {
                    Id = 2731,
                    CurrencyTypeId = 2,
                    Name = "Utrum",
                    Abbreviation = "OOT"
                }, new Currency
                {
                    Id = 2707,
                    CurrencyTypeId = 2,
                    Name = "FLIP",
                    Abbreviation = "FLP"
                }, new Currency
                {
                    Id = 2898,
                    CurrencyTypeId = 2,
                    Name = "GoNetwork",
                    Abbreviation = "GOT"
                }, new Currency
                {
                    Id = 2006,
                    CurrencyTypeId = 2,
                    Name = "Cobinhood",
                    Abbreviation = "COB"
                }, new Currency
                {
                    Id = 2678,
                    CurrencyTypeId = 2,
                    Name = "TraDove B2BCoin",
                    Abbreviation = "BBC"
                }, new Currency
                {
                    Id = 2674,
                    CurrencyTypeId = 2,
                    Name = "Masari",
                    Abbreviation = "MSR"
                }, new Currency
                {
                    Id = 3234,
                    CurrencyTypeId = 2,
                    Name = "Xriba",
                    Abbreviation = "XRA"
                }, new Currency
                {
                    Id = 2708,
                    CurrencyTypeId = 2,
                    Name = "Crowd Machine",
                    Abbreviation = "CMCT"
                }, new Currency
                {
                    Id = 2240,
                    CurrencyTypeId = 2,
                    Name = "SoMee.Social",
                    Abbreviation = "ONG"
                }, new Currency
                {
                    Id = 3894,
                    CurrencyTypeId = 2,
                    Name = "Crypto Sports",
                    Abbreviation = "CSPN"
                }, new Currency
                {
                    Id = 2185,
                    CurrencyTypeId = 2,
                    Name = "Lethean",
                    Abbreviation = "LTHN"
                }, new Currency
                {
                    Id = 2594,
                    CurrencyTypeId = 2,
                    Name = "LatiumX",
                    Abbreviation = "LATX"
                }, new Currency
                {
                    Id = 1399,
                    CurrencyTypeId = 2,
                    Name = "Sequence",
                    Abbreviation = "SEQ"
                }, new Currency
                {
                    Id = 2421,
                    CurrencyTypeId = 2,
                    Name = "VouchForMe",
                    Abbreviation = "IPL"
                }, new Currency
                {
                    Id = 1991,
                    CurrencyTypeId = 2,
                    Name = "Rivetz",
                    Abbreviation = "RVT"
                }, new Currency
                {
                    Id = 1769,
                    CurrencyTypeId = 2,
                    Name = "Denarius",
                    Abbreviation = "D"
                }, new Currency
                {
                    Id = 3113,
                    CurrencyTypeId = 2,
                    Name = "InterCrone",
                    Abbreviation = "ICR"
                }, new Currency
                {
                    Id = 1191,
                    CurrencyTypeId = 2,
                    Name = "Memetic / PepeCoin",
                    Abbreviation = "MEME"
                }, new Currency
                {
                    Id = 293,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Plus",
                    Abbreviation = "XBC"
                }, new Currency
                {
                    Id = 2508,
                    CurrencyTypeId = 2,
                    Name = "DCORP Utility",
                    Abbreviation = "DRPU"
                }, new Currency
                {
                    Id = 3141,
                    CurrencyTypeId = 2,
                    Name = "Blockpass",
                    Abbreviation = "PASS"
                }, new Currency
                {
                    Id = 3080,
                    CurrencyTypeId = 2,
                    Name = "Commercium",
                    Abbreviation = "CMM"
                }, new Currency
                {
                    Id = 2528,
                    CurrencyTypeId = 2,
                    Name = "Dether",
                    Abbreviation = "DTH"
                }, new Currency
                {
                    Id = 2912,
                    CurrencyTypeId = 2,
                    Name = "SnowGem",
                    Abbreviation = "XSG"
                }, new Currency
                {
                    Id = 2236,
                    CurrencyTypeId = 2,
                    Name = "MyWish",
                    Abbreviation = "WISH"
                }, new Currency
                {
                    Id = 2775,
                    CurrencyTypeId = 2,
                    Name = "Faceter",
                    Abbreviation = "FACE"
                }, new Currency
                {
                    Id = 633,
                    CurrencyTypeId = 2,
                    Name = "ExclusiveCoin",
                    Abbreviation = "EXCL"
                }, new Currency
                {
                    Id = 2649,
                    CurrencyTypeId = 2,
                    Name = "DeviantCoin",
                    Abbreviation = "DEV"
                }, new Currency
                {
                    Id = 3323,
                    CurrencyTypeId = 2,
                    Name = "PAYCENT",
                    Abbreviation = "PYN"
                }, new Currency
                {
                    Id = 313,
                    CurrencyTypeId = 2,
                    Name = "PinkCoin",
                    Abbreviation = "PINK"
                }, new Currency
                {
                    Id = 2418,
                    CurrencyTypeId = 2,
                    Name = "Maverick Chain",
                    Abbreviation = "MVC"
                }, new Currency
                {
                    Id = 788,
                    CurrencyTypeId = 2,
                    Name = "Circuits of Value",
                    Abbreviation = "COVAL"
                }, new Currency
                {
                    Id = 3360,
                    CurrencyTypeId = 2,
                    Name = "Eristica",
                    Abbreviation = "ERT"
                }, new Currency
                {
                    Id = 1340,
                    CurrencyTypeId = 2,
                    Name = "Karbo",
                    Abbreviation = "KRB"
                }, new Currency
                {
                    Id = 2582,
                    CurrencyTypeId = 2,
                    Name = "LALA World",
                    Abbreviation = "LALA"
                }, new Currency
                {
                    Id = 3220,
                    CurrencyTypeId = 2,
                    Name = "DAV Coin",
                    Abbreviation = "DAV"
                }, new Currency
                {
                    Id = 3118,
                    CurrencyTypeId = 2,
                    Name = "Graviocoin",
                    Abbreviation = "GIO"
                }, new Currency
                {
                    Id = 2127,
                    CurrencyTypeId = 2,
                    Name = "eBitcoin",
                    Abbreviation = "EBTC"
                }, new Currency
                {
                    Id = 2002,
                    CurrencyTypeId = 2,
                    Name = "TrezarCoin",
                    Abbreviation = "TZC"
                }, new Currency
                {
                    Id = 3469,
                    CurrencyTypeId = 2,
                    Name = "TrueDeck",
                    Abbreviation = "TDP"
                }, new Currency
                {
                    Id = 2547,
                    CurrencyTypeId = 2,
                    Name = "Experty",
                    Abbreviation = "EXY"
                }, new Currency
                {
                    Id = 1873,
                    CurrencyTypeId = 2,
                    Name = "Blocktix",
                    Abbreviation = "TIX"
                }, new Currency
                {
                    Id = 3237,
                    CurrencyTypeId = 2,
                    Name = "Timicoin",
                    Abbreviation = "TMC"
                }, new Currency
                {
                    Id = 2724,
                    CurrencyTypeId = 2,
                    Name = "Zippie",
                    Abbreviation = "ZIPT"
                }, new Currency
                {
                    Id = 3877,
                    CurrencyTypeId = 2,
                    Name = "WebDollar",
                    Abbreviation = "WEBD"
                }, new Currency
                {
                    Id = 1762,
                    CurrencyTypeId = 2,
                    Name = "Ergo",
                    Abbreviation = "EFYT"
                }, new Currency
                {
                    Id = 3455,
                    CurrencyTypeId = 2,
                    Name = "DEEX",
                    Abbreviation = "DEEX"
                }, new Currency
                {
                    Id = 3365,
                    CurrencyTypeId = 2,
                    Name = "VeriSafe",
                    Abbreviation = "VSF"
                }, new Currency
                {
                    Id = 1002,
                    CurrencyTypeId = 2,
                    Name = "Sprouts",
                    Abbreviation = "SPRTS"
                }, new Currency
                {
                    Id = 2273,
                    CurrencyTypeId = 2,
                    Name = "Uquid Coin",
                    Abbreviation = "UQC"
                }, new Currency
                {
                    Id = 3302,
                    CurrencyTypeId = 2,
                    Name = "UChain",
                    Abbreviation = "UCN"
                }, new Currency
                {
                    Id = 1304,
                    CurrencyTypeId = 2,
                    Name = "Syndicate",
                    Abbreviation = "SYNX"
                }, new Currency
                {
                    Id = 3240,
                    CurrencyTypeId = 2,
                    Name = "Ethersocial",
                    Abbreviation = "ESN"
                }, new Currency
                {
                    Id = 2676,
                    CurrencyTypeId = 2,
                    Name = "PHI Token",
                    Abbreviation = "PHI"
                }, new Currency
                {
                    Id = 1156,
                    CurrencyTypeId = 2,
                    Name = "Yocoin",
                    Abbreviation = "YOC"
                }, new Currency
                {
                    Id = 2985,
                    CurrencyTypeId = 2,
                    Name = "ARBITRAGE",
                    Abbreviation = "ARB"
                }, new Currency
                {
                    Id = 3148,
                    CurrencyTypeId = 2,
                    Name = "MetaMorph",
                    Abbreviation = "METM"
                }, new Currency
                {
                    Id = 3028,
                    CurrencyTypeId = 2,
                    Name = "Formosa Financial",
                    Abbreviation = "FMF"
                }, new Currency
                {
                    Id = 2495,
                    CurrencyTypeId = 2,
                    Name = "PARETO Rewards",
                    Abbreviation = "PARETO"
                }, new Currency
                {
                    Id = 322,
                    CurrencyTypeId = 2,
                    Name = "Energycoin",
                    Abbreviation = "ENRG"
                }, new Currency
                {
                    Id = 3017,
                    CurrencyTypeId = 2,
                    Name = "Coinvest",
                    Abbreviation = "COIN"
                }, new Currency
                {
                    Id = 2863,
                    CurrencyTypeId = 2,
                    Name = "HOLD",
                    Abbreviation = "HOLD"
                }, new Currency
                {
                    Id = 3909,
                    CurrencyTypeId = 2,
                    Name = "SPIDER VPS",
                    Abbreviation = "SPDR"
                }, new Currency
                {
                    Id = 3094,
                    CurrencyTypeId = 2,
                    Name = "Scorum Coins",
                    Abbreviation = "SCR"
                }, new Currency
                {
                    Id = 3689,
                    CurrencyTypeId = 2,
                    Name = "Mocrow",
                    Abbreviation = "MCW"
                }, new Currency
                {
                    Id = 3264,
                    CurrencyTypeId = 2,
                    Name = "Digital Insurance Token",
                    Abbreviation = "DIT"
                }, new Currency
                {
                    Id = 2272,
                    CurrencyTypeId = 2,
                    Name = "Soma",
                    Abbreviation = "SCT"
                }, new Currency
                {
                    Id = 1916,
                    CurrencyTypeId = 2,
                    Name = "BiblePay",
                    Abbreviation = "BBP"
                }, new Currency
                {
                    Id = 2199,
                    CurrencyTypeId = 2,
                    Name = "ALQO",
                    Abbreviation = "XLQ"
                }, new Currency
                {
                    Id = 3423,
                    CurrencyTypeId = 2,
                    Name = "Sharpay",
                    Abbreviation = "S"
                }, new Currency
                {
                    Id = 2966,
                    CurrencyTypeId = 2,
                    Name = "Fox Trading",
                    Abbreviation = "FOXT"
                }, new Currency
                {
                    Id = 1845,
                    CurrencyTypeId = 2,
                    Name = "IXT",
                    Abbreviation = "IXT"
                }, new Currency
                {
                    Id = 2614,
                    CurrencyTypeId = 2,
                    Name = "BlitzPredict",
                    Abbreviation = "XBP"
                }, new Currency
                {
                    Id = 1226,
                    CurrencyTypeId = 2,
                    Name = "Qwark",
                    Abbreviation = "QWARK"
                }, new Currency
                {
                    Id = 3171,
                    CurrencyTypeId = 2,
                    Name = "HeartBout",
                    Abbreviation = "HB"
                }, new Currency
                {
                    Id = 1970,
                    CurrencyTypeId = 2,
                    Name = "ATBCoin",
                    Abbreviation = "ATB"
                }, new Currency
                {
                    Id = 3024,
                    CurrencyTypeId = 2,
                    Name = "Arionum",
                    Abbreviation = "ARO"
                }, new Currency
                {
                    Id = 3402,
                    CurrencyTypeId = 2,
                    Name = "Ifoods Chain",
                    Abbreviation = "IFOOD"
                }, new Currency
                {
                    Id = 3765,
                    CurrencyTypeId = 2,
                    Name = "Serve",
                    Abbreviation = "SERV"
                }, new Currency
                {
                    Id = 1008,
                    CurrencyTypeId = 2,
                    Name = "Capricoin",
                    Abbreviation = "CPC"
                }, new Currency
                {
                    Id = 3753,
                    CurrencyTypeId = 2,
                    Name = "PlatonCoin",
                    Abbreviation = "PLTC"
                }, new Currency
                {
                    Id = 2976,
                    CurrencyTypeId = 2,
                    Name = "Ryo Currency",
                    Abbreviation = "RYO"
                }, new Currency
                {
                    Id = 3879,
                    CurrencyTypeId = 2,
                    Name = "ESBC",
                    Abbreviation = "ESBC"
                }, new Currency
                {
                    Id = 1578,
                    CurrencyTypeId = 2,
                    Name = "Zero",
                    Abbreviation = "ZER"
                }, new Currency
                {
                    Id = 3230,
                    CurrencyTypeId = 2,
                    Name = "VULCANO",
                    Abbreviation = "VULC"
                }, new Currency
                {
                    Id = 3732,
                    CurrencyTypeId = 2,
                    Name = "Conceal",
                    Abbreviation = "CCX"
                }, new Currency
                {
                    Id = 3244,
                    CurrencyTypeId = 2,
                    Name = "Mindexcoin",
                    Abbreviation = "MIC"
                }, new Currency
                {
                    Id = 2879,
                    CurrencyTypeId = 2,
                    Name = "Origin Sport",
                    Abbreviation = "ORS"
                }, new Currency
                {
                    Id = 3245,
                    CurrencyTypeId = 2,
                    Name = "Ubcoin Market",
                    Abbreviation = "UBC"
                }, new Currency
                {
                    Id = 1894,
                    CurrencyTypeId = 2,
                    Name = "The ChampCoin",
                    Abbreviation = "TCC"
                }, new Currency
                {
                    Id = 2278,
                    CurrencyTypeId = 2,
                    Name = "HollyWoodCoin",
                    Abbreviation = "HWC"
                }, new Currency
                {
                    Id = 3166,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Incognito",
                    Abbreviation = "XBI"
                }, new Currency
                {
                    Id = 1869,
                    CurrencyTypeId = 2,
                    Name = "Mao Zedong",
                    Abbreviation = "MAO"
                }, new Currency
                {
                    Id = 3674,
                    CurrencyTypeId = 2,
                    Name = "Globatalent",
                    Abbreviation = "GBT"
                }, new Currency
                {
                    Id = 3901,
                    CurrencyTypeId = 2,
                    Name = "KuboCoin",
                    Abbreviation = "KUBO"
                }, new Currency
                {
                    Id = 3752,
                    CurrencyTypeId = 2,
                    Name = "uPlexa",
                    Abbreviation = "UPX"
                }, new Currency
                {
                    Id = 2414,
                    CurrencyTypeId = 2,
                    Name = "RealChain",
                    Abbreviation = "RCT"
                }, new Currency
                {
                    Id = 2663,
                    CurrencyTypeId = 2,
                    Name = "StarCoin",
                    Abbreviation = "KST"
                }, new Currency
                {
                    Id = 3805,
                    CurrencyTypeId = 2,
                    Name = "BoatPilot Token",
                    Abbreviation = "NAVY"
                }, new Currency
                {
                    Id = 2680,
                    CurrencyTypeId = 2,
                    Name = "HBZ coin",
                    Abbreviation = "HBZ"
                }, new Currency
                {
                    Id = 2920,
                    CurrencyTypeId = 2,
                    Name = "0xcert",
                    Abbreviation = "ZXC"
                }, new Currency
                {
                    Id = 2988,
                    CurrencyTypeId = 2,
                    Name = "Pigeoncoin",
                    Abbreviation = "PGN"
                }, new Currency
                {
                    Id = 3362,
                    CurrencyTypeId = 2,
                    Name = "Auxilium",
                    Abbreviation = "AUX"
                }, new Currency
                {
                    Id = 3373,
                    CurrencyTypeId = 2,
                    Name = "Bethereum",
                    Abbreviation = "BETHER"
                }, new Currency
                {
                    Id = 3596,
                    CurrencyTypeId = 2,
                    Name = "Nerva",
                    Abbreviation = "XNV"
                }, new Currency
                {
                    Id = 3011,
                    CurrencyTypeId = 2,
                    Name = "BitScreener Token",
                    Abbreviation = "BITX"
                }, new Currency
                {
                    Id = 2922,
                    CurrencyTypeId = 2,
                    Name = "Atonomi",
                    Abbreviation = "ATMI"
                }, new Currency
                {
                    Id = 3597,
                    CurrencyTypeId = 2,
                    Name = "InterValue",
                    Abbreviation = "INVE"
                }, new Currency
                {
                    Id = 1745,
                    CurrencyTypeId = 2,
                    Name = "Dinastycoin",
                    Abbreviation = "DCY"
                }, new Currency
                {
                    Id = 1216,
                    CurrencyTypeId = 2,
                    Name = "EDRCoin",
                    Abbreviation = "EDRC"
                }, new Currency
                {
                    Id = 2584,
                    CurrencyTypeId = 2,
                    Name = "Debitum",
                    Abbreviation = "DEB"
                }, new Currency
                {
                    Id = 3440,
                    CurrencyTypeId = 2,
                    Name = "AirWire",
                    Abbreviation = "WIRE"
                }, new Currency
                {
                    Id = 3158,
                    CurrencyTypeId = 2,
                    Name = "ZCore",
                    Abbreviation = "ZCR"
                }, new Currency
                {
                    Id = 3758,
                    CurrencyTypeId = 2,
                    Name = "Max Property Group",
                    Abbreviation = "MPG"
                }, new Currency
                {
                    Id = 3411,
                    CurrencyTypeId = 2,
                    Name = "Welltrado",
                    Abbreviation = "WTL"
                }, new Currency
                {
                    Id = 3643,
                    CurrencyTypeId = 2,
                    Name = "TENA",
                    Abbreviation = "TENA"
                }, new Currency
                {
                    Id = 2413,
                    CurrencyTypeId = 2,
                    Name = "Ethouse",
                    Abbreviation = "HORSE"
                }, new Currency
                {
                    Id = 2557,
                    CurrencyTypeId = 2,
                    Name = "Bee Token",
                    Abbreviation = "BEE"
                }, new Currency
                {
                    Id = 2568,
                    CurrencyTypeId = 2,
                    Name = "JET8",
                    Abbreviation = "J8T"
                }, new Currency
                {
                    Id = 2759,
                    CurrencyTypeId = 2,
                    Name = "Patron",
                    Abbreviation = "PAT"
                }, new Currency
                {
                    Id = 2956,
                    CurrencyTypeId = 2,
                    Name = "Narrative",
                    Abbreviation = "NRVE"
                }, new Currency
                {
                    Id = 3661,
                    CurrencyTypeId = 2,
                    Name = "Stronghold Token",
                    Abbreviation = "SHX"
                }, new Currency
                {
                    Id = 3613,
                    CurrencyTypeId = 2,
                    Name = "Dash Green",
                    Abbreviation = "DASHG"
                }, new Currency
                {
                    Id = 3947,
                    CurrencyTypeId = 2,
                    Name = "HashNet BitEco",
                    Abbreviation = "HNB"
                }, new Currency
                {
                    Id = 3774,
                    CurrencyTypeId = 2,
                    Name = "Maincoin",
                    Abbreviation = "MNC"
                }, new Currency
                {
                    Id = 2601,
                    CurrencyTypeId = 2,
                    Name = "1World",
                    Abbreviation = "1WO"
                }, new Currency
                {
                    Id = 2142,
                    CurrencyTypeId = 2,
                    Name = "FORCE",
                    Abbreviation = "FOR"
                }, new Currency
                {
                    Id = 2653,
                    CurrencyTypeId = 2,
                    Name = "Auctus",
                    Abbreviation = "AUC"
                }, new Currency
                {
                    Id = 2330,
                    CurrencyTypeId = 2,
                    Name = "Pylon Network",
                    Abbreviation = "PYLNT"
                }, new Currency
                {
                    Id = 3876,
                    CurrencyTypeId = 2,
                    Name = "EnterCoin",
                    Abbreviation = "ENTRC"
                }, new Currency
                {
                    Id = 2200,
                    CurrencyTypeId = 2,
                    Name = "GoByte",
                    Abbreviation = "GBX"
                }, new Currency
                {
                    Id = 1387,
                    CurrencyTypeId = 2,
                    Name = "VeriumReserve",
                    Abbreviation = "VRM"
                }, new Currency
                {
                    Id = 2295,
                    CurrencyTypeId = 2,
                    Name = "Starbase",
                    Abbreviation = "STAR"
                }, new Currency
                {
                    Id = 2465,
                    CurrencyTypeId = 2,
                    Name = "Blockport",
                    Abbreviation = "BPT"
                }, new Currency
                {
                    Id = 2311,
                    CurrencyTypeId = 2,
                    Name = "ACE (TokenStars)",
                    Abbreviation = "ACE"
                }, new Currency
                {
                    Id = 2961,
                    CurrencyTypeId = 2,
                    Name = "Relex",
                    Abbreviation = "RLX"
                }, new Currency
                {
                    Id = 1694,
                    CurrencyTypeId = 2,
                    Name = "Sumokoin",
                    Abbreviation = "SUMO"
                }, new Currency
                {
                    Id = 2258,
                    CurrencyTypeId = 2,
                    Name = "Snovian.Space",
                    Abbreviation = "SNOV"
                }, new Currency
                {
                    Id = 3818,
                    CurrencyTypeId = 2,
                    Name = "GoPower",
                    Abbreviation = "GPT"
                }, new Currency
                {
                    Id = 2836,
                    CurrencyTypeId = 2,
                    Name = "Bigbom",
                    Abbreviation = "BBO"
                }, new Currency
                {
                    Id = 1588,
                    CurrencyTypeId = 2,
                    Name = "Tokes",
                    Abbreviation = "TKS"
                }, new Currency
                {
                    Id = 1249,
                    CurrencyTypeId = 2,
                    Name = "Elcoin",
                    Abbreviation = "EL"
                }, new Currency
                {
                    Id = 3372,
                    CurrencyTypeId = 2,
                    Name = "Repme",
                    Abbreviation = "RPM"
                }, new Currency
                {
                    Id = 1902,
                    CurrencyTypeId = 2,
                    Name = "MyBit",
                    Abbreviation = "MYB"
                }, new Currency
                {
                    Id = 2884,
                    CurrencyTypeId = 2,
                    Name = "FSBT API Token",
                    Abbreviation = "FSBT"
                }, new Currency
                {
                    Id = 3179,
                    CurrencyTypeId = 2,
                    Name = "Arbidex",
                    Abbreviation = "ABX"
                }, new Currency
                {
                    Id = 3128,
                    CurrencyTypeId = 2,
                    Name = "SiaCashCoin",
                    Abbreviation = "SCC"
                }, new Currency
                {
                    Id = 3119,
                    CurrencyTypeId = 2,
                    Name = "Alchemint Standards",
                    Abbreviation = "SDS"
                }, new Currency
                {
                    Id = 3106,
                    CurrencyTypeId = 2,
                    Name = "PKG Token",
                    Abbreviation = "PKG"
                }, new Currency
                {
                    Id = 3837,
                    CurrencyTypeId = 2,
                    Name = "MFCoin",
                    Abbreviation = "MFC"
                }, new Currency
                {
                    Id = 2089,
                    CurrencyTypeId = 2,
                    Name = "ClearPoll",
                    Abbreviation = "POLL"
                }, new Currency
                {
                    Id = 3071,
                    CurrencyTypeId = 2,
                    Name = "EUNO",
                    Abbreviation = "EUNO"
                }, new Currency
                {
                    Id = 2271,
                    CurrencyTypeId = 2,
                    Name = "Verify",
                    Abbreviation = "CRED"
                }, new Currency
                {
                    Id = 1704,
                    CurrencyTypeId = 2,
                    Name = "eBoost",
                    Abbreviation = "EBST"
                }, new Currency
                {
                    Id = 2611,
                    CurrencyTypeId = 2,
                    Name = "Spectiv",
                    Abbreviation = "SIG"
                }, new Currency
                {
                    Id = 56,
                    CurrencyTypeId = 2,
                    Name = "Zetacoin",
                    Abbreviation = "ZET"
                }, new Currency
                {
                    Id = 2323,
                    CurrencyTypeId = 2,
                    Name = "HEROcoin",
                    Abbreviation = "PLAY"
                }, new Currency
                {
                    Id = 3101,
                    CurrencyTypeId = 2,
                    Name = "OptiToken",
                    Abbreviation = "OPTI"
                }, new Currency
                {
                    Id = 2237,
                    CurrencyTypeId = 2,
                    Name = "EventChain",
                    Abbreviation = "EVC"
                }, new Currency
                {
                    Id = 2968,
                    CurrencyTypeId = 2,
                    Name = "Bridge Protocol",
                    Abbreviation = "BRDG"
                }, new Currency
                {
                    Id = 2269,
                    CurrencyTypeId = 2,
                    Name = "WandX",
                    Abbreviation = "WAND"
                }, new Currency
                {
                    Id = 977,
                    CurrencyTypeId = 2,
                    Name = "GravityCoin",
                    Abbreviation = "GXX"
                }, new Currency
                {
                    Id = 2754,
                    CurrencyTypeId = 2,
                    Name = "HeroNode",
                    Abbreviation = "HER"
                }, new Currency
                {
                    Id = 2963,
                    CurrencyTypeId = 2,
                    Name = "View",
                    Abbreviation = "VIEW"
                }, new Currency
                {
                    Id = 2658,
                    CurrencyTypeId = 2,
                    Name = "SyncFab",
                    Abbreviation = "MFG"
                }, new Currency
                {
                    Id = 3247,
                    CurrencyTypeId = 2,
                    Name = "Fire Lotto",
                    Abbreviation = "FLOT"
                }, new Currency
                {
                    Id = 2774,
                    CurrencyTypeId = 2,
                    Name = "Invacio",
                    Abbreviation = "INV"
                }, new Currency
                {
                    Id = 2646,
                    CurrencyTypeId = 2,
                    Name = "AdHive",
                    Abbreviation = "ADH"
                }, new Currency
                {
                    Id = 2504,
                    CurrencyTypeId = 2,
                    Name = "Iungo",
                    Abbreviation = "ING"
                }, new Currency
                {
                    Id = 2946,
                    CurrencyTypeId = 2,
                    Name = "Proton Token",
                    Abbreviation = "PTT"
                }, new Currency
                {
                    Id = 2656,
                    CurrencyTypeId = 2,
                    Name = "Daneel",
                    Abbreviation = "DAN"
                }, new Currency
                {
                    Id = 3942,
                    CurrencyTypeId = 2,
                    Name = "Qwertycoin",
                    Abbreviation = "QWC"
                }, new Currency
                {
                    Id = 3672,
                    CurrencyTypeId = 2,
                    Name = "DogeCash",
                    Abbreviation = "DOGEC"
                }, new Currency
                {
                    Id = 2445,
                    CurrencyTypeId = 2,
                    Name = "Block Array",
                    Abbreviation = "ARY"
                }, new Currency
                {
                    Id = 3793,
                    CurrencyTypeId = 2,
                    Name = "Galilel",
                    Abbreviation = "GALI"
                }, new Currency
                {
                    Id = 2126,
                    CurrencyTypeId = 2,
                    Name = "FlypMe",
                    Abbreviation = "FYP"
                }, new Currency
                {
                    Id = 2615,
                    CurrencyTypeId = 2,
                    Name = "Blocklancer",
                    Abbreviation = "LNC"
                }, new Currency
                {
                    Id = 3203,
                    CurrencyTypeId = 2,
                    Name = "Lobstex",
                    Abbreviation = "LOBS"
                }, new Currency
                {
                    Id = 2870,
                    CurrencyTypeId = 2,
                    Name = "FantasyGold",
                    Abbreviation = "FGC"
                }, new Currency
                {
                    Id = 3627,
                    CurrencyTypeId = 2,
                    Name = "Block-Logic",
                    Abbreviation = "BLTG"
                }, new Currency
                {
                    Id = 3278,
                    CurrencyTypeId = 2,
                    Name = "PENG",
                    Abbreviation = "PENG"
                }, new Currency
                {
                    Id = 2990,
                    CurrencyTypeId = 2,
                    Name = "EXMR",
                    Abbreviation = "EXMR"
                }, new Currency
                {
                    Id = 2940,
                    CurrencyTypeId = 2,
                    Name = "Sp8de",
                    Abbreviation = "SPX"
                }, new Currency
                {
                    Id = 3619,
                    CurrencyTypeId = 2,
                    Name = "BEAT",
                    Abbreviation = "BEAT"
                }, new Currency
                {
                    Id = 3615,
                    CurrencyTypeId = 2,
                    Name = "HyperQuant",
                    Abbreviation = "HQT"
                }, new Currency
                {
                    Id = 2705,
                    CurrencyTypeId = 2,
                    Name = "Amon",
                    Abbreviation = "AMN"
                }, new Currency
                {
                    Id = 2733,
                    CurrencyTypeId = 2,
                    Name = "Freyrchain",
                    Abbreviation = "FREC"
                }, new Currency
                {
                    Id = 2509,
                    CurrencyTypeId = 2,
                    Name = "EtherSportz",
                    Abbreviation = "ESZ"
                }, new Currency
                {
                    Id = 2367,
                    CurrencyTypeId = 2,
                    Name = "Aigang",
                    Abbreviation = "AIX"
                }, new Currency
                {
                    Id = 3878,
                    CurrencyTypeId = 2,
                    Name = "Swap",
                    Abbreviation = "XWP"
                }, new Currency
                {
                    Id = 2679,
                    CurrencyTypeId = 2,
                    Name = "Decentralized Machine Learning",
                    Abbreviation = "DML"
                }, new Currency
                {
                    Id = 1933,
                    CurrencyTypeId = 2,
                    Name = "Suretly",
                    Abbreviation = "SUR"
                }, new Currency
                {
                    Id = 2660,
                    CurrencyTypeId = 2,
                    Name = "Aditus",
                    Abbreviation = "ADI"
                }, new Currency
                {
                    Id = 2889,
                    CurrencyTypeId = 2,
                    Name = "Bob's Repair",
                    Abbreviation = "BOB"
                }, new Currency
                {
                    Id = 3390,
                    CurrencyTypeId = 2,
                    Name = "Quantis Network",
                    Abbreviation = "QUAN"
                }, new Currency
                {
                    Id = 145,
                    CurrencyTypeId = 2,
                    Name = "DopeCoin",
                    Abbreviation = "DOPE"
                }, new Currency
                {
                    Id = 3285,
                    CurrencyTypeId = 2,
                    Name = "Birake",
                    Abbreviation = "BIR"
                }, new Currency
                {
                    Id = 2684,
                    CurrencyTypeId = 2,
                    Name = "Aphelion",
                    Abbreviation = "APH"
                }, new Currency
                {
                    Id = 3018,
                    CurrencyTypeId = 2,
                    Name = "Kalkulus",
                    Abbreviation = "KLKS"
                }, new Currency
                {
                    Id = 2729,
                    CurrencyTypeId = 2,
                    Name = "TEAM (TokenStars)",
                    Abbreviation = "TEAM"
                }, new Currency
                {
                    Id = 2849,
                    CurrencyTypeId = 2,
                    Name = "Hurify",
                    Abbreviation = "HUR"
                }, new Currency
                {
                    Id = 2948,
                    CurrencyTypeId = 2,
                    Name = "Jury.Online Token",
                    Abbreviation = "JOT"
                }, new Currency
                {
                    Id = 3953,
                    CurrencyTypeId = 2,
                    Name = "Evedo",
                    Abbreviation = "EVED"
                }, new Currency
                {
                    Id = 3132,
                    CurrencyTypeId = 2,
                    Name = "EtherGem",
                    Abbreviation = "EGEM"
                }, new Currency
                {
                    Id = 3482,
                    CurrencyTypeId = 2,
                    Name = "Teloscoin",
                    Abbreviation = "TELOS"
                }, new Currency
                {
                    Id = 3604,
                    CurrencyTypeId = 2,
                    Name = "SkyHub Coin",
                    Abbreviation = "SHB"
                }, new Currency
                {
                    Id = 2923,
                    CurrencyTypeId = 2,
                    Name = "XMCT",
                    Abbreviation = "XMCT"
                }, new Currency
                {
                    Id = 3248,
                    CurrencyTypeId = 2,
                    Name = "AiLink Token",
                    Abbreviation = "ALI"
                }, new Currency
                {
                    Id = 3505,
                    CurrencyTypeId = 2,
                    Name = "Typerium",
                    Abbreviation = "TYPE"
                }, new Currency
                {
                    Id = 2744,
                    CurrencyTypeId = 2,
                    Name = "NPER",
                    Abbreviation = "NPER"
                }, new Currency
                {
                    Id = 3339,
                    CurrencyTypeId = 2,
                    Name = "Puregold Token",
                    Abbreviation = "PGTS"
                }, new Currency
                {
                    Id = 3112,
                    CurrencyTypeId = 2,
                    Name = "Bitnation",
                    Abbreviation = "XPAT"
                }, new Currency
                {
                    Id = 3742,
                    CurrencyTypeId = 2,
                    Name = "Chimpion",
                    Abbreviation = "BNANA"
                }, new Currency
                {
                    Id = 3209,
                    CurrencyTypeId = 2,
                    Name = "4NEW",
                    Abbreviation = "KWATT"
                }, new Currency
                {
                    Id = 3868,
                    CurrencyTypeId = 2,
                    Name = "SignatureChain ",
                    Abbreviation = "SICA"
                }, new Currency
                {
                    Id = 3027,
                    CurrencyTypeId = 2,
                    Name = "Webcoin",
                    Abbreviation = "WEB"
                }, new Currency
                {
                    Id = 551,
                    CurrencyTypeId = 2,
                    Name = "NeosCoin",
                    Abbreviation = "NEOS"
                }, new Currency
                {
                    Id = 3149,
                    CurrencyTypeId = 2,
                    Name = "NetKoin",
                    Abbreviation = "NTK"
                }, new Currency
                {
                    Id = 2270,
                    CurrencyTypeId = 2,
                    Name = "SportyCo",
                    Abbreviation = "SPF"
                }, new Currency
                {
                    Id = 2356,
                    CurrencyTypeId = 2,
                    Name = "CFun",
                    Abbreviation = "CFUN"
                }, new Currency
                {
                    Id = 3792,
                    CurrencyTypeId = 2,
                    Name = "ARAW",
                    Abbreviation = "ARAW"
                }, new Currency
                {
                    Id = 1465,
                    CurrencyTypeId = 2,
                    Name = "Veros",
                    Abbreviation = "VRS"
                }, new Currency
                {
                    Id = 3763,
                    CurrencyTypeId = 2,
                    Name = "ODUWA",
                    Abbreviation = "OWC"
                }, new Currency
                {
                    Id = 3256,
                    CurrencyTypeId = 2,
                    Name = "Bitether",
                    Abbreviation = "BTR"
                }, new Currency
                {
                    Id = 2944,
                    CurrencyTypeId = 2,
                    Name = "Elysian",
                    Abbreviation = "ELY"
                }, new Currency
                {
                    Id = 2984,
                    CurrencyTypeId = 2,
                    Name = "Newton Coin Project",
                    Abbreviation = "NCP"
                }, new Currency
                {
                    Id = 3121,
                    CurrencyTypeId = 2,
                    Name = "IGToken",
                    Abbreviation = "IG"
                }, new Currency
                {
                    Id = 730,
                    CurrencyTypeId = 2,
                    Name = "GCN Coin",
                    Abbreviation = "GCN"
                }, new Currency
                {
                    Id = 3097,
                    CurrencyTypeId = 2,
                    Name = "XOVBank",
                    Abbreviation = "XOV"
                }, new Currency
                {
                    Id = 2720,
                    CurrencyTypeId = 2,
                    Name = "Parkgene",
                    Abbreviation = "GENE"
                }, new Currency
                {
                    Id = 3823,
                    CurrencyTypeId = 2,
                    Name = "OLXA",
                    Abbreviation = "OLXA"
                }, new Currency
                {
                    Id = 3386,
                    CurrencyTypeId = 2,
                    Name = "Actinium",
                    Abbreviation = "ACM"
                }, new Currency
                {
                    Id = 1252,
                    CurrencyTypeId = 2,
                    Name = "2GIVE",
                    Abbreviation = "2GIVE"
                }, new Currency
                {
                    Id = 2165,
                    CurrencyTypeId = 2,
                    Name = "ERC20",
                    Abbreviation = "ERC20"
                }, new Currency
                {
                    Id = 3383,
                    CurrencyTypeId = 2,
                    Name = "Knekted",
                    Abbreviation = "KNT"
                }, new Currency
                {
                    Id = 3497,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Zero",
                    Abbreviation = "BZX"
                }, new Currency
                {
                    Id = 2717,
                    CurrencyTypeId = 2,
                    Name = "BoutsPro",
                    Abbreviation = "BOUTS"
                }, new Currency
                {
                    Id = 3808,
                    CurrencyTypeId = 2,
                    Name = "Cointorox",
                    Abbreviation = "OROX"
                }, new Currency
                {
                    Id = 1787,
                    CurrencyTypeId = 2,
                    Name = "Jetcoin",
                    Abbreviation = "JET"
                }, new Currency
                {
                    Id = 2825,
                    CurrencyTypeId = 2,
                    Name = "Naviaddress",
                    Abbreviation = "NAVI"
                }, new Currency
                {
                    Id = 3452,
                    CurrencyTypeId = 2,
                    Name = "Ether-1",
                    Abbreviation = "ETHO"
                }, new Currency
                {
                    Id = 2704,
                    CurrencyTypeId = 2,
                    Name = "Transcodium",
                    Abbreviation = "TNS"
                }, new Currency
                {
                    Id = 2172,
                    CurrencyTypeId = 2,
                    Name = "Emphy",
                    Abbreviation = "EPY"
                }, new Currency
                {
                    Id = 916,
                    CurrencyTypeId = 2,
                    Name = "MedicCoin",
                    Abbreviation = "MEDIC"
                }, new Currency
                {
                    Id = 1803,
                    CurrencyTypeId = 2,
                    Name = "PeepCoin",
                    Abbreviation = "PCN"
                }, new Currency
                {
                    Id = 3589,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Meta",
                    Abbreviation = "ETHM"
                }, new Currency
                {
                    Id = 1736,
                    CurrencyTypeId = 2,
                    Name = "Unify",
                    Abbreviation = "UNIFY"
                }, new Currency
                {
                    Id = 2452,
                    CurrencyTypeId = 2,
                    Name = "Tokenbox",
                    Abbreviation = "TBX"
                }, new Currency
                {
                    Id = 2747,
                    CurrencyTypeId = 2,
                    Name = "BlockMesh",
                    Abbreviation = "BMH"
                }, new Currency
                {
                    Id = 3856,
                    CurrencyTypeId = 2,
                    Name = "SF Capital",
                    Abbreviation = "SFCP"
                }, new Currency
                {
                    Id = 3449,
                    CurrencyTypeId = 2,
                    Name = "MMOCoin",
                    Abbreviation = "MMO"
                }, new Currency
                {
                    Id = 2256,
                    CurrencyTypeId = 2,
                    Name = "Bonpay",
                    Abbreviation = "BON"
                }, new Currency
                {
                    Id = 2244,
                    CurrencyTypeId = 2,
                    Name = "Payfair",
                    Abbreviation = "PFR"
                }, new Currency
                {
                    Id = 1650,
                    CurrencyTypeId = 2,
                    Name = "ProCurrency",
                    Abbreviation = "PROC"
                }, new Currency
                {
                    Id = 2286,
                    CurrencyTypeId = 2,
                    Name = "MicroMoney",
                    Abbreviation = "AMM"
                }, new Currency
                {
                    Id = 3869,
                    CurrencyTypeId = 2,
                    Name = "Alpha Token",
                    Abbreviation = "A"
                }, new Currency
                {
                    Id = 2198,
                    CurrencyTypeId = 2,
                    Name = "Viuly",
                    Abbreviation = "VIU"
                }, new Currency
                {
                    Id = 3741,
                    CurrencyTypeId = 2,
                    Name = "EurocoinToken",
                    Abbreviation = "ECTE"
                }, new Currency
                {
                    Id = 1830,
                    CurrencyTypeId = 2,
                    Name = "SkinCoin",
                    Abbreviation = "SKIN"
                }, new Currency
                {
                    Id = 2419,
                    CurrencyTypeId = 2,
                    Name = "Profile Utility Token",
                    Abbreviation = "PUT"
                }, new Currency
                {
                    Id = 3708,
                    CurrencyTypeId = 2,
                    Name = "Exosis",
                    Abbreviation = "EXO"
                }, new Currency
                {
                    Id = 2042,
                    CurrencyTypeId = 2,
                    Name = "HelloGold",
                    Abbreviation = "HGT"
                }, new Currency
                {
                    Id = 3523,
                    CurrencyTypeId = 2,
                    Name = "SnodeCoin",
                    Abbreviation = "SND"
                }, new Currency
                {
                    Id = 2745,
                    CurrencyTypeId = 2,
                    Name = "Joint Ventures",
                    Abbreviation = "JOINT"
                }, new Currency
                {
                    Id = 2974,
                    CurrencyTypeId = 2,
                    Name = "Lightpaycoin",
                    Abbreviation = "LPC"
                }, new Currency
                {
                    Id = 1678,
                    CurrencyTypeId = 2,
                    Name = "InsaneCoin",
                    Abbreviation = "INSN"
                }, new Currency
                {
                    Id = 2752,
                    CurrencyTypeId = 2,
                    Name = "Datarius Credit",
                    Abbreviation = "DTRC"
                }, new Currency
                {
                    Id = 2942,
                    CurrencyTypeId = 2,
                    Name = "Aegeus",
                    Abbreviation = "AEG"
                }, new Currency
                {
                    Id = 3068,
                    CurrencyTypeId = 2,
                    Name = "BitcoiNote",
                    Abbreviation = "BTCN"
                }, new Currency
                {
                    Id = 3086,
                    CurrencyTypeId = 2,
                    Name = "Kora Network Token",
                    Abbreviation = "KNT"
                }, new Currency
                {
                    Id = 2931,
                    CurrencyTypeId = 2,
                    Name = "Engagement Token",
                    Abbreviation = "ENGT"
                }, new Currency
                {
                    Id = 945,
                    CurrencyTypeId = 2,
                    Name = "Bata",
                    Abbreviation = "BTA"
                }, new Currency
                {
                    Id = 2565,
                    CurrencyTypeId = 2,
                    Name = "StarterCoin",
                    Abbreviation = "STAC"
                }, new Currency
                {
                    Id = 3021,
                    CurrencyTypeId = 2,
                    Name = "InternationalCryptoX",
                    Abbreviation = "INCX"
                }, new Currency
                {
                    Id = 3588,
                    CurrencyTypeId = 2,
                    Name = "Absolute",
                    Abbreviation = "ABS"
                }, new Currency
                {
                    Id = 3433,
                    CurrencyTypeId = 2,
                    Name = "EUNOMIA",
                    Abbreviation = "ENTS"
                }, new Currency
                {
                    Id = 3001,
                    CurrencyTypeId = 2,
                    Name = "KWHCoin",
                    Abbreviation = "KWH"
                }, new Currency
                {
                    Id = 3312,
                    CurrencyTypeId = 2,
                    Name = "Evimeria",
                    Abbreviation = "EVI"
                }, new Currency
                {
                    Id = 3798,
                    CurrencyTypeId = 2,
                    Name = "Xuez",
                    Abbreviation = "XUEZ"
                }, new Currency
                {
                    Id = 3048,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Turbo Koin",
                    Abbreviation = "BTK"
                }, new Currency
                {
                    Id = 1153,
                    CurrencyTypeId = 2,
                    Name = "Creditbit",
                    Abbreviation = "CRB"
                }, new Currency
                {
                    Id = 3685,
                    CurrencyTypeId = 2,
                    Name = "BTC Lite",
                    Abbreviation = "BTCL"
                }, new Currency
                {
                    Id = 3777,
                    CurrencyTypeId = 2,
                    Name = "Spectrum",
                    Abbreviation = "SPT"
                }, new Currency
                {
                    Id = 3629,
                    CurrencyTypeId = 2,
                    Name = "LRM Coin",
                    Abbreviation = "LRM"
                }, new Currency
                {
                    Id = 2977,
                    CurrencyTypeId = 2,
                    Name = "BitRewards",
                    Abbreviation = "BIT"
                }, new Currency
                {
                    Id = 2721,
                    CurrencyTypeId = 2,
                    Name = "APR Coin",
                    Abbreviation = "APR"
                }, new Currency
                {
                    Id = 3451,
                    CurrencyTypeId = 2,
                    Name = "BLOC.MONEY",
                    Abbreviation = "BLOC"
                }, new Currency
                {
                    Id = 1683,
                    CurrencyTypeId = 2,
                    Name = "RouletteToken",
                    Abbreviation = "RLT"
                }, new Currency
                {
                    Id = 3646,
                    CurrencyTypeId = 2,
                    Name = "Herbalist Token",
                    Abbreviation = "HERB"
                }, new Currency
                {
                    Id = 3189,
                    CurrencyTypeId = 2,
                    Name = "Mainstream For The Underground",
                    Abbreviation = "MFTU"
                }, new Currency
                {
                    Id = 3687,
                    CurrencyTypeId = 2,
                    Name = "BitBall",
                    Abbreviation = "BTB"
                }, new Currency
                {
                    Id = 3413,
                    CurrencyTypeId = 2,
                    Name = "nDEX",
                    Abbreviation = "NDX"
                }, new Currency
                {
                    Id = 3599,
                    CurrencyTypeId = 2,
                    Name = "EtherInc",
                    Abbreviation = "ETI"
                }, new Currency
                {
                    Id = 3919,
                    CurrencyTypeId = 2,
                    Name = "Doge Token",
                    Abbreviation = "DOGET"
                }, new Currency
                {
                    Id = 3678,
                    CurrencyTypeId = 2,
                    Name = "ICOBay",
                    Abbreviation = "IBT"
                }, new Currency
                {
                    Id = 3359,
                    CurrencyTypeId = 2,
                    Name = "WITChain",
                    Abbreviation = "WIT"
                }, new Currency
                {
                    Id = 3746,
                    CurrencyTypeId = 2,
                    Name = "electrumdark",
                    Abbreviation = "ELD"
                }, new Currency
                {
                    Id = 2932,
                    CurrencyTypeId = 2,
                    Name = "No BS Crypto",
                    Abbreviation = "NOBS"
                }, new Currency
                {
                    Id = 3796,
                    CurrencyTypeId = 2,
                    Name = "MESG",
                    Abbreviation = "MESG"
                }, new Currency
                {
                    Id = 2475,
                    CurrencyTypeId = 2,
                    Name = "Garlicoin",
                    Abbreviation = "GRLC"
                }, new Currency
                {
                    Id = 3181,
                    CurrencyTypeId = 2,
                    Name = "ShowHand",
                    Abbreviation = "HAND"
                }, new Currency
                {
                    Id = 3398,
                    CurrencyTypeId = 2,
                    Name = "SCRIV NETWORK",
                    Abbreviation = "SCRIV"
                }, new Currency
                {
                    Id = 3730,
                    CurrencyTypeId = 2,
                    Name = "The Currency Analytics",
                    Abbreviation = "TCAT"
                }, new Currency
                {
                    Id = 3771,
                    CurrencyTypeId = 2,
                    Name = "EthereumX",
                    Abbreviation = "ETX"
                }, new Currency
                {
                    Id = 3056,
                    CurrencyTypeId = 2,
                    Name = "Thore Cash",
                    Abbreviation = "TCH"
                }, new Currency
                {
                    Id = 2489,
                    CurrencyTypeId = 2,
                    Name = "BitWhite",
                    Abbreviation = "BTW"
                }, new Currency
                {
                    Id = 3935,
                    CurrencyTypeId = 2,
                    Name = "Sparkpoint",
                    Abbreviation = "SRK"
                }, new Currency
                {
                    Id = 1474,
                    CurrencyTypeId = 2,
                    Name = "Eternity",
                    Abbreviation = "ENT"
                }, new Currency
                {
                    Id = 3810,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Gold Project",
                    Abbreviation = "ETGP"
                }, new Currency
                {
                    Id = 2448,
                    CurrencyTypeId = 2,
                    Name = "SparksPay",
                    Abbreviation = "SPK"
                }, new Currency
                {
                    Id = 3889,
                    CurrencyTypeId = 2,
                    Name = "Natmin Pure Escrow",
                    Abbreviation = "NAT"
                }, new Currency
                {
                    Id = 3151,
                    CurrencyTypeId = 2,
                    Name = "Akroma",
                    Abbreviation = "AKA"
                }, new Currency
                {
                    Id = 3446,
                    CurrencyTypeId = 2,
                    Name = "Zenswap Network Token",
                    Abbreviation = "ZNT"
                }, new Currency
                {
                    Id = 2583,
                    CurrencyTypeId = 2,
                    Name = "Octoin Coin",
                    Abbreviation = "OCC"
                }, new Currency
                {
                    Id = 3338,
                    CurrencyTypeId = 2,
                    Name = "PAXEX",
                    Abbreviation = "PAXEX"
                }, new Currency
                {
                    Id = 3509,
                    CurrencyTypeId = 2,
                    Name = "Provoco Token",
                    Abbreviation = "VOCO"
                }, new Currency
                {
                    Id = 3476,
                    CurrencyTypeId = 2,
                    Name = "Italian Lira",
                    Abbreviation = "ITL"
                }, new Currency
                {
                    Id = 3184,
                    CurrencyTypeId = 2,
                    Name = "Gold Poker",
                    Abbreviation = "GPKR"
                }, new Currency
                {
                    Id = 2635,
                    CurrencyTypeId = 2,
                    Name = "TokenDesk",
                    Abbreviation = "TDS"
                }, new Currency
                {
                    Id = 3739,
                    CurrencyTypeId = 2,
                    Name = "Constant",
                    Abbreviation = "CONST"
                }, new Currency
                {
                    Id = 3270,
                    CurrencyTypeId = 2,
                    Name = "e-Chat",
                    Abbreviation = "ECHT"
                }, new Currency
                {
                    Id = 2147,
                    CurrencyTypeId = 2,
                    Name = "ELTCOIN",
                    Abbreviation = "ELTCOIN"
                }, new Currency
                {
                    Id = 3459,
                    CurrencyTypeId = 2,
                    Name = "GoHelpFund",
                    Abbreviation = "HELP"
                }, new Currency
                {
                    Id = 3501,
                    CurrencyTypeId = 2,
                    Name = "CryptoSoul",
                    Abbreviation = "SOUL"
                }, new Currency
                {
                    Id = 2386,
                    CurrencyTypeId = 2,
                    Name = "KZ Cash",
                    Abbreviation = "KZC"
                }, new Currency
                {
                    Id = 3484,
                    CurrencyTypeId = 2,
                    Name = "Waletoken",
                    Abbreviation = "WTN"
                }, new Currency
                {
                    Id = 2461,
                    CurrencyTypeId = 2,
                    Name = "Peerguess",
                    Abbreviation = "GUESS"
                }, new Currency
                {
                    Id = 3271,
                    CurrencyTypeId = 2,
                    Name = "Ether Kingdoms Token",
                    Abbreviation = "IMP"
                }, new Currency
                {
                    Id = 3460,
                    CurrencyTypeId = 2,
                    Name = "Bitcoinus",
                    Abbreviation = "BITS"
                }, new Currency
                {
                    Id = 3510,
                    CurrencyTypeId = 2,
                    Name = "Traid",
                    Abbreviation = "TRAID"
                }, new Currency
                {
                    Id = 3787,
                    CurrencyTypeId = 2,
                    Name = "InnovativeBioresearchClassic",
                    Abbreviation = "INNBCL"
                }, new Currency
                {
                    Id = 2148,
                    CurrencyTypeId = 2,
                    Name = "Desire",
                    Abbreviation = "DSR"
                }, new Currency
                {
                    Id = 3610,
                    CurrencyTypeId = 2,
                    Name = "Micromines",
                    Abbreviation = "MICRO"
                }, new Currency
                {
                    Id = 3172,
                    CurrencyTypeId = 2,
                    Name = "LogisCoin",
                    Abbreviation = "LGS"
                }, new Currency
                {
                    Id = 3841,
                    CurrencyTypeId = 2,
                    Name = "StellarPay",
                    Abbreviation = "XLB"
                }, new Currency
                {
                    Id = 2420,
                    CurrencyTypeId = 2,
                    Name = "Nitro",
                    Abbreviation = "NOX"
                }, new Currency
                {
                    Id = 3454,
                    CurrencyTypeId = 2,
                    Name = "Decentralized Asset Trading Platform",
                    Abbreviation = "DATP"
                }, new Currency
                {
                    Id = 3429,
                    CurrencyTypeId = 2,
                    Name = "Cyber Movie Chain",
                    Abbreviation = "CMCT"
                }, new Currency
                {
                    Id = 3255,
                    CurrencyTypeId = 2,
                    Name = "CyberMusic",
                    Abbreviation = "CYMT"
                }, new Currency
                {
                    Id = 3659,
                    CurrencyTypeId = 2,
                    Name = "QUINADS",
                    Abbreviation = "QUIN"
                }, new Currency
                {
                    Id = 3468,
                    CurrencyTypeId = 2,
                    Name = "Fivebalance",
                    Abbreviation = "FBN"
                }, new Currency
                {
                    Id = 3131,
                    CurrencyTypeId = 2,
                    Name = "Thingschain",
                    Abbreviation = "TIC"
                }, new Currency
                {
                    Id = 2965,
                    CurrencyTypeId = 2,
                    Name = "VikkyToken",
                    Abbreviation = "VIKKY"
                }, new Currency
                {
                    Id = 3770,
                    CurrencyTypeId = 2,
                    Name = "CustomContractNetwork",
                    Abbreviation = "CCN"
                }, new Currency
                {
                    Id = 1897,
                    CurrencyTypeId = 2,
                    Name = "Bolenum",
                    Abbreviation = "BLN"
                }, new Currency
                {
                    Id = 3583,
                    CurrencyTypeId = 2,
                    Name = "Posscoin",
                    Abbreviation = "POSS"
                }, new Currency
                {
                    Id = 3216,
                    CurrencyTypeId = 2,
                    Name = "DeltaChain",
                    Abbreviation = "DELTA"
                }, new Currency
                {
                    Id = 3521,
                    CurrencyTypeId = 2,
                    Name = "PAWS Fund",
                    Abbreviation = "PAWS"
                }, new Currency
                {
                    Id = 3265,
                    CurrencyTypeId = 2,
                    Name = "Havy",
                    Abbreviation = "HAVY"
                }, new Currency
                {
                    Id = 3397,
                    CurrencyTypeId = 2,
                    Name = "Neural Protocol",
                    Abbreviation = "NRP"
                }, new Currency
                {
                    Id = 2960,
                    CurrencyTypeId = 2,
                    Name = "Tourist Token",
                    Abbreviation = "TOTO"
                }, new Currency
                {
                    Id = 3162,
                    CurrencyTypeId = 2,
                    Name = "YoloCash",
                    Abbreviation = "YLC"
                }, new Currency
                {
                    Id = 3219,
                    CurrencyTypeId = 2,
                    Name = "FUTURAX",
                    Abbreviation = "FTXT"
                }, new Currency
                {
                    Id = 3254,
                    CurrencyTypeId = 2,
                    Name = "Mirai",
                    Abbreviation = "MRI"
                }, new Currency
                {
                    Id = 3512,
                    CurrencyTypeId = 2,
                    Name = "Alpha Coin",
                    Abbreviation = "APC"
                }, new Currency
                {
                    Id = 3374,
                    CurrencyTypeId = 2,
                    Name = "Ragnarok",
                    Abbreviation = "RAGNA"
                }, new Currency
                {
                    Id = 3740,
                    CurrencyTypeId = 2,
                    Name = "Blacer Coin",
                    Abbreviation = "BLCR"
                }, new Currency
                {
                    Id = 3769,
                    CurrencyTypeId = 2,
                    Name = "HashBX ",
                    Abbreviation = "HBX"
                }, new Currency
                {
                    Id = 1515,
                    CurrencyTypeId = 2,
                    Name = "iBank",
                    Abbreviation = "IBANK"
                }, new Currency
                {
                    Id = 3317,
                    CurrencyTypeId = 2,
                    Name = "Cryptrust",
                    Abbreviation = "CTRT"
                }, new Currency
                {
                    Id = 3263,
                    CurrencyTypeId = 2,
                    Name = "Dinero",
                    Abbreviation = "DIN"
                }, new Currency
                {
                    Id = 3222,
                    CurrencyTypeId = 2,
                    Name = "Bionic",
                    Abbreviation = "BNC"
                }, new Currency
                {
                    Id = 3444,
                    CurrencyTypeId = 2,
                    Name = "KUN",
                    Abbreviation = "KUN"
                }, new Currency
                {
                    Id = 3807,
                    CurrencyTypeId = 2,
                    Name = "LitecoinToken",
                    Abbreviation = "LTK"
                }, new Currency
                {
                    Id = 3804,
                    CurrencyTypeId = 2,
                    Name = "SpectrumNetwork",
                    Abbreviation = "SPEC"
                }, new Currency
                {
                    Id = 3931,
                    CurrencyTypeId = 2,
                    Name = "Elementeum",
                    Abbreviation = "ELET"
                }, new Currency
                {
                    Id = 3424,
                    CurrencyTypeId = 2,
                    Name = "QYNO",
                    Abbreviation = "QNO"
                }, new Currency
                {
                    Id = 3886,
                    CurrencyTypeId = 2,
                    Name = "ICOCalendar.Today",
                    Abbreviation = "ICT"
                }, new Currency
                {
                    Id = 3749,
                    CurrencyTypeId = 2,
                    Name = "IceChain",
                    Abbreviation = "ICHX"
                }, new Currency
                {
                    Id = 3287,
                    CurrencyTypeId = 2,
                    Name = "Abulaba",
                    Abbreviation = "AAA"
                }, new Currency
                {
                    Id = 3580,
                    CurrencyTypeId = 2,
                    Name = "Crystal Token",
                    Abbreviation = "CYL"
                }, new Currency
                {
                    Id = 1832,
                    CurrencyTypeId = 2,
                    Name = "HarmonyCoin",
                    Abbreviation = "HMC"
                }, new Currency
                {
                    Id = 67,
                    CurrencyTypeId = 2,
                    Name = "Unobtanium",
                    Abbreviation = "UNO"
                }, new Currency
                {
                    Id = 2585,
                    CurrencyTypeId = 2,
                    Name = "Centrality",
                    Abbreviation = "CENNZ"
                }, new Currency
                {
                    Id = 1408,
                    CurrencyTypeId = 2,
                    Name = "Iconomi",
                    Abbreviation = "ICN"
                }, new Currency
                {
                    Id = 2304,
                    CurrencyTypeId = 2,
                    Name = "DEW",
                    Abbreviation = "DEW"
                }, new Currency
                {
                    Id = 2371,
                    CurrencyTypeId = 2,
                    Name = "United Traders Token",
                    Abbreviation = "UTT"
                }, new Currency
                {
                    Id = 3225,
                    CurrencyTypeId = 2,
                    Name = "BitNewChain",
                    Abbreviation = "BTN"
                }, new Currency
                {
                    Id = 3354,
                    CurrencyTypeId = 2,
                    Name = "TRONCLASSIC",
                    Abbreviation = "TRXC"
                }, new Currency
                {
                    Id = 1782,
                    CurrencyTypeId = 2,
                    Name = "Ecobit",
                    Abbreviation = "ECOB"
                }, new Currency
                {
                    Id = 3072,
                    CurrencyTypeId = 2,
                    Name = "MassGrid",
                    Abbreviation = "MGD"
                }, new Currency
                {
                    Id = 2134,
                    CurrencyTypeId = 2,
                    Name = "Grid+",
                    Abbreviation = "GRID"
                }, new Currency
                {
                    Id = 212,
                    CurrencyTypeId = 2,
                    Name = "ECC",
                    Abbreviation = "ECC"
                }, new Currency
                {
                    Id = 2732,
                    CurrencyTypeId = 2,
                    Name = "Aston",
                    Abbreviation = "ATX"
                }, new Currency
                {
                    Id = 1454,
                    CurrencyTypeId = 2,
                    Name = "Lykke",
                    Abbreviation = "LKK"
                }, new Currency
                {
                    Id = 1963,
                    CurrencyTypeId = 2,
                    Name = "Credo",
                    Abbreviation = "CREDO"
                }, new Currency
                {
                    Id = 141,
                    CurrencyTypeId = 2,
                    Name = "MintCoin",
                    Abbreviation = "MINT"
                }, new Currency
                {
                    Id = 3608,
                    CurrencyTypeId = 2,
                    Name = "Howdoo",
                    Abbreviation = "UDOO"
                }, new Currency
                {
                    Id = 224,
                    CurrencyTypeId = 2,
                    Name = "FairCoin",
                    Abbreviation = "FAIR"
                }, new Currency
                {
                    Id = 3054,
                    CurrencyTypeId = 2,
                    Name = "DACSEE",
                    Abbreviation = "DACS"
                }, new Currency
                {
                    Id = 89,
                    CurrencyTypeId = 2,
                    Name = "Mooncoin",
                    Abbreviation = "MOON"
                }, new Currency
                {
                    Id = 2384,
                    CurrencyTypeId = 2,
                    Name = "Vezt",
                    Abbreviation = "VZT"
                }, new Currency
                {
                    Id = 1244,
                    CurrencyTypeId = 2,
                    Name = "HiCoin",
                    Abbreviation = "XHI"
                }, new Currency
                {
                    Id = 161,
                    CurrencyTypeId = 2,
                    Name = "Pandacoin",
                    Abbreviation = "PND"
                }, new Currency
                {
                    Id = 3407,
                    CurrencyTypeId = 2,
                    Name = "Ondori",
                    Abbreviation = "RSTR"
                }, new Currency
                {
                    Id = 666,
                    CurrencyTypeId = 2,
                    Name = "Aurum Coin",
                    Abbreviation = "AU"
                }, new Currency
                {
                    Id = 3605,
                    CurrencyTypeId = 2,
                    Name = "Vites",
                    Abbreviation = "VITES"
                }, new Currency
                {
                    Id = 3585,
                    CurrencyTypeId = 2,
                    Name = "WeShow Token",
                    Abbreviation = "WET"
                }, new Currency
                {
                    Id = 2867,
                    CurrencyTypeId = 2,
                    Name = "Bittwatt",
                    Abbreviation = "BWT"
                }, new Currency
                {
                    Id = 2607,
                    CurrencyTypeId = 2,
                    Name = "AMLT",
                    Abbreviation = "AMLT"
                }, new Currency
                {
                    Id = 2924,
                    CurrencyTypeId = 2,
                    Name = "FNKOS",
                    Abbreviation = "FNKOS"
                }, new Currency
                {
                    Id = 2722,
                    CurrencyTypeId = 2,
                    Name = "AC3",
                    Abbreviation = "AC3"
                }, new Currency
                {
                    Id = 3799,
                    CurrencyTypeId = 2,
                    Name = "SafeCoin",
                    Abbreviation = "SAFE"
                }, new Currency
                {
                    Id = 2881,
                    CurrencyTypeId = 2,
                    Name = "Distributed Credit Chain",
                    Abbreviation = "DCC"
                }, new Currency
                {
                    Id = 2875,
                    CurrencyTypeId = 2,
                    Name = "ALAX",
                    Abbreviation = "ALX"
                }, new Currency
                {
                    Id = 1721,
                    CurrencyTypeId = 2,
                    Name = "Mysterium",
                    Abbreviation = "MYST"
                }, new Currency
                {
                    Id = 1109,
                    CurrencyTypeId = 2,
                    Name = "Elite",
                    Abbreviation = "1337"
                }, new Currency
                {
                    Id = 298,
                    CurrencyTypeId = 2,
                    Name = "NewYorkCoin",
                    Abbreviation = "NYC"
                }, new Currency
                {
                    Id = 3092,
                    CurrencyTypeId = 2,
                    Name = "Nuggets",
                    Abbreviation = "NUG"
                }, new Currency
                {
                    Id = 2993,
                    CurrencyTypeId = 2,
                    Name = "HorusPay",
                    Abbreviation = "HORUS"
                }, new Currency
                {
                    Id = 1238,
                    CurrencyTypeId = 2,
                    Name = "Espers",
                    Abbreviation = "ESP"
                }, new Currency
                {
                    Id = 1819,
                    CurrencyTypeId = 2,
                    Name = "Starta",
                    Abbreviation = "STA"
                }, new Currency
                {
                    Id = 2211,
                    CurrencyTypeId = 2,
                    Name = "Bodhi",
                    Abbreviation = "BOT"
                }, new Currency
                {
                    Id = 3728,
                    CurrencyTypeId = 2,
                    Name = "Halo Platform",
                    Abbreviation = "HALO"
                }, new Currency
                {
                    Id = 3584,
                    CurrencyTypeId = 2,
                    Name = "TV-TWO",
                    Abbreviation = "TTV"
                }, new Currency
                {
                    Id = 2954,
                    CurrencyTypeId = 2,
                    Name = "wys Token",
                    Abbreviation = "WYS"
                }, new Currency
                {
                    Id = 2877,
                    CurrencyTypeId = 2,
                    Name = "Bodhi [ETH]",
                    Abbreviation = "BOE"
                }, new Currency
                {
                    Id = 2909,
                    CurrencyTypeId = 2,
                    Name = "LikeCoin",
                    Abbreviation = "LIKE"
                }, new Currency
                {
                    Id = 260,
                    CurrencyTypeId = 2,
                    Name = "PetroDollar",
                    Abbreviation = "XPD"
                }, new Currency
                {
                    Id = 2543,
                    CurrencyTypeId = 2,
                    Name = "COPYTRACK",
                    Abbreviation = "CPY"
                }, new Currency
                {
                    Id = 2031,
                    CurrencyTypeId = 2,
                    Name = "Hubii Network",
                    Abbreviation = "HBT"
                }, new Currency
                {
                    Id = 1595,
                    CurrencyTypeId = 2,
                    Name = "Soarcoin",
                    Abbreviation = "SOAR"
                }, new Currency
                {
                    Id = 2377,
                    CurrencyTypeId = 2,
                    Name = "Leverj",
                    Abbreviation = "LEV"
                }, new Currency
                {
                    Id = 2208,
                    CurrencyTypeId = 2,
                    Name = "EncrypGen",
                    Abbreviation = "DNA"
                }, new Currency
                {
                    Id = 3426,
                    CurrencyTypeId = 2,
                    Name = "Incodium",
                    Abbreviation = "INCO"
                }, new Currency
                {
                    Id = 1611,
                    CurrencyTypeId = 2,
                    Name = "DubaiCoin",
                    Abbreviation = "DBIX"
                }, new Currency
                {
                    Id = 2687,
                    CurrencyTypeId = 2,
                    Name = "Proxeus",
                    Abbreviation = "XES"
                }, new Currency
                {
                    Id = 3336,
                    CurrencyTypeId = 2,
                    Name = "IQeon",
                    Abbreviation = "IQN"
                }, new Currency
                {
                    Id = 1375,
                    CurrencyTypeId = 2,
                    Name = "Golfcoin",
                    Abbreviation = "GOLF"
                }, new Currency
                {
                    Id = 1968,
                    CurrencyTypeId = 2,
                    Name = "XPA",
                    Abbreviation = "XPA"
                }, new Currency
                {
                    Id = 2899,
                    CurrencyTypeId = 2,
                    Name = "Thrive Token",
                    Abbreviation = "THRT"
                }, new Currency
                {
                    Id = 3089,
                    CurrencyTypeId = 2,
                    Name = "AVINOC",
                    Abbreviation = "AVINOC"
                }, new Currency
                {
                    Id = 3274,
                    CurrencyTypeId = 2,
                    Name = "Carboneum [C8] Token",
                    Abbreviation = "C8"
                }, new Currency
                {
                    Id = 2166,
                    CurrencyTypeId = 2,
                    Name = "Ties.DB",
                    Abbreviation = "TIE"
                }, new Currency
                {
                    Id = 2030,
                    CurrencyTypeId = 2,
                    Name = "REAL",
                    Abbreviation = "REAL"
                }, new Currency
                {
                    Id = 1503,
                    CurrencyTypeId = 2,
                    Name = "Darcrus",
                    Abbreviation = "DAR"
                }, new Currency
                {
                    Id = 2260,
                    CurrencyTypeId = 2,
                    Name = "Bulwark",
                    Abbreviation = "BWK"
                }, new Currency
                {
                    Id = 1308,
                    CurrencyTypeId = 2,
                    Name = "HEAT",
                    Abbreviation = "HEAT"
                }, new Currency
                {
                    Id = 3471,
                    CurrencyTypeId = 2,
                    Name = "RoBET",
                    Abbreviation = "ROBET"
                }, new Currency
                {
                    Id = 53,
                    CurrencyTypeId = 2,
                    Name = "Quark",
                    Abbreviation = "QRK"
                }, new Currency
                {
                    Id = 80,
                    CurrencyTypeId = 2,
                    Name = "Orbitcoin",
                    Abbreviation = "ORB"
                }, new Currency
                {
                    Id = 3519,
                    CurrencyTypeId = 2,
                    Name = "Breezecoin",
                    Abbreviation = "BRZC"
                }, new Currency
                {
                    Id = 2374,
                    CurrencyTypeId = 2,
                    Name = "BitDegree",
                    Abbreviation = "BDG"
                }, new Currency
                {
                    Id = 168,
                    CurrencyTypeId = 2,
                    Name = "Uniform Fiscal Object",
                    Abbreviation = "UFO"
                }, new Currency
                {
                    Id = 2104,
                    CurrencyTypeId = 2,
                    Name = "iEthereum",
                    Abbreviation = "IETH"
                }, new Currency
                {
                    Id = 93,
                    CurrencyTypeId = 2,
                    Name = "42-coin",
                    Abbreviation = "42"
                }, new Currency
                {
                    Id = 1967,
                    CurrencyTypeId = 2,
                    Name = "Indorse Token",
                    Abbreviation = "IND"
                }, new Currency
                {
                    Id = 2378,
                    CurrencyTypeId = 2,
                    Name = "Karma",
                    Abbreviation = "KRM"
                },
                new Currency
                {
                    Id = 39,
                    CurrencyTypeId = 2,
                    Name = "Terracoin",
                    Abbreviation = "TRC"
                },
                new Currency
                {
                    Id = 234,
                    CurrencyTypeId = 2,
                    Name = "e-Gulden",
                    Abbreviation = "EFL"
                },
                new Currency
                {
                    Id = 3161,
                    CurrencyTypeId = 2,
                    Name = "savedroid",
                    Abbreviation = "SVD"
                }, new Currency
                {
                    Id = 1988,
                    CurrencyTypeId = 2,
                    Name = "Lampix",
                    Abbreviation = "PIX"
                }, new Currency
                {
                    Id = 3492,
                    CurrencyTypeId = 2,
                    Name = "Vetri",
                    Abbreviation = "VLD"
                }, new Currency
                {
                    Id = 2589,
                    CurrencyTypeId = 2,
                    Name = "Guaranteed Ethurance Token Extra",
                    Abbreviation = "GETX"
                }, new Currency
                {
                    Id = 128,
                    CurrencyTypeId = 2,
                    Name = "Maxcoin",
                    Abbreviation = "MAX"
                }, new Currency
                {
                    Id = 3406,
                    CurrencyTypeId = 2,
                    Name = "Block-Chain.com",
                    Abbreviation = "BC"
                }, new Currency
                {
                    Id = 2368,
                    CurrencyTypeId = 2,
                    Name = "REBL",
                    Abbreviation = "REBL"
                }, new Currency
                {
                    Id = 1686,
                    CurrencyTypeId = 2,
                    Name = "EquiTrader",
                    Abbreviation = "EQT"
                }, new Currency
                {
                    Id = 2872,
                    CurrencyTypeId = 2,
                    Name = "EnergiToken",
                    Abbreviation = "ETK"
                }, new Currency
                {
                    Id = 2050,
                    CurrencyTypeId = 2,
                    Name = "Swisscoin",
                    Abbreviation = "SIC"
                }, new Currency
                {
                    Id = 1019,
                    CurrencyTypeId = 2,
                    Name = "Manna",
                    Abbreviation = "MANNA"
                }, new Currency
                {
                    Id = 3592,
                    CurrencyTypeId = 2,
                    Name = "Coin Lion",
                    Abbreviation = "LION"
                }, new Currency
                {
                    Id = 3487,
                    CurrencyTypeId = 2,
                    Name = "Pedity",
                    Abbreviation = "PEDI"
                }, new Currency
                {
                    Id = 3706,
                    CurrencyTypeId = 2,
                    Name = "ALBOS",
                    Abbreviation = "ALB"
                }, new Currency
                {
                    Id = 2012,
                    CurrencyTypeId = 2,
                    Name = "Voise",
                    Abbreviation = "VOISE"
                }, new Currency
                {
                    Id = 2333,
                    CurrencyTypeId = 2,
                    Name = "FidentiaX",
                    Abbreviation = "FDX"
                }, new Currency
                {
                    Id = 3611,
                    CurrencyTypeId = 2,
                    Name = "Noir",
                    Abbreviation = "NOR"
                }, new Currency
                {
                    Id = 2854,
                    CurrencyTypeId = 2,
                    Name = "PikcioChain",
                    Abbreviation = "PKC"
                }, new Currency
                {
                    Id = 2513,
                    CurrencyTypeId = 2,
                    Name = "GoldMint",
                    Abbreviation = "MNTP"
                }, new Currency
                {
                    Id = 1714,
                    CurrencyTypeId = 2,
                    Name = "EncryptoTel [WAVES]",
                    Abbreviation = "ETT"
                }, new Currency
                {
                    Id = 1480,
                    CurrencyTypeId = 2,
                    Name = "Golos",
                    Abbreviation = "GOLOS"
                }, new Currency
                {
                    Id = 799,
                    CurrencyTypeId = 2,
                    Name = "SmileyCoin",
                    Abbreviation = "SMLY"
                }, new Currency
                {
                    Id = 2701,
                    CurrencyTypeId = 2,
                    Name = "TrustNote",
                    Abbreviation = "TTT"
                }, new Currency
                {
                    Id = 2833,
                    CurrencyTypeId = 2,
                    Name = "Ivy",
                    Abbreviation = "IVY"
                }, new Currency
                {
                    Id = 2753,
                    CurrencyTypeId = 2,
                    Name = "Colu Local Network",
                    Abbreviation = "CLN"
                }, new Currency
                {
                    Id = 1510,
                    CurrencyTypeId = 2,
                    Name = "CryptoCarbon",
                    Abbreviation = "CCRB"
                }, new Currency
                {
                    Id = 2352,
                    CurrencyTypeId = 2,
                    Name = "Coinlancer",
                    Abbreviation = "CL"
                }, new Currency
                {
                    Id = 1962,
                    CurrencyTypeId = 2,
                    Name = "BuzzCoin",
                    Abbreviation = "BUZZ"
                }, new Currency
                {
                    Id = 205,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Scrypt",
                    Abbreviation = "BTCS"
                }, new Currency
                {
                    Id = 148,
                    CurrencyTypeId = 2,
                    Name = "Auroracoin",
                    Abbreviation = "AUR"
                }, new Currency
                {
                    Id = 2425,
                    CurrencyTypeId = 2,
                    Name = "Global Awards Token",
                    Abbreviation = "GAT"
                }, new Currency
                {
                    Id = 29,
                    CurrencyTypeId = 2,
                    Name = "WorldCoin",
                    Abbreviation = "WDC"
                }, new Currency
                {
                    Id = 1931,
                    CurrencyTypeId = 2,
                    Name = "Opus",
                    Abbreviation = "OPT"
                }, new Currency
                {
                    Id = 1123,
                    CurrencyTypeId = 2,
                    Name = "OBITS",
                    Abbreviation = "OBITS"
                }, new Currency
                {
                    Id = 2555,
                    CurrencyTypeId = 2,
                    Name = "Sether",
                    Abbreviation = "SETH"
                }, new Currency
                {
                    Id = 1852,
                    CurrencyTypeId = 2,
                    Name = "KekCoin",
                    Abbreviation = "KEK"
                }, new Currency
                {
                    Id = 360,
                    CurrencyTypeId = 2,
                    Name = "Motocoin",
                    Abbreviation = "MOTO"
                }, new Currency
                {
                    Id = 1917,
                    CurrencyTypeId = 2,
                    Name = "bitqy",
                    Abbreviation = "BQ"
                }, new Currency
                {
                    Id = 506,
                    CurrencyTypeId = 2,
                    Name = "CannabisCoin",
                    Abbreviation = "CANN"
                }, new Currency
                {
                    Id = 2422,
                    CurrencyTypeId = 2,
                    Name = "IDEX Membership",
                    Abbreviation = "IDXM"
                }, new Currency
                {
                    Id = 3291,
                    CurrencyTypeId = 2,
                    Name = "Bettex Coin",
                    Abbreviation = "BTXC"
                }, new Currency
                {
                    Id = 948,
                    CurrencyTypeId = 2,
                    Name = "AudioCoin",
                    Abbreviation = "ADC"
                }, new Currency
                {
                    Id = 1995,
                    CurrencyTypeId = 2,
                    Name = "Target Coin",
                    Abbreviation = "TGT"
                }, new Currency
                {
                    Id = 3003,
                    CurrencyTypeId = 2,
                    Name = "White Standard",
                    Abbreviation = "WSD"
                }, new Currency
                {
                    Id = 525,
                    CurrencyTypeId = 2,
                    Name = "HyperStake",
                    Abbreviation = "HYP"
                }, new Currency
                {
                    Id = 1466,
                    CurrencyTypeId = 2,
                    Name = "Hush",
                    Abbreviation = "HUSH"
                }, new Currency
                {
                    Id = 1915,
                    CurrencyTypeId = 2,
                    Name = "AdCoin",
                    Abbreviation = "ACC"
                }, new Currency
                {
                    Id = 2436,
                    CurrencyTypeId = 2,
                    Name = "RefToken",
                    Abbreviation = "REF"
                }, new Currency
                {
                    Id = 502,
                    CurrencyTypeId = 2,
                    Name = "Carboncoin",
                    Abbreviation = "CARBON"
                }, new Currency
                {
                    Id = 2521,
                    CurrencyTypeId = 2,
                    Name = "BioCoin",
                    Abbreviation = "BIO"
                }, new Currency
                {
                    Id = 199,
                    CurrencyTypeId = 2,
                    Name = "Skeincoin",
                    Abbreviation = "SKC"
                }, new Currency
                {
                    Id = 3465,
                    CurrencyTypeId = 2,
                    Name = "Alt.Estate token",
                    Abbreviation = "ALT"
                }, new Currency
                {
                    Id = 1699,
                    CurrencyTypeId = 2,
                    Name = "Ethbits",
                    Abbreviation = "ETBS"
                }, new Currency
                {
                    Id = 2703,
                    CurrencyTypeId = 2,
                    Name = "BetterBetting",
                    Abbreviation = "BETR"
                }, new Currency
                {
                    Id = 3755,
                    CurrencyTypeId = 2,
                    Name = "Moneynet",
                    Abbreviation = "MNC"
                }, new Currency
                {
                    Id = 501,
                    CurrencyTypeId = 2,
                    Name = "Cryptonite",
                    Abbreviation = "XCN"
                }, new Currency
                {
                    Id = 629,
                    CurrencyTypeId = 2,
                    Name = "Magi",
                    Abbreviation = "XMG"
                }, new Currency
                {
                    Id = 2980,
                    CurrencyTypeId = 2,
                    Name = "WABnetwork",
                    Abbreviation = "WAB"
                }, new Currency
                {
                    Id = 2598,
                    CurrencyTypeId = 2,
                    Name = "Banyan Network",
                    Abbreviation = "BBN"
                }, new Currency
                {
                    Id = 3078,
                    CurrencyTypeId = 2,
                    Name = "Kind Ads Token",
                    Abbreviation = "KIND"
                }, new Currency
                {
                    Id = 2672,
                    CurrencyTypeId = 2,
                    Name = "SRCOIN",
                    Abbreviation = "SRCOIN"
                }, new Currency
                {
                    Id = 32,
                    CurrencyTypeId = 2,
                    Name = "Freicoin",
                    Abbreviation = "FRC"
                }, new Currency
                {
                    Id = 853,
                    CurrencyTypeId = 2,
                    Name = "LiteDoge",
                    Abbreviation = "LDOGE"
                }, new Currency
                {
                    Id = 3241,
                    CurrencyTypeId = 2,
                    Name = "Knoxstertoken",
                    Abbreviation = "FKX"
                }, new Currency
                {
                    Id = 2255,
                    CurrencyTypeId = 2,
                    Name = "Social Send",
                    Abbreviation = "SEND"
                }, new Currency
                {
                    Id = 2926,
                    CurrencyTypeId = 2,
                    Name = "PRASM",
                    Abbreviation = "PSM"
                }, new Currency
                {
                    Id = 2015,
                    CurrencyTypeId = 2,
                    Name = "ATMChain",
                    Abbreviation = "ATM"
                }, new Currency
                {
                    Id = 175,
                    CurrencyTypeId = 2,
                    Name = "Photon",
                    Abbreviation = "PHO"
                }, new Currency
                {
                    Id = 2491,
                    CurrencyTypeId = 2,
                    Name = "Travelflex",
                    Abbreviation = "TRF"
                }, new Currency
                {
                    Id = 1882,
                    CurrencyTypeId = 2,
                    Name = "BlockCAT",
                    Abbreviation = "CAT"
                }, new Currency
                {
                    Id = 2628,
                    CurrencyTypeId = 2,
                    Name = "Rentberry",
                    Abbreviation = "BERRY"
                }, new Currency
                {
                    Id = 30,
                    CurrencyTypeId = 2,
                    Name = "BitBar",
                    Abbreviation = "BTB"
                }, new Currency
                {
                    Id = 3700,
                    CurrencyTypeId = 2,
                    Name = "Centauri",
                    Abbreviation = "CTX"
                }, new Currency
                {
                    Id = 1961,
                    CurrencyTypeId = 2,
                    Name = "imbrex",
                    Abbreviation = "REX"
                }, new Currency
                {
                    Id = 1969,
                    CurrencyTypeId = 2,
                    Name = "Sociall",
                    Abbreviation = "SCL"
                }, new Currency
                {
                    Id = 990,
                    CurrencyTypeId = 2,
                    Name = "Bitzeny",
                    Abbreviation = "ZNY"
                }, new Currency
                {
                    Id = 275,
                    CurrencyTypeId = 2,
                    Name = "PopularCoin",
                    Abbreviation = "POP"
                }, new Currency
                {
                    Id = 2625,
                    CurrencyTypeId = 2,
                    Name = "VIT",
                    Abbreviation = "VIT"
                }, new Currency
                {
                    Id = 2334,
                    CurrencyTypeId = 2,
                    Name = "BitClave",
                    Abbreviation = "CAT"
                }, new Currency
                {
                    Id = 38,
                    CurrencyTypeId = 2,
                    Name = "Megacoin",
                    Abbreviation = "MEC"
                }, new Currency
                {
                    Id = 3206,
                    CurrencyTypeId = 2,
                    Name = "Mithril Ore",
                    Abbreviation = "MORE"
                }, new Currency
                {
                    Id = 1495,
                    CurrencyTypeId = 2,
                    Name = "PoSW Coin",
                    Abbreviation = "POSW"
                }, new Currency
                {
                    Id = 3467,
                    CurrencyTypeId = 2,
                    Name = "Helium",
                    Abbreviation = "HLM"
                }, new Currency
                {
                    Id = 1833,
                    CurrencyTypeId = 2,
                    Name = "ToaCoin",
                    Abbreviation = "TOA"
                }, new Currency
                {
                    Id = 31,
                    CurrencyTypeId = 2,
                    Name = "Ixcoin",
                    Abbreviation = "IXC"
                }, new Currency
                {
                    Id = 2617,
                    CurrencyTypeId = 2,
                    Name = "IP Exchange",
                    Abbreviation = "IPSX"
                }, new Currency
                {
                    Id = 2372,
                    CurrencyTypeId = 2,
                    Name = "CDX Network",
                    Abbreviation = "CDX"
                }, new Currency
                {
                    Id = 3047,
                    CurrencyTypeId = 2,
                    Name = "UltraNote Coin",
                    Abbreviation = "XUN"
                }, new Currency
                {
                    Id = 3335,
                    CurrencyTypeId = 2,
                    Name = "Shard",
                    Abbreviation = "SHARD"
                }, new Currency
                {
                    Id = 1878,
                    CurrencyTypeId = 2,
                    Name = "Shadow Token",
                    Abbreviation = "SHDW"
                }, new Currency
                {
                    Id = 3751,
                    CurrencyTypeId = 2,
                    Name = "Stakinglab",
                    Abbreviation = "LABX"
                }, new Currency
                {
                    Id = 151,
                    CurrencyTypeId = 2,
                    Name = "Pesetacoin",
                    Abbreviation = "PTC"
                }, new Currency
                {
                    Id = 1141,
                    CurrencyTypeId = 2,
                    Name = "Moin",
                    Abbreviation = "MOIN"
                }, new Currency
                {
                    Id = 3370,
                    CurrencyTypeId = 2,
                    Name = "Nerves",
                    Abbreviation = "NER"
                }, new Currency
                {
                    Id = 43,
                    CurrencyTypeId = 2,
                    Name = "Anoncoin",
                    Abbreviation = "ANC"
                }, new Currency
                {
                    Id = 2616,
                    CurrencyTypeId = 2,
                    Name = "Stipend",
                    Abbreviation = "SPD"
                }, new Currency
                {
                    Id = 2523,
                    CurrencyTypeId = 2,
                    Name = "Tigereum",
                    Abbreviation = "TIG"
                }, new Currency
                {
                    Id = 35,
                    CurrencyTypeId = 2,
                    Name = "Argentum",
                    Abbreviation = "ARG"
                }, new Currency
                {
                    Id = 3180,
                    CurrencyTypeId = 2,
                    Name = "Compound Coin",
                    Abbreviation = "COMP"
                }, new Currency
                {
                    Id = 1605,
                    CurrencyTypeId = 2,
                    Name = "Universe",
                    Abbreviation = "UNI"
                }, new Currency
                {
                    Id = 3361,
                    CurrencyTypeId = 2,
                    Name = "Webchain",
                    Abbreviation = "WEB"
                }, new Currency
                {
                    Id = 3933,
                    CurrencyTypeId = 2,
                    Name = "SwiftCash",
                    Abbreviation = "SWIFT"
                }, new Currency
                {
                    Id = 1279,
                    CurrencyTypeId = 2,
                    Name = "PWR Coin",
                    Abbreviation = "PWR"
                }, new Currency
                {
                    Id = 594,
                    CurrencyTypeId = 2,
                    Name = "BunnyCoin",
                    Abbreviation = "BUN"
                }, new Currency
                {
                    Id = 1195,
                    CurrencyTypeId = 2,
                    Name = "HOdlcoin",
                    Abbreviation = "HODL"
                }, new Currency
                {
                    Id = 3633,
                    CurrencyTypeId = 2,
                    Name = "BitGuild PLAT",
                    Abbreviation = "PLAT"
                }, new Currency
                {
                    Id = 3045,
                    CurrencyTypeId = 2,
                    Name = "OPCoinX",
                    Abbreviation = "OPCX"
                }, new Currency
                {
                    Id = 894,
                    CurrencyTypeId = 2,
                    Name = "Neutron",
                    Abbreviation = "NTRN"
                }, new Currency
                {
                    Id = 1629,
                    CurrencyTypeId = 2,
                    Name = "Zennies",
                    Abbreviation = "ZENI"
                }, new Currency
                {
                    Id = 120,
                    CurrencyTypeId = 2,
                    Name = "Nyancoin",
                    Abbreviation = "NYAN"
                }, new Currency
                {
                    Id = 1066,
                    CurrencyTypeId = 2,
                    Name = "Pakcoin",
                    Abbreviation = "PAK"
                }, new Currency
                {
                    Id = 290,
                    CurrencyTypeId = 2,
                    Name = "BlueCoin",
                    Abbreviation = "BLU"
                }, new Currency
                {
                    Id = 1299,
                    CurrencyTypeId = 2,
                    Name = "PutinCoin",
                    Abbreviation = "PUT"
                }, new Currency
                {
                    Id = 1522,
                    CurrencyTypeId = 2,
                    Name = "FirstCoin",
                    Abbreviation = "FRST"
                }, new Currency
                {
                    Id = 2622,
                    CurrencyTypeId = 2,
                    Name = "ClearCoin",
                    Abbreviation = "XCLR"
                }, new Currency
                {
                    Id = 644,
                    CurrencyTypeId = 2,
                    Name = "GlobalBoost-Y",
                    Abbreviation = "BSTY"
                }, new Currency
                {
                    Id = 389,
                    CurrencyTypeId = 2,
                    Name = "Startcoin",
                    Abbreviation = "START"
                }, new Currency
                {
                    Id = 2005,
                    CurrencyTypeId = 2,
                    Name = "Obsidian",
                    Abbreviation = "ODN"
                }, new Currency
                {
                    Id = 1175,
                    CurrencyTypeId = 2,
                    Name = "Rubies",
                    Abbreviation = "RBIES"
                }, new Currency
                {
                    Id = 2612,
                    CurrencyTypeId = 2,
                    Name = "BitRent",
                    Abbreviation = "RNTB"
                }, new Currency
                {
                    Id = 2288,
                    CurrencyTypeId = 2,
                    Name = "Worldcore",
                    Abbreviation = "WRC"
                }, new Currency
                {
                    Id = 2664,
                    CurrencyTypeId = 2,
                    Name = "CryCash",
                    Abbreviation = "CRC"
                }, new Currency
                {
                    Id = 3621,
                    CurrencyTypeId = 2,
                    Name = "BitNautic Token",
                    Abbreviation = "BTNT"
                }, new Currency
                {
                    Id = 2695,
                    CurrencyTypeId = 2,
                    Name = "VeriME",
                    Abbreviation = "VME"
                }, new Currency
                {
                    Id = 1951,
                    CurrencyTypeId = 2,
                    Name = "Vsync",
                    Abbreviation = "VSX"
                }, new Currency
                {
                    Id = 3006,
                    CurrencyTypeId = 2,
                    Name = "Niobio Cash",
                    Abbreviation = "NBR"
                }, new Currency
                {
                    Id = 1582,
                    CurrencyTypeId = 2,
                    Name = "Netko",
                    Abbreviation = "NETKO"
                }, new Currency
                {
                    Id = 719,
                    CurrencyTypeId = 2,
                    Name = "TittieCoin",
                    Abbreviation = "TIT"
                }, new Currency
                {
                    Id = 911,
                    CurrencyTypeId = 2,
                    Name = "Advanced Internet Blocks",
                    Abbreviation = "AIB"
                }, new Currency
                {
                    Id = 3480,
                    CurrencyTypeId = 2,
                    Name = "StrongHands Masternode",
                    Abbreviation = "SHMN"
                }, new Currency
                {
                    Id = 181,
                    CurrencyTypeId = 2,
                    Name = "Zeitcoin",
                    Abbreviation = "ZEIT"
                }, new Currency
                {
                    Id = 654,
                    CurrencyTypeId = 2,
                    Name = "DigitalPrice",
                    Abbreviation = "DP"
                }, new Currency
                {
                    Id = 1247,
                    CurrencyTypeId = 2,
                    Name = "AquariusCoin",
                    Abbreviation = "ARCO"
                }, new Currency
                {
                    Id = 2411,
                    CurrencyTypeId = 2,
                    Name = "Galactrum",
                    Abbreviation = "ORE"
                }, new Currency
                {
                    Id = 638,
                    CurrencyTypeId = 2,
                    Name = "Trollcoin",
                    Abbreviation = "TROLL"
                }, new Currency
                {
                    Id = 3603,
                    CurrencyTypeId = 2,
                    Name = "Menlo One",
                    Abbreviation = "ONE"
                }, new Currency
                {
                    Id = 2683,
                    CurrencyTypeId = 2,
                    Name = "TrakInvest",
                    Abbreviation = "TRAK"
                }, new Currency
                {
                    Id = 3436,
                    CurrencyTypeId = 2,
                    Name = "SIMDAQ",
                    Abbreviation = "SMQ"
                }, new Currency
                {
                    Id = 1004,
                    CurrencyTypeId = 2,
                    Name = "Helleniccoin",
                    Abbreviation = "HNC"
                }, new Currency
                {
                    Id = 61,
                    CurrencyTypeId = 2,
                    Name = "TagCoin",
                    Abbreviation = "TAG"
                }, new Currency
                {
                    Id = 2768,
                    CurrencyTypeId = 2,
                    Name = "Fabric Token",
                    Abbreviation = "FT"
                }, new Currency
                {
                    Id = 3422,
                    CurrencyTypeId = 2,
                    Name = "SHPING",
                    Abbreviation = "SHPING"
                }, new Currency
                {
                    Id = 2056,
                    CurrencyTypeId = 2,
                    Name = "PiplCoin",
                    Abbreviation = "PIPL"
                }, new Currency
                {
                    Id = 2065,
                    CurrencyTypeId = 2,
                    Name = "XGOX",
                    Abbreviation = "XGOX"
                }, new Currency
                {
                    Id = 3511,
                    CurrencyTypeId = 2,
                    Name = "Bitibu Coin",
                    Abbreviation = "BTB"
                }, new Currency
                {
                    Id = 3348,
                    CurrencyTypeId = 2,
                    Name = "MNPCoin",
                    Abbreviation = "MNP"
                }, new Currency
                {
                    Id = 2230,
                    CurrencyTypeId = 2,
                    Name = "Monkey Project",
                    Abbreviation = "MONK"
                }, new Currency
                {
                    Id = 3488,
                    CurrencyTypeId = 2,
                    Name = "Gravity",
                    Abbreviation = "GZRO"
                }, new Currency
                {
                    Id = 625,
                    CurrencyTypeId = 2,
                    Name = "bitBTC",
                    Abbreviation = "BITBTC"
                }, new Currency
                {
                    Id = 1731,
                    CurrencyTypeId = 2,
                    Name = "GlobalToken",
                    Abbreviation = "GLT"
                }, new Currency
                {
                    Id = 3387,
                    CurrencyTypeId = 2,
                    Name = "BLAST",
                    Abbreviation = "BLAST"
                }, new Currency
                {
                    Id = 2218,
                    CurrencyTypeId = 2,
                    Name = "Magnet",
                    Abbreviation = "MAG"
                }, new Currency
                {
                    Id = 1799,
                    CurrencyTypeId = 2,
                    Name = "Rupee",
                    Abbreviation = "RUP"
                }, new Currency
                {
                    Id = 1257,
                    CurrencyTypeId = 2,
                    Name = "LanaCoin",
                    Abbreviation = "LANA"
                }, new Currency
                {
                    Id = 3472,
                    CurrencyTypeId = 2,
                    Name = "JSECOIN",
                    Abbreviation = "JSE"
                }, new Currency
                {
                    Id = 1185,
                    CurrencyTypeId = 2,
                    Name = "TrumpCoin",
                    Abbreviation = "TRUMP"
                }, new Currency
                {
                    Id = 2464,
                    CurrencyTypeId = 2,
                    Name = "Devery",
                    Abbreviation = "EVE"
                }, new Currency
                {
                    Id = 1434,
                    CurrencyTypeId = 2,
                    Name = "Advanced Technology Coin",
                    Abbreviation = "ARC"
                }, new Currency
                {
                    Id = 1439,
                    CurrencyTypeId = 2,
                    Name = "AllSafe",
                    Abbreviation = "ASAFE"
                }, new Currency
                {
                    Id = 3262,
                    CurrencyTypeId = 2,
                    Name = "CYCLEAN",
                    Abbreviation = "CCL"
                }, new Currency
                {
                    Id = 2531,
                    CurrencyTypeId = 2,
                    Name = "W3Coin",
                    Abbreviation = "W3C"
                }, new Currency
                {
                    Id = 2910,
                    CurrencyTypeId = 2,
                    Name = "Crowdholding",
                    Abbreviation = "YUP"
                }, new Currency
                {
                    Id = 2749,
                    CurrencyTypeId = 2,
                    Name = "Signals Network",
                    Abbreviation = "SGN"
                }, new Currency
                {
                    Id = 2779,
                    CurrencyTypeId = 2,
                    Name = "Level Up Coin",
                    Abbreviation = "LUC"
                }, new Currency
                {
                    Id = 276,
                    CurrencyTypeId = 2,
                    Name = "Bitstar",
                    Abbreviation = "BITS"
                }, new Currency
                {
                    Id = 1777,
                    CurrencyTypeId = 2,
                    Name = "CryptoPing",
                    Abbreviation = "PING"
                }, new Currency
                {
                    Id = 3479,
                    CurrencyTypeId = 2,
                    Name = "MODEL-X-coin",
                    Abbreviation = "MODX"
                }, new Currency
                {
                    Id = 960,
                    CurrencyTypeId = 2,
                    Name = "FujiCoin",
                    Abbreviation = "FJC"
                }, new Currency
                {
                    Id = 3431,
                    CurrencyTypeId = 2,
                    Name = "Iconiq Lab Token",
                    Abbreviation = "ICNQ"
                }, new Currency
                {
                    Id = 3159,
                    CurrencyTypeId = 2,
                    Name = "Apollon",
                    Abbreviation = "XAP"
                }, new Currency
                {
                    Id = 1725,
                    CurrencyTypeId = 2,
                    Name = "Adelphoi",
                    Abbreviation = "ADL"
                }, new Currency
                {
                    Id = 1148,
                    CurrencyTypeId = 2,
                    Name = "EverGreenCoin",
                    Abbreviation = "EGC"
                }, new Currency
                {
                    Id = 764,
                    CurrencyTypeId = 2,
                    Name = "PayCoin",
                    Abbreviation = "XPY"
                }, new Currency
                {
                    Id = 50,
                    CurrencyTypeId = 2,
                    Name = "Emerald Crypto",
                    Abbreviation = "EMD"
                }, new Currency
                {
                    Id = 2332,
                    CurrencyTypeId = 2,
                    Name = "STRAKS",
                    Abbreviation = "STAK"
                }, new Currency
                {
                    Id = 1754,
                    CurrencyTypeId = 2,
                    Name = "Bitradio",
                    Abbreviation = "BRO"
                }, new Currency
                {
                    Id = 813,
                    CurrencyTypeId = 2,
                    Name = "bitSilver",
                    Abbreviation = "BITSILVER"
                }, new Currency
                {
                    Id = 2520,
                    CurrencyTypeId = 2,
                    Name = "Jesus Coin",
                    Abbreviation = "JC"
                }, new Currency
                {
                    Id = 2518,
                    CurrencyTypeId = 2,
                    Name = "LOCIcoin",
                    Abbreviation = "LOCI"
                }, new Currency
                {
                    Id = 1603,
                    CurrencyTypeId = 2,
                    Name = "Databits",
                    Abbreviation = "DTB"
                }, new Currency
                {
                    Id = 1251,
                    CurrencyTypeId = 2,
                    Name = "SixEleven",
                    Abbreviation = "611"
                }, new Currency
                {
                    Id = 3495,
                    CurrencyTypeId = 2,
                    Name = "ModulTrade",
                    Abbreviation = "MTRC"
                }, new Currency
                {
                    Id = 815,
                    CurrencyTypeId = 2,
                    Name = "Kobocoin",
                    Abbreviation = "KOBO"
                }, new Currency
                {
                    Id = 1702,
                    CurrencyTypeId = 2,
                    Name = "Version",
                    Abbreviation = "V"
                }, new Currency
                {
                    Id = 1120,
                    CurrencyTypeId = 2,
                    Name = "DraftCoin",
                    Abbreviation = "DFT"
                }, new Currency
                {
                    Id = 1790,
                    CurrencyTypeId = 2,
                    Name = "WomenCoin",
                    Abbreviation = "WOMEN"
                }, new Currency
                {
                    Id = 951,
                    CurrencyTypeId = 2,
                    Name = "Synergy",
                    Abbreviation = "SNRG"
                }, new Currency
                {
                    Id = 1481,
                    CurrencyTypeId = 2,
                    Name = "Nexium",
                    Abbreviation = "NXC"
                }, new Currency
                {
                    Id = 1643,
                    CurrencyTypeId = 2,
                    Name = "WavesGo",
                    Abbreviation = "WGO"
                }, new Currency
                {
                    Id = 3043,
                    CurrencyTypeId = 2,
                    Name = "PitisCoin",
                    Abbreviation = "PTS"
                }, new Currency
                {
                    Id = 1985,
                    CurrencyTypeId = 2,
                    Name = "Chronologic",
                    Abbreviation = "DAY"
                }, new Currency
                {
                    Id = 1724,
                    CurrencyTypeId = 2,
                    Name = "Linx",
                    Abbreviation = "LINX"
                }, new Currency
                {
                    Id = 2103,
                    CurrencyTypeId = 2,
                    Name = "Intelligent Trading Foundation",
                    Abbreviation = "ITT"
                }, new Currency
                {
                    Id = 3091,
                    CurrencyTypeId = 2,
                    Name = "Sapien",
                    Abbreviation = "SPN"
                }, new Currency
                {
                    Id = 1381,
                    CurrencyTypeId = 2,
                    Name = "Bitcloud",
                    Abbreviation = "BTDX"
                }, new Currency
                {
                    Id = 2935,
                    CurrencyTypeId = 2,
                    Name = "CDMCOIN",
                    Abbreviation = "CDM"
                }, new Currency
                {
                    Id = 1722,
                    CurrencyTypeId = 2,
                    Name = "More Coin",
                    Abbreviation = "MORE"
                }, new Currency
                {
                    Id = 72,
                    CurrencyTypeId = 2,
                    Name = "Deutsche eMark",
                    Abbreviation = "DEM"
                }, new Currency
                {
                    Id = 1752,
                    CurrencyTypeId = 2,
                    Name = "Goodomy",
                    Abbreviation = "GOOD"
                }, new Currency
                {
                    Id = 2196,
                    CurrencyTypeId = 2,
                    Name = "Sugar Exchange",
                    Abbreviation = "SGR"
                }, new Currency
                {
                    Id = 597,
                    CurrencyTypeId = 2,
                    Name = "Opal",
                    Abbreviation = "OPAL"
                }, new Currency
                {
                    Id = 1334,
                    CurrencyTypeId = 2,
                    Name = "Elementrem",
                    Abbreviation = "ELE"
                }, new Currency
                {
                    Id = 778,
                    CurrencyTypeId = 2,
                    Name = "bitGold",
                    Abbreviation = "BITGOLD"
                }, new Currency
                {
                    Id = 954,
                    CurrencyTypeId = 2,
                    Name = "bitEUR",
                    Abbreviation = "BITEUR"
                }, new Currency
                {
                    Id = 1504,
                    CurrencyTypeId = 2,
                    Name = "InflationCoin",
                    Abbreviation = "IFLT"
                }, new Currency
                {
                    Id = 702,
                    CurrencyTypeId = 2,
                    Name = "SpreadCoin",
                    Abbreviation = "SPR"
                }, new Currency
                {
                    Id = 1297,
                    CurrencyTypeId = 2,
                    Name = "ChessCoin",
                    Abbreviation = "CHESS"
                }, new Currency
                {
                    Id = 3690,
                    CurrencyTypeId = 2,
                    Name = "Bulleon",
                    Abbreviation = "BUL"
                }, new Currency
                {
                    Id = 869,
                    CurrencyTypeId = 2,
                    Name = "Crave",
                    Abbreviation = "CRAVE"
                }, new Currency
                {
                    Id = 703,
                    CurrencyTypeId = 2,
                    Name = "Rimbit",
                    Abbreviation = "RBT"
                }, new Currency
                {
                    Id = 33,
                    CurrencyTypeId = 2,
                    Name = "Mincoin",
                    Abbreviation = "MNC"
                }, new Currency
                {
                    Id = 2160,
                    CurrencyTypeId = 2,
                    Name = "Innova",
                    Abbreviation = "INN"
                }, new Currency
                {
                    Id = 3124,
                    CurrencyTypeId = 2,
                    Name = "Dragonglass",
                    Abbreviation = "DGS"
                }, new Currency
                {
                    Id = 2063,
                    CurrencyTypeId = 2,
                    Name = "Tracto",
                    Abbreviation = "TRCT"
                }, new Currency
                {
                    Id = 3439,
                    CurrencyTypeId = 2,
                    Name = "iDealCash",
                    Abbreviation = "DEAL"
                }, new Currency
                {
                    Id = 2514,
                    CurrencyTypeId = 2,
                    Name = "Shekel",
                    Abbreviation = "JEW"
                }, new Currency
                {
                    Id = 2848,
                    CurrencyTypeId = 2,
                    Name = "Paymon",
                    Abbreviation = "PMNT"
                }, new Currency
                {
                    Id = 1980,
                    CurrencyTypeId = 2,
                    Name = "Elixir",
                    Abbreviation = "ELIX"
                }, new Currency
                {
                    Id = 3399,
                    CurrencyTypeId = 2,
                    Name = "Wispr",
                    Abbreviation = "WSP"
                }, new Currency
                {
                    Id = 3457,
                    CurrencyTypeId = 2,
                    Name = "Iridium",
                    Abbreviation = "IRD"
                }, new Currency
                {
                    Id = 3311,
                    CurrencyTypeId = 2,
                    Name = "Castle",
                    Abbreviation = "CSTL"
                }, new Currency
                {
                    Id = 921,
                    CurrencyTypeId = 2,
                    Name = "Universal Currency",
                    Abbreviation = "UNIT"
                }, new Currency
                {
                    Id = 3331,
                    CurrencyTypeId = 2,
                    Name = "CrowdWiz",
                    Abbreviation = "WIZ"
                }, new Currency
                {
                    Id = 3764,
                    CurrencyTypeId = 2,
                    Name = "Save Environment Token",
                    Abbreviation = "SET"
                }, new Currency
                {
                    Id = 2580,
                    CurrencyTypeId = 2,
                    Name = "Leadcoin",
                    Abbreviation = "LDC"
                }, new Currency
                {
                    Id = 2681,
                    CurrencyTypeId = 2,
                    Name = "Origami",
                    Abbreviation = "ORI"
                }, new Currency
                {
                    Id = 2395,
                    CurrencyTypeId = 2,
                    Name = "Ignition",
                    Abbreviation = "IC"
                }, new Currency
                {
                    Id = 1959,
                    CurrencyTypeId = 2,
                    Name = "Oceanlab",
                    Abbreviation = "OCL"
                }, new Currency
                {
                    Id = 1671,
                    CurrencyTypeId = 2,
                    Name = "iTicoin",
                    Abbreviation = "ITI"
                }, new Currency
                {
                    Id = 3087,
                    CurrencyTypeId = 2,
                    Name = "CROAT",
                    Abbreviation = "CROAT"
                }, new Currency
                {
                    Id = 121,
                    CurrencyTypeId = 2,
                    Name = "UltraCoin",
                    Abbreviation = "UTC"
                }, new Currency
                {
                    Id = 2542,
                    CurrencyTypeId = 2,
                    Name = "Tidex Token",
                    Abbreviation = "TDX"
                }, new Currency
                {
                    Id = 3268,
                    CurrencyTypeId = 2,
                    Name = "DarexTravel",
                    Abbreviation = "DART"
                }, new Currency
                {
                    Id = 3122,
                    CurrencyTypeId = 2,
                    Name = "Help The Homeless Coin",
                    Abbreviation = "HTH"
                }, new Currency
                {
                    Id = 3665,
                    CurrencyTypeId = 2,
                    Name = "Impleum",
                    Abbreviation = "IMPL"
                }, new Currency
                {
                    Id = 3412,
                    CurrencyTypeId = 2,
                    Name = "Simmitri",
                    Abbreviation = "SIM"
                }, new Currency
                {
                    Id = 331,
                    CurrencyTypeId = 2,
                    Name = "Litecoin Plus",
                    Abbreviation = "LCP"
                }, new Currency
                {
                    Id = 3688,
                    CurrencyTypeId = 2,
                    Name = "MoX",
                    Abbreviation = "MOX"
                }, new Currency
                {
                    Id = 978,
                    CurrencyTypeId = 2,
                    Name = "Ratecoin",
                    Abbreviation = "XRA"
                }, new Currency
                {
                    Id = 3463,
                    CurrencyTypeId = 2,
                    Name = "RPICoin",
                    Abbreviation = "RPI"
                }, new Currency
                {
                    Id = 2404,
                    CurrencyTypeId = 2,
                    Name = "TOKYO",
                    Abbreviation = "TOKC"
                }, new Currency
                {
                    Id = 3013,
                    CurrencyTypeId = 2,
                    Name = "PRiVCY",
                    Abbreviation = "PRIV"
                }, new Currency
                {
                    Id = 2007,
                    CurrencyTypeId = 2,
                    Name = "Regalcoin",
                    Abbreviation = "REC"
                }, new Currency
                {
                    Id = 3093,
                    CurrencyTypeId = 2,
                    Name = "BBSCoin",
                    Abbreviation = "BBS"
                }, new Currency
                {
                    Id = 2122,
                    CurrencyTypeId = 2,
                    Name = "Ellaism",
                    Abbreviation = "ELLA"
                }, new Currency
                {
                    Id = 1842,
                    CurrencyTypeId = 2,
                    Name = "CampusCoin",
                    Abbreviation = "CC"
                }, new Currency
                {
                    Id = 2180,
                    CurrencyTypeId = 2,
                    Name = "bitJob",
                    Abbreviation = "STU"
                }, new Currency
                {
                    Id = 3882,
                    CurrencyTypeId = 2,
                    Name = "Arqma",
                    Abbreviation = "ARQ"
                }, new Currency
                {
                    Id = 3146,
                    CurrencyTypeId = 2,
                    Name = "CyberFM",
                    Abbreviation = "CYFM"
                }, new Currency
                {
                    Id = 3214,
                    CurrencyTypeId = 2,
                    Name = "Soniq",
                    Abbreviation = "SONIQ"
                }, new Currency
                {
                    Id = 2690,
                    CurrencyTypeId = 2,
                    Name = "Biotron",
                    Abbreviation = "BTRN"
                }, new Currency
                {
                    Id = 2048,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Cash",
                    Abbreviation = "ECASH"
                }, new Currency
                {
                    Id = 1468,
                    CurrencyTypeId = 2,
                    Name = "Kurrent",
                    Abbreviation = "KURT"
                }, new Currency
                {
                    Id = 3800,
                    CurrencyTypeId = 2,
                    Name = "FidexToken",
                    Abbreviation = "FEX"
                }, new Currency
                {
                    Id = 28,
                    CurrencyTypeId = 2,
                    Name = "Digitalcoin",
                    Abbreviation = "DGC"
                }, new Currency
                {
                    Id = 1266,
                    CurrencyTypeId = 2,
                    Name = "MarteXcoin",
                    Abbreviation = "MXT"
                }, new Currency
                {
                    Id = 3207,
                    CurrencyTypeId = 2,
                    Name = "Social Activity Token",
                    Abbreviation = "SAT"
                }, new Currency
                {
                    Id = 1390,
                    CurrencyTypeId = 2,
                    Name = "Jin Coin",
                    Abbreviation = "JIN"
                }, new Currency
                {
                    Id = 3409,
                    CurrencyTypeId = 2,
                    Name = "Etheera",
                    Abbreviation = "ETA"
                }, new Currency
                {
                    Id = 1276,
                    CurrencyTypeId = 2,
                    Name = "ICO OpenLedger",
                    Abbreviation = "ICOO"
                }, new Currency
                {
                    Id = 3133,
                    CurrencyTypeId = 2,
                    Name = "Arepacoin",
                    Abbreviation = "AREPA"
                }, new Currency
                {
                    Id = 2890,
                    CurrencyTypeId = 2,
                    Name = "KanadeCoin",
                    Abbreviation = "KNDC"
                }, new Currency
                {
                    Id = 1662,
                    CurrencyTypeId = 2,
                    Name = "Condensate",
                    Abbreviation = "RAIN"
                }, new Currency
                {
                    Id = 1483,
                    CurrencyTypeId = 2,
                    Name = "vSlice",
                    Abbreviation = "VSL"
                }, new Currency
                {
                    Id = 350,
                    CurrencyTypeId = 2,
                    Name = "BoostCoin",
                    Abbreviation = "BOST"
                }, new Currency
                {
                    Id = 1511,
                    CurrencyTypeId = 2,
                    Name = "PureVidz",
                    Abbreviation = "VIDZ"
                }, new Currency
                {
                    Id = 1306,
                    CurrencyTypeId = 2,
                    Name = "Cryptojacks",
                    Abbreviation = "CJ"
                }, new Currency
                {
                    Id = 1085,
                    CurrencyTypeId = 2,
                    Name = "Swing",
                    Abbreviation = "SWING"
                }, new Currency
                {
                    Id = 3031,
                    CurrencyTypeId = 2,
                    Name = "Orbis Token",
                    Abbreviation = "OBT"
                }, new Currency
                {
                    Id = 3008,
                    CurrencyTypeId = 2,
                    Name = "Vivid Coin",
                    Abbreviation = "VIVID"
                }, new Currency
                {
                    Id = 295,
                    CurrencyTypeId = 2,
                    Name = "BTCtalkcoin",
                    Abbreviation = "TALK"
                }, new Currency
                {
                    Id = 3443,
                    CurrencyTypeId = 2,
                    Name = "HUZU",
                    Abbreviation = "HUZU"
                }, new Currency
                {
                    Id = 3223,
                    CurrencyTypeId = 2,
                    Name = "DOWCOIN",
                    Abbreviation = "DOW"
                }, new Currency
                {
                    Id = 3125,
                    CurrencyTypeId = 2,
                    Name = "XDNA",
                    Abbreviation = "XDNA"
                }, new Currency
                {
                    Id = 426,
                    CurrencyTypeId = 2,
                    Name = "BritCoin",
                    Abbreviation = "BRIT"
                }, new Currency
                {
                    Id = 113,
                    CurrencyTypeId = 2,
                    Name = "SmartCoin",
                    Abbreviation = "SMC"
                }, new Currency
                {
                    Id = 2770,
                    CurrencyTypeId = 2,
                    Name = "Cazcoin",
                    Abbreviation = "CAZ"
                }, new Currency
                {
                    Id = 130,
                    CurrencyTypeId = 2,
                    Name = "HunterCoin",
                    Abbreviation = "HUC"
                }, new Currency
                {
                    Id = 1165,
                    CurrencyTypeId = 2,
                    Name = "Evil Coin",
                    Abbreviation = "EVIL"
                }, new Currency
                {
                    Id = 1981,
                    CurrencyTypeId = 2,
                    Name = "Billionaire Token",
                    Abbreviation = "XBL"
                }, new Currency
                {
                    Id = 2486,
                    CurrencyTypeId = 2,
                    Name = "Speed Mining Service",
                    Abbreviation = "SMS"
                }, new Currency
                {
                    Id = 2355,
                    CurrencyTypeId = 2,
                    Name = "OP Coin",
                    Abbreviation = "OPC"
                }, new Currency
                {
                    Id = 341,
                    CurrencyTypeId = 2,
                    Name = "SuperCoin",
                    Abbreviation = "SUPER"
                }, new Currency
                {
                    Id = 2751,
                    CurrencyTypeId = 2,
                    Name = "FundRequest",
                    Abbreviation = "FND"
                }, new Currency
                {
                    Id = 3246,
                    CurrencyTypeId = 2,
                    Name = "Thunderstake",
                    Abbreviation = "TSC"
                }, new Currency
                {
                    Id = 1747,
                    CurrencyTypeId = 2,
                    Name = "Onix",
                    Abbreviation = "ONX"
                }, new Currency
                {
                    Id = 1285,
                    CurrencyTypeId = 2,
                    Name = "GoldBlocks",
                    Abbreviation = "GB"
                }, new Currency
                {
                    Id = 1956,
                    CurrencyTypeId = 2,
                    Name = "VIVO",
                    Abbreviation = "VIVO"
                }, new Currency
                {
                    Id = 1673,
                    CurrencyTypeId = 2,
                    Name = "Minereum",
                    Abbreviation = "MNE"
                }, new Currency
                {
                    Id = 2069,
                    CurrencyTypeId = 2,
                    Name = "Open Trading Network",
                    Abbreviation = "OTN"
                }, new Currency
                {
                    Id = 3414,
                    CurrencyTypeId = 2,
                    Name = "ZeusNetwork",
                    Abbreviation = "ZEUS"
                }, new Currency
                {
                    Id = 2648,
                    CurrencyTypeId = 2,
                    Name = "Bitsum",
                    Abbreviation = "BSM"
                }, new Currency
                {
                    Id = 3165,
                    CurrencyTypeId = 2,
                    Name = "Arion",
                    Abbreviation = "ARION"
                }, new Currency
                {
                    Id = 1836,
                    CurrencyTypeId = 2,
                    Name = "Signatum",
                    Abbreviation = "SIGT"
                }, new Currency
                {
                    Id = 2093,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Red",
                    Abbreviation = "BTCRED"
                }, new Currency
                {
                    Id = 3517,
                    CurrencyTypeId = 2,
                    Name = "SONDER",
                    Abbreviation = "SNR"
                }, new Currency
                {
                    Id = 3377,
                    CurrencyTypeId = 2,
                    Name = "GenesisX",
                    Abbreviation = "XGS"
                }, new Currency
                {
                    Id = 3046,
                    CurrencyTypeId = 2,
                    Name = "Blocknode",
                    Abbreviation = "BND"
                }, new Currency
                {
                    Id = 2415,
                    CurrencyTypeId = 2,
                    Name = "ArbitrageCT",
                    Abbreviation = "ARCT"
                }, new Currency
                {
                    Id = 1763,
                    CurrencyTypeId = 2,
                    Name = "BriaCoin",
                    Abbreviation = "BRIA"
                }, new Currency
                {
                    Id = 1273,
                    CurrencyTypeId = 2,
                    Name = "Citadel",
                    Abbreviation = "CTL"
                }, new Currency
                {
                    Id = 3266,
                    CurrencyTypeId = 2,
                    Name = "Carebit",
                    Abbreviation = "CARE"
                }, new Currency
                {
                    Id = 988,
                    CurrencyTypeId = 2,
                    Name = "IrishCoin",
                    Abbreviation = "IRL"
                }, new Currency
                {
                    Id = 1922,
                    CurrencyTypeId = 2,
                    Name = "Monoeci",
                    Abbreviation = "XMCC"
                }, new Currency
                {
                    Id = 2973,
                    CurrencyTypeId = 2,
                    Name = "empowr coin",
                    Abbreviation = "EMPR"
                }, new Currency
                {
                    Id = 1053,
                    CurrencyTypeId = 2,
                    Name = "Bolivarcoin",
                    Abbreviation = "BOLI"
                }, new Currency
                {
                    Id = 2619,
                    CurrencyTypeId = 2,
                    Name = "BitStation",
                    Abbreviation = "BSTN"
                }, new Currency
                {
                    Id = 3493,
                    CurrencyTypeId = 2,
                    Name = "SAKECOIN",
                    Abbreviation = "SAKE"
                }, new Currency
                {
                    Id = 3289,
                    CurrencyTypeId = 2,
                    Name = "BitCoen",
                    Abbreviation = "BEN"
                }, new Currency
                {
                    Id = 1223,
                    CurrencyTypeId = 2,
                    Name = "BERNcash",
                    Abbreviation = "BERN"
                }, new Currency
                {
                    Id = 3447,
                    CurrencyTypeId = 2,
                    Name = "Atheios",
                    Abbreviation = "ATH"
                }, new Currency
                {
                    Id = 1888,
                    CurrencyTypeId = 2,
                    Name = "InvestFeed",
                    Abbreviation = "IFT"
                }, new Currency
                {
                    Id = 1607,
                    CurrencyTypeId = 2,
                    Name = "Impact",
                    Abbreviation = "IMX"
                }, new Currency
                {
                    Id = 3293,
                    CurrencyTypeId = 2,
                    Name = "Olympic",
                    Abbreviation = "OLMP"
                }, new Currency
                {
                    Id = 3395,
                    CurrencyTypeId = 2,
                    Name = "SteepCoin",
                    Abbreviation = "STEEP"
                }, new Currency
                {
                    Id = 316,
                    CurrencyTypeId = 2,
                    Name = "Dreamcoin",
                    Abbreviation = "DRM"
                }, new Currency
                {
                    Id = 2290,
                    CurrencyTypeId = 2,
                    Name = "YENTEN",
                    Abbreviation = "YTN"
                }, new Currency
                {
                    Id = 125,
                    CurrencyTypeId = 2,
                    Name = "Blakecoin",
                    Abbreviation = "BLC"
                }, new Currency
                {
                    Id = 1033,
                    CurrencyTypeId = 2,
                    Name = "GuccioneCoin",
                    Abbreviation = "GCC"
                }, new Currency
                {
                    Id = 2883,
                    CurrencyTypeId = 2,
                    Name = "ZINC",
                    Abbreviation = "ZINC"
                }, new Currency
                {
                    Id = 2996,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin W Spectrum",
                    Abbreviation = "BWS"
                }, new Currency
                {
                    Id = 3350,
                    CurrencyTypeId = 2,
                    Name = "Dach Coin",
                    Abbreviation = "DACHX"
                }, new Currency
                {
                    Id = 3652,
                    CurrencyTypeId = 2,
                    Name = "bitcoin2network",
                    Abbreviation = "B2N"
                }, new Currency
                {
                    Id = 1136,
                    CurrencyTypeId = 2,
                    Name = "Adzcoin",
                    Abbreviation = "ADZ"
                }, new Currency
                {
                    Id = 3051,
                    CurrencyTypeId = 2,
                    Name = "Bitblocks",
                    Abbreviation = "BBK"
                }, new Currency
                {
                    Id = 837,
                    CurrencyTypeId = 2,
                    Name = "X-Coin",
                    Abbreviation = "XCO"
                }, new Currency
                {
                    Id = 1617,
                    CurrencyTypeId = 2,
                    Name = "Ultimate Secure Cash",
                    Abbreviation = "USC"
                }, new Currency
                {
                    Id = 1035,
                    CurrencyTypeId = 2,
                    Name = "AmsterdamCoin",
                    Abbreviation = "AMS"
                }, new Currency
                {
                    Id = 3332,
                    CurrencyTypeId = 2,
                    Name = "Gossipcoin",
                    Abbreviation = "GOSS"
                }, new Currency
                {
                    Id = 1550,
                    CurrencyTypeId = 2,
                    Name = "Master Swiscoin",
                    Abbreviation = "MSCN"
                }, new Currency
                {
                    Id = 1877,
                    CurrencyTypeId = 2,
                    Name = "Rupaya",
                    Abbreviation = "RUPX"
                }, new Currency
                {
                    Id = 1113,
                    CurrencyTypeId = 2,
                    Name = "SecretCoin",
                    Abbreviation = "SCRT"
                }, new Currency
                {
                    Id = 2221,
                    CurrencyTypeId = 2,
                    Name = "VoteCoin",
                    Abbreviation = "VOT"
                }, new Currency
                {
                    Id = 1250,
                    CurrencyTypeId = 2,
                    Name = "Zurcoin",
                    Abbreviation = "ZUR"
                }, new Currency
                {
                    Id = 2668,
                    CurrencyTypeId = 2,
                    Name = "Earth Token",
                    Abbreviation = "EARTH"
                }, new Currency
                {
                    Id = 2651,
                    CurrencyTypeId = 2,
                    Name = "GreenMed",
                    Abbreviation = "GRMD"
                }, new Currency
                {
                    Id = 3503,
                    CurrencyTypeId = 2,
                    Name = "CommunityGeneration",
                    Abbreviation = "CGEN"
                }, new Currency
                {
                    Id = 3250,
                    CurrencyTypeId = 2,
                    Name = "Zoomba",
                    Abbreviation = "ZBA"
                }, new Currency
                {
                    Id = 3009,
                    CurrencyTypeId = 2,
                    Name = "Pure",
                    Abbreviation = "PUREX"
                }, new Currency
                {
                    Id = 3668,
                    CurrencyTypeId = 2,
                    Name = "ProxyNode",
                    Abbreviation = "PRX"
                }, new Currency
                {
                    Id = 3403,
                    CurrencyTypeId = 2,
                    Name = "EagleX",
                    Abbreviation = "EGX"
                }, new Currency
                {
                    Id = 1248,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin 21",
                    Abbreviation = "XBTC21"
                }, new Currency
                {
                    Id = 3353,
                    CurrencyTypeId = 2,
                    Name = "WELL",
                    Abbreviation = "WELL"
                }, new Currency
                {
                    Id = 3416,
                    CurrencyTypeId = 2,
                    Name = "Digiwage",
                    Abbreviation = "WAGE"
                }, new Currency
                {
                    Id = 2978,
                    CurrencyTypeId = 2,
                    Name = "AceD",
                    Abbreviation = "ACED"
                }, new Currency
                {
                    Id = 3321,
                    CurrencyTypeId = 2,
                    Name = "BunnyToken",
                    Abbreviation = "BUNNY"
                }, new Currency
                {
                    Id = 3656,
                    CurrencyTypeId = 2,
                    Name = "Beacon",
                    Abbreviation = "BECN"
                }, new Currency
                {
                    Id = 278,
                    CurrencyTypeId = 2,
                    Name = "Quebecoin",
                    Abbreviation = "QBC"
                }, new Currency
                {
                    Id = 536,
                    CurrencyTypeId = 2,
                    Name = "Joincoin",
                    Abbreviation = "J"
                }, new Currency
                {
                    Id = 3341,
                    CurrencyTypeId = 2,
                    Name = "Phonecoin",
                    Abbreviation = "PHON"
                }, new Currency
                {
                    Id = 3778,
                    CurrencyTypeId = 2,
                    Name = "EVOS",
                    Abbreviation = "EVOS"
                }, new Currency
                {
                    Id = 1846,
                    CurrencyTypeId = 2,
                    Name = "GeyserCoin",
                    Abbreviation = "GSR"
                }, new Currency
                {
                    Id = 3267,
                    CurrencyTypeId = 2,
                    Name = "Project Coin",
                    Abbreviation = "PRJ"
                }, new Currency
                {
                    Id = 2051,
                    CurrencyTypeId = 2,
                    Name = "Authorship",
                    Abbreviation = "ATS"
                }, new Currency
                {
                    Id = 367,
                    CurrencyTypeId = 2,
                    Name = "Coin2.1",
                    Abbreviation = "C2"
                }, new Currency
                {
                    Id = 3050,
                    CurrencyTypeId = 2,
                    Name = "Dystem",
                    Abbreviation = "DTEM"
                }, new Currency
                {
                    Id = 2715,
                    CurrencyTypeId = 2,
                    Name = "ConnectJob",
                    Abbreviation = "CJT"
                }, new Currency
                {
                    Id = 3273,
                    CurrencyTypeId = 2,
                    Name = "IQ.cash",
                    Abbreviation = "IQ"
                }, new Currency
                {
                    Id = 1850,
                    CurrencyTypeId = 2,
                    Name = "Cream",
                    Abbreviation = "CRM"
                }, new Currency
                {
                    Id = 1496,
                    CurrencyTypeId = 2,
                    Name = "Luna Coin",
                    Abbreviation = "LUNA"
                }, new Currency
                {
                    Id = 1218,
                    CurrencyTypeId = 2,
                    Name = "PostCoin",
                    Abbreviation = "POST"
                }, new Currency
                {
                    Id = 3393,
                    CurrencyTypeId = 2,
                    Name = "MASTERNET",
                    Abbreviation = "MASH"
                }, new Currency
                {
                    Id = 3324,
                    CurrencyTypeId = 2,
                    Name = "PluraCoin",
                    Abbreviation = "PLURA"
                }, new Currency
                {
                    Id = 3464,
                    CurrencyTypeId = 2,
                    Name = "Cheesecoin",
                    Abbreviation = "CHEESE"
                }, new Currency
                {
                    Id = 1793,
                    CurrencyTypeId = 2,
                    Name = "Bitdeal",
                    Abbreviation = "BDL"
                }, new Currency
                {
                    Id = 1743,
                    CurrencyTypeId = 2,
                    Name = "KingN Coin",
                    Abbreviation = "KNC"
                }, new Currency
                {
                    Id = 3645,
                    CurrencyTypeId = 2,
                    Name = "Shivers",
                    Abbreviation = "SHVR"
                }, new Currency
                {
                    Id = 2241,
                    CurrencyTypeId = 2,
                    Name = "Ccore",
                    Abbreviation = "CCO"
                }, new Currency
                {
                    Id = 2032,
                    CurrencyTypeId = 2,
                    Name = "Crystal Clear ",
                    Abbreviation = "CCT"
                }, new Currency
                {
                    Id = 2074,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Gold",
                    Abbreviation = "ETG"
                }, new Currency
                {
                    Id = 3624,
                    CurrencyTypeId = 2,
                    Name = "Zealium",
                    Abbreviation = "NZL"
                }, new Currency
                {
                    Id = 1539,
                    CurrencyTypeId = 2,
                    Name = "Elysium",
                    Abbreviation = "ELS"
                }, new Currency
                {
                    Id = 1506,
                    CurrencyTypeId = 2,
                    Name = "Safe Trade Coin",
                    Abbreviation = "XSTC"
                }, new Currency
                {
                    Id = 3489,
                    CurrencyTypeId = 2,
                    Name = "Escroco Emerald",
                    Abbreviation = "ESCE"
                }, new Currency
                {
                    Id = 1687,
                    CurrencyTypeId = 2,
                    Name = "Digital Money Bits",
                    Abbreviation = "DMB"
                }, new Currency
                {
                    Id = 1254,
                    CurrencyTypeId = 2,
                    Name = "PlatinumBAR",
                    Abbreviation = "XPTX"
                }, new Currency
                {
                    Id = 601,
                    CurrencyTypeId = 2,
                    Name = "Acoin",
                    Abbreviation = "ACOIN"
                }, new Currency
                {
                    Id = 1089,
                    CurrencyTypeId = 2,
                    Name = "ParallelCoin",
                    Abbreviation = "DUO"
                }, new Currency
                {
                    Id = 1890,
                    CurrencyTypeId = 2,
                    Name = "Etheriya",
                    Abbreviation = "RIYA"
                }, new Currency
                {
                    Id = 3294,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Adult",
                    Abbreviation = "BTAD"
                }, new Currency
                {
                    Id = 1200,
                    CurrencyTypeId = 2,
                    Name = "NevaCoin",
                    Abbreviation = "NEVA"
                }, new Currency
                {
                    Id = 3201,
                    CurrencyTypeId = 2,
                    Name = "Printex",
                    Abbreviation = "PRTX"
                }, new Currency
                {
                    Id = 898,
                    CurrencyTypeId = 2,
                    Name = "Californium",
                    Abbreviation = "CF"
                }, new Currency
                {
                    Id = 3062,
                    CurrencyTypeId = 2,
                    Name = "GambleCoin",
                    Abbreviation = "GMCN"
                }, new Currency
                {
                    Id = 1212,
                    CurrencyTypeId = 2,
                    Name = "MojoCoin",
                    Abbreviation = "MOJO"
                }, new Currency
                {
                    Id = 513,
                    CurrencyTypeId = 2,
                    Name = "Titcoin",
                    Abbreviation = "TIT"
                }, new Currency
                {
                    Id = 2983,
                    CurrencyTypeId = 2,
                    Name = "AdultChain",
                    Abbreviation = "XXX"
                }, new Currency
                {
                    Id = 1241,
                    CurrencyTypeId = 2,
                    Name = "FuzzBalls",
                    Abbreviation = "FUZZ"
                }, new Currency
                {
                    Id = 3355,
                    CurrencyTypeId = 2,
                    Name = "Azart",
                    Abbreviation = "AZART"
                }, new Currency
                {
                    Id = 1717,
                    CurrencyTypeId = 2,
                    Name = "Neuro",
                    Abbreviation = "NRO"
                }, new Currency
                {
                    Id = 3192,
                    CurrencyTypeId = 2,
                    Name = "CatoCoin",
                    Abbreviation = "CATO"
                }, new Currency
                {
                    Id = 934,
                    CurrencyTypeId = 2,
                    Name = "ParkByte",
                    Abbreviation = "PKB"
                }, new Currency
                {
                    Id = 57,
                    CurrencyTypeId = 2,
                    Name = "SecureCoin",
                    Abbreviation = "SRC"
                }, new Currency
                {
                    Id = 3088,
                    CurrencyTypeId = 2,
                    Name = "BitCoin One",
                    Abbreviation = "BTCONE"
                }, new Currency
                {
                    Id = 1198,
                    CurrencyTypeId = 2,
                    Name = "BigUp",
                    Abbreviation = "BIGUP"
                }, new Currency
                {
                    Id = 1395,
                    CurrencyTypeId = 2,
                    Name = "Dollarcoin",
                    Abbreviation = "DLC"
                }, new Currency
                {
                    Id = 1353,
                    CurrencyTypeId = 2,
                    Name = "TajCoin",
                    Abbreviation = "TAJ"
                }, new Currency
                {
                    Id = 1514,
                    CurrencyTypeId = 2,
                    Name = "ICOBID",
                    Abbreviation = "ICOB"
                }, new Currency
                {
                    Id = 1534,
                    CurrencyTypeId = 2,
                    Name = "BOAT",
                    Abbreviation = "BOAT"
                }, new Currency
                {
                    Id = 2205,
                    CurrencyTypeId = 2,
                    Name = "Phantomx",
                    Abbreviation = "PNX"
                }, new Currency
                {
                    Id = 3333,
                    CurrencyTypeId = 2,
                    Name = "Sola Token",
                    Abbreviation = "SOL"
                }, new Currency
                {
                    Id = 1535,
                    CurrencyTypeId = 2,
                    Name = "Eryllium",
                    Abbreviation = "ERY"
                }, new Currency
                {
                    Id = 3228,
                    CurrencyTypeId = 2,
                    Name = "Cryptosolartech",
                    Abbreviation = "CST"
                }, new Currency
                {
                    Id = 3129,
                    CurrencyTypeId = 2,
                    Name = "Nyerium",
                    Abbreviation = "NYEX"
                }, new Currency
                {
                    Id = 3349,
                    CurrencyTypeId = 2,
                    Name = "PyrexCoin",
                    Abbreviation = "PYX"
                }, new Currency
                {
                    Id = 520,
                    CurrencyTypeId = 2,
                    Name = "Virtacoin",
                    Abbreviation = "VTA"
                }, new Currency
                {
                    Id = 1038,
                    CurrencyTypeId = 2,
                    Name = "Eurocoin",
                    Abbreviation = "EUC"
                }, new Currency
                {
                    Id = 3644,
                    CurrencyTypeId = 2,
                    Name = "TravelNote",
                    Abbreviation = "TVNT"
                }, new Currency
                {
                    Id = 1581,
                    CurrencyTypeId = 2,
                    Name = "Honey",
                    Abbreviation = "HONEY"
                }, new Currency
                {
                    Id = 2168,
                    CurrencyTypeId = 2,
                    Name = "Grimcoin",
                    Abbreviation = "GRIM"
                }, new Currency
                {
                    Id = 2140,
                    CurrencyTypeId = 2,
                    Name = "SONO",
                    Abbreviation = "SONO"
                }, new Currency
                {
                    Id = 1420,
                    CurrencyTypeId = 2,
                    Name = "Atomic Coin",
                    Abbreviation = "ATOM"
                }, new Currency
                {
                    Id = 1282,
                    CurrencyTypeId = 2,
                    Name = "High Voltage",
                    Abbreviation = "HVCO"
                }, new Currency
                {
                    Id = 3030,
                    CurrencyTypeId = 2,
                    Name = "BrokerNekoNetwork",
                    Abbreviation = "BNN"
                }, new Currency
                {
                    Id = 1206,
                    CurrencyTypeId = 2,
                    Name = "BumbaCoin",
                    Abbreviation = "BUMBA"
                }, new Currency
                {
                    Id = 3421,
                    CurrencyTypeId = 2,
                    Name = "PrimeStone",
                    Abbreviation = "PSC"
                }, new Currency
                {
                    Id = 1651,
                    CurrencyTypeId = 2,
                    Name = "SpeedCash",
                    Abbreviation = "SCS"
                }, new Currency
                {
                    Id = 1155,
                    CurrencyTypeId = 2,
                    Name = "Litecred",
                    Abbreviation = "LTCR"
                }, new Currency
                {
                    Id = 3682,
                    CurrencyTypeId = 2,
                    Name = "Italo",
                    Abbreviation = "XTA"
                }, new Currency
                {
                    Id = 3442,
                    CurrencyTypeId = 2,
                    Name = "INDINODE",
                    Abbreviation = "XIND"
                }, new Currency
                {
                    Id = 1209,
                    CurrencyTypeId = 2,
                    Name = "PosEx",
                    Abbreviation = "PEX"
                }, new Currency
                {
                    Id = 1597,
                    CurrencyTypeId = 2,
                    Name = "Bankcoin",
                    Abbreviation = "B@"
                }, new Currency
                {
                    Id = 2025,
                    CurrencyTypeId = 2,
                    Name = "FLiK",
                    Abbreviation = "FLIK"
                }, new Currency
                {
                    Id = 1926,
                    CurrencyTypeId = 2,
                    Name = "BROTHER",
                    Abbreviation = "BRAT"
                }, new Currency
                {
                    Id = 159,
                    CurrencyTypeId = 2,
                    Name = "Cashcoin",
                    Abbreviation = "CASH"
                }, new Currency
                {
                    Id = 1389,
                    CurrencyTypeId = 2,
                    Name = "Zayedcoin",
                    Abbreviation = "ZYD"
                }, new Currency
                {
                    Id = 1396,
                    CurrencyTypeId = 2,
                    Name = "MustangCoin",
                    Abbreviation = "MST"
                }, new Currency
                {
                    Id = 69,
                    CurrencyTypeId = 2,
                    Name = "Datacoin",
                    Abbreviation = "DTC"
                }, new Currency
                {
                    Id = 3174,
                    CurrencyTypeId = 2,
                    Name = "Fintab",
                    Abbreviation = "FNTB"
                }, new Currency
                {
                    Id = 3508,
                    CurrencyTypeId = 2,
                    Name = "BZLCOIN",
                    Abbreviation = "BZL"
                }, new Currency
                {
                    Id = 1889,
                    CurrencyTypeId = 2,
                    Name = "CoinonatX",
                    Abbreviation = "XCXT"
                }, new Currency
                {
                    Id = 2905,
                    CurrencyTypeId = 2,
                    Name = "Qurito",
                    Abbreviation = "QURO"
                }, new Currency
                {
                    Id = 2257,
                    CurrencyTypeId = 2,
                    Name = "Nekonium",
                    Abbreviation = "NUKO"
                }, new Currency
                {
                    Id = 2460,
                    CurrencyTypeId = 2,
                    Name = "Qbic",
                    Abbreviation = "QBIC"
                }, new Currency
                {
                    Id = 1912,
                    CurrencyTypeId = 2,
                    Name = "Dalecoin",
                    Abbreviation = "DALC"
                }, new Currency
                {
                    Id = 1194,
                    CurrencyTypeId = 2,
                    Name = "Independent Money System",
                    Abbreviation = "IMS"
                }, new Currency
                {
                    Id = 993,
                    CurrencyTypeId = 2,
                    Name = "BowsCoin",
                    Abbreviation = "BSC"
                }, new Currency
                {
                    Id = 3410,
                    CurrencyTypeId = 2,
                    Name = "Bitspace",
                    Abbreviation = "BSX"
                }, new Currency
                {
                    Id = 3586,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin X",
                    Abbreviation = "BTX"
                }, new Currency
                {
                    Id = 1429,
                    CurrencyTypeId = 2,
                    Name = "Levocoin",
                    Abbreviation = "LEVO"
                }, new Currency
                {
                    Id = 938,
                    CurrencyTypeId = 2,
                    Name = "ARbit",
                    Abbreviation = "ARB"
                }, new Currency
                {
                    Id = 1576,
                    CurrencyTypeId = 2,
                    Name = "MiloCoin",
                    Abbreviation = "MILO"
                }, new Currency
                {
                    Id = 3491,
                    CurrencyTypeId = 2,
                    Name = "EZOOW",
                    Abbreviation = "EZW"
                }, new Currency
                {
                    Id = 1674,
                    CurrencyTypeId = 2,
                    Name = "Cannation",
                    Abbreviation = "CNNC"
                }, new Currency
                {
                    Id = 1693,
                    CurrencyTypeId = 2,
                    Name = "Theresa May Coin",
                    Abbreviation = "MAY"
                }, new Currency
                {
                    Id = 3478,
                    CurrencyTypeId = 2,
                    Name = "BitMoney",
                    Abbreviation = "BIT"
                }, new Currency
                {
                    Id = 2284,
                    CurrencyTypeId = 2,
                    Name = "Trident Group",
                    Abbreviation = "TRDT"
                }, new Currency
                {
                    Id = 1935,
                    CurrencyTypeId = 2,
                    Name = "LiteCoin Ultra",
                    Abbreviation = "LTCU"
                }, new Currency
                {
                    Id = 3313,
                    CurrencyTypeId = 2,
                    Name = "CryptoFlow",
                    Abbreviation = "CFL"
                }, new Currency
                {
                    Id = 1748,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Planet",
                    Abbreviation = "BTPL"
                }, new Currency
                {
                    Id = 656,
                    CurrencyTypeId = 2,
                    Name = "Prime-XI",
                    Abbreviation = "PXI"
                }, new Currency
                {
                    Id = 3197,
                    CurrencyTypeId = 2,
                    Name = "Graphcoin",
                    Abbreviation = "GRPH"
                }, new Currency
                {
                    Id = 1528,
                    CurrencyTypeId = 2,
                    Name = "Iconic",
                    Abbreviation = "ICON"
                }, new Currency
                {
                    Id = 1368,
                    CurrencyTypeId = 2,
                    Name = "Veltor",
                    Abbreviation = "VLT"
                }, new Currency
                {
                    Id = 1291,
                    CurrencyTypeId = 2,
                    Name = "Comet",
                    Abbreviation = "CMT"
                }, new Currency
                {
                    Id = 1509,
                    CurrencyTypeId = 2,
                    Name = "BenjiRolls",
                    Abbreviation = "BENJI"
                }, new Currency
                {
                    Id = 2100,
                    CurrencyTypeId = 2,
                    Name = "JavaScript Token",
                    Abbreviation = "JS"
                }, new Currency
                {
                    Id = 1546,
                    CurrencyTypeId = 2,
                    Name = "Centurion",
                    Abbreviation = "CNT"
                }, new Currency
                {
                    Id = 1052,
                    CurrencyTypeId = 2,
                    Name = "VectorAI",
                    Abbreviation = "VEC2"
                }, new Currency
                {
                    Id = 1774,
                    CurrencyTypeId = 2,
                    Name = "SocialCoin",
                    Abbreviation = "SOCC"
                }, new Currency
                {
                    Id = 1210,
                    CurrencyTypeId = 2,
                    Name = "Cabbage",
                    Abbreviation = "CAB"
                }, new Currency
                {
                    Id = 1716,
                    CurrencyTypeId = 2,
                    Name = "Ammo Reloaded",
                    Abbreviation = "AMMO"
                }, new Currency
                {
                    Id = 263,
                    CurrencyTypeId = 2,
                    Name = "PLNcoin",
                    Abbreviation = "PLNC"
                }, new Currency
                {
                    Id = 3163,
                    CurrencyTypeId = 2,
                    Name = "Mero",
                    Abbreviation = "MERO"
                }, new Currency
                {
                    Id = 2769,
                    CurrencyTypeId = 2,
                    Name = "Rhenium",
                    Abbreviation = "XRH"
                }, new Currency
                {
                    Id = 3427,
                    CurrencyTypeId = 2,
                    Name = "Agrolot",
                    Abbreviation = "AGLT"
                }, new Currency
                {
                    Id = 1559,
                    CurrencyTypeId = 2,
                    Name = "Renos",
                    Abbreviation = "RNS"
                }, new Currency
                {
                    Id = 1657,
                    CurrencyTypeId = 2,
                    Name = "Bitvolt",
                    Abbreviation = "VOLT"
                }, new Currency
                {
                    Id = 3298,
                    CurrencyTypeId = 2,
                    Name = "UralsCoin",
                    Abbreviation = "URALS"
                }, new Currency
                {
                    Id = 857,
                    CurrencyTypeId = 2,
                    Name = "SongCoin",
                    Abbreviation = "SONG"
                }, new Currency
                {
                    Id = 2131,
                    CurrencyTypeId = 2,
                    Name = "iBTC",
                    Abbreviation = "IBTC"
                }, new Currency
                {
                    Id = 831,
                    CurrencyTypeId = 2,
                    Name = "Wild Beast Block",
                    Abbreviation = "WBB"
                }, new Currency
                {
                    Id = 3308,
                    CurrencyTypeId = 2,
                    Name = "Xchange",
                    Abbreviation = "XCG"
                }, new Currency
                {
                    Id = 1824,
                    CurrencyTypeId = 2,
                    Name = "BitCoal",
                    Abbreviation = "COAL"
                }, new Currency
                {
                    Id = 1825,
                    CurrencyTypeId = 2,
                    Name = "LiteBitcoin",
                    Abbreviation = "LBTC"
                }, new Currency
                {
                    Id = 3235,
                    CurrencyTypeId = 2,
                    Name = "FolmCoin",
                    Abbreviation = "FLM"
                }, new Currency
                {
                    Id = 3578,
                    CurrencyTypeId = 2,
                    Name = "CoinToGo",
                    Abbreviation = "2GO"
                }, new Currency
                {
                    Id = 3677,
                    CurrencyTypeId = 2,
                    Name = "ROIyal Coin",
                    Abbreviation = "ROCO"
                }, new Currency
                {
                    Id = 3394,
                    CurrencyTypeId = 2,
                    Name = "Ourcoin",
                    Abbreviation = "OUR"
                }, new Currency
                {
                    Id = 2214,
                    CurrencyTypeId = 2,
                    Name = "ZoZoCoin",
                    Abbreviation = "ZZC"
                }, new Currency
                {
                    Id = 986,
                    CurrencyTypeId = 2,
                    Name = "CrevaCoin",
                    Abbreviation = "CREVA"
                }, new Currency
                {
                    Id = 2115,
                    CurrencyTypeId = 2,
                    Name = "PlayerCoin",
                    Abbreviation = "PLACO"
                }, new Currency
                {
                    Id = 2581,
                    CurrencyTypeId = 2,
                    Name = "Sharpe Platform Token",
                    Abbreviation = "SHP"
                }, new Currency
                {
                    Id = 1259,
                    CurrencyTypeId = 2,
                    Name = "PonziCoin",
                    Abbreviation = "PONZI"
                }, new Currency
                {
                    Id = 1632,
                    CurrencyTypeId = 2,
                    Name = "Concoin",
                    Abbreviation = "CONX"
                }, new Currency
                {
                    Id = 1523,
                    CurrencyTypeId = 2,
                    Name = "Magnum",
                    Abbreviation = "MGM"
                }, new Currency
                {
                    Id = 3232,
                    CurrencyTypeId = 2,
                    Name = "Staker",
                    Abbreviation = "STR"
                }, new Currency
                {
                    Id = 1691,
                    CurrencyTypeId = 2,
                    Name = "Project-X",
                    Abbreviation = "NANOX"
                }, new Currency
                {
                    Id = 994,
                    CurrencyTypeId = 2,
                    Name = "AnarchistsPrime",
                    Abbreviation = "ACP"
                }, new Currency
                {
                    Id = 1630,
                    CurrencyTypeId = 2,
                    Name = "Coinonat",
                    Abbreviation = "CXT"
                }, new Currency
                {
                    Id = 3481,
                    CurrencyTypeId = 2,
                    Name = "Peony",
                    Abbreviation = "PNY"
                }, new Currency
                {
                    Id = 1090,
                    CurrencyTypeId = 2,
                    Name = "Save and Gain",
                    Abbreviation = "SANDG"
                }, new Currency
                {
                    Id = 3384,
                    CurrencyTypeId = 2,
                    Name = "Benz",
                    Abbreviation = "BENZ"
                }, new Currency
                {
                    Id = 1994,
                    CurrencyTypeId = 2,
                    Name = "Interzone",
                    Abbreviation = "ITZ"
                }, new Currency
                {
                    Id = 3025,
                    CurrencyTypeId = 2,
                    Name = "ACRE",
                    Abbreviation = "ACRE"
                }, new Currency
                {
                    Id = 3494,
                    CurrencyTypeId = 2,
                    Name = "Rocketcoin",
                    Abbreviation = "ROCK"
                }, new Currency
                {
                    Id = 3486,
                    CurrencyTypeId = 2,
                    Name = "dietbitcoin",
                    Abbreviation = "DDX"
                }, new Currency
                {
                    Id = 1558,
                    CurrencyTypeId = 2,
                    Name = "Argus",
                    Abbreviation = "ARGUS"
                }, new Currency
                {
                    Id = 3524,
                    CurrencyTypeId = 2,
                    Name = "MFIT COIN",
                    Abbreviation = "MFIT"
                }, new Currency
                {
                    Id = 2045,
                    CurrencyTypeId = 2,
                    Name = "Coimatic 3.0",
                    Abbreviation = "CTIC3"
                }, new Currency
                {
                    Id = 3391,
                    CurrencyTypeId = 2,
                    Name = "SmartFox",
                    Abbreviation = "FOX"
                }, new Currency
                {
                    Id = 3453,
                    CurrencyTypeId = 2,
                    Name = "CJs",
                    Abbreviation = "CJS"
                }, new Currency
                {
                    Id = 3401,
                    CurrencyTypeId = 2,
                    Name = "SHADE Token",
                    Abbreviation = "SHADE"
                }, new Currency
                {
                    Id = 2253,
                    CurrencyTypeId = 2,
                    Name = "Jiyo [OLD]",
                    Abbreviation = "JIYO"
                }, new Currency
                {
                    Id = 2596,
                    CurrencyTypeId = 2,
                    Name = "CK USD",
                    Abbreviation = "CKUSD"
                }, new Currency
                {
                    Id = 3766,
                    CurrencyTypeId = 2,
                    Name = "Fatcoin",
                    Abbreviation = "FAT"
                }, new Currency
                {
                    Id = 3897,
                    CurrencyTypeId = 2,
                    Name = "OKB",
                    Abbreviation = "OKB"
                }, new Currency
                {
                    Id = 3946,
                    CurrencyTypeId = 2,
                    Name = "Carry",
                    Abbreviation = "CRE"
                }, new Currency
                {
                    Id = 3351,
                    CurrencyTypeId = 2,
                    Name = "ZB",
                    Abbreviation = "ZB"
                }, new Currency
                {
                    Id = 3930,
                    CurrencyTypeId = 2,
                    Name = "Thunder Token",
                    Abbreviation = "TT"
                }, new Currency
                {
                    Id = 3806,
                    CurrencyTypeId = 2,
                    Name = "TigerCash",
                    Abbreviation = "TCH"
                }, new Currency
                {
                    Id = 3860,
                    CurrencyTypeId = 2,
                    Name = "Blockcloud",
                    Abbreviation = "BLOC"
                }, new Currency
                {
                    Id = 3964,
                    CurrencyTypeId = 2,
                    Name = "Reserve Rights",
                    Abbreviation = "RSR"
                }, new Currency
                {
                    Id = 3891,
                    CurrencyTypeId = 2,
                    Name = "V-Dimension",
                    Abbreviation = "VOLLAR"
                }, new Currency
                {
                    Id = 2280,
                    CurrencyTypeId = 2,
                    Name = "Filecoin [Futures]",
                    Abbreviation = "FIL"
                }, new Currency
                {
                    Id = 3217,
                    CurrencyTypeId = 2,
                    Name = "Ontology Gas",
                    Abbreviation = "ONG"
                }, new Currency
                {
                    Id = 2907,
                    CurrencyTypeId = 2,
                    Name = "Karatgold Coin",
                    Abbreviation = "KBC"
                }, new Currency
                {
                    Id = 1706,
                    CurrencyTypeId = 2,
                    Name = "Aidos Kuneen",
                    Abbreviation = "ADK"
                }, new Currency
                {
                    Id = 3957,
                    CurrencyTypeId = 2,
                    Name = "UNUS SED LEO",
                    Abbreviation = "LEO"
                }, new Currency
                {
                    Id = 3252,
                    CurrencyTypeId = 2,
                    Name = "ShineChain",
                    Abbreviation = "SHE"
                }, new Currency
                {
                    Id = 3914,
                    CurrencyTypeId = 2,
                    Name = "GlitzKoin",
                    Abbreviation = "GTN"
                }, new Currency
                {
                    Id = 2878,
                    CurrencyTypeId = 2,
                    Name = "DigiFinexToken",
                    Abbreviation = "DFT"
                }, new Currency
                {
                    Id = 3295,
                    CurrencyTypeId = 2,
                    Name = "BUMO",
                    Abbreviation = "BU"
                }, new Currency
                {
                    Id = 2997,
                    CurrencyTypeId = 2,
                    Name = "DIPNET",
                    Abbreviation = "DPN"
                }, new Currency
                {
                    Id = 2895,
                    CurrencyTypeId = 2,
                    Name = "Coni",
                    Abbreviation = "CONI"
                }, new Currency
                {
                    Id = 3673,
                    CurrencyTypeId = 2,
                    Name = "BitMax Token",
                    Abbreviation = "BTMX"
                }, new Currency
                {
                    Id = 2441,
                    CurrencyTypeId = 2,
                    Name = "Molecular Future",
                    Abbreviation = "MOF"
                }, new Currency
                {
                    Id = 3188,
                    CurrencyTypeId = 2,
                    Name = "SuperEdge",
                    Abbreviation = "ECT"
                }, new Currency
                {
                    Id = 3236,
                    CurrencyTypeId = 2,
                    Name = "WinToken",
                    Abbreviation = "WIN"
                }, new Currency
                {
                    Id = 3924,
                    CurrencyTypeId = 2,
                    Name = "DREP",
                    Abbreviation = "DREP"
                }, new Currency
                {
                    Id = 3238,
                    CurrencyTypeId = 2,
                    Name = "ABCC Token",
                    Abbreviation = "AT"
                }, new Currency
                {
                    Id = 3789,
                    CurrencyTypeId = 2,
                    Name = "Boltt Coin ",
                    Abbreviation = "BOLTT"
                }, new Currency
                {
                    Id = 2712,
                    CurrencyTypeId = 2,
                    Name = "MyToken",
                    Abbreviation = "MT"
                }, new Currency
                {
                    Id = 3883,
                    CurrencyTypeId = 2,
                    Name = "QuickX Protocol",
                    Abbreviation = "QCX"
                }, new Currency
                {
                    Id = 3704,
                    CurrencyTypeId = 2,
                    Name = "V Systems",
                    Abbreviation = "VSYS"
                }, new Currency
                {
                    Id = 2947,
                    CurrencyTypeId = 2,
                    Name = "SoPay",
                    Abbreviation = "SOP"
                }, new Currency
                {
                    Id = 3283,
                    CurrencyTypeId = 2,
                    Name = "FOIN",
                    Abbreviation = "FOIN"
                }, new Currency
                {
                    Id = 3962,
                    CurrencyTypeId = 2,
                    Name = "Vodi X",
                    Abbreviation = "VDX"
                }, new Currency
                {
                    Id = 2366,
                    CurrencyTypeId = 2,
                    Name = "FairGame",
                    Abbreviation = "FAIR"
                }, new Currency
                {
                    Id = 3653,
                    CurrencyTypeId = 2,
                    Name = "Baer Chain",
                    Abbreviation = "BRC"
                }, new Currency
                {
                    Id = 2941,
                    CurrencyTypeId = 2,
                    Name = "CoinEx Token",
                    Abbreviation = "CET"
                }, new Currency
                {
                    Id = 3905,
                    CurrencyTypeId = 2,
                    Name = "DUO Network Token",
                    Abbreviation = "DUO"
                }, new Currency
                {
                    Id = 3829,
                    CurrencyTypeId = 2,
                    Name = "Nash Exchange",
                    Abbreviation = "NEX"
                }, new Currency
                {
                    Id = 3296,
                    CurrencyTypeId = 2,
                    Name = "MINDOL",
                    Abbreviation = "MIN"
                }, new Currency
                {
                    Id = 2282,
                    CurrencyTypeId = 2,
                    Name = "Super Bitcoin",
                    Abbreviation = "SBTC"
                }, new Currency
                {
                    Id = 3053,
                    CurrencyTypeId = 2,
                    Name = "YOU COIN",
                    Abbreviation = "YOU"
                }, new Currency
                {
                    Id = 3937,
                    CurrencyTypeId = 2,
                    Name = "NNB Token",
                    Abbreviation = "NNB"
                }, new Currency
                {
                    Id = 2432,
                    CurrencyTypeId = 2,
                    Name = "StarChain",
                    Abbreviation = "STC"
                }, new Currency
                {
                    Id = 3721,
                    CurrencyTypeId = 2,
                    Name = "Huobi Pool Token",
                    Abbreviation = "HPT"
                }, new Currency
                {
                    Id = 3182,
                    CurrencyTypeId = 2,
                    Name = "HitChain",
                    Abbreviation = "HIT"
                }, new Currency
                {
                    Id = 2013,
                    CurrencyTypeId = 2,
                    Name = "Infinity Economics",
                    Abbreviation = "XIN"
                }, new Currency
                {
                    Id = 3874,
                    CurrencyTypeId = 2,
                    Name = "IRISnet",
                    Abbreviation = "IRIS"
                }, new Currency
                {
                    Id = 3846,
                    CurrencyTypeId = 2,
                    Name = "VeriBlock",
                    Abbreviation = "VBK"
                }, new Currency
                {
                    Id = 3832,
                    CurrencyTypeId = 2,
                    Name = "Big Bang Game Coin",
                    Abbreviation = "BBGC"
                }, new Currency
                {
                    Id = 3012,
                    CurrencyTypeId = 2,
                    Name = "VeThor Token",
                    Abbreviation = "VTHO"
                }, new Currency
                {
                    Id = 3485,
                    CurrencyTypeId = 2,
                    Name = "Game Stars",
                    Abbreviation = "GST"
                }, new Currency
                {
                    Id = 3934,
                    CurrencyTypeId = 2,
                    Name = "CNNS",
                    Abbreviation = "CNNS"
                }, new Currency
                {
                    Id = 2740,
                    CurrencyTypeId = 2,
                    Name = "Influence Chain",
                    Abbreviation = "INC"
                }, new Currency
                {
                    Id = 2454,
                    CurrencyTypeId = 2,
                    Name = "UnlimitedIP",
                    Abbreviation = "UIP"
                }, new Currency
                {
                    Id = 3196,
                    CurrencyTypeId = 2,
                    Name = "KNOW",
                    Abbreviation = "KNOW"
                }, new Currency
                {
                    Id = 2376,
                    CurrencyTypeId = 2,
                    Name = "TopChain",
                    Abbreviation = "TOPC"
                }, new Currency
                {
                    Id = 2456,
                    CurrencyTypeId = 2,
                    Name = "OFCOIN",
                    Abbreviation = "OF"
                }, new Currency
                {
                    Id = 3620,
                    CurrencyTypeId = 2,
                    Name = "Atlas Protocol",
                    Abbreviation = "ATP"
                }, new Currency
                {
                    Id = 3791,
                    CurrencyTypeId = 2,
                    Name = "Jewel",
                    Abbreviation = "JWL"
                }, new Currency
                {
                    Id = 3347,
                    CurrencyTypeId = 2,
                    Name = "CARAT",
                    Abbreviation = "CARAT"
                }, new Currency
                {
                    Id = 2981,
                    CurrencyTypeId = 2,
                    Name = "Consentium",
                    Abbreviation = "CSM"
                }, new Currency
                {
                    Id = 2435,
                    CurrencyTypeId = 2,
                    Name = "LightChain",
                    Abbreviation = "LIGHT"
                }, new Currency
                {
                    Id = 3875,
                    CurrencyTypeId = 2,
                    Name = "Valor Token",
                    Abbreviation = "VALOR"
                }, new Currency
                {
                    Id = 2987,
                    CurrencyTypeId = 2,
                    Name = "ThingsOperatingSystem",
                    Abbreviation = "TOS"
                }, new Currency
                {
                    Id = 3880,
                    CurrencyTypeId = 2,
                    Name = "OceanEx Token",
                    Abbreviation = "OCE"
                }, new Currency
                {
                    Id = 2741,
                    CurrencyTypeId = 2,
                    Name = "Intelligent Investment Chain",
                    Abbreviation = "IIC"
                }, new Currency
                {
                    Id = 2846,
                    CurrencyTypeId = 2,
                    Name = "FuturoCoin",
                    Abbreviation = "FTO"
                }, new Currency
                {
                    Id = 3734,
                    CurrencyTypeId = 2,
                    Name = "ELA Coin",
                    Abbreviation = "ELAC"
                }, new Currency
                {
                    Id = 3795,
                    CurrencyTypeId = 2,
                    Name = "ZEON",
                    Abbreviation = "ZEON"
                }, new Currency
                {
                    Id = 2396,
                    CurrencyTypeId = 2,
                    Name = "WETH",
                    Abbreviation = "WETH"
                }, new Currency
                {
                    Id = 3060,
                    CurrencyTypeId = 2,
                    Name = "Yuan Chain Coin",
                    Abbreviation = "YCC"
                }, new Currency
                {
                    Id = 3888,
                    CurrencyTypeId = 2,
                    Name = "bitCEO",
                    Abbreviation = "BCEO"
                }, new Currency
                {
                    Id = 3660,
                    CurrencyTypeId = 2,
                    Name = "USDCoin",
                    Abbreviation = "USC"
                }, new Currency
                {
                    Id = 3797,
                    CurrencyTypeId = 2,
                    Name = "Bitex Global XBX Coin",
                    Abbreviation = "XBX"
                }, new Currency
                {
                    Id = 2928,
                    CurrencyTypeId = 2,
                    Name = "PlayCoin [QRC20]",
                    Abbreviation = "PLY"
                }, new Currency
                {
                    Id = 3259,
                    CurrencyTypeId = 2,
                    Name = "YouLive Coin",
                    Abbreviation = "UC"
                }, new Currency
                {
                    Id = 3938,
                    CurrencyTypeId = 2,
                    Name = "Muzika",
                    Abbreviation = "MZK"
                }, new Currency
                {
                    Id = 2091,
                    CurrencyTypeId = 2,
                    Name = "Exchange Union",
                    Abbreviation = "XUC"
                }, new Currency
                {
                    Id = 2871,
                    CurrencyTypeId = 2,
                    Name = "Ubique Chain Of Things",
                    Abbreviation = "UCT"
                }, new Currency
                {
                    Id = 3707,
                    CurrencyTypeId = 2,
                    Name = "T.OS",
                    Abbreviation = "TOSC"
                }, new Currency
                {
                    Id = 2969,
                    CurrencyTypeId = 2,
                    Name = "Globalvillage Ecosystem",
                    Abbreviation = "GVE"
                }, new Currency
                {
                    Id = 3872,
                    CurrencyTypeId = 2,
                    Name = "Bilaxy Token",
                    Abbreviation = "BIA"
                }, new Currency
                {
                    Id = 3873,
                    CurrencyTypeId = 2,
                    Name = "botXcoin",
                    Abbreviation = "BOTX"
                }, new Currency
                {
                    Id = 3858,
                    CurrencyTypeId = 2,
                    Name = "FNB Protocol",
                    Abbreviation = "FNB"
                }, new Currency
                {
                    Id = 3803,
                    CurrencyTypeId = 2,
                    Name = "Diruna",
                    Abbreviation = "DRA"
                }, new Currency
                {
                    Id = 2361,
                    CurrencyTypeId = 2,
                    Name = "Show",
                    Abbreviation = "SHOW"
                }, new Currency
                {
                    Id = 3970,
                    CurrencyTypeId = 2,
                    Name = "Trias",
                    Abbreviation = "TRY"
                }, new Currency
                {
                    Id = 3061,
                    CurrencyTypeId = 2,
                    Name = "Promotion Coin",
                    Abbreviation = "PC"
                }, new Currency
                {
                    Id = 2950,
                    CurrencyTypeId = 2,
                    Name = "LemoChain",
                    Abbreviation = "LEMO"
                }, new Currency
                {
                    Id = 3898,
                    CurrencyTypeId = 2,
                    Name = "AXE",
                    Abbreviation = "AXE"
                }, new Currency
                {
                    Id = 2734,
                    CurrencyTypeId = 2,
                    Name = "EduCoin",
                    Abbreviation = "EDU"
                }, new Currency
                {
                    Id = 3929,
                    CurrencyTypeId = 2,
                    Name = "BQT",
                    Abbreviation = "BQTX"
                }, new Currency
                {
                    Id = 2434,
                    CurrencyTypeId = 2,
                    Name = "Maggie",
                    Abbreviation = "MAG"
                }, new Currency
                {
                    Id = 3595,
                    CurrencyTypeId = 2,
                    Name = "PalletOne",
                    Abbreviation = "PTN"
                }, new Currency
                {
                    Id = 2852,
                    CurrencyTypeId = 2,
                    Name = "Engine",
                    Abbreviation = "EGCC"
                }, new Currency
                {
                    Id = 2914,
                    CurrencyTypeId = 2,
                    Name = "BeeKan",
                    Abbreviation = "BKBT"
                }, new Currency
                {
                    Id = 3258,
                    CurrencyTypeId = 2,
                    Name = "BitUP Token",
                    Abbreviation = "BUT"
                }, new Currency
                {
                    Id = 3320,
                    CurrencyTypeId = 2,
                    Name = "TCOIN",
                    Abbreviation = "TCN"
                }, new Currency
                {
                    Id = 3922,
                    CurrencyTypeId = 2,
                    Name = "Tarush",
                    Abbreviation = "TAS"
                }, new Currency
                {
                    Id = 2247,
                    CurrencyTypeId = 2,
                    Name = "BlockCDN",
                    Abbreviation = "BCDN"
                }, new Currency
                {
                    Id = 3004,
                    CurrencyTypeId = 2,
                    Name = "Volt",
                    Abbreviation = "ACDC"
                }, new Currency
                {
                    Id = 3825,
                    CurrencyTypeId = 2,
                    Name = "PUBLYTO Token",
                    Abbreviation = "PUB"
                }, new Currency
                {
                    Id = 3958,
                    CurrencyTypeId = 2,
                    Name = "RedFOX Labs",
                    Abbreviation = "RFOX"
                }, new Currency
                {
                    Id = 3831,
                    CurrencyTypeId = 2,
                    Name = "Safe Haven",
                    Abbreviation = "SHA"
                }, new Currency
                {
                    Id = 3950,
                    CurrencyTypeId = 2,
                    Name = "Netrum",
                    Abbreviation = "NTR"
                }, new Currency
                {
                    Id = 3153,
                    CurrencyTypeId = 2,
                    Name = "Twinkle",
                    Abbreviation = "TKT"
                }, new Currency
                {
                    Id = 3090,
                    CurrencyTypeId = 2,
                    Name = "Wiki Token",
                    Abbreviation = "WIKI"
                }, new Currency
                {
                    Id = 2986,
                    CurrencyTypeId = 2,
                    Name = "DACC",
                    Abbreviation = "DACC"
                }, new Currency
                {
                    Id = 2962,
                    CurrencyTypeId = 2,
                    Name = "CHEX",
                    Abbreviation = "CHEX"
                }, new Currency
                {
                    Id = 3626,
                    CurrencyTypeId = 2,
                    Name = "RSK Smart Bitcoin",
                    Abbreviation = "RBTC"
                }, new Currency
                {
                    Id = 2408,
                    CurrencyTypeId = 2,
                    Name = "Qube",
                    Abbreviation = "QUBE"
                }, new Currency
                {
                    Id = 3851,
                    CurrencyTypeId = 2,
                    Name = "Airline & Life Networking Token",
                    Abbreviation = "ALLN"
                }, new Currency
                {
                    Id = 3286,
                    CurrencyTypeId = 2,
                    Name = "MEX",
                    Abbreviation = "MEX"
                }, new Currency
                {
                    Id = 2470,
                    CurrencyTypeId = 2,
                    Name = "CoinMeet",
                    Abbreviation = "MEET"
                }, new Currency
                {
                    Id = 3136,
                    CurrencyTypeId = 2,
                    Name = "MEET.ONE",
                    Abbreviation = "MEETONE"
                }, new Currency
                {
                    Id = 2713,
                    CurrencyTypeId = 2,
                    Name = "KEY",
                    Abbreviation = "KEY"
                }, new Currency
                {
                    Id = 3134,
                    CurrencyTypeId = 2,
                    Name = "ETERNAL TOKEN",
                    Abbreviation = "XET"
                }, new Currency
                {
                    Id = 3949,
                    CurrencyTypeId = 2,
                    Name = "Asian Fintech",
                    Abbreviation = "AFIN"
                }, new Currency
                {
                    Id = 3967,
                    CurrencyTypeId = 2,
                    Name = "Eva Cash",
                    Abbreviation = "EVC"
                }, new Currency
                {
                    Id = 3705,
                    CurrencyTypeId = 2,
                    Name = "DEXTER",
                    Abbreviation = "DXR"
                }, new Currency
                {
                    Id = 3801,
                    CurrencyTypeId = 2,
                    Name = "BORA",
                    Abbreviation = "BORA"
                }, new Currency
                {
                    Id = 3780,
                    CurrencyTypeId = 2,
                    Name = "Sparkle",
                    Abbreviation = "SPRKL"
                }, new Currency
                {
                    Id = 3918,
                    CurrencyTypeId = 2,
                    Name = "Safe",
                    Abbreviation = "SAFE"
                }, new Currency
                {
                    Id = 2736,
                    CurrencyTypeId = 2,
                    Name = "InsurChain",
                    Abbreviation = "INSUR"
                }, new Currency
                {
                    Id = 3812,
                    CurrencyTypeId = 2,
                    Name = "Flexacoin",
                    Abbreviation = "FXC"
                }, new Currency
                {
                    Id = 3916,
                    CurrencyTypeId = 2,
                    Name = "ThoreNext",
                    Abbreviation = "THX"
                }, new Currency
                {
                    Id = 3952,
                    CurrencyTypeId = 2,
                    Name = "IOTW",
                    Abbreviation = "IOTW"
                }, new Currency
                {
                    Id = 2293,
                    CurrencyTypeId = 2,
                    Name = "United Bitcoin",
                    Abbreviation = "UBTC"
                }, new Currency
                {
                    Id = 3282,
                    CurrencyTypeId = 2,
                    Name = "Ti-Value",
                    Abbreviation = "TV"
                }, new Currency
                {
                    Id = 3817,
                    CurrencyTypeId = 2,
                    Name = "Solareum",
                    Abbreviation = "SLRM"
                }, new Currency
                {
                    Id = 3904,
                    CurrencyTypeId = 2,
                    Name = "Electronic Energy Coin",
                    Abbreviation = "E2C"
                }, new Currency
                {
                    Id = 3205,
                    CurrencyTypeId = 2,
                    Name = "VeriDocGlobal",
                    Abbreviation = "VDG"
                }, new Currency
                {
                    Id = 3747,
                    CurrencyTypeId = 2,
                    Name = "Aunite",
                    Abbreviation = "AUNIT"
                }, new Currency
                {
                    Id = 3959,
                    CurrencyTypeId = 2,
                    Name = "ALLUVA",
                    Abbreviation = "ALV"
                }, new Currency
                {
                    Id = 2281,
                    CurrencyTypeId = 2,
                    Name = "BitcoinX",
                    Abbreviation = "BCX"
                }, new Currency
                {
                    Id = 3960,
                    CurrencyTypeId = 2,
                    Name = "Coineal Token",
                    Abbreviation = "NEAL"
                }, new Currency
                {
                    Id = 3892,
                    CurrencyTypeId = 2,
                    Name = "DEXON",
                    Abbreviation = "DXN"
                }, new Currency
                {
                    Id = 3849,
                    CurrencyTypeId = 2,
                    Name = "WHEN Token",
                    Abbreviation = "WHEN"
                }, new Currency
                {
                    Id = 3367,
                    CurrencyTypeId = 2,
                    Name = "CryptalDash",
                    Abbreviation = "CRD"
                }, new Currency
                {
                    Id = 2746,
                    CurrencyTypeId = 2,
                    Name = "DasCoin",
                    Abbreviation = "DASC"
                }, new Currency
                {
                    Id = 2975,
                    CurrencyTypeId = 2,
                    Name = "FundToken",
                    Abbreviation = "FUNDZ"
                }, new Currency
                {
                    Id = 3839,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin Rhodium",
                    Abbreviation = "XRC"
                }, new Currency
                {
                    Id = 3827,
                    CurrencyTypeId = 2,
                    Name = "BUDDY",
                    Abbreviation = "BUD"
                }, new Currency
                {
                    Id = 2994,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin File",
                    Abbreviation = "BIFI"
                }, new Currency
                {
                    Id = 3915,
                    CurrencyTypeId = 2,
                    Name = "Merebel",
                    Abbreviation = "MERI"
                }, new Currency
                {
                    Id = 3927,
                    CurrencyTypeId = 2,
                    Name = "Atlas Token",
                    Abbreviation = "ATLS"
                }, new Currency
                {
                    Id = 1944,
                    CurrencyTypeId = 2,
                    Name = "Moving Cloud Coin",
                    Abbreviation = "MCC"
                }, new Currency
                {
                    Id = 3965,
                    CurrencyTypeId = 2,
                    Name = "TouchCon",
                    Abbreviation = "TOC"
                }, new Currency
                {
                    Id = 2719,
                    CurrencyTypeId = 2,
                    Name = "Cybereits",
                    Abbreviation = "CRE"
                }, new Currency
                {
                    Id = 3834,
                    CurrencyTypeId = 2,
                    Name = "Dexter G",
                    Abbreviation = "DXG"
                }, new Currency
                {
                    Id = 3522,
                    CurrencyTypeId = 2,
                    Name = "MESSE TOKEN",
                    Abbreviation = "MESSE"
                }, new Currency
                {
                    Id = 2440,
                    CurrencyTypeId = 2,
                    Name = "Read",
                    Abbreviation = "READ"
                }, new Currency
                {
                    Id = 3844,
                    CurrencyTypeId = 2,
                    Name = "Xtock",
                    Abbreviation = "XTX"
                }, new Currency
                {
                    Id = 3857,
                    CurrencyTypeId = 2,
                    Name = "GoldenFever",
                    Abbreviation = "GFR"
                }, new Currency
                {
                    Id = 3736,
                    CurrencyTypeId = 2,
                    Name = "Marginless",
                    Abbreviation = "MRS"
                }, new Currency
                {
                    Id = 3277,
                    CurrencyTypeId = 2,
                    Name = "vSportCoin",
                    Abbreviation = "VSC"
                }, new Currency
                {
                    Id = 3249,
                    CurrencyTypeId = 2,
                    Name = "Usechain Token",
                    Abbreviation = "USE"
                }, new Currency
                {
                    Id = 2903,
                    CurrencyTypeId = 2,
                    Name = "SEER",
                    Abbreviation = "SEER"
                }, new Currency
                {
                    Id = 3717,
                    CurrencyTypeId = 2,
                    Name = "Wrapped Bitcoin",
                    Abbreviation = "WBTC"
                }, new Currency
                {
                    Id = 2919,
                    CurrencyTypeId = 2,
                    Name = "DWS",
                    Abbreviation = "DWS"
                }, new Currency
                {
                    Id = 3231,
                    CurrencyTypeId = 2,
                    Name = "Blockchain Quotations Index Token",
                    Abbreviation = "BQT"
                }, new Currency
                {
                    Id = 3948,
                    CurrencyTypeId = 2,
                    Name = "TERA",
                    Abbreviation = "TERA"
                }, new Currency
                {
                    Id = 2406,
                    CurrencyTypeId = 2,
                    Name = "InvestDigital",
                    Abbreviation = "IDT"
                }, new Currency
                {
                    Id = 3582,
                    CurrencyTypeId = 2,
                    Name = "MediBit",
                    Abbreviation = "MEDIBIT"
                }, new Currency
                {
                    Id = 3925,
                    CurrencyTypeId = 2,
                    Name = "Tratok",
                    Abbreviation = "TRAT"
                }, new Currency
                {
                    Id = 3640,
                    CurrencyTypeId = 2,
                    Name = "Livepeer",
                    Abbreviation = "LPT"
                }, new Currency
                {
                    Id = 3720,
                    CurrencyTypeId = 2,
                    Name = "SDUSD",
                    Abbreviation = "SDUSD"
                }, new Currency
                {
                    Id = 3002,
                    CurrencyTypeId = 2,
                    Name = "Master Contract Token",
                    Abbreviation = "MCT"
                }, new Currency
                {
                    Id = 3074,
                    CurrencyTypeId = 2,
                    Name = "Experience Token",
                    Abbreviation = "EXT"
                }, new Currency
                {
                    Id = 3744,
                    CurrencyTypeId = 2,
                    Name = "WEBN token",
                    Abbreviation = "WEBN"
                }, new Currency
                {
                    Id = 3790,
                    CurrencyTypeId = 2,
                    Name = "RoboCalls",
                    Abbreviation = "RC20"
                }, new Currency
                {
                    Id = 3912,
                    CurrencyTypeId = 2,
                    Name = "W Green Pay",
                    Abbreviation = "WGP"
                }, new Currency
                {
                    Id = 2670,
                    CurrencyTypeId = 2,
                    Name = "Pixie Coin",
                    Abbreviation = "PXC"
                }, new Currency
                {
                    Id = 3123,
                    CurrencyTypeId = 2,
                    Name = "GSENetwork",
                    Abbreviation = "GSE"
                }, new Currency
                {
                    Id = 1037,
                    CurrencyTypeId = 2,
                    Name = "Agoras Tokens",
                    Abbreviation = "AGRS"
                }, new Currency
                {
                    Id = 3105,
                    CurrencyTypeId = 2,
                    Name = "Atlantis Blue Digital Token",
                    Abbreviation = "ABDT"
                }, new Currency
                {
                    Id = 3855,
                    CurrencyTypeId = 2,
                    Name = "Locus Chain",
                    Abbreviation = "LOCUS"
                }, new Currency
                {
                    Id = 2858,
                    CurrencyTypeId = 2,
                    Name = "Couchain",
                    Abbreviation = "COU"
                }, new Currency
                {
                    Id = 3885,
                    CurrencyTypeId = 2,
                    Name = "WPP TOKEN",
                    Abbreviation = "WPP"
                }, new Currency
                {
                    Id = 3670,
                    CurrencyTypeId = 2,
                    Name = "ROMToken",
                    Abbreviation = "ROM"
                }, new Currency
                {
                    Id = 3951,
                    CurrencyTypeId = 2,
                    Name = "Pirate Chain",
                    Abbreviation = "ARRR"
                }, new Currency
                {
                    Id = 3198,
                    CurrencyTypeId = 2,
                    Name = "KingXChain",
                    Abbreviation = "KXC"
                }, new Currency
                {
                    Id = 3910,
                    CurrencyTypeId = 2,
                    Name = "pEOS",
                    Abbreviation = "PEOS"
                }, new Currency
                {
                    Id = 3292,
                    CurrencyTypeId = 2,
                    Name = "CariNet",
                    Abbreviation = "CIT"
                }, new Currency
                {
                    Id = 2358,
                    CurrencyTypeId = 2,
                    Name = "Content and AD Network",
                    Abbreviation = "CAN"
                }, new Currency
                {
                    Id = 3448,
                    CurrencyTypeId = 2,
                    Name = "Commerce Data Connection",
                    Abbreviation = "CDC"
                }, new Currency
                {
                    Id = 3848,
                    CurrencyTypeId = 2,
                    Name = "OOOBTC TOKEN",
                    Abbreviation = "OBX"
                }, new Currency
                {
                    Id = 3257,
                    CurrencyTypeId = 2,
                    Name = "GazeCoin",
                    Abbreviation = "GZE"
                }, new Currency
                {
                    Id = 3127,
                    CurrencyTypeId = 2,
                    Name = "Themis",
                    Abbreviation = "GET"
                }, new Currency
                {
                    Id = 3693,
                    CurrencyTypeId = 2,
                    Name = "M2O",
                    Abbreviation = "M2O"
                }, new Currency
                {
                    Id = 2999,
                    CurrencyTypeId = 2,
                    Name = "Hdac",
                    Abbreviation = "HDAC"
                }, new Currency
                {
                    Id = 2700,
                    CurrencyTypeId = 2,
                    Name = "Celsius",
                    Abbreviation = "CEL"
                }, new Currency
                {
                    Id = 3328,
                    CurrencyTypeId = 2,
                    Name = "CMITCOIN",
                    Abbreviation = "CMIT"
                }, new Currency
                {
                    Id = 3940,
                    CurrencyTypeId = 2,
                    Name = "P2P Global Network",
                    Abbreviation = "P2PX"
                }, new Currency
                {
                    Id = 2383,
                    CurrencyTypeId = 2,
                    Name = "Jingtum Tech",
                    Abbreviation = "SWTC"
                }, new Currency
                {
                    Id = 3926,
                    CurrencyTypeId = 2,
                    Name = "Stellar Gold",
                    Abbreviation = "XLMG"
                }, new Currency
                {
                    Id = 3819,
                    CurrencyTypeId = 2,
                    Name = "GAMB",
                    Abbreviation = "GMB"
                }, new Currency
                {
                    Id = 3923,
                    CurrencyTypeId = 2,
                    Name = "VENJOCOIN",
                    Abbreviation = "VJC"
                }, new Currency
                {
                    Id = 2593,
                    CurrencyTypeId = 2,
                    Name = "Dragon Coins",
                    Abbreviation = "DRG"
                }, new Currency
                {
                    Id = 3356,
                    CurrencyTypeId = 2,
                    Name = "The Midas Touch Gold",
                    Abbreviation = "TMTG"
                }, new Currency
                {
                    Id = 3861,
                    CurrencyTypeId = 2,
                    Name = "Infinitus Token",
                    Abbreviation = "INF"
                }, new Currency
                {
                    Id = 2894,
                    CurrencyTypeId = 2,
                    Name = "OTCBTC Token",
                    Abbreviation = "OTB"
                }, new Currency
                {
                    Id = 3667,
                    CurrencyTypeId = 2,
                    Name = "Atomic Wallet Coin",
                    Abbreviation = "AWC"
                }, new Currency
                {
                    Id = 3681,
                    CurrencyTypeId = 2,
                    Name = "CENTERCOIN",
                    Abbreviation = "CENT"
                }, new Currency
                {
                    Id = 3968,
                    CurrencyTypeId = 2,
                    Name = "Elitium",
                    Abbreviation = "EUM"
                }, new Currency
                {
                    Id = 3076,
                    CurrencyTypeId = 2,
                    Name = "Endorsit",
                    Abbreviation = "EDS"
                }, new Currency
                {
                    Id = 2370,
                    CurrencyTypeId = 2,
                    Name = "Bitcoin God",
                    Abbreviation = "GOD"
                }, new Currency
                {
                    Id = 918,
                    CurrencyTypeId = 2,
                    Name = "Bubble",
                    Abbreviation = "BUB"
                }, new Currency
                {
                    Id = 3820,
                    CurrencyTypeId = 2,
                    Name = "BuckHathCoin",
                    Abbreviation = "BHIG"
                }, new Currency
                {
                    Id = 3941,
                    CurrencyTypeId = 2,
                    Name = "Fast Access Blockchain",
                    Abbreviation = "FAB"
                }, new Currency
                {
                    Id = 2618,
                    CurrencyTypeId = 2,
                    Name = "StockChain",
                    Abbreviation = "SCC"
                }, new Currency
                {
                    Id = 3881,
                    CurrencyTypeId = 2,
                    Name = "BitStash",
                    Abbreviation = "STASH"
                }, new Currency
                {
                    Id = 3896,
                    CurrencyTypeId = 2,
                    Name = "HoryouToken",
                    Abbreviation = "HYT"
                }, new Currency
                {
                    Id = 1135,
                    CurrencyTypeId = 2,
                    Name = "ClubCoin",
                    Abbreviation = "CLUB"
                }, new Currency
                {
                    Id = 2262,
                    CurrencyTypeId = 2,
                    Name = "COMSA [ETH]",
                    Abbreviation = "CMS"
                }, new Currency
                {
                    Id = 3193,
                    CurrencyTypeId = 2,
                    Name = "RRCoin",
                    Abbreviation = "RRC"
                }, new Currency
                {
                    Id = 3745,
                    CurrencyTypeId = 2,
                    Name = "BiNGO.Fun",
                    Abbreviation = "777"
                }, new Currency
                {
                    Id = 2888,
                    CurrencyTypeId = 2,
                    Name = "CarBlock",
                    Abbreviation = "CAR"
                }, new Currency
                {
                    Id = 3836,
                    CurrencyTypeId = 2,
                    Name = "HOT Token",
                    Abbreviation = "HOT"
                }, new Currency
                {
                    Id = 3955,
                    CurrencyTypeId = 2,
                    Name = "Rentoo",
                    Abbreviation = "RENTOO"
                }, new Currency
                {
                    Id = 3759,
                    CurrencyTypeId = 2,
                    Name = "Jinbi Token",
                    Abbreviation = "JNB"
                }, new Currency
                {
                    Id = 2655,
                    CurrencyTypeId = 2,
                    Name = "Monero Classic",
                    Abbreviation = "XMC"
                }, new Currency
                {
                    Id = 3117,
                    CurrencyTypeId = 2,
                    Name = "Social Lending Token",
                    Abbreviation = "SLT"
                }, new Currency
                {
                    Id = 3956,
                    CurrencyTypeId = 2,
                    Name = "BOMB",
                    Abbreviation = "BOMB"
                }, new Currency
                {
                    Id = 3907,
                    CurrencyTypeId = 2,
                    Name = "BitCash",
                    Abbreviation = "BITC"
                }, new Currency
                {
                    Id = 3507,
                    CurrencyTypeId = 2,
                    Name = "MicroBitcoin",
                    Abbreviation = "MBC"
                }, new Currency
                {
                    Id = 2911,
                    CurrencyTypeId = 2,
                    Name = "ORS Group",
                    Abbreviation = "ORS"
                }, new Currency
                {
                    Id = 3318,
                    CurrencyTypeId = 2,
                    Name = "Countinghouse",
                    Abbreviation = "CHT"
                }, new Currency
                {
                    Id = 2483,
                    CurrencyTypeId = 2,
                    Name = "OceanChain",
                    Abbreviation = "OC"
                }, new Currency
                {
                    Id = 3477,
                    CurrencyTypeId = 2,
                    Name = "Asura Coin",
                    Abbreviation = "ASA"
                }, new Currency
                {
                    Id = 3776,
                    CurrencyTypeId = 2,
                    Name = "GoldFund",
                    Abbreviation = "GFUN"
                }, new Currency
                {
                    Id = 3110,
                    CurrencyTypeId = 2,
                    Name = "NewsToken",
                    Abbreviation = "NEWOS"
                }, new Currency
                {
                    Id = 3417,
                    CurrencyTypeId = 2,
                    Name = "Future1coin",
                    Abbreviation = "F1C"
                }, new Currency
                {
                    Id = 2266,
                    CurrencyTypeId = 2,
                    Name = "COMSA [XEM]",
                    Abbreviation = "CMS"
                }, new Currency
                {
                    Id = 3369,
                    CurrencyTypeId = 2,
                    Name = "Kuende",
                    Abbreviation = "KUE"
                }, new Currency
                {
                    Id = 3936,
                    CurrencyTypeId = 2,
                    Name = "GNY",
                    Abbreviation = "GNY"
                }, new Currency
                {
                    Id = 1487,
                    CurrencyTypeId = 2,
                    Name = "Pabyosi Coin (Special)",
                    Abbreviation = "PCS"
                }, new Currency
                {
                    Id = 3895,
                    CurrencyTypeId = 2,
                    Name = "Matrexcoin",
                    Abbreviation = "MAC"
                }, new Currency
                {
                    Id = 3579,
                    CurrencyTypeId = 2,
                    Name = "APOT",
                    Abbreviation = "APOT"
                }, new Currency
                {
                    Id = 3007,
                    CurrencyTypeId = 2,
                    Name = "Haracoin",
                    Abbreviation = "HRC"
                }, new Currency
                {
                    Id = 3630,
                    CurrencyTypeId = 2,
                    Name = "Hercules",
                    Abbreviation = "HERC"
                }, new Currency
                {
                    Id = 3767,
                    CurrencyTypeId = 2,
                    Name = "1X2 COIN",
                    Abbreviation = "1X2"
                }, new Currency
                {
                    Id = 2842,
                    CurrencyTypeId = 2,
                    Name = "Bankera",
                    Abbreviation = "BNK"
                }, new Currency
                {
                    Id = 3380,
                    CurrencyTypeId = 2,
                    Name = "WIZBL",
                    Abbreviation = "WBL"
                }, new Currency
                {
                    Id = 3026,
                    CurrencyTypeId = 2,
                    Name = "Carlive Chain",
                    Abbreviation = "IOV"
                }, new Currency
                {
                    Id = 3253,
                    CurrencyTypeId = 2,
                    Name = "eosBLACK",
                    Abbreviation = "BLACK"
                }, new Currency
                {
                    Id = 2485,
                    CurrencyTypeId = 2,
                    Name = "Candy",
                    Abbreviation = "CANDY"
                }, new Currency
                {
                    Id = 2008,
                    CurrencyTypeId = 2,
                    Name = "MSD",
                    Abbreviation = "MSD"
                }, new Currency
                {
                    Id = 3456,
                    CurrencyTypeId = 2,
                    Name = "PlusOneCoin",
                    Abbreviation = "PLUS1"
                }, new Currency
                {
                    Id = 3743,
                    CurrencyTypeId = 2,
                    Name = "QUSD",
                    Abbreviation = "QUSD"
                }, new Currency
                {
                    Id = 3893,
                    CurrencyTypeId = 2,
                    Name = "NOW Token",
                    Abbreviation = "NOW"
                }, new Currency
                {
                    Id = 549,
                    CurrencyTypeId = 2,
                    Name = "Storjcoin X",
                    Abbreviation = "SJCX"
                }, new Currency
                {
                    Id = 3135,
                    CurrencyTypeId = 2,
                    Name = "CEDEX Coin",
                    Abbreviation = "CEDEX"
                }, new Currency
                {
                    Id = 3696,
                    CurrencyTypeId = 2,
                    Name = "SpectrumCash",
                    Abbreviation = "XSM"
                }, new Currency
                {
                    Id = 3865,
                    CurrencyTypeId = 2,
                    Name = "BIZKEY",
                    Abbreviation = "BZKY"
                }, new Currency
                {
                    Id = 1921,
                    CurrencyTypeId = 2,
                    Name = "SIGMAcoin",
                    Abbreviation = "SIGMA"
                }, new Currency
                {
                    Id = 1323,
                    CurrencyTypeId = 2,
                    Name = "First Bitcoin",
                    Abbreviation = "BIT"
                }, new Currency
                {
                    Id = 3908,
                    CurrencyTypeId = 2,
                    Name = "Decimated",
                    Abbreviation = "DIO"
                }, new Currency
                {
                    Id = 2654,
                    CurrencyTypeId = 2,
                    Name = "Budbo",
                    Abbreviation = "BUBO"
                }, new Currency
                {
                    Id = 3290,
                    CurrencyTypeId = 2,
                    Name = "Elliot Coin",
                    Abbreviation = "ELLI"
                }, new Currency
                {
                    Id = 3067,
                    CurrencyTypeId = 2,
                    Name = "XTRD",
                    Abbreviation = "XTRD"
                }, new Currency
                {
                    Id = 2522,
                    CurrencyTypeId = 2,
                    Name = "Superior Coin",
                    Abbreviation = "SUP"
                }, new Currency
                {
                    Id = 1809,
                    CurrencyTypeId = 2,
                    Name = "TerraNova",
                    Abbreviation = "TER"
                }, new Currency
                {
                    Id = 3326,
                    CurrencyTypeId = 2,
                    Name = "Crypto Harbor Exchange",
                    Abbreviation = "CHE"
                }, new Currency
                {
                    Id = 3939,
                    CurrencyTypeId = 2,
                    Name = "Tronipay",
                    Abbreviation = "TRP"
                }, new Currency
                {
                    Id = 3902,
                    CurrencyTypeId = 2,
                    Name = "MoneroV ",
                    Abbreviation = "XMV"
                }, new Currency
                {
                    Id = 3921,
                    CurrencyTypeId = 2,
                    Name = "Hilux",
                    Abbreviation = "HLX"
                }, new Currency
                {
                    Id = 58,
                    CurrencyTypeId = 2,
                    Name = "Sexcoin",
                    Abbreviation = "SXC"
                }, new Currency
                {
                    Id = 2018,
                    CurrencyTypeId = 2,
                    Name = "EncryptoTel [ETH]",
                    Abbreviation = "ETT"
                }, new Currency
                {
                    Id = 3963,
                    CurrencyTypeId = 2,
                    Name = "DreamTeam",
                    Abbreviation = "DREAM"
                }, new Currency
                {
                    Id = 2192,
                    CurrencyTypeId = 2,
                    Name = "GOLD Reward Token",
                    Abbreviation = "GRX"
                }, new Currency
                {
                    Id = 2329,
                    CurrencyTypeId = 2,
                    Name = "Hyper Pay",
                    Abbreviation = "HPY"
                }, new Currency
                {
                    Id = 3343,
                    CurrencyTypeId = 2,
                    Name = "ANON",
                    Abbreviation = "ANON"
                }, new Currency
                {
                    Id = 3697,
                    CurrencyTypeId = 2,
                    Name = "Gamblica",
                    Abbreviation = "GMBC"
                }, new Currency
                {
                    Id = 3726,
                    CurrencyTypeId = 2,
                    Name = "Bidooh DOOH Token",
                    Abbreviation = "DOOH"
                }, new Currency
                {
                    Id = 2046,
                    CurrencyTypeId = 2,
                    Name = "Bastonet",
                    Abbreviation = "BSN"
                }, new Currency
                {
                    Id = 3208,
                    CurrencyTypeId = 2,
                    Name = "YUKI",
                    Abbreviation = "YUKI"
                }, new Currency
                {
                    Id = 3069,
                    CurrencyTypeId = 2,
                    Name = "NAM COIN",
                    Abbreviation = "NAM"
                }, new Currency
                {
                    Id = 2609,
                    CurrencyTypeId = 2,
                    Name = "Lendroid Support Token",
                    Abbreviation = "LST"
                }, new Currency
                {
                    Id = 3490,
                    CurrencyTypeId = 2,
                    Name = "Valuto",
                    Abbreviation = "VLU"
                }, new Currency
                {
                    Id = 3363,
                    CurrencyTypeId = 2,
                    Name = "WXCOINS",
                    Abbreviation = "WXC"
                }, new Currency
                {
                    Id = 41,
                    CurrencyTypeId = 2,
                    Name = "Infinitecoin",
                    Abbreviation = "IFC"
                }, new Currency
                {
                    Id = 2755,
                    CurrencyTypeId = 2,
                    Name = "Hero",
                    Abbreviation = "HERO"
                }, new Currency
                {
                    Id = 3943,
                    CurrencyTypeId = 2,
                    Name = "NEOX",
                    Abbreviation = "NEOX"
                }, new Currency
                {
                    Id = 3169,
                    CurrencyTypeId = 2,
                    Name = "Hybrid Block",
                    Abbreviation = "HYB"
                }, new Currency
                {
                    Id = 3176,
                    CurrencyTypeId = 2,
                    Name = "Alttex",
                    Abbreviation = "ALTX"
                }, new Currency
                {
                    Id = 3802,
                    CurrencyTypeId = 2,
                    Name = "Cryptoinvest",
                    Abbreviation = "CTT"
                }, new Currency
                {
                    Id = 3272,
                    CurrencyTypeId = 2,
                    Name = "Coin2Play",
                    Abbreviation = "C2P"
                }, new Currency
                {
                    Id = 3671,
                    CurrencyTypeId = 2,
                    Name = "Almeela",
                    Abbreviation = "KZE"
                }, new Currency
                {
                    Id = 1863,
                    CurrencyTypeId = 2,
                    Name = "Minex",
                    Abbreviation = "MINEX"
                }, new Currency
                {
                    Id = 2201,
                    CurrencyTypeId = 2,
                    Name = "WINCOIN",
                    Abbreviation = "WC"
                }, new Currency
                {
                    Id = 3276,
                    CurrencyTypeId = 2,
                    Name = "SaveNode",
                    Abbreviation = "SNO"
                }, new Currency
                {
                    Id = 3382,
                    CurrencyTypeId = 2,
                    Name = "CARDbuyers",
                    Abbreviation = "BCARD"
                }, new Currency
                {
                    Id = 3425,
                    CurrencyTypeId = 2,
                    Name = "EmaratCoin",
                    Abbreviation = "AEC"
                }, new Currency
                {
                    Id = 3304,
                    CurrencyTypeId = 2,
                    Name = "MobilinkToken",
                    Abbreviation = "MOLK"
                }, new Currency
                {
                    Id = 2072,
                    CurrencyTypeId = 2,
                    Name = "SegWit2x",
                    Abbreviation = "B2X"
                }, new Currency
                {
                    Id = 3160,
                    CurrencyTypeId = 2,
                    Name = "Infinipay",
                    Abbreviation = "IFP"
                }, new Currency
                {
                    Id = 3381,
                    CurrencyTypeId = 2,
                    Name = "Civitas",
                    Abbreviation = "CIV"
                }, new Currency
                {
                    Id = 2119,
                    CurrencyTypeId = 2,
                    Name = "BTCMoon",
                    Abbreviation = "BTCM"
                }, new Currency
                {
                    Id = 3073,
                    CurrencyTypeId = 2,
                    Name = "Esports Token",
                    Abbreviation = "EST"
                }, new Currency
                {
                    Id = 3212,
                    CurrencyTypeId = 2,
                    Name = "CottonCoin",
                    Abbreviation = "COTN"
                }, new Currency
                {
                    Id = 3059,
                    CurrencyTypeId = 2,
                    Name = "EscrowCoin",
                    Abbreviation = "ESCO"
                }, new Currency
                {
                    Id = 3400,
                    CurrencyTypeId = 2,
                    Name = "X12 Coin",
                    Abbreviation = "X12"
                }, new Currency
                {
                    Id = 572,
                    CurrencyTypeId = 2,
                    Name = "RabbitCoin",
                    Abbreviation = "RBBT"
                }, new Currency
                {
                    Id = 3483,
                    CurrencyTypeId = 2,
                    Name = "OmenCoin",
                    Abbreviation = "OMEN"
                }, new Currency
                {
                    Id = 3516,
                    CurrencyTypeId = 2,
                    Name = "DarkPayCoin",
                    Abbreviation = "DKPC"
                }, new Currency
                {
                    Id = 2350,
                    CurrencyTypeId = 2,
                    Name = "GameChain System",
                    Abbreviation = "GCS"
                }, new Currency
                {
                    Id = 733,
                    CurrencyTypeId = 2,
                    Name = "Quotient",
                    Abbreviation = "XQN"
                }, new Currency
                {
                    Id = 1542,
                    CurrencyTypeId = 2,
                    Name = "Golos Gold",
                    Abbreviation = "GBG"
                }, new Currency
                {
                    Id = 3111,
                    CurrencyTypeId = 2,
                    Name = "PayDay Coin",
                    Abbreviation = "PDX"
                }, new Currency
                {
                    Id = 1351,
                    CurrencyTypeId = 2,
                    Name = "Aces",
                    Abbreviation = "ACES"
                }, new Currency
                {
                    Id = 3504,
                    CurrencyTypeId = 2,
                    Name = "HondaisCoin",
                    Abbreviation = "HNDC"
                }, new Currency
                {
                    Id = 3269,
                    CurrencyTypeId = 2,
                    Name = "Crypto Improvement Fund",
                    Abbreviation = "CIF"
                }, new Currency
                {
                    Id = 3450,
                    CurrencyTypeId = 2,
                    Name = "ShopZcoin",
                    Abbreviation = "SZC"
                }, new Currency
                {
                    Id = 1843,
                    CurrencyTypeId = 2,
                    Name = "EmberCoin",
                    Abbreviation = "EMB"
                }, new Currency
                {
                    Id = 1020,
                    CurrencyTypeId = 2,
                    Name = "Axiom",
                    Abbreviation = "AXIOM"
                }, new Currency
                {
                    Id = 2517,
                    CurrencyTypeId = 2,
                    Name = "Animation Vision Cash",
                    Abbreviation = "AVH"
                }, new Currency
                {
                    Id = 1398,
                    CurrencyTypeId = 2,
                    Name = "PROUD Money",
                    Abbreviation = "PROUD"
                }, new Currency
                {
                    Id = 3039,
                    CurrencyTypeId = 2,
                    Name = "Excaliburcoin",
                    Abbreviation = "EXC"
                }, new Currency
                {
                    Id = 3420,
                    CurrencyTypeId = 2,
                    Name = "Cobrabytes",
                    Abbreviation = "COBRA"
                }, new Currency
                {
                    Id = 3229,
                    CurrencyTypeId = 2,
                    Name = "Centaure",
                    Abbreviation = "CEN"
                }, new Currency
                {
                    Id = 1971,
                    CurrencyTypeId = 2,
                    Name = "iQuant",
                    Abbreviation = "IQT"
                }, new Currency
                {
                    Id = 1497,
                    CurrencyTypeId = 2,
                    Name = "Fargocoin",
                    Abbreviation = "FRGC"
                }, new Currency
                {
                    Id = 70,
                    CurrencyTypeId = 2,
                    Name = "BetaCoin",
                    Abbreviation = "BET"
                }, new Currency
                {
                    Id = 1146,
                    CurrencyTypeId = 2,
                    Name = "AvatarCoin",
                    Abbreviation = "AV"
                }, new Currency
                {
                    Id = 1164,
                    CurrencyTypeId = 2,
                    Name = "Francs",
                    Abbreviation = "FRN"
                }, new Currency
                {
                    Id = 1393,
                    CurrencyTypeId = 2,
                    Name = "Tellurion",
                    Abbreviation = "TELL"
                }, new Currency
                {
                    Id = 1436,
                    CurrencyTypeId = 2,
                    Name = "DynamicCoin",
                    Abbreviation = "DMC"
                }, new Currency
                {
                    Id = 1623,
                    CurrencyTypeId = 2,
                    Name = "BlazerCoin",
                    Abbreviation = "BLAZR"
                }, new Currency
                {
                    Id = 1679,
                    CurrencyTypeId = 2,
                    Name = "Halloween Coin",
                    Abbreviation = "HALLO"
                }, new Currency
                {
                    Id = 1849,
                    CurrencyTypeId = 2,
                    Name = "Birds",
                    Abbreviation = "BIRDS"
                }, new Currency
                {
                    Id = 1851,
                    CurrencyTypeId = 2,
                    Name = "ERA",
                    Abbreviation = "ERA"
                }, new Currency
                {
                    Id = 1865,
                    CurrencyTypeId = 2,
                    Name = "Wink",
                    Abbreviation = "WINK"
                }, new Currency
                {
                    Id = 2067,
                    CurrencyTypeId = 2,
                    Name = "Dutch Coin",
                    Abbreviation = "DUTCH"
                }, new Currency
                {
                    Id = 2077,
                    CurrencyTypeId = 2,
                    Name = "Runners",
                    Abbreviation = "RUNNERS"
                }, new Currency
                {
                    Id = 2101,
                    CurrencyTypeId = 2,
                    Name = "Ethereum Lite",
                    Abbreviation = "ELITE"
                }, new Currency
                {
                    Id = 2488,
                    CurrencyTypeId = 2,
                    Name = "ValueChain",
                    Abbreviation = "VLC"
                }, new Currency
                {
                    Id = 2515,
                    CurrencyTypeId = 2,
                    Name = "ACChain",
                    Abbreviation = "ACC"
                }, new Currency
                {
                    Id = 2647,
                    CurrencyTypeId = 2,
                    Name = "SnipCoin",
                    Abbreviation = "SNIP"
                }, new Currency
                {
                    Id = 2671,
                    CurrencyTypeId = 2,
                    Name = "Cropcoin",
                    Abbreviation = "CROP"
                }, new Currency
                {
                    Id = 2834,
                    CurrencyTypeId = 2,
                    Name = "ContractNet",
                    Abbreviation = "CNET"
                }, new Currency
                {
                    Id = 2857,
                    CurrencyTypeId = 2,
                    Name = "SalPay",
                    Abbreviation = "SAL"
                }, new Currency
                {
                    Id = 2904,
                    CurrencyTypeId = 2,
                    Name = "FToken",
                    Abbreviation = "FT"
                }, new Currency
                {
                    Id = 2943,
                    CurrencyTypeId = 2,
                    Name = "Rocket Pool",
                    Abbreviation = "RPL"
                }, new Currency
                {
                    Id = 2959,
                    CurrencyTypeId = 2,
                    Name = "WeToken",
                    Abbreviation = "WT"
                }, new Currency
                {
                    Id = 3100,
                    CurrencyTypeId = 2,
                    Name = "Ultra Salescloud",
                    Abbreviation = "UST"
                }, new Currency
                {
                    Id = 3107,
                    CurrencyTypeId = 2,
                    Name = "BingoCoin",
                    Abbreviation = "BOC"
                }, new Currency
                {
                    Id = 3109,
                    CurrencyTypeId = 2,
                    Name = "Ordocoin",
                    Abbreviation = "RDC"
                }, new Currency
                {
                    Id = 3143,
                    CurrencyTypeId = 2,
                    Name = "Pecunio",
                    Abbreviation = "PCO"
                }, new Currency
                {
                    Id = 3152,
                    CurrencyTypeId = 2,
                    Name = "Obitan Chain",
                    Abbreviation = "OBTC"
                }, new Currency
                {
                    Id = 3157,
                    CurrencyTypeId = 2,
                    Name = "Smart Application Chain",
                    Abbreviation = "SAC"
                }, new Currency
                {
                    Id = 3288,
                    CurrencyTypeId = 2,
                    Name = "Digital Asset Exchange Token",
                    Abbreviation = "DAXT"
                }, new Currency
                {
                    Id = 3309,
                    CurrencyTypeId = 2,
                    Name = "Concierge Coin",
                    Abbreviation = "CCC"
                }, new Currency
                {
                    Id = 3310,
                    CurrencyTypeId = 2,
                    Name = "ALLCOIN",
                    Abbreviation = "ALC"
                }, new Currency
                {
                    Id = 3358,
                    CurrencyTypeId = 2,
                    Name = "Helper Search Token",
                    Abbreviation = "HSN"
                }, new Currency
                {
                    Id = 3378,
                    CurrencyTypeId = 2,
                    Name = "Labh Coin",
                    Abbreviation = "LABH"
                }, new Currency
                {
                    Id = 3385,
                    CurrencyTypeId = 2,
                    Name = "GIGA",
                    Abbreviation = "XG"
                }, new Currency
                {
                    Id = 3405,
                    CurrencyTypeId = 2,
                    Name = "Pandemia",
                    Abbreviation = "PNDM"
                }, new Currency
                {
                    Id = 3458,
                    CurrencyTypeId = 2,
                    Name = "ZTCoin",
                    Abbreviation = "ZT"
                }, new Currency
                {
                    Id = 3470,
                    CurrencyTypeId = 2,
                    Name = "Dragon Token",
                    Abbreviation = "DT"
                }, new Currency
                {
                    Id = 3498,
                    CurrencyTypeId = 2,
                    Name = "CapdaxToken",
                    Abbreviation = "XCD"
                }, new Currency
                {
                    Id = 3500,
                    CurrencyTypeId = 2,
                    Name = "Delizia",
                    Abbreviation = "DELIZ"
                }, new Currency
                {
                    Id = 3587,
                    CurrencyTypeId = 2,
                    Name = "Bgogo Token",
                    Abbreviation = "BGG"
                }, new Currency
                {
                    Id = 3692,
                    CurrencyTypeId = 2,
                    Name = "TOKOK",
                    Abbreviation = "TOK"
                }, new Currency
                {
                    Id = 3864,
                    CurrencyTypeId = 2,
                    Name = "UTEMIS",
                    Abbreviation = "UTS"
                }, new Currency
                {
                    Id = 3866,
                    CurrencyTypeId = 2,
                    Name = "CONUN",
                    Abbreviation = "CON"
                }, new Currency
                {
                    Id = 3900,
                    CurrencyTypeId = 2,
                    Name = "Hellenic Node",
                    Abbreviation = "HN"
                }, new Currency
                {
                    Id = 3961,
                    CurrencyTypeId = 2,
                    Name = "BZEdge",
                    Abbreviation = "BZE"
                });
        }
    }
}