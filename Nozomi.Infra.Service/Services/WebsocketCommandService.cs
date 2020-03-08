using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class WebsocketCommandService : BaseService<WebsocketCommandService, NozomiDbContext>, 
        IWebsocketCommandService
    {
        private readonly IRequestEvent _requestEvent;
        private readonly IWebsocketCommandEvent _websocketCommandEvent;
        
        public WebsocketCommandService(ILogger<WebsocketCommandService> logger, NozomiDbContext context,
            IRequestEvent requestEvent, IWebsocketCommandEvent websocketCommandEvent) 
            : base(logger, context)
        {
            _requestEvent = requestEvent;
            _websocketCommandEvent = websocketCommandEvent;
        }

        public WebsocketCommandService(IHttpContextAccessor contextAccessor, ILogger<WebsocketCommandService> logger, 
            NozomiDbContext context, IRequestEvent requestEvent, 
            IWebsocketCommandEvent websocketCommandEvent) 
            : base(contextAccessor, logger, context)
        {
            _requestEvent = requestEvent;
            _websocketCommandEvent = websocketCommandEvent;
        }

        public void Create(CreateWebsocketCommandInputModel vm, string userId = null)
        {
            if (vm.IsValid())
            {
                // Obtain the request and see if it is appropriate
                if (!_requestEvent.Exists(vm.RequestGuid, false, userId) 
                    || !Guid.TryParse(vm.RequestGuid, out var parsedGuid))
                    throw new KeyNotFoundException("Request not found!");
                var request = _requestEvent.GetByGuid(parsedGuid);
                
                // Setup the command first
                var command = new WebsocketCommand
                {
                    CommandType = vm.Type,
                    Name = vm.Name,
                    Delay = vm.Delay,
                    RequestId = request.Id
                };
                
                // Then the properties
                if (vm.Properties != null && vm.Properties.Any())
                {
                    // Populate
                    command.WebsocketCommandProperties = new List<WebsocketCommandProperty>(
                        vm.Properties
                            .Select(p => new WebsocketCommandProperty
                            {
                                CommandPropertyType = p.Type,
                                Key = p.Key,
                                Value = p.Value
                            }));
                }
                
                _context.WebsocketCommands.Add(command); // Add
                _context.SaveChanges(userId); // Save
                return; // Exit
            }
            
            throw new ArgumentException("Invalid payload!");
        }

        public void Update(UpdateWebsocketCommandInputModel vm, string userId = null)
        {
            if (vm.IsValid() && _websocketCommandEvent.Exists(vm.Guid, userId) 
                             && Guid.TryParse(vm.Guid, out var parsedGuid))
            {
                var query = _context.WebsocketCommands.Include(c => c.WebsocketCommandProperties)
                    .AsTracking()
                    .SingleOrDefault(c => c.Guid.Equals(parsedGuid));
                
                if (query == null)
                    throw new KeyNotFoundException("Command not found!");
                
                // Update the command
                if (query.CommandType != vm.Type)
                    query.CommandType = vm.Type;
                if (!string.IsNullOrEmpty(vm.Name))
                    query.Name = vm.Name;
                if (vm.Delay >= -1)
                    query.Delay = vm.Delay;
                if (query.IsEnabled != vm.IsEnabled)
                    query.IsEnabled = vm.IsEnabled;
                
                // Now, the properties
                if (vm.Properties != null && vm.Properties.Any())
                {
                    // Iterate the updated properties collection
                    foreach (var updatedProperty in vm.Properties)
                    {
                        // Look for the property we're gonna update
                        var currentProperty = query.WebsocketCommandProperties
                            .SingleOrDefault(p => 
                                p.Guid.ToString().Equals(updatedProperty.Guid) 
                                || (p.Id > 0 && p.Id.Equals(updatedProperty.Id)));

                        // If there's something, means it exists
                        if (currentProperty != null)
                        {
                            // Update its values
                            if (currentProperty.CommandPropertyType != updatedProperty.Type)
                                currentProperty.CommandPropertyType = updatedProperty.Type;
                            if (!string.IsNullOrEmpty(updatedProperty.Key))
                                currentProperty.Key = updatedProperty.Key;
                            if (!string.IsNullOrEmpty(updatedProperty.Value))
                                currentProperty.Value = updatedProperty.Value;
                            currentProperty.IsEnabled = updatedProperty.IsEnabled;
                        }
                        else // Else we add it since its new
                        {
                            query.WebsocketCommandProperties.Add(new WebsocketCommandProperty
                            {
                                CommandPropertyType = updatedProperty.Type,
                                Key = updatedProperty.Key,
                                Value = updatedProperty.Value
                            });
                        }
                    }
                }
                
                // Then we update
                _context.WebsocketCommands.Update(query); // Save
                _context.SaveChanges(userId); // Commit
            }
            
            throw new ArgumentException("Invalid payload!");
        }

        public void Delete(string commandGuid, string userId, bool hardDelete = true)
        {
            if (Guid.TryParse(commandGuid, out var parsedGuid))
            {
                var command = _context.WebsocketCommands.AsTracking()
                    .SingleOrDefault(c => c.Guid.Equals(parsedGuid));

                if (command != null)
                {
                    _context.WebsocketCommands.Remove(command); // Delete
                    _context.SaveChanges(userId); // Save

                    return;
                }
            }
            
            throw new ArgumentException("Invalid GUID!");
        }

        public void Delete(long commandId, string userId, bool hardDelete = true)
        {
            if (commandId > 0)
            {
                var command = _context.WebsocketCommands.AsTracking()
                    .SingleOrDefault(c => c.Id.Equals(commandId));

                if (command != null)
                {
                    _context.WebsocketCommands.Remove(command); // Delete
                    _context.SaveChanges(userId); // Save

                    return;
                }
            }
            
            throw new ArgumentException("Invalid Command ID!");
        }
    }
}