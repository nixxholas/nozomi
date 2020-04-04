﻿using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.RequestComponent;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class ComponentService : BaseService<ComponentService, NozomiDbContext>,
        IComponentService
    {
        private readonly IRequestEvent _requestEvent;
        private readonly IComponentHistoricItemService _componentHistoricItemService;

        public ComponentService(ILogger<ComponentService> logger, IRequestEvent requestEvent,
            IComponentHistoricItemService componentHistoricItemService,
            NozomiDbContext context) : base(logger, context)
        {
            _requestEvent = requestEvent;
            _componentHistoricItemService = componentHistoricItemService;
        }

        public void Create(CreateComponentInputModel vm, string userId = null)
        {
            if (vm.IsValid() && !_requestEvent.Exists(vm.Type, vm.RequestId))
            {
                var requestId = _requestEvent.GetId(vm.RequestId);
                if (requestId <= 0)
                    throw new ArgumentException("Request not found.");

                var requestComponent = new Component(vm.Type, vm.Identifier,
                    vm.QueryComponent, vm.AnomalyIgnorance, vm.IsDenominated, vm.StoreHistoricals, requestId);

                _context.Components.Add(requestComponent);
                _context.SaveChanges(userId);
                
                _logger.LogInformation($"{_serviceName} Create (CreateComponentInputModel): request " +
                                       $"component {requestComponent.Guid} is created by {userId}");

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

                _context.Components.Add(newRequestComponent);
                _context.SaveChanges(userId);

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
                var entity = _context.Components.Include(rc => rc.Request)
                    .AsTracking()
                    .SingleOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled
                                                                && rc.ModifiedAt.AddMilliseconds(rc.Request.Delay)
                                                                >= DateTime.UtcNow
                                                                && rc.Id.Equals(id));

                if (entity != null)
                {
                    entity.ModifiedAt = DateTime.UtcNow;
                    
                    _logger.LogInformation($"{_serviceName} Checked (long): Component {id} successfully " +
                                           "marked as checked.");

                    _context.SaveChanges(userId);
                    return true;
                }
                
                _logger.LogWarning($"{_serviceName} Checked (long): Invalid component {id}, can't be found.");
            }
            else
            {
                _logger.LogWarning($"{_serviceName} Checked (long): Invalid component id.");
            }
            
            return false;
        }

        public NozomiResult<string> UpdatePairValue(long id, decimal val)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"[{_serviceName}]: Invalid component datum id:{id}. Null payload");
                return new NozomiResult<string>(NozomiResultType.Failed, $"[{_serviceName}]: " +
                                                                         $"Invalid component datum id:{id}. Null payload");
            }

            var lastValue = _context.ComponentHistoricItems
                .Include(c => c.Component)
                .ThenInclude(c => c.Request)
                .OrderByDescending(e => e.HistoricDateTime)
                .FirstOrDefault(e => e.DeletedAt == null && e.IsEnabled
                                                         && e.RequestComponentId.Equals(id));

            // Let's get the placeholder new value up first.
            var newValueItem = new ComponentHistoricItem
            {
                HistoricDateTime = DateTime.UtcNow,
                Value = val.ToString(CultureInfo.InvariantCulture),
                RequestComponentId = id
            };
            
            _logger.LogInformation($"{_serviceName} UpdatePairValue (decimal): Attempting to update " +
                                   $"component {id} with a value of {val}");

            // SCENARIO 1: WHEN THIS COMPONENT IS FRESH
            // If there ain't a latest value
            if (lastValue == null)
            {
                _context.ComponentHistoricItems.Add(newValueItem); // Add
                _context.SaveChanges(); // Save

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
                    _context.ComponentHistoricItems.Add(newValueItem); // Add
                    _context.SaveChanges(); // Save
            
                    _logger.LogInformation($"{_serviceName} UpdatePairValue (decimal): Component {id} is " +
                                           $"successfully updated HISTORICALLY with a value of {val}");

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Component successfully updated!");
                }
                else // No? We need to hard delete the existing one if we can proceed with the update
                {
                    // Add first then delete.
                    _context.ComponentHistoricItems.Add(newValueItem); // Add
                    _componentHistoricItemService.Remove(lastValue.Guid, null, true);
                    _context.SaveChanges(); // Save
            
                    _logger.LogInformation($"{_serviceName} UpdatePairValue (decimal): Component {id} is " +
                                           $"successfully updated with a value of {val}");

                    return new NozomiResult<string>
                        (NozomiResultType.Success, "Component successfully updated!");
                }
            }
            // SCENARIO 3: WHEN THIS COMPONENT WAS ALREADY UPDATED BY ANOTHER INSTANCE? 
            else
            {
                _logger.LogWarning($"{_serviceName} UpdatePairValue (decimal): Component {id} value is " +
                                   "the same.");

                return new NozomiResult<string>
                    (NozomiResultType.Success, "Value is the same!");
            }
        }

        [Obsolete]
        public NozomiResult<string> UpdatePairValue(long id, string val)
        {
            try
            {
                var lastCompVal = _context
                    .Components.AsTracking()
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
                            _logger.LogInformation($"[{_serviceName}]: UpdatePairValue successfully saved " +
                                                   $"the current RCD value.");
                        }
                        // Old data failed to save. Something along the lines between the old data being new
                        // or the old data having a similarity with the latest rcdhi.
                        else
                        {
                            _logger.LogWarning($"[{_serviceName}]: UpdatePairValue failed to save " +
                                               $"the current RCD value.");
                        }
                    }

                    // lastCompVal.Value = val.ToString(CultureInfo.InvariantCulture);

                    _context.Components.Update(lastCompVal);
                    _context.SaveChanges();

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
                _logger.LogCritical($"{_serviceName} " + ex);

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

                var rcToUpd = _context.Components
                    .SingleOrDefault(rc => rc.Id.Equals(updateRequestComponent.Id) 
                                           && rc.DeletedAt == null && rc.IsEnabled);

                if (rcToUpd != null)
                {
                    rcToUpd.QueryComponent = updateRequestComponent.QueryComponent;
                    rcToUpd.Identifier = updateRequestComponent.Identifier;
                    rcToUpd.ComponentTypeId = updateRequestComponent.ComponentType;
                    rcToUpd.IsDenominated = updateRequestComponent.IsDenominated;
                    rcToUpd.AnomalyIgnorance = updateRequestComponent.AnomalyIgnorance;

                    _context.Components.Update(rcToUpd);
                    _context.SaveChanges(userId);

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

            var cpcToDel = _context.Components
                .SingleOrDefault(rc => rc.Id.Equals(id) && rc.DeletedAt == null && rc.IsEnabled);

            if (cpcToDel != null)
            {
                if (hardDelete)
                {
                    _context.Components.Remove(cpcToDel);
                }
                else
                {
                    cpcToDel.DeletedAt = DateTime.UtcNow;
                    cpcToDel.DeletedById = userId;

                    _context.Components.Update(cpcToDel);
                }

                _context.SaveChanges(userId);

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