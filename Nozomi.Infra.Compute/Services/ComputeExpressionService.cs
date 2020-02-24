using System;
using System.Collections.Generic;
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
            if (Guid.TryParse(expressionGuid, out var parsedGuid) && !string.IsNullOrEmpty(value))
            {
                var query = _unitOfWork.GetRepository<ComputeExpression>()
                    .GetQueryable()
                    .AsTracking()
                    .SingleOrDefault(e => e.Guid.Equals(parsedGuid));
            
                if (query == null)
                    throw new KeyNotFoundException($"{_serviceName} UpdateValue (string): Can't find expression " +
                                                   $"{parsedGuid}.");

                if (!value.Equals(query.Value)) // If value is the same, ignore and check for updated timestamp
                {
                    // Else update the value
                    query.Value = value;
                }
                    
                query.ModifiedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<ComputeExpression>().Update(query);
                _unitOfWork.Commit();
                _logger.LogInformation($"{_serviceName} UpdateValue (string): Updated expression " +
                                       $"{query.Guid}.");
                return;
            }
            
            throw new NullReferenceException($"{_serviceName} UpdateValue (string): Invalid guid or value.");
        }

        public void UpdateValue(Guid expressionGuid, string value)
        {
            var query = _unitOfWork.GetRepository<ComputeExpression>()
                .GetQueryable()
                .AsTracking()
                .SingleOrDefault(e => e.Guid.Equals(expressionGuid));
            
            if (query == null)
                throw new KeyNotFoundException($"{_serviceName} UpdateValue (Guid): Can't find expression " +
                                               $"{expressionGuid}.");

            if (!string.IsNullOrEmpty(value))
            {
                if (!value.Equals(query.Value)) // If value is the same, ignore and check for updated timestamp
                {
                    // Else update the value
                    query.Value = value;
                }
            }
            
            query.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<ComputeExpression>().Update(query);
            _unitOfWork.Commit();
            _logger.LogInformation($"{_serviceName} UpdateValue (Guid): Updated expression " +
                                   $"{query.Guid}.");
        }
    }
}