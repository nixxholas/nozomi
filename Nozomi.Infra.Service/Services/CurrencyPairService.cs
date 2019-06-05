using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        [Obsolete]
        public bool Create(CreateCurrencyPair createCurrencyPair, long userId)
        {
            if (createCurrencyPair == null || !createCurrencyPair.IsValid()) return false;

            var currencyPair = new CurrencyPair()
            {
                CurrencyPairType = createCurrencyPair.CurrencyPairType,
                APIUrl = createCurrencyPair.ApiUrl,
                DefaultComponent = createCurrencyPair.DefaultComponent,
                SourceId = createCurrencyPair.CurrencySourceId,
                Requests = createCurrencyPair.CurrencyPairRequests
                    .Select(cpr => new Request()
                    {
                        RequestType = cpr.RequestType,
                        DataPath = cpr.DataPath,
                        Delay = cpr.Delay,
                        RequestComponents = cpr.RequestComponents
                            .Select(rc => new RequestComponent()
                            {
                                ComponentType = rc.ComponentType,
                                QueryComponent = rc.QueryComponent
                            })
                            .ToList(),
                        RequestProperties = cpr.RequestProperties
                            .Select(rp => new RequestProperty()
                            {
                                RequestPropertyType = rp.RequestPropertyType,
                                Key = rp.Key,
                                Value = rp.Value
                            })
                            .ToList()
                    })
                    .ToList()
            };
            
            if (userId > 0)
            {
                _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
                _unitOfWork.Commit(userId);

                return true;
            }
            else
            {
                _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
                _unitOfWork.Commit();

                return true;
            }
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