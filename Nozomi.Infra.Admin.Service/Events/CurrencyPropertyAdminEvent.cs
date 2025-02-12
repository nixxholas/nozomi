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
    public class CurrencyPropertyAdminEvent : BaseEvent<CurrencyPropertyAdminEvent, NozomiDbContext>, ICurrencyPropertyAdminEvent
    {
        public CurrencyPropertyAdminEvent(ILogger<CurrencyPropertyAdminEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
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
                    .OrderBy(cp => cp.Id)
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
            if (id > 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyProperty>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.Id.Equals(id));

                if (track)
                {
                    query = query.Include(cp => cp.Currency);
                }

                return query.SingleOrDefault();
            }

            return null;
        }

        public ICollection<CurrencyProperty> GetAllByCurrency(long currencyId, bool track = false)
        {
            if (currencyId > 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyProperty>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.CurrencyId.Equals(currencyId));

                if (track)
                {
                    query = query.Include(cp => cp.Currency);
                }

                return query.ToList();
            }

            return null;
        }
    }
}