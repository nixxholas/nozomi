using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class SourceTypeMap : BaseMap<SourceType>
    {
        public SourceTypeMap(EntityTypeBuilder<SourceType> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(e => e.Id).HasName("SourceType_Id_PK");
            entityTypeBuilder.Property(e => e.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.HasAlternateKey(e => e.Guid);
            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd();

            entityTypeBuilder.HasIndex(e => e.Abbreviation).IsUnique();
            entityTypeBuilder.Property(e => e.Abbreviation).IsRequired();

            entityTypeBuilder.Property(e => e.Name).IsRequired();
        }
    }
}