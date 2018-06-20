using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyMap
    {
        public CurrencyMap(EntityTypeBuilder<Currency> entityTypeBuilder)
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
        }
    }
}
