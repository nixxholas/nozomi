using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent
{
    public class CreateCurrencyPairComponentExample : IExamplesProvider<CreateCurrencyPairComponent>
    {
        public CreateCurrencyPairComponent GetExamples()
        {
            return new CreateCurrencyPairComponent()
            {
                ComponentType = ComponentType.High,
                QueryComponent = "High",
                RequestId = 1
            };
        }
    }
}