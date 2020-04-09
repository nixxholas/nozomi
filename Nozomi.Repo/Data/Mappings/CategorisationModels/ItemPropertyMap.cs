using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Category;

namespace Nozomi.Repo.Data.Mappings.CategorisationModels
{
    public class ItemPropertyMap : BaseMap<ItemProperty>
    {
        public ItemPropertyMap(EntityTypeBuilder<ItemProperty> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => new {e.Guid, e.Name});

            entityTypeBuilder.Property(e => e.Name).IsRequired();
            
            entityTypeBuilder.Property(c => c.Value).IsRequired(false);

            entityTypeBuilder.HasOne(e => e.Item)
                .WithMany(i => i.ItemProperties).HasForeignKey(e => e.ItemGuid)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}