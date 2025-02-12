using System;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IWebsocketCommandPropertyEvent
    {
        bool Exists(long websocketCommandPropertyId, string userId = null);
        
        bool Exists(string websocketCommandPropertyGuid, string userId = null);

        bool Exists(Guid propertyGuid, string userId = null);
        
        bool Exists(long commandId, CommandPropertyType type, string key, string userId = null);
        
        bool Exists(string commandGuid, CommandPropertyType type, string key, string userId = null);

        WebsocketCommandProperty Get(long id, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        WebsocketCommandProperty Get(string guid, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        WebsocketCommandProperty Get(Guid guid, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        IEnumerable<WebsocketCommandProperty> GetAllByCommand(long commandId, bool ensureNotDisabledOrDeleted = true,
            string userId = null, bool track = false);
        
        IEnumerable<WebsocketCommandProperty> GetAllByCommand(string commandGuid, bool ensureNotDisabledOrDeleted = true,
            string userId = null, bool track = false);
        
        WebsocketCommandPropertyViewModel View(long id, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        WebsocketCommandPropertyViewModel View(string guid, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false);

        IEnumerable<WebsocketCommandPropertyViewModel> ViewAll(int index = 0, string commandGuid = null, 
            string userId = null);

        IEnumerable<WebsocketCommandPropertyViewModel> ViewAllByCommand(long commandId, 
            bool ensureNotDisabledOrDeleted = true, bool track = false);
        
        IEnumerable<WebsocketCommandPropertyViewModel> ViewAllByCommand(string commandGuid, 
            bool ensureNotDisabledOrDeleted = true, bool track = false);
    }
}