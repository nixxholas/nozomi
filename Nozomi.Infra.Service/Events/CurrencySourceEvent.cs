using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Categorisation;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencySourceEvent : BaseEvent<ItemSource, NozomiDbContext>, ICurrencySourceEvent
    {
        public CurrencySourceEvent(ILogger<ItemSource> logger, NozomiDbContext unitOfWork) : base(logger, unitOfWork)
        {
        }

        public bool Exists(long sourceId, long currencyId)
        {
            return _context.CurrencySources
                .Any(cs => cs.DeletedAt == null
                           && cs.SourceId.Equals(sourceId)
                           && cs.ItemId.Equals(currencyId));
        }
    }
}