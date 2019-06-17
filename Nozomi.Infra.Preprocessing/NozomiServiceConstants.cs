using System;
using System.Collections;
using System.Collections.Generic;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels;
using Nozomi.Data.ResponseModels.Ticker;

namespace Nozomi.Preprocessing
{
    public static class NozomiServiceConstants
    {
        public const string CurrencyPairCachePrefix = "CURRPAIR_";
        public const string JwtIssuerName = "NozomiJwt";
        public const string ApiTokenHeaderKey = "NozApiToken";
        
        /// <summary>
        /// A collection containing ALL active and enabled sources.
        /// </summary>
        public static IEnumerable<Source> Sources { get; set; }

        public static List<KeyValuePair<string, int>> analysedComponentTypes =
            EnumHelper.GetEnumValuesAndDescriptions<AnalysedComponentType>();
        public static List<KeyValuePair<string, int>> CurrencyPropertyTypes =
            EnumHelper.GetEnumValuesAndDescriptions<CurrencyPropertyType>();
        public static List<KeyValuePair<string, int>> requestComponentTypes = 
            EnumHelper.GetEnumValuesAndDescriptions<ComponentType>();
        public static List<KeyValuePair<string, int>> requestTypes =
            EnumHelper.GetEnumValuesAndDescriptions<RequestType>();
        public static List<KeyValuePair<string, int>> responseTypes =
            EnumHelper.GetEnumValuesAndDescriptions<ResponseType>();
        public static List<KeyValuePair<string, int>> requestPropertyTypes =
            EnumHelper.GetEnumValuesAndDescriptions<RequestPropertyType>();
        public static List<KeyValuePair<string, int>> currencyPairType =
            EnumHelper.GetEnumValuesAndDescriptions<CurrencyPairType>();
        
        /// <summary>
        /// Let's say if an AnalysedComponent Updates every second,
        /// It means that the maximum rows obtainable would be = to this.
        ///
        /// i.e. 1 hour of data, NozomiServiceConstants.AnalysedComponentTakeoutLimit is 1000 = 3600 rows. You need 4 queries to do that.
        /// 4000 rows will cover 3.6k
        /// </summary>
        public const int AnalysedComponentTakeoutLimit = 5000;
        
        public const int AnalysedHistoricItemTakeoutLimit = 5000;

        public const int CurrencyTypeTakeoutLimit = 20;

        public const int CurrencyPropertyTakeoutLimit = 500;
        
        public const int RequestComponentTakeoutLimit = 10000;
    }
}