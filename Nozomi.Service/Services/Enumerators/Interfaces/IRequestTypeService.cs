using System.Collections.Generic;

namespace Nozomi.Service.Services.Enumerators.Interfaces
{
    public interface IRequestTypeService
    {
        ICollection<KeyValuePair<string, int>> All();
    }
}