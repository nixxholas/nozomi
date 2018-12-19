using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairComponentService
    {
        ICollection<RequestComponent> AllByRequestId(long requestId, bool includeNested = false);

        ICollection<RequestComponent> All(int index = 0, bool includeNested = false);

        NozomiResult<RequestComponent> Get(long id, bool includeNested = false);

        NozomiResult<string> Create(CreateCurrencyPairComponent obj, long userId = 0);
        
        NozomiResult<string> UpdatePairValue(long id, decimal val);

        NozomiResult<string> Update(UpdateCurrencyPairComponent obj, long userId = 0);

        NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false);
    }
}
