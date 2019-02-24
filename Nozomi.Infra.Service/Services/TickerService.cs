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
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Data.WebModels;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class TickerService : BaseService<TickerService, NozomiDbContext>, ITickerService
    {
        public TickerService(ILogger<TickerService> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
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
                CurrencySourceId = createTickerInputModel.CurrencySourceId,
                CurrencyTypeId = createTickerInputModel.MainCurrencyTypeId,
                Abbrv = createTickerInputModel.MainCurrencyAbbrv,
                Name = createTickerInputModel.MainCurrencyName
            };
            
            // Main
            if (_unitOfWork.GetRepository<Currency>().GetQueryable()
                .Any(c => c.Abbrv.Equals(mainCurrency.Abbrv)
                          && c.CurrencySourceId.Equals(createTickerInputModel.CurrencySourceId)
                          && c.DeletedAt == null))
            {
                // Currency already exists
                mainCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbrv.Equals(mainCurrency.Abbrv)
                              && c.CurrencySourceId.Equals(createTickerInputModel.CurrencySourceId))
                    .SingleOrDefault();
            }
            else
            {
                // Create the currency
                _unitOfWork.GetRepository<Currency>().Add(mainCurrency);
                _unitOfWork.Commit();
                
                // Retrieve it
                mainCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbrv.Equals(mainCurrency.Abbrv)
                              && c.CurrencySourceId.Equals(mainCurrency.CurrencySourceId))
                    .SingleOrDefault();
                
                _logger.LogInformation($"Currency {mainCurrency.Name} created for " +
                                       $"source {mainCurrency.CurrencySourceId}.");
            }

            var counterCurrency = new Currency
            {
                CurrencySourceId = createTickerInputModel.CurrencySourceId,
                CurrencyTypeId = createTickerInputModel.CounterCurrencyTypeId,
                Abbrv = createTickerInputModel.CounterCurrencyAbbrv,
                Name = createTickerInputModel.CounterCurrencyName
            };
            
            // Counter
            if (_unitOfWork.GetRepository<Currency>().GetQueryable()
                .Any(c => c.Abbrv.Equals(counterCurrency.Abbrv)
                          && c.CurrencySourceId.Equals(createTickerInputModel.CurrencySourceId)
                          && c.DeletedAt == null))
            {
                // Currency already exists
                counterCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbrv.Equals(counterCurrency.Abbrv)
                              && c.CurrencySourceId.Equals(createTickerInputModel.CurrencySourceId))
                    .SingleOrDefault();
            }
            else
            {
                // Create the currency
                _unitOfWork.GetRepository<Currency>().Add(counterCurrency);
                _unitOfWork.Commit();
                
                // Retrieve it
                counterCurrency = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Abbrv.Equals(counterCurrency.Abbrv)
                              && c.CurrencySourceId.Equals(counterCurrency.CurrencySourceId))
                    .SingleOrDefault();
                
                _logger.LogInformation($"Currency {counterCurrency.Name} created for " +
                                       $"source {counterCurrency.CurrencySourceId}.");
            }

            // Currency check
            if (counterCurrency == null || mainCurrency == null)
            {
                return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                    "Invalid currency sub pair/s.");
            }
            
            // Currency source check
            if (counterCurrency.CurrencySourceId != mainCurrency.CurrencySourceId)
            {
                return new NozomiResult<UniqueTickerResponse>(NozomiResultType.Failed,
                    "Unable to peg a main and counter currency that have a different" +
                    " source.");
            }

            var currencyPair = new CurrencyPair
            {
                APIUrl = createTickerInputModel.DataPath,
                CurrencyPairType = createTickerInputModel.CurrencyPairType,
                CurrencySourceId = createTickerInputModel.CurrencySourceId,
                PartialCurrencyPairs = new List<PartialCurrencyPair>
                {
                    new PartialCurrencyPair
                    {
                        IsMain = true,
                        CurrencyId = mainCurrency.Id
                    },
                    new PartialCurrencyPair
                    {
                        IsMain = false,
                        CurrencyId = counterCurrency.Id
                    }
                }
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
                    RequestComponentDatum = new RequestComponentDatum
                    {
                        Value = "0"
                    }
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
                        TickerAbbreviation = mainCurrency.Abbrv + counterCurrency.Abbrv
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
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Include(cp => cp.PartialCurrencyPairs)
                .ThenInclude(pcp => pcp.Currency)
                .Include(cp => cp.CurrencySource)
                .Include(cp => cp.CurrencyPairRequests)
                .ThenInclude(cpr => cpr.RequestComponents)
                .ThenInclude(rc => rc.RequestComponentDatum)
                .SingleOrDefault(cp => string.Concat(
                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv,
                    cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv)
                    .Equals(ticker, StringComparison.InvariantCultureIgnoreCase)
                && cp.CurrencySource.Abbreviation.Equals(exchangeAbbrv, StringComparison.InvariantCultureIgnoreCase));

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
            return Task.FromResult(new NozomiResult<ICollection<UniqueTickerResponse>>
            {
                Data = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Skip(index * 20)
                    .Take(20)
                    .OrderBy(cp => cp.Id)
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                    .Include(cp => cp.CurrencySource)
                    .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .ThenInclude(rc => rc.RequestComponentDatum)
                    .Select(cp => new UniqueTickerResponse
                    {
                        TickerAbbreviation = string.Concat(
                            cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv,
                            cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv),
                        Exchange = cp.CurrencySource.Name,
                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .RequestComponentDatum
                            .ModifiedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents
                            .Select(rc => new KeyValuePair<string, string>(
                                rc.ComponentType.ToString(), 
                                rc.RequestComponentDatum.Value))
                            .ToList() 
                    })
                    .ToList()
            });
        }

        public Task<NozomiResult<TickerByExchangeResponse>> GetById(long id)
        {
            return Task.FromResult(new NozomiResult<TickerByExchangeResponse>(
                NozomiServiceConstants.CurrencyPairDictionary[id]));
            
//            return Task.FromResult(new NozomiResult<TickerResponse>()
//            {
//                Data = _unitOfWork.GetRepository<CurrencyPair>()
//                    .GetQueryable()
//                    .AsNoTracking()
//                    .Where(cp => cp.Id.Equals(id))
//                    .Include(cp => cp.CurrencyPairRequests)
//                    .ThenInclude(cpr => cpr.RequestComponents)
//                    .ThenInclude(rc => rc.RequestComponentDatum)
//                    .Select(cp => new TickerResponse()
//                    {
//                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
//                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
//                            .RequestComponentDatum
//                            .CreatedAt,
//                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
//                            .RequestComponents
//                            .Select(rc => new KeyValuePair<string, string>(
//                                rc.ComponentType.ToString(), 
//                                rc.RequestComponentDatum.Value))
//                            .ToList()
//                                    
//                    })
//                    .SingleOrDefault()
//            });
        }

        public NozomiResult<ICollection<TickerByExchangeResponse>> GetByAbbreviation(string ticker, 
            string exchangeAbbrv = null)
        {
            try
            {
                if (ticker.Length != 6) return new NozomiResult<ICollection<TickerByExchangeResponse>>(
                    NozomiResultType.Failed, "Invalid Ticker Symbol."); // Invalid ticker length
                
                // Exchange? Specification.
                if (!string.IsNullOrEmpty(exchangeAbbrv))
                {
                    var key = new Tuple<string, string>(ticker, exchangeAbbrv);

                    if (NozomiServiceConstants.CurrencySourceSymbolDictionary
                        .ContainsKey(key))
                    {
                        return new NozomiResult<ICollection<TickerByExchangeResponse>>(
                            new List<TickerByExchangeResponse> {
                                NozomiServiceConstants.CurrencyPairDictionary[
                                    NozomiServiceConstants.CurrencySourceSymbolDictionary[key]
                                ]
                            });
                    }
                    else
                    {
                        return new NozomiResult<ICollection<TickerByExchangeResponse>>(
                            NozomiResultType.Failed, "The ticker specific to the exchange stated does not exist.");
                    }
                }
                
                return new NozomiResult<ICollection<TickerByExchangeResponse>>(
                    NozomiServiceConstants.TickerSymbolDictionary[ticker].Where(i => i > 0).Select(
                    i => NozomiServiceConstants.CurrencyPairDictionary[i]).ToList());

//                var query = _unitOfWork.GetRepository<CurrencyPair>()
//                    .GetQueryable()
//                    .AsNoTracking()
//                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
//                    .Include(cp => cp.PartialCurrencyPairs)
//                    .ThenInclude(pcp => pcp.Currency)
//                    .Where(cp => (cp.PartialCurrencyPairs.SingleOrDefault(pcp => pcp.IsMain).Currency
//                                      .Abbrv // Make sure the first currency (main) is equal to the ticker's first
//                                  + cp.PartialCurrencyPairs.SingleOrDefault(pcp => !pcp.IsMain).Currency.Abbrv) // other way round
//                        .Equals(ticker, StringComparison.InvariantCultureIgnoreCase))
//                    .Include(cp => cp.CurrencySource)
//                    .Where(cp => cp.CurrencySource != null) // Make sure we have a source
//                    .Include(cp => cp.CurrencyPairRequests)
//                        .ThenInclude(cpr => cpr.RequestComponents)
//                            .ThenInclude(rc => rc.RequestComponentDatum)
//                    // Make sure there's something
//                    .Where(cp => cp.CurrencyPairRequests
//                        .Any(cpr => cpr.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null && 
//                                                                    rc.RequestComponentDatum != null)))
//                    .Select(cp => new DistinctiveTickerResponse()
//                    {
//                        Exchange = cp.CurrencySource.Name,
//                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
//                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault()
//                            .RequestComponents.FirstOrDefault()
//                            .RequestComponentDatum.ModifiedAt,
//                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
//                            .RequestComponents
//                            .Select(rc => new KeyValuePair<string, string>(
//                                rc.ComponentType.ToString(), 
//                                rc.RequestComponentDatum.Value))
//                            .ToList()
//                    });
//
//                // Exchange-based filter
//                if (!exchangeAbbrv.IsNullOrEmpty())
//                {
//                    query = query.Where(cp => cp.ExchangeAbbrv.Equals(exchangeAbbrv));
//                }
//
//                return new NozomiResult<ICollection<DistinctiveTickerResponse>>()
//                {
//                    ResultType = NozomiResultType.Success,
//                    Data = query.ToList()
//                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new NozomiResult<ICollection<TickerByExchangeResponse>>()
                {
                    ResultType = NozomiResultType.Failed,
                    Message = "An error has occurred.",
                    Data = null
                }; 
            }
        }

        public NozomiResult<ICollection<TickerByExchangeResponse>> GetAllActive()
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Include(cp => cp.PartialCurrencyPairs)
                    .ThenInclude(pcp => pcp.Currency)
                    .Include(cp => cp.CurrencySource)
                    .Where(cp => cp.CurrencySource != null) // Make sure we have a source
                    .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                            .ThenInclude(rc => rc.RequestComponentDatum)
                    // Make sure there's something
                    .Where(cp => cp.CurrencyPairRequests
                        .Any(cpr => cpr.RequestComponents.Any(rc => rc.IsEnabled && rc.DeletedAt == null && 
                                                                    rc.RequestComponentDatum != null)))
                    .Select(cp => new TickerByExchangeResponse()
                    {
                        Exchange = cp.CurrencySource.Name,
                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents.FirstOrDefault()
                            .RequestComponentDatum.ModifiedAt,
                        Properties = cp.CurrencyPairRequests.FirstOrDefault()
                            .RequestComponents
                            .Select(rc => new KeyValuePair<string, string>(
                                rc.ComponentType.ToString(), 
                                rc.RequestComponentDatum.Value))
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