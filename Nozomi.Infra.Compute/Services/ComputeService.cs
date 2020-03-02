using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Compute.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Compute.Data;

namespace Nozomi.Infra.Compute.Services
{
    public class ComputeService : BaseService<ComputeService, NozomiComputeDbContext>, IComputeService
    {
        public ComputeService(ILogger<ComputeService> logger, NozomiComputeDbContext context) 
            : base(logger, context)
        {
        }

        public ComputeService(IHttpContextAccessor contextAccessor, ILogger<ComputeService> logger, 
            NozomiComputeDbContext context) 
            : base(contextAccessor, logger, context)
        {
        }

        public void Modified(Guid guid, bool failed = false, string userId = null)
        {
            var compute = _context.Computes.AsTracking()
                .SingleOrDefault(c => c.Guid.Equals(guid));
            
            if (compute == null)
                throw new NullReferenceException($"{_serviceName} Modified (GUID): Invalid guid.");
            
            compute.ModifiedAt = DateTime.UtcNow;

            if (failed)
                compute.FailCount += 1;
            
            _context.Computes.Update(compute);
            _context.SaveChanges(userId);
        }

        public void Modified(string guid, bool failed = false, string userId = null)
        {
            if (!Guid.TryParse(guid, out var parsedGuid))
                throw new ArgumentException($"{_serviceName} Modified (string): Invalid guid string.");
            
            var compute = _context.Computes.AsTracking()
                .SingleOrDefault(c => c.Guid.Equals(parsedGuid));
            
            if (compute == null)
                throw new NullReferenceException($"{_serviceName} Modified (string): Invalid guid.");
            
            compute.ModifiedAt = DateTime.UtcNow;

            if (failed)
                compute.FailCount += 1;
            
            _context.Computes.Update(compute);
            _context.SaveChanges(userId);
        }
    }
}