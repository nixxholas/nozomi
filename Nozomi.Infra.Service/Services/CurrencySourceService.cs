using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencySource;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencySource;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencySourceService : BaseService<CurrencySourceService, NozomiDbContext>, ICurrencySourceService
    {
        private readonly ICurrencyEvent _currencyEvent;
        private readonly ICurrencySourceEvent _currencySourceEvent;
        private readonly ISourceEvent _sourceEvent;
        
        public CurrencySourceService(ILogger<CurrencySourceService> logger, ICurrencyEvent currencyEvent, 
            ICurrencySourceEvent currencySourceEvent, ISourceEvent sourceEvent, 
            IUnitOfWork<NozomiDbContext> context) : base(logger, context)
        {
            _currencyEvent = currencyEvent;
            _currencySourceEvent = currencySourceEvent;
            _sourceEvent = sourceEvent;
        }

        public NozomiResult<string> Create(CreateCurrencySource currencySource, string userId = null)
        {
            try
            {
                if (_context.GetRepository<CurrencySource>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(cs => cs.CurrencyId.Equals(currencySource.CurrencyId)
                               && cs.SourceId.Equals(currencySource.SourceId)))
                    return new NozomiResult<string>(NozomiResultType.Failed, "Source to currency binding already exists.");
                
                _context.GetRepository<CurrencySource>().Add(new CurrencySource
                {
                    CurrencyId = currencySource.CurrencyId,
                    SourceId = currencySource.SourceId
                });

                _context.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Source successfully added!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }

        public bool Create(CreateCurrencySourceViewModel vm, string userId = null)
        {
            if (vm.IsValid())
            {
                var source = _sourceEvent.GetByGuid(vm.SourceGuid);
                var currency = _currencyEvent.GetBySlug(vm.CurrencySlug);

                if (source != null && currency != null 
                                   && !_currencySourceEvent.Exists(source.Id, currency.Id))
                {
                    _context.GetRepository<CurrencySource>()
                        .Add(new CurrencySource(source.Id, currency.Id));

                    _context.Commit(userId);

                    return true;
                }
            }

            _logger.LogInformation($"{_serviceName} - [Create]: Invalid ViewModel.");
            return false;
        }

        public bool EnsurePairIsCreated(string mainTicker, string counterTicker, long sourceId,
            string userId = null)
        {
            if (!_context.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.DeletedAt == null && cs.IsEnabled 
                                                  && cs.Currency != null 
                                                  && cs.Currency.Slug.Equals(mainTicker)))
            {
                // Since it doesn't exist, symlink the new currency source
                var mainCurrencySource = new CurrencySource(sourceId);
                var mainCurrency = _currencyEvent.GetBySlug(mainTicker);
                mainCurrencySource.CurrencyId = mainCurrency.Id;
                
                _context.GetRepository<CurrencySource>().Add(mainCurrencySource);
                _context.Commit(userId);
            }
            
            if (!_context.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.DeletedAt == null && cs.IsEnabled 
                                                && cs.Currency != null 
                                                && cs.Currency.Slug.Equals(counterTicker)))
            {
                // Since it doesn't exist, symlink the new currency source
                var counterCurrencySource = new CurrencySource(sourceId);
                var counterCurrency = _currencyEvent.GetBySlug(counterTicker);
                counterCurrencySource.CurrencyId = counterCurrency.Id;
                
                _context.GetRepository<CurrencySource>().Add(counterCurrencySource);
                _context.Commit(userId);
            }

            return true;
        }

        public NozomiResult<string> Delete(long id, string userId = null)
        {
            try
            {
                var csToDelete = _context.GetRepository<CurrencySource>()
                    .Get(cs => cs.Id.Equals(id) && cs.DeletedAt == null)
                    .SingleOrDefault();

                if (csToDelete == null)
                    return new NozomiResult<string>(NozomiResultType.Failed, "Unable to delete currency source");
                
                csToDelete.DeletedAt = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(userId))
                    csToDelete.DeletedById = userId;

                _context.GetRepository<CurrencySource>().Update(csToDelete);
                _context.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "Currency source successfully deleted!");
            }
            catch (Exception ex)
            {
                return new NozomiResult<string>(NozomiResultType.Failed, ex.ToString());
            }
        }
    }
}