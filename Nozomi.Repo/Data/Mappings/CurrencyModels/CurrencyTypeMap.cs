using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Categorisation;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyTypeMap : BaseMap<ItemType>
    {
        public CurrencyTypeMap(EntityTypeBuilder<ItemType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ct => ct.Id).HasName("CurrencyType_PK_Id");
            entityTypeBuilder.Property(ct => ct.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ct => ct.Name).IsRequired();
            entityTypeBuilder.Property(ct => ct.TypeShortForm).IsRequired();

            // entityTypeBuilder.HasMany(ct => ct.AnalysedComponents).WithOne(ac => ac.ItemType)
            //     .HasForeignKey(ac => ac.CurrencyTypeId).HasConstraintName("CurrencyType_AnalysedComponents_Constraint")
            //     .IsRequired(false);
            // entityTypeBuilder.HasMany(ct => ct.Items).WithOne(c => c.ItemType)
            //     .HasForeignKey(c => c.CurrencyTypeId).HasConstraintName("CurrencyType_Currencies_Constraint");
            // entityTypeBuilder.HasMany(ct => ct.Requests)
            //     .WithOne(r => r.ItemType).HasForeignKey(r => r.CurrencyTypeId)
            //     .HasConstraintName("CurrencyType_Request_Constraint")
            //     .IsRequired(false);
        }
    }
}