using System.ComponentModel;

namespace Nozomi.Data.Models.Currency
{
    public enum ComponentType
    {
        [Description("Unknown")]
        Unknown = 0, // string
        [Description("Ask")]
        Ask = 1, // float
        [Description("Ask period covered in days")]
        Ask_Period = 7, // int
        [Description("Sum of the 25 lowest ask sizes")]
        Ask_Size = 8, // float
        [Description("Bid")]
        Bid = 2, // float
        [Description("Bid period covered in days")]
        Bid_Period = 4, // int
        [Description("Sum of the 25 highest bid sizes")]
        Bid_Size = 5, // float
        [Description("Flash Return Rate")]
        FRR = 3,	// float - average of all fixed rate funding over the last hour
        [Description("Amount that the last price has changed since yesterday")]
        Daily_Change = 9, // float
        [Description("Amount that the price has changed expressed in percentage terms")]
        Daily_Change_Perc = 10, // float
        [Description("Daily volume")]
        VOLUME = 12, // float
        [Description("Daily high")]
        High = 13, // float
        [Description("Daily low")]
        Low	= 14, // float
        
        // Orderbook related enumerators.
        Order = 100,
        [Description("Price of the last successfully closed order")]
        Last_Price = 101, // float
        
        [Description("The current circulating supply of this asset.")]
        Circulating_Supply = 1000
    }
}
