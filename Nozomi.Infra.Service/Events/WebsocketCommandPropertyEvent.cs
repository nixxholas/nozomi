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

        public bool Exists(long commandId, CommandPropertyType type, string key, string userId = null)
        {
            if (commandId > 0)
            {
                var query = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                    .GetQueryable()
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(p => p.CreatedById.Equals(userId));

                return query.Any(p => p.WebsocketCommandId.Equals(commandId)
                                      && p.CommandPropertyType.Equals(type) && p.Key.Equals(key));
            }
            
            throw new ArgumentOutOfRangeException("Invalid ID!");
        }

        public bool Exists(string commandGuid, CommandPropertyType type, string key, string userId = null)
        {
            if (Guid.TryParse(commandGuid, out var parsedGuid))
            {
                var query = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                    .GetQueryable()
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(p => p.CreatedById.Equals(userId));

                return query
                    .Include(p => p.WebsocketCommand)
                    .Any(p => p.WebsocketCommand.Guid.Equals(parsedGuid)
                              && p.CommandPropertyType.Equals(type) && p.Key.Equals(key));
            }
            
            throw new ArgumentOutOfRangeException("Invalid GUID!");
        }

        public WebsocketCommandProperty Get(long id, bool ensureNotDisabledOrDeleted = true, string userId = null, 
            bool track = false)
        {
            var property = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Where(c => c.Id.Equals(id));

            if (!string.IsNullOrEmpty(userId))
                property = property.Where(c => c.CreatedById.Equals(userId));

            if (ensureNotDisabledOrDeleted)
                property = property.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                property = property.AsTracking();

            return property.SingleOrDefault();
        }

        public WebsocketCommandProperty Get(string guid, bool ensureNotDisabledOrDeleted = true, string userId = null,
            bool track = false)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                return null;
            
            var property = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Where(c => c.Guid.Equals(parsedGuid));

            if (!string.IsNullOrEmpty(userId))
                property = property.Where(c => c.CreatedById.Equals(userId));

            if (ensureNotDisabledOrDeleted)
                property = property.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                property = property.AsTracking();

            return property.SingleOrDefault();
        }

        public WebsocketCommandProperty Get(Guid guid, bool ensureNotDisabledOrDeleted = true, string userId = null,
            bool track = false)
        {
            var query = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable();

            if (track)
                query = query.AsTracking();
            
            if (ensureNotDisabledOrDeleted)
                query = query.Where(c => c.DeletedAt == null && c.IsEnabled);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(c => c.CreatedById.Equals(userId));

            return query.SingleOrDefault(c => c.Guid.Equals(guid));
        }

        public IEnumerable<WebsocketCommandProperty> GetAllByCommand(long commandId, 
            bool ensureNotDisabledOrDeleted = true, string userId = null, bool track = false)
        {
            if (commandId <= 0)
                throw new ArgumentOutOfRangeException($"{_eventName} GetAllByCommand (LONG): Invalid " +
                                                      "commandId!");
            
            var properties = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Where(c => c.WebsocketCommandId.Equals(commandId));

            if (!string.IsNullOrEmpty(userId))
                properties = properties.Where(c => c.CreatedById.Equals(userId));

            if (track)
                properties = properties.AsTracking();

            if (ensureNotDisabledOrDeleted)
                properties = properties.Where(c => c.IsEnabled && c.DeletedAt == null);

            return properties;
        }

        public IEnumerable<WebsocketCommandProperty> GetAllByCommand(string commandGuid, 
            bool ensureNotDisabledOrDeleted = true, string userId = null, bool track = false)
        {
            if (!Guid.TryParse(commandGuid, out var parsedGuid))
                throw new ArgumentOutOfRangeException($"{_eventName} GetAllByRequest (GUID): Invalid " +
                                                      "requestId!");
            
            var properties = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Include(p => p.WebsocketCommand)
                .Where(c => c.WebsocketCommand.Guid.Equals(parsedGuid));

            if (!string.IsNullOrEmpty(userId))
                properties = properties.Where(c => c.CreatedById.Equals(userId));

            if (track)
                properties = properties.AsTracking();

            if (ensureNotDisabledOrDeleted)
                properties = properties.Where(c => c.IsEnabled && c.DeletedAt == null);

            return properties;
        }

        public WebsocketCommandPropertyViewModel View(long id, bool ensureNotDisabledOrDeleted = true, 
            string userId = null, bool track = false)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(null, "Invalid command ID!");
            
            var property = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Where(c => c.Id.Equals(id));

            if (!string.IsNullOrEmpty(userId))
                property = property.Where(c => c.CreatedById.Equals(userId));

            if (ensureNotDisabledOrDeleted)
                property = property.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                property = property.AsTracking();

            return property.Select(c => new WebsocketCommandPropertyViewModel(c.Guid, 
                    c.CommandPropertyType, c.Key, c.Value))
                .SingleOrDefault();
        }

        public WebsocketCommandPropertyViewModel View(string guid, bool ensureNotDisabledOrDeleted = true, 
            string userId = null, bool track = false)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                throw new ArgumentException("Invalid guid!");
            
            var command = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Where(c => c.Guid.Equals(parsedGuid));

            if (!string.IsNullOrEmpty(userId))
                command = command.Where(c => c.CreatedById.Equals(userId));

            if (ensureNotDisabledOrDeleted)
                command = command.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                command = command.AsTracking();

            return command.Select(c => new WebsocketCommandPropertyViewModel(c.Guid, 
                    c.CommandPropertyType, c.Key, c.Value))
                .SingleOrDefault();
        }

        public IEnumerable<WebsocketCommandPropertyViewModel> ViewAllByCommand(long commandId, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            if (commandId <= 0)
                throw new ArgumentException("Invalid request ID!");
            
            var properties = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Where(c => c.WebsocketCommandId.Equals(commandId));

            if (ensureNotDisabledOrDeleted)
                properties = properties.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                properties = properties.AsTracking();
                
            return properties.Select(p => new WebsocketCommandPropertyViewModel(p.Id, 
                p.CommandPropertyType, p.Key, p.Value));
        }

        public IEnumerable<WebsocketCommandPropertyViewModel> ViewAllByCommand(string commandGuid, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            if (!Guid.TryParse(commandGuid, out var parsedGuid))
                throw new ArgumentException("Invalid request GUID!");
            
            var properties = _unitOfWork.GetRepository<WebsocketCommandProperty>()
                .GetQueryable()
                .Include(c => c.WebsocketCommand)
                .Where(c => c.WebsocketCommand.Guid.Equals(parsedGuid));

            if (ensureNotDisabledOrDeleted)
                properties = properties.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                properties = properties.AsTracking();

            return properties.Select(p => new WebsocketCommandPropertyViewModel(p.Id, 
                p.CommandPropertyType, p.Key, p.Value));
        }
    }
}