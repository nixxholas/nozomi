﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Requests.Interfaces;

namespace Nozomi.Service.Services.Requests
{
    public class RequestService : BaseService<RequestService, NozomiDbContext>, IRequestService
    {
        public RequestService(ILogger<RequestService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateRequest createRequest, long userId = 0)
        {
            if (createRequest == null || !createRequest.IsValid())
                return new NozomiResult<string>(NozomiResultType.Failed, "Failed to create request. Please make sure " +
                                                                         "that your request object is proper");
            var request = new Request()
            {
                DataPath = createRequest.DataPath,
                Delay = createRequest.Delay,
                RequestType = createRequest.RequestType,
                ResponseType = createRequest.ResponseType
            };
                
            _unitOfWork.GetRepository<Request>().Add(request);
            _unitOfWork.Commit(userId);

            return new NozomiResult<string>(NozomiResultType.Success, "Request successfully created!", request);

        }

        public bool Delay(Request request, TimeSpan duration)
        {
            var req = _unitOfWork.GetRepository<Request>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(r => r.Id.Equals(request.Id)
                                      && r.DeletedAt == null
                                      && r.IsEnabled);

            if (req != null)
            {
                req.ModifiedAt = req.ModifiedAt.Add(duration);
                
                _unitOfWork.GetRepository<Request>().Update(req);
                _unitOfWork.Commit();

                return true;
            }

            return false;
        }

        public bool Update(Request req, long userId = 0)
        {
            // Safetynet
            if (req == null || !req.IsValid()) return false;
            
            var reqToUpd = _unitOfWork.GetRepository<Request>()
                .Get(r => r.Id.Equals(req.Id) && r.DeletedAt == null)
                .SingleOrDefault();

            if (reqToUpd == null) return false;

            reqToUpd.DataPath = req.DataPath;
            reqToUpd.RequestType = req.RequestType;
            reqToUpd.IsEnabled = req.IsEnabled;
                
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

                    _unitOfWork.GetRepository<Request>().Update(reqToDel);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }
    }
}