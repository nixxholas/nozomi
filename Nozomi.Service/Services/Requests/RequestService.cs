using System.Collections.Generic;
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
            if (req != null && req.IsValid() && userId >= 0)
            {
                _unitOfWork.GetRepository<Request>().Add(req);
                _unitOfWork.Commit(userId);

                return req.Id;
            }

            return -1;
        }

        public bool Update(Request req, long userId = 0)
        {
            throw new System.NotImplementedException();
        }

        public bool SoftDelete(long reqId, long userId = 0)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Request> GetAllActive(bool track = false)
        {
            throw new System.NotImplementedException();
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