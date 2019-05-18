using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Web;

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
                // ETH BFX Etherscan Request Property
                new RequestProperty
                {
                    Id = 1,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "module",
                    Value = "stats",
                    RequestId = 1
                },
                // ETH BFX Etherscan Request Property
                new RequestProperty
                {
                    Id = 2,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "action",
                    Value = "ethsupply",
                    RequestId = 1
                },
                // ETH BFX Etherscan Request Property
                new RequestProperty
                {
                    Id = 3,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "apikey",
                    Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                    RequestId = 1
                },
                new RequestProperty
                {
                    Id = 4,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "module",
                    Value = "stats",
                    RequestId = 2
                },
                new RequestProperty
                {
                    Id = 5,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "action",
                    Value = "tokensupply",
                    RequestId = 2
                },
                new RequestProperty
                {
                    Id = 6,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "contractaddress",
                    Value = "0xdd974d5c2e2928dea5f71b9825b8b646686bd200",
                    RequestId = 2
                },
                new RequestProperty
                {
                    Id = 7,
                    RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                    Key = "apikey",
                    Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                    RequestId = 2
                }
            );
        }
    }
}