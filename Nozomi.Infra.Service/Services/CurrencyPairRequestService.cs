
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairRequestService : BaseService<CurrencyPairRequestService, NozomiDbContext>, ICurrencyPairRequestService
    {
        public CurrencyPairRequestService(ILogger<CurrencyPairRequestService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(CurrencyPairRequest cpRequest, long userId = 0)
        {
            // Safetynet
            if (cpRequest != null && cpRequest.IsValid())
            {
                _unitOfWork.GetRepository<CurrencyPairRequest>().Add(cpRequest);
                _unitOfWork.Commit(userId);

                return cpRequest.Id;
            }

            return -1;
        }

        public CurrencyPairRequest Get(Expression<Func<CurrencyPairRequest, bool>> predicate)
        {
            return _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                .Include(r => r.RequestComponents)
                .Include(r => r.RequestProperties)
                .SingleOrDefault(predicate) ?? null;
        }

        public dynamic GetObsc(Expression<Func<CurrencyPairRequest, bool>> predicate)
        {
            return _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                .Include(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                .Include(r => r.RequestProperties)
                .Include(r => r.RequestLogs)
                .Where(predicate)
                .Select(r => new
                {
                    id = r.Id,
                    guid = r.Guid,
                    requestType = r.RequestType,
                    dataPath = r.DataPath,
                    requestComponents = r.RequestComponents
                        .Select(rc => new
                        {
                            id = rc.Id,
                            componentType = rc.ComponentType,
                            queryComponent = rc.QueryComponent,
                            value = rc.RequestComponentDatum,
                            isEnabled = rc.IsEnabled,
                            createdAt = rc.CreatedAt,
                            createdBy = rc.CreatedBy,
                            modifiedAt = rc.ModifiedAt,
                            modifiedBy = rc.ModifiedBy
                        }),
                    requestProperties = r.RequestProperties
                        .Select(rp => new
                        {
                            id = rp.Id,
                            requestPropertyType = rp.RequestPropertyType,
                            key = rp.Key,
                            value = rp.Value,
                            isEnabled = rp.IsEnabled,
                            createdAt = rp.CreatedAt,
                            createdBy = rp.CreatedBy,
                            modifiedAt = rp.ModifiedAt,
                            modifiedBy = rp.ModifiedBy
                        }),
                    requestLogs = r.RequestLogs
                        .Select(rl => new
                        {
                            id = rl.Id,
                            type = rl.Type,
                            rawPayload = rl.RawPayload,
                            isEnabled = rl.IsEnabled,
                            createdAt = rl.CreatedAt,
                            modifiedAt = rl.ModifiedAt,
                            modifiedBy = rl.ModifiedBy
                        }),
                    isEnabled = r.IsEnabled,
                    createdAt = r.CreatedAt,
                    createdBy = r.CreatedBy,
                    modifiedAt = r.ModifiedAt,
                    modifiedBy = r.ModifiedBy
                })
                .SingleOrDefault() ?? null;
        }

        public bool Update(CurrencyPairRequest cpRequest, long userId = 0)
        {
            // Safetynet
            if (cpRequest == null || !cpRequest.IsValid()) return false;

            var reqToUpd = _unitOfWork.GetRepository<CurrencyPairRequest>()
                .Get(r => r.Id.Equals(cpRequest.Id) && r.DeletedAt == null)
                .SingleOrDefault();

            if (reqToUpd == null) return false;

            reqToUpd.DataPath = cpRequest.DataPath;
            reqToUpd.RequestType = cpRequest.RequestType;
            reqToUpd.IsEnabled = cpRequest.IsEnabled;

            _unitOfWork.GetRepository<CurrencyPairRequest>().Update(reqToUpd);
            _unitOfWork.Commit(userId);

            return true;
        }
        
        public bool Update(UpdateCurrencyPairRequest cpRequest, long userId = 0)
        {
            // Safetynet
            if (cpRequest == null) return false;

            var reqToUpd = _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable(r => r.Id.Equals(cpRequest.Id) && r.DeletedAt == null)
                .Include(cpr => cpr.RequestComponents)
                .Include(cpr => cpr.RequestProperties)
                .SingleOrDefault();

            if (reqToUpd == null) return false;

            // Include RequestComponents if there are any modified objects
            if (cpRequest.CurrencyPairComponents != null && cpRequest.CurrencyPairComponents.Count > 0)
            {
                foreach (var ucpc in cpRequest.CurrencyPairComponents)
                {
                    var cpc = reqToUpd.RequestComponents.SingleOrDefault(rc => rc.Id.Equals(ucpc.Id));
 
                    if (cpc == null) return false;

                    // Deleting?
                    if (ucpc.ToBeDeleted())
                    {
                        cpc.DeletedAt = DateTime.UtcNow;
                        cpc.DeletedBy = userId;
                        
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
            if (cpRequest.RequestProperties != null && cpRequest.RequestProperties.Count > 0)
            {
                foreach (var urp in cpRequest.RequestProperties)
                {
                    var requestProperty = reqToUpd.RequestProperties.SingleOrDefault(rc => rc.Id.Equals(urp.Id));
 
                    if (requestProperty == null) return false;

                    // Deleting?
                    if (urp.ToBeDeleted())
                    {
                        requestProperty.DeletedAt = DateTime.UtcNow;
                        requestProperty.DeletedBy = userId;
                        
                        _unitOfWork.GetRepository<RequestProperty>().Update(requestProperty);
                    }
                    // Updating?
                    else
                    {
                        if (urp.RequestPropertyType > 0) requestProperty.RequestPropertyType = urp.RequestPropertyType;
                        if (urp.Key != null) requestProperty.Key = urp.Key;
                        if (urp.Value != null) requestProperty.Value = urp.Value;
                        
                        _unitOfWork.GetRepository<RequestProperty>().Update(requestProperty);
                    }
                }
            }

            reqToUpd.DataPath = cpRequest.DataPath;
            reqToUpd.RequestType = cpRequest.RequestType;
            reqToUpd.IsEnabled = cpRequest.IsEnabled;

            _unitOfWork.GetRepository<CurrencyPairRequest>().Update(reqToUpd);
            _unitOfWork.Commit(userId);

            return true;
        }

        public bool Delete(long cpRequestId, bool hardDelete, long userId = 0)
        {
            if (cpRequestId > 0)
            {
                var reqToDel = _unitOfWork.GetRepository<CurrencyPairRequest>()
                    .Get(r => r.Id.Equals(cpRequestId) && r.DeletedAt == null)
                    .SingleOrDefault();

                if (reqToDel != null)
                {
                    reqToDel.DeletedAt = DateTime.UtcNow;
                    reqToDel.DeletedBy = userId;

                    _unitOfWork.GetRepository<CurrencyPairRequest>().Update(reqToDel);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public IQueryable<CurrencyPairRequest> GetAllActive(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<CurrencyPairRequest>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(r => r.DeletedAt == null && r.IsEnabled);
            }

            return _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.DeletedAt == null && r.IsEnabled)
                .Include(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                .Include(r => r.CurrencyPair)
                .Include(r => r.RequestProperties);
        }

        public IQueryable<CurrencyPairRequest> GetAllActive(Expression<Func<CurrencyPairRequest, bool>> predicate, bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<CurrencyPairRequest>()
                    .GetQueryable()
                    .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                    .Where(predicate)
                    .AsNoTracking();
            }

            return _unitOfWork.GetRepository<CurrencyPairRequest>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                .Include(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                .Include(r => r.CurrencyPair)
                .Include(r => r.RequestProperties)
                .Where(predicate);
        }

        // Introducing Compiled Queries
        private static readonly Func<NozomiDbContext, RequestType, IEnumerable<CurrencyPairRequest>> _getCurrencyPairRequestByRequestType =
            EF.CompileQuery((NozomiDbContext context, RequestType type) =>
                context.CurrencyPairRequests
                    .AsQueryable()
                    .Include(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .Include(r => r.CurrencyPair)
                    .Include(r => r.RequestProperties)
                    .Where(r => r.IsEnabled && r.DeletedAt == null
                                            && r.RequestType == type
                                            && r.RequestComponents.Any(rc => rc.RequestComponentDatum == null
                                                                             || (DateTime.UtcNow > (rc.RequestComponentDatum
                                                                             .CreatedAt.Add(TimeSpan.FromMilliseconds(r.Delay)))))));

        public ICollection<CurrencyPairRequest> GetAllByRequestType(RequestType requestType)
        {
            return _getCurrencyPairRequestByRequestType(_unitOfWork.Context, requestType).ToList();
        }

        public bool ManualPoll(long id, long userId = 0)
        {
            return false;
        }
    }
}