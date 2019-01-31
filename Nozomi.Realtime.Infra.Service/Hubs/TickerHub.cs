using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.ResponseModels;
using Nozomi.Realtime.Infra.Service.Hubs.Enumerators;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Realtime.Infra.Service.Hubs
{
    /// <summary>
    /// The SignalR Hub for Ticker-related endpoints. 
    /// </summary>
    public class TickerHub : Hub
    {
        public const string _hubName = "NozomiTickerHub_";
        private IEnumerable<CurrencyPair> _currencyPairs;
        private readonly ITickerEvent _tickerEvent;
        private readonly ICurrencyPairService _cpService;
        private readonly ILogger<TickerHub> _logger;

        public TickerHub(ITickerEvent tickerEvent, ICurrencyPairService cpService,
            ILogger<TickerHub> logger)
        {
            _tickerEvent = tickerEvent;
            _cpService = cpService;
            _logger = logger;
        }
        
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Allows Nozomi to Broadcast data to subscribed clients.
        ///
        /// We're unable to use Generics in a SignalR Hub class.
        /// https://stackoverflow.com/questions/21759577/using-generic-methods-on-signalr-hub
        /// </summary>
        /// <param name="data"></param>
//        public async void BroadcastData(TickerHubGroup hubGroup)
//        {
//            var channel = Channel.CreateUnbounded<NozomiResult<IDictionary<KeyValuePair<string, string>, 
//                DistinctiveTickerResponse>>>();
//
//            await channel.Writer.WriteAsync(new NozomiResult<IDictionary<KeyValuePair<string, string>,
//                DistinctiveTickerResponse>>(payload));
//
//        }
        
        public Task<IDictionary<KeyValuePair<string, string>, DistinctiveTickerResponse>> Tickers()
        {
            return Task.FromResult(_tickerEvent.GetAll());
        }

        // We can use this to return a payload
//        public async Task<ChannelReader<NozomiResult<IEnumerable<CurrencyPair>>>> SubscribeToAll()
//        {
//            // Initialize an unbounded channel
//            // 
//            // Unbounded Channels have no boundaries, allowing the server/client to transmit
//            // limitless amounts of payload. Bounded channels have limits and will tend to 
//            // drop the clients after awhile.
//            var channel = Channel.CreateUnbounded<NozomiResult<IEnumerable<CurrencyPair>>>();
//
//            _ = WriteToChannel(channel.Writer); // Write all Currency Pairs to the channel
//
//            // Return the reader
//            return channel.Reader;
//
//            // This is a nested method, allowing us to write repeated methods
//            // with the same semantic conventions while maintaining conformity.
//            async Task WriteToChannel(ChannelWriter<NozomiResult<IEnumerable<CurrencyPair>>> writer)
//            {
//                // Pull in the latest data
//                _currencyPairs = _cpService.GetAllActive();
//
//                // Iterate them currency pairs
//                foreach (var cPair in _currencyPairs)
//                {
//                    // Write one by one, and the client receives them one by one as well
//                    await writer.WriteAsync(new NozomiResult<IEnumerable<CurrencyPair>>()
//                    {
//                        ResultType = (cPair != null) ? NozomiResultType.Success : NozomiResultType.Failed,
//                        Data = new[] {cPair}
//                    });
//                }
//
//                // Beep the client, telling them you're done
//                writer.Complete();
//            }
//        }

        public async Task Register(TickerHubGroup hubGroup)
        {
            _logger.LogInformation($"ConnectionId: {Context.ConnectionId} is subscribing to " +
                                   $"{hubGroup.GetDescription()}");
            
            await Groups.AddToGroupAsync(Context.ConnectionId, hubGroup.GetDescription());
        }

        public async Task Unregister(TickerHubGroup hubGroup)
        {
            _logger.LogInformation($"ConnectionId: {Context.ConnectionId} is un-subscribing from " +
                                   $"{hubGroup.GetDescription()}");
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, hubGroup.GetDescription());
        }
    }
}