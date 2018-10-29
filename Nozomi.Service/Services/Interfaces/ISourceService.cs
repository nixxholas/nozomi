using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ISourceService
    {
        bool Create(CreateSource createSource);
        
        IEnumerable<dynamic> GetAllNested();

        IEnumerable<Source> GetAllActive(bool includeNested);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested);

        bool Update(UpdateSource updateSource);

        bool Delete(Expression<Func<Source, bool>> predicate, bool hardDelete = false, long userId = 0);
    }
}
