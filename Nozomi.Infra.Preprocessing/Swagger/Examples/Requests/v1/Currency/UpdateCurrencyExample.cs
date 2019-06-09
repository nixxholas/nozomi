using Nozomi.Data.AreaModels.v1.Currency;
using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Preprocessing.Swagger.Examples.Requests.v1.Currency
{
    public class UpdateCurrencyExample : IExamplesProvider<UpdateCurrency>
    {
        public UpdateCurrency GetExamples()
        {
            return new UpdateCurrency()
            {
                CurrencyTypeId = 2, // 1, FIAT; 2, CRYPTO
                Abbreviation = "USDT",
                Name = "Tether USD",
                CurrencySourceId = 1,
                WalletTypeId = 9
            };
        }
    }
}