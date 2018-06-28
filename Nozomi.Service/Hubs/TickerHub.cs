using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Nozomi.Data;
using Nozomi.Data.CurrencyModels;
using Nozomi.Data.HubModels.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Hubs
{
    public class TickerHub : Hub, ITickerHubClient
    {
        private IEnumerable<CurrencyPair> _currencyPairs;
        private readonly ICurrencyPairService _cpService;
        
        public TickerHub(ICurrencyPairService cpService)
        {
            _cpService = cpService;
        }
        
        // We can use this to return a payload
        public async Task<ChannelReader<NozomiResult<CurrencyPair>>> SubscribeToAll()
        {
            // Initialize an unbounded channel
            // 
            // Unbounded Channels have no boundaries, allowing the server/client to transmit
            // limitless amounts of payload. Bounded channels have limits and will tend to 
            // drop the clients after awhile.
            var channel = Channel.CreateUnbounded<NozomiResult<CurrencyPair>>();

            _ = WriteToChannel(channel.Writer); // Write all Currency Pairs to the channel

            // Return the reader
            return channel.Reader;

            // This is a nested method, allowing us to write repeated methods
            // with the same semantic conventions while maintaining conformity.
            async Task WriteToChannel(ChannelWriter<NozomiResult<CurrencyPair>> writer)
            {
                while (true)
                {
                    // Pull in the latest data
                    _currencyPairs = _cpService.GetAllActive();

                    // Iterate them currency pairs
                    foreach (var cPair in _currencyPairs)
                    {
                        // Write one by one, and the client receives them one by one as well
                        await writer.WriteAsync(new NozomiResult<CurrencyPair>()
                        {
                            Success = (cPair != null),
                            ResultType = (cPair != null) ? NozomiResultType.Success : NozomiResultType.Failed,
                            Data = new[] { cPair }
                        });
                    }

                    writer.Complete();

                    await Task.Delay(1000);
                }
                
                // Beep the client, telling them you're done
                //writer.Complete();
            }
        }
    }
}