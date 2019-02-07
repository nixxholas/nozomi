using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data;
using Nozomi.Infra.Preprocessing.SignalR;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Hubs
{
    public class NozomiStreamHub : Hub<ITickerHubClient>
    {
        private readonly ILogger<NozomiStreamHub> _logger;
        public IDictionary<string, ICollection<NozomiSocketGroup>> _subscriptions;
        //private IEnumerable<CurrencyPair> _currencyPairs;
        //private readonly ICurrencyPairService _cpService;

        public NozomiStreamHub(ILogger<NozomiStreamHub> logger, ICurrencyPairService cpService)
        {
            //_cpService = cpService;
            _logger = logger;
            
            // Initialize
            _subscriptions = new Dictionary<string, ICollection<NozomiSocketGroup>>();
        }

        /// <summary>
        /// Allows clients to Broadcast data to Nozomi.
        ///
        /// We're unable to use Generics in a SignalR Hub class.
        /// https://stackoverflow.com/questions/21759577/using-generic-methods-on-signalr-hub
        /// </summary>
        /// <param name="data"></param>
        public async void BroadcastData(JObject data)
        {
            var channel = Channel.CreateUnbounded<NozomiResult<IEnumerable<JObject>>>();

            await channel.Writer.WriteAsync(new NozomiResult<IEnumerable<JObject>>()
            {
                ResultType = NozomiResultType.Success,
                Data = new[] { data }
            });
            
            channel.Writer.Complete();
        }

        /// <summary>
        /// Allows clients to subscribe to a specific group.
        ///
        /// Unsubscription is via .NET Core's native keep-alive rates at 10s.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<NozomiResult<string>> Subscribe(NozomiSocketGroup group)
        {
            switch (group)
            {
                case NozomiSocketGroup.Tickers:
                    if (_subscriptions.TryAdd(Context.ConnectionId, new List<NozomiSocketGroup>()
                    {
                        group // Add him in
                    }))
                    {
                        return new NozomiResult<string>(NozomiResultType.Success,
                            $"Subscribed to {group.GetDescription()}.");
                    }
                    
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Incorrect group ID.");
                default:
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Incorrect group identifier.");
            }
        }
        
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connected! ConnectionId: {Context.ConnectionId}");
            
            return base.OnConnectedAsync();
        }
        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_subscriptions.ContainsKey(Context.ConnectionId) && !_subscriptions.Remove(Context.ConnectionId))
            {
                _logger.LogCritical($"Failed to remove connectionId from subscriptions dictionary. " +
                                    $"id: {Context.ConnectionId}");
            }
            
            return base.OnDisconnectedAsync(exception);
        }
        
//        public async Task<NozomiResult<IEnumerable<CurrencyPair>>> Tickers(IEnumerable<CurrencyPair> currencyPairs = null)
//        {
//            var nozRes = new NozomiResult<IEnumerable<CurrencyPair>>()
//            {
//                ResultType = NozomiResultType.Success,
//                Data = currencyPairs
//            };
//            
//            return nozRes;
//        }

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
    }
}