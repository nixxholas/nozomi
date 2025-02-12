using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairRequest
{
    public class UpdateCurrencyPairRequestExample : IExamplesProvider<UpdateCurrencyPairRequest>
    {
        public UpdateCurrencyPairRequest GetExamples()
        {
            return new UpdateCurrencyPairRequest()
            {
                CurrencyPairId = 1,
                RequestType = RequestType.HttpPost,
                DataPath = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                Delay = 3600000,
                RequestComponents = new List<UpdateCurrencyPairComponent>()
                {
                    new UpdateCurrencyPairComponent()
                    {
                        Id = 1,
                        ComponentType = (long)GenericComponentType.Ask,
                        QueryComponent = "Ask"
                    }
                },
                RequestProperties = new List<UpdateRequestProperty>()
                {
                    new UpdateRequestProperty()
                    {
                        Id = 1,
                        RequestPropertyType = RequestPropertyType.HttpBody,
                        Key = "",
                        Value = "Test"
                    }
                }
            };
        }
    }
}