using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.UI;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Events
{
    public class TickerEvent : BaseEvent<TickerEvent, NozomiDbContext>, ITickerEvent
    {
        public TickerEvent(ILogger<TickerEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public DataTableResult<UniqueTickerResponse> GetAllForDatatable(int index = 0)
        {
            var data = _unitOfWork.GetRepository<CurrencyPair>()
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
                .Select(cp => new UniqueTickerResponse
                {
                    MainTickerAbbreviation = 
                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Abbrv,
                    MainTickerName = 
                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain).Currency.Name,
                    CounterTickerAbbreviation = 
                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Abbrv,
                    CounterTickerName = 
                        cp.PartialCurrencyPairs.FirstOrDefault(pcp => !pcp.IsMain).Currency.Name,
                    Exchange = cp.CurrencySource.Name,
                    ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                    LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                        .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                        .ModifiedAt,
                    Properties = cp.CurrencyPairRequests.FirstOrDefault()
                        .RequestComponents
                        .Select(rc => new KeyValuePair<string, string>(
                            rc.ComponentType.ToString(),
                            rc.Value))
                        .ToList()
                })
                .ToList();
            var fullCount = _unitOfWork.GetRepository<CurrencyPair>().GetQueryable().Count();
            
            return new DataTableResult<UniqueTickerResponse>
            {
                Draw = index,
                RecordsTotal = fullCount,
                RecordFiltered = fullCount - data.Count,
                Data = data
            };
        }

        public Task<NozomiResult<TickerByExchangeResponse>> GetById(long id)
        {
//            return Task.FromResult(new NozomiResult<TickerByExchangeResponse>(
//                NozomiServiceConstants.CurrencyPairDictionary[id]));
            
            return Task.FromResult(new NozomiResult<TickerByExchangeResponse>()
            {
                Data = _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .AsNoTracking()
                    .Where(cp => cp.Id.Equals(id))
                    .Include(cp => cp.CurrencySource)
                    .Where(cp => cp.CurrencySource.IsEnabled && cp.CurrencySource.DeletedAt == null)
                    .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                    .Select(cp => new TickerByExchangeResponse()
                    {
                        Exchange = cp.CurrencySource.Name,
                        ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                        LastUpdated = cp.CurrencyPairRequests.FirstOrDefault(cpr => cpr.DeletedAt == null && cpr.IsEnabled)
                            .RequestComponents.FirstOrDefault(rc => rc.DeletedAt == null && rc.IsEnabled)
                            .CreatedAt,
                        Properties = cp.CurrencyPairRequests
                            .SelectMany(cpr => cpr.RequestComponents)
                            .OrderBy(rc => rc.ComponentType).Select(rc => 
                                new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                    rc.Value)).ToList()
                    })
                    .SingleOrDefault()
            });
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
                    return new NozomiResult<ICollection<TickerByExchangeResponse>>(_unitOfWork.GetRepository<CurrencyPair>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Include(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                        .Where(cp => cp.IsEnabled && cp.DeletedAt == null
                                                  && (cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain)
                                                          .Currency.Abbrv + cp.PartialCurrencyPairs
                                                          .FirstOrDefault(pcp => !pcp.IsMain)
                                                          .Currency.Abbrv).Equals(ticker,
                                                      StringComparison.InvariantCultureIgnoreCase))
                        .Include(cp => cp.CurrencySource)
                        .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                        .Where(cp => cp.CurrencySource.Abbreviation.Equals(exchangeAbbrv, 
                            StringComparison.InvariantCultureIgnoreCase))
                        .Select(cp => new TickerByExchangeResponse
                        {
                            Exchange = cp.CurrencySource.Name,
                            ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                            LastUpdated = cp.CurrencyPairRequests
                                .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                                .RequestComponents.FirstOrDefault(rc => rc.IsEnabled && rc.DeletedAt == null)
                                .ModifiedAt,
                            Properties = cp.CurrencyPairRequests
                                .SelectMany(cpr => cpr.RequestComponents)
                                .OrderBy(rc => rc.ComponentType).Select(rc => 
                                    new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                        rc.Value)).ToList()
                        })
                        .ToList());
                    
//                    var key = new Tuple<string, string>(ticker, exchangeAbbrv);
//
//                    if (NozomiServiceConstants.CurrencySourceSymbolDictionary
//                        .ContainsKey(key))
//                    {
//                        return new NozomiResult<ICollection<TickerByExchangeResponse>>(
//                            new List<TickerByExchangeResponse> {
//                                NozomiServiceConstants.CurrencyPairDictionary[
//                                    NozomiServiceConstants.CurrencySourceSymbolDictionary[key]
//                                ]
//                            });
//                    }
//                    else
//                    {
//                        return new NozomiResult<ICollection<TickerByExchangeResponse>>(
//                            NozomiResultType.Failed, "The ticker specific to the exchange stated does not exist.");
//                    }
                }
                
//                return new NozomiResult<ICollection<TickerByExchangeResponse>>(
//                    NozomiServiceConstants.TickerSymbolDictionary[ticker].Where(i => i > 0).Select(
//                    i => NozomiServiceConstants.CurrencyPairDictionary[i]).ToList());

                    return new NozomiResult<ICollection<TickerByExchangeResponse>>(_unitOfWork.GetRepository<CurrencyPair>()
                        .GetQueryable()
                        .AsNoTracking()
                        .Include(cp => cp.PartialCurrencyPairs)
                        .ThenInclude(pcp => pcp.Currency)
                        .Where(cp => cp.IsEnabled && cp.DeletedAt == null
                                                  && (cp.PartialCurrencyPairs.FirstOrDefault(pcp => pcp.IsMain)
                                                          .Currency.Abbrv + cp.PartialCurrencyPairs
                                                          .FirstOrDefault(pcp => !pcp.IsMain)
                                                          .Currency.Abbrv).Equals(ticker,
                                                      StringComparison.InvariantCultureIgnoreCase))
                        .Include(cp => cp.CurrencySource)
                        .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                        .Select(cp => new TickerByExchangeResponse
                        {
                            Exchange = cp.CurrencySource.Name,
                            ExchangeAbbrv = cp.CurrencySource.Abbreviation,
                            LastUpdated = cp.CurrencyPairRequests
                                .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                                .RequestComponents.FirstOrDefault(rc => rc.IsEnabled && rc.DeletedAt == null)
                                .ModifiedAt,
                            Properties = cp.CurrencyPairRequests
                                .SelectMany(cpr => cpr.RequestComponents)
                                .OrderBy(rc => rc.ComponentType).Select(rc => 
                                    new KeyValuePair<string,string>(rc.ComponentType.ToString(), 
                                        rc.Value)).ToList()
                        })
                        .ToList());
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
    }
}