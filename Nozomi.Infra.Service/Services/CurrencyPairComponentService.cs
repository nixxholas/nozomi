using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.NozomiRedisModels;
using Nozomi.Data.WebModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairComponentService : BaseService<CurrencyPairComponentService, NozomiDbContext>, ICurrencyPairComponentService
    {
        private IRcdHistoricItemService _rcdHistoricItemService { get; set; }
        
        public CurrencyPairComponentService(ILogger<CurrencyPairComponentService> logger, 
            IRcdHistoricItemService rcdHistoricItemService,
            IUnitOfWork<NozomiDbContext> unitOfWork, IDistributedCache distributedCache) : base(logger, unitOfWork)
        {
            _rcdHistoricItemService = rcdHistoricItemService;
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

        public ICollection<RequestComponent> All(int index = 0, bool includeNested = false)
        {
            return includeNested ?
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Include(rc => rc.RequestComponentDatum)
                    .Include(rc => rc.Request)
                    .Skip(index * 20)
                    .Take(20)
                    .ToList() :
                _unitOfWork.GetRepository<RequestComponent>()
                    .GetQueryable(rc => rc.DeletedAt == null && rc.IsEnabled)
                    .AsNoTracking()
                    .Skip(index * 20)
                    .Take(20)
                    .ToList();
        }

        public NozomiResult<RequestComponent> Get(long id, bool includeNested = false)
        {
            if (includeNested)
                return new NozomiResult<RequestComponent>(_unitOfWork.GetRepository<RequestComponent>().GetQueryable()
                    .Include(rc => rc.Request)
                    .Include(rc => rc.RequestComponentDatum)
                    .SingleOrDefault(rc => rc.Id.Equals(id) && rc.IsEnabled && rc.DeletedAt == null));
            
            return new NozomiResult<RequestComponent>(_unitOfWork.GetRepository<RequestComponent>()
                .Get(rc => rc.Id.Equals(id) && rc.DeletedAt == null && rc.IsEnabled)
                .SingleOrDefault());
        }

        public NozomiResult<string> Create(CreateCurrencyPairComponent obj, long userId = 0)
        {
            if (obj == null || userId < 0) return new NozomiResult<string>
                (NozomiResultType.Failed, "Invalid payload or userId.");
            
            _unitOfWork.GetRepository<RequestComponent>().Add(new RequestComponent()
            {
                ComponentType = obj.ComponentType,
                QueryComponent = obj.QueryComponent,
                RequestId = obj.RequestId
            });
            _unitOfWork.Commit(userId);

            return new NozomiResult<string>
                (NozomiResultType.Success, "Currency Pair Component successfully created!");
        }

        public NozomiResult<string> UpdatePairValue(long id, decimal val)
        {
            var pairToUpd = _unitOfWork
                                     .GetRepository<RequestComponent>()
                                     .GetQueryable()
                                     .AsTracking()
                                     .Where(cp => cp.Id.Equals(id))
                                     .Include(cpc => cpc.RequestComponentDatum)
                                     .SingleOrDefault(cp => cp.DeletedAt == null && cp.IsEnabled);

            // Anomaly Detection
            if (pairToUpd?.RequestComponentDatum != null && 
                pairToUpd.RequestComponentDatum.HasAbnormalValue(val))
            {
                // Save old data first
                _rcdHistoricItemService.Push(pairToUpd.RequestComponentDatum);
                
                pairToUpd.RequestComponentDatum.Value = val.ToString(CultureInfo.InvariantCulture);
                
                _unitOfWork.GetRepository<RequestComponent>().Update(pairToUpd);
                _unitOfWork.GetRepository<RequestComponentDatum>().Update(pairToUpd.RequestComponentDatum);
                _unitOfWork.Commit();

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Currency Pair Component successfully updated!");
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

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Currency Pair Component successfully updated!");
            }
            else
            {
                return new NozomiResult<string>
                    (NozomiResultType.Failed, $"Invalid component datum id:{id}, val:{val}. Please make sure that the " +
                                              "RequestComponent is properly instantiated.");
            }
        }

        public NozomiResult<string> Update(UpdateCurrencyPairComponent obj, long userId = 0)
        {
            if (obj == null || userId < 0) return new NozomiResult<string>
                (NozomiResultType.Failed, "Invalid payload or userId.");

            var cpcToUpd = _unitOfWork.GetRepository<RequestComponent>()
                .Get(rc => rc.Id.Equals(obj.Id) && rc.DeletedAt == null && rc.IsEnabled)
                .SingleOrDefault();

            if (cpcToUpd != null)
            {
                cpcToUpd.ComponentType = obj.ComponentType;
                cpcToUpd.QueryComponent = obj.QueryComponent;
                
                _unitOfWork.GetRepository<RequestComponent>().Update(cpcToUpd);
                _unitOfWork.Commit(userId);
                
                return new NozomiResult<string>
                    (NozomiResultType.Success, "Currency Pair Component successfully updated!");
            }
            else
            {
                return new NozomiResult<string>
                    (NozomiResultType.Failed, "Invalid Currency Pair Component. " +
                                              "Please make sure it is not deleted or disabled.");
            }
        }

        public NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false)
        {
            if (id < 1 || userId < 0) return new NozomiResult<string>
                (NozomiResultType.Failed, "Invalid payload or userId.");

            var cpcToDel = _unitOfWork.GetRepository<RequestComponent>()
                .Get(rc => rc.Id.Equals(id) && rc.DeletedAt == null && rc.IsEnabled)
                .SingleOrDefault();

            if (cpcToDel != null)
            {
                if (hardDelete)
                {
                    _unitOfWork.GetRepository<RequestComponent>().Delete(cpcToDel);
                }
                else
                {
                    cpcToDel.DeletedAt = DateTime.UtcNow;
                    cpcToDel.DeletedBy = userId;

                    _unitOfWork.GetRepository<RequestComponent>().Update(cpcToDel);
                }

                _unitOfWork.Commit(userId);
                
                return new NozomiResult<string>(NozomiResultType.Success,
                    "Currency Pair Component Successfully deleted!");
            }
            else
            {
                return new NozomiResult<string>(NozomiResultType.Failed,
                    "Invalid Currency Pair Component. Please make sure it isn't deleted and" +
                    " is enabled before attempting to delete.");
            }
        }
    }
}
