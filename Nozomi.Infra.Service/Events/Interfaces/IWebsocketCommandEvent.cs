using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommand;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IWebsocketCommandEvent
    {
        bool Exists(long websocketCommandId, string userId = null);
        
        bool Exists(string websocketCommandGuid, string userId = null);
        
        bool Exists(long requestId, CommandType type, string name);

        bool Exists(string requestGuid, CommandType type, string name);

        WebsocketCommand Get(long id, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        WebsocketCommand Get(string guid, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        WebsocketCommand Get(Guid guid, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        IEnumerable<WebsocketCommand> GetAllByRequest(long requestId, bool ensureNotDisabledOrDeleted = true,
            string userId = null, bool track = false);
        
        IEnumerable<WebsocketCommand> GetAllByRequest(string requestGuid, bool ensureNotDisabledOrDeleted = true,
            string userId = null, bool track = false);
        
        WebsocketCommandViewModel View(long id, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        WebsocketCommandViewModel View(string guid, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        IEnumerable<WebsocketCommandViewModel> ViewAllByRequest(long requestId, 
            bool ensureNotDisabledOrDeleted = true, bool track = false);
        
        IEnumerable<WebsocketCommandViewModel> ViewAllByRequest(string requestGuid, 
            bool ensureNotDisabledOrDeleted = true, bool track = false);
    }
}