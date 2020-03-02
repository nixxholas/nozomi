using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ViewModels.CurrencyPair;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairService : BaseService<CurrencyPairService, NozomiDbContext>, ICurrencyPairService
    {
        private readonly ISourceEvent _sourceEvent;
        private readonly ICurrencySourceService _currencySourceService;
        
        public CurrencyPairService(ILogger<CurrencyPairService> logger, ISourceEvent sourceEvent, 
            ICurrencySourceService currencySourceService, IUnitOfWork<NozomiDbContext> context) : base(logger,
            context)
        {
            _sourceEvent = sourceEvent;
            _currencySourceService = currencySourceService;
        }

        public bool Create(CreateCurrencyPairViewModel vm, string userId = null)
        {
            if (vm != null && vm.IsValid())
            {
                var source = _sourceEvent.GetByGuid(vm.SourceGuid, true);
                
                // Obtain currencies needed first, if it ain't there, make it.
                if (source != null && 
                    _currencySourceService.EnsurePairIsCreated(vm.MainTicker, vm.CounterTicker, source.Id, userId))
                {
                    _context.GetRepository<CurrencyPair>().Add(new CurrencyPair(vm.Type,
                        vm.MainTicker, vm.CounterTicker, vm.ApiUrl, vm.DefaultComponent, source.Id, vm.IsEnabled));
                
                    return _context.Commit(userId) == 1;   
                }
            }

            return false;
        }

        public bool Update(UpdateCurrencyPairViewModel vm, string userId = null)
        {
            if (vm != null && vm.IsValid())
            {
                var source = _sourceEvent.GetByGuid(vm.SourceGuid, true);
                
                // Obtain currencies needed first, if it ain't there, make it.
                if (source != null && 
                    _currencySourceService.EnsurePairIsCreated(vm.MainTicker, vm.CounterTicker, source.Id, userId))
                {
                    var currencyPair = _context.GetRepository<CurrencyPair>()
                        .GetQueryable()
                        .AsTracking()
                        .SingleOrDefault(cp => cp.Guid.Equals(vm.Guid));

                    if (currencyPair != null)
                    {
                        currencyPair.CurrencyPairType = vm.Type;
                        currencyPair.MainTicker = vm.MainTicker;
                        currencyPair.CounterTicker = vm.CounterTicker;
                        currencyPair.APIUrl = vm.ApiUrl;
                        currencyPair.DefaultComponent = vm.DefaultComponent;
                        currencyPair.SourceId = source.Id;
                        currencyPair.IsEnabled = source.IsEnabled;

                        _context.GetRepository<CurrencyPair>().Update(currencyPair);
                        return _context.Commit(userId) == 1;
                    }
                }
            }

            return false;
        }

        public NozomiResult<string> Create(CreateCurrencyPair createCurrencyPair, string userId = null)
        {
            if (createCurrencyPair == null || !createCurrencyPair.IsValid() ||
                // Make sure the pair we're creating doesn't exist.
                _context.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(cp => cp.MainTicker.Equals(createCurrencyPair.MainCurrencyAbbrv,
                        StringComparison.InvariantCultureIgnoreCase)
                               && cp.CounterTicker.Equals(createCurrencyPair.CounterCurrencyAbbrv,
                                   StringComparison.InvariantCultureIgnoreCase)
                               && cp.SourceId.Equals(createCurrencyPair.SourceId))) 
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid");
            
            // Check the main ticker
            if (!_context.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.Currency.Abbreviation
                               .Equals(createCurrencyPair.MainCurrencyAbbrv,
                                   StringComparison.InvariantCultureIgnoreCase)
                           && cs.SourceId.Equals(createCurrencyPair.SourceId)))
            {
                // Since this doesn't exist, beep the user.
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the main ticker is valid, that it exists in that source.");
            }

            // Check the counter ticker
            if (!_context.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.Currency.Abbreviation.Equals(createCurrencyPair.CounterCurrencyAbbrv,
                               StringComparison.InvariantCultureIgnoreCase)
                           && cs.SourceId.Equals(createCurrencyPair.SourceId)))
            {
                // Since this doesn't exist, beep the user.
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the main ticker is valid, that it exists in that source.");
            }
            
            var currencyPair = new CurrencyPair()
            {
                CurrencyPairType = createCurrencyPair.CurrencyPairType,
                APIUrl = createCurrencyPair.ApiUrl,
                DefaultComponent = createCurrencyPair.DefaultComponent,
                SourceId = createCurrencyPair.SourceId,
                MainTicker = createCurrencyPair.MainCurrencyAbbrv,
                CounterTicker = createCurrencyPair.CounterCurrencyAbbrv,
                IsEnabled = createCurrencyPair.IsEnabled
            };
            
            _context.GetRepository<CurrencyPair>().Add(currencyPair);
            _context.Commit(userId);

            return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair successfully created");
        }

        public NozomiResult<string> Update(UpdateCurrencyPair updateCurrencyPair, string userId = null)
        {
            if (updateCurrencyPair == null || !updateCurrencyPair.IsValid()
                || !_context.GetRepository<CurrencySource>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Include(cs => cs.Currency)
                    .Any(cs => cs.Currency.Abbreviation.Equals(updateCurrencyPair.MainCurrencyAbbrv)
                        && cs.SourceId.Equals(updateCurrencyPair.SourceId))
                || !_context.GetRepository<CurrencySource>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cs => cs.Currency)
                .Any(cs => cs.Currency.Abbreviation.Equals(updateCurrencyPair.CounterCurrencyAbbrv)
                           && cs.SourceId.Equals(updateCurrencyPair.SourceId)))
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid");

            var cpToUpd = _context.GetRepository<CurrencyPair>()
                .Get(cp => cp.Id.Equals(updateCurrencyPair.Id) && cp.DeletedAt == null)
                .SingleOrDefault();

            if (cpToUpd == null || _context.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Any(cp => !cp.Id.Equals(updateCurrencyPair.Id) 
                               && cp.SourceId.Equals(updateCurrencyPair.SourceId)
                               && cp.MainTicker.Equals(cp.MainTicker, StringComparison.InvariantCultureIgnoreCase)
                               && cp.CounterTicker.Equals(cp.CounterTicker, StringComparison.InvariantCultureIgnoreCase)))
                return new NozomiResult<string>(
                    NozomiResultType.Failed, "Please ensure that the payload is valid and if the data you're " +
                                             "submitting does not contain a pre-existing ticker pair.");

            cpToUpd.MainTicker = updateCurrencyPair.MainCurrencyAbbrv;
            cpToUpd.CounterTicker = updateCurrencyPair.CounterCurrencyAbbrv;
            cpToUpd.SourceId = updateCurrencyPair.SourceId;
            cpToUpd.CurrencyPairType = updateCurrencyPair.CurrencyPairType;
            cpToUpd.APIUrl = updateCurrencyPair.ApiUrl;
            cpToUpd.DefaultComponent = updateCurrencyPair.DefaultComponent;
            cpToUpd.IsEnabled = updateCurrencyPair.IsEnabled;
            
            _context.GetRepository<CurrencyPair>().Update(cpToUpd);
            _context.Commit(userId);
            
            return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair successfully updated!");
        }

        public NozomiResult<string> Delete(long currencyPairId, string userId = null, bool hardDelete = false)
        {
            if (currencyPairId > 0)
            {
                var cpToDel = _context.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .SingleOrDefault(cp => cp.Id.Equals(currencyPairId));

                if (hardDelete && cpToDel != null)
                {
                    _context.GetRepository<CurrencyPair>().Delete(cpToDel);
                }
                else if (cpToDel != null && cpToDel.DeletedAt == null)
                {
                    cpToDel.DeletedAt = DateTime.UtcNow;
                    if (string.IsNullOrWhiteSpace(userId))
                        cpToDel.DeletedById = userId;
                    
                    _context.GetRepository<CurrencyPair>().Update(cpToDel);
                }
                else
                {
                    // User has attempted to delete a deleted entity.
                    return new NozomiResult<string>(NozomiResultType.Failed, "Delete failed, the currency pair does not exist");
                }

                _context.Commit(userId);

                return new NozomiResult<string>(NozomiResultType.Success, "CurrencyPair has been successfully deleted!");
            }

            return new NozomiResult<string>(NozomiResultType.Failed, "Delete failed.");
        }

        public IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false)
        {
            return !includeNested ? _context.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Include(cp => cp.Requests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Skip(index * 20)
                .Take(20) :
                _context.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Skip(index * 20)
                    .Take(20);
        }

        public IEnumerable<string> GetAllCurrencyPairUrls()
        {
            return _context.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Select(cp => cp.APIUrl)
                .ToList();
        }

        public long[][] GetCurrencySourceMappings()
        {
            return _context.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null)
                .Where(cp => cp.IsEnabled)
                .Include(cp => cp.Source)
                .Select(cp => new long[] {cp.Id, cp.Source.Id})
                .ToArray();
        }
    }
}