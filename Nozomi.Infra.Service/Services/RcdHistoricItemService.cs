using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
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

        public bool Push(RequestComponent rc)
        {
            // Make sure nothing is newer
            if (!_unitOfWork.GetRepository<RcdHistoricItem>()
                // include any rcdhi if it is NEWER than the current rcd
                    .Get(rcdhi => rcdhi.RequestComponentId.Equals(rc.Id)
                                  && rcdhi.HistoricDateTime > rc.ModifiedAt)
                .Any())
            {
                var lastHistoric = _unitOfWork.GetRepository<RcdHistoricItem>()
                    .Get(rcdhi => rcdhi.RequestComponentId.Equals(rc.Id))
                    .OrderByDescending(rcdhi => rcdhi.HistoricDateTime)
                    .FirstOrDefault();

                if (lastHistoric == null || !lastHistoric.Value.Equals(rc.Value))
                {
                // Push it
                _unitOfWork.GetRepository<RcdHistoricItem>().Add(new RcdHistoricItem
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