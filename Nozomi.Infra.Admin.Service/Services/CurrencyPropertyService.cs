using System;
using System.Linq;
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
            throw new System.NotImplementedException();
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