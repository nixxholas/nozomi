using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RcdHistoricItemMap : BaseMap<RcdHistoricItem>
    {
        
        public RcdHistoricItemMap(EntityTypeBuilder<RcdHistoricItem> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rcdhi => rcdhi.Id).HasName("RcdHistoricItem_PK_Id");
            entityTypeBuilder.Property(rcdhi => rcdhi.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.Property(rcdhi => rcdhi.Value).HasDefaultValue(string.Empty);
            entityTypeBuilder.Property(rcdhi => rcdhi.HistoricDateTime).IsRequired();

            entityTypeBuilder.HasOne(rcdhi => rcdhi.RequestComponentDatum)
                .WithMany(rcd => rcd.RcdHistoricItems)
                .HasForeignKey(rcdhi => rcdhi.RequestComponentDatumId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}