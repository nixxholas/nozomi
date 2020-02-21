using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.CurrencyPairComponent
{
    public class UpdateCurrencyPairComponentExample : IExamplesProvider<UpdateCurrencyPairComponent>
    {
        public UpdateCurrencyPairComponent GetExamples()
        {
            return new UpdateCurrencyPairComponent()
            {
                ComponentType = ComponentType.High,
                QueryComponent = "High",
                RequestId = 1
            };
        }
    }
}