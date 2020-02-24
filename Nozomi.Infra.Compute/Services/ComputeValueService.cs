using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Services
{
    public class ComputeValueService : BaseService<ComputeValueService, NozomiComputeDbContext>, IComputeValueService
    {
        public ComputeValueService(ILogger<ComputeValueService> logger, IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ComputeValueService(IHttpContextAccessor contextAccessor, ILogger<ComputeValueService> logger, 
            IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void Add(ComputeValue computeValue)
        {
            throw new System.NotImplementedException();
        }

        public void Push(ComputeValue computeValue)
        {
            throw new System.NotImplementedException();
        }
    }
}