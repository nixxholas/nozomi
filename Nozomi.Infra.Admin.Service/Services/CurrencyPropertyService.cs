using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Services
{
    public class CurrencyPropertyService : BaseService<CurrencyPropertyService, NozomiDbContext>, ICurrencyPropertyService
    {
        public CurrencyPropertyService(ILogger<CurrencyPropertyService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public long Create(CurrencyProperty currencyProperty, long userId = 0)
        {
            if (currencyProperty != null && currencyProperty.IsValid())
            {
                _unitOfWork.GetRepository<CurrencyProperty>().Add(currencyProperty);
                _unitOfWork.Commit(userId);

                return currencyProperty.Id;
            }

            return long.MinValue;
        }

        public bool Update(CurrencyProperty currencyProperty, long userId = 0)
        {
            if (currencyProperty != null && currencyProperty.Id > 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyProperty>()
                    .GetQueryable()
                    .AsTracking()
                    .SingleOrDefault(cp => cp.DeletedAt == null && cp.Id.Equals(currencyProperty.Id));

                if (query != null)
                {
                    query.Type = currencyProperty.Type;
                    query.Value = currencyProperty.Value;
                    query.IsEnabled = currencyProperty.IsEnabled;
                    
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public bool Delete(long currencyPropertyId, bool hardDelete = false, long userId = 0)
        {
            if (currencyPropertyId > 0)
            {
                var query = _unitOfWork.GetRepository<CurrencyProperty>()
                    .Get(cp => cp.Id.Equals(currencyPropertyId))
                    .SingleOrDefault(cp => cp.DeletedAt == null);

                if (query != null)
                {
                    if (hardDelete)
                    {
                        _unitOfWork.GetRepository<CurrencyProperty>().Delete(query);
                    }
                    else
                    {
                        query.DeletedAt = DateTime.UtcNow;
                        _unitOfWork.GetRepository<CurrencyProperty>().Update(query);
                    }

                    _unitOfWork.Commit(userId);
                }
            }

            return false;
        }
    }
}