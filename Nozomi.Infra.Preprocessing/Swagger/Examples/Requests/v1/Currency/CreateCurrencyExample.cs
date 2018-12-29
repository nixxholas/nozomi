using Nozomi.Data.AreaModels.v1.Currency;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.Currency
{
    public class CreateCurrencyExample : IExamplesProvider<CreateCurrency>
    {
        public CreateCurrency GetExamples()
        {
            return new CreateCurrency()
            {
                CurrencyTypeId = 1, // 1, FIAT; 2, CRYPTO
                Abbrv = "EUR",
                Name = "Euro",
                CurrencySourceId = 1,
                WalletTypeId = 0
            };
        }
    }
}