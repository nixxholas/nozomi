using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels
{
    public class AnalysedHistoricItemMap : BaseMap<AnalysedHistoricItem>
    {
        public AnalysedHistoricItemMap(EntityTypeBuilder<AnalysedHistoricItem> entityTypeBuilder) 
            : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(ahi => ahi.Id).HasName("AnalysedHistoricItem_PK_Id");
            entityTypeBuilder.Property(ahi => ahi.Id)
                .ValueGeneratedOnAdd();

            entityTypeBuilder.Property(ahi => ahi.Value).IsRequired();
            entityTypeBuilder.Property(ahi => ahi.HistoricDateTime).IsRequired();

            entityTypeBuilder.HasOne(ahi => ahi.AnalysedComponent)
                .WithMany(ac => ac.AnalysedHistoricItems).HasForeignKey(ac => ac.AnalysedComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}