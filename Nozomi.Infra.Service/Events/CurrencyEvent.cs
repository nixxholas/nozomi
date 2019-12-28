using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Base.Core;
using Nozomi.Base.Core.Extensions;
using Nozomi.Base.Core.Helpers.Enumerable;
using Nozomi.Base.Core.Helpers.Native.Numerals;
using Nozomi.Base.Core.Helpers.Native.Text;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.PartialCurrencyPair;
using Nozomi.Data.ResponseModels.Source;
using Nozomi.Data.ViewModels.AnalysedComponent;
using Nozomi.Data.ViewModels.AnalysedHistoricItem;
using Nozomi.Data.ViewModels.Component;
using Nozomi.Data.ViewModels.ComponentHistoricItem;
using Nozomi.Data.ViewModels.Currency;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;
using Component = Nozomi.Data.Models.Web.Component;
using SourceViewModel = Nozomi.Data.ViewModels.Source.SourceViewModel;

namespace Nozomi.Service.Events
{
    public class CurrencyEvent : BaseEvent<CurrencyEvent, NozomiDbContext>, ICurrencyEvent
    {
        private readonly ITickerEvent _tickerEvent;

        public CurrencyEvent(ILogger<CurrencyEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            ITickerEvent tickerEvent)
            : base(logger, unitOfWork)
        {
            _tickerEvent = tickerEvent;
        }

        public bool Exists(string slug)
        {
            return !string.IsNullOrWhiteSpace(slug) && _unitOfWork.GetRepository<Currency>()
                       .GetQueryable()
                       .AsNoTracking()
                       .Any(c => c.Slug.Equals(slug));
        }

        public CurrencyViewModel Get(string slug)
        {
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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
                return _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .Select(c => new BaseCurrencyViewModel
                    {
                        Abbreviation = c.Abbreviation,
                        Name = c.Name,
                        Slug = c.Slug
                    });

            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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

            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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
                                Type = rc.ComponentType,
                                Guid = rc.Guid,
                                Value = rc.Value,
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
                                Type = rc.ComponentType,
                                Guid = rc.Guid,
                                Value = rc.Value,
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

            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Include(c => c.CurrencyType)
                .Where(c => c.DeletedAt == null && c.IsEnabled && c.CurrencyType != null
                            && c.CurrencyType.TypeShortForm
                                .Equals(currencyType.ToUpper()))
                .Include(c => c.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems);

            if (!query.Any())
                // https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.empty?view=netframework-4.8#examples
                return Enumerable.Empty<CurrencyViewModel>();

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
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable();

            if (track)
            {
                query = query.AsTracking()
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.CurrencySources)
                    .ThenInclude(cs => cs.Source)
                    .Include(c => c.Requests)
                    .ThenInclude(cr => cr.RequestComponents);
            }

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
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

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

            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(c => c.DeletedAt == null && c.IsEnabled
                                                          && c.Slug.Equals(slug));
        }

        public decimal GetCirculatingSupply(AnalysedComponent analysedComponent)
        {
            var circulatingSupplyEnum = ComponentType.CirculatingSupply;
            // If its a currency-based ac
            if (analysedComponent.CurrencyId != null && analysedComponent.CurrencyId > 0)
            {
                // Obtain the currency that is required
                var curr = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
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

                var reqComp = _unitOfWork.GetRepository<Request>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cr => cr.DeletedAt == null && cr.IsEnabled
                                                      && cr.CurrencyId.Equals(curr.Id))
                    .Include(cp => cp.Currency)
                    .Include(cpr => cpr.RequestComponents)
                    // Obtain only the circulating supply
                    .SelectMany(cpr => cpr.RequestComponents
                        .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                          && rc.ComponentType.Equals(circulatingSupplyEnum)
                                                          && !string.IsNullOrEmpty(rc.Value)))
                    .FirstOrDefault();

                return reqComp != null
                    ? decimal.Parse(reqComp.Value ?? "0") /
                      (reqComp.IsDenominated ? (decimal) Math.Pow(10, curr.Denominations) : decimal.One)
                    : decimal.Zero;
            }
            else if (analysedComponent.CurrencyPairId != null && analysedComponent.CurrencyPairId > 0)
                // It means that this is a currency pair 
            {
                // Obtain the main ticker first
                var mainTicker = _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
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
                    .SelectMany(c => c.Requests
                        .Where(r => r.DeletedAt == null && r.IsEnabled))
                    .SelectMany(r => r.RequestComponents
                        .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                          && rc.ComponentType.Equals(circulatingSupplyEnum)))
                    .AsEnumerable()
                    .Where(rc => NumberHelper.IsNumericDecimal(rc.Value))
                    .Select(rc => decimal.Parse(rc.Value))
                    .DefaultIfEmpty(decimal.MinusOne) // Give it -1
                    .FirstOrDefault();
            }

            return decimal.MinusOne;
        }

        public long Count(bool ignoreDeleted = false, bool ignoreDisabled = false)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

            if (!ignoreDeleted)
                query = query.Where(c => c.DeletedAt == null);

            if (!ignoreDisabled)
                query = query.Where(c => c.IsEnabled);

            return query.LongCount();
        }

        public long GetCountByType(string typeShortForm)
        {
            if (string.IsNullOrEmpty(typeShortForm) || string.IsNullOrWhiteSpace(typeShortForm))
                return _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .LongCount();

            var query = _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
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
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking();

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
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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

        public ICollection<GeneralisedCurrencyResponse> GetAllDetailed(string typeShortForm = "CRYPTO",
            int index = 0, int countPerIndex = 20, int daysOfData = 7)
        {
            if (countPerIndex <= 0)
                countPerIndex = 20; // Defaulting checks

            if (daysOfData <= 0 || daysOfData >= 32)
                daysOfData = 7; // Defaulting checks

            var currencies = _unitOfWork.GetRepository<CurrencyType>()
                .GetQueryable()
                .AsNoTracking()
                .Where(ct => ct.TypeShortForm.Equals(typeShortForm, StringComparison.InvariantCultureIgnoreCase))
                .Include(ct => ct.Currencies)
                .ThenInclude(c => c.AnalysedComponents)
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .Include(ct => ct.Currencies)
                .ThenInclude(c => c.Requests)
                .ThenInclude(r => r.RequestComponents)
                .SelectMany(ct => ct.Currencies
                    .Where(c => c.DeletedAt == null && c.IsEnabled
//                                                    && c.AnalysedComponents.Any(ac =>
//                                                        ac.ComponentType.Equals(AnalysedComponentType.MarketCap)
//                                                        && !string.IsNullOrEmpty(ac.Value)
//                                                        && NumberHelper.IsNumericDecimal(ac.Value))
                    )
//                    .Where(c => c.AnalysedComponents
//                        .Any(ac => ac.DeletedAt == null && ac.IsEnabled))
//                    .OrderByDescending(c => decimal.Parse(c.AnalysedComponents
//                        .SingleOrDefault(ac => ac.ComponentType == AnalysedComponentType.MarketCap).Value))
                    .OrderBy(c => c.Id)
                    .Skip(index * countPerIndex)
                    .Take(countPerIndex)
                    .Select(c => new Currency
                    {
                        Id = c.Id,
                        CurrencyTypeId = c.CurrencyTypeId,
                        Abbreviation = c.Abbreviation,
                        Slug = c.Slug,
                        Name = c.Name,
                        Description = c.Description,
                        Denominations = c.Denominations,
                        DenominationName = c.DenominationName,
                        LogoPath = c.LogoPath,
                        AnalysedComponents = c.AnalysedComponents
                            .Where(ac => ac.DeletedAt == null && ac.IsEnabled)
                            .Select(ac => new AnalysedComponent(ac, index,
                                NozomiServiceConstants.AnalysedComponentTakeoutLimit,
                                ahi => ahi.DeletedAt == null && ahi.IsEnabled
                                                             && ahi.HistoricDateTime >=
                                                             DateTime.UtcNow.Subtract(
                                                                 TimeSpan.FromDays(daysOfData))
                                                             && NumberHelper.IsNumericDecimal(ahi.Value)))
                            .ToList(),
                        Requests = c.Requests
                            .Where(r => r.DeletedAt == null && r.IsEnabled)
                            .Select(r => new Request
                            {
                                Guid = r.Guid,
                                RequestComponents = r.RequestComponents
                                    .Where(rc => rc.DeletedAt == null && rc.IsEnabled)
                                    .ToList()
                            })
                            .ToList()
                    }))
                .ToList();

            var res = new List<GeneralisedCurrencyResponse>();

            foreach (var currency in currencies)
            {
                res.Add(new GeneralisedCurrencyResponse(currency,
                    _tickerEvent.GetCurrencyTickerPairs(currency.Abbreviation)));
            }

            return res.OrderByDescending(dcr => dcr.MarketCap).ToList();
        }

        public DetailedCurrencyResponse GetDetailedById(long currencyId,
            ICollection<AnalysedComponentType> componentTypes)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.Id.Equals(currencyId))
                .Include(cp => cp.AnalysedComponents
                    .Where(ac => componentTypes.Contains(ac.ComponentType)))
                .ThenInclude(ac => ac.AnalysedHistoricItems)
                .SingleOrDefault();

            if (query == null) return null;

            return new DetailedCurrencyResponse(query,
                _tickerEvent.GetCurrencyTickerPairs(query.Abbreviation));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="componentTypes"></param>
        /// <returns></returns>
        public DetailedCurrencyResponse GetDetailedBySlug(string slug,
            ICollection<ComponentType> componentTypes, ICollection<AnalysedComponentType> analysedComponentTypes,
            int componentTypesIndex = 0, int analysedComponentTypesIndex = 0)
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled
                                                && c.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                .Include(c => c.CurrencyProperties);

            if (!query.Any() || componentTypesIndex < 0 || analysedComponentTypesIndex < 0) return null;

            var analysedComponents = new List<AnalysedComponent>();
            if (analysedComponentTypes != null)
            {
                // https://github.com/aspnet/EntityFrameworkCore/issues/1833
//                query.AnalysedComponents = query.AnalysedComponents
//                    .Where(ac => analysedComponentTypes.Contains(ac.ComponentType))
//                    .ToList();
                analysedComponents = query
                    .Include(cp => cp.AnalysedComponents)
                    .ThenInclude(ac => ac.AnalysedHistoricItems)
                    .SelectMany(c => c.AnalysedComponents.Where(ac =>
                        analysedComponentTypes.Contains(ac.ComponentType)))
                    .Select(ac => new AnalysedComponent(ac, 0,
                        NozomiServiceConstants.AnalysedHistoricItemTakeoutLimit))
                    .OrderBy(ac => ac.Id)
                    .Skip(analysedComponentTypesIndex * NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.AnalysedComponentTakeoutLimit)
                    .ToList();
            }

            var requestComponents = new List<Component>();
            if (componentTypes != null)
            {
                requestComponents = query.SelectMany(c => c.Requests)
                    .Where(r => r.DeletedAt == null && r.IsEnabled)
                    .SelectMany(r => r.RequestComponents)
                    .Where(rc => rc.DeletedAt == null && rc.IsEnabled
                                                      && componentTypes.Contains(rc.ComponentType))
                    .OrderBy(rc => rc.Id)
                    .Skip(componentTypesIndex * NozomiServiceConstants.RequestComponentTakeoutLimit)
                    .Take(NozomiServiceConstants.RequestComponentTakeoutLimit)
                    .ToList();
            }

            var resultantItem = query.SingleOrDefault();

            if (resultantItem == null) return null;

            resultantItem.AnalysedComponents = analysedComponents;

            return new DetailedCurrencyResponse(resultantItem, requestComponents);
        }

        public bool Any(CreateCurrency createCurrency)
        {
            if (createCurrency != null && createCurrency.IsValid())
            {
                return _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbreviation.Equals(createCurrency.Abbreviation)).Any();
            }

            return false;
        }

        public IEnumerable<Currency> GetAllActive(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null && c.IsEnabled)
                    .Include(c => c.AnalysedComponents)
                    .Include(c => c.Requests)
                    .Include(c => c.CurrencySources)
                    .Include(c => c.CurrencyProperties)
                    .Include(c => c.CurrencyType);
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
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
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
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
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
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
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
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
                return _unitOfWork.GetRepository<Currency>()
                    .GetQueryable()
                    .AsNoTracking()
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
            var mainCurrency = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .SingleOrDefault(c => c.DeletedAt == null && c.IsEnabled && c.Id.Equals(currencyId));

            var components = _unitOfWork.GetRepository<AnalysedComponent>()
                .GetQueryable()
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
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .Select(c => c.Slug)
                .ToList();
        }

        public IEnumerable<CurrencyViewModel> ListAll(int page = 0, int itemsPerPage = 50,
            string currencyTypeName = null, bool orderAscending = true, string orderingParam = "Name")
        {
            var query = _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.IsEnabled && c.DeletedAt == null && c.CurrencyTypeId > 0);

            if (!string.IsNullOrEmpty(currencyTypeName))
                query = query
                    .Include(c => c.CurrencyType)
                    .Where(c => c.CurrencyType.DeletedAt == null && c.CurrencyType.IsEnabled &&
                                c.CurrencyType.Name.ToUpper().Equals(currencyTypeName.ToUpper()));

            switch (orderingParam.ToLower()) // Ignore case sensitivity
            {
                case "abbreviation":
                    query = orderAscending
                        ? query.OrderBy(c => c.Abbreviation)
                        : query.OrderByDescending(c => c.Abbreviation);
                    break;
                case "slug":
                    query = orderAscending ? query.OrderBy(c => c.Slug) : query.OrderByDescending(c => c.Slug);
                    break;
                case "currencytype":
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
            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
                .Where(c => c.DeletedAt == null && c.IsEnabled)
                .ToDictionary(c => c.Slug, c => c.Id);
        }

        public long SourceCount(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return long.MinValue;

            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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

            return _unitOfWork.GetRepository<Currency>()
                .GetQueryable()
                .AsNoTracking()
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
                    SourceTypeGuid = cs.Source.Guid.ToString()
                });
        }
    }
}