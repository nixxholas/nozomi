using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Data.ViewModels.RequestProperty;
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

        public bool Exists(long requestId, bool ignoreDeletedOrDisabled = false, string userId = null)
        {
            if (requestId > 0)
            {
                var query = _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(r => r.Id.Equals(requestId));

                if (!ignoreDeletedOrDisabled) // Filter if false
                    query = query.Where(r => r.IsEnabled && r.DeletedAt == null);

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(r => r.CreatedById.Equals(userId));

                return query.Any();
            }

            return false;
        }

        public bool Exists(string requestGuid, bool ignoreDeletedOrDisabled = false, string userId = null)
        {
            if (Guid.TryParse(requestGuid, out var parsedGuid))
            {
                var query = _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(r => r.Guid.Equals(parsedGuid));

                if (!ignoreDeletedOrDisabled)
                    query = query.Where(r => r.IsEnabled && r.DeletedAt == null);

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(r => r.CreatedById.Equals(userId));

                return query.Any();
            }

            return false;
        }

        public bool Exists(ComponentType type, long requestId)
        {
            return _unitOfWork.GetRepository<Component>()
                .GetQueryable()
                .AsNoTracking()
                .Any(rc => rc.DeletedAt == null && rc.IsEnabled
                                                && rc.ComponentType.Equals(type) && rc.RequestId.Equals(requestId));
        }

        public bool Exists(ComponentType type, string requestGuid)
        {
            return _unitOfWork.GetRepository<Component>()
                .GetQueryable()
                .AsNoTracking()
                .Include(rc => rc.Request)
                .Any(rc => rc.DeletedAt == null && rc.IsEnabled
                                                && rc.ComponentType.Equals(type)
                                                && rc.Request.Guid.Equals(Guid.Parse(requestGuid)));
        }

        public long GetId(string guid)
        {
            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.DeletedAt == null && r.IsEnabled
                                                && r.Guid.Equals(Guid.Parse(guid)))
                .Select(r => r.Id)
                .FirstOrDefault();
        }

        public Request Get(Expression<Func<Request, bool>> predicate)
        {
            return _unitOfWork.GetRepository<Request>()
                       .GetQueryable()
                       .AsNoTracking()
                       .Where(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                       .Include(r => r.RequestComponents)
                       .Include(r => r.RequestProperties)
                       .SingleOrDefault(predicate) ?? null;
        }

        public Request GetByGuid(Guid guid, bool track = false)
        {
            var query = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking();

            if (track)
            {
                query.Include(r => r.RequestComponents)
                    .Include(r => r.RequestProperties);
            }

            return query.FirstOrDefault(r => r.Guid.Equals(guid));
        }

        public Request GetActive(long id, bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking();

            if (includeNested)
            {
                query = query.Include(r => r.RequestComponents)
                    .Include(r => r.RequestProperties);
            }

            return query?
                .SingleOrDefault(r => r.Id.Equals(id) && r.DeletedAt == null);
        }

        public IQueryable<RequestViewModel> ViewAll(string userId = null, bool enabledOnly = true, bool track = false)
        {
            var query = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Include(r => r.Currency)
                .Include(r => r.CurrencyPair)
                .Include(r => r.CurrencyType)
                .Where(r => r.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrEmpty(userId))
                query = query.Where(r => r.CreatedById.Equals(userId));
            
            if (enabledOnly)
                query = query.Where(r => r.IsEnabled);

            if (track)
                return query
                    .Include(r => r.RequestComponents)
                    .Include(r => r.RequestProperties)
                    .Include(r => r.RequestType)
                    .Select(r => new RequestViewModel(r.Guid, r.RequestType, r.ResponseType, r.DataPath, 
                        r.Delay, r.FailureDelay, r.IsEnabled, r.CurrencyId > 0 ? r.Currency.Slug : null, 
                        r.CurrencyPairId > 0 ? r.CurrencyPair.Guid.ToString() : null, 
                        r.CurrencyTypeId > 0 ? r.CurrencyType.Guid.ToString() : null,
                        r.RequestComponents.Select(rc => new ComponentViewModel
                        {
                            Guid = rc.Guid,
                            Type = rc.ComponentType,
                            Value = rc.Value,
                            IsDenominated = rc.IsDenominated
                        }).ToList(),
                        r.RequestProperties.Select(rp => new RequestPropertyViewModel(rp.Guid, 
                            rp.RequestPropertyType, rp.Key, rp.Value)).ToList()));
            
            return query
                .Select(r => new RequestViewModel(r.Guid, r.RequestType, r.ResponseType, r.DataPath, r.Delay,
                    r.FailureDelay, r.IsEnabled, r.CurrencyId > 0 ? r.Currency.Slug : null, 
                    r.CurrencyPairId > 0 ? r.CurrencyPair.Guid.ToString() : null, 
                    r.CurrencyTypeId > 0 ? r.CurrencyType.Guid.ToString() : null, 
                    null, null));
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
                    Id = r.Id,
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
                        createdBy = r.CreatedById,
                        modifiedAt = r.ModifiedAt,
                        modifiedBy = r.ModifiedById
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
                    createdBy = r.CreatedById,
                    modifiedAt = r.ModifiedAt,
                    modifiedBy = r.ModifiedById,
                    requestComponents = r.RequestComponents
                        .Select(rc => new
                        {
                            id = rc.Id,
                            queryComponent = rc.QueryComponent,
                            value = rc.Value,
                            isEnabled = rc.IsEnabled,
                            createdAt = rc.CreatedAt,
                            createdBy = rc.CreatedById,
                            modifiedAt = rc.ModifiedAt,
                            modifiedBy = rc.ModifiedById
                        }),
                    requestProperties = r.RequestProperties
                        .Select(rp => new
                        {
                            id = rp.Id,
                            requestPropertyType = rp.RequestPropertyType,
                            key = rp.Key,
                            value = rp.Value,
                            createdAt = rp.CreatedAt,
                            createdBy = rp.CreatedById,
                            modifiedAt = rp.ModifiedAt,
                            modifiedBy = rp.ModifiedById
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
                        createdBy = r.CreatedById,
                        modifiedAt = r.ModifiedAt,
                        modifiedBy = r.ModifiedById,
                        deletedAt = r.DeletedAt,
                        deletedBy = r.DeletedById
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
                    createdBy = r.CreatedById,
                    modifiedAt = r.ModifiedAt,
                    modifiedBy = r.ModifiedById,
                    deletedAt = r.DeletedAt,
                    deletedBy = r.DeletedById,
                    requestComponents = r.RequestComponents
                        .Select(rc => new
                        {
                            id = rc.Id,
                            queryComponent = rc.QueryComponent,
                            value = rc.Value,
                            isEnabled = rc.IsEnabled,
                            createdAt = rc.CreatedAt,
                            createdBy = rc.CreatedById,
                            modifiedAt = rc.ModifiedAt,
                            modifiedBy = rc.ModifiedById,
                            deletedAt = rc.DeletedAt,
                            deletedBy = rc.DeletedById
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
                            createdBy = rp.CreatedById,
                            modifiedAt = rp.ModifiedAt,
                            modifiedBy = rp.ModifiedById,
                            deletedAt = rp.DeletedAt,
                            deletedBy = rp.DeletedById
                        })
                });
        }

        // CurrencyRequest obtainability
        private static readonly Func<NozomiDbContext, RequestType, IEnumerable<Request>>
            GetActiveCurrencyRequests =
                EF.CompileQuery((NozomiDbContext context, RequestType requestType) =>
                    context.Requests
                        .Where(cr => cr.DeletedAt == null && cr.IsEnabled
                                                          && cr.CurrencyId > 0
                                                          && cr.RequestType.Equals(requestType))
                        // TODO: Websocket Support
                        .Include(cr => cr.RequestComponents)
                        .Include(cr => cr.RequestProperties));

        public IDictionary<string, ICollection<Request>> GetAllByRequestTypeUniqueToUrl(
            NozomiDbContext nozomiDbContext, RequestType requestType)
        {
            var dict = new Dictionary<string, ICollection<Request>>();
            var currencyRequests = GetActiveCurrencyRequests(_unitOfWork.Context, requestType);

            foreach (var cReq in currencyRequests)
            {
                // If the key exists,
                if (dict.ContainsKey(cReq.DataPath) && dict[cReq.DataPath] != null
                                                    && dict[cReq.DataPath].Count > 0)
                {
                    dict[cReq.DataPath].Add(cReq);
                }
                // If not create it
                else
                {
                    dict.Add(cReq.DataPath, new List<Request>());
                    dict[cReq.DataPath].Add(cReq);
                }
            }

            return dict;
        }

        private static readonly Func<NozomiDbContext, RequestType, IEnumerable<Request>>
            CompiledGetAllByRequestType =
                EF.CompileQuery((NozomiDbContext context, RequestType type) =>
                    context.Requests
                        .AsQueryable()
                        .Include(cpr => cpr.RequestComponents)
                        .Include(r => r.CurrencyPair)
                        .Include(r => r.RequestProperties)
                        .Include(r => r.WebsocketCommands)
                        .ThenInclude(wsc => wsc.WebsocketCommandProperties)
                        .Where(r => r.IsEnabled && r.DeletedAt == null
                                                && r.RequestComponents
                                                    .Any(rc => rc.DeletedAt == null && rc.IsEnabled)
                                                && r.RequestType == type));

        public ICollection<Request> GetAllByRequestType(RequestType requestType)
        {
            return CompiledGetAllByRequestType.Invoke(_unitOfWork.Context, requestType)
                .Where(r => DateTime.UtcNow >= r.ModifiedAt.AddMilliseconds(r.Delay)
                            // This means the request has been recently created and requires syncing
                            || r.CreatedAt.Equals(r.ModifiedAt))
                .Select(r => new Request(r)).ToList();
        }

        public IDictionary<string, ICollection<Request>> GetAllByRequestTypeUniqueToURL(RequestType requestType)
        {
            var dict = new Dictionary<string, ICollection<Request>>();

            var requests = CompiledGetAllByRequestType.Invoke(_unitOfWork.Context, requestType)
                .Where(r => DateTime.UtcNow >= r.ModifiedAt.AddMilliseconds(r.Delay)
                            // This means the request has been recently created and requires syncing
                            || r.CreatedAt.Equals(r.ModifiedAt));

            foreach (var request in requests)
            {
                // If the key exists,
                if (dict.ContainsKey(request.DataPath) && dict[request.DataPath] != null
                                                       && dict[request.DataPath].Count > 0)
                {
                    dict[request.DataPath].Add(request);
                }
                // If not create it
                else
                {
                    dict.Add(request.DataPath, new List<Request>());
                    dict[request.DataPath].Add(request);
                }
            }

            return dict;
        }
    }
}