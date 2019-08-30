using System.ComponentModel;
using Nozomi.Base.Core.Helpers.Attributes;

namespace Nozomi.Data.Models.Currency
{
    /// <summary>
    /// Reminder to everyone that this component type is only for RequestComponents.
    /// </summary>
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
        AskPeriod = 7, // int
        [Comparable(false)]
        [Description("Sum of the 25 lowest ask sizes")]
        AskSize = 8, // float
        [Comparable(true)]
        [Description("Bid")]
        Bid = 2, // float
        [Comparable(false)]
        [Description("Bid period covered in days")]
        BidPeriod = 4, // int
        [Comparable(false)]
        [Description("Sum of the 25 highest bid sizes")]
        BidSize = 5, // float
        [Comparable(false)]
        [Description("Flash Return Rate")]
        FRR = 3,	// float - average of all fixed rate funding over the last hour
        [Comparable(true)]
        [Description("Daily price change")]
        DailyChange = 9, // float
        [Comparable(true)]
        [Description("Daily price change expressed in percentage terms")]
        DailyChangePerc = 10, // float
        [Comparable(true)]
        [Description("Daily volume")]
        DailyVolume = 12, // float
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
        LastPrice = 101, // float
        
        [Comparable(false)]
        [Description("The current circulating supply of this asset.")]
        CirculatingSupply = 1000,
        [Comparable(false)]
        [Description("The current block count of this crypto.")]
        BlockCount = 1005,
        [Comparable(false)]
        [Description("The current mining difficulty of this asset.")]
        Difficulty = 1010,
    }
}
