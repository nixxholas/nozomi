using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.Models;
using Nozomi.Repo.BCL;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestPropertyMap : BaseMap<RequestProperty>
    {
        public RequestPropertyMap(EntityTypeBuilder<RequestProperty> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rp => rp.Id).HasName("RequestProperty_PK_Id");
            entityTypeBuilder.Property(rp => rp.Id).ValueGeneratedOnAdd();
            
            entityTypeBuilder.HasAlternateKey(r => r.Guid);
            entityTypeBuilder.Property(r => r.Guid).HasDefaultValueSql("uuid_generate_v4()");

            entityTypeBuilder.Property(rp => rp.RequestPropertyType).IsRequired();

            entityTypeBuilder.Property(rp => rp.Key).IsRequired(false);
            entityTypeBuilder.Property(rp => rp.Value).IsRequired(false);

            entityTypeBuilder.HasOne(rp => rp.Request).WithMany(r => r.RequestProperties)
                .HasForeignKey(rp => rp.RequestId);
        }
    }
}