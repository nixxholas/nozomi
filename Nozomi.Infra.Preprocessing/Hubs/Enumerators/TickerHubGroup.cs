using System.ComponentModel;

namespace Nozomi.Preprocessing.Hubs.Enumerators
{
    public enum TickerHubGroup
    {
        [Description("TickerHubPing")]
        Ping = 0,
        Subscription = 1,
        [Description("TickerGroup")]
        Ticker = 100
    }
}