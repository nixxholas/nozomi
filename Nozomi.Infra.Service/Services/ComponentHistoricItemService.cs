using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class ComponentHistoricItemService : BaseService<ComponentHistoricItemService, NozomiDbContext>, 
        IComponentHistoricItemService
    {
        public ComponentHistoricItemService(ILogger<ComponentHistoricItemService> logger, 
        NozomiDbContext context) 
            : base(logger, context)
        {
        }

        public bool Push(Component rc)
        {
            // TODO:
            // if (!string.IsNullOrEmpty(rc.Value))
            // {
            //     var lastHistoric = _unitOfWork.GetRepository<ComponentHistoricItem>()
            //         .GetQueryable()
            //         .AsNoTracking()
            //         .OrderByDescending(rcdhi => rcdhi.CreatedAt)
            //         .FirstOrDefault(rcdhi => rcdhi.RequestComponentId.Equals(rc.Id));
            //
            //     if (lastHistoric != null)
            //     {
            //         var lastHistoricVal = decimal.Parse(lastHistoric.Value);
            //         var existingVal = decimal.Parse(rc.Value);
            //
            //         if (lastHistoricVal != existingVal)
            //         {
            //             // Push it
            //             _unitOfWork.RcdHistoricItems.Add(new ComponentHistoricItem
            //             {
            //                 RequestComponentId = rc.Id,
            //                 Value = rc.Value,
            //                 HistoricDateTime = rc.ModifiedAt
            //             });
            //             _unitOfWork.SaveChanges(); // done
            //         }
            //
            //         return true;
            //     }
            //     else
            //     {
            //         // Push it
            //         _unitOfWork.RcdHistoricItems.Add(new ComponentHistoricItem
            //         {
            //             RequestComponentId = rc.Id,
            //             Value = rc.Value,
            //             HistoricDateTime = rc.ModifiedAt
            //         });
            //         _unitOfWork.SaveChanges(); // done
            //     }
            //     
            //     // Return true anyway since the value is a dupe
            //     return true;
            // }

            // Failed
            return false;
        }

        public void Remove(string guid, string userId = null, bool hardDelete = false)
        {
            if (Guid.TryParse(guid, out var parsedGuid))
            {
                var itemToDel = _context.ComponentHistoricItems.AsNoTracking()
                    .SingleOrDefault(e => e.Guid.Equals(parsedGuid));

                if (itemToDel != null) // Since it exists, let's delete it
                {
                    if (hardDelete)
                    {
                        _context.ComponentHistoricItems.Remove(itemToDel); // Delete
                        _context.SaveChanges(userId); // Save
                        return;
                    }
                    else
                    {
                        itemToDel.DeletedAt = DateTime.UtcNow;
                        itemToDel.DeletedById = userId;
                        _context.ComponentHistoricItems.Update(itemToDel); // Save
                        _context.SaveChanges(userId); // Commit
                        return;
                    }
                }
            }

            throw new NullReferenceException("Invalid id for removal.");
        }

        public void Remove(Guid guid, string userId = null, bool hardDelete = false)
        {
            var itemToDel = _context.ComponentHistoricItems.AsTracking()
                .SingleOrDefault(e => e.Guid.Equals(guid));
            
            _logger.LogInformation($"{_serviceName} Remove (Guid): Deleting {guid}");

            if (itemToDel != null) // Since it exists, let's delete it
            {
                if (hardDelete)
                {
                    _context.ComponentHistoricItems.Remove(itemToDel); // Delete
                    _context.SaveChanges(userId); // Save
                    _logger.LogInformation($"{_serviceName} Remove (Guid): Hard deleted {guid}");
                    return;
                }
                else
                {
                    itemToDel.DeletedAt = DateTime.UtcNow;
                    itemToDel.DeletedById = userId;
                    _context.ComponentHistoricItems.Update(itemToDel); // Save
                    _context.SaveChanges(userId); // Commit
                    _logger.LogInformation($"{_serviceName} Remove (Guid): Soft deleted {guid}");
                    return;
                }
            }
            
            _logger.LogError($"{_serviceName} Remove (Guid): There was a problem deleting {guid}.");

            throw new NullReferenceException($"{_serviceName } Remove (Guid): Invalid Guid for removal.");
        }

        public void Remove(ComponentHistoricItem componentHistoricItem, string userId = null, bool hardDelete = false)
        {
            if (componentHistoricItem != null)
            {
                if (hardDelete)
                {
                    _context.ComponentHistoricItems.Remove(componentHistoricItem); // Delete
                    _context.SaveChanges(userId); // Save
                    return;
                }
                else
                {
                    componentHistoricItem.DeletedAt = DateTime.UtcNow;
                    componentHistoricItem.DeletedById = userId;
                    _context.ComponentHistoricItems.Update(componentHistoricItem); // Save
                    _context.SaveChanges(userId); // Commit
                    return;
                }
            }

            throw new NullReferenceException("Invalid historic item for removal.");
        }
    }
}