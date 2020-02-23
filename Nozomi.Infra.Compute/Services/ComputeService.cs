using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Services
{
    public class ComputeService : BaseService<ComputeService, NozomiComputeDbContext>, IComputeService
    {
        public ComputeService(ILogger<ComputeService> logger, IUnitOfWork<NozomiComputeDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public ComputeService(IHttpContextAccessor contextAccessor, ILogger<ComputeService> logger, IUnitOfWork<NozomiComputeDbContext> unitOfWork) : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void Modified(Guid guid, string userId = null)
        {
            throw new NotImplementedException();
        }

        public void Modified(string guid, string userId = null)
        {
            throw new NotImplementedException();
        }
    }
}