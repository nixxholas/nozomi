using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data.WebModels.LoggingModels;

namespace Nozomi.Service.Services.Requests.Interfaces
{
    public interface IRequestLogService
    {
        long Create(RequestLog rLog);

        // Self-updating mechanism support. We can create 
        // infra in the future to allow self-reliant resolutions.
        bool Update(RequestLog rLog, long userId = 0);

        IEnumerable<dynamic> GetAllObsc(bool track = false);

        IEnumerable<RequestLog> GetAll(bool track = false);

        IEnumerable<RequestLog> GetAll(Expression<Func<RequestLog, bool>> predicate, bool track = false);
    }
}