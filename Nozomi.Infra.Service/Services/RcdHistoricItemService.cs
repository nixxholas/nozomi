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

        public bool Push(RequestComponentDatum rcd)
        {
            // Make sure nothing is newer
            if (!_unitOfWork.GetRepository<RcdHistoricItem>()
                // include any rcdhi if it is NEWER than the current rcd
                    .Get(rcdhi => rcdhi.RequestComponentDatumId.Equals(rcd.Id)
                                  && rcdhi.HistoricDateTime > rcd.ModifiedAt)
                .Any())
            {
                // Push it
                _unitOfWork.GetRepository<RcdHistoricItem>().Add(new RcdHistoricItem
                {
                    RequestComponentDatumId = rcd.Id,
                    Value = rcd.Value,
                    HistoricDateTime = rcd.ModifiedAt
                });
                _unitOfWork.Commit(); // done
                
                return true;
            }

            // Failed
            return false;
        }
    }
}