using System;
using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Preprocessing
{
    public static class NozomiServiceConstants
    {
        public const string CurrencyPairCachePrefix = "CURRPAIR_";
        public const string JwtIssuerName = "NozomiJwt";
        public const string ApiTokenHeaderKey = "NozApiToken";
        
        /// <summary>
        /// A dictionary with a key of CurrencySourceId and the Currency pair ticker symbol to return the
        /// currency pair id in question.
        /// </summary>
        /// <returns>ID of the CurrencyPair's Request</returns>
        public static Dictionary<Tuple<string, string>, long> CurrencySourceSymbolDictionary = 
            new Dictionary<Tuple<string, string>, long>();

        /// <summary>
        /// A dictionary with a key of the ticker symbol and the value of an array of currency pairs ids
        /// </summary>
        public static Dictionary<string, LinkedList<long>> TickerSymbolDictionary = 
            new Dictionary<string, LinkedList<long>>();
        
        /// <summary>
        /// A dictionary with a key of the currencypairid and the value of the currencypair's ticker response.
        /// </summary>
        public static Dictionary<long, DistinctiveTickerResponse> CurrencyPairDictionary = 
            new Dictionary<long, DistinctiveTickerResponse>();
        
        /// <summary>
        /// A collection containing ALL tickers unique to their source and ticker abbreviation.
        /// </summary>
        public static ICollection<UniqueTickerResponse> UniqueCurrencyPairs = 
            new LinkedList<UniqueTickerResponse>();
    }
}