using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.NozomiRedisModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;

namespace CounterCore.Service.Services
{
    public class CurrencyPairComponentService : BaseService<CurrencyPairComponentService, NozomiDbContext>, ICurrencyPairComponentService
    {
        private readonly IDistributedCache _distributedCache;
        
        public CurrencyPairComponentService(ILogger<CurrencyPairComponentService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, IDistributedCache distributedCache) : base(logger, unitOfWork)
        {
            _distributedCache = distributedCache;
        }

        public bool UpdatePairValue(long id, decimal val)
        {
            var pairToUpd = _unitOfWork
                                     .GetRepository<RequestComponent>()
                                     .GetQueryable()
                                     .Where(cp => cp.Id.Equals(id))
                                     .Include(cpc => cpc.RequestComponentDatum)
                                     .SingleOrDefault(cp => cp.DeletedAt == null && cp.IsEnabled);

            // Anomaly Detection
            if (pairToUpd?.RequestComponentDatum != null)
            {
                // Redis, Toss the old datum there.
                _distributedCache.SetString(
                    JsonConvert.SerializeObject(new RCCachedDatumKey()
                    {
                        Id = pairToUpd.RequestComponentDatumId, 
                        DatumTime = pairToUpd.RequestComponentDatum.ModifiedAt
                    }),
                    pairToUpd.RequestComponentDatum.Value);
                
                pairToUpd.RequestComponentDatum.Value = val.ToString(CultureInfo.InvariantCulture);
                
                _unitOfWork.GetRepository<RequestComponentDatum>().Update(pairToUpd.RequestComponentDatum);
                _unitOfWork.Commit();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
