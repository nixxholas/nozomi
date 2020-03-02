using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL.Helpers.Enumerable;
using Nozomi.Base.BCL.Helpers.Native.Numerals;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using SourceViewModel = Nozomi.Data.ViewModels.Source.SourceViewModel;

namespace Nozomi.Service.Events
{
    public class CurrencyEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, ICurrencyEvent
    {
        public CurrencyEvent(ILogger<CurrencyEvent> logger, NozomiDbContext unitOfWork)
            : base(logger, unitOfWork)
        {
        }

        public bool Exists(string slug)
        {
            return !string.IsNullOrWhiteSpace(slug) && _context.Currencies.AsNoTracking()
                       .Any(c => c.Slug.Equals(slug));
        }

        public CurrencyViewModel Get(string slug)
        {
            return _context.Currencies.AsNoTracking()
                .Where(c => c.Slug.Equals(slug) && c.DeletedAt == null)
                .Include(c => c.AnalysedComponents)
                .Include(c => c.CurrencyType)
                .Select(c => new CurrencyViewModel
                {
                    CurrencyTypeGuid = c.CurrencyType.Guid,
                    Abbreviation = c.Abbreviation,
                    Slug = c.Slug,
                    Name = c.Name,
                    LogoPath = c.LogoPath,
                    Description = c.Description,
                    Denominations = c.Denominations,
                    DenominationName = c.DenominationName,
                    Components = c.AnalysedComponents
                        .Where(ac => ac.IsEnabled && ac.DeletedAt == null)
                        .Select(ac => new AnalysedComponentViewModel
                        {
                            Guid = ac.Guid,
                            IsDenominated = ac.IsDenominated,
                            Type = ac.ComponentType,
                            UiFormatting = ac.UIFormatting,
                            Value = ac.Value
                        })
                })
                .SingleOrDefault();
        }

        public IEnumerable<BaseCurrencyViewModel> All(string slug = null)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .Select(c => new BaseCurrencyViewModel
                    {
                        Abbreviation = c.Abbreviation,
                        Name = c.Name,
                        Slug = c.Slug
                    });

            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled
                                                && c.Slug.Contains(slug))
                .Select(c => new BaseCurrencyViewModel
                {
                    Abbreviation = c.Abbreviation,
                    Name = c.Name,
                    Slug = c.Slug
                });
        }

        public IEnumerable<CurrencyViewModel> All(string currencyType = "CRYPTO", int itemsPerIndex = 20, int index = 0,
            ICollection<ComponentType> typesToTake = null, ICollection<ComponentType> typesToDeepen = null)
        {
            if (itemsPerIndex <= 0 || itemsPerIndex > 100)
                itemsPerIndex = 20;

            if (index < 0)
                index = 0;

            if (string.IsNullOrWhiteSpace(currencyType))
                throw new ArgumentNullException("Parameter 'currencyType' is supposed to contain a valid string.");

            var query = _context.Currencies.AsNoTracking()
                .Include(c => c.CurrencyType)
                .Where(c => c.DeletedAt == null && c.IsEnabled
                                                && c.CurrencyType.TypeShortForm.Equals(currencyType,
                                                    StringComparison.InvariantCultureIgnoreCase));

            if (!query.Any())
                return Enumerable.Empty<CurrencyViewModel>();

            query = query.Include(c => c.Requests)
                .ThenInclude(r => r.RequestComponents)
                .Skip(itemsPerIndex * index)
                .Take(itemsPerIndex);

            if (typesToTake != null && typesToTake.Any() && typesToDeepen != null && typesToDeepen.Any())
                return query
                    .Select(c => new CurrencyViewModel
                    {
                        CurrencyTypeGuid = c.CurrencyType.Guid,
                        Abbreviation = c.Abbreviation,
                        Slug = c.Slug,
                        Name = c.Name,
                        LogoPath = c.LogoPath,
                        Description = c.Description,
                        Denominations = c.Denominations,
                        DenominationName = c.DenominationName,
                        RawComponents = c.Requests.SelectMany(r => r.RequestComponents
                                .Where(rc => rc.DeletedAt == null && rc.IsEnabled))
                            .Where(rc => typesToTake.Contains(rc.ComponentType))
                            .Select(rc => new ComponentViewModel
                            {
                                Type = rc.ComponentTypeId,
                                Guid = rc.Guid,
                                IsDenominated = rc.IsDenominated,
                                History = typesToDeepen.Contains(rc.ComponentType)
                                    ? rc.RcdHistoricItems
                                        .Where(e => e.DeletedAt == null && e.IsEnabled)
                                        .OrderByDescending(e => e.HistoricDateTime)
                                        .Select(e => new ComponentHistoricItemViewModel()
                                        {
                                            Timestamp = e.HistoricDateTime,
                                            Value = e.Value
                                        })
                                    : null
                            })
                    });
            else if (typesToTake != null && typesToTake.Any())
                return query
                    .Select(c => new CurrencyViewModel
                    {
                        CurrencyTypeGuid = c.CurrencyType.Guid,
                        Abbreviation = c.Abbreviation,
                        Slug = c.Slug,
                        Name = c.Name,
                        LogoPath = c.LogoPath,
                        Description = c.Description,
                        Denominations = c.Denominations,
                        DenominationName = c.DenominationName,
                        RawComponents = c.Requests.SelectMany(r => r.RequestComponents
                                .Where(rc => rc.DeletedAt == null && rc.IsEnabled))
                            .Where(rc => typesToTake.Contains(rc.ComponentType))
                            .Select(rc => new ComponentViewModel
                            {
                                Type = rc.ComponentTypeId,
                                Guid = rc.Guid,
                                IsDenominated = rc.IsDenominated
                            })
                    });

            return query
                .Select(c => new CurrencyViewModel
                {
                    CurrencyTypeGuid = c.CurrencyType.Guid,
                    Abbreviation = c.Abbreviation,
                    Slug = c.Slug,
                    Name = c.Name,
                    LogoPath = c.LogoPath,
                    Description = c.Description,
                    Denominations = c.Denominations,
                    DenominationName = c.DenominationName
                });
        }

        public IEnumerable<CurrencyViewModel> All(string currencyType = "CRYPTO", int itemsPerIndex = 20, int index = 0,
            CurrencySortingEnum currencySortingEnum = CurrencySortingEnum.None,
            AnalysedComponentType sortType = AnalysedComponentType.Unknown, bool orderDescending = true,
            ICollection<AnalysedComponentType> typesToTake = null,
            ICollection<AnalysedComponentType> typesToDeepen = null)
        {
            if (itemsPerIndex <= 0 || itemsPerIndex > 100)
                itemsPerIndex = 20;

            if (index < 0)
                index = 0;

            if (string.IsNullOrWhiteSpace(currencyType))
                throw new ArgumentNullException("Parameter 'currencyType' is supposed to contain a valid string.");

            var query = _context.Currencies.AsNoTracking()
                .Include(c => c.CurrencyType)
                .Include(c => c.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .Where(c => c.DeletedAt == null && c.IsEnabled && c.CurrencyType != null
                            && c.CurrencyType.TypeShortForm
                                .Equals(currencyType.ToUpper()));

            if (!query.Any())
                // https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.empty?view=netframework-4.8#examples
                return Enumerable.Empty<CurrencyViewModel>();
            
            switch (currencySortingEnum) // Ignore case sensitivity
            {
                case CurrencySortingEnum.Abbreviation:
                    query = !orderDescending
                        ? query.OrderBy(c => c.Abbreviation)
                        : query.OrderByDescending(c => c.Abbreviation);
                    break;
                case CurrencySortingEnum.Slug:
                    query = !orderDescending 
                        ? query.OrderBy(c => c.Slug) 
                        : query.OrderByDescending(c => c.Slug);
                    break;
                case CurrencySortingEnum.Type:
                    query = !orderDescending
                        ? query
                            .Include(c => c.CurrencyType)
                            .OrderBy(c => c.CurrencyType.Name)
                        : query
                            .Include(c => c.CurrencyType)
                            .OrderByDescending(c => c.CurrencyType.Name);
                    break;
                case CurrencySortingEnum.Name: // Handle all cases.
                    query = !orderDescending 
                        ? query.OrderBy(c => c.Name) 
                        : query.OrderByDescending(c => c.Name);
                    break;
            }

            if (orderDescending && sortType != AnalysedComponentType.Unknown)
            {
                // Order by the market cap
                var descendingQuery = query
                    .OrderByDescending(c => c.AnalysedComponents
                        .Where(ac => ac.DeletedAt == null && ac.IsEnabled
                                                          && ac.ComponentType.Equals(sortType))
                        .Select(ac => ac.Value)
                        .FirstOrDefault()
                    )
                    .Skip(itemsPerIndex * index)
                    .Take(itemsPerIndex);

                if (typesToTake != null && typesToTake.Any() && typesToDeepen != null && typesToDeepen.Any())
                    return descendingQuery
                        .Select(c => new CurrencyViewModel
                        {
                            CurrencyTypeGuid = c.CurrencyType.Guid,
                            Abbreviation = c.Abbreviation,
                            Slug = c.Slug,
                            Name = c.Name,
                            LogoPath = c.LogoPath,
                            Description = c.Description,
                            Denominations = c.Denominations,
                            DenominationName = c.DenominationName,
                            Components = c.AnalysedComponents
                                .Where(ac => typesToTake.Contains(ac.ComponentType))
                                .Select(ac => new AnalysedComponentViewModel
                                {
                                    Guid = ac.Guid,
                                    Type = ac.ComponentType,
                                    UiFormatting = ac.UIFormatting,
                                    Value = ac.Value,
                                    IsDenominated = ac.IsDenominated,
                                    History = typesToDeepen.Contains(ac.ComponentType)
                                        ? ac.AnalysedHistoricItems
                                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                                            .OrderByDescending(ahi => ahi.HistoricDateTime)
                                            .Select(ahi => new AnalysedHistoricItemViewModel
                                            {
                                                Timestamp = ahi.HistoricDateTime,
                                                Value = ahi.Value
                                            })
                                        : null
                                })
                        });
                else if (typesToTake != null && typesToTake.Any())
                    return descendingQuery
                        .Select(c => new CurrencyViewModel
                        {
                            CurrencyTypeGuid = c.CurrencyType.Guid,
                            Abbreviation = c.Abbreviation,
                            Slug = c.Slug,
                            Name = c.Name,
                            LogoPath = c.LogoPath,
                            Description = c.Description,
                            Denominations = c.Denominations,
                            DenominationName = c.DenominationName,
                            Components = c.AnalysedComponents
                                .Where(ac => typesToTake.Contains(ac.ComponentType))
                                .Select(ac => new AnalysedComponentViewModel
                                {
                                    Guid = ac.Guid,
                                    Type = ac.ComponentType,
                                    UiFormatting = ac.UIFormatting,
                                    Value = ac.Value,
                                    IsDenominated = ac.IsDenominated
                                })
                        });

                return descendingQuery
                    .Select(c => new CurrencyViewModel
                    {
                        CurrencyTypeGuid = c.CurrencyType.Guid,
                        Abbreviation = c.Abbreviation,
                        Slug = c.Slug,
                        Name = c.Name,
                        LogoPath = c.LogoPath,
                        Description = c.Description,
                        Denominations = c.Denominations,
                        DenominationName = c.DenominationName
                    });
            }
            else if (sortType != AnalysedComponentType.Unknown)
            {
                var ascendingQuery = query
                    .OrderBy(c => c.AnalysedComponents
                        .Where(ac => ac.DeletedAt == null && ac.IsEnabled
                                                          && ac.ComponentType.Equals(sortType))
                        .Select(ac => ac.Value)
                        .FirstOrDefault()
                    )
                    .Skip(itemsPerIndex * index)
                    .Take(itemsPerIndex);

                if (typesToTake != null && typesToTake.Any() && typesToDeepen != null && typesToDeepen.Any())
                    return ascendingQuery
                        .Select(c => new CurrencyViewModel
                        {
                            CurrencyTypeGuid = c.CurrencyType.Guid,
                            Abbreviation = c.Abbreviation,
                            Slug = c.Slug,
                            Name = c.Name,
                            LogoPath = c.LogoPath,
                            Description = c.Description,
                            Denominations = c.Denominations,
                            DenominationName = c.DenominationName,
                            Components = c.AnalysedComponents
                                .Where(ac => typesToTake.Contains(ac.ComponentType))
                                .Select(ac => new AnalysedComponentViewModel
                                {
                                    Guid = ac.Guid,
                                    Type = ac.ComponentType,
                                    UiFormatting = ac.UIFormatting,
                                    Value = ac.Value,
                                    IsDenominated = ac.IsDenominated,
                                    History = typesToDeepen.Contains(ac.ComponentType)
                                        ? ac.AnalysedHistoricItems
                                            .Where(ahi => ahi.DeletedAt == null && ahi.IsEnabled)
                                            .OrderByDescending(ahi => ahi.HistoricDateTime)
                                            .Select(ahi => new AnalysedHistoricItemViewModel
                                            {
                                                Timestamp = ahi.HistoricDateTime,
                                                Value = ahi.Value
                                            })
                                        : null
                                })
                        });
                else if (typesToTake != null && typesToTake.Any())
                    return ascendingQuery
                        .Select(c => new CurrencyViewModel
                        {
                            CurrencyTypeGuid = c.CurrencyType.Guid,
                            Abbreviation = c.Abbreviation,
                            Slug = c.Slug,
                            Name = c.Name,
                            LogoPath = c.LogoPath,
                            Description = c.Description,
                            Denominations = c.Denominations,
                            DenominationName = c.DenominationName,
                            Components = c.AnalysedComponents
                                .Where(ac => typesToTake.Contains(ac.ComponentType))
                                .Select(ac => new AnalysedComponentViewModel
                                {
                                    Guid = ac.Guid,
                                    Type = ac.ComponentType,
                                    UiFormatting = ac.UIFormatting,
                                    Value = ac.Value,
                                    IsDenominated = ac.IsDenominated
                                })
                        });

                return ascendingQuery
                    .Select(c => new CurrencyViewModel
                    {
                        CurrencyTypeGuid = c.CurrencyType.Guid,
                        Abbreviation = c.Abbreviation,
                        Slug = c.Slug,
                        Name = c.Name,
                        LogoPath = c.LogoPath,
                        Description = c.Description,
                        Denominations = c.Denominations,
                        DenominationName = c.DenominationName
                    });
            }

            return query
                .Skip(itemsPerIndex * index)
                .Take(itemsPerIndex)
                .Select(c => new CurrencyViewModel
                {
                    CurrencyTypeGuid = c.CurrencyType.Guid,
                    Abbreviation = c.Abbreviation,
                    Slug = c.Slug,
                    Name = c.Name,
                    LogoPath = c.LogoPath,
                    Description = c.Description,
                    Denominations = c.Denominations,
                    DenominationName = c.DenominationName
                });
        }

        public Currency Get(long id, bool track = false)
        {
            var query = _context.Currencies.AsNoTracking();

            if (track)
                query = query.AsTracking()
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.Requests)
                    .ThenInclude(cr => cr.RequestComponents);

            return query
                .SingleOrDefault(c => c.Id.Equals(id));
        }

        /// <summary>
        /// Public API for obtaining data specific to a currency with its abbreviation.
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public Currency GetCurrencyByAbbreviation(string abbreviation, bool track = false)
        {
            var query = _context.Currencies.AsNoTracking();

            if (track)
            {
                query = query.Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.Requests)
                    .ThenInclude(cr => cr.RequestComponents);
            }

            return query
                .SingleOrDefault(c => c.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase));
        }

        public Currency GetBySlug(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                throw new ArgumentNullException("Invalid slug.");

            return _context.Currencies.AsNoTracking()
                .SingleOrDefault(c => c.DeletedAt == null && c.IsEnabled
                                                          && c.Slug.Equals(slug));
        }

        public decimal GetCirculatingSupply(AnalysedComponent analysedComponent)
        {
            var circulatingSupplyEnum = GenericComponentType.CirculatingSupply;
            // If its a currency-based ac
            if (analysedComponent.CurrencyId != null && analysedComponent.CurrencyId > 0)
            {
                // Obtain the currency that is required
                var curr = _context.Currencies.AsNoTracking()
                    .SingleOrDefault(c => c.Id.Equals(analysedComponent.CurrencyId)
                                          && c.IsEnabled && c.DeletedAt == null);

                // Safetynet
                if (curr == null)
                {
                    return decimal.MinusOne;
                }

                // Denomination safetynet
                if (curr.Denominations <= 0)
                {
                    curr.Denominations = 1; // Neutraliser
                }

                // Then, we obtain the circulating supply.

                // TODO: Validate with multiple sources.

                var reqComp = _context.Requests.AsNoTracking()
                    .Where(cr => cr.DeletedAt == null && cr.IsEnabled
                                                      && cr.CurrencyId.Equals(curr.Id))
                    .Include(cp => cp.Currency)
                    .Include(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RcdHistoricItems)
                    // Obtain only the circulating supply
                    .SelectMany(cpr => cpr.RequestComponents
                        .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                          && rc.ComponentTypeId.Equals((long)circulatingSupplyEnum)
                                                          && rc.RcdHistoricItems.Count > 0))
                    .FirstOrDefault();

                return reqComp != null
                    ? decimal.Parse(reqComp.RcdHistoricItems
                          .OrderByDescending(e => e.HistoricDateTime)
                          .FirstOrDefault()
                          ?.Value ?? "0") /
                      (reqComp.IsDenominated ? (decimal) Math.Pow(10, curr.Denominations) : decimal.One)
                    : decimal.Zero;
            }
            else if (analysedComponent.CurrencyPairId != null && analysedComponent.CurrencyPairId > 0)
                // It means that this is a currency pair 
            {
                // Obtain the main ticker first
                var mainTicker = _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .ThenInclude(s => s.CurrencyPairs)
                    .Where(c => c.CurrencySources
                        .Any(cs => cs.Source.CurrencyPairs
                            .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                            .Any(cp => cp.Id.Equals(analysedComponent.CurrencyPairId))));

                if (!mainTicker.Any())
                    return decimal.MinusOne;

                // We need to make sure that no null exceptions will appear here
                return mainTicker
                    .Include(c => c.Requests)
                    .ThenInclude(r => r.RequestComponents)
                    .ThenInclude(rc => rc.RcdHistoricItems)
                    .SelectMany(c => c.Requests
                        .Where(r => r.DeletedAt == null && r.IsEnabled))
                    .SelectMany(r => r.RequestComponents
                        .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                          && rc.ComponentType.Equals(circulatingSupplyEnum)
                                                          && rc.RcdHistoricItems.Count > 0))
                    .Select(rc => rc.RcdHistoricItems
                        .OrderByDescending(e => e.HistoricDateTime))
                    .AsEnumerable()
                    .Where(e => 
                        NumberHelper.IsNumericDecimal(e.FirstOrDefault()?.Value))
                    .Select(e => decimal.Parse(e.FirstOrDefault()?.Value))
                    .DefaultIfEmpty(decimal.MinusOne) // Give it -1
                    .FirstOrDefault();
            }

            return decimal.MinusOne;
        }

        public long Count(bool ignoreDeleted = false, bool ignoreDisabled = false)
        {
            var query = _context.Currencies.AsNoTracking();

            if (!ignoreDeleted)
                query = query.Where(c => c.DeletedAt == null);

            if (!ignoreDisabled)
                query = query.Where(c => c.IsEnabled);

            return query.LongCount();
        }

        public long GetCountByType(string typeShortForm)
        {
            if (string.IsNullOrEmpty(typeShortForm) || string.IsNullOrWhiteSpace(typeShortForm))
                return _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .LongCount();

            var query = _context.CurrencyTypes.AsNoTracking()
                .Where(ct => ct.TypeShortForm.ToLower().Equals(typeShortForm.ToLower()));

            if (!query.Any())
                return 0;

            return query
                .Include(ct => ct.Currencies)
                .ThenInclude(c => c.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .Include(ct => ct.Currencies)
                .ThenInclude(c => c.Requests)
                .ThenInclude(r => r.RequestComponents)
                .SelectMany(ct => ct.Currencies
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .OrderBy(c => c.Id))
                .LongCount();
        }

        public ICollection<Currency> GetAll(bool includeNested = false)
        {
            var query = _context.Currencies.AsNoTracking();

            if (includeNested)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.Requests)
                    .Include(c => c.CurrencyProperties);
            }

            return query.ToList();
        }

        public ICollection<Currency> GetAllNonDeleted(bool includeNested = false)
        {
            var query = _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null);

            if (includeNested)
            {
                query = query
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencyType)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.Requests)
                    .Include(c => c.CurrencyProperties);
            }

            return query.ToList();
        }

        public ICollection<CurrencyDTO> GetAllDTO()
        {
            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .Include(c => c.CurrencySources)
                .Select(c => new CurrencyDTO
                {
                    Id = c.Id,
                    CurrencyType = c.CurrencyType,
                    LogoPath = c.LogoPath,
                    Abbreviation = c.Abbreviation,
                    SourceCount = c.CurrencySources.Count,
                    Slug = c.Slug,
                    Name = c.Name,
                    Description = c.Description,
                    DenominationName = c.DenominationName,
                    IsEnabled = c.IsEnabled
                }).ToList();
        }

        public bool Any(CreateCurrency createCurrency)
        {
            if (createCurrency != null && createCurrency.IsValid())
            {
                return _context.Currencies
                    .Where(c => c.Abbreviation.Equals(createCurrency.Abbreviation)).Any();
            }

            return false;
        }

        public IEnumerable<Currency> GetAllActive(bool includeNested = false)
        {
            if (includeNested)
            {
                return _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.Requests)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.CurrencyProperties)
                    .Include(c => c.CurrencyType);
            }
            else
            {
                return _context.Currencies
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled);
            }
        }

        /// <summary>
        /// Gets all active.
        /// 
        /// </summary>
        /// <param name="includeNested"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAllActiveObsc(bool includeNested = false)
        {
            if (includeNested)
            {
                return _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyType)
                    .Select(c => new
                    {
                        id = c.Id,
                        abbrv = c.Abbreviation,
                        CurrencyType =
                            new
                            {
                                name = c.CurrencyType.Name,
                                typeShortForm = c.CurrencyType.TypeShortForm
                            },
                        name = c.Name
                    });
            }
            else
            {
                return _context.Currencies
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Select(c => new
                    {
                        id = c.Id,
                        abbrv = c.Abbreviation,
                        name = c.Name
                    });
            }
        }

        /// <summary>
        /// Gets all active distinctively.
        /// 
        /// This is to prevent duplicates for displaying purposes. The user would normally
        /// be prompted after selecting a choice from this result before creating
        /// a final decision.
        /// </summary>
        /// <returns>The all active distinct.</returns>
        /// <param name="includeNested">If set to <c>true</c> include nested.</param>
        public IEnumerable<dynamic> GetAllActiveDistinctObsc(bool includeNested = false)
        {
            if (includeNested)
            {
                return _context.Currencies
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.CurrencyType)
                    .DistinctBy(c => c.Abbreviation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbreviation,
                        CurrencyType =
                            new
                            {
                                name = c.CurrencyType.Name,
                                typeShortForm = c.CurrencyType.TypeShortForm
                            },
                        Name = c.Name
                    });
            }
            else
            {
                return _context.Currencies.AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .DistinctBy(c => c.Abbreviation)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbreviation,
                        Name = c.Name
                    });
            }
        }

        public ICollection<AnalysedComponent> GetTickerPairComponents(long currencyId, bool ensureValid = false,
            int index = 0, bool track = false,
            Expression<Func<AnalysedComponent, bool>> predicate = null,
            Func<AnalysedComponent, bool> clientPredicate = null, int historicItemIndex = 0)
        {
            // obtain the currency
            var mainCurrency = _context.Currencies.AsNoTracking()
                .SingleOrDefault(c => c.DeletedAt == null && c.IsEnabled && c.Id.Equals(currencyId));

            var components = _context.AnalysedComponents
                .AsNoTracking()
                .Where(ac => ac.CurrencyPairId != null && ac.CurrencyPairId > 0);

            if (ensureValid)
                components = components.Where(ac => ac.DeletedAt == null && ac.IsEnabled);

            components = components
                .Include(ac => ac.CurrencyPair)
                .ThenInclude(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .Where(ac => ac.CurrencyPair != null // Make sure the currency pair is not null
                             && ac.CurrencyPair.MainTicker.Equals(mainCurrency
                                 .Abbreviation) // Make sure the main ticker is the currency
                             && ac.CurrencyPair.Source != null &&
                             ac.CurrencyPair.Source.SourceCurrencies !=
                             null // Make sure the source currency is not empty
                             && ac.CurrencyPair.Source.SourceCurrencies // Second layer check.
                                 .Any(sc => sc.DeletedAt == null && sc.IsEnabled && sc.CurrencyId.Equals(currencyId)));

            if (track)
                components = components.Include(ac => ac.AnalysedHistoricItems);

            if (predicate != null)
                components = components.Where(predicate);

            if (clientPredicate != null)
                return components
                    .OrderBy(ac => ac.Id)
                    .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .AsEnumerable()
                    .Where(clientPredicate)
                    .Select(ac => new AnalysedComponent(ac, index,
                        NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                    .ToList();

            return components
                .OrderBy(ac => ac.Id)
                .Skip(index * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                .AsEnumerable()
                .Select(ac => new AnalysedComponent(ac, index,
                    NozomiServiceConstants.AnalysedComponentTakeoutLimit))
                .ToList();
        }

        public ICollection<string> ListAllSlugs()
        {
            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .Select(c => c.Slug)
                .ToList();
        }

        public IEnumerable<CurrencyViewModel> ListAll(int page = 0, int itemsPerPage = 50,
            string currencyTypeName = null, bool orderAscending = true, 
            CurrencySortingEnum orderingParam = CurrencySortingEnum.None)
        {
            var query = _context.Currencies.AsNoTracking()
                .Where(c => c.IsEnabled && c.DeletedAt == null && c.CurrencyTypeId > 0);

            if (!string.IsNullOrEmpty(currencyTypeName))
                query = query
                    .Include(c => c.CurrencyType)
                    .Where(c => c.CurrencyType.DeletedAt == null && c.CurrencyType.IsEnabled &&
                                c.CurrencyType.Name.ToUpper().Equals(currencyTypeName.ToUpper()));

            switch (orderingParam) // Ignore case sensitivity
            {
                case CurrencySortingEnum.Abbreviation:
                    query = orderAscending
                        ? query.OrderBy(c => c.Abbreviation)
                        : query.OrderByDescending(c => c.Abbreviation);
                    break;
                case CurrencySortingEnum.Slug:
                    query = orderAscending ? query.OrderBy(c => c.Slug) : query.OrderByDescending(c => c.Slug);
                    break;
                case CurrencySortingEnum.Type:
                    query = orderAscending
                        ? query
                            .Include(c => c.CurrencyType)
                            .OrderBy(c => c.CurrencyType.Name)
                        : query
                            .Include(c => c.CurrencyType)
                            .OrderByDescending(c => c.CurrencyType.Name);
                    break;
                default: // Handle all cases.
                    query = orderAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
            }

            return query
                // .OrderBy(orderingParam, orderAscending) // TODO: Make use of LinqExtensions again
                .Skip(page * itemsPerPage)
                .Take(itemsPerPage)
                .Include(c => c.CurrencyType)
                .Select(c => new CurrencyViewModel
                {
                    Abbreviation = c.Abbreviation,
                    CurrencyTypeGuid = c.CurrencyType.Guid,
                    DenominationName = c.DenominationName,
                    Denominations = c.Denominations,
                    Description = c.Description,
                    LogoPath = c.LogoPath,
                    Name = c.Name,
                    Slug = c.Slug
                });
        }

        public IReadOnlyDictionary<string, long> ListAllMapped()
        {
            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .ToDictionary(c => c.Slug, c => c.Id);
        }

        public long SourceCount(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return long.MinValue;

            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled && c.Slug.Equals(slug))
                .Include(c => c.CurrencySources)
                .ThenInclude(cs => cs.Source)
                .SelectMany(c => c.CurrencySources
                    .Where(cs => cs.IsEnabled && cs.DeletedAt == null
                                              && cs.Source.DeletedAt == null && cs.Source.IsEnabled))
                .LongCount();
        }

        public IEnumerable<SourceViewModel> ListSources(string slug, int page = 0, int itemsPerPage = 50)
        {
            if (string.IsNullOrWhiteSpace(slug) || page < 0 || itemsPerPage < 1)
                throw new ArgumentOutOfRangeException("Invalid request parameters.");

            return _context.Currencies.AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled && c.Slug.Equals(slug))
                .Include(c => c.CurrencySources)
                .ThenInclude(cs => cs.Source)
                .SelectMany(c => c.CurrencySources
                    .Where(cs => cs.IsEnabled && cs.DeletedAt == null
                                              && cs.Source.DeletedAt == null && cs.Source.IsEnabled))
                .Select(cs => new SourceViewModel
                {
                    Guid = cs.Source.Guid,
                    Abbreviation = cs.Source.Abbreviation,
                    Name = cs.Source.Name,
                    ApiDocsUrl = cs.Source.APIDocsURL,
                    SourceTypeGuid = cs.Source.Guid
                });
        }
    }
}