using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class ComponentHistoricItemService : BaseService<ComponentHistoricItemService, NozomiDbContext>, 
        IComponentHistoricItemService
    {
        public ComponentHistoricItemService(ILogger<ComponentHistoricItemService> logger, 
        IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
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
            //             _unitOfWork.GetRepository<ComponentHistoricItem>().Add(new ComponentHistoricItem
            //             {
            //                 RequestComponentId = rc.Id,
            //                 Value = rc.Value,
            //                 HistoricDateTime = rc.ModifiedAt
            //             });
            //             _unitOfWork.Commit(); // done
            //         }
            //
            //         return true;
            //     }
            //     else
            //     {
            //         // Push it
            //         _unitOfWork.GetRepository<ComponentHistoricItem>().Add(new ComponentHistoricItem
            //         {
            //             RequestComponentId = rc.Id,
            //             Value = rc.Value,
            //             HistoricDateTime = rc.ModifiedAt
            //         });
            //         _unitOfWork.Commit(); // done
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
                var itemToDel = _unitOfWork.GetRepository<ComponentHistoricItem>()
                    .GetQueryable()
                    .AsNoTracking()
                    .SingleOrDefault(e => e.Guid.Equals(parsedGuid));

                if (itemToDel != null) // Since it exists, let's delete it
                {
                    if (hardDelete)
                    {
                        _unitOfWork.GetRepository<ComponentHistoricItem>().Delete(itemToDel); // Delete
                        _unitOfWork.Commit(userId); // Save
                        return;
                    }
                    else
                    {
                        itemToDel.DeletedAt = DateTime.UtcNow;
                        itemToDel.DeletedById = userId;
                        _unitOfWork.GetRepository<ComponentHistoricItem>().Update(itemToDel); // Save
                        _unitOfWork.Commit(userId); // Commit
                        return;
                    }
                }
            }

            throw new NullReferenceException("Invalid id for removal.");
        }

        public void Remove(ComponentHistoricItem componentHistoricItem, string userId = null, bool hardDelete = false)
        {
            if (componentHistoricItem != null)
            {
                if (hardDelete)
                {
                    _unitOfWork.GetRepository<ComponentHistoricItem>().Delete(componentHistoricItem); // Delete
                    _unitOfWork.Commit(userId); // Save
                    return;
                }
                else
                {
                    componentHistoricItem.DeletedAt = DateTime.UtcNow;
                    componentHistoricItem.DeletedById = userId;
                    _unitOfWork.GetRepository<ComponentHistoricItem>().Update(componentHistoricItem); // Save
                    _unitOfWork.Commit(userId); // Commit
                    return;
                }
            }

            throw new NullReferenceException("Invalid historic item for removal.");
        }
    }
}