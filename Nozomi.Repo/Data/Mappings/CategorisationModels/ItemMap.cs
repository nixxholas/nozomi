using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Category;

namespace Nozomi.Repo.Data.Mappings.CategorisationModels
{
    public class ItemMap : BaseMap<Item>
    {
        public ItemMap(EntityTypeBuilder<Item> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(c => c.Slug).IsUnique();
            entityTypeBuilder.Property(c => c.Slug).IsRequired();

            entityTypeBuilder.Property(c => c.Abbreviation).IsRequired();
            entityTypeBuilder.Property(c => c.LogoPath).IsRequired()
                .HasDefaultValue("assets/svg/icons/question.svg");
            entityTypeBuilder.Property(c => c.Denominations).HasDefaultValue(0);
            entityTypeBuilder.Property(c => c.DenominationName).IsRequired(false);
            entityTypeBuilder.Property(c => c.Name).IsRequired();

            entityTypeBuilder.HasMany(e => e.ItemProperties)
                .WithOne(ip => ip.Item).HasForeignKey(e => e.ItemGuid)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(e => e.ItemSources)
                .WithOne(iso => iso.Item).HasForeignKey(e => e.ItemGuid)
                .OnDelete(DeleteBehavior.Cascade);
            entityTypeBuilder.HasMany(e => e.Requests)
                .WithOne(r => r.Item).HasForeignKey(r => r.ItemGuid)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}