using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ISourceService
    {
        IEnumerable<dynamic> GetAllNested();

        IEnumerable<Source> GetAllActive(bool includeNested);

        IEnumerable<dynamic> GetAllActiveObsc(bool includeNested);
    }
}
