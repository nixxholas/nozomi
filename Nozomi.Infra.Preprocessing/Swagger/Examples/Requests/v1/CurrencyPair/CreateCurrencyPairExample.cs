using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Categorisation;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPair
{
    public class CreateCurrencyPairExample : IExamplesProvider<CreateCurrencyPair>
    {
        public CreateCurrencyPair GetExamples()
        {
            return new CreateCurrencyPair()
            {
                CurrencyPairType = CurrencyPairType.EXCHANGEABLE,
                ApiUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD",
                DefaultComponent = "0",
                SourceId = 1,
//                PartialCurrencyPairs = new List<CreatePartialCurrencyPair>()
//                {
//                    new CreatePartialCurrencyPair()
//                    {
//                        CurrencyId = 1,
//                        IsMain = true
//                    },
//                    new CreatePartialCurrencyPair()
//                    {
//                        CurrencyId = 3,
//                        IsMain = false
//                    }
//                },
//                CurrencyPairRequests = new List<CreateCurrencyPairRequest>()
//                {
//                    new CreateCurrencyPairRequest()
//                    {
//                        RequestType = RequestType.HttpGet,
//                        DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD",
//                        Delay = 3000,
//                        RequestComponents = new List<CreateCurrencyPairComponent>()
//                        {
//                            new CreateCurrencyPairComponent()
//                            {
//                                ComponentType = ComponentType.Ask,
//                                QueryComponent = "0"
//                            }
//                        },
//                        RequestProperties = new List<CreateRequestProperty>()
//                        {
//                            new CreateRequestProperty()
//                            {
//                                RequestPropertyType = RequestPropertyType.HttpHeader_Custom,
//                                Key = "Header_hello",
//                                Value = "Hello"
//                            }
//                        }
//                    }
//                }
            };
        }
    }
}