using System.ComponentModel;

namespace Nozomi.Realtime.Infra.Service.Hubs.Enumerators
{
    public enum TickerHubGroup
    {
        [Description("TickerHub_Ping")]
        Ping = 0,
        Subscription = 1,
        [Description("TickerHub_Ticker")]
        Ticker = 100
    }
}