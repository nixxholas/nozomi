using System;
using System.Collections.Generic;
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
    public class CurrencyAdminEvent : BaseEvent<CurrencyAdminEvent, NozomiDbContext>, ICurrencyAdminEvent
    {
        private readonly Interfaces.ICurrencyPairSourceCurrencyAdminEvent _currencyPairSourceCurrencyAdminEvent;

        public CurrencyAdminEvent(ILogger<CurrencyAdminEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            Interfaces.ICurrencyPairSourceCurrencyAdminEvent currencyPairSourceCurrencyAdminEvent)
            : base(logger, unitOfWork)
        {
            _currencyPairSourceCurrencyAdminEvent = currencyPairSourceCurrencyAdminEvent;
        }

        public Currency GetCurrencyBySlug(string slug, bool track = false)
        {
            var currency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase));

            if (track)
            {
                currency = currency
                    .Include(c => c.CurrencyProperties)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .ThenInclude(s => s.CurrencyPairs)
                    .Include(c => c.Requests)
                    .ThenInclude(cr => cr.RequestComponents);
            }

            return currency.SingleOrDefault();
        }

        public ICollection<Currency> GetAll(bool track = false)
        {
            if (!track)
                return _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .ToList();
            
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .Include(c => c.AnalysedComponents)
                .Include(c => c.CurrencyProperties)
                .Include(c => c.CurrencySources)
                .Include(c => c.Requests)
                .Include(c => c.CurrencyType)
                .ToList();
        }
    }
}