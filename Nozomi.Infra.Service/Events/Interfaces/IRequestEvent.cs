using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Request;
using Nozomi.Repo.Data;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestEvent
    {
        IEnumerable<Request> All(int index = 0, string createdBy = null);

        long Count(string userId);
        
        bool Exists(long requestId, bool ignoreDeletedOrDisabled = false, string userId = null);

        bool Exists(string requestGuid, bool ignoreDeletedOrDisabled = false, string userId = null);
        
        bool Exists(ComponentType type, long requestId);
        
        bool Exists(ComponentType type, string requestGuid);

        long GetId(string guid);
        
        Request Get(Expression<Func<Request, bool>> predicate);
        
        Request GetByGuid(Guid guid, string userId = null, bool track = false);
        
        Request GetActive(long id, bool track = false);

        IQueryable<RequestInputModel> ViewAll(int index = 0, string createdBy = null, bool enabledOnly = true, 
            bool track = false);

        /// <summary>
        /// Select all Requests with a limit of 50.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ICollection<RequestDTO> GetAllDTO(int index);
        
        IEnumerable<Request> GetAllActive(bool track = false);

        IEnumerable<dynamic> GetAllActiveObsc(bool track = false);

        IEnumerable<Request> GetAll(bool track = false);

        IEnumerable<Request> GetAll(Expression<Func<Request, bool>> predicate, bool track = false);

        IEnumerable<dynamic> GetAllObsc(bool track = false);
        
        IDictionary<string, ICollection<Request>> GetAllByRequestTypeUniqueToUrl(
            NozomiDbContext nozomiDbContext, RequestType requestType);
        
        ICollection<Request> GetAllByRequestType(RequestType requestType);

        IDictionary<string, ICollection<Request>> GetAllByRequestTypeUniqueToURL(RequestType requestType);
    }
}