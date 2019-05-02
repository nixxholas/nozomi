using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerable;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyService : BaseService<CurrencyService, NozomiDbContext>, ICurrencyService
    {
        public CurrencyService(ILogger<CurrencyService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateCurrency createCurrency, long userId = 0)
        {
            try
            {
                if (createCurrency != null && createCurrency.IsValid())
                {
                    _unitOfWork.GetRepository<Currency>().Add(new Currency()
                    {
                        Abbrv = createCurrency.Abbrv,
                        Name = createCurrency.Name,
                        CurrencyTypeId = createCurrency.CurrencyTypeId,
                        CurrencySourceId = createCurrency.CurrencySourceId,
                        WalletTypeId = createCurrency.WalletTypeId
                    });
                    _unitOfWork.Commit(userId);

                    return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully created!");
                }

                return new NozomiResult<string>(NozomiResultType.Failed, "Failed to create currency. Please make sure " +
                                                                         "that your currency object is proper.");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> Update(UpdateCurrency currency, long userId = 0)
        {
            if (currency != null && currency.IsValid())
            {
                var currToUpd = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Id.Equals(currency.Id) && c.DeletedAt == null)
                    .SingleOrDefault();

                if (currToUpd != null)
                {
                    currToUpd.Abbrv = currency.Abbrv;
                    currToUpd.CurrencyTypeId = currency.CurrencyTypeId;
                    currToUpd.Description = currency.Description;
                    currToUpd.Denominations = currency.Denominations;
                    currToUpd.DenominationName = currency.DenomationName;
                    currToUpd.Name = currency.Name;
                    currToUpd.WalletTypeId = currency.WalletTypeId;
                    currToUpd.IsEnabled = currency.IsEnabled;
                    
                    _unitOfWork.GetRepository<Currency>().Update(currToUpd);
                    _unitOfWork.Commit(userId);

                    return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully updated!");
                }
            }

            return new NozomiResult<string>(NozomiResultType.Failed, "Invalid payload, please ensure that your currency" +
                                                                     " payload contains valid entries.");
        }

        public NozomiResult<string> Delete(long currencyId, bool hardDelete = false, long userId = 0)
        {
            if (currencyId > 0 && userId >= 0)
            {
                var currToDel = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Id.Equals(currencyId) && c.DeletedAt == null)
                    .SingleOrDefault();

                if (currToDel != null)
                {
                    currToDel.DeletedAt = DateTime.UtcNow;
                    currToDel.DeletedBy = userId;
                    
                    _unitOfWork.GetRepository<Currency>().Update(currToDel);
                    _unitOfWork.Commit(userId);

                    return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully deleted!");
                }
            }

            return new NozomiResult<string>(NozomiResultType.Failed, "Invalid Currency ID.");
        }
    }
}