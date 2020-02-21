using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class ComponentHistoricItemMap : BaseMap<ComponentHistoricItem>
    {
        
        public ComponentHistoricItemMap(EntityTypeBuilder<ComponentHistoricItem> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rcdhi => rcdhi.Guid).HasName("RcdHistoricItem_PK_Guid");
            entityTypeBuilder.Property(rcdhi => rcdhi.Guid).ValueGeneratedOnAdd()
                .HasDefaultValueSql("uuid_generate_v4()");
            
            entityTypeBuilder.Property(rcdhi => rcdhi.Value).HasDefaultValue(string.Empty);
            entityTypeBuilder.Property(rcdhi => rcdhi.HistoricDateTime).IsRequired();

            entityTypeBuilder.HasOne(rcdhi => rcdhi.Component)
                .WithMany(rc => rc.RcdHistoricItems)
                .HasForeignKey(rcdhi => rcdhi.RequestComponentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}