using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;

namespace Nozomi.Service
{
    public class NozomiServiceConstants
    {
        public static string CurrencyPairCachePrefix = "CURRPAIR_";
        
        /// <summary>
        /// A dictionary with a key of CurrencySourceId and the Currency pair ticker symbol to return the
        /// list of request components in question.
        /// </summary>
        /// <returns>ID of the CurrencyPair's Request</returns>
        public static Dictionary<Tuple<long, string>, long> CurrencySourceSymbolDictionary = 
            new Dictionary<Tuple<long, string>, long>();

        /// <summary>
        /// A dictionary with a key of the ticker symbol and the value of an array of currency pairs ids
        /// </summary>
        public static Dictionary<string, long[]> TickerSymbolDictionary = new Dictionary<string, long[]>();
        
        /// <summary>
        /// A dictionary with a key of the currencypairid and the value of the currencypair's ticker response.
        /// </summary>
        public static Dictionary<long, DistinctiveTickerResponse> CurrencyPairDictionary = 
            new Dictionary<long, DistinctiveTickerResponse>();
    }
}