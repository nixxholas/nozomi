
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
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
                    .ThenInclude(rc => rc.RequestComponentData)
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
                            value = rc.RequestComponentData
                                .OrderByDescending(rcd => rcd.CreatedAt)
                                .Select(rcd => rcd.Value).FirstOrDefault(),
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

        public bool SoftDelete(long cpRequestId, long userId = 0)
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

        public IEnumerable<CurrencyPairRequest> GetAllActive(bool track = false)
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
                .Include(r => r.CurrencyPair)
                    .ThenInclude(cp => cp.CurrencyPairComponents)
                .Include(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentData)
                .Include(r => r.RequestProperties);
        }

        public IEnumerable<CurrencyPairRequest> GetAllActive(Expression<Func<CurrencyPairRequest, bool>> predicate, bool track = false)
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
                .Include(r => r.CurrencyPair)
                    .ThenInclude(cp => cp.CurrencyPairComponents)
                        .ThenInclude(cpc => cpc.RequestComponentData)
                .Include(r => r.RequestProperties)
                .Where(predicate);
        }
    }
}