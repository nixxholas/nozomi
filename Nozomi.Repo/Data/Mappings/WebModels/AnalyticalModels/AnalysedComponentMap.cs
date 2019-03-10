using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels
{
    public class AnalysedComponentMap : BaseMap<AnalysedComponent>
    {
        public AnalysedComponentMap(EntityTypeBuilder<AnalysedComponent> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ac => ac.Id).HasName("AnalysedComponent_PK_Id");
            entityTypeBuilder.Property(ac => ac.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ac => ac.Value).IsRequired(false);
            entityTypeBuilder.Property(ac => ac.Delay).HasDefaultValue(86400000); // 24 hours
            entityTypeBuilder.Property(ac => ac.ComponentType).HasDefaultValue(AnalysedComponentType.Unknown);

            entityTypeBuilder.HasOne(ac => ac.Currency)
                .WithMany(c => c.AnalysedComponents).HasForeignKey(ac => ac.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasOne(ac => ac.Request)
                .WithMany(r => r.AnalysedComponents).HasForeignKey(ac => ac.RequestId)
                .OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasMany(ac => ac.AnalysedHistoricItems)
                .WithOne(ahi => ahi.AnalysedComponent).HasForeignKey(ac => ac.AnalysedComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}