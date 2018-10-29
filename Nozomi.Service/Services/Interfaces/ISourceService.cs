using System;
using System.Collections.Generic;
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
    }
}
