using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Category;

namespace Nozomi.Repo.Data.Mappings.CategorisationModels
{
    public class ItemTypeMap : BaseMap<ItemType>
    {
        public ItemTypeMap(EntityTypeBuilder<ItemType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(e => e.Name).IsRequired();

            entityTypeBuilder.HasAlternateKey(e => e.Slug);
            entityTypeBuilder.Property(e => e.Slug).IsRequired();

            entityTypeBuilder.HasMany(e => e.AnalysedComponents)
                .WithOne(ac => ac.ItemType)
                .HasForeignKey(e => e.ItemTypeGuid)
                .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            entityTypeBuilder.HasMany(e => e.Items)
                .WithOne(i => i.ItemType).HasForeignKey(e => e.ItemTypeGuid)
                .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
            entityTypeBuilder.HasMany(e => e.Requests)
                .WithOne(r => r.ItemType).HasForeignKey(e => e.ItemTypeGuid)
                .OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        }
    }
}