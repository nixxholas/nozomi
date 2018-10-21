using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.RequestModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ISourceService
    {
        bool Create(CreateSource createSource);
        
        IEnumerable<dynamic> GetAllNested();

        IEnumerable<Source> GetAllActive(bool includeNested);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested);
    }
}
