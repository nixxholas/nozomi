using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Extensions;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class ComponentService : BaseService<ComponentService, NozomiDbContext>,
        IComponentService
    {
        private const string serviceName = "[CurrencyPairComponentService]";

        private readonly IRequestEvent _requestEvent;
        private readonly IComponentHistoricItemService _componentHistoricItemService;

        public ComponentService(ILogger<ComponentService> logger, IRequestEvent requestEvent,
            IComponentHistoricItemService componentHistoricItemService,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _requestEvent = requestEvent;
            _componentHistoricItemService = componentHistoricItemService;
        }

        public void Create(CreateComponentViewModel vm, string userId = null)
        {
            if (vm.IsValid() && !_requestEvent.Exists(vm.Type, vm.RequestId))
            {
                var requestId = _requestEvent.GetId(vm.RequestId);
                if (requestId <= 0)
                    throw new ArgumentException("Request not found.");

                var requestComponent = new Component(vm.Type, vm.Identifier,
                    vm.QueryComponent, vm.AnomalyIgnorance, vm.IsDenominated, vm.StoreHistoricals, requestId);

                _unitOfWork.GetRepository<Component>().Add(requestComponent);
                _unitOfWork.Commit(userId);

                return; // Done
            }

            throw new InvalidOperationException("Invalid payload, fill up the model properly.");
        }

        public NozomiResult<string> Create(CreateRequestComponent createRequestComponent, string userId = null)
        {
            try
            {
                if (createRequestComponent == null || !string.IsNullOrWhiteSpace(userId))
                    return new NozomiResult<string>
                        (NozomiResultType.Failed, "Invalid payload or userId.");

                var newRequestComponent = new Component()
                {
                    RequestId = createRequestComponent.RequestId,
                    ComponentTypeId = createRequestComponent.ComponentType,
                    Identifier = createRequestComponent.Identifier,
                    QueryComponent = createRequestComponent.QueryComponent,
                    IsDenominated = createRequestComponent.IsDenominated,
                    AnomalyIgnorance = createRequestComponent.AnomalyIgnorance
                };

                _unitOfWork.GetRepository<Component>().Add(newRequestComponent);
                _unitOfWork.Commit(userId);

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Currency Pair Component successfully created!", newRequestComponent);
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Checked(long id, string userId = null)
        {
            if (id > 0)
            {
                var entity = _unitOfWork.GetRepository<Component>()
                    .GetQueryable()
                    .Include(rc => rc.Request)
                    .AsTracking()
                    .SingleOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled
                                                                && rc.ModifiedAt.AddMilliseconds(rc.Request.Delay)
                                                                >= DateTime.UtcNow
                                                                && rc.Id.Equals(id));

                if (entity != null)
                {
                    entity.ModifiedAt = DateTime.UtcNow;

                    _unitOfWork.Commit(userId);
                    return true;
                }
            }

            return false;
        }

        public NozomiResult<string> UpdatePairValue(long id, decimal val)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"[{serviceName}]: Invalid component datum id:{id}. Null payload");
                return new NozomiResult<string>(NozomiResultType.Failed, $"[{serviceName}]: " +
                                                                         $"Invalid component datum id:{id}. Null payload");
            }

            var lastValue = _unitOfWork.GetRepository<ComponentHistoricItem>()
                .GetQueryable()
                .Include(c => c.Component)
                .ThenInclude(c => c.Request)
                .OrderByDescending(e => e.HistoricDateTime)
                .FirstOrDefault(e => e.DeletedAt == null && e.IsEnabled
                                                         && e.RequestComponentId.Equals(id));

            // Let's get the placeholder new value up first.
            var newValueItem = new ComponentHistoricItem
            {
                HistoricDateTime = DateTime.UtcNow,
                Value = val.ToString(),
                RequestComponentId = id
            };

            // SCENARIO 1: WHEN THIS COMPONENT IS FRESH
            // If there ain't a latest value
            if (lastValue == null)
            {
                _unitOfWork.GetRepository<ComponentHistoricItem>().Add(newValueItem); // Add
                _unitOfWork.Commit(); // Save

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Component successfully updated!");
            }
            // SCENARIO 2: WHEN THIS COMPONENT IS PENDING FOR AN UPDATE (RIPE)
            // Ensure that the last time it was added wasn't recently.
            else if (lastValue.Component != null && lastValue.Component.Request != null
                                                 && lastValue.CreatedAt
                                                     .AddMilliseconds(lastValue.Component.Request.Delay)
                                                 <= DateTime.UtcNow)
            {
                // Since it's not and it requires an update, let's perform a simply check.
                if (lastValue.Component.StoreHistoricals) // Do we want to stash historicals?
                {
                    // Yes we do let's proceed to add it and move along
                    _unitOfWork.GetRepository<ComponentHistoricItem>().Add(newValueItem); // Add
                    _unitOfWork.Commit(); // Save

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Component successfully updated!");
                }
                else // No? We need to hard delete the existing one if we can proceed with the update
                {
                    // Add first then delete.
                    _unitOfWork.GetRepository<ComponentHistoricItem>().Add(newValueItem); // Add
                    _componentHistoricItemService.Remove(lastValue, null, true);
                    _unitOfWork.Commit(); // Save

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Component successfully updated!");
                }
            }
            // SCENARIO 3: WHEN THIS COMPONENT WAS ALREADY UPDATED BY ANOTHER INSTANCE? 
            else
            {
                _logger.LogWarning($"{serviceName} UpdatePairValue (decimal): datum id:{id}. " +
                                   "Value is the same.");

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Value is the same!");
            }
        }

        public NozomiResult<string> UpdatePairValue(long id, string val)
        {
            try
            {
                var lastCompVal = _unitOfWork
                    .GetRepository<Component>()
                    .GetQueryable()
                    .AsTracking()
                    .Include(rc => rc.Request)
                    .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                      && rc.ModifiedAt.AddMilliseconds(rc.Request.Delay) <=
                                                      DateTime.UtcNow)
                    .SingleOrDefault(rc => rc.Id.Equals(id));

                if (lastCompVal != null)
                {
                    if (lastCompVal.StoreHistoricals
                        // TODO: Ensure proper checks
                        // && !string.IsNullOrEmpty(lastCompVal.Value)
                    )
                    {
                        // Save old data first
                        if (_componentHistoricItemService.Push(lastCompVal))
                        {
                            _logger.LogInformation($"[{serviceName}]: UpdatePairValue successfully saved " +
                                                   $"the current RCD value.");
                        }
                        // Old data failed to save. Something along the lines between the old data being new
                        // or the old data having a similarity with the latest rcdhi.
                        else
                        {
                            _logger.LogWarning($"[{serviceName}]: UpdatePairValue failed to save " +
                                               $"the current RCD value.");
                        }
                    }

                    // lastCompVal.Value = val.ToString(CultureInfo.InvariantCulture);

                    _unitOfWork.GetRepository<Component>().Update(lastCompVal);
                    _unitOfWork.Commit();

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Currency Pair Component successfully updated!");
                }

                return new NozomiResult<string>
                (NozomiResultType.Failed,
                    $"Invalid component datum id:{id}, val:{val}. Please make sure that the " +
                    "Component is properly instantiated.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"{serviceName} " + ex);

                return new NozomiResult<string>
                (NozomiResultType.Failed,
                    $"Invalid component datum id:{id}, val:{val}. Please make sure that the " +
                    "Component is properly instantiated.");
            }
        }

        public NozomiResult<string> Update(UpdateRequestComponent updateRequestComponent, string userId = null)
        {
            try
            {
                if (updateRequestComponent == null || !string.IsNullOrWhiteSpace(userId))
                    return new NozomiResult<string>
                        (NozomiResultType.Failed, "Invalid payload or userId.");

                var rcToUpd = _unitOfWork.GetRepository<Component>()
                    .Get(rc => rc.Id.Equals(updateRequestComponent.Id) && rc.DeletedAt == null && rc.IsEnabled)
                    .SingleOrDefault();

                if (rcToUpd != null)
                {
                    rcToUpd.QueryComponent = updateRequestComponent.QueryComponent;
                    rcToUpd.Identifier = updateRequestComponent.Identifier;
                    rcToUpd.ComponentTypeId = updateRequestComponent.ComponentType;
                    rcToUpd.IsDenominated = updateRequestComponent.IsDenominated;
                    rcToUpd.AnomalyIgnorance = updateRequestComponent.AnomalyIgnorance;

                    _unitOfWork.GetRepository<Component>().Update(rcToUpd);
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

        public NozomiResult<string> Delete(long id, string userId = null, bool hardDelete = false)
        {
            if (id < 1 || string.IsNullOrWhiteSpace(userId))
                return new NozomiResult<string>
                    (NozomiResultType.Failed, "Invalid payload or userId.");

            var cpcToDel = _unitOfWork.GetRepository<Component>()
                .Get(rc => rc.Id.Equals(id) && rc.DeletedAt == null && rc.IsEnabled)
                .SingleOrDefault();

            if (cpcToDel != null)
            {
                if (hardDelete)
                {
                    _unitOfWork.GetRepository<Component>().Delete(cpcToDel);
                }
                else
                {
                    cpcToDel.DeletedAt = DateTime.UtcNow;
                    cpcToDel.DeletedById = userId;

                    _unitOfWork.GetRepository<Component>().Update(cpcToDel);
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