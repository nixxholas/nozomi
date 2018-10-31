﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nozomi.Data.AreaModels.v1.CurrencyPairRequest;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.WebModels;

namespace Nozomi.Service.Services.Interfaces
{
    public interface ICurrencyPairRequestService
    {
        long Create(CurrencyPairRequest cpRequest, long userId = 0);

        CurrencyPairRequest Get(Expression<Func<CurrencyPairRequest, bool>> predicate);

        dynamic GetObsc(Expression<Func<CurrencyPairRequest, bool>> predicate);

        bool Update(CurrencyPairRequest cpRequest, long userId = 0);
        
        bool Update(UpdateCurrencyPairRequest obj, long userId = 0);

        bool Delete(long cpRequestId, bool hardDelete, long userId = 0);
        
        IQueryable<CurrencyPairRequest> GetAllActive(bool track = false);

        IQueryable<CurrencyPairRequest> GetAllActive(Expression<Func<CurrencyPairRequest, bool>> predicate, bool track = false);

        ICollection<CurrencyPairRequest> GetAllByRequestType(RequestType requestType);

        bool ManualPoll(long id, long userId = 0);
    }
}