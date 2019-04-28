using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}