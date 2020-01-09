using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class SourceTypeMap : BaseMap<SourceType>
    {
        public SourceTypeMap(EntityTypeBuilder<SourceType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Guid).HasName("SourceType_Guid_PK");
            entityTypeBuilder.Property(e => e.Guid)
                .ValueGeneratedOnAdd().
                HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.HasIndex(e => e.Abbreviation).IsUnique();
            entityTypeBuilder.Property(e => e.Abbreviation).IsRequired();

            entityTypeBuilder.Property(e => e.Name).IsRequired();

            entityTypeBuilder.HasMany(e => e.Sources)
                .WithOne(s => s.SourceType)
                .HasForeignKey(s => s.SourceTypeGuid).OnDelete(DeleteBehavior.Cascade);
        }
    }
}