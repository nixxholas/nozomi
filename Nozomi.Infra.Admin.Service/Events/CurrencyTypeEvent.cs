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
    public class CurrencyTypeEvent : BaseEvent<CurrencyTypeEvent, NozomiDbContext>, ICurrencyTypeEvent
    {
        public CurrencyTypeEvent(ILogger<CurrencyTypeEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public CurrencyType Get(long id, bool track = false)
        {
            if (id > 0)
            {
                var currencyType = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ct => ct.Id.Equals(id));

                if (currencyType.Any())
                {
                    if (track)
                    {
                        currencyType = currencyType
                            .Include(ct => ct.AnalysedComponents)
                            .Include(ct => ct.Currencies)
                            .Include(ct => ct.Requests);
                    }

                    return currencyType.SingleOrDefault();
                }
            }

            return null;
        }

        public ICollection<CurrencyType> GetAll(int index = 0, bool track = false)
        {
            if (index >= 0)
            {
                var cTypes = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking();

                if (track)
                {
                    cTypes = cTypes
                        .Include(ct => ct.AnalysedComponents)
                        .Include(ct => ct.Currencies)
                        .Include(ct => ct.Requests);
                }

                return cTypes
                    .Skip(index * NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                    .Take(NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                    .ToList();
            }

            return null;
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