using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestPropertyEvent : BaseEvent<RequestPropertyEvent, NozomiDbContext>, IRequestPropertyEvent
    {
        public RequestPropertyEvent(ILogger<RequestPropertyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public IEnumerable<RequestPropertyViewModel> GetByRequest(string requestGuid)
        {
            throw new System.NotImplementedException();
        }
    }
}