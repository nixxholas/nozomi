﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
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
                        currencySourceId = cp.CurrencySourceId,
                    })
                    .SingleOrDefault();
            }

            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.Id.Equals(id))
                //.Include(cp => cp.Adverts)
                //.Include(cp => cp.CurrencyPairAdvertTypes)
                .Include(cp => cp.CurrencyPairComponents)
                    .ThenInclude(cpc => cpc.RequestComponentData)
                .Include(cp => cp.CurrencySource)
                .Include(cp => cp.PartialCurrencyPairs)
                .Select(cp => new
                {
                    id = cp.Id,
                    currencyPairType = cp.CurrencyPairType,
                    apiUrl = cp.APIUrl,
                    defaultComponent = cp.DefaultComponent,
                    currencySourceId = cp.CurrencySourceId,
                    currencySource = new
                    {
                        abbrv = cp.CurrencySource.Abbreviation,
                        name = cp.CurrencySource.Name
                    },
                    //advertCount = cp.Adverts.Count,
                    currencyPairComponents = cp.CurrencyPairComponents.Select(cpc => new
                    {
                        id = cpc.Id,
                        componentType = cpc.ComponentType,
                        queryComponent = cpc.QueryComponent,
                        value = cpc.RequestComponentData
                            .OrderByDescending(rcd => rcd.CreatedAt)
                            .Select(rcd => rcd.Value).FirstOrDefault()
                    }),
                    partialCurrencyPairs = cp.PartialCurrencyPairs.Select(pcp => new
                    {
                        isMain = pcp.IsMain,
                        currencyId = pcp.CurrencyId,
                        currency = new
                        {
                            abbrv = pcp.Currency.Abbrv,
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
                        currencySourceId = cp.CurrencySourceId,
                    });
            }

            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                //.Include(cp => cp.Adverts)
                //.Include(cp => cp.CurrencyPairAdvertTypes)
                .Include(cp => cp.CurrencyPairComponents)
                    .ThenInclude(cpc => cpc.RequestComponentData)
                .Include(cp => cp.CurrencySource)
                .Include(cp => cp.PartialCurrencyPairs)
                .Select(cp => new
                {
                    id = cp.Id,
                    currencyPairType = cp.CurrencyPairType,
                    apiUrl = cp.APIUrl,
                    defaultComponent = cp.DefaultComponent,
                    currencySourceId = cp.CurrencySourceId,
                    currencySource = new
                    {
                        abbrv = cp.CurrencySource.Abbreviation,
                        name = cp.CurrencySource.Name
                    },
                    //advertCount = cp.Adverts.Count,
                    currencyPairComponents = cp.CurrencyPairComponents.Select(cpc => new
                    {
                        id = cpc.Id,
                        componentType = cpc.ComponentType,
                        queryComponent = cpc.QueryComponent,
                        value = cpc.RequestComponentData
                            .OrderByDescending(rcd => rcd.CreatedAt)
                            .Select(rcd => rcd.Value).FirstOrDefault()
                    }),
                    partialCurrencyPairs = cp.PartialCurrencyPairs.Select(pcp => new
                    {
                        isMain = pcp.IsMain,
                        currencyId = pcp.CurrencyId,
                        currency = new
                        {
                            abbrv = pcp.Currency.Abbrv,
                            name = pcp.Currency.Name,
                            walletTypeId = pcp.Currency.WalletTypeId
                        }
                    })
                });
        }

        public IEnumerable<CurrencyPair> GetAllActive()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Include(cp => cp.CurrencyPairComponents)
                .Where(cp => cp.DeletedAt == null && cp.IsEnabled);
        }

        public IDictionary<string, IDictionary<long, long>> GetCurrencyPairSources()
        {
            // Prep the result
            // BTCUSDT => CurrencySources => CurrencyId of USDT
            IDictionary<string, IDictionary<long, long>> result = new Dictionary<string, IDictionary<long, long>>();

            var pcPairs = _unitOfWork.GetRepository<PartialCurrencyPair>()
                .GetQueryable()
                .Include(pcp => pcp.CurrencyPair)
                .Include(pcp => pcp.Currency)
                .Include(pcp => pcp.CurrencyPair.CurrencySource)
                .Include(pcp => pcp.CurrencyPair.PartialCurrencyPairs)
                .ToList();

            // Foreach partialcurrencypair in pcps
            foreach (var pcPair in pcPairs)
            {
                // Make sure this partial pair is the main unit
                if (pcPair.IsMain)
                {
                    var counterPair = pcPairs.FirstOrDefault(pcp =>
                        pcp.CurrencyPairId.Equals(pcPair.CurrencyPairId) &&
                        !pcp.IsMain);
                    var counterPairStr = counterPair?.Currency.Abbrv;

                    if (!string.IsNullOrEmpty(counterPairStr))
                    {
                        // Find the Pair string first
                        var pairStr = pcPair.Currency.Abbrv + counterPairStr;

                        // Get the currencysource
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
                .Where(cp => cp.CurrencySourceId.Equals(currencySourceId))
                .FirstOrDefault(cp => cp.PartialCurrencyPairs // CurrencyId is the counterpair
                                          .Any(pcp => pcp.CurrencyId.Equals(currencyId) &&
                                                      !pcp.IsMain) &&
                                      cp.PartialCurrencyPairs.Any(pcp =>
                                          pcp.Currency.WalletTypeId.Equals(walletTypeId) &&
                                          pcp.CurrencyPair.CurrencySourceId.Equals(currencySourceId)));

            if (cPair != null)
                return cPair.Id;

            return -1; // bad
        }

        public ICollection<dynamic> GetAvailCPairsObsc(bool track = false)
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
                    .Include(cp => cp.PartialCurrencyPairs)
                    .Include(cp => cp.CurrencyPairComponents)
                        .ThenInclude(cpc => cpc.RequestComponentData)
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencySourceId = cp.CurrencySourceId,
                        currencyPairComponents = cp.CurrencyPairComponents
                            .Select(cpc => new
                            {
                                id = cpc.Id,
                                componentType = cpc.ComponentType,
                                value = cpc.RequestComponentData
                                    .OrderByDescending(rcd => rcd.CreatedAt)
                                    .Select(rcd => rcd.Value).FirstOrDefault(),
                                isEnabled = cpc.IsEnabled
                            }),
                        partialCurrencyPairs = cp.PartialCurrencyPairs
                            .Select(pcp => new
                            {
                                currencyId = pcp.CurrencyId,
                                currency = new
                                {
                                    abbrv = pcp.Currency.Abbrv,
                                    name = pcp.Currency.Name,
                                    walletTypeId = pcp.Currency.WalletTypeId,
                                    isEnabled = pcp.Currency.IsEnabled
                                },
                                isMain = pcp.IsMain
                            }),
                        defaultComponent = cp.DefaultComponent
                    })
                    .ToList<dynamic>();
            }
            else
            {
                return query.Where(wExpression)
                    .Select(cp => new
                    {
                        id = cp.Id,
                        currencySourceId = cp.CurrencySourceId,
                        defaultComponent = cp.DefaultComponent
                    })
                    .ToList<dynamic>();
            }
        }

        public long[][] GetCurrencySourceMappings()
        {
            return _unitOfWork.GetRepository<CurrencyPair>()
                .GetQueryable()
                .Where(cp => cp.DeletedAt == null)
                .Where(cp => cp.IsEnabled)
                .Include(cp => cp.CurrencySource)
                .Select(cp => new long[] {cp.Id, cp.CurrencySource.Id})
                .ToArray();
        }
    }
}