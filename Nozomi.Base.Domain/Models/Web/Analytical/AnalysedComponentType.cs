using System.ComponentModel;

namespace Nozomi.Data.Models.Web.Analytical
{
    public enum AnalysedComponentType
    {
        Unknown = 0,
        [Description("Market Cap")]
        MarketCap = 1,
        [Description("Hourly Market Cap")]
        HourlyMarketCap = 2,
        [Description("Daily Market Cap")]
        DailyMarketCap = 3,
        [Description("Market Cap Change")]
        MarketCapChange = 6,
        [Description("Hourly Market Cap Change")]
        MarketCapHourlyChange = 7,
        [Description("Daily Market Cap Change")]
        MarketCapDailyChange = 8,
        [Description("Market Cap % Change")]
        MarketCapPctChange = 20,
        [Description("Hourly Market Cap % Change")]
        MarketCapHourlyPctChange = 21,
        [Description("Daily Market Cap % Change")]
        MarketCapDailyPctChange = 22,
        
        [Description("Price")]
        CurrentAveragePrice = 10,
        [Description("Hourly Average Price")]
        HourlyAveragePrice = 11,
        [Description("Daily Average Price")]
        DailyAveragePrice = 12,
        
        [Description("Change")]
        DailyPriceChange = 50,
        [Description("Weekly Change")]
        WeeklyPriceChange = 51,
        [Description("Monthly % Change")]
        MonthlyPriceChange = 52,
        
        [Description("% Change")]
        DailyPricePctChange = 70,
        [Description("Hourly % Change")]
        HourlyPricePctChange = 71,
        
        [Description("Volume")]
        DailyVolume = 80
    }
}