using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.Core.Helpers.Mapping;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;

namespace Nozomi.Repo.Data.Mappings.WebModels
{
    public class CurrencyRequestMap : BaseMap<CurrencyRequest>
    {
        public CurrencyRequestMap(EntityTypeBuilder<CurrencyRequest> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasOne(cr => cr.Currency)
                .WithMany(c => c.CurrencyRequests).HasForeignKey(cr => cr.CurrencyId)
                .HasConstraintName("CurrencyRequest_Currency_Constraint");

            entityTypeBuilder.HasData(
                new CurrencyRequest
                {
                    Id = 1,
                    CurrencyId = 3,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000,
                    RequestComponents = new List<RequestComponent>
                    {
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Circulating_Supply,
                            IsDenominated = true,
                            QueryComponent = "result",
                            CreatedAt = DateTime.UtcNow,
                            ModifiedAt = DateTime.UtcNow,
                            DeletedAt = null
                        }
                    },
                    RequestProperties = new List<RequestProperty>
                    {
                        new RequestProperty
                        {
                            RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                            Key = "module",
                            Value = "stats",
                        },
                        new RequestProperty
                        {
                            RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                            Key = "action",
                            Value = "ethsupply",
                        },
                        new RequestProperty
                        {
                            RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                            Key = "apikey",
                            Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                        }
                    }
                },
                new CurrencyRequest
                {
                    Id = 2,
                    CurrencyId = 4,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.etherscan.io/api",
                    Delay = 5000,
                    RequestComponents = new List<RequestComponent>
                    {
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Circulating_Supply,
                            IsDenominated = true,
                            QueryComponent = "result",
                            CreatedAt = DateTime.UtcNow,
                            ModifiedAt = DateTime.UtcNow,
                            DeletedAt = null
                        }
                    },
                    RequestProperties = new List<RequestProperty>
                    {
                        new RequestProperty
                        {
                            RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                            Key = "module",
                            Value = "stats",
                        },
                        new RequestProperty
                        {
                            RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                            Key = "action",
                            Value = "tokensupply",
                        },
                        new RequestProperty
                        {
                            RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
                            Key = "apikey",
                            Value = "TGAFGMGDKHJ8W2EKI26MJRRWGH44AV9224",
                        }
                    }
                },
                new CurrencyRequest
                {
                    Id = 3,
                    CurrencyId = 12,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://insight.bitpay.com/api/status?q=getBlockCount",
                    Delay = 90000,
                    RequestComponents = new List<RequestComponent>
                    {
                        new RequestComponent
                        {
                            ComponentType = ComponentType.BlockCount,
                            QueryComponent = "info/blocks",
                            CreatedAt = DateTime.UtcNow,
                            ModifiedAt = DateTime.UtcNow,
                            DeletedAt = null
                        },
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Difficulty,
                            QueryComponent = "info/difficulty",
                            CreatedAt = DateTime.UtcNow,
                            ModifiedAt = DateTime.UtcNow,
                            DeletedAt = null
                        }
                    }
                },
                new CurrencyRequest
                {
                    Id = 4,
                    CurrencyId = 12,
                    Guid = Guid.NewGuid(),
                    RequestType = RequestType.HttpGet,
                    DataPath = "https://api.coinranking.com/v1/public/coin/1?base=USD",
                    Delay = 90000,
                    RequestComponents = new List<RequestComponent>
                    {
                        new RequestComponent
                        {
                            ComponentType = ComponentType.Circulating_Supply,
                            QueryComponent = "data/coin/circulatingSupply",
                            CreatedAt = DateTime.UtcNow,
                            ModifiedAt = DateTime.UtcNow,
                            DeletedAt = null
                        }
                    }
                }
            );
        }
    }
}