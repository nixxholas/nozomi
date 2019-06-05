using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
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
    }
}