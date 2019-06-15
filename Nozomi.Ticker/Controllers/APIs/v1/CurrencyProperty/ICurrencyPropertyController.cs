using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyProperty;

namespace Nozomi.Ticker.Controllers.APIs.v1.CurrencyProperty
{
    public interface ICurrencyPropertyController
    {
        NozomiResult<string> Create(CreateCurrencyProperty currencyProperty);
    }
}