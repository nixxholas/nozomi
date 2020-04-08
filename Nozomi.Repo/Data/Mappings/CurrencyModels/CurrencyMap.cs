using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyMap : BaseMap<Currency>
    {
        public CurrencyMap(EntityTypeBuilder<Currency> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Items");
        
            entityTypeBuilder.HasKey(c => c.Id).HasName("Item_PK_Id");
            entityTypeBuilder.Property(c => c.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(c => c.Slug).IsUnique().HasName("Item_Index_Slug");
            entityTypeBuilder.Property(c => c.Slug).IsRequired();

            entityTypeBuilder.Property(c => c.Abbreviation).IsRequired();
            entityTypeBuilder.Property(c => c.LogoPath).IsRequired()
                .HasDefaultValue("assets/svg/icons/question.svg");
            entityTypeBuilder.Property(c => c.Denominations).HasDefaultValue(0);
            entityTypeBuilder.Property(c => c.DenominationName).IsRequired(false);
            entityTypeBuilder.Property(c => c.Name).IsRequired();

            entityTypeBuilder.HasMany(c => c.AnalysedComponents)
                .WithOne(ac => ac.Currency)
                .HasForeignKey(c => c.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Item_AnalysedComponents_Constraint")
                .IsRequired(false);
            entityTypeBuilder.HasOne(c => c.CurrencyType)
                .WithMany(ct => ct.Currencies)
                .HasForeignKey(c => c.CurrencyTypeId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Items_ItemType_Constraint");
            entityTypeBuilder.HasMany(c => c.CurrencySources)
                .WithOne(cs => cs.Currency)
                .HasForeignKey(cs => cs.CurrencyId).OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Item_CurrencySources_Constraint");
            entityTypeBuilder.HasMany(c => c.Requests)
                .WithOne(r => r.Currency)
                .HasForeignKey(r => r.CurrencyId).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Item_Requests_Constraint")
                .IsRequired(false);
        }
    }
}