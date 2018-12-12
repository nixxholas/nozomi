using System.Collections.Generic;

namespace Nozomi.Service.Services.Enumerators.Interfaces
{
    public interface IRequestPropertyTypeService
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}