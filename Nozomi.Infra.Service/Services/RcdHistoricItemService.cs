using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
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
            // If rcd is the latest
            if (!_unitOfWork.GetRepository<RcdHistoricItem>()
                .Get(rcdhi => rcdhi.ModifiedAt >= rcd.ModifiedAt)
                .OrderBy(rcdhi => rcdhi.ModifiedAt)
                .Any())
            {
                // Push it
                _unitOfWork.GetRepository<RcdHistoricItem>().Add(new RcdHistoricItem
                {
                    RequestComponentDatumId = rcd.Id,
                    Value = rcd.Value,
                    CreatedAt = rcd.ModifiedAt
                });
                _unitOfWork.Commit(); // done
                
                return true;
            }

            // Failed
            return false;
        }
    }
}