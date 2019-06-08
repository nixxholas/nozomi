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
    public class CurrencyTypeService : BaseService<CurrencyTypeService, NozomiDbContext>, ICurrencyTypeService
    {
        public CurrencyTypeService(ILogger<CurrencyTypeService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public long Create(CurrencyType currencyType, long userId = 0)
        {
            if (currencyType != null && currencyType.IsValid())
            {
                _unitOfWork.GetRepository<CurrencyType>().Add(currencyType);
                _unitOfWork.Commit(userId);

                return currencyType.Id;
            }

            return long.MinValue;
        }

        public bool Delete(long currencyTypeId, bool hardDelete = false, long userId = 0)
        {
            if (currencyTypeId > 0)
            {
                var cTypeToDel = _unitOfWork.GetRepository<CurrencyType>()
                    .GetQueryable()
                    .SingleOrDefault(ct => ct.DeletedAt == null && ct.Id.Equals(currencyTypeId));

                if (cTypeToDel != null)
                {
                    if (hardDelete)
                    {
                        _unitOfWork.GetRepository<CurrencyType>().Delete(cTypeToDel);
                    }
                    else
                    {
                        cTypeToDel.DeletedAt = DateTime.UtcNow;
                        cTypeToDel.DeletedBy = userId;
                    }

                    _unitOfWork.Commit(userId);
                }
            }

            return false;
        }
    }
}