using System.ComponentModel;
using Nozomi.Base.Core.Helpers.Attributes;

namespace Nozomi.Data.Models.Currency
{
    public enum ComponentType
    {
        [Comparable(false)]
        [Description("Unknown")]
        Unknown = 0, // string
        [Comparable(true)]
        [Description("Ask")]
        Ask = 1, // float
        [Comparable(false)]
        [Description("Ask period covered in days")]
        Ask_Period = 7, // int
        [Comparable(false)]
        [Description("Sum of the 25 lowest ask sizes")]
        Ask_Size = 8, // float
        [Comparable(true)]
        [Description("Bid")]
        Bid = 2, // float
        [Comparable(false)]
        [Description("Bid period covered in days")]
        Bid_Period = 4, // int
        [Comparable(false)]
        [Description("Sum of the 25 highest bid sizes")]
        Bid_Size = 5, // float
        [Comparable(false)]
        [Description("Flash Return Rate")]
        FRR = 3,	// float - average of all fixed rate funding over the last hour
        [Comparable(true)]
        [Description("Amount that the last price has changed since yesterday")]
        Daily_Change = 9, // float
        [Comparable(true)]
        [Description("Amount that the price has changed expressed in percentage terms")]
        Daily_Change_Perc = 10, // float
        [Comparable(true)]
        [Description("Daily volume")]
        VOLUME = 12, // float
        [Comparable(true)]
        [Description("Daily high")]
        High = 13, // float
        [Comparable(true)]
        [Description("Daily low")]
        Low	= 14, // float
        
        // Orderbook related enumerators.
        [Comparable(false)]
        [Description("Order")]
        Order = 100,
        [Comparable(true)]
        [Description("Price of the last successfully closed order")]
        Last_Price = 101, // float
        
        [Comparable(false)]
        [Description("The current circulating supply of this asset.")]
        Circulating_Supply = 1000
    }
}
