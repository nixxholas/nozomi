using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestPropertyEvent : BaseEvent<RequestPropertyEvent, NozomiDbContext>, IRequestPropertyEvent
    {
        public RequestPropertyEvent(ILogger<RequestPropertyEvent> logger, NozomiDbContext unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public RequestProperty GetByGuid(string guid, string validatingUserId = null, 
            bool ensureNotDisabledOrDeleted = true, bool track = false)
        {
            if (!string.IsNullOrEmpty(guid) && Guid.TryParse(guid, out var requestPropertyGuid))
            {
                var query = _context.RequestProperties
                    .Where(rp => rp.Guid.Equals(requestPropertyGuid));

                query = track ? query.AsTracking() : query.AsNoTracking();

                if (query.Any())
                {
                    if (!string.IsNullOrEmpty(validatingUserId))
                        query = query.Where(rp => rp.CreatedById.Equals(validatingUserId));

                    if (ensureNotDisabledOrDeleted)
                        query = query.Where(rp => rp.DeletedAt == null && rp.IsEnabled);

                    return query.SingleOrDefault();
                }
                
                throw new KeyNotFoundException($"{_eventName} GetByGuid: Can't find the request property " +
                                               $"with this guid {guid}.");
            }
            
            throw new NullReferenceException($"{_eventName} GetByGuid: Invalid Request property GUID.");
        }

        /// <summary>
        /// Get Properties By Request API
        /// </summary>
        /// <param name="requestGuid">The GUID of the request.</param>
        /// <param name="validatingUserId">Optional. The ID of the user to validate the requests against.</param>
        /// <param name="ensureNotDisabledOrDeleted">Optional. Ensure the properties that are disabled or deleted
        /// should be ignored.</param>
        /// <returns>List of matching properties for the request.</returns>
        public IEnumerable<RequestPropertyViewModel> GetByRequest(string requestGuid, string validatingUserId = null,
            bool ensureNotDisabledOrDeleted = true)
        {
            if (!string.IsNullOrEmpty(requestGuid))
            {
                var query = _context.RequestProperties.AsNoTracking()
                    .Include(rp => rp.Request)
                    .Where(rp => rp.Request.Guid.ToString().Equals(requestGuid));

                if (ensureNotDisabledOrDeleted)
                    query = query.Where(rp => rp.DeletedAt == null && rp.IsEnabled);
                
                if (!string.IsNullOrEmpty(validatingUserId))
                    query = query.Where(rp => rp.CreatedById.Equals(validatingUserId));

                return query.Select(rp => new RequestPropertyViewModel(rp.Guid, rp.RequestPropertyType, 
                    rp.Key, rp.Value));
            }
            
            throw new NullReferenceException($"{_eventName} GetByRequest: Invalid Request GUID.");
        }

        public IEnumerable<RequestPropertyViewModel> ViewByRequest(string requestGuid, int index = 0, string userId = null)
        {
            if (Guid.TryParse(requestGuid, out var parsedGuid) && index >= 0)
            {
                var query = _context.RequestProperties.AsNoTracking()
                    .Include(rp => rp.Request)
                    .Where(rp => rp.Request.Guid.Equals(parsedGuid));

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(rp => rp.CreatedById.Equals(userId) ||
                                              rp.Request.CreatedById.Equals(userId));

                return query
                    .Skip(index * NozomiServiceConstants.RequestPropertyTakeoutLimit)
                    .Take(NozomiServiceConstants.RequestPropertyTakeoutLimit)
                    .Select(rp => new RequestPropertyViewModel(rp.Guid, rp.RequestPropertyType, 
                    rp.Key, rp.Value));
            }
            
            throw new ArgumentException("Invalid parameter/s.");
        }
    }
}