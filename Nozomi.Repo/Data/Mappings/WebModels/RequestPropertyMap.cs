using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Data.WebModels;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class RequestPropertyMap : BaseMap<RequestProperty>
    {
        public RequestPropertyMap(EntityTypeBuilder<RequestProperty> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(rp => rp.Id).HasName("RequestProperty_PK_Id");
            entityTypeBuilder.Property(rp => rp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(rp => rp.RequestPropertyType).IsRequired();

            entityTypeBuilder.Property(rp => rp.Key).IsRequired(false);
            entityTypeBuilder.Property(rp => rp.Value).IsRequired(false);

            entityTypeBuilder.HasOne(rp => rp.Request).WithMany(r => r.RequestProperties)
                .HasForeignKey(rp => rp.RequestId);

            entityTypeBuilder.HasData(
                new RequestProperty()
                {
                    Id = 1,
                    RequestPropertyType = RequestPropertyType.HttpQuery,
                    Key = "apiKey",
                    Value = "TV5HJJHNP8094BRO",
                    RequestId = 5
                },
                new RequestProperty()
                {
                    Id = 2,
                    RequestPropertyType = RequestPropertyType.HttpQuery,
                    Key = "function",
                    Value = "CURRENCY_EXCHANGE_RATE",
                    RequestId = 5
                },
                new RequestProperty()
                {
                    Id = 3,
                    RequestPropertyType = RequestPropertyType.HttpQuery,
                    Key = "from_currency",
                    Value = "USD",
                    RequestId = 5
                },
                new RequestProperty()
                {
                    Id = 4,
                    RequestPropertyType = RequestPropertyType.HttpQuery,
                    Key = "to_currency",
                    Value = "CNY",
                    RequestId = 5
                }
                );
        }
    }
}