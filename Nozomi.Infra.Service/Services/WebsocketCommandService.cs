using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommand;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class WebsocketCommandService : BaseService<WebsocketCommandService, NozomiDbContext>, 
        IWebsocketCommandService
    {
        private readonly IRequestEvent _requestEvent;
        
        public WebsocketCommandService(ILogger<WebsocketCommandService> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            IRequestEvent requestEvent) 
            : base(logger, unitOfWork)
        {
            _requestEvent = requestEvent;
        }

        public WebsocketCommandService(IHttpContextAccessor contextAccessor, ILogger<WebsocketCommandService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, IRequestEvent requestEvent) 
            : base(contextAccessor, logger, unitOfWork)
        {
            _requestEvent = requestEvent;
        }

        public void Create(CreateWebsocketCommandInputModel vm, string userId)
        {
            if (vm.IsValid() && !string.IsNullOrEmpty(userId))
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
                
                _unitOfWork.GetRepository<WebsocketCommand>().Add(command); // Add
                _unitOfWork.Commit(userId); // Save
                return; // Exit
            }
            
            throw new ArgumentException("Invalid payload!");
        }

        public void Update(UpdateWebsocketCommandInputModel vm, string userId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string commandGuid, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long commandId, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }
    }
}