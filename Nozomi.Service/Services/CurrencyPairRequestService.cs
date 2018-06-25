
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairRequestService : BaseService<CurrencyPairRequestService, NozomiDbContext>, ICurrencyPairRequestService
    {
        public CurrencyPairRequestService(ILogger<CurrencyPairRequestService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(CurrencyPairRequest cpRequest, long userId = 0)
        {
            throw new NotImplementedException();
        }

        public CurrencyPairRequest Get(Expression<Func<CurrencyPairRequest, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public dynamic GetObsc(Expression<Func<CurrencyPairRequest, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Update(CurrencyPairRequest cpRequest, long userId = 0)
        {
            throw new NotImplementedException();
        }

        public bool SoftDelete(long cpRequestId, long userId = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CurrencyPairRequest> GetAllActive(bool track = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CurrencyPairRequest> GetAllActive(Expression<Func<CurrencyPairRequest, bool>> predicate, bool track = false)
        {
            throw new NotImplementedException();
        }
    }
}