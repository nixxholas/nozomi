using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class SourceEvent : BaseEvent<SourceEvent, NozomiDbContext>, ISourceEvent
    {
        public SourceEvent(ILogger<SourceEvent> logger, NozomiDbContext unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        public Source GetByGuid(string guid, bool filterActive = false)
        {
            if (string.IsNullOrEmpty(guid) || string.IsNullOrWhiteSpace(guid))
                return null;

            var query = _context.Sources.AsNoTracking();

            if (filterActive)
                query = query.Where(s => s.DeletedAt == null && s.IsEnabled);

            return query
                .SingleOrDefault(s => s.Guid.ToString().Equals(guid));
        }

        public IEnumerable<Nozomi.Data.ViewModels.Source.SourceViewModel> GetAll()
        {
            return _context.Sources.AsNoTracking()
                .Where(s => s.DeletedAt == null && s.IsEnabled)
                .Include(s => s.SourceType)
                .Select(s => new Nozomi.Data.ViewModels.Source.SourceViewModel
                {
                    Guid = s.Guid,
                    Abbreviation = s.Abbreviation,
                    ApiDocsUrl = s.APIDocsURL,
                    Name = s.Name,
                    SourceTypeGuid = s.SourceType.Guid
                });
        }

        public IEnumerable<Source> GetAllActive(bool countPairs = false, bool includeNested = false)
        {
            var query = _context.Sources.Where(s => s.DeletedAt == null && s.IsEnabled);

            if (countPairs)
            {
                query = query
                    .Include(s => s.CurrencyPairs);
            }

            if (includeNested)
            {
                query = query
                    .Include(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency)
                    .Include(s => s.CurrencyPairs)
                    .ThenInclude(cp => cp.Source)
                    .ThenInclude(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency);
            }

            return query.ToList();
        }
        
        public IEnumerable<Source> GetAllNonDeleted(bool countPairs = false, bool includeNested = false)
        {
            var query = _context.Sources.Where(s => s.DeletedAt == null);

            if (countPairs)
            {
                query = query
                    .Include(s => s.CurrencyPairs);
            }

            if (includeNested)
            {
                query = query
                    .Include(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency)
                    .Include(s => s.CurrencyPairs)
                    .ThenInclude(cp => cp.Source)
                    .ThenInclude(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency);
            }
            
            return query;
        }
        
        // Get all including disabled sources.
//        public IEnumerable<Source> GetAll(bool countPairs = false, bool includeNested = false)
//        {
//            var query = _context.GetRepository<Source>()
//                .GetQueryable();
//
//            if (countPairs)
//            {
//                query = query
//                    .Include(s => s.CurrencyPairs);
//            }
//
//            if (includeNested)
//            {
//                query = query
//                    .Include(s => s.SourceCurrencies)
//                    .ThenInclude(sc => sc.Currency)
//                    .Include(s => s.CurrencyPairs)
//                    .ThenInclude(cp => cp.Source)
//                    .ThenInclude(s => s.SourceCurrencies)
//                    .ThenInclude(sc => sc.Currency);
//            }
//
//            return query;
//        }

        public IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false)
        {
            if (includeNested) {
                return _context.Sources.Where(cs => cs.DeletedAt == null)
                    .Where(cs => cs.IsEnabled)
                    .Include(cs => cs.SourceCurrencies)
                    .Include(cs => cs.CurrencyPairs)
                    .Select(cs => new
                    {
                        id = cs.Id, 
                        abbrv = cs.Abbreviation,
                        name = cs.Name,
                        currencies = cs.SourceCurrencies,
                        currencyPairs = cs.CurrencyPairs
                    });
            } else {
                return _context.Sources.Where(cs => cs.DeletedAt == null)
                    .Where(cs => cs.IsEnabled)
                    .Select(cs => new
                    {
                        id = cs.Id, 
                        abbrv = cs.Abbreviation,
                        name = cs.Name
                    });
            }
        }

        public bool Exists(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
                return false;
            
            return _context.Sources
                .Any(s => s.DeletedAt == null
                          && s.Guid.ToString().Equals(guid));
        }

        public bool AbbreviationIsUsed(string abbrv)
        {
            return _context.Sources
                .Any(s => s.DeletedAt == null &&
                          s.Abbreviation.Equals(abbrv.ToUpper()));
        }

        public IEnumerable<Source> GetAllCurrencySourceOptions(IEnumerable<CurrencySource> currencySources)
        {
            IEnumerable<Source> sources = currencySources.Select(cs => cs.Source).ToList();
            
            var query = _context.Sources.Where(s => s.DeletedAt == null && s.IsEnabled
                            && sources.All(x => x.Id != s.Id))
                .ToList();
            
            return query;

        }

        public IEnumerable<Source> GetCurrencySources(string slug, int page = 0)
        {            
            return _context.CurrencySources.AsNoTracking()
                .Where(cs => cs.DeletedAt == null && cs.IsEnabled)
                .Include(cs => cs.Currency)
                .Where(cs => cs.Currency.Slug.Equals(slug))
                .Include(cs => cs.Source)
                .Skip(page * 20)
                .Take(20)
                .Select(cs => cs.Source);
        }
    }
}