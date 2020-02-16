using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class WebsocketCommandPropertyEvent : BaseEvent<WebsocketCommandPropertyEvent, NozomiDbContext>,
        IWebsocketCommandPropertyEvent
    {
        public WebsocketCommandPropertyEvent(ILogger<WebsocketCommandPropertyEvent> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public bool Exists(long websocketCommandPropertyId, string userId = null)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string websocketCommandPropertyGuid, string userId = null)
        {
            throw new System.NotImplementedException();
        }

        public WebsocketCommandProperty Get(long id, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public WebsocketCommandProperty Get(string guid, bool ensureNotDisabledOrDeleted = true, string userId = null,
            bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WebsocketCommandProperty> GetAllByCommand(long commandId, 
            bool ensureNotDisabledOrDeleted = true, string userId = null, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WebsocketCommandProperty> GetAllByCommand(string commandGuid, 
            bool ensureNotDisabledOrDeleted = true, string userId = null, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public WebsocketCommandPropertyViewModel View(long id, bool ensureNotDisabledOrDeleted = true, 
            string userId = null, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public WebsocketCommandPropertyViewModel View(string guid, bool ensureNotDisabledOrDeleted = true, 
            string userId = null, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WebsocketCommandPropertyViewModel> ViewAllByCommand(long commandId, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WebsocketCommandPropertyViewModel> ViewAllByCommand(string commandGuid, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            throw new System.NotImplementedException();
        }
    }
}