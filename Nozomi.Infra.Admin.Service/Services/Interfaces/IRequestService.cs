using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;

namespace Nozomi.Infra.Admin.Service.Services.Interfaces
{
    public interface IRequestService
    {
        long Create(Request request, string userId = null);
        
        NozomiResult<string> Create(CreateRequest createRequest, string userId = null);
        
        bool Delay(Request request, TimeSpan duration);

        bool HasUpdated(long requestId);

        bool HasUpdated(ICollection<Request> requests);

        NozomiResult<string> Update(UpdateRequest updateRequest, string userId = null);

        NozomiResult<string> Delete(long reqId, bool hardDelete = false, string userId = null);

        bool ManualPoll(long id, string userId = null);
    } 
}