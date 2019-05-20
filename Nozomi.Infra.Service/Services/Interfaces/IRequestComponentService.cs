using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Services.Interfaces
{
    public interface IRequestComponentService
    {
        NozomiResult<string> Create(CreateCurrencyPairComponent obj, long userId = 0);
        
        NozomiResult<string> UpdatePairValue(long id, decimal val);
        NozomiResult<string> UpdatePairValue(long id, string val);

        NozomiResult<string> Update(UpdateCurrencyPairComponent obj, long userId = 0);

        NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false);
    }
}
