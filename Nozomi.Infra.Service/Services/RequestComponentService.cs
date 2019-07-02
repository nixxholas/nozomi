using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPairComponent;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class RequestComponentService : BaseService<RequestComponentService, NozomiDbContext>,
        IRequestComponentService
    {
        private const string serviceName = "[CurrencyPairComponentService]";

        private IRcdHistoricItemService _rcdHistoricItemService { get; set; }

        public RequestComponentService(ILogger<RequestComponentService> logger,
            IRcdHistoricItemService rcdHistoricItemService,
            IUnitOfWork<NozomiDbContext> unitOfWork, IDistributedCache distributedCache) : base(logger, unitOfWork)
        {
            _rcdHistoricItemService = rcdHistoricItemService;
        }

        public NozomiResult<string> Create(CreateRequestComponent createRequestComponent, long userId = 0)
        {
            try
            {
                if (createRequestComponent == null || userId < 0)
                    return new NozomiResult<string>
                        (NozomiResultType.Failed, "Invalid payload or userId.");

                var newRequestComponent = new RequestComponent()
                {
                    RequestId = createRequestComponent.RequestId,
                    ComponentType = createRequestComponent.ComponentType,
                    Identifier = createRequestComponent.Identifier,
                    QueryComponent = createRequestComponent.QueryComponent,
                    IsDenominated = createRequestComponent.IsDenominated,
                    AnomalyIgnorance = createRequestComponent.AnomalyIgnorance
                };

                _unitOfWork.GetRepository<RequestComponent>().Add(newRequestComponent);
                _unitOfWork.Commit(userId);

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Currency Pair Component successfully created!", newRequestComponent);
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> UpdatePairValue(long id, decimal val)
        {
            try
            {
                var lastCompVal = _unitOfWork
                    .GetRepository<RequestComponent>()
                    .GetQueryable()
                    .AsTracking()
                    .Where(cp => cp.Id.Equals(id))
                    .SingleOrDefault(cp => cp.DeletedAt == null && cp.IsEnabled);

                // Anomaly Detection
                // Let's make it more efficient by checking if the price has changed
                if (lastCompVal != null && !lastCompVal.HasAbnormalValue(val))
                {
                    if (!string.IsNullOrEmpty(lastCompVal.Value))
                    {
                        // Save old data first
                        if (_rcdHistoricItemService.Push(lastCompVal))
                        {
                        }
                        // Old data failed to save. Something along the lines between the old data being new
                        // or the old data having a similarity with the latest rcdhi.
                        else
                        {
                            _logger.LogInformation($"[{serviceName}]: UpdatePairValue failed to save " +
                                                   $"the current RCD value.");
                        }
                    }

                    lastCompVal.Value = val.ToString(CultureInfo.InvariantCulture);

                    _unitOfWork.GetRepository<RequestComponent>().Update(lastCompVal);
                    _unitOfWork.Commit();

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Currency Pair Component successfully updated!");
                }
                else if (lastCompVal == null)
                {
                    return new NozomiResult<string>
                    (NozomiResultType.Failed,
                        $"Invalid component datum id:{id}, val:{val}. Null payload!!!");
                }
                else
                {
                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Value is the same!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{serviceName} " + ex);

                return new NozomiResult<string>
                (NozomiResultType.Failed,
                    $"Invalid component datum id:{id}, val:{val}. Please make sure that the " +
                    "RequestComponent is properly instantiated.");
            }
        }

        public NozomiResult<string> UpdatePairValue(long id, string val)
        {
            try
            {
                var lastCompVal = _unitOfWork
                    .GetRepository<RequestComponent>()
                    .GetQueryable()
                    .AsTracking()
                    .Where(cp => cp.Id.Equals(id))
                    .SingleOrDefault(cp => cp.DeletedAt == null && cp.IsEnabled);

                if (lastCompVal != null)
                {
                    if (!string.IsNullOrEmpty(lastCompVal.Value))
                    {
                        // Save old data first
                        if (_rcdHistoricItemService.Push(lastCompVal))
                        {
                        }
                        // Old data failed to save. Something along the lines between the old data being new
                        // or the old data having a similarity with the latest rcdhi.
                        else
                        {
                            _logger.LogInformation($"[{serviceName}]: UpdatePairValue failed to save " +
                                                   $"the current RCD value.");
                        }
                    }

                    lastCompVal.Value = val.ToString(CultureInfo.InvariantCulture);

                    _unitOfWork.GetRepository<RequestComponent>().Update(lastCompVal);
                    _unitOfWork.Commit();

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Currency Pair Component successfully updated!");
                }

                return new NozomiResult<string>
                (NozomiResultType.Failed,
                    $"Invalid component datum id:{id}, val:{val}. Please make sure that the " +
                    "RequestComponent is properly instantiated.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{serviceName} " + ex);

                return new NozomiResult<string>
                (NozomiResultType.Failed,
                    $"Invalid component datum id:{id}, val:{val}. Please make sure that the " +
                    "RequestComponent is properly instantiated.");
            }
        }

        public NozomiResult<string> Update(UpdateRequestComponent updateRequestComponent, long userId = 0)
        {
            try
            {
                if (updateRequestComponent == null || userId < 0)
                    return new NozomiResult<string>
                        (NozomiResultType.Failed, "Invalid payload or userId.");

                var rcToUpd = _unitOfWork.GetRepository<RequestComponent>()
                    .Get(rc => rc.Id.Equals(updateRequestComponent.Id) && rc.DeletedAt == null && rc.IsEnabled)
                    .SingleOrDefault();

                if (rcToUpd != null)
                {
                    rcToUpd.QueryComponent = updateRequestComponent.QueryComponent;
                    rcToUpd.Identifier = updateRequestComponent.Identifier;
                    rcToUpd.ComponentType = updateRequestComponent.ComponentType;
                    rcToUpd.IsDenominated = updateRequestComponent.IsDenominated;
                    rcToUpd.AnomalyIgnorance = updateRequestComponent.AnomalyIgnorance;

                    _unitOfWork.GetRepository<RequestComponent>().Update(rcToUpd);
                    _unitOfWork.Commit(userId);

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Request Component successfully updated!", rcToUpd);
                }

                return new NozomiResult<string>
                (NozomiResultType.Failed, "Invalid Currency Pair Component. " +
                                          "Please make sure it is not deleted or disabled.");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> Delete(long id, long userId = 0, bool hardDelete = false)
        {
            if (id < 1 || userId < 0)
                return new NozomiResult<string>
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