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
        long Create(Request request, long userId = 0);
        
        NozomiResult<string> Create(CreateRequest createRequest, long userId = 0);
        
        bool Delay(Request request, TimeSpan duration);

        bool HasUpdated(long requestId);

        NozomiResult<string> Update(UpdateRequest updateRequest, long userId = 0);

        NozomiResult<string> Delete(long reqId, bool hardDelete = false, long userId = 0);

        bool ManualPoll(long id, long userId = 0);
    } 
}