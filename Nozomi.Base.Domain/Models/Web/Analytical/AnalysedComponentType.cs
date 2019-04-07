using System.ComponentModel;

namespace Nozomi.Data.Models.Web.Analytical
{
    public enum AnalysedComponentType
    {
        Unknown = 0,
        [Description("Market Cap")]
        MarketCap = 1,
        
        [Description("Price")]
        CurrentAveragePrice = 10,
        [Description("Hourly Average Price")]
        HourlyAveragePrice = 11,
        
        [Description("Change")]
        DailyPriceChange = 50,
        [Description("Weekly % Change")]
        WeeklyPriceChange = 51,
        [Description("Monthly % Change")]
        MonthlyPriceChange = 52,
        
        [Description("% Change")]
        DailyPricePctChange = 70,
        
        [Description("Volume")]
        DailyVolume = 80
    }
}