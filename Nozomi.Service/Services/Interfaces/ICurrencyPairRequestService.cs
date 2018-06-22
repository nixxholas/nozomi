using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairRequestService
    {
        long Create(CurrencyPairRequest cpRequest, long userId = 0);

        CurrencyPairRequest Get(Expression<Func<CurrencyPairRequest, bool>> predicate);

        dynamic GetObsc(Expression<Func<CurrencyPairRequest, bool>> predicate);

        bool Update(CurrencyPairRequest cpRequest, long userId = 0);

        bool SoftDelete(long cpRequestId, long userId = 0);
        
        IEnumerable<CurrencyPairRequest> GetAllActive(bool track = false);
    }
}