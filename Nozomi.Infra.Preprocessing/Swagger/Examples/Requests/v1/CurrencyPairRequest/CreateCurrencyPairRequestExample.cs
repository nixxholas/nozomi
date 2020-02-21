using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.AreaModels.v1.RequestProperty;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairRequest
{
    public class CreateCurrencyPairRequestExample : IExamplesProvider<CreateCurrencyPairRequest>
    {
        public CreateCurrencyPairRequest GetExamples()
        {
            return new CreateCurrencyPairRequest()
            {
                CurrencyPairId = 1,
                RequestType = RequestType.HttpPost,
                DataPath = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
                Delay = 3600000,
                RequestComponents = new List<CreateCurrencyPairComponent>()
                {
                    new CreateCurrencyPairComponent()
                    {
                        ComponentType = (long)GenericComponentType.Ask,
                        QueryComponent = "Ask"
                    }
                },
                RequestProperties = new List<CreateRequestProperty>()
                {
                    new CreateRequestProperty()
                    {
                        RequestPropertyType = RequestPropertyType.HttpBody,
                        Key = "",
                        Value = "Test"
                    }
                }
            };
        }
    }
}