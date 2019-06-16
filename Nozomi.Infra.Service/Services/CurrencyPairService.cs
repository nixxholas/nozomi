using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairService : BaseService<CurrencyPairService, NozomiDbContext>, ICurrencyPairService
    {
        public CurrencyPairService(ILogger<CurrencyPairService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger,
            unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateCurrencyPair createCurrencyPair, long userId)
        {
            if (createCurrencyPair == null || !createCurrencyPair.IsValid()) 
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid");
            
            var currencyPair = new CurrencyPair()
            {
                CurrencyPairType = createCurrencyPair.CurrencyPairType,
                APIUrl = createCurrencyPair.APIUrl,
                DefaultComponent = createCurrencyPair.DefaultComponent,
                SourceId = createCurrencyPair.SourceId,
                MainCurrencyAbbrv = createCurrencyPair.MainCurrency.Abbreviation,
                CounterCurrencyAbbrv = createCurrencyPair.CounterCurrency.Abbreviation
            };
            
            _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
            _unitOfWork.Commit(userId);

            return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair successfully created");
        }

        public bool Delete(long currencyPairId, long userId = 0, bool hardDelete = false)
        {
            if (currencyPairId > 0)
            {
                var cpToDel = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .SingleOrDefault(cp => cp.Id.Equals(currencyPairId));

                if (hardDelete && cpToDel != null)
                {
                    _unitOfWork.GetRepository<CurrencyPair>().Delete(cpToDel);
                }
                else if (cpToDel != null && cpToDel.DeletedAt == null)
                {
                    cpToDel.DeletedAt = DateTime.UtcNow;
                    cpToDel.DeletedBy = userId;
                }
                else
                {
                    // User has attempted to delete a deleted entity.
                    return false;
                }

                _unitOfWork.Commit(userId);

                return true;
            }

            return false;
        }

        public IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false)
        {
            return !includeNested ? _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Include(cp => cp.Requests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Skip(index * 20)
                .Take(20) :
                _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Skip(index * 20)
                    .Take(20);
        }

        public IEnumerable<string> GetAllCurrencyPairUrls()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Select(cp => cp.APIUrl)
                .ToList();
        }

        public long[][] GetCurrencySourceMappings()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null)
                .Where(cp => cp.IsEnabled)
                .Include(cp => cp.Source)
                .Select(cp => new long[] {cp.Id, cp.Source.Id})
                .ToArray();
        }
    }
}