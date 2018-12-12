using System.Collections.Generic;

namespace Nozomi.Service.Services.Enumerators.Interfaces
{
    public interface IComponentTypeService
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}