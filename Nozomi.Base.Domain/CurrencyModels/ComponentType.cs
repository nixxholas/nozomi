using System;
using System.ComponentModel;

namespace Nozomi.Data.CurrencyModels
{
    public enum ComponentType
    {
        [Description("Unknown")]
        Unknown = 0, // string
        [Description("Ask")]
        Ask = 1, // float
        [Description("Bid")]
        Bid = 2, // float
        [Description("Flash Return Rate")]
        FRR = 3,	// float - average of all fixed rate funding over the last hour
        [Description("Bid period covered in days")]
        BID_PERIOD = 4, // int
        [Description("Sum of the 25 highest bid sizes")]
        BID_SIZE = 5, // float
        [Description("Price of last lowest ask")]
        ASK	= 6, // float
        [Description("Ask period covered in days")]
        ASK_PERIOD = 7, // int
        [Description("Sum of the 25 lowest ask sizes")]
        ASK_SIZE = 8, // float
        [Description("Amount that the last price has changed since yesterday")]
        DAILY_CHANGE = 9, // float
        [Description("Amount that the price has changed expressed in percentage terms")]
        DAILY_CHANGE_PERC = 10, // float
        [Description("Price of the last trade")]
        LAST_PRICE = 11, // float
        [Description("Daily volume")]
        VOLUME = 12, // float
        [Description("Daily high")]
        HIGH = 13, // float
        [Description("Daily low")]
        LOW	= 14 // float
    }
}
