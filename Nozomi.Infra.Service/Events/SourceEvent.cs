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
                .Include(cs => cs.CurrencyPairs)
                .Where(cs => cs.DeletedAt == null)
                .Where(cs => cs.IsEnabled);

            if (includeNested)
            {
                query = query
                    .Include(cs => cs.SourceCurrencies);
            }

            if (countPairs)
            {
                query = query
                    .Select(s => new Source
                    {
                        Id = s.Id,
                        Abbreviation = s.Abbreviation, 
                        Name = s.Name,
                        APIDocsURL = s.APIDocsURL,
                        PairCount = s.CurrencyPairs != null ? s.CurrencyPairs.Count : 0,
                        CurrencyPairs = s.CurrencyPairs,
                        SourceCurrencies = s.SourceCurrencies
                    });
            }

            return query;
        }
        
        // Get all including disabled sources.
        public IEnumerable<Source> GetAll(bool countPairs = false, bool includeNested = false)
        {
            var query = _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Include(cs => cs.CurrencyPairs)
                .Where(cs => cs.DeletedAt == null);

            if (includeNested)
            {
                query = query
                    .Include(cs => cs.SourceCurrencies);
            }

            if (countPairs)
            {
                query = query
                    .Select(s => new Source
                    {
                        Id = s.Id,
                        Abbreviation = s.Abbreviation, 
                        Name = s.Name,
                        APIDocsURL = s.APIDocsURL,
                        PairCount = s.CurrencyPairs != null ? s.CurrencyPairs.Count : 0,
                        CurrencyPairs = s.CurrencyPairs,
                        SourceCurrencies = s.SourceCurrencies,
                        IsEnabled = s.IsEnabled
                    });
            }

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
                // Extend towards Currency Pairs
                .Include(s => s.CurrencyPairs)
                .ThenInclude(cp => cp.CurrencyPairSourceCurrencies)
                // Extend towards Currencies
                .Include(s => s.SourceCurrencies)
                .ThenInclude(cs => cs.Currency)
                .Select(s => new XSourceResponse
                {
                    Abbreviation = s.Abbreviation,
                    Name = s.Name,
                    APIDocsURL = s.APIDocsURL,
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