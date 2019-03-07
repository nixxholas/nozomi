namespace Nozomi.Data.Models.Web.Analytical
{
    public enum AnalysedComponentType
    {
        Unknown = 0,
        MarketCap = 1,
        CurrentAveragePrice = 10,
        DailyPriceChange = 50,
        WeeklyPriceChange = 51,
        MonthlyPriceChange = 52,
        DailyPricePctChange = 70,
        DailyVolume = 80
    }
}