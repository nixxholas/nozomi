using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels.CurrencyType;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class CurrencyTypeEvent : BaseEvent<CurrencyPairEvent, NozomiDbContext>, ICurrencyTypeEvent
    {
        public CurrencyTypeEvent(ILogger<CurrencyPairEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public CurrencyType Get(string guid, bool track = false)
        {
            if (!string.IsNullOrWhiteSpace(guid))
            {
                var currencyType = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ct => ct.DeletedAt == null && ct.IsEnabled 
                                                      && ct.Guid.Equals(Guid.Parse(guid)));

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

        public CurrencyType Get(long id, bool track = false)
        {
            if (id > 0)
            {
                var currencyType = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(ct => ct.DeletedAt == null && ct.IsEnabled 
                                                      && ct.Id.Equals(id));

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
                    .AsNoTracking()
                    .Where(ct => ct.DeletedAt == null && ct.IsEnabled);

                if (track)
                {
                    cTypes = cTypes
                        .Include(ct => ct.AnalysedComponents)
                        .Include(ct => ct.Currencies)
                        .Include(ct => ct.Requests);
                }

                return cTypes
                    .OrderBy(ct => ct.Id)
                    .Skip(index * NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                    .Take(NozomiServiceConstants.CurrencyTypeTakeoutLimit)
                    .ToList();
            }

            return null;
        }

        public ICollection<DistinctCurrencyTypeResponse> ListAll()
        {
            return _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ct => ct.DeletedAt == null && ct.IsEnabled)
                .Select(ct => new DistinctCurrencyTypeResponse()
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    ShortForm = ct.TypeShortForm
                })
                .ToList();
        }
    }
}