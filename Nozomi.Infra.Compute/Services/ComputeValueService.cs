using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            if (computeValue != null)
            {
                _unitOfWork.GetRepository<ComputeValue>().Add(computeValue);
                _unitOfWork.Commit(computeValue.CreatedById);
                return;
            }
            
            _logger.LogWarning($"{_serviceName} Add: Null compute value.");
        }

        public void Push(ComputeValue computeValue)
        {
            if (computeValue != null)
            {
                // Obtain the last value
                var lastComputeValue = _unitOfWork.GetRepository<ComputeValue>()
                    .GetQueryable()
                    .AsNoTracking()
                    .OrderByDescending(v => v.CreatedAt)
                    .FirstOrDefault(v => v.ComputeGuid.Equals(computeValue.ComputeGuid));
                
                // Do not add if its the same
                if (lastComputeValue != null && lastComputeValue.Value.Equals(computeValue.Value))
                {
                    _logger.LogInformation($"{_serviceName} Push: Incoming value is the same as the previous " +
                                           $"value for Compute {computeValue.ComputeGuid}; Ignoring update.");
                    return;
                }
                
                _unitOfWork.GetRepository<ComputeValue>().Add(computeValue);
                _unitOfWork.Commit(computeValue.CreatedById);
                return;
            }
            
            _logger.LogWarning($"{_serviceName} Push: Null compute value.");
        }
    }
}