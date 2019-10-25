using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Interfaces;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Services.Enumerators
{
    public class CurrencyPairTypeEvent : BaseService<CurrencyPairTypeEvent, NozomiDbContext>, ICurrencyPairTypeEvent
    {
        private readonly ICollection<KeyValuePair<string, int>> _currencyPairTypeMap;

        public CurrencyPairTypeEvent(ILogger<CurrencyPairTypeEvent> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _currencyPairTypeMap = new List<KeyValuePair<string, int>>();

            foreach (var name in Enum.GetNames(typeof(CurrencyPairType)))
            {
                _currencyPairTypeMap.Add(new KeyValuePair<string, int>(name,
                    (int) Enum.Parse(typeof(CurrencyPairType), name)));
            }
        }

        public ICollection<KeyValuePair<string, int>> All()
        {
            return _currencyPairTypeMap;
        }
    }
}