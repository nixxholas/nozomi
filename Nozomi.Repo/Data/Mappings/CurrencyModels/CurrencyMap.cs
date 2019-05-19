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

            entityTypeBuilder.HasAlternateKey(c => c.Abbreviation).HasName("Currency_AK_Abbreviation");
            entityTypeBuilder.Property(c => c.Abbreviation).IsRequired();

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
            entityTypeBuilder.HasMany(c => c.CurrencyCurrencyPairs).WithOne(pcp => pcp.Currency)
                .HasForeignKey(pcp => pcp.CurrencyId)
                .HasConstraintName("Currency_PartialCurrencyPairs_Constraint");

            entityTypeBuilder.HasData(new Currency
                {
                    Id = 1,
                    CurrencyTypeId = 1,
                    Abbreviation = "USD",
                    Name = "United States Dollar",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 2,
                    CurrencyTypeId = 1,
                    Abbreviation = "EUR",
                    Name = "Euro",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 3,
                    CurrencyTypeId = 2,
                    Abbreviation = "ETH",
                    Name = "Ethereum",
                    WalletTypeId = 1, // As per CNWallet
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
                    WalletTypeId = 4, // As per CNWallet
                },
                new Currency
                {
                    Id = 5,
                    CurrencyTypeId = 2,
                    Abbreviation = "KNC",
                    Name = "Kyber Network Crystal",
                    WalletTypeId = 4 // As per CNWallet
                },
                new Currency
                {
                    Id = 6,
                    CurrencyTypeId = 2,
                    Abbreviation = "ETH",
                    Name = "Ethereum",
                    WalletTypeId = 1, // As per CNWallet
                    Denominations = 18,
                    DenominationName = "Wei",
                },
                new Currency
                {
                    Id = 7,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTC",
                    Name = "Bitcoin",
                    WalletTypeId = 0, // As per CNWallet
                    Denominations = 8,
                    DenominationName = "Sat"
                },
                new Currency
                {
                    Id = 8,
                    CurrencyTypeId = 1,
                    Abbreviation = "EUR",
                    Name = "Euro",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 9,
                    CurrencyTypeId = 1,
                    Abbreviation = "USD",
                    Name = "United States Dollar",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 10,
                    CurrencyTypeId = 1,
                    Abbreviation = "EUR",
                    Name = "Euro",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 11,
                    CurrencyTypeId = 1,
                    Abbreviation = "USD",
                    Name = "United States Dollar",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 12,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTC",
                    Name = "Bitcoin",
                    WalletTypeId = 0,
                    Denominations = 8,
                    DenominationName = "Sat"
                },
                new Currency
                {
                    Id = 13,
                    CurrencyTypeId = 2,
                    Abbreviation = "BCN",
                    Name = "Bytecoin",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 14,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTS",
                    Name = "BitShares",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 15,
                    CurrencyTypeId = 1,
                    Abbreviation = "USDT",
                    Name = "Tether USD",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 16,
                    CurrencyTypeId = 2,
                    Abbreviation = "BTC",
                    Name = "Bitcoin",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 17,
                    CurrencyTypeId = 1,
                    Abbreviation = "SGD",
                    Name = "Singapore Dollar",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 18,
                    CurrencyTypeId = 2,
                    Abbreviation = "ETH",
                    Name = "Ethereum",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 19,
                    CurrencyTypeId = 1,
                    Abbreviation = "USD",
                    Name = "United States Dollar",
                    WalletTypeId = 0
                },
                new Currency
                {
                    Id = 20,
                    CurrencyTypeId = 2,
                    Abbreviation = "LTC",
                    Name = "Litecoin",
                    WalletTypeId = 0
                });
        }
    }
}
