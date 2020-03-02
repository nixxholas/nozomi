using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;
using Component = Nozomi.Data.Models.Web.Component;

namespace Nozomi.Service.Services.Requests
{
    public class RequestService : BaseService<RequestService, NozomiDbContext>, IRequestService
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        
        public RequestService(ILogger<RequestService> logger, IUnitOfWork<NozomiDbContext> context,
            ICurrencyEvent currencyEvent, ICurrencyPairEvent currencyPairEvent, ICurrencyTypeEvent currencyTypeEvent)
            : base(logger, context)
        {
            _currencyEvent = currencyEvent;
            _currencyPairEvent = currencyPairEvent;
            _currencyTypeEvent = currencyTypeEvent;
        }

        public long Create(Request request, string userId = null)
        {
            try
            {
                _context.GetRepository<Request>().Add(request);
                _context.Commit(userId);

                return request.Id;
            }
            catch (Exception ex)
            {
                return long.MinValue;
            }
        }

        public void Create(CreateRequestViewModel vm, string userId = null)
        {
            if (vm.IsValid())
            {
                var request = new Request(vm.RequestType, vm.ResponseType, vm.DataPath, vm.Delay, vm.FailureDelay);

                switch (vm.ParentType)
                {
                    // Validate it at the db end
                    case CreateRequestViewModel.RequestParentType.Currency:
                        var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                        if (currency == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestViewModel]: " +
                                                           "Currency not found.");

                        request.CurrencyId = currency.Id;
                        break;
                    case CreateRequestViewModel.RequestParentType.CurrencyPair:
                        var currencyPair = _currencyPairEvent.Get(vm.CurrencyPairGuid);
                        
                        if (currencyPair == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestViewModel]: " +
                                                           "Currency pair not found.");
                        
                        request.CurrencyPairId = currencyPair.Id;
                        break;
                    case CreateRequestViewModel.RequestParentType.CurrencyType:
                        if (!Guid.TryParse(vm.CurrencyTypeGuid, out var ctGuid))
                            throw new ArgumentException("Invalid currency type key.");
                        
                        var currencyType = _currencyTypeEvent.Get(ctGuid);
                        
                        if (currencyType == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestViewModel]: " +
                                                           "Currency type not found.");
                        
                        request.CurrencyTypeId = currencyType.Id;
                        break;
                    default:
                        throw new InvalidEnumArgumentException("[RequestService/Create/CreateRequestViewModel]: "
                                                               + "Invalid parent type.");
                }
                
                _context.GetRepository<Request>().Add(request);
                _context.Commit(userId);
            }
            else
            {
                throw new InvalidCastException(
                    "[RequestService/Create/CreateRequestViewModel]: Invalid input given.");   
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

                _context.GetRepository<Request>().Add(request);
                _context.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Request successfully created!", request);
            }

            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Delay(Request request, TimeSpan duration)
        {
            var req = _context.GetRepository<Request>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(r => r.Id.Equals(request.Id)
                                      && r.DeletedAt == null
                                      && r.IsEnabled);

            if (req != null)
            {
                req.ModifiedAt = req.ModifiedAt.Add(duration);
                
                _context.GetRepository<Request>().Update(req);
                _context.Commit();

                return true;
            }

            return false;
        }

        public bool HasUpdated(long requestId)
        {
            if (requestId > 0)
            {
                var req = _context.GetRepository<Request>()
                    .GetQueryable()
                    .AsTracking()
                    .SingleOrDefault(r => r.DeletedAt == null && r.IsEnabled
                                          && r.Id.Equals(requestId));

                if (req != null)
                {
                    req.ModifiedAt = DateTime.UtcNow;

                    _context.Commit();

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

                var reqToUpd = _context.GetRepository<Request>()
                    .GetQueryable()
                    .Include(r => r.RequestComponents)
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

                            _context.GetRepository<Component>().Update(cpc);
                        }
                        // Updating?
                        else
                        {
                            if (ucpc.ComponentType >= 0) cpc.ComponentTypeId = ucpc.ComponentType;
                            if (!string.IsNullOrEmpty(ucpc.QueryComponent)) cpc.QueryComponent = ucpc.QueryComponent;

                            _context.GetRepository<Component>().Update(cpc);
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

                            _context.GetRepository<RequestProperty>().Update(requestProperty);
                        }
                        // Updating?
                        else
                        {
                            if (urp.RequestPropertyType > 0)
                                requestProperty.RequestPropertyType = urp.RequestPropertyType;
                            if (urp.Key != null) requestProperty.Key = urp.Key;
                            if (urp.Value != null) requestProperty.Value = urp.Value;

                            _context.GetRepository<RequestProperty>().Update(requestProperty);
                        }
                    }
                }

                _context.GetRepository<Request>().Update(reqToUpd);
                _context.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Successfully updated the request!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Update(UpdateRequestViewModel vm, string userId = null)
        {
            // Safetynet
            if (vm != null && vm.IsValid())
            {
                var request = _context.GetRepository<Request>()
                    .GetQueryable()
                    .AsTracking()
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

                    switch (vm.ParentType)
                    {
                        case CreateRequestViewModel.RequestParentType.Currency:
                            var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                            if (currency != null)
                                request.CurrencyId = currency.Id;
                            else
                                return false;
                            
                            break;
                        case CreateRequestViewModel.RequestParentType.CurrencyPair:
                            var currencyPair = _currencyPairEvent.Get(vm.CurrencyPairGuid);

                            if (currencyPair != null)
                                request.CurrencyPairId = currencyPair.Id;
                            else
                                return false;

                            break;
                        case CreateRequestViewModel.RequestParentType.CurrencyType:
                            if (!Guid.TryParse(vm.CurrencyTypeGuid, out var ctGuid))
                                throw new ArgumentException("Invalid currency type id.");
                            
                            var currencyType = _currencyTypeEvent.Get(ctGuid);

                            if (currencyType != null)
                                request.CurrencyTypeId = currencyType.Id;
                            else
                                return false;

                            break;
                    }
                    
                    _context.GetRepository<Request>().Update(request);
                    _context.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public void Delete(string requestGuid, bool hardDelete = true, string userId = null)
        {
            if (Guid.TryParse(requestGuid, out var parsedGuid))
            {
                var query = _context.GetRepository<Request>()
                    .GetQueryable()
                    .AsTracking()
                    .Where(r => r.Guid.Equals(parsedGuid));

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(r => r.CreatedById.Equals(userId));

                if (!query.Any())
                    throw new NullReferenceException("No request found to delete.");

                var request = query.SingleOrDefault();
                if (hardDelete)
                    _context.GetRepository<Request>().Delete(request);
                else
                {
                    request.DeletedAt = DateTime.UtcNow;
                    request.DeletedById = userId;
                    _context.GetRepository<Request>().Update(request);
                }

                _context.Commit(userId);
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
                    var reqToDel = _context.GetRepository<Request>()
                        .Get(r => r.Id.Equals(reqId) && r.DeletedAt == null)
                        .SingleOrDefault();

                    if (reqToDel != null)
                    {
                        if (!hardDelete)
                        {
                            reqToDel.DeletedAt = DateTime.UtcNow;
                            reqToDel.DeletedById = userId;
                            _context.GetRepository<Request>().Update(reqToDel);
                        }
                        else
                        {
                            _context.GetRepository<Request>().Delete(reqToDel);
                        }

                        _context.Commit(userId);

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