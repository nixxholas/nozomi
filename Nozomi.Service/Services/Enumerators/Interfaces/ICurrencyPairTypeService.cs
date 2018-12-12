using System.Collections.Generic;

namespace Nozomi.Service.Services.Enumerators.Interfaces
{
    public interface ICurrencyPairTypeService
    {
        // KVP version of the enums
        ICollection<KeyValuePair<string, int>> All();
    }
}