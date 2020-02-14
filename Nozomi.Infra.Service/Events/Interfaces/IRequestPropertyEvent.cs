using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestPropertyEvent
    {
        RequestProperty GetByGuid(string guid, string validatingUserId = null, bool ensureDisabledOrDeleted = true, 
            bool track = false);
        
        IEnumerable<RequestPropertyViewModel> GetByRequest(string requestGuid, string validatingUserId = null, 
            bool ensureDisabledOrDeleted = true);
    }
}