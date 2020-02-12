using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.RequestProperty;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class RequestPropertyEvent : BaseEvent<RequestPropertyEvent, NozomiDbContext>, IRequestPropertyEvent
    {
        public RequestPropertyEvent(ILogger<RequestPropertyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public RequestProperty GetByGuid(string guid, string validatingUserId = null, 
            bool ensureDisabledOrDeleted = true, bool track = false)
        {
            if (!string.IsNullOrEmpty(guid) && Guid.TryParse(guid, out var requestPropertyGuid))
            {
                var query = _unitOfWork.GetRepository<RequestProperty>()
                    .GetQueryable();
                    
                if (track)
                    query = query.AsTracking()
                            .Where(rp => rp.Guid.Equals(requestPropertyGuid));
                else
                    query = query.AsNoTracking()
                    .Where(rp => rp.Guid.Equals(requestPropertyGuid));

                if (query.Any())
                {
                    if (!string.IsNullOrEmpty(validatingUserId))
                        query = query.Where(rp => rp.CreatedById.Equals(validatingUserId));

                    if (ensureDisabledOrDeleted)
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
        /// <param name="ensureDisabledOrDeleted">Optional. Ensure the properties that are disabled or deleted
        /// should be ignored.</param>
        /// <returns>List of matching properties for the request.</returns>
        public IEnumerable<RequestPropertyViewModel> GetByRequest(string requestGuid, string validatingUserId = null,
            bool ensureDisabledOrDeleted = true)
        {
            if (!string.IsNullOrEmpty(requestGuid))
            {
                var query = _unitOfWork.GetRepository<RequestProperty>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Include(rp => rp.Request)
                    .Where(rp => rp.Request.Guid.ToString().Equals(requestGuid));

                if (!ensureDisabledOrDeleted)
                    query = query.Where(rp => rp.DeletedAt == null && rp.IsEnabled);
                
                if (!string.IsNullOrEmpty(validatingUserId))
                    query = query.Where(rp => rp.CreatedById.Equals(validatingUserId));

                return query.Select(rp => new RequestPropertyViewModel(rp.Guid, rp.RequestPropertyType, 
                    rp.Key, rp.Value));
            }
            
            throw new NullReferenceException($"{_eventName} GetByRequest: Invalid Request GUID.");
        }
    }
}