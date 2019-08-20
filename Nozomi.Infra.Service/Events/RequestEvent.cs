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
            NozomiDbContext nozomiDbContext, RequestType requestType, bool includeNonHistorical = false)
        {
            var dict = new Dictionary<string, ICollection<Request>>();
            var currencyRequests = GetActiveCurrencyRequests(_unitOfWork.Context, requestType);

            if (!includeNonHistorical)
                currencyRequests = currencyRequests.Select(r => new Request
                {
                    Id = r.Id,
                    Guid = r.Guid,
                    RequestType = r.RequestType,
                    ResponseType = r.ResponseType,
                    DataPath = r.DataPath,
                    Delay = r.Delay,
                    FailureDelay = r.FailureDelay,
                    CurrencyId = r.CurrencyId,
                    Currency = r.Currency,
                    CurrencyPairId = r.CurrencyPairId,
                    CurrencyPair = r.CurrencyPair,
                    CurrencyTypeId = r.CurrencyTypeId,
                    CurrencyType = r.CurrencyType,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    IsEnabled = r.IsEnabled,
                    RequestComponents = r.RequestComponents
                        .Where(rc => rc.StoreHistoricals).ToList(),
                    RequestProperties = r.RequestProperties,
                    WebsocketCommands = r.WebsocketCommands
                });

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
                                            && r.RequestType == type
                                            && (DateTime.UtcNow >= r.ModifiedAt.Add(TimeSpan.FromMilliseconds(r.Delay))
                                                // This means the request has been recently created and requires syncing
                                                || r.CreatedAt.Equals(r.ModifiedAt))
                                            )); 

        public ICollection<Request> GetAllByRequestType(RequestType requestType, bool includeNonHistorical = false)
        {
            if (!includeNonHistorical)
                return CompiledGetAllByRequestType(_unitOfWork.Context, requestType).Select(r => new Request
                {
                    Id = r.Id,
                    Guid = r.Guid,
                    RequestType = r.RequestType,
                    ResponseType = r.ResponseType,
                    DataPath = r.DataPath,
                    Delay = r.Delay,
                    FailureDelay = r.FailureDelay,
                    CurrencyId = r.CurrencyId,
                    Currency = r.Currency,
                    CurrencyPairId = r.CurrencyPairId,
                    CurrencyPair = r.CurrencyPair,
                    CurrencyTypeId = r.CurrencyTypeId,
                    CurrencyType = r.CurrencyType,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    IsEnabled = r.IsEnabled,
                    RequestComponents = r.RequestComponents
                        .Where(rc => rc.StoreHistoricals).ToList(),
                    RequestProperties = r.RequestProperties,
                    WebsocketCommands = r.WebsocketCommands
                }).ToList();
            
            return CompiledGetAllByRequestType(_unitOfWork.Context, requestType).ToList();
        }

        public IDictionary<string, ICollection<Request>> GetAllByRequestTypeUniqueToURL(RequestType requestType
            , bool includeNonHistorical = false)
        {
            var dict = new Dictionary<string, ICollection<Request>>();
            
            #if DEBUG
            // Check the context
            var testRequestsFromContext = _unitOfWork.Context.Requests.ToList();
            #endif
            
            var requests = CompiledGetAllByRequestType(_unitOfWork.Context, requestType);
            
            if (!includeNonHistorical)
                requests = requests.Select(r => new Request
                {
                    Id = r.Id,
                    Guid = r.Guid,
                    RequestType = r.RequestType,
                    ResponseType = r.ResponseType,
                    DataPath = r.DataPath,
                    Delay = r.Delay,
                    FailureDelay = r.FailureDelay,    
                    CurrencyId = r.CurrencyId,
                    Currency = r.Currency,
                    CurrencyPairId = r.CurrencyPairId,
                    CurrencyPair = r.CurrencyPair,
                    CurrencyTypeId = r.CurrencyTypeId,
                    CurrencyType = r.CurrencyType,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    IsEnabled = r.IsEnabled,
                    RequestComponents = r.RequestComponents
                        .Where(rc => rc.StoreHistoricals).ToList(),
                    RequestProperties = r.RequestProperties,
                    WebsocketCommands = r.WebsocketCommands
                });

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