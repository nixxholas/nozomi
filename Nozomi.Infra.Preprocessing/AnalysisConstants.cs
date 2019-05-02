using System.Collections;
using System.Collections.Generic;
using Nozomi.Data.Models.Web.Analytical;

namespace Nozomi.Preprocessing
{
    public static class AnalysisConstants
    {
        /// <summary>
        /// Component Types that have a very thin layer of history.
        /// </summary>
        public static ICollection<AnalysedComponentType> CompactAnalysedComponentTypes = 
            new List<AnalysedComponentType>()
            {
                AnalysedComponentType.HourlyMarketCap,
                AnalysedComponentType.DailyMarketCap,
                AnalysedComponentType.HourlyAveragePrice,
                AnalysedComponentType.DailyAveragePrice,
                AnalysedComponentType.DailyPriceChange,
                AnalysedComponentType.WeeklyPriceChange,
                AnalysedComponentType.MonthlyPriceChange,
                AnalysedComponentType.DailyPricePctChange,
                AnalysedComponentType.HourlyPricePctChange
            };
        
        public static ICollection<AnalysedComponentType> LiveAnalysedComponentTypes = new List<AnalysedComponentType>()
        {
            AnalysedComponentType.CurrentAveragePrice
        };
    }
}