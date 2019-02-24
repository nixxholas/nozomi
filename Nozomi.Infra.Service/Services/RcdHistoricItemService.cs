using System.Linq;
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
            // If rcd is the latest
            if (!_unitOfWork.GetRepository<RcdHistoricItem>()
                .Get(rcdhi => rcdhi.ModifiedAt >= rcd.ModifiedAt)
                .OrderBy(rcdhi => rcdhi.ModifiedAt)
                .Any())
            {
                var latestRcdhi = _unitOfWork.GetRepository<RcdHistoricItem>()
                    .GetQueryable()
                    .OrderByDescending(rcdhi => rcdhi.ModifiedAt).FirstOrDefault();
                
                // Let's make it more efficient by checking if the price has changed
                if (latestRcdhi != null && latestRcdhi.Value.Equals(rcd.Value))
                {
                    // Move on bro
                    return true;
                }
                
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