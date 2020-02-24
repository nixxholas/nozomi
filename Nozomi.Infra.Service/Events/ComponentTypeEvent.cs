using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class ComponentTypeEvent : BaseService<ComponentTypeEvent, NozomiDbContext>, IComponentTypeEvent
    {
        // private readonly ICollection<KeyValuePair<string, int>> _componentTypeMap;

        public ComponentTypeEvent(ILogger<ComponentTypeEvent> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            // _componentTypeMap = new List<KeyValuePair<string, int>>();
            //
            // foreach (var name in Enum.GetNames(typeof(ComponentType)))
            // {
            //     _componentTypeMap.Add(new KeyValuePair<string, int>(name,
            //         (int) Enum.Parse(typeof(ComponentType), name)));
            // }
        }

        public IEnumerable<KeyValuePair<string, long>> All()
        {
            return _unitOfWork.GetRepository<ComponentType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ct => ct.DeletedAt == null && ct.IsEnabled)
                .Select(ct => new KeyValuePair<string, long>(ct.Name, ct.Id));
        }
    }
}