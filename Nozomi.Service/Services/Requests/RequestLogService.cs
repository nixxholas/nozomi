using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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
            if (rLog != null)
            {
                var rLogToUpd = _unitOfWork.GetRepository<RequestLog>()
                    .Get(rl => rl.Id.Equals(rLog.Id) && rl.DeletedAt == null)
                    .SingleOrDefault();

                if (rLogToUpd != null)
                {
                    // Prohibit Log Contamination
                    // Relax... I didn't make these codes work, i wrote them here to make sure no
                    // one approves such an example through code review
                    
                    // rLogToUpd.RawPayload = rLog.RawPayload
                    rLogToUpd.Type = rLog.Type;
                    rLogToUpd.IsEnabled = rLog.IsEnabled; // idk why we need this but...
                    
                    _unitOfWork.GetRepository<RequestLog>().Update(rLogToUpd);
                    _unitOfWork.Commit(userId);

                    return true;
                } 
            }

            return false;
        }

        public IEnumerable<dynamic> GetAllObsc(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<RequestLog>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Select(rl => new
                    {
                        id = rl.Id,
                        rawPayload = rl.RawPayload,
                        requestId = rl.RequestId,
                        type = rl.Type,
                        isEnabled = rl.IsEnabled,
                        createdAt = rl.CreatedAt,
                        createdBy = rl.CreatedBy,
                        modifiedAt = rl.ModifiedAt,
                        modifiedBy = rl.ModifiedBy,
                        deletedAt = rl.DeletedAt,
                        deletedBy = rl.DeletedBy
                    });
            }

            return _unitOfWork.GetRepository<RequestLog>()
                .GetQueryable()
                .AsNoTracking()
                .Include(rl => rl.Request)
                .Select(rl => new
                {
                    id = rl.Id,
                    rawPayload = rl.RawPayload,
                    requestId = rl.RequestId,
                    request = new
                    {
                        dataPath = rl.Request.DataPath,
                        guid = rl.Request.Guid,
                        requestType = rl.Request.RequestType,
                        isEnabled = rl.Request.IsEnabled,
                        createdAt = rl.Request.CreatedAt,
                        createdBy = rl.Request.CreatedBy,
                        modifiedAt = rl.Request.ModifiedAt,
                        modifiedBy = rl.Request.ModifiedBy,
                        deletedAt = rl.Request.DeletedAt,
                        deletedBy = rl.Request.DeletedBy
                    },
                    type = rl.Type,
                    isEnabled = rl.IsEnabled,
                    createdAt = rl.CreatedAt,
                    createdBy = rl.CreatedBy,
                    modifiedAt = rl.ModifiedAt,
                    modifiedBy = rl.ModifiedBy,
                    deletedAt = rl.DeletedAt,
                    deletedBy = rl.DeletedBy
                });
        }

        public IEnumerable<RequestLog> GetAll(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<RequestLog>()
                    .GetQueryable()
                    .AsNoTracking();
            }

            return _unitOfWork.GetRepository<RequestLog>()
                .GetQueryable()
                .AsNoTracking()
                .Include(rl => rl.Request);
        }

        public IEnumerable<RequestLog> GetAll(Expression<Func<RequestLog, bool>> predicate, bool track = false)
        {
            throw new NotImplementedException();
        }
    }
}