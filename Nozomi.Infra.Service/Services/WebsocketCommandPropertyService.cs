using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.WebsocketCommandProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class WebsocketCommandPropertyService : BaseService<WebsocketCommandPropertyService, NozomiDbContext>,
        IWebsocketCommandPropertyService
    {
        private readonly IWebsocketCommandEvent _websocketCommandEvent;
        private readonly IWebsocketCommandPropertyEvent _websocketCommandPropertyEvent;

        public WebsocketCommandPropertyService(ILogger<WebsocketCommandPropertyService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, IWebsocketCommandEvent websocketCommandEvent, 
            IWebsocketCommandPropertyEvent websocketCommandPropertyEvent) 
            : base(logger, unitOfWork)
        {
            _websocketCommandEvent = websocketCommandEvent;
            _websocketCommandPropertyEvent = websocketCommandPropertyEvent;
        }

        public WebsocketCommandPropertyService(IHttpContextAccessor contextAccessor, 
            ILogger<WebsocketCommandPropertyService> logger, IUnitOfWork<NozomiDbContext> unitOfWork, 
            IWebsocketCommandEvent websocketCommandEvent, IWebsocketCommandPropertyEvent websocketCommandPropertyEvent) 
            : base(contextAccessor, logger, unitOfWork)
        {
            _websocketCommandEvent = websocketCommandEvent;
            _websocketCommandPropertyEvent = websocketCommandPropertyEvent;
        }

        public void Create(CreateWebsocketCommandPropertyInputModel vm, string userId)
        {
            // If it is a solo create
            if (vm.IsValidDependant())
            {
                var property = new WebsocketCommandProperty
                {
                    WebsocketCommandId = vm.CommandId,
                    CommandPropertyType = vm.Type,
                    Key = vm.Key,
                    Value = vm.Value
                };
                
                // Populate the property through the GUID method
                if (Guid.TryParse(vm.CommandGuid, out var parsedGuid))
                    property.WebsocketCommandId = _websocketCommandEvent.Get(parsedGuid).Id;
                
                _unitOfWork.GetRepository<WebsocketCommandProperty>().Add(property); // Add
                _unitOfWork.Commit(userId); // Commit
                return; // Exit
            }
            
            _logger.LogInformation($"{_serviceName} Create: Invalid view model.");
            throw new InvalidOperationException("Invalid payload!");
        }

        public void Update(UpdateWebsocketCommandPropertyInputModel vm, string userId)
        {
            if (vm.IsValid())
            {
                // Since its a GUID
                if (Guid.TryParse(vm.Guid, out var parsedGuid) 
                    && _websocketCommandPropertyEvent.Exists(parsedGuid))
                {
                    var property = _websocketCommandPropertyEvent.Get(parsedGuid, false, 
                        userId); // Obtain the property

                    if (property != null) // Update it if not null
                    {
                        if (!string.IsNullOrEmpty(vm.Key))
                            property.Key = vm.Key;
                        if (!string.IsNullOrEmpty(vm.Value))
                            property.Value = vm.Value;
                        if (!vm.Type.Equals(property.CommandPropertyType))
                            property.CommandPropertyType = vm.Type;
                        
                        _unitOfWork.GetRepository<WebsocketCommandProperty>().Update(property); // Update
                        _unitOfWork.Commit(userId); // Save
                        return;
                    }
                }
                else if (vm.Id > 0 && _websocketCommandEvent.Exists(vm.Id)) // Since its an ID
                {
                    var property = _websocketCommandPropertyEvent.Get(vm.Id, false, 
                        userId); // Obtain the property

                    if (property != null) // Update it if not null
                    {
                        if (!string.IsNullOrEmpty(vm.Key))
                            property.Key = vm.Key;
                        if (!string.IsNullOrEmpty(vm.Value))
                            property.Value = vm.Value;
                        if (!vm.Type.Equals(property.CommandPropertyType))
                            property.CommandPropertyType = vm.Type;
                        
                        _unitOfWork.GetRepository<WebsocketCommandProperty>().Update(property); // Update
                        _unitOfWork.Commit(userId); // Save
                        return;
                    }
                }
            }
            
            _logger.LogInformation($"{_serviceName} Update: Invalid view model.");
            throw new InvalidOperationException("Invalid payload!");
        }

        public void Delete(string propertyGuid, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long propertyId, string userId, bool hardDelete = true)
        {
            throw new System.NotImplementedException();
        }
    }
}