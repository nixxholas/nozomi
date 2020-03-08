using System;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IComponentHistoricItemEvent
    {
        ComponentHistoricItem GetLastItem(long id, bool includeNested = false);
        
        ComponentHistoricItem GetLastItem(string guid);
        
        ComponentHistoricItem GetLastItem(Guid guid);
    }
}