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
                    .Include(cs => cs.Currencies);
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
                        Currencies = s.Currencies
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

        public bool SourceExists(string abbrv)
        {
            return _unitOfWork.GetRepository<Source>()
                .Get(s => s.DeletedAt == null &&
                          s.Abbreviation.Equals(abbrv, StringComparison.InvariantCultureIgnoreCase))
                .Any();
        }

        public SourceResponse Get(long id)
        {
            return _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Where(s => s.DeletedAt == null && s.IsEnabled && s.Id.Equals(id))
                .Include(s => s.Currencies)
                    .ThenInclude(c => c.CurrencyType)
                .Include(s => s.CurrencyPairs)
                    .ThenInclude(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                .Select(s => new SourceResponse
                {
                    Abbreviation = s.Abbreviation,
                    Name = s.Name,
                    Currencies = s.Currencies
                        .Where(c => c.IsEnabled && c.DeletedAt == null)
                        .Select(c => new CurrencyResponse
                        {
                            Id = c.Id,
                            CurrencyTypeId = c.CurrencyTypeId,
                            CurrencyType = c.CurrencyType.Name,
                            Abbrv = c.Abbrv,
                            Name = c.Name
                        })
                        .ToList()
                })
                .SingleOrDefault();
        }

        public SourceResponse Get(string abbreviation)
        {
            return _unitOfWork.GetRepository<Source>()
                .GetQueryable()
                .Where(s => s.DeletedAt == null && s.IsEnabled && s.Abbreviation.Equals(abbreviation))
                .Include(s => s.Currencies)
                .ThenInclude(c => c.CurrencyType)
                .Include(s => s.CurrencyPairs)
                .ThenInclude(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Select(s => new SourceResponse
                {
                    Abbreviation = s.Abbreviation,
                    Name = s.Name,
                    Currencies = s.Currencies
                        .Where(c => c.IsEnabled && c.DeletedAt == null)
                        .Select(c => new CurrencyResponse
                        {
                            Id = c.Id,
                            CurrencyTypeId = c.CurrencyTypeId,
                            CurrencyType = c.CurrencyType.Name,
                            Abbrv = c.Abbrv,
                            Name = c.Name
                        })
                        .ToList()
                })
                .SingleOrDefault();
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