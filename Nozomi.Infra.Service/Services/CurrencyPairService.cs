using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.AreaModels.v1.CurrencyPair;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyPairService : BaseService<CurrencyPairService, NozomiDbContext>, ICurrencyPairService
    {
        public CurrencyPairService(ILogger<CurrencyPairService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger,
            unitOfWork)
        {
        }

        public bool Create(CreateCurrencyPair createCurrencyPair, long userId)
        {
            if (createCurrencyPair == null || !createCurrencyPair.IsValid()) return false;

            var currencyPair = new CurrencyPair()
            {
                CurrencyPairType = createCurrencyPair.CurrencyPairType,
                APIUrl = createCurrencyPair.ApiUrl,
                DefaultComponent = createCurrencyPair.DefaultComponent,
                SourceId = createCurrencyPair.CurrencySourceId,
                CurrencyPairCurrencies = createCurrencyPair.PartialCurrencyPairs
                    .Select(pcp => new CurrencyPairSourceCurrency
                    {
                        CurrencyId = pcp.CurrencyId
                    })
                    .ToList(),
                CurrencyPairRequests = createCurrencyPair.CurrencyPairRequests
                    .Select(cpr => new CurrencyPairRequest()
                    {
                        RequestType = cpr.RequestType,
                        DataPath = cpr.DataPath,
                        Delay = cpr.Delay,
                        RequestComponents = cpr.RequestComponents
                            .Select(rc => new RequestComponent()
                            {
                                ComponentType = rc.ComponentType,
                                QueryComponent = rc.QueryComponent
                            })
                            .ToList(),
                        RequestProperties = cpr.RequestProperties
                            .Select(rp => new RequestProperty()
                            {
                                RequestPropertyType = rp.RequestPropertyType,
                                Key = rp.Key,
                                Value = rp.Value
                            })
                            .ToList()
                    })
                    .ToList()
            };
            
            if (userId > 0)
            {
                _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
                _unitOfWork.Commit(userId);

                return true;
            }
            else
            {
                _unitOfWork.GetRepository<CurrencyPair>().Add(currencyPair);
                _unitOfWork.Commit();

                return true;
            }
        }

        public dynamic GetByIdObsc(long id, bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Where(cp => cp.Id.Equals(id))
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencyPairType = cp.CurrencyPairType,
                        apiUrl = cp.APIUrl,
                        defaultComponent = cp.DefaultComponent,
                        currencySourceId = cp.SourceId,
                    })
                    .SingleOrDefault();
            }

            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.Id.Equals(id))
                //.Include(cp => cp.Adverts)
                //.Include(cp => cp.CurrencyPairAdvertTypes)
                .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Include(cp => cp.Source)
                .Include(cp => cp.CurrencyPairCurrencies)
                .Select(cp => new
                {
                    id = cp.Id,
                    currencyPairType = cp.CurrencyPairType,
                    apiUrl = cp.APIUrl,
                    defaultComponent = cp.DefaultComponent,
                    currencySourceId = cp.SourceId,
                    currencySource = new
                    {
                        abbrv = cp.Source.Abbreviation,
                        name = cp.Source.Name
                    },
                    //advertCount = cp.Adverts.Count,
                    currencyPairComponents = cp.CurrencyPairRequests
                        .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                        .RequestComponents.Select(cpc => new
                    {
                        id = cpc.Id,
                        componentType = cpc.ComponentType,
                        queryComponent = cpc.QueryComponent,
                        value = cpc.Value
                    }),
                    partialCurrencyPairs = cp.CurrencyPairCurrencies.Select(pcp => new
                    {
                        currencyId = pcp.CurrencyId,
                        currency = new
                        {
                            abbrv = pcp.Currency.Abbreviation,
                            name = pcp.Currency.Name,
                            walletTypeId = pcp.Currency.WalletTypeId
                        }
                    })
                })
                .SingleOrDefault();
        }

        public IEnumerable<dynamic> GetAllObsc(bool track = false)
        {
            if (!track)
            {
                return _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencyPairType = cp.CurrencyPairType,
                        apiUrl = cp.APIUrl,
                        defaultComponent = cp.DefaultComponent,
                        currencySourceId = cp.SourceId,
                    });
            }

            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                //.Include(cp => cp.Adverts)
                //.Include(cp => cp.CurrencyPairAdvertTypes)
                .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Include(cp => cp.Source)
                .Include(cp => cp.CurrencyPairCurrencies)
                .Select(cp => new
                {
                    id = cp.Id,
                    currencyPairType = cp.CurrencyPairType,
                    apiUrl = cp.APIUrl,
                    defaultComponent = cp.DefaultComponent,
                    currencySourceId = cp.SourceId,
                    currencySource = new
                    {
                        abbrv = cp.Source.Abbreviation,
                        name = cp.Source.Name
                    },
                    //advertCount = cp.Adverts.Count,
                    currencyPairComponents = cp.CurrencyPairRequests
                        .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                        .RequestComponents.Select(cpc => new
                    {
                        id = cpc.Id,
                        componentType = cpc.ComponentType,
                        queryComponent = cpc.QueryComponent,
                        value = cpc.Value
                    }),
                    partialCurrencyPairs = cp.CurrencyPairCurrencies.Select(pcp => new
                    {
                        currencyId = pcp.CurrencyId,
                        currency = new
                        {
                            abbrv = pcp.Currency.Abbreviation,
                            name = pcp.Currency.Name,
                            walletTypeId = pcp.Currency.WalletTypeId
                        }
                    })
                });
        }

        public IEnumerable<CurrencyPair> GetAllActive(int index = 0, bool includeNested = false)
        {
            return !includeNested ? _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Include(cp => cp.CurrencyPairRequests)
                    .ThenInclude(cpr => cpr.RequestComponents)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Skip(index * 20)
                .Take(20) :
                _unitOfWork.GetRepository<CurrencyPair>()
                    .GetQueryable()
                    .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                    .Skip(index * 20)
                    .Take(20);
        }

        public IDictionary<string, IDictionary<long, long>> GetCurrencyPairSources()
        {
            // Prep the result
            // BTCUSDT => CurrencySources => CurrencyId of USDT
            IDictionary<string, IDictionary<long, long>> result = new Dictionary<string, IDictionary<long, long>>();

            var pcPairs = _unitOfWork.GetRepository<CurrencyPairSourceCurrency>()
                .GetQueryable()
                .Include(pcp => pcp.CurrencyPair)
                .Include(pcp => pcp.Currency)
                .Include(pcp => pcp.CurrencyPair.CurrencySource)
                .Include(pcp => pcp.CurrencyPair.CurrencyPairCurrencies)
                .ToList();

            // Foreach partialcurrencypair in pcps
            foreach (var pcPair in pcPairs)
            {
                // Make sure this partial pair is the main unit
                if (pcPair.Currency.Abbreviation.Equals(pcPair.CurrencyPair.MainCurrency))
                {
                    var counterPair = pcPairs.FirstOrDefault(pcp =>
                        pcp.CurrencyPairId.Equals(pcPair.CurrencyPairId) &&
                        pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.CounterCurrency, StringComparison.InvariantCultureIgnoreCase));
                    var counterPairStr = counterPair?.Currency.Abbreviation;

                    if (!string.IsNullOrEmpty(counterPairStr))
                    {
                        // Find the Pair string first
                        var pairStr = pcPair.Currency.Abbreviation + counterPairStr;

                        // GetAll the currencysource
                        var cSource = pcPair.CurrencyPair.CurrencySource;

                        // Add the CurrencySource if it hasn't been added already
                        if (!result.ContainsKey(pairStr))
                        {
                            result.Add(pairStr,
                                new Dictionary<long, long>() { { cSource.Id, counterPair.CurrencyPairId } });
                        }
                        // If the Pair has been added, but the value doesn't exist
                        else if (result.ContainsKey(pairStr) && !result[pairStr].ContainsKey(cSource.Id))
                        {
                            result[pairStr].Add(cSource.Id, counterPair.CurrencyPairId);
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> GetAllCurrencyPairUrls()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled)
                .Select(cp => cp.APIUrl)
                .ToList();
        }

//        public IEnumerable<CurrencyPair> GetAvailCPairsForAdvType(long id)
//        {
//            return _unitOfWork.GetRepository<CurrencyPair>()
//                .GetQueryable()
//                .Where(cp => cp.DeletedAt == null)
//                .Where(cp => cp.IsEnabled)
//                .Where(cp => cp.CurrencyPairAdvertTypes
//                    .Where(cpat => cpat.AdvertTypeId.Equals(id))
//                    .Any());
//        }

        public long GetCPairIdByTrio(long walletTypeId, long currencyId, long currencySourceId)
        {
            var cPair = _unitOfWork
                .GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Where(cp => cp.SourceId.Equals(currencySourceId))
                .FirstOrDefault(cp => cp.CurrencyPairCurrencies // CurrencyId is the counterpair
                                          .Any(pcp => pcp.CurrencyId.Equals(currencyId) &&
                                                      pcp.Currency.Abbreviation.Equals(pcp.CurrencyPair.CounterCurrency,
                                                          StringComparison.InvariantCultureIgnoreCase)) &&
                                      cp.CurrencyPairCurrencies.Any(pcp =>
                                          pcp.Currency.WalletTypeId.Equals(walletTypeId) &&
                                          pcp.CurrencyPair.CurrencySourceId.Equals(currencySourceId)));

            if (cPair != null)
                return cPair.Id;

            return -1; // bad
        }

        public ICollection<dynamic> GetAvailCPairsObsc(int index = 0, bool track = false)
        {
            var query = _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .AsNoTracking();

            // Future possible integrations
            // https://stackoverflow.com/questions/31305839/how-to-dynamically-generate-includes-in-entity-framework

            // https://stackoverflow.com/questions/8718480/c-sharp-linq-where-clause-as-a-variable
            Expression<Func<CurrencyPair, bool>> wExpression;
            //Expression<Func<CurrencyPair, bool>> iExpression; // Out for now

            wExpression = (cp => cp.DeletedAt == null && cp.IsEnabled);

            if (track)
            {
                return query
                    .Where(wExpression)
                    .Include(cp => cp.CurrencyPairCurrencies)
                    .Include(cp => cp.CurrencyPairRequests)
                        .ThenInclude(cpr => cpr.RequestComponents)
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencySourceId = cp.SourceId,
                        currencyPairComponents = cp.CurrencyPairRequests
                            .FirstOrDefault(cpr => cpr.IsEnabled && cpr.DeletedAt == null)
                            .RequestComponents
                            .Select(cpc => new
                            {
                                id = cpc.Id,
                                componentType = cpc.ComponentType,
                                value = cpc.Value,
                                isEnabled = cpc.IsEnabled
                            }),
                        partialCurrencyPairs = cp.CurrencyPairCurrencies
                            .Select(pcp => new
                            {
                                currencyId = pcp.CurrencyId,
                                currency = new
                                {
                                    abbrv = pcp.Currency.Abbreviation,
                                    name = pcp.Currency.Name,
                                    walletTypeId = pcp.Currency.WalletTypeId,
                                    isEnabled = pcp.Currency.IsEnabled
                                }
                            }),
                        defaultComponent = cp.DefaultComponent
                    })
                    .Skip(index * 20)
                    .Take(20)
                    .ToList<dynamic>();
            }
            else
            {
                return query.Where(wExpression)
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencySourceId = cp.SourceId,
                        defaultComponent = cp.DefaultComponent
                    })
                    .Skip(index * 20)
                    .Take(20)
                    .ToList<dynamic>();
            }
        }

        public long[][] GetCurrencySourceMappings()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null)
                .Where(cp => cp.IsEnabled)
                .Include(cp => cp.Source)
                .Select(cp => new long[] {cp.Id, cp.Source.Id})
                .ToArray();
        }
    }
}