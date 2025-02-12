﻿using System;
using System.Collections.Generic;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Requests;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ViewModels.Request;

namespace Nozomi.Service.Services.Requests.Interfaces
{
    public interface IRequestService
    {
        long Create(Request request, string userId = null);

        void Create(CreateRequestInputModel vm, string userId = null);
        
        NozomiResult<string> Create(CreateRequest createRequest, string userId = null);
        
        bool Delay(Request request, TimeSpan duration);

        void DelayFailure(Guid guid);

        bool HasUpdated(long requestId);

        bool HasUpdated(ICollection<Request> requests);

        NozomiResult<string> Update(UpdateRequest updateRequest, string userId = null);

        bool Update(UpdateRequestInputModel vm, string userId = null);

        void Delete(string requestGuid, bool hardDelete = true, string userId = null);

        NozomiResult<string> Delete(long reqId, bool hardDelete = false, string userId = null);

        bool ManualPoll(long id, string userId = null);
    } 
}