using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class ComponentTypeEvent : BaseService<ComponentTypeEvent, NozomiDbContext>, IComponentTypeEvent
    {
        public ComponentTypeEvent(ILogger<ComponentTypeEvent> logger,
            NozomiDbContext context) : base(logger, context)
        {
        }

        public IEnumerable<KeyValuePair<string, long>> All(string userId = null)
        {
            var query = _context.ComponentTypes.AsNoTracking()
                .Where(ct => ct.DeletedAt == null && ct.IsEnabled);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(e => e.CreatedById.Equals(userId))
                    .Where(e => e.CreatedById == null);
            else
                query = query.Where(e => e.CreatedById == null);
            
            return query
                .Select(ct => new KeyValuePair<string, long>(ct.Name, ct.Id));
        }
    }
}