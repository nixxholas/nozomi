using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RcdHistoricItemMap : BaseMap<ComponentHistoricItem>
    {
        
        public RcdHistoricItemMap(EntityTypeBuilder<ComponentHistoricItem> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rcdhi => rcdhi.Guid).HasName("RcdHistoricItem_PK_Id");
            entityTypeBuilder.Property(rcdhi => rcdhi.Guid).ValueGeneratedOnAdd();
            
            entityTypeBuilder.Property(rcdhi => rcdhi.Value).HasDefaultValue(string.Empty);
            entityTypeBuilder.Property(rcdhi => rcdhi.HistoricDateTime).IsRequired();

            entityTypeBuilder.HasOne(rcdhi => rcdhi.Component)
                .WithMany(rc => rc.RcdHistoricItems)
                .HasForeignKey(rcdhi => rcdhi.RequestComponentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}