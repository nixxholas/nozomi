using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.Services.Requests
{
    public class RequestService : BaseService<RequestService, NozomiDbContext>, IRequestService
    {
        public RequestService(ILogger<RequestService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(Request req, long userId = 0)
        {
            // Safetynet
            if (req != null && req.IsValid())
            {
                _unitOfWork.GetRepository<Request>().Add(req);
                _unitOfWork.Commit(userId);

                return req.Id;
            }

            return -1;
        }

        public bool Update(Request req, long userId = 0)
        {
            // Safetynet
            if (req == null || !req.IsValid()) return false;
            
            var reqToUpd = _unitOfWork.GetRepository<Request>()
                .Get(r => r.Id.Equals(req.Id) && r.DeletedAt == null)
                .SingleOrDefault();

            if (reqToUpd == null) return false;

            reqToUpd.DataPath = req.DataPath;
            reqToUpd.RequestType = req.RequestType;
            reqToUpd.IsEnabled = req.IsEnabled;
                
            _unitOfWork.GetRepository<Request>().Update(reqToUpd);
            _unitOfWork.Commit(userId);

            return true;
        }

        public bool SoftDelete(long reqId, long userId = 0)
        {
            if (reqId > 0)
            {
                var reqToDel = _unitOfWork.GetRepository<Request>()
                    .Get(r => r.Id.Equals(reqId) && r.DeletedAt == null)
                    .SingleOrDefault();

                if (reqToDel != null)
                {
                    reqToDel.DeletedAt = DateTime.UtcNow;
                    reqToDel.DeletedBy = userId;

                    _unitOfWork.GetRepository<Request>().Update(reqToDel);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Request> GetAllActive(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(r => r.DeletedAt == null && r.IsEnabled);
            }

            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.DeletedAt == null && r.IsEnabled)
                .Include(r => r.RequestComponents)
                .Include(r => r.RequestProperties);
        }

        public IEnumerable<dynamic> GetAllActiveObsc(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(r => r.DeletedAt == null && r.IsEnabled)
                    .Select(r => new
                    {
                        id = r.Id,
                        dataPath = r.DataPath,
                        guid = r.Guid,
                        requestType = r.RequestType,
                        createdAt = r.CreatedAt,
                        createdBy = r.CreatedBy,
                        modifiedAt = r.ModifiedAt,
                        modifiedBy = r.ModifiedBy
                    });
            }

            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.DeletedAt == null && r.IsEnabled)
                .Include(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                .Include(r => r.RequestProperties)
                .Where(r => r.RequestComponents
                    .Any(rc => rc.DeletedAt == null && rc.IsEnabled))
                .Where(r => r.RequestProperties
                    .Any(rp => rp.DeletedAt == null & rp.IsEnabled))
                .Select(r => new
                {
                    id = r.Id,
                    dataPath = r.DataPath,
                    guid = r.Guid,
                    requestType = r.RequestType,
                    createdAt = r.CreatedAt,
                    createdBy = r.CreatedBy,
                    modifiedAt = r.ModifiedAt,
                    modifiedBy = r.ModifiedBy,
                    requestComponents = r.RequestComponents
                        .Select(rc => new
                        {
                            id = rc.Id,
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
                            createdAt = rp.CreatedAt,
                            createdBy = rp.CreatedBy,
                            modifiedAt = rp.ModifiedAt,
                            modifiedBy = rp.ModifiedBy
                        })
                });
        }

        public IEnumerable<Request> GetAll(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking();
            }

            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Include(r => r.RequestComponents)
                .Include(r => r.RequestProperties);
        }

        public IEnumerable<Request> GetAll(Expression<Func<Request, bool>> predicate, bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .Where(predicate)
                    .AsNoTracking();
            }

            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Include(r => r.RequestComponents)
                .Include(r => r.RequestProperties)
                .Where(predicate);
        }

        public IEnumerable<dynamic> GetAllObsc(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Select(r => new
                    {
                        id = r.Id,
                        dataPath = r.DataPath,
                        guid = r.Guid,
                        requestType = r.RequestType,
                        isEnabled = r.IsEnabled,
                        createdAt = r.CreatedAt,
                        createdBy = r.CreatedBy,
                        modifiedAt = r.ModifiedAt,
                        modifiedBy = r.ModifiedBy,
                        deletedAt = r.DeletedAt,
                        deletedBy = r.DeletedBy
                    });
            }

            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Include(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                .Include(r => r.RequestProperties)
                .Select(r => new
                {
                    id = r.Id,
                    dataPath = r.DataPath,
                    guid = r.Guid,
                    requestType = r.RequestType,
                    isEnabled = r.IsEnabled,
                    createdAt = r.CreatedAt,
                    createdBy = r.CreatedBy,
                    modifiedAt = r.ModifiedAt,
                    modifiedBy = r.ModifiedBy,
                    deletedAt = r.DeletedAt,
                    deletedBy = r.DeletedBy,
                    requestComponents = r.RequestComponents
                        .Select(rc => new
                        {
                            id = rc.Id,
                            queryComponent = rc.QueryComponent,
                            value = rc.RequestComponentDatum,
                            isEnabled = rc.IsEnabled,
                            createdAt = rc.CreatedAt,
                            createdBy = rc.CreatedBy,
                            modifiedAt = rc.ModifiedAt,
                            modifiedBy = rc.ModifiedBy,
                            deletedAt = rc.DeletedAt,
                            deletedBy = rc.DeletedBy
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
                            modifiedBy = rp.ModifiedBy,
                            deletedAt = rp.DeletedAt,
                            deletedBy = rp.DeletedBy
                        })
                });
        }
    }
}