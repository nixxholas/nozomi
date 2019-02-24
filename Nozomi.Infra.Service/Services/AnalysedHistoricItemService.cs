using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class AnalysedHistoricItemService : BaseService<AnalysedHistoricItemService, NozomiDbContext>, 
        IAnalysedHistoricItemService
    {
        public AnalysedHistoricItemService(ILogger<AnalysedHistoricItemService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public long Create(AnalysedHistoricItem item, long userId = 0)
        {
            if (item != null)
            {
                _unitOfWork.GetRepository<AnalysedHistoricItem>().Add(item);
                _unitOfWork.Commit(userId);
            }

            return -1; // Failed..
        }
    }
}