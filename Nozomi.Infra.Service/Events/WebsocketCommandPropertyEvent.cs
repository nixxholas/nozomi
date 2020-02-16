using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            if (websocketCommandPropertyId > 0)
            {
                var query = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                    .GetQueryable()
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(p => p.CreatedById.Equals(userId));

                return query.Any(p => p.Id.Equals(websocketCommandPropertyId));
            }
            
            throw new ArgumentOutOfRangeException("Invalid ID!");
        }

        public bool Exists(string websocketCommandPropertyGuid, string userId = null)
        {
            if (Guid.TryParse(websocketCommandPropertyGuid, out var parsedGuid))
            {
                var query = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                    .GetQueryable()
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(p => p.CreatedById.Equals(userId));

                return query.Any(p => p.Guid.Equals(parsedGuid));
            }
            
            throw new ArgumentOutOfRangeException("Invalid GUID!");
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