using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        public long Create(Request request, long userId = 0)
        {
            try
            {
                _unitOfWork.GetRepository<Request>().Add(request);
                _unitOfWork.SaveChanges(userId);

                return request.Id;
            }
            catch (Exception ex)
            {
                return long.MinValue;
            }
        }

        public NozomiResult<string> Create(CreateRequest createRequest, long userId = 0)
        {
            try
            {
                if (createRequest == null || !createRequest.IsValid())
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Failed to create request. Please make sure " +
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

            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
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

        public NozomiResult<string> Update(UpdateRequest updateRequest, long userId = 0)
        {
            try
            {
                if (updateRequest == null || !updateRequest.IsValid())
                    return new NozomiResult<string>(NozomiResultType.Failed, "Failed to update request");

                var reqToUpd = _unitOfWork.GetRepository<Request>()
                    .Get(r => r.Id.Equals(updateRequest.Id) && r.DeletedAt == null)
                    .SingleOrDefault();

                if (reqToUpd == null)
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Failed to update request. Unable to find the request");

                reqToUpd.DataPath = updateRequest.DataPath;
                reqToUpd.Delay = updateRequest.Delay;
                reqToUpd.RequestType = updateRequest.RequestType;
                reqToUpd.ResponseType = updateRequest.ResponseType;
                reqToUpd.IsEnabled = updateRequest.IsEnabled;

                _unitOfWork.GetRepository<Request>().Update(reqToUpd);
                _unitOfWork.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Successfully updated the request!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> Delete(long reqId, bool hardDelete = false, long userId = 0)
        {
            try
            {
                if (reqId > 0 && userId >= 0)
                {
                    var reqToDel = _unitOfWork.GetRepository<Request>()
                        .Get(r => r.Id.Equals(reqId) && r.DeletedAt == null)
                        .SingleOrDefault();

                    if (reqToDel != null)
                    {
                        if (!hardDelete)
                        {
                            reqToDel.DeletedAt = DateTime.UtcNow;
                            reqToDel.DeletedBy = userId;
                            _unitOfWork.GetRepository<Request>().Update(reqToDel);
                        }
                        else
                        {
                            _unitOfWork.GetRepository<Request>().Delete(reqToDel);
                        }
                        
                        _unitOfWork.Commit(userId);

                        return new NozomiResult<string>(NozomiResultType.Success, "Request successfully deleted!");
                    }
                }

                return new NozomiResult<string>(NozomiResultType.Failed, "Invalid request ID.");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }
    }
}