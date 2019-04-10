using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    }
}