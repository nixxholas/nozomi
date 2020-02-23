using System;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class ComponentHistoricItemEvent : BaseEvent<ComponentHistoricItemEvent, NozomiDbContext>, 
        IComponentHistoricItemEvent
    {
        public ComponentHistoricItemEvent(ILogger<ComponentHistoricItemEvent> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ComponentHistoricItem GetLastItem(long id, bool includeNested = false)
        {
            throw new NotImplementedException();
        }

        public ComponentHistoricItem GetLastItem(string guid)
        {
            throw new NotImplementedException();
        }

        public ComponentHistoricItem GetLastItem(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}