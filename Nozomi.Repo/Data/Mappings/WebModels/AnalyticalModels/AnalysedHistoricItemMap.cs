using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.Models.Analytical;
using Nozomi.Repo.BCL;

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

            entityTypeBuilder.HasAlternateKey(r => r.Guid);
            entityTypeBuilder.Property(r => r.Guid).HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.Property(ahi => ahi.Value).IsRequired();
            entityTypeBuilder.Property(ahi => ahi.HistoricDateTime).IsRequired();

            entityTypeBuilder.HasOne(ahi => ahi.AnalysedComponent)
                .WithMany(ac => ac.AnalysedHistoricItems).HasForeignKey(ac => ac.AnalysedComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}