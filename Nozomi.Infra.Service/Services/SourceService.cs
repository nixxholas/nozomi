using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Source;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.Source;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class SourceService : BaseService<SourceService, NozomiDbContext>, ISourceService
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ISourceTypeEvent _sourceTypeEvent;
        
        public SourceService(ILogger<SourceService> logger, IUnitOfWork<NozomiDbContext> unitOfWork, 
            ISourceEvent sourceEvent, ISourceTypeEvent sourceTypeEvent) : base(logger, unitOfWork)
        {
            _sourceEvent = sourceEvent;
            _sourceTypeEvent = sourceTypeEvent;
        }

        public void Create(CreateSourceViewModel vm, string userId)
        {
            if (vm.IsValid() && !_sourceEvent.AbbreviationIsUsed(vm.Abbreviation))
            {
                var sourceType = _sourceTypeEvent.Find(vm.SourceType);
                if (sourceType == null)
                    throw new ArgumentNullException("Invalid source type.");
                
                var source = new Source(vm.Abbreviation, vm.Name, vm.ApiDocsUrl, sourceType.Guid);
                
                _unitOfWork.GetRepository<Source>().Add(source);
                _unitOfWork.Commit(userId);

                return;
            }
            
            throw new ArgumentException("Invalid properties or a source type with the same abbreviation " +
                                        "already exists.");
        }

        public NozomiResult<string> Create(CreateSource createSource, string userId = null)
        {
            try
            {
                if (_unitOfWork.GetRepository<Source>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(s => s.Abbreviation.Equals(createSource.Abbreviation)))
                    return new NozomiResult<string>(NozomiResultType.Failed, "An existing source already exists!");

                var source = new Source
                {
                    APIDocsURL = createSource.ApiDocsUrl,
                    Abbreviation = createSource.Abbreviation,
                    Name = createSource.Name
                };
                
                _unitOfWork.GetRepository<Source>().Add(source);
                _unitOfWork.Commit(userId);
                
                return new NozomiResult<string>(NozomiResultType.Success, "Source successfully created!");
            }
            catch (DbUpdateException ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, "An exisiting source already exist");
            }
        }

        public bool Update(UpdateSource updateSource)
        {
            if (updateSource == null) return false;

            var sourceToUpd = _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Include(s => s.SourceCurrencies)
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
                        if (sourceToUpd.SourceCurrencies
                                .Any(c => c.Id.Equals(usc.Id)) // Make sure this source has the currency first
                            && usc.SourceId >= 0) // Make sure we're not making an invalid modification
                        {
                            // Modification
                            var currencySource = _unitOfWork.GetRepository<CurrencySource>()
                                .Get(c => c.Id.Equals(usc.Id) && c.DeletedAt == null).SingleOrDefault();

                            if (currencySource != null)
                            {
                                if (usc.SourceId.Equals(0))
                                {
                                    currencySource.DeletedAt = DateTime.Now;
                                }
                                else
                                {
                                    currencySource.SourceId = usc.SourceId;
                                }

                                _unitOfWork.GetRepository<CurrencySource>().Update(currencySource);
                                _unitOfWork.Commit(); // Commit this modification.
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                        else if (!sourceToUpd.SourceCurrencies.Any(c => c.Id.Equals(usc.Id) 
                                                                  && c.SourceId.Equals(usc.SourceId))
                            && usc.SourceId.Equals(sourceToUpd.Id))
                        {
                            // Addition?
                            var currency = _unitOfWork.GetRepository<Currency>()
                                .Get(c => c.Id.Equals(usc.Id) && c.DeletedAt == null)
                                .SingleOrDefault();

                            if (currency != null)
                            {
                                _unitOfWork.GetRepository<CurrencySource>().Add(new CurrencySource
                                {
                                    CurrencyId = usc.Id,
                                    SourceId = usc.SourceId
                                });
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
                                    currencyPair.SourceId = ucp.CurrencySourceId;
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
                                                                  && cp.SourceId.Equals(ucp.CurrencySourceId))
                            && ucp.CurrencySourceId.Equals(sourceToUpd.Id))
                        {
                            // Addition?
                            var currencyPair = _unitOfWork.GetRepository<CurrencyPair>()
                                .Get(cp => cp.Id.Equals(ucp.Id) && cp.DeletedAt == null)
                                .SingleOrDefault();

                            if (currencyPair != null)
                            {
                                currencyPair.SourceId = ucp.CurrencySourceId;
                                
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

        public bool Delete(long id, bool hardDelete = false, string userId = null)
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

                if (!string.IsNullOrWhiteSpace(userId)) source.DeletedById = userId;
                    
                _unitOfWork.GetRepository<Source>().Update(source);
                _unitOfWork.Commit();
            }

            return true;
        }
        
        // Services for Staff
        public bool StaffSourceUpdate(UpdateSource updateSource)
        {
            if (updateSource == null) return false;

            var sourceToUpd = _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Include(s => s.SourceCurrencies)
                .Include(s => s.CurrencyPairs)
                .SingleOrDefault(s => s.DeletedAt == null
                                      && s.Id.Equals(updateSource.Id));

            if (sourceToUpd != null)
            {
                // Don't perform string checks for more efficiency, allow the user to 
                // comply via due diligence
                sourceToUpd.Abbreviation = updateSource.Abbreviation;
                sourceToUpd.Name = updateSource.Name;
                sourceToUpd.APIDocsURL = updateSource.ApiDocsUrl;
                sourceToUpd.IsEnabled = updateSource.IsEnabled;

                if (updateSource.UpdateSourceCurrencies != null && updateSource.UpdateSourceCurrencies.Count > 0)
                {
                    foreach (var usc in updateSource.UpdateSourceCurrencies)
                    {
                        // Modification or Addition?
                        if (sourceToUpd.SourceCurrencies
                                .Any(c => c.Id.Equals(usc.Id)) // Make sure this source has the currency first
                            && usc.SourceId >= 0) // Make sure we're not making an invalid modification
                        {
                            // Modification
                            var currency = _unitOfWork.GetRepository<CurrencySource>()
                                .Get(c => c.Id.Equals(usc.Id) && c.DeletedAt == null).SingleOrDefault();

                            if (currency != null)
                            {
                                if (usc.SourceId.Equals(0))
                                {
                                    currency.DeletedAt = DateTime.Now;

                                    _unitOfWork.GetRepository<CurrencySource>().Update(currency);
                                    _unitOfWork.Commit(); // Commit this modification.
                                }
                                else
                                {
                                    if (!_unitOfWork
                                        .GetRepository<CurrencySource>()
                                        .GetQueryable()
                                        .AsNoTracking()
                                        .Any(cs => cs.SourceId.Equals(usc.SourceId)
                                                                  && cs.CurrencyId.Equals(currency.CurrencyId)))
                                    {
                                        currency.SourceId = usc.SourceId;
                                        
                                        _unitOfWork.GetRepository<CurrencySource>().Update(currency);
                                        _unitOfWork.Commit(); // Commit this modification.
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                        else if (!sourceToUpd.SourceCurrencies.Any(c => c.Id.Equals(usc.Id) 
                                                                  && c.SourceId.Equals(usc.SourceId))
                            && usc.SourceId.Equals(sourceToUpd.Id))
                        {
                            // Addition?
                            var currency = _unitOfWork.GetRepository<Currency>()
                                .Get(c => c.Id.Equals(usc.Id) && c.DeletedAt == null)
                                .SingleOrDefault();

                            if (currency != null)
                            {
                                _unitOfWork.GetRepository<CurrencySource>().Add(new CurrencySource
                                {
                                    CurrencyId = currency.Id,
                                    SourceId = usc.SourceId
                                });
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

                                    _unitOfWork.GetRepository<CurrencyPair>().Update(currencyPair);
                                    _unitOfWork.Commit(); // Commit this modification.
                                }
                                else
                                {
                                    // Make sure we're not changing the source where it already exists
                                    if (!_unitOfWork
                                        .GetRepository<CurrencyPair>()
                                        .GetQueryable()
                                        .AsNoTracking()
                                        .Any(cp => cp.MainTicker.Equals(currencyPair.MainTicker
                                                       ,StringComparison.InvariantCultureIgnoreCase)
                                        && cp.CounterTicker.Equals(currencyPair.CounterTicker,
                                            StringComparison.InvariantCultureIgnoreCase)
                                        && cp.SourceId.Equals(ucp.CurrencySourceId)))
                                    {
                                        currencyPair.SourceId = ucp.CurrencySourceId;

                                        _unitOfWork.GetRepository<CurrencyPair>().Update(currencyPair);
                                        _unitOfWork.Commit(); // Commit this modification.
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                // Log failure
                            }
                        }
                        else if (!sourceToUpd.CurrencyPairs.Any(cp => cp.Id.Equals(ucp.Id) 
                                                                  && cp.SourceId.Equals(ucp.CurrencySourceId))
                            && ucp.CurrencySourceId.Equals(sourceToUpd.Id))
                        {
                            // Addition?
                            var currencyPair = _unitOfWork.GetRepository<CurrencyPair>()
                                .Get(cp => cp.Id.Equals(ucp.Id) && cp.DeletedAt == null)
                                .SingleOrDefault();

                            if (currencyPair != null)
                            {
                                currencyPair.SourceId = ucp.CurrencySourceId;
                                
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

                _unitOfWork.GetRepository<Source>().Update(sourceToUpd);
                _unitOfWork.Commit();
                
                return true;
            }

            return false;
        }
    }
}
