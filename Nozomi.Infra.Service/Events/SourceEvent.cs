using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class SourceEvent : BaseEvent<SourceEvent, NozomiDbContext>, ISourceEvent
    {
        public SourceEvent(ILogger<SourceEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }
        
        public IEnumerable<Source> GetAllActive(bool countPairs = false, bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Where(s => s.DeletedAt == null && s.IsEnabled);

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
            
            #if DEBUG
            var testCol = query.ToList();
            #endif

            return query;
        }
        
        // Get all including disabled sources.
        public IEnumerable<Source> GetAll(bool countPairs = false, bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Source>()
                .GetQueryable();

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
            
#if DEBUG
            var testCol = query.ToList();
#endif

            return query;
        }

        public IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false)
        {
            if (includeNested) {
                return _unitOfWork.GetRepository<Source>()
                    .GetQueryable()
                    .Where(cs => cs.DeletedAt == null)
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

        public bool SourceExists(string abbrv)
        {
            return _unitOfWork.GetRepository<Source>()
                .Get(s => s.DeletedAt == null &&
                          s.Abbreviation.Equals(abbrv, StringComparison.InvariantCultureIgnoreCase))
                .Any();
        }

        public XSourceResponse Get(long id)
        {
            return _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Where(s => s.DeletedAt == null && s.IsEnabled && s.Id.Equals(id))
                .Include(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency)
                        .ThenInclude(c => c.CurrencyType)
                .Select(s => new XSourceResponse
                {
                    Abbreviation = s.Abbreviation,
                    Name = s.Name,
                    Currencies = s.SourceCurrencies
                        .Where(c => c.IsEnabled && c.DeletedAt == null)
                        .Select(c => new CurrencyResponse
                        {
                            Id = c.Id,
                            CurrencyTypeId = c.Currency.CurrencyTypeId,
                            CurrencyType = c.Currency.CurrencyType.Name,
                            Abbrv = c.Currency.Abbreviation,
                            Name = c.Currency.Name
                        })
                        .ToList()
                })
                .SingleOrDefault();
        }

        public XSourceResponse Get(string abbreviation)
        {
            return _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Where(s => s.DeletedAt == null && s.IsEnabled && s.Abbreviation.Equals(abbreviation))
                .Include(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                .ThenInclude(c => c.CurrencyType)
                .Select(s => new XSourceResponse
                {
                    Abbreviation = s.Abbreviation,
                    Name = s.Name,
                    Currencies = s.SourceCurrencies
                        .Where(c => c.IsEnabled && c.DeletedAt == null)
                        .Select(c => new CurrencyResponse
                        {
                            Id = c.Id,
                            CurrencyTypeId = c.Currency.CurrencyTypeId,
                            CurrencyType = c.Currency.CurrencyType.Name,
                            Abbrv = c.Currency.Abbreviation,
                            Name = c.Currency.Name
                        })
                        .ToList()
                })
                .SingleOrDefault();
        }
    }
}