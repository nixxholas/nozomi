using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class SourceService : BaseService<SourceService, NozomiDbContext>, ISourceService
    {
        public SourceService(ILogger<SourceService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public bool Create(CreateSource createSource)
        {
            if (!createSource.IsValid()) return false;
            
            _unitOfWork.GetRepository<Source>().Add(new Source()
            {
                APIDocsURL = createSource.ApiDocsUrl,
                Abbreviation = createSource.Abbreviation,
                Name = createSource.Name
            });

            return true;
        }

        public bool Update(UpdateSource updateSource)
        {
            if (updateSource == null) return false;

            var sourceToUpd = _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Include(s => s.Currencies)
                .Include(s => s.CurrencyPairs)
                .SingleOrDefault(s => s.IsEnabled && s.DeletedAt == null
                                      && s.Id.Equals(updateSource.Id));

            if (sourceToUpd != null)
            {
                // Don't perform string checks for more efficiency, allow the user to 
                // comply via due diligence
                sourceToUpd.Abbreviation = updateSource.Abbreviation;
                sourceToUpd.Name = updateSource.Name;
                sourceToUpd.APIDocsURL = updateSource.APIDocsURL;

                if (updateSource.UpdateSourceCurrencies != null && updateSource.UpdateSourceCurrencies.Count > 0)
                {
                    foreach (var usc in updateSource.UpdateSourceCurrencies)
                    {
                        // Modification or Addition?
                        if (sourceToUpd.Currencies
                                .Any(c => c.Id.Equals(usc.Id)) // Make sure this source has the currency first
                            && usc.CurrencySourceId >= 0) // Make sure we're not making an invalid modification
                        {
                            // Modification
                            var currency = _unitOfWork.GetRepository<Currency>()
                                .Get(c => c.Id.Equals(usc.Id) && c.DeletedAt == null).SingleOrDefault();

                            if (currency != null)
                            {
                                if (usc.CurrencySourceId.Equals(0))
                                {
                                    currency.DeletedAt = DateTime.Now;
                                }
                                else
                                {
                                    currency.CurrencySourceId = usc.CurrencySourceId;
                                }

                                _unitOfWork.GetRepository<Currency>().Update(currency);
                                _unitOfWork.Commit(); // Commit this modification.
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                        else if (!sourceToUpd.Currencies.Any(c => c.Id.Equals(usc.Id) 
                                                                  && c.CurrencySourceId.Equals(usc.CurrencySourceId))
                            && usc.CurrencySourceId.Equals(sourceToUpd.Id))
                        {
                            // Addition?
                            var currency = _unitOfWork.GetRepository<Currency>()
                                .Get(c => c.Id.Equals(usc.Id) && c.DeletedAt == null)
                                .SingleOrDefault();

                            if (currency != null)
                            {
                                currency.CurrencySourceId = usc.CurrencySourceId;
                                
                                _unitOfWork.GetRepository<Currency>().Update(currency);
                                _unitOfWork.Commit();
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                    }
                }

                if (updateSource.UpdateCurrencyPairs != null && updateSource.UpdateCurrencyPairs.Count > 0)
                {
                    foreach (var ucp in updateSource.UpdateCurrencyPairs)
                    {
                        // Modification or addition?
                        if (sourceToUpd.CurrencyPairs
                                .Any(cp => cp.Id.Equals(ucp.Id)) // Make sure this source has the currency first
                            && ucp.CurrencySourceId >= 0) // Make sure we're not making an invalid modification
                        {
                            // Modification
                            var currencyPair = _unitOfWork.GetRepository<CurrencyPair>()
                                .Get(cp => cp.Id.Equals(ucp.Id) && cp.DeletedAt == null)
                                .SingleOrDefault();

                            if (currencyPair != null)
                            {
                                if (ucp.CurrencySourceId.Equals(0))
                                {
                                    currencyPair.DeletedAt = DateTime.Now;
                                }
                                else
                                {
                                    currencyPair.CurrencySourceId = ucp.CurrencySourceId;
                                }

                                _unitOfWork.GetRepository<CurrencyPair>().Update(currencyPair);
                                _unitOfWork.Commit(); // Commit this modification.
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                        else if (!sourceToUpd.CurrencyPairs.Any(cp => cp.Id.Equals(ucp.Id) 
                                                                  && cp.CurrencySourceId.Equals(ucp.CurrencySourceId))
                            && ucp.CurrencySourceId.Equals(sourceToUpd.Id))
                        {
                            // Addition?
                            var currencyPair = _unitOfWork.GetRepository<CurrencyPair>()
                                .Get(cp => cp.Id.Equals(ucp.Id) && cp.DeletedAt == null)
                                .SingleOrDefault();

                            if (currencyPair != null)
                            {
                                currencyPair.CurrencySourceId = ucp.CurrencySourceId;
                                
                                _unitOfWork.GetRepository<CurrencyPair>().Update(currencyPair);
                                _unitOfWork.Commit();
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                    }
                }

                _unitOfWork.Commit();
                
                return true;
            }

            return false;
        }

        public IEnumerable<Source> GetAllActive(bool includeNested = false)
        {
            if (includeNested) {
                return _unitOfWork.GetRepository<Source>()
                                  .GetQueryable()
                                  .Where(cs => cs.DeletedAt == null)
                                  .Where(cs => cs.IsEnabled)
                                  .Include(cs => cs.Currencies)
                                  .Include(cs => cs.CurrencyPairs);
            } else {
                return _unitOfWork.GetRepository<Source>()
                                  .GetQueryable()
                                  .Where(cs => cs.DeletedAt == null)
                                  .Where(cs => cs.IsEnabled);
            }
        }

        public IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false)
        {
            if (includeNested) {
                return _unitOfWork.GetRepository<Source>()
                    .GetQueryable()
                    .Where(cs => cs.DeletedAt == null)
                    .Where(cs => cs.IsEnabled)
                    .Include(cs => cs.Currencies)
                    .Include(cs => cs.CurrencyPairs)
                    .Select(cs => new
                    {
                        id = cs.Id, 
                        abbrv = cs.Abbreviation,
                        name = cs.Name,
                        currencies = cs.Currencies,
                        currencyPairs = cs.CurrencyPairs
                    });
            } else {
                return _unitOfWork.GetRepository<Source>()
                    .GetQueryable()
                    .Where(cs => cs.DeletedAt == null)
                    .Where(cs => cs.IsEnabled)
                    .Select(cs => new
                    {
                        id = cs.Id, 
                        abbrv = cs.Abbreviation,
                        name = cs.Name
                    });
            }
        }

        public IEnumerable<dynamic> GetAllNested()
        {
            return _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .AsNoTracking()
                // Make sure all currency sources are not disabled or deleted
                .Where(cs => cs.IsEnabled && cs.DeletedAt == null)
                .Include(cs => cs.CurrencyPairs)
                    .ThenInclude(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                            .ThenInclude(c => c.CurrencyType)
                .Where(cs => cs.CurrencyPairs
                    // Make sure all currencypairs are not disabled or deleted
                    .Any(cp => cp.IsEnabled && cp.DeletedAt == null
                    &&
                    // Make sure none of the currency pair's partial currency pair is not disabled or deleted
                    cp.PartialCurrencyPairs
                    .Any(pcp => pcp.Currency.IsEnabled && pcp.Currency.DeletedAt == null)))
                .Select(cs => new {
                    id = cs.Id,
                    abbreviation = cs.Abbreviation,
                    name = cs.Name,
                    currencyPairs = cs.CurrencyPairs
                        .Select(cp => new
                        {
                            id = cp.Id,
                            partialCurrencyPairs = cp.PartialCurrencyPairs
                                .Select(pcp => new
                                {
                                    currencyId = pcp.CurrencyId,
                                    currency = new
                                    {
                                        abbrv = pcp.Currency.Abbrv,
                                        currencyTypeId = pcp.Currency.CurrencyTypeId,
                                        currencyType = new {
                                            typeShortForm = pcp.Currency.CurrencyType.TypeShortForm,
                                            name = pcp.Currency.CurrencyType.Name
                                        },
                                        name = pcp.Currency.Name,
                                        walletTypeId = pcp.Currency.WalletTypeId
                                    },
                                    isMain = pcp.IsMain
                                })
                        })
                });
        }
    }
}
