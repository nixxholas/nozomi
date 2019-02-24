using Nozomi.Data.AreaModels.v1.PartialCurrencyPair;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.PartialCurrencyPair
{
    public class CreatePartialCurrencyPairExample : IExamplesProvider<CreatePartialCurrencyPair>
    {
        public CreatePartialCurrencyPair GetExamples()
        {
            return new CreatePartialCurrencyPair()
            {
                CurrencyId = 1,
                IsMain = true
            };
        }
    }
}