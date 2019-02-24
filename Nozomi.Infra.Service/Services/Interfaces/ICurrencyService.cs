using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyService
    {
        NozomiResult<string> Create(CreateCurrency currency, long userId = 0);
        NozomiResult<string> Update(UpdateCurrency currency, long userId = 0);
        NozomiResult<string> Delete(long currencyId, bool hardDelete = false, long userId = 0);
    }
}
