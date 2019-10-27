using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyService
    {
        NozomiResult<string> Create(CreateCurrency currency, string userId = null);
        NozomiResult<string> Update(UpdateCurrency currency, string userId = null);
        NozomiResult<string> Delete(long currencyId, bool hardDelete = false, string userId = null);
        
    }
}
