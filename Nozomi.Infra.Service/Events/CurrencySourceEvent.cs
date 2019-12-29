using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencySourceEvent : BaseEvent<CurrencySource, NozomiDbContext>, ICurrencySourceEvent
    {
        public CurrencySourceEvent(ILogger<CurrencySource> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public bool Exists(long sourceId, long currencyId)
        {
            throw new System.NotImplementedException();
        }
    }
}