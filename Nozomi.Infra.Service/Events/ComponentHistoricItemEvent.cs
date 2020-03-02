using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class ComponentHistoricItemEvent : BaseEvent<ComponentHistoricItemEvent, NozomiDbContext>, 
        IComponentHistoricItemEvent
    {
        public ComponentHistoricItemEvent(ILogger<ComponentHistoricItemEvent> logger, 
            NozomiDbContext unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ComponentHistoricItem GetLastItem(long id, bool includeNested = false)
        {
            var query = _context.RcdHistoricItems.AsNoTracking()
                .Where(e => e.RequestComponentId.Equals(id));

            if (includeNested)
                query = query.Include(e => e.Component);

            return query.OrderByDescending(e => e.CreatedAt).FirstOrDefault();
        }

        public ComponentHistoricItem GetLastItem(string guid)
        {
            if (Guid.TryParse(guid, out var parsedGuid))
            {
                return _context.RcdHistoricItems.AsNoTracking()
                    .OrderByDescending(i => i.CreatedAt)
                    .Include(i => i.Component)
                    .FirstOrDefault(e => e.Component.Guid.Equals(parsedGuid));
            }
            
            throw new NullReferenceException($"{_eventName} GetLastItem (String): Invalid guid.");
        }

        public ComponentHistoricItem GetLastItem(Guid guid)
        {
            return _context.RcdHistoricItems.AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .Include(e => e.Component)
                .FirstOrDefault(e => e.Component.Guid.Equals(guid));
        }
    }
}