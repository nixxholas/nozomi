using Microsoft.OpenApi.Models;
using Nozomi.Data.CurrencyModels;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nozomi.Data.AreaModels.v1.CurrencyPair
{
    public class CurrencyPairSchemaFilter : ISchemaFilter
    {
//        public void Apply(Schema schema, SchemaFilterContext context)
//        {
//            schema.Example = new CurrencyModels.CurrencyPair
//            {
//                CurrencyPairType = CurrencyPairType.UNKNOWN,
//                APIUrl = "https://counter.network/api/v1/ping",
//                DefaultComponent = "0",
//                CurrencySourceId = 0
//            };
//        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}