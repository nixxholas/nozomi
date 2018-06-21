using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Counter.SDK.Utils.Iterations;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Counter.SDK.SharedModels.WalletModels;
using Counter.SDK.Utils.Iterations;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data;
using Nozomi.Repo.Repositories;
using Nozomi.Service.Services;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Services
{
    public class CurrencyService : BaseService<CurrencyService, NozomiDbContext>, ICurrencyService
    {
        public CurrencyService(ILogger<CurrencyService> logger, 
            IUnitOfWork<NozomiDbContext> unitOfWork) : base(logger, unitOfWork)
        {
        }

        public long Create(Currency currency, long userId = 0)
        {
            if (currency != null && currency.IsValid())
            {
                _unitOfWork.GetRepository<Currency>().Add(currency);
                _unitOfWork.Commit(userId);

                return currency.Id;
            }

            return -1;
        }

        public bool Update(long userId, long currencyId,Currency currency)
        {
            if (currency != null && currency.IsValid() && currencyId > 0)
            {
                var currToUpd = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Id.Equals(currencyId) && c.DeletedAt == null)
                    .SingleOrDefault();

                if (currToUpd != null)
                {
                    currToUpd.Abbrv = currency.Abbrv;
                    currToUpd.CurrencyTypeId = currency.CurrencyTypeId;
                    currToUpd.Name = currency.Name;
                    currToUpd.WalletTypeId = currency.WalletTypeId;
                    currToUpd.IsEnabled = currency.IsEnabled;
                    
                    _unitOfWork.GetRepository<Currency>().Update(currToUpd);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public bool SoftDelete(long currencyId, long userId)
        {
            if (currencyId > 0 && userId > 0)
            {
                var currToDel = _unitOfWork.GetRepository<Currency>()
                    .Get(c => c.Id.Equals(currencyId) && c.DeletedAt == null)
                    .SingleOrDefault();

                if (currToDel != null)
                {
                    currToDel.DeletedAt = DateTime.UtcNow;
                    currToDel.DeletedBy = userId;
                    
                    _unitOfWork.GetRepository<Currency>().Update(currToDel);
                    _unitOfWork.Commit(userId);

                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Currency> GetAllActive(bool includeNested = false)
        {
            if (includeNested)
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .Include(c => c.PartialCurrencyPairs);
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled);
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
                        abbrv = c.Abbrv,
                        CurrencyType =
                        new
                        {
                            name = c.CurrencyType.Name,
                            typeShortForm = c.CurrencyType.TypeShortForm
                        },
                        name = c.Name,
                        walletTypeId = c.WalletTypeId
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
                        abbrv = c.Abbrv,
                        name = c.Name,
                        walletTypeId = c.WalletTypeId
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
                    .DistinctBy(c => c.Abbrv)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbrv,
                        CurrencyType = 
                        new {
                            name = c.CurrencyType.Name,
                            typeShortForm = c.CurrencyType.TypeShortForm
                        },
                        Name = c.Name
                    });
            }
            else
            {
                return _unitOfWork.GetRepository<Currency>().GetQueryable()
                    .AsNoTracking()
                    .Where(c => c.DeletedAt == null)
                    .Where(c => c.IsEnabled)
                    .DistinctBy(c => c.Abbrv)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Abbrv = c.Abbrv,
                        Name = c.Name
                    });
            }
        }

        /// <summary>
        /// Gets all pairs with currency rs with CurrencyId <=> CurrencyId
        /// 
        /// In this API, we'll only map currency to currency. Selection of which
        /// currencypair will only come at the margin setup section.
        /// 
        /// Key => CurrencyId
        /// Value => Tuple<long (CurrencyId), string (CurrencyName), string (Abbreviation)>
        /// </summary>
        /// <returns>All the pairs with currency rs.</returns>
        public IDictionary<long, IDictionary<long, Tuple<string, string>>> GetAllCurrencyPairings()
        {
            // Prep the result
            IDictionary<long, IDictionary<long, Tuple<string, string>>> result =
                new Dictionary<long, IDictionary<long, Tuple<string, string>>>();

            // Deque (Thought of using this because c++)
            // A double-ended queue, which provides O(1) indexed access, 
            // O(1) removals, insertions to the front and back, O(N) to everywhere else
            var pcPairs = new List<PartialCurrencyPair>(_unitOfWork.GetRepository<PartialCurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.Currency));

            // You see. two pcps make up one cpair.
            // If odd, c# defaults to rounding down (not really rounding... it just removes the decimals)
            long dataCount = pcPairs.Count();
            for (var i = 0; i < dataCount / 2; i++)
            {
                // pop!
                var currPcPair = pcPairs.First();
                pcPairs.Remove(currPcPair);
                
                var currCurrencyId = currPcPair.CurrencyId;

                // Retrieve the the counter/mainpair
                var subPCPair = pcPairs // Not the same currency
                    .SingleOrDefault(pcp => pcp.CurrencyPairId.Equals(currPcPair.CurrencyPairId) // Same Currency pair
                                            && !pcp.CurrencyId.Equals(currCurrencyId));

                // Second layer check
                if (subPCPair != null && !subPCPair.CurrencyId.Equals(currCurrencyId))
                {
                    // Remove the pair
                    pcPairs.Remove(subPCPair);

                    // Add the pair

                    // If let's BTC's currencyid already exists and if it exists, the subpair hasn't existed yet
                    if (result.ContainsKey(currCurrencyId) && !result[currCurrencyId].ContainsKey(subPCPair.CurrencyId))
                    {
                        // Add the next pair in
                        result[currCurrencyId].Add(subPCPair.CurrencyId,
                            Tuple.Create(subPCPair.Currency.Name, subPCPair.Currency.Abbrv));
                    }
                    else
                    {
                        // If not, add it
                        result.Add(currCurrencyId, new Dictionary<long, Tuple<string, string>>()
                        {
                            // Add item on initialization
                            {subPCPair.CurrencyId, Tuple.Create(subPCPair.Currency.Name, subPCPair.Currency.Abbrv)}
                        });
                    }
                }
                else
                {
                    {
                        // Wow bad
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves all the pairs with the currency rs in WalletTypeId <=> CurrencyId
        /// 
        /// Note that CurrencyId's WalletTypeId CANNOT match WalletTypeId.
        /// 
        /// Key = WalletTypeId, Value => Key = CounterCurrencyId. Value = CurrencyPairId
        /// </summary>
        /// <returns>All the pairs</returns>
        public IDictionary<long, IDictionary<long, long>> GetAllWalletTypeCurrencyPairings()
        {
            // Prep the result
            IDictionary<long, IDictionary<long, long>> result = new Dictionary<long, IDictionary<long, long>>();

            var pcPairs = _unitOfWork.GetRepository<PartialCurrencyPair>()
                .GetQueryable()
                .AsNoTracking()
                .Include(cp => cp.Currency)
                .Include(cp => cp.CurrencyPair)
                .ToList();

            // You see. two pcps make up one cpair.
            // If odd, c# defaults to rounding down (not really rounding... it just removes the decimals)
            long dataCount = pcPairs.Count();
            for (var i = 0; i < dataCount / 2; i++)
            {
                // pop!
                var currPCPair = pcPairs.First(cpcp => cpcp.IsMain);
                pcPairs.Remove(currPCPair);
                
                var currCurrencyId = currPCPair.CurrencyId;

                // Retrieve the the counter/mainpair
                var subPCPair = pcPairs // Not the same currency
                    .SingleOrDefault(pcp => pcp.CurrencyPairId.Equals(currPCPair.CurrencyPairId) // Same Currency pair
                                            && !pcp.IsMain);
                
                // Second layer check
                if (subPCPair != null && !subPCPair.CurrencyId.Equals(currCurrencyId))
                {
                    long currCurrencySourceId =
                        _unitOfWork.GetRepository<CurrencyPair>()
                            .Get(predicate: cp => cp.Id.Equals(subPCPair.CurrencyPairId))
                            .Single().CurrencySourceId;
                    
                    // Now let's see what pair this is
                    // We just need to make sure this is a semi-crypto pair because this API specifically only retrieves
                    // crypto-related pairs
                    if (currPCPair.Currency.WalletTypeId > 0 && subPCPair.Currency.WalletTypeId > 0) // Crypto-Crypto Pair
                    {
                        var walletTypeId = currPCPair.Currency.WalletTypeId;

                        // Remove the pair
                        pcPairs.Remove(subPCPair);

                        // Add the pair

                        // If let's BTC's currencyid already exists and if it exists, the subpair hasn't existed yet
                        if (result.ContainsKey(walletTypeId) &&
                            !result[walletTypeId].ContainsKey(subPCPair.CurrencyId))
                        {
                            // Add the next pair in
                            result[walletTypeId].Add(subPCPair.CurrencyId, currCurrencySourceId);
                        }
                        else
                        {
                            // If not, add it
                            result.Add(walletTypeId, new Dictionary<long, long>()
                            {
                                // Add item on initialization
                                {subPCPair.CurrencyId, currCurrencySourceId }
                            });
                        }
                    } else if (currPCPair.Currency.WalletTypeId > 0 && subPCPair.Currency.WalletTypeId.Equals(0)) // Crypto-Fiat Pair
                    {
                        var walletTypeId = currPCPair.Currency.WalletTypeId;

                        // Remove the pair
                        pcPairs.Remove(subPCPair);

                        // Add the pair

                        // If let's BTC's currencyid already exists and if it exists, the subpair hasn't existed yet
                        if (result.ContainsKey(walletTypeId) &&
                            !result[walletTypeId].ContainsKey(subPCPair.CurrencyId))
                        {
                            // Add the next pair in
                            result[walletTypeId].Add(subPCPair.CurrencyId, currCurrencySourceId);
                        }
                        else
                        {
                            // If not, add it
                            result.Add(walletTypeId, new Dictionary<long, long>()
                            {
                                // Add item on initialization
                                {subPCPair.CurrencyId, currCurrencySourceId}
                            });
                        }
                    }
                    // If you really want to implement Fiat-Crypto, do it here when you really need it
                }
            }

            return result;
        }
    }
}