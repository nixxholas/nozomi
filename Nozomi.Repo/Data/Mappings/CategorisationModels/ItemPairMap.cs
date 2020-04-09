using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Category;

namespace Nozomi.Repo.Data.Mappings.CategorisationModels
{
    public class ItemPairMap : BaseMap<ItemPair>
    {
        public ItemPairMap(EntityTypeBuilder<ItemPair> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Id);
            entityTypeBuilder.Property(e => e.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(cp => new
            {
                cp.MainTicker, 
                cp.CounterTicker,
                cp.SourceId
            });

            entityTypeBuilder.Property(cp => cp.MainTicker)
                .HasConversion(val => val.ToUpperInvariant(), 
                    val => val);
            entityTypeBuilder.Property(cp => cp.CounterTicker)
                .HasConversion(val => val.ToUpperInvariant(), 
                    val => val);

            entityTypeBuilder.HasOne(e => e.Source)
                .WithMany(s => s.ItemPairs).HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(e => e.Requests)
                .WithOne(e => e.ItemPair).HasForeignKey(e => e.ItemPairGuid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}