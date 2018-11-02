using System;
using System.Collections.Generic;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Service
{
    public class NozomiServiceConstants
    {
        public static string CurrencyPairCachePrefix = "CURRPAIR_";
        
        /// <summary>
        /// A dictionary with a key of CurrencySourceId and the Currency pair ticker symbol to return the
        /// currency pair in question.
        /// </summary>
        /// <returns>ID of the CurrencyPair's Request</returns>
        public static Dictionary<Tuple<long, string>, long> CurrencyPairDictionary = 
            new Dictionary<Tuple<long, string>, long>(); 
    }
}