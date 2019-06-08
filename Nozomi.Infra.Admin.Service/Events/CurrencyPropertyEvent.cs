using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Events.Interfaces;
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
            throw new System.NotImplementedException();
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