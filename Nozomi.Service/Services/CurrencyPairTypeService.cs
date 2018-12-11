using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nozomi.Core.Helpers.Enumerator;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairTypeService : BaseService<CurrencyPairTypeService, NozomiDbContext>, ICurrencyPairTypeService
    {
        private ICollection<KeyValuePair<string, int>> _currencyPairTypeMap;

        public CurrencyPairTypeService(ILogger<CurrencyPairTypeService> logger,
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