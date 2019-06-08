using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Events
{
    public class CurrencyPropertyEvent : BaseEvent<CurrencyPropertyEvent, NozomiDbContext>, ICurrencyPropertyEvent
    {
        public CurrencyPropertyEvent(ILogger<CurrencyPropertyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ICollection<CurrencyProperty> GetAll(int index = 0, bool track = false)
        {
            if (index > 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyProperty>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Skip(index * NozomiServiceConstants.CurrencyPropertyTakeoutLimit)
                    .Take(NozomiServiceConstants.CurrencyPropertyTakeoutLimit);

                if (track)
                {
                    query = query
                        .Include(cp => cp.Currency);
                }

                return query.ToList();
            }

            return null;
        }

        public CurrencyProperty Get(long id, bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<CurrencyProperty> GetAllByCurrency(long currencyId, bool track = false)
        {
            throw new System.NotImplementedException();
        }
    }
}