using Microsoft.Extensions.Logging;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Services
{
    public class AnalysedComponentService : BaseService<AnalysedComponentService, NozomiDbContext>, IAnalysedComponentService
    {
        public AnalysedComponentService(ILogger<AnalysedComponentService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }
    }
}