using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Extensions;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Nozomi.Preprocessing;
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
            var query = _context.ComponentHistoricItems.AsNoTracking()
                .Where(e => e.RequestComponentId.Equals(id));

            if (includeNested)
                query = query.Include(e => e.Component);

            return query.OrderByDescending(e => e.CreatedAt).FirstOrDefault();
        }

        public ComponentHistoricItem GetLastItem(string guid)
        {
            if (Guid.TryParse(guid, out var parsedGuid))
            {
                return _context.ComponentHistoricItems.AsNoTracking()
                    .OrderByDescending(i => i.CreatedAt)
                    .Include(i => i.Component)
                    .FirstOrDefault(e => e.Component.Guid.Equals(parsedGuid));
            }
            
            throw new NullReferenceException($"{_eventName} GetLastItem (String): Invalid guid.");
        }

        public ComponentHistoricItem GetLastItem(Guid guid)
        {
            return _context.ComponentHistoricItems.AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .Include(e => e.Component)
                .FirstOrDefault(e => e.Component.Guid.Equals(guid));
        }

        public IEnumerable<ComponentHistoricItemViewModel> ViewAll(int index = 0, string componentGuid = null)
        {
            if (index < 0) throw new IndexOutOfRangeException("Invalid index.");

            var query = _context.ComponentHistoricItems.AsNoTracking()
                .Where(chi => chi.DeletedAt == null && chi.IsEnabled);

            if (!string.IsNullOrEmpty(componentGuid) && Guid.TryParse(componentGuid, out var parsedGuid))
                query = query
                    .Include(chi => chi.Component)
                    .Where(chi => chi.Component.Guid.Equals(parsedGuid));

            return query
                .OrderByDescending(chi => chi.HistoricDateTime)
                .Skip(index * NozomiServiceConstants.RcdHistoricItemTakeoutLimit)
                .Take(NozomiServiceConstants.RcdHistoricItemTakeoutLimit)
                .Select(chi => new ComponentHistoricItemViewModel
                {
                    Timestamp = chi.HistoricDateTime,
                    Value = chi.Value
                });
        }
    }
}