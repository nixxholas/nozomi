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
        private readonly IRequestPropertyEvent _requestPropertyEvent;
        
        public RequestPropertyService(ILogger<RequestPropertyService> logger, IUnitOfWork<NozomiDbContext> context,
            IRequestEvent requestEvent, IRequestPropertyEvent requestPropertyEvent) 
            : base(logger, context)
        {
            _requestEvent = requestEvent;
            _requestPropertyEvent = requestPropertyEvent;
        }

        public RequestPropertyService(IHttpContextAccessor contextAccessor, ILogger<RequestPropertyService> logger, 
            IUnitOfWork<NozomiDbContext> context, IRequestEvent requestEvent, 
            IRequestPropertyEvent requestPropertyEvent) 
            : base(contextAccessor, logger, context)
        {
            _requestEvent = requestEvent;
            _requestPropertyEvent = requestPropertyEvent;
        }

        public Task Create(CreateRequestPropertyInputModel inputModel, string userId)
        {
            if (inputModel.IsValid())
            {
                var request = _requestEvent.GetByGuid(Guid.Parse(inputModel.RequestGuid), true);
                
                if (request != null && (request.RequestProperties == null || !request.RequestProperties
                        // Ensure we don't check for duplicate custom headers
                    .Any(rp => (!rp.RequestPropertyType.Equals(RequestPropertyType.HttpHeader_Custom)
                                // Ignore invalids
                               || !rp.RequestPropertyType.Equals(RequestPropertyType.Invalid)) 
                               // And ensure there are no dupes among the defaults
                               && rp.RequestPropertyType.Equals(inputModel.Type))))
                {
                    // Initialise the soon to be added Request property
                    var requestProperty = new RequestProperty
                    {
                        RequestPropertyType = inputModel.Type,
                        Key = inputModel.Key,
                        Value = inputModel.Value,
                        RequestId = request.Id
                    };
                    // Push
                    _context.GetRepository<RequestProperty>().Add(requestProperty);
                    // Commit
                    _context.Commit(userId);
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
            if (inputModel.IsValid())
            {
                var requestProperty = _requestPropertyEvent.GetByGuid(inputModel.Guid, userId, 
                    true, true);
                
                if (requestProperty != null)
                {
                    // Update the Request property
                    requestProperty.RequestPropertyType = inputModel.Type;
                    requestProperty.Key = inputModel.Key;
                    requestProperty.Value = inputModel.Value;
                    // Push
                    _context.GetRepository<RequestProperty>().Update(requestProperty);
                    // Commit
                    _context.Commit(userId);
                    // Complete
                    _logger.LogInformation($"{_serviceName} Update: user {userId} has successfully " +
                                           $"updated the request property {inputModel.Guid}.");
                    return Task.CompletedTask;
                }
                
                throw new ArgumentNullException($"{_serviceName} Update: user {userId} either has no " +
                                                "rights to this request property or it doesn't exist.");
            }

            throw new ArgumentNullException($"{_serviceName} Update: Invalid input model.");
        }

        public Task Delete(string guid, string userId, bool hardDelete = true)
        {
            if (!string.IsNullOrEmpty(guid) && Guid.TryParse(guid, out var requestPropertyGuid)
                                            && !string.IsNullOrEmpty(userId))
            {
                // Obtain an untracked entity
                var requestProperty = _requestPropertyEvent.GetByGuid(guid, userId,
                    true, true);

                if (requestProperty != null)
                {
                    if (hardDelete)
                    {
                        // Delete the entity directly
                        _context.Context.Remove(requestProperty);
                    }
                    else
                    {
                        // Soft Delete
                        requestProperty.DeletedAt = DateTime.UtcNow;
                        requestProperty.DeletedById = userId;
                        // Push
                        _context.GetRepository<RequestProperty>().Update(requestProperty);
                    }
                    
                    //Commit
                    _context.Commit(userId);
                    // Complete
                    _logger.LogInformation($"{_serviceName} Delete: user {userId} has successfully " +
                                           $"deleted the request property {guid}.");
                    return Task.CompletedTask;
                }
                
                throw new ArgumentNullException($"{_serviceName} Delete: user {userId} either has no " +
                                                "rights to this request property or it doesn't exist");
            }

            throw new ArgumentNullException($"{_serviceName} Delete: Invalid request property guid.");
        }
    }
}