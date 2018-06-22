using System.Collections.Generic;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.Services.Requests.Interfaces
{
    public interface IRequestService
    {
        long Create(Request req, long userId = 0);

        bool Update(Request req, long userId = 0);

        bool SoftDelete(long reqId, long userId = 0);

        IEnumerable<Request> GetAllActive(bool track = false);

        IEnumerable<dynamic> GetAllActiveObsc(bool track = false);

        IEnumerable<Request> GetAll(bool track = false);

        IEnumerable<dynamic> GetAllObsc(bool track = false);
    } 
}