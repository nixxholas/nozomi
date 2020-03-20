using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.ComponentHistoricItem;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IComponentHistoricItemEvent
    {
        ComponentHistoricItem GetLastItem(long id, bool includeNested = false);
        
        ComponentHistoricItem GetLastItem(string guid);
        
        ComponentHistoricItem GetLastItem(Guid guid);

        IEnumerable<ComponentHistoricItemViewModel> ViewAll(int index = 0, string componentGuid = null);
    }
}