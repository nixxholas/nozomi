using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.Services.Requests
{
    public class RequestService : BaseService<RequestService, NozomiDbContext>, IRequestService
    {
        public RequestService(ILogger<RequestService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(Request req, long userId = 0)
        {
            // Safetynet
            if (req != null && req.IsValid())
            {
                _unitOfWork.GetRepository<Request>().Add(req);
                _unitOfWork.Commit(userId);

                return req.Id;
            }

            return -1;
        }

        public bool Update(Request req, long userId = 0)
        {
            // Safetynet
            if (req == null || !req.IsValid()) return false;
            
            var reqToUpd = _unitOfWork.GetRepository<Request>()
                .Get(r => r.Id.Equals(req.Id) && r.DeletedAt == null)
                .SingleOrDefault();

            if (reqToUpd == null) return false;
                
            req.DataPath = req.DataPath;
            req.RequestType = req.RequestType;
            req.IsEnabled = req.IsEnabled;
                
            _unitOfWork.GetRepository<Request>().Update(reqToUpd);
            _unitOfWork.Commit(userId);

            return true;

        }

        public bool SoftDelete(long reqId, long userId = 0)
        {
            if (reqId > 0)
            {
                var reqToDel = _unitOfWork.GetRepository<Request>()
                    .Get(r => r.Id.Equals(reqId) && r.DeletedAt == null)
                    .SingleOrDefault();

                if (reqToDel != null)
                {
                    reqToDel.DeletedAt = DateTime.UtcNow;
                    reqToDel.DeletedBy = userId;

                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Request> GetAllActive(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(r => r.DeletedAt == null && r.IsEnabled);
            }

            return _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .Where(r => r.DeletedAt == null && r.IsEnabled)
                .Include(r => r.RequestComponents)
                .Include(r => r.RequestProperties);
        }

        public IEnumerable<dynamic> GetAllActiveObsc(bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Request> GetAll(bool track = false)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<dynamic> GetAllObsc(bool track = false)
        {
            throw new System.NotImplementedException();
        }
    }
}