using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Native.Collections;
using Nozomi.Base.Identity.ViewModels.Manage.Tickers;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.Currency;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class TickerService : BaseService<TickerService, NozomiDbContext>, ITickerService
    {
        private readonly ICurrencyService _currencyService;
        
        public TickerService(ICurrencyService currencyService, ILogger<TickerService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
            _currencyService = currencyService;
        }

        public NozomiResult<UniqueTickerResponse> Create(CreateTickerInputModel createTickerInputModel)
        {
            // Validate the source
            if (!_unitOfWork.GetRepository<Source>().GetQueryable()
                .Any(s => s.Id.Equals(createTickerInputModel.CurrencySourceId)
                          && s.DeletedAt == null))
            {
                return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                    "Invalid Source.");
            }
            
            // Aggregate the currencies

            var mainCurrency = new Currency
            {
                CurrencyTypeId = createTickerInputModel.MainCurrencyTypeId,
                Abbreviation = createTickerInputModel.MainCurrencyAbbrv,
                Name = createTickerInputModel.MainCurrencyName
            };
            
            // Main
            // Make sure the currency exists
            if (_unitOfWork.GetRepository<Currency>().GetQueryable()
                .Any(c => c.Abbreviation.Equals(mainCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase)
                          && c.DeletedAt == null))
            {
                // Currency already exists
                mainCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbreviation.Equals(mainCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();
            }
            else
            {
                // Create the currency
                _currencyService.Create(new CreateCurrency
                {
                    CurrencySourceId = createTickerInputModel.CurrencySourceId,
                    CurrencyTypeId = mainCurrency.CurrencyTypeId,
                    Abbreviation = mainCurrency.Abbreviation,
                    Name = mainCurrency.Name
                });
                
                // Retrieve it
                mainCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbreviation.Equals(mainCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();
                
                _logger.LogInformation($"Currency {mainCurrency.Name} created.");
            }

            var counterCurrency = new Currency
            {
                CurrencyTypeId = createTickerInputModel.CounterCurrencyTypeId,
                Abbreviation = createTickerInputModel.CounterCurrencyAbbrv,
                Name = createTickerInputModel.CounterCurrencyName
            };
            
            // Counter
            if (_unitOfWork.GetRepository<Currency>().GetQueryable()
                .Any(c => c.Abbreviation.Equals(counterCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase)
                          && c.DeletedAt == null))
            {
                // Currency already exists
                counterCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbreviation.Equals(counterCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();
            }
            else
            {
                // Create the currency
                _currencyService.Create(new CreateCurrency
                {
                    CurrencySourceId = createTickerInputModel.CurrencySourceId,
                    CurrencyTypeId = counterCurrency.CurrencyTypeId,
                    Abbreviation = counterCurrency.Abbreviation,
                    Name = counterCurrency.Name
                });
                
                // Retrieve it
                counterCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbreviation.Equals(counterCurrency.Abbreviation, StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();
                
                _logger.LogInformation($"Currency {counterCurrency.Name} created.");
            }

            // Currency check
            if (counterCurrency == null || mainCurrency == null)
            {
                return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                    "Invalid currency sub pair/s.");
            }

            var currencyPair = new CurrencyPair
            {
                APIUrl = createTickerInputModel.DataPath,
                CurrencyPairType = createTickerInputModel.CurrencyPairType,
                SourceId = createTickerInputModel.CurrencySourceId,
                MainCurrencyAbbrv = mainCurrency.Abbreviation,
                CounterCurrencyAbbrv = counterCurrency.Abbreviation
            };
            
            var currencyPairRequest = new CurrencyPairRequest
            {
                CurrencyPair = currencyPair,
                DataPath = createTickerInputModel.DataPath,
                Delay = createTickerInputModel.Delay,
                RequestType = createTickerInputModel.RequestType,
                ResponseType = createTickerInputModel.ResponseType,
                RequestComponents = new List<RequestComponent>(),
                RequestProperties = new List<RequestProperty>()
            };
            
            // Request Property aggregation
            if (!string.IsNullOrEmpty(createTickerInputModel.RequestProperties))
            {
                var requestProperties = createTickerInputModel.RequestProperties.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );

                foreach (var reqProp in requestProperties)
                {
                    var reqPropEl = reqProp.Split(">");

                    if (reqPropEl.Length != 3)
                    {
                        return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                            $"Invalid request property format. {reqProp}");
                    }
                    
                    var reqPropType = Enum.TryParse(reqPropEl[0], out RequestPropertyType reqPropElType) ? reqPropElType
                        : RequestPropertyType.Invalid;

                    if (reqPropType.Equals(RequestPropertyType.Invalid))
                    {
                        return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                            $"Invalid request property type. {reqProp}");
                    }
                    
                    var requestProperty = new RequestProperty
                    {
                        RequestPropertyType = reqPropType,
                        Key = reqPropEl[1],
                        Value = reqPropEl[2]
                    };

                    if (currencyPairRequest.RequestProperties
                        .Any(rp => rp.RequestPropertyType.Equals(requestProperty.RequestPropertyType)))
                    {
                        return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                            $"A duplicate property exists. {reqProp}");
                    }
                    
                    currencyPairRequest.RequestProperties.Add(requestProperty);
                }
            }
            
            // Request Component aggregation
            // https://stackoverflow.com/questions/1547476/easiest-way-to-split-a-string-on-newlines-in-net
            var requestComponents = createTickerInputModel.QueryComponents.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            if (requestComponents.Length < 1)
            {
                return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                    "Please enter a request component.");
            }

            foreach (var requestComponent in requestComponents)
            {
                var requestComponentEl = requestComponent.Split(">");
                if (requestComponentEl.Length != 2)
                {
                    return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                        $"Invalid request component. [{requestComponent}]");
                }
                
                // https://stackoverflow.com/questions/16100/how-should-i-convert-a-string-to-an-enum-in-c
                var componentType = Enum.TryParse(requestComponentEl[0], out ComponentType outComponentType) 
                    ? outComponentType
                    : ComponentType.Unknown;
                
                // Safety check
                if (componentType.Equals(ComponentType.Unknown)
                    && currencyPairRequest.RequestComponents
                        .Any(rc => rc.ComponentType == ComponentType.Unknown))
                {
                    return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                        "There already is another component with the same component type.");
                }

                currencyPairRequest.RequestComponents.Add(new RequestComponent
                {
                    ComponentType = componentType,
                    QueryComponent = requestComponentEl[1],
                    Value = "0"
                });
            }

            currencyPair.DefaultComponent = currencyPairRequest.RequestComponents
                .FirstOrDefault()?.QueryComponent;

            try
            {
                // Create the request
                _unitOfWork.GetRepository<CurrencyPairRequest>().Add(currencyPairRequest);
                _unitOfWork.Commit();
                
                return new NozomiResult<UniqueTickerResponse>(
                    new UniqueTickerResponse
                    {
                        MainTickerAbbreviation = 
                            mainCurrency.Abbreviation,
                        MainTickerName = 
                            mainCurrency.Name,
                        CounterTickerAbbreviation = 
                            counterCurrency.Abbreviation,
                        CounterTickerName = 
                            counterCurrency.Name,
                    });
            }
            catch (Exception ex)
            {
                return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                    ex.ToString());
            }
        }

        public NozomiResult<string> Delete(string ticker, string exchangeAbbrv)
        {
            var tickerObj = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled 
                                                  // Ticker pair check
                                                  && string.Concat(cp.MainCurrencyAbbrv, cp.CounterCurrencyAbbrv)
                                                      .Equals(ticker, StringComparison.InvariantCultureIgnoreCase))
                .Include(cp => cp.Source)
                .SingleOrDefault(cp => cp.Source.Abbreviation
                    .Equals(exchangeAbbrv, StringComparison.InvariantCultureIgnoreCase));

            if (tickerObj != null)
            {
                tickerObj.DeletedAt = DateTime.UtcNow;
                
                _unitOfWork.GetRepository<CurrencyPair>().Update(tickerObj);
                _unitOfWork.Commit();
                
                return new NozomiResult<string>(NozomiResultType.Success,
                    "Ticker successfully deleted!");
            }
            
            return new NozomiResult<string>(NozomiResultType.Failed,
                "Incorrect ticker and exchange abbreviation combination.");
        }

        public Task<NozomiResult<ICollection<UniqueTickerResponse>>> GetAll(int index)
        {
            var utrByCpr = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .Skip(index * 20)
                .Take(20)
                .OrderBy(cp => cp.Id)
                .DefaultIfEmpty()
                .Select(cp => new UniqueTickerResponse
                {
                    MainTickerAbbreviation = cp.MainCurrencyAbbrv,
                    MainTickerName = cp.Source.SourceCurrencies
                        .SingleOrDefault(sc => sc.Currency.Abbreviation.Equals(cp.MainCurrencyAbbrv))
                        .Currency
                        .Name,
                    CounterTickerAbbreviation = cp.CounterCurrencyAbbrv,
                    CounterTickerName = cp.Source.SourceCurrencies
                        .SingleOrDefault(sc => sc.Currency.Abbreviation.Equals(cp.CounterCurrencyAbbrv))
                        .Currency
                        .Name,
                    Exchange = cp.Source.Name,
                    ExchangeAbbrv = cp.Source.Abbreviation,
                    LastUpdated = cp.CurrencyPairRequests
                        .Select(cpr => cpr.RequestComponents
                            .OrderByDescending(rc => rc.ModifiedAt)
                            .FirstOrDefault()
                            .ModifiedAt)
                        .SingleOrDefault(),
                    Properties = cp.CurrencyPairRequests
                        .SelectMany(cpr => cpr.RequestComponents)
                        .Where(rc => rc.IsEnabled && rc.DeletedAt == null
                                                  && !string.IsNullOrEmpty(rc.Value))
                        .Select(rc => new KeyValuePair<string, string>(
                            rc.ComponentType.ToString(),
                            rc.Value))
                        .DefaultIfEmpty()
                        .ToList()
                })
                .ToList();

            utrByCpr.AddRange(_unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.Source)
                .ThenInclude(s => s.SourceCurrencies)
                .ThenInclude(sc => sc.Currency)
                .Include(cp => cp.WebsocketRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .Skip(index * 20)
                .Take(20)
                .OrderBy(cp => cp.Id)
                .DefaultIfEmpty()
                .Select(cp => new UniqueTickerResponse
                {
                    MainTickerAbbreviation = cp.MainCurrencyAbbrv,
                    MainTickerName = cp.Source.SourceCurrencies
                        .SingleOrDefault(sc => sc.Currency.Abbreviation.Equals(cp.MainCurrencyAbbrv))
                        .Currency
                        .Name,
                    CounterTickerAbbreviation = cp.CounterCurrencyAbbrv,
                    CounterTickerName = cp.Source.SourceCurrencies
                        .SingleOrDefault(sc => sc.Currency.Abbreviation.Equals(cp.CounterCurrencyAbbrv))
                        .Currency
                        .Name,
                    Exchange = cp.Source.Name,
                    ExchangeAbbrv = cp.Source.Abbreviation,
                    LastUpdated = cp.WebsocketRequests
                        .Select(cpr => cpr.RequestComponents
                            .OrderByDescending(rc => rc.ModifiedAt)
                            .FirstOrDefault()
                            .ModifiedAt)
                        .SingleOrDefault(),
                    Properties = cp.WebsocketRequests
                        .SelectMany(cpr => cpr.RequestComponents)
                        .Where(rc => rc.IsEnabled && rc.DeletedAt == null
                                                  && !string.IsNullOrEmpty(rc.Value))
                        .Select(rc => new KeyValuePair<string, string>(
                            rc.ComponentType.ToString(),
                            rc.Value))
                        .DefaultIfEmpty()
                        .ToList()
                }));
            
            return Task.FromResult(new NozomiResult<ICollection<UniqueTickerResponse>>
            {
                Data = utrByCpr
            });
        }

        public NozomiResult<ICollection<TickerByExchangeResponse>> GetAllActive()
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.Source)
                    .ThenInclude(s => s.SourceCurrencies)
                    .ThenInclude(sc => sc.Currency)
                    .Include(cp => cp.Source)
                    .Where(cp => cp.Source != null) // Make sure we have a source
                    .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                    // Make sure there's something
                    .Where(cp => cp.CurrencyPairRequests
                        .Any(cpr => cpr.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null)))
                    .Select(cp => new TickerByExchangeResponse()
                    {
                        Exchange = cp.Source.Name,
                        ExchangeAbbrv = cp.Source.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents.FirstOrDefault()
                            .ModifiedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents
                            .Select(rc => new KeyValuePair<string, string>(
                                rc.ComponentType.ToString(), 
                                rc.Value))
                            .ToList()
                    })
                    .ToList();
            
            return new NozomiResult<ICollection<TickerByExchangeResponse>>()
            {
                ResultType = (query != null) ? NozomiResultType.Success : NozomiResultType.Failed,
                Data = (query != null) ? query : new List<TickerByExchangeResponse>()
            };
        }
    }
}