using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Events
{
    public class CurrencyPairAdminEvent : BaseEvent<CurrencyPairAdminEvent, NozomiDbContext>, ICurrencyPairAdminEvent
    {
        public CurrencyPairAdminEvent(ILogger<CurrencyPairAdminEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }
        
        public CurrencyPair Get(long id, bool track = false)
        {
            if (track)
                return _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Include(cp => cp.Requests)
                    .Include(cp => cp.Source)
                    .Include(cp => cp.AnalysedComponents)
                    .SingleOrDefault(cp => cp.Id.Equals(id));
            
            return _unitOfWork
                .GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(cp => cp.Id.Equals(id));
        }
    }
}