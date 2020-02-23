using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Services
{
    public class ComputeService : BaseService<ComputeService, NozomiComputeDbContext>, IComputeService
    {
        public ComputeService(ILogger<ComputeService> logger, IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public ComputeService(IHttpContextAccessor contextAccessor, ILogger<ComputeService> logger, 
            IUnitOfWork<NozomiComputeDbContext> unitOfWork) 
            : base(contextAccessor, logger, unitOfWork)
        {
        }

        public void Modified(Guid guid, string userId = null)
        {
            var compute = _unitOfWork.GetRepository<Data.Models.Web.Compute>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(c => c.Guid.Equals(guid));
            
            if (compute == null)
                throw new NullReferenceException($"{_serviceName} Modified (GUID): Invalid guid.");
            
            compute.ModifiedAt = DateTime.UtcNow;
            
            _unitOfWork.GetRepository<Data.Models.Web.Compute>().Update(compute);
            _unitOfWork.Commit(userId);
        }

        public void Modified(string guid, string userId = null)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                throw new ArgumentException($"{_serviceName} Modified (string): Invalid guid string.");
            
            var compute = _unitOfWork.GetRepository<Data.Models.Web.Compute>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(c => c.Guid.Equals(parsedGuid));
            
            if (compute == null)
                throw new NullReferenceException($"{_serviceName} Modified (string): Invalid guid.");
            
            compute.ModifiedAt = DateTime.UtcNow;
            
            _unitOfWork.GetRepository<Data.Models.Web.Compute>().Update(compute);
            _unitOfWork.Commit(userId);
        }
    }
}