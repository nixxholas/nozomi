using System.ComponentModel;

namespace Nozomi.Infra.Preprocessing.SignalR
{
    /// <summary>
    /// Defines all groups available to a SignalR/Websocket client.
    /// </summary>
    public enum NozomiSocketGroup
    {
        [Description("Ticker Stream")]
        Tickers = 1,
        [Description("Currency Stream")]
        Currencies = 2,
        [Description("Market Stream")]
        MarketCaps = 3
    }
}