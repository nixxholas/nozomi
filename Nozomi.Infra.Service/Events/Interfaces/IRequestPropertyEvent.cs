using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestPropertyEvent
    {
        bool Exists(Guid guid, string userId = null);
         
        RequestProperty GetByGuid(string guid, string validatingUserId = null, bool ensureNotDisabledOrDeleted = true, 
            bool track = false);
        
        IEnumerable<RequestPropertyViewModel> GetByRequest(string requestGuid, string validatingUserId = null, 
            bool ensureNotDisabledOrDeleted = true);

        IEnumerable<RequestPropertyViewModel> ViewAll(int index = 0, string userId = null);

        IEnumerable<RequestPropertyViewModel> ViewByRequest(string requestGuid, int index = 0, string userId = null);
    }
}