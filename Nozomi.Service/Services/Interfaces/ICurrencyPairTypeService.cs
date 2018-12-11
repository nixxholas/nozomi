using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairTypeService
    {
        // KVP version of the enums
        ICollection<KeyValuePair<string, int>> All();
    }
}