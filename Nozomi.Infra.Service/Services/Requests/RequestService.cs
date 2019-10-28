using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.Services.Requests
{
    public class RequestService : BaseService<RequestService, NozomiDbContext>, IRequestService
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencyPairEvent _currencyPairEvent;
        private readonly ICurrencyTypeEvent _currencyTypeEvent;
        
        public RequestService(ILogger<RequestService> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ICurrencyEvent currencyEvent, ICurrencyPairEvent currencyPairEvent, ICurrencyTypeEvent currencyTypeEvent)
            : base(logger, unitOfWork)
        {
            _currencyEvent = currencyEvent;
            _currencyPairEvent = currencyPairEvent;
            _currencyTypeEvent = currencyTypeEvent;
        }

        public long Create(Request request, string userId = null)
        {
            try
            {
                _unitOfWork.GetRepository<Request>().Add(request);
                _unitOfWork.Commit(userId);

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
                        var currencyPair = _currencyPairEvent.Get(vm.CurrencyPair.Id);
                        
                        if (currencyPair == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestViewModel]: " +
                                                           "Currency pair not found.");
                        
                        request.CurrencyPairId = vm.CurrencyPair.Id;
                        break;
                    case CreateRequestViewModel.RequestParentType.CurrencyType:
                        var currencyType = _currencyTypeEvent.Get(vm.CurrencyTypeId);
                        
                        if (currencyType == null)
                            throw new KeyNotFoundException("[RequestService/Create/CreateRequestViewModel]: " +
                                                           "Currency type not found.");
                        
                        request.CurrencyTypeId = vm.CurrencyTypeId;
                        break;
                    default:
                        throw new InvalidEnumArgumentException("[RequestService/Create/CreateRequestViewModel]: "
                                                               + "Invalid parent type.");
                }
                
                _unitOfWork.GetRepository<Request>().Add(request);
                _unitOfWork.Commit(userId);
            }

            throw new InvalidCastException(
                "[RequestService/Create/CreateRequestViewModel]: Invalid input given.");
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
                        .Select(rc => new RequestComponent()
                        {
                            ComponentType = rc.ComponentType,
                            QueryComponent = rc.QueryComponent
                        })
                        .ToList() 
                        : new List<RequestComponent>(),
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

                _unitOfWork.GetRepository<Request>().Add(request);
                _unitOfWork.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Request successfully created!", request);
            }

            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Delay(Request request, TimeSpan duration)
        {
            var req = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(r => r.Id.Equals(request.Id)
                                      && r.DeletedAt == null
                                      && r.IsEnabled);

            if (req != null)
            {
                req.ModifiedAt = req.ModifiedAt.Add(duration);
                
                _unitOfWork.GetRepository<Request>().Update(req);
                _unitOfWork.Commit();

                return true;
            }

            return false;
        }

        public bool HasUpdated(long requestId)
        {
            if (requestId > 0)
            {
                var req = _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsTracking()
                    .SingleOrDefault(r => r.DeletedAt == null && r.IsEnabled
                                          && r.Id.Equals(requestId));

                if (req != null)
                {
                    req.ModifiedAt = DateTime.UtcNow;

                    _unitOfWork.Commit();

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

                var reqToUpd = _unitOfWork.GetRepository<Request>()
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

                            _unitOfWork.GetRepository<RequestComponent>().Update(cpc);
                        }
                        // Updating?
                        else
                        {
                            if (ucpc.ComponentType >= 0) cpc.ComponentType = ucpc.ComponentType;
                            if (!string.IsNullOrEmpty(ucpc.QueryComponent)) cpc.QueryComponent = ucpc.QueryComponent;

                            _unitOfWork.GetRepository<RequestComponent>().Update(cpc);
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

                            _unitOfWork.GetRepository<RequestProperty>().Update(requestProperty);
                        }
                        // Updating?
                        else
                        {
                            if (urp.RequestPropertyType > 0)
                                requestProperty.RequestPropertyType = urp.RequestPropertyType;
                            if (urp.Key != null) requestProperty.Key = urp.Key;
                            if (urp.Value != null) requestProperty.Value = urp.Value;

                            _unitOfWork.GetRepository<RequestProperty>().Update(requestProperty);
                        }
                    }
                }

                _unitOfWork.GetRepository<Request>().Update(reqToUpd);
                _unitOfWork.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Successfully updated the request!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> Delete(long reqId, bool hardDelete = false, string userId = null)
        {
            try
            {
                if (reqId > 0 && !string.IsNullOrWhiteSpace(userId))
                {
                    var reqToDel = _unitOfWork.GetRepository<Request>()
                        .Get(r => r.Id.Equals(reqId) && r.DeletedAt == null)
                        .SingleOrDefault();

                    if (reqToDel != null)
                    {
                        if (!hardDelete)
                        {
                            reqToDel.DeletedAt = DateTime.UtcNow;
                            reqToDel.DeletedById = userId;
                            _unitOfWork.GetRepository<Request>().Update(reqToDel);
                        }
                        else
                        {
                            _unitOfWork.GetRepository<Request>().Delete(reqToDel);
                        }

                        _unitOfWork.Commit(userId);

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