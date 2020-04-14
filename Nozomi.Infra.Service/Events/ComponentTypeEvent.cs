using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.ComponentType;
using Nozomi.Preprocessing;
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

        public IEnumerable<ComponentTypeViewModel> ViewAll(string userId = null, int index = 0)
        {
            if (index < 0) throw new IndexOutOfRangeException("Invalid index.");
            
            var query = _context.ComponentTypes.AsNoTracking()
                .Include(e => e.Components)
                .ThenInclude(c => c.RcdHistoricItems)
                .Where(ct => ct.DeletedAt == null && ct.IsEnabled);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(e => e.CreatedById.Equals(userId))
                    .Where(e => e.CreatedById == null);
            else
                query = query.Where(e => e.CreatedById == null);
            
            return query
                .Skip(index * NozomiServiceConstants.ComponentTypeTakeoutLimit)
                .Take(NozomiServiceConstants.ComponentTypeTakeoutLimit)
                .Select(ct => new ComponentTypeViewModel
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    Slug = ct.Slug,
                    Description = ct.Description,
                    IsEnabled = ct.IsEnabled,
                    Components = ct.Components
                        .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                        .Select(c => new ComponentViewModel
                        {
                            Guid = c.Guid,
                            Type = c.ComponentTypeId,
                            Identifier = c.Identifier,
                            Query = c.QueryComponent,
                            Value = c.RcdHistoricItems
                                    // Always take the first 'page'
                                .Take(NozomiServiceConstants.RcdHistoricItemTakeoutLimit)
                                .OrderByDescending(rcdhi => rcdhi.HistoricDateTime)
                                .FirstOrDefault()
                                .Value
                        })
                });
        }
    }
}