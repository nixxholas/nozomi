using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;

namespace Nozomi.Service.Services.Requests.Interfaces
{
    public interface IRequestService
    {
        NozomiResult<string> Create(CreateRequest createRequest, long userId = 0);
        
        bool Delay(Request request, TimeSpan duration);

        bool Update(Request req, long userId = 0);

        bool SoftDelete(long reqId, long userId = 0);

    } 
}