﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Infra.Admin.Service.Services.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Admin.Service.Services
{
    public class CurrencyService : BaseService<CurrencyService, NozomiDbContext>, ICurrencyService
    {
        public CurrencyService(ILogger<CurrencyService> logger,
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateCurrency createCurrency, string userId = null)
        {
            try
            {
                if (createCurrency != null && createCurrency.IsValid())
                {
                    var currencyExists = _unitOfWork.GetRepository<Currency>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Any(c => c.Abbreviation.Equals(createCurrency.Abbreviation,
                            StringComparison.InvariantCultureIgnoreCase));

                    var sourceExists = _unitOfWork.GetRepository<Source>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Any(s => s.Id.Equals(createCurrency.CurrencySourceId));

                    if (!currencyExists)
                    {
                        var currency = new Currency()
                        {
                            Abbreviation = createCurrency.Abbreviation,
                            Slug = createCurrency.Slug,
                            Name = createCurrency.Name,
                            CurrencyTypeId = createCurrency.CurrencyTypeId,
                            IsEnabled = createCurrency.IsEnabled
                        };

                        _unitOfWork.GetRepository<Currency>().Add(currency);
                        _unitOfWork.Commit(userId);

                        // Make sure source exists before adding
                        if (sourceExists)
                        {
                            _unitOfWork.GetRepository<CurrencySource>().Add(new CurrencySource
                            {
                                CurrencyId = currency.Id,
                                SourceId = createCurrency.CurrencySourceId
                            });

                            _unitOfWork.Commit(userId);
                        }

                        return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully created" +
                                                                                  "and binded to source!");
                    }
                    else
                    {
                        var currency = _unitOfWork.GetRepository<Currency>()
                            .GetQueryable()
                            .AsNoTracking()
                            .SingleOrDefault(c => c.Abbreviation.Equals(createCurrency.Abbreviation,
                                StringComparison.InvariantCultureIgnoreCase));

                        // Make sure source exists before adding
                        if (sourceExists && currency != null)
                        {
                            _unitOfWork.GetRepository<CurrencySource>().Add(new CurrencySource
                            {
                                CurrencyId = currency.Id,
                                SourceId = createCurrency.CurrencySourceId
                            });

                            _unitOfWork.Commit(userId);
                        }
                    }

                    return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully created!");
                }

                return new NozomiResult<string>(NozomiResultType.Failed,
                    "Failed to create currency. Please make sure " +
                    "that your currency object is proper.");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public NozomiResult<string> Update(UpdateCurrency currency, string userId = null)
        {
            if (currency != null && currency.IsValid())
            {
                var currToUpd = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsTracking()
                    .SingleOrDefault(c => c.Id.Equals(currency.Id) && c.DeletedAt == null);

                if (currToUpd != null)
                {
                    currToUpd.Abbreviation = currency.Abbreviation;
                    currToUpd.Slug = currency.Slug;
                    currToUpd.LogoPath = currency.LogoPath;
                    currToUpd.CurrencyTypeId = currency.CurrencyTypeId;
                    currToUpd.Description = currency.Description;
                    currToUpd.Denominations = currency.Denominations;
                    currToUpd.DenominationName = currency.DenomationName;
                    currToUpd.Name = currency.Name;
                    currToUpd.IsEnabled = currency.IsEnabled;

                    _unitOfWork.Commit(userId);

                    return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully updated!");
                }
            }

            return new NozomiResult<string>(NozomiResultType.Failed,
                "Invalid payload, please ensure that your currency" +
                " payload contains valid entries.");
        }

        public NozomiResult<string> Delete(long currencyId, bool hardDelete = false, string userId = null)
        {
            if (currencyId > 0 && !string.IsNullOrWhiteSpace(userId))
            {
                var currToDel = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Id.Equals(currencyId) && c.DeletedAt == null)
                    .SingleOrDefault();

                if (currToDel != null)
                {
                    currToDel.DeletedAt = DateTime.UtcNow;
                    currToDel.DeletedById = userId;

                    _unitOfWork.GetRepository<Currency>().Update(currToDel);
                    _unitOfWork.Commit(userId);

                    return new NozomiResult<string>(NozomiResultType.Success, "Currency successfully deleted!");
                }
            }

            return new NozomiResult<string>(NozomiResultType.Failed, "Invalid Currency ID.");
        }
    }
}