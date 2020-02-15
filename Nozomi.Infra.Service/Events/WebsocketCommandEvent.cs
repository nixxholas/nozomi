using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class WebsocketCommandEvent : BaseEvent<WebsocketCommandEvent, NozomiDbContext>, IWebsocketCommandEvent
    {
        public WebsocketCommandEvent(ILogger<WebsocketCommandEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public bool Exists(long requestId, CommandType type, string name)
        {
            if (requestId > 0 && !string.IsNullOrEmpty(name))
            {
                return _unitOfWork.GetRepository<WebsocketCommand>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(c => c.RequestId.Equals(requestId) && c.CommandType.Equals(type) 
                                                            && c.Name.Equals(name));
            }

            return false;
        }

        public bool Exists(string requestGuid, CommandType type, string name)
        {
            if (Guid.TryParse(requestGuid, out var guid) && !string.IsNullOrEmpty(name))
            {
                return _unitOfWork.GetRepository<WebsocketCommand>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Include(c => c.Request)
                    .Any(c => c.Request.Guid.Equals(guid) && c.CommandType.Equals(type) 
                                                            && c.Name.Equals(name));
            }

            return false;
        }

        public WebsocketCommand Get(long id, bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            var command = _unitOfWork.GetRepository<WebsocketCommand>()
                .GetQueryable()
                .Where(c => c.Id.Equals(id));

            if (ensureNotDisabledOrDeleted)
                command = command.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                command = command.AsTracking();

            return command.SingleOrDefault();
        }

        public WebsocketCommand Get(string guid, bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                return null;
            
            var command = _unitOfWork.GetRepository<WebsocketCommand>()
                .GetQueryable()
                .Include(c => c.Request)
                .Where(c => c.Guid.Equals(parsedGuid));

            if (ensureNotDisabledOrDeleted)
                command = command.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                command = command.AsTracking();

            return command.SingleOrDefault();
        }

        public IEnumerable<WebsocketCommand> GetAllByRequest(long requestId, bool ensureNotDisabledOrDeleted = true, 
            bool track = false)
        {
            if (requestId <= 0)
                throw new ArgumentOutOfRangeException($"{_eventName} GetAllByRequest (LONG): Invalid " +
                                                      "requestId!");
            
            var commands = _unitOfWork.GetRepository<WebsocketCommand>()
                .GetQueryable()
                .Where(c => c.RequestId.Equals(requestId));

            if (track)
                commands = commands.AsTracking();

            if (ensureNotDisabledOrDeleted)
                commands = commands.Where(c => c.IsEnabled && c.DeletedAt == null);

            return commands;
        }

        public IEnumerable<WebsocketCommand> GetAllByRequest(string requestGuid, bool ensureNotDisabledOrDeleted = true, 
            bool track = false)
        {
            if (!Guid.TryParse(requestGuid, out var parsedGuid))
                throw new ArgumentOutOfRangeException($"{_eventName} GetAllByRequest (GUID): Invalid " +
                                                      "requestId!");
            
            var commands = _unitOfWork.GetRepository<WebsocketCommand>()
                .GetQueryable()
                .Include(c => c.Request)
                .Where(c => c.Guid.Equals(parsedGuid));

            if (track)
                commands = commands.AsTracking();

            if (ensureNotDisabledOrDeleted)
                commands = commands.Where(c => c.IsEnabled && c.DeletedAt == null);

            return commands;
        }

        public WebsocketCommandViewModel View(long id, bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(null, "Invalid command ID!");
            
            var command = _unitOfWork.GetRepository<WebsocketCommand>()
                .GetQueryable()
                .Include(c => c.WebsocketCommandProperties)
                .Where(c => c.Id.Equals(id));

            if (ensureNotDisabledOrDeleted)
                command = command.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                command = command.AsTracking();

            return command.Select(c => new WebsocketCommandViewModel(c.Id, 
                c.CommandType, c.Name, c.Delay, c.WebsocketCommandProperties
                    .Select(p => 
                        new WebsocketCommandPropertyViewModel(p.Guid, p.CommandPropertyType, p.Key, p.Value))
                    .ToList()))
                .SingleOrDefault();
        }

        public WebsocketCommandViewModel View(string guid, bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                throw new ArgumentException("Invalid guid!");
            
            var command = _unitOfWork.GetRepository<WebsocketCommand>()
                .GetQueryable()
                .Include(c => c.WebsocketCommandProperties)
                .Where(c => c.Guid.Equals(guid));

            if (ensureNotDisabledOrDeleted)
                command = command.Where(c => c.IsEnabled && c.DeletedAt == null);

            if (track)
                command = command.AsTracking();

            return command.Select(c => new WebsocketCommandViewModel(c.Id, 
                    c.CommandType, c.Name, c.Delay, c.WebsocketCommandProperties
                        .Select(p => 
                            new WebsocketCommandPropertyViewModel(p.Guid, p.CommandPropertyType, p.Key, p.Value))
                        .ToList()))
                .SingleOrDefault();
        }

        public IEnumerable<WebsocketCommandViewModel> ViewAllByRequest(long requestId, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WebsocketCommandViewModel> ViewAllByRequest(string requestGuid, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            throw new System.NotImplementedException();
        }
    }
}