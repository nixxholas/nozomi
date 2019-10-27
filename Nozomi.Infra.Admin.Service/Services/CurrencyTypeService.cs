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

        public long Create(CurrencyType currencyType, string userId = null)
        {
            if (currencyType != null && currencyType.IsValid())
            {
                _unitOfWork.GetRepository<CurrencyType>().Add(currencyType);
                _unitOfWork.Commit(userId);

                return currencyType.Id;
            }

            return long.MinValue;
        }

        public bool Update(CurrencyType currencyType, string userId = null)
        {
            if (currencyType != null && currencyType.IsValid())
            {
                var cType = _unitOfWork.GetRepository<CurrencyType>()
                    .Get(ct => ct.Id.Equals(currencyType.Id))
                    .SingleOrDefault();

                if (cType != null)
                {
                    cType.Name = currencyType.Name;
                    cType.TypeShortForm = currencyType.TypeShortForm;
                    cType.IsEnabled = currencyType.IsEnabled;
                    
                    _unitOfWork.GetRepository<CurrencyType>().Update(cType);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public bool Delete(long currencyTypeId, bool hardDelete = false, string userId = null)
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
                        
                        if (!string.IsNullOrWhiteSpace(userId))
                            cTypeToDel.DeletedBy = Guid.Parse(userId);
                    }

                    _unitOfWork.Commit(userId);
                }
            }

            return false;
        }
    }
}