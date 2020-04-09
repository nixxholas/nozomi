using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Category;

namespace Nozomi.Repo.Data.Mappings.CategorisationModels
{
    public class ItemSourceMap : BaseMap<ItemSource>
    {
        public ItemSourceMap(EntityTypeBuilder<ItemSource> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Id);
            entityTypeBuilder.Property(e => e.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.HasIndex(e => new { e.ItemGuid, e.SourceId }).IsUnique();

            entityTypeBuilder.HasOne(e => e.Item)
                .WithMany(i => i.ItemSources).HasForeignKey(e => e.ItemGuid)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();
            entityTypeBuilder.HasOne(e => e.Source)
                .WithMany(s => s.SourceItems).HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict).IsRequired();
        }
    }
}