using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
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

        public ICollection<RequestComponent> AllByRequestId(long requestId, bool includeNested = false)
        {
            if (requestId <= 0) return null;
            
            return includeNested ? 
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.RequestId.Equals(requestId) && rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.RequestComponentDatum)
                    .Include(rc => rc.Request)
                    .ToList() :
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.RequestId.Equals(requestId) && rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .ToList();
        }

        public ICollection<RequestComponent> All(bool includeNested = false)
        {
            return includeNested ?
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.RequestComponentDatum)
                    .Include(rc => rc.Request)
                    .ToList() :
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .ToList();
        }

        public bool Create(CreateCurrencyPairComponent obj, long userId = 0)
        {
            if (obj == null || userId < 0) return false;
            
            _unitOfWork.GetRepository<RequestComponent>().Add(new RequestComponent()
            {
                ComponentType = obj.ComponentType,
                QueryComponent = obj.QueryComponent,
                RequestId = obj.RequestId
            });
            _unitOfWork.Commit(userId);

            return true;
        }

        public bool UpdatePairValue(long id, decimal val)
        {
            var pairToUpd = _unitOfWork
                                     .GetRepository<RequestComponent>()
                                     .GetQueryable()
                                     .AsTracking()
                                     .Where(cp => cp.Id.Equals(id))
                                     .Include(cpc => cpc.RequestComponentDatum)
                                     .SingleOrDefault(cp => cp.DeletedAt == null && cp.IsEnabled);

            // Anomaly Detection
            if (pairToUpd?.RequestComponentDatum != null)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                {
                    // Redis, Toss the old datum there.
                    _distributedCache.SetStringAsync(
                        JsonConvert.SerializeObject(new RCCachedDatumKey()
                        {
                            Id = pairToUpd.RequestComponentDatumId, 
                            DatumTime = pairToUpd.RequestComponentDatum.ModifiedAt
                        }),
                        pairToUpd.RequestComponentDatum.Value);
                }
                
                pairToUpd.RequestComponentDatum.Value = val.ToString(CultureInfo.InvariantCulture);
                
                _unitOfWork.GetRepository<RequestComponent>().Update(pairToUpd);
                _unitOfWork.GetRepository<RequestComponentDatum>().Update(pairToUpd.RequestComponentDatum);
                _unitOfWork.Commit();

                return true;
            }
            else if (pairToUpd != null && pairToUpd.RequestComponentDatum == null)
            {
                // Since the RCD is null but not the RC,
                _unitOfWork.GetRepository<RequestComponentDatum>().Add(new RequestComponentDatum()
                {
                    RequestComponentId = pairToUpd.Id,
                    Value = val.ToString(CultureInfo.InvariantCulture)
                });
                _unitOfWork.Commit();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(UpdateCurrencyPairComponent obj, long userId = 0)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id, long userId = 0, bool hardDelete = false)
        {
            throw new NotImplementedException();
        }
    }
}
