using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyTypeEvent : BaseEvent<CurrencyTypeEvent, NozomiDbContext>, ICurrencyTypeEvent
    {
        public CurrencyTypeEvent(ILogger<CurrencyTypeEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyType> GetAllActive(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .Where(ct => ct.IsEnabled && ct.DeletedAt == null)
                    .Include(ct => ct.Currencies)
                    .ToList();
            }
            
            return _unitOfWork.GetRepository<CurrencyType>()
                .Get(ct => ct.IsEnabled && ct.DeletedAt == null)
                .ToList();
        }
    }
}