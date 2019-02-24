using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.CurrencyModels;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class SourceService : BaseService<SourceService, NozomiDbContext>, ISourceService
    {
        public SourceService(ILogger<SourceService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public NozomiResult<string> Create(CreateSource createSource)
        {
            var res = new NozomiResult<string>(string.Empty);

            if (!createSource.IsValid())
            {
                res.ResultType = NozomiResultType.Failed;
                res.Message = "Invalid payload.";
                return res;
            }

            try
            {
                _unitOfWork.GetRepository<Source>().Add(new Source()
                {
                    APIDocsURL = createSource.ApiDocsUrl,
                    Abbreviation = createSource.Abbreviation,
                    Name = createSource.Name
                });

                _unitOfWork.Commit();
                
                res.ResultType = NozomiResultType.Success;
                res.Message = "Source created successfully.";
                return res;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex);
                
                res.ResultType = NozomiResultType.Failed;
                res.Message = "An existing source already exists.";
                return res;
            }
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
                sourceToUpd.APIDocsURL = updateSource.ApiDocsUrl;

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

        public bool Delete(long id, bool hardDelete = false, long userId = 0)
        {
            var source = _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .SingleOrDefault(s => s.Id.Equals(id) && s.DeletedAt == null);

            if (source == null) return false; // Null predicate result.
            
            if (hardDelete)
            {
                _logger.LogInformation("[SourceService] Hard delete of source id: " + source.Id 
                                                                                    + " is in progress by" 
                                                                                    + userId + " .");
                    
                _unitOfWork.GetRepository<Source>().Delete(source);
                _unitOfWork.Commit();
            }
            else
            {
                _logger.LogInformation("[SourceService] Soft delete of source id: " + source.Id 
                                                                                    + " is in progress by" 
                                                                                    + userId + " .");

                source.DeletedAt = DateTime.UtcNow;

                if (userId > 0) source.DeletedBy = userId;
                    
                _unitOfWork.GetRepository<Source>().Update(source);
                _unitOfWork.Commit();
            }

            return true;
        }
    }
}
