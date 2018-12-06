﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyMap : BaseMap<Currency>
    {
        public CurrencyMap(EntityTypeBuilder<Currency> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(c => c.Id);
            entityTypeBuilder.Property(c => c.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(c => c.Abbrv).IsRequired();
            entityTypeBuilder.Property(c => c.Name).IsRequired();

            entityTypeBuilder.HasOne(c => c.CurrencyType).WithMany(ct => ct.Currencies)
                .HasForeignKey(c => c.CurrencyTypeId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasOne(c => c.CurrencySource).WithMany(cs => cs.Currencies)
                .HasForeignKey(c => c.CurrencySourceId).OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(c => c.PartialCurrencyPairs).WithOne(pcp => pcp.Currency)
                .HasForeignKey(pcp => pcp.CurrencyId);

            entityTypeBuilder.HasData(
                new Currency()
                {
                    Id = 1,
                    CurrencyTypeId = 1,
                    Abbrv = "USD",
                    Name = "United States Dollar",
                    CurrencySourceId = 1,
                    WalletTypeId = 0
                },
                new Currency()
                {
                    Id = 2,
                    CurrencyTypeId = 2,
                    Abbrv = "ETH",
                    Name = "Ethereum",
                    CurrencySourceId = 1,
                    WalletTypeId = 1 // As per CNWallet
                },
                new Currency()
                {
                    Id = 3,
                    CurrencyTypeId = 2,
                    Abbrv = "KNC",
                    Name = "Kyber Network Coin",
                    CurrencySourceId = 1,
                    WalletTypeId = 4 // As per CNWallet
                },
                new Currency()
                {
                    Id = 4,
                    CurrencyTypeId = 2,
                    Abbrv = "KNC",
                    Name = "Kyber Network Coin",
                    CurrencySourceId = 3,
                    WalletTypeId = 4 // As per CNWallet
                },
                new Currency()
                {
                    Id = 5,
                    CurrencyTypeId = 2,
                    Abbrv = "ETH",
                    Name = "Ethereum",
                    CurrencySourceId = 3,
                    WalletTypeId = 1 // As per CNWallet
                },
                new Currency()
                {
                    Id = 6,
                    CurrencyTypeId = 1,
                    Abbrv = "EUR",
                    Name = "Euro",
                    CurrencySourceId = 4,
                    WalletTypeId = 0
                },
                new Currency()
                {
                    Id = 7,
                    CurrencyTypeId = 1,
                    Abbrv = "USD",
                    Name = "United States Dollar",
                    CurrencySourceId = 4,
                    WalletTypeId = 0
                }
            );
        }
    }
}
