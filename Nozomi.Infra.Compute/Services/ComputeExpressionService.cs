using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Services
{
    public class ComputeExpressionService : BaseService<ComputeExpressionService, NozomiComputeDbContext>,
        IComputeExpressionService
    {
        public ComputeExpressionService(ILogger<ComputeExpressionService> logger, 
            IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ComputeExpressionService(IHttpContextAccessor contextAccessor, ILogger<ComputeExpressionService> logger, 
            IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void UpdateValue(string expressionGuid, string value)
        {
            throw new NotImplementedException();
        }

        public void UpdateValue(Guid expressionGuid, string value)
        {
            throw new NotImplementedException();
        }
    }
}