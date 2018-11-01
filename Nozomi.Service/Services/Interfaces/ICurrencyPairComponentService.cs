using System;
using System.Collections.Generic;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairComponentService
    {
        ICollection<RequestComponent> AllByRequestId(long requestId, bool includeNested = false);

        ICollection<RequestComponent> All(bool includeNested = false);
        
        bool Create(CreateCurrencyPairComponent obj, long userId = 0);
        
        bool UpdatePairValue(long id, decimal val);

        bool Update(UpdateCurrencyPairComponent obj, long userId = 0);

        bool Delete(long id, long userId = 0, bool hardDelete = false);
    }
}
