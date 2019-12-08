using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class RcdHistoricItemService : BaseService<RcdHistoricItemService, NozomiDbContext>, IRcdHistoricItemService
    {
        public RcdHistoricItemService(ILogger<RcdHistoricItemService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public bool Push(Component rc)
        {
            if (!string.IsNullOrEmpty(rc.Value))
            {
                var lastHistoric = _unitOfWork.GetRepository<ComponentHistoricItem>()
                    .GetQueryable()
                    .AsNoTracking()
                    .OrderByDescending(rcdhi => rcdhi.CreatedAt)
                    .FirstOrDefault(rcdhi => rcdhi.RequestComponentId.Equals(rc.Id));

                if (lastHistoric != null)
                {
                    var lastHistoricVal = decimal.Parse(lastHistoric.Value);
                    var existingVal = decimal.Parse(rc.Value);

                    if (lastHistoricVal != existingVal)
                    {
                        // Push it
                        _unitOfWork.GetRepository<ComponentHistoricItem>().Add(new ComponentHistoricItem
                        {
                            RequestComponentId = rc.Id,
                            Value = rc.Value,
                            HistoricDateTime = rc.ModifiedAt
                        });
                        _unitOfWork.Commit(); // done
                    }

                    return true;
                }
                else
                {
                    // Push it
                    _unitOfWork.GetRepository<ComponentHistoricItem>().Add(new ComponentHistoricItem
                    {
                        RequestComponentId = rc.Id,
                        Value = rc.Value,
                        HistoricDateTime = rc.ModifiedAt
                    });
                    _unitOfWork.Commit(); // done
                }
                
                // Return true anyway since the value is a dupe
                return true;
            }

            // Failed
            return false;
        }
    }
}