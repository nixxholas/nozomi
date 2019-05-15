using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Events.Interfaces
{
    public interface IRequestEvent
    {
        Request GetByGuid(Guid guid, bool track = false);
        
        Request GetActive(long id, bool track = false);
        
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
    }
}