using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.Services.Requests
{
    public class RequestLogService : BaseService<RequestLogService, NozomiDbContext>, IRequestLogService
    {
        public RequestLogService(ILogger<RequestLogService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(RequestLog rLog)
        {
            if (rLog != null)
            {
                _unitOfWork.GetRepository<RequestLog>().Add(rLog);
                _unitOfWork.Commit();
                
                return rLog.Id;
            }

            return -1;
        }

        public bool Update(RequestLog rLog, long userId = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetAllObsc(bool track = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RequestLog> GetAll(bool track = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RequestLog> GetAll(Expression<Func<RequestLog, bool>> predicate, bool track = false)
        {
            throw new NotImplementedException();
        }
    }
}