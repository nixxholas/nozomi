﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;
using Component = Nozomi.Data.Models.Web.Component;

namespace Nozomi.Service.Services.Requests
{
    public class RequestService : BaseService<RequestService, NozomiDbContext>, IRequestService
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        private readonly IRequestPropertyEvent _requestPropertyEvent;
        private readonly IWebsocketCommandEvent _websocketCommandEvent;
        private readonly IWebsocketCommandService _websocketCommandService;
        
        public RequestService(ILogger<RequestService> logger, NozomiDbContext context,
            ICurrencyEvent currencyEvent, ICurrencyPairEvent currencyPairEvent, ICurrencyTypeEvent currencyTypeEvent,
            IRequestPropertyEvent requestPropertyEvent, IWebsocketCommandEvent websocketCommandEvent, 
            IWebsocketCommandService websocketCommandService)
            : base(logger, context)
        {
            _currencyEvent = currencyEvent;
            _currencyPairEvent = currencyPairEvent;
            _currencyTypeEvent = currencyTypeEvent;
            _requestPropertyEvent = requestPropertyEvent;
            _websocketCommandEvent = websocketCommandEvent;
            _websocketCommandService = websocketCommandService;
        }

        public long Create(Request request, string userId = null)
        {
            try
            {
                _context.Requests.Add(request);
                _context.SaveChanges(userId);

                return request.Id;
            }
            catch (Exception ex)
            {
                return long.MinValue;
            }
        }

        public void Create(CreateRequestInputModel vm, string userId = null)
        {
            if (vm.IsValid())
            {
                var request = new Request(vm.RequestType, vm.ResponseType, vm.DataPath, vm.Delay, vm.FailureDelay,
                    vm.RequestProperties, vm.WebsocketCommands, vm.Components);

                switch (vm.ParentType)
                {
                    // Validate it at the db end
                    case CreateRequestInputModel.RequestParentType.Currency:
                        var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                        if (currency == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestInputModel]: " +
                                                           "Currency not found.");

                        request.CurrencyId = currency.Id;
                        break;
                    case CreateRequestInputModel.RequestParentType.CurrencyPair:
                        var currencyPair = _currencyPairEvent.Get(vm.CurrencyPairGuid);
                        
                        if (currencyPair == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestInputModel]: " +
                                                           "Currency pair not found.");
                        
                        request.CurrencyPairId = currencyPair.Id;
                        break;
                    case CreateRequestInputModel.RequestParentType.CurrencyType:
                        if (!Guid.TryParse(vm.CurrencyTypeGuid, out var ctGuid))
                            throw new ArgumentException("Invalid currency type key.");
                        
                        var currencyType = _currencyTypeEvent.Get(ctGuid);
                        
                        if (currencyType == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestInputModel]: " +
                                                           "Currency type not found.");
                        
                        request.CurrencyTypeId = currencyType.Id;
                        break;
                    case CreateRequestInputModel.RequestParentType.None:
                        // No parent type, continue
                        break;
                    default:
                        throw new InvalidEnumArgumentException("[RequestService/Create/CreateRequestInputModel]: "
                                                               + "Invalid parent type.");
                }
                
                _context.Requests.Add(request);
                _context.SaveChanges(userId);
            }
            else
            {
                throw new InvalidCastException(
                    "[RequestService/Create/CreateRequestInputModel]: Invalid input given.");   
            }
        }

        public NozomiResult<string> Create(CreateRequest createRequest, string userId = null)
        {
            try
            {
                if (createRequest == null || !createRequest.IsValid())
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Failed to create request. Please make sure " +
                        "that your request object is proper");

                var request = new Request()
                {
                    CurrencyId = createRequest.CurrencyId,
                    CurrencyPairId = createRequest.CurrencyPairId,
                    CurrencyTypeId = createRequest.CurrencyTypeId,
                    DataPath = createRequest.DataPath,
                    Delay = createRequest.Delay,
                    FailureDelay = createRequest.FailureDelay,
                    RequestType = createRequest.RequestType,
                    ResponseType = createRequest.ResponseType,
                    RequestComponents = createRequest.RequestComponents?.Count > 0 ? 
                        createRequest.RequestComponents
                        .Select(rc => new Component()
                        {
                            ComponentTypeId = rc.ComponentType,
                            QueryComponent = rc.QueryComponent
                        })
                        .ToList() 
                        : new List<Component>(),
                    RequestProperties = createRequest.RequestProperties?.Count > 0 ? 
                        createRequest.RequestProperties
                        .Select(rp => new RequestProperty()
                        {
                            RequestPropertyType = rp.RequestPropertyType,
                            Key = rp.Key,
                            Value = rp.Value
                        })
                        .ToList() 
                        : new List<RequestProperty>()
                };

                _context.Requests.Add(request);
                _context.SaveChanges(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Request successfully created!", request);
            }

            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Delay(Request request, TimeSpan duration)
        {
            var req = _context.Requests.AsTracking()
                .SingleOrDefault(r => r.Id.Equals(request.Id)
                                      && r.DeletedAt == null
                                      && r.IsEnabled);

            if (req != null)
            {
                req.ModifiedAt = req.ModifiedAt.Add(duration);
                
                _context.Requests.Update(req);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public void DelayFailure(Guid guid)
        {
            var request = _context.Requests.AsTracking().SingleOrDefault(r => r.Guid.Equals(guid));

            if (request != null)
            {
                request.FailureCount += 1; // Bump failure
                request.ModifiedAt = DateTime.UtcNow
                    .AddMilliseconds(request.FailureDelay * request.FailureCount); // Bump the delay
                _context.Requests.Update(request);
                _context.SaveChanges();

                _logger.LogInformation($"{_serviceName} DelayFailure (Guid): Delay due to failure " +
                                       $"successfully pushed for request {guid} by " +
                                       $"{request.FailureDelay * request.FailureCount / 1000}s.");
                return;
            }
            
            _logger.LogCritical($"{_serviceName} DelayFailure (Guid): Unable to delay failure for request " +
                                $"{guid}");
        }

        public bool HasUpdated(long requestId)
        {
            if (requestId > 0)
            {
                var req = _context.Requests.AsTracking()
                    .SingleOrDefault(r => r.DeletedAt == null && r.IsEnabled
                                          && r.Id.Equals(requestId));

                if (req != null)
                {
                    req.FailureCount = 0; // RESET!!
                    req.ModifiedAt = DateTime.UtcNow;

                    _context.SaveChanges();

                    return true;
                }
            }

            _logger.LogCritical($"[{_serviceName}] HasUpdated: Incorrect Request ID.");
            return false;
        }

        public bool HasUpdated(ICollection<Request> requests)
        {
            if (requests != null && requests.Any())
            {
                foreach (var req in requests)
                {
                    HasUpdated(req.Id);
                }

                return true;
            }

            _logger.LogCritical($"[{_serviceName}] HasUpdated: Incorrect Request collection.");
            return false;
        }

        public NozomiResult<string> Update(UpdateRequest updateRequest, string userId = null)
        {
            try
            {
                if (updateRequest == null || !updateRequest.IsValid())
                    return new NozomiResult<string>(NozomiResultType.Failed, "Failed to update request");

                var reqToUpd = _context.Requests.Include(r => r.RequestComponents)
                    .Include(r => r.RequestProperties)
                    .SingleOrDefault(r => r.Id.Equals(updateRequest.Id) && r.DeletedAt == null);

                if (reqToUpd == null)
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Failed to update request. Unable to find the request");

                reqToUpd.DataPath = updateRequest.DataPath;
                reqToUpd.Delay = updateRequest.Delay;
                reqToUpd.RequestType = updateRequest.RequestType;
                reqToUpd.ResponseType = updateRequest.ResponseType;
                reqToUpd.IsEnabled = updateRequest.IsEnabled;

                // Include RequestComponents if there are any modified objects
                if (updateRequest.RequestComponents != null && updateRequest.RequestComponents.Count > 0)
                {
                    foreach (var ucpc in updateRequest.RequestComponents)
                    {
                        var cpc = reqToUpd.RequestComponents.SingleOrDefault(rc => rc.Id.Equals(ucpc.Id));

                        if (cpc == null)
                            return new NozomiResult<string>(NozomiResultType.Failed, "Failed to update request");

                        // Deleting?
                        if (ucpc.ToBeDeleted())
                        {
                            cpc.DeletedAt = DateTime.UtcNow;
                            if (!string.IsNullOrWhiteSpace(userId))
                                cpc.DeletedById = userId;

                            _context.Components.Update(cpc);
                        }
                        // Updating?
                        else
                        {
                            if (ucpc.ComponentType >= 0) cpc.ComponentTypeId = ucpc.ComponentType;
                            if (!string.IsNullOrEmpty(ucpc.QueryComponent)) cpc.QueryComponent = ucpc.QueryComponent;

                            _context.Components.Update(cpc);
                        }
                    }
                }

                // Include RequestProperties if there are any modified objects
                if (updateRequest.RequestProperties != null && updateRequest.RequestProperties.Count > 0)
                {
                    foreach (var urp in updateRequest.RequestProperties)
                    {
                        var requestProperty = reqToUpd.RequestProperties.SingleOrDefault(rc => rc.Id.Equals(urp.Id));

                        if (requestProperty == null)
                            return new NozomiResult<string>(NozomiResultType.Failed, "Failed to update request");

                        // Deleting?
                        if (urp.ToBeDeleted())
                        {
                            requestProperty.DeletedAt = DateTime.UtcNow;
                            if (!string.IsNullOrWhiteSpace(userId))
                                requestProperty.DeletedById = userId;

                            _context.RequestProperties.Update(requestProperty);
                        }
                        // Updating?
                        else
                        {
                            if (urp.RequestPropertyType > 0)
                                requestProperty.RequestPropertyType = urp.RequestPropertyType;
                            if (urp.Key != null) requestProperty.Key = urp.Key;
                            if (urp.Value != null) requestProperty.Value = urp.Value;

                            _context.RequestProperties.Update(requestProperty);
                        }
                    }
                }

                _context.Requests.Update(reqToUpd);
                _context.SaveChanges(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Successfully updated the request!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Update(UpdateRequestInputModel vm, string userId = null)
        {
            // Safetynet
            if (vm != null && vm.IsValid())
            {
                var request = _context.Requests.AsTracking()
                    .SingleOrDefault(r => r.DeletedAt == null && r.Guid.Equals(vm.Guid));

                if (request != null)
                {
                    if (vm.IsEnabled != null)
                        request.IsEnabled = (bool)vm.IsEnabled;
                    request.RequestType = vm.RequestType;
                    request.ResponseType = vm.ResponseType;
                    request.DataPath = vm.DataPath;
                    request.Delay = vm.Delay;
                    request.FailureDelay = vm.FailureDelay;
                    request.RequestComponents = vm.Components
                        .Select(c => new Component()).ToList();
                    request.RequestProperties = vm.Properties
                        .Select(p => new RequestProperty()).ToList();
                    if (vm.WebsocketCommands != null && vm.WebsocketCommands.Any()) { // Safetynet
                        // Update the commands if any
                        foreach (var updWsc in vm.WebsocketCommands)
                        {
                            if (_websocketCommandEvent.Exists(updWsc.Guid, userId)) // Ensure it exists first
                            {
                                _websocketCommandService.Update(updWsc, userId);
                            }
                        }
                    }

                    switch (vm.ParentType)
                    {
                        case CreateRequestInputModel.RequestParentType.Currency:
                            var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                            if (currency != null)
                                request.CurrencyId = currency.Id;
                            else
                                return false;
                            
                            break;
                        case CreateRequestInputModel.RequestParentType.CurrencyPair:
                            var currencyPair = _currencyPairEvent.Get(vm.CurrencyPairGuid);

                            if (currencyPair != null)
                                request.CurrencyPairId = currencyPair.Id;
                            else
                                return false;

                            break;
                        case CreateRequestInputModel.RequestParentType.CurrencyType:
                            if (!Guid.TryParse(vm.CurrencyTypeGuid, out var ctGuid))
                                throw new ArgumentException("Invalid currency type id.");
                            
                            var currencyType = _currencyTypeEvent.Get(ctGuid);

                            if (currencyType != null)
                                request.CurrencyTypeId = currencyType.Id;
                            else
                                return false;

                            break;
                        case CreateRequestInputModel.RequestParentType.None:
                            // No parent type, continue
                            break;
                        default:
                            throw new InvalidEnumArgumentException("[RequestService/Update/UpdateRequestInputModel]: "
                                                                   + "Invalid parent type.");
                    }
                    
                    _context.Requests.Update(request);
                    _context.SaveChanges(userId);

                    return true;
                }
            }

            return false;
        }

        public void Delete(string requestGuid, bool hardDelete = true, string userId = null)
        {
            if (Guid.TryParse(requestGuid, out var parsedGuid))
            {
                var query = _context.Requests.AsTracking()
                    .Where(r => r.Guid.Equals(parsedGuid));

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(r => r.CreatedById.Equals(userId));

                var request = query.SingleOrDefault();
                
                if (request == null)
                    throw new NullReferenceException("No request found to delete.");
                
                if (hardDelete)
                    _context.Requests.Remove(request);
                else
                {
                    request.DeletedAt = DateTime.UtcNow;
                    request.DeletedById = userId;
                    _context.Requests.Update(request);
                }

                _context.SaveChanges(userId);
                return;
            }

            throw new ArgumentNullException("Invalid GUID!");
        }

        public NozomiResult<string> Delete(long reqId, bool hardDelete = false, string userId = null)
        {
            try
            {
                if (reqId > 0 && !string.IsNullOrWhiteSpace(userId))
                {
                    var reqToDel = _context.Requests
                        .SingleOrDefault(r => r.Id.Equals(reqId) && r.DeletedAt == null);

                    if (reqToDel != null)
                    {
                        if (!hardDelete)
                        {
                            reqToDel.DeletedAt = DateTime.UtcNow;
                            reqToDel.DeletedById = userId;
                            _context.Requests.Update(reqToDel);
                        }
                        else
                        {
                            _context.Requests.Remove(reqToDel);
                        }

                        _context.SaveChanges(userId);

                        return new NozomiResult<string>(NozomiResultType.Success, "Request successfully deleted!");
                    }
                }

                return new NozomiResult<string>(NozomiResultType.Failed, "Invalid request ID.");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool ManualPoll(long id, string userId = null)
        {
            return false;
        }
    }
}