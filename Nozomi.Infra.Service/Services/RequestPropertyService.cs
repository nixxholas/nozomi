using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class RequestPropertyService : BaseService<RequestPropertyService, NozomiDbContext>, IRequestPropertyService
    {
        private readonly IRequestEvent _requestEvent;
        
        public RequestPropertyService(ILogger<RequestPropertyService> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            IRequestEvent requestEvent) 
            : base(logger, unitOfWork)
        {
            _requestEvent = requestEvent;
        }

        public RequestPropertyService(IHttpContextAccessor contextAccessor, ILogger<RequestPropertyService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork, IRequestEvent requestEvent) 
            : base(contextAccessor, logger, unitOfWork)
        {
            _requestEvent = requestEvent;
        }

        public Task Create(CreateRequestPropertyInputModel inputModel, string userId)
        {
            if (inputModel.IsValid())
            {
                var request = _requestEvent.GetByGuid(Guid.Parse(inputModel.RequestGuid), true);
                
                if (request != null && !request.RequestProperties
                        // Ensure we don't check for duplicate custom headers
                    .Any(rp => (!rp.RequestPropertyType.Equals(RequestPropertyType.HttpHeader_Custom)
                                // Ignore invalids
                               || !rp.RequestPropertyType.Equals(RequestPropertyType.Invalid)) 
                               // And ensure there are no dupes among the defaults
                               && rp.RequestPropertyType.Equals(inputModel.Type)))
                {
                    // Initialise the soon to be added Request property
                    var requestProperty = new RequestProperty(inputModel.Type, inputModel.Key, inputModel.Value);
                    // Relation
                    requestProperty.RequestId = request.Id;
                    // Push
                    _unitOfWork.GetRepository<RequestProperty>().Add(requestProperty);
                    // Commit
                    _unitOfWork.Commit(userId);
                    // Complete
                    _logger.LogInformation($"{_serviceName} Create: user {userId} has successfully " +
                                           $"created a request property for request {inputModel.RequestGuid} of " +
                                           $"type {inputModel.Type}.");
                    return Task.CompletedTask;
                }
                
            }

            throw new ArgumentNullException($"{_serviceName} Create: Invalid input model.");
        }

        public Task Update(UpdateRequestPropertyInputModel inputModel, string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string guid, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}