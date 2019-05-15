using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestEvent : BaseEvent<RequestEvent, NozomiDbContext>, IRequestEvent
    {
        public RequestEvent(ILogger<RequestEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        // Elvis 4/17/2019
        // TODO: Fix the query so that it returns it's related data
        public Request GetByGuid(Guid guid, bool track = false)
        {
            var query = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking();

            if (track)
            {
                query.Include(r => r.RequestComponents)
                    .Include(r => r.RequestLogs)
                    .Include(r => r.RequestProperties)
                    .Include(r => r.AnalysedComponents);
            }
            
            return query.FirstOrDefault(r => r.Guid.Equals(guid));
        }

        public Request GetActive(long id, bool track = false)
        {
            var query = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking();

            if (query != null)
            {
                if (track)
                {
                    query.Include(r => r.RequestComponents)
                        .Include(r => r.RequestLogs)
                        .Include(r => r.RequestProperties)
                        .Include(r => r.AnalysedComponents);
                }
            }

            return query?
                .SingleOrDefault(r => r.Id.Equals(id) && r.DeletedAt == null
                                                      && r.IsEnabled);
        }

        public ICollection<RequestDTO> GetAllDTO(int index)
        {
            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.DeletedAt == null && r.IsEnabled)
                .Skip(index * 50)
                .Take(50)
                .Select(r => new RequestDTO
                {
                    Guid = r.Guid,
                    RequestType = r.RequestType,
                    ResponseType = r.ResponseType,
                    DataPath = r.DataPath,
                    Delay = r.Delay,
                    FailureDelay = r.FailureDelay
                })
                .ToList();
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
                            value = rc.Value,
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
                            value = rc.Value,
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