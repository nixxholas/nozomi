using Swashbuckle.AspNetCore.Filters;

namespace Nozomi.Data.AreaModels.v1.Currency
{
    public class CreateCurrencyExample : IExamplesProvider<CreateCurrency>
    {
        public CreateCurrency GetExamples()
        {
            return new CreateCurrency
            {
                CurrencyTypeId = 1,
                Abbrv = "USD",
                Name = "United States Dollar",
                CurrencySourceId = 1,
                WalletTypeId = 0
            };
        }
    }
}