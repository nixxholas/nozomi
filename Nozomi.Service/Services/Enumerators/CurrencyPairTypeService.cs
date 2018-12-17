using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Enumerators.Interfaces;

namespace Nozomi.Service.Services.Enumerators
{
    public class CurrencyPairTypeService : BaseService<CurrencyPairTypeService, NozomiDbContext>, ICurrencyPairTypeService
    {
        private readonly ICollection<KeyValuePair<string, int>> _currencyPairTypeMap;

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