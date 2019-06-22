﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Data;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.ResponseModels.AnalysedComponent;
using Nozomi.Data.ResponseModels.Currency;
using Nozomi.Data.ResponseModels.CurrencyType;
using Nozomi.Infra.Preprocessing.SignalR;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Service.Events.Analysis.Interfaces;
using Nozomi.Service.Events.Interfaces;
using Nozomi.Service.Services.Interfaces;

namespace Nozomi.Service.Hubs
{
    public class NozomiStreamHub : Hub<INozomiStreamClient>
    {
        private readonly ILogger<NozomiStreamHub> _logger;
        private readonly IAnalysedComponentEvent _analysedComponentEvent;
        private readonly ICurrencyEvent _currencyEvent;
        public IDictionary<string, ICollection<NozomiSocketGroup>> _subscriptions;
        private readonly IHubContext<NozomiStreamHub, INozomiStreamClient> _nozomiStreamHub;

        public NozomiStreamHub(ILogger<NozomiStreamHub> logger, IAnalysedComponentEvent analysedComponentEvent, 
            ICurrencyEvent currencyEvent,
        IHubContext<NozomiStreamHub, INozomiStreamClient> nozomiStreamHub)
        {
            _logger = logger;
            _analysedComponentEvent = analysedComponentEvent;
            _currencyEvent = currencyEvent;
            _nozomiStreamHub = nozomiStreamHub;
            
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
//        public async void BroadcastData(JObject data)
//        {
//            var channel = Channel.CreateUnbounded<NozomiResult<IEnumerable<JObject>>>();
//
//            await channel.Writer.WriteAsync(new NozomiResult<IEnumerable<JObject>>()
//            {
//                ResultType = NozomiResultType.Success,
//                Data = new[] { data }
//            });
//            
//            channel.Writer.Complete();
//        }

        public async Task<NozomiResult<ICollection<DetailedCurrencyResponse>>> Currencies(string currencyType = "CRYPTO")
        {
            return new NozomiResult<ICollection<DetailedCurrencyResponse>>(_currencyEvent.GetAllDetailed(currencyType));
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
                case NozomiSocketGroup.Currencies:
                case NozomiSocketGroup.CurrencyTypes:
                    return await PropagateSubscription(group);
                default:
                    return new NozomiResult<string>(NozomiResultType.Failed,
                        "Incorrect group identifier.");
            }
        }

        private async Task<NozomiResult<string>> PropagateSubscription(NozomiSocketGroup group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group.GetDescription());

            switch (group)
            {
                case NozomiSocketGroup.Currencies:
                    await _nozomiStreamHub.Clients.Client(Context.ConnectionId).Currencies(_currencyEvent.GetAllDetailed());
                    break;
                case NozomiSocketGroup.CurrencyTypes:
                    await _nozomiStreamHub.Clients.Client(Context.ConnectionId).CurrencyTypes(
                        ObtainCurrencyTypeResponses(
                            _analysedComponentEvent.GetAllCurrencyTypeAnalysedComponents(0, true, true)));
                    break;
            }

            if (_subscriptions.ContainsKey(Context.ConnectionId))
            {
                if (!_subscriptions[Context.ConnectionId].Contains(group))
                {
                    _subscriptions[Context.ConnectionId].Add(group);
                            
                    return new NozomiResult<string>(NozomiResultType.Success,
                        $"Subscribed to {group.GetDescription()}.");
                }
                else
                {
                    // Already has been added if this is touched
                    return new NozomiResult<string>(NozomiResultType.Failed, 
                        "You're already subscribed.");
                }
            } else if (_subscriptions.TryAdd(Context.ConnectionId, new List<NozomiSocketGroup>()
            {
                group // Add him in
            }))
            {
                return new NozomiResult<string>(NozomiResultType.Success,
                    $"Subscribed to {group.GetDescription()}.");
            }
                    
            return new NozomiResult<string>(NozomiResultType.Failed,
                "Incorrect group ID.");
        }
        
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connected! ConnectionId: {Context.ConnectionId}");
            
            return base.OnConnectedAsync();
        }
        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_subscriptions.ContainsKey(Context.ConnectionId))
            {
                // Clear subs
                foreach (var sub in _subscriptions[Context.ConnectionId])
                {
                    Groups.RemoveFromGroupAsync(Context.ConnectionId, sub.GetDescription())
                        .Wait();
                }
                
                // Clear connection
                if (!_subscriptions.Remove(Context.ConnectionId))
                {
                    _logger.LogCritical($"Failed to remove connectionId from subscriptions dictionary. " +
                                        $"id: {Context.ConnectionId}");
                    return Task.FromException(new HubException("Unable to unsubscribe from your source " +
                                                               "subscriptions."));
                }
            }
            
            return base.OnDisconnectedAsync(exception);
        }

        private ICollection<CurrencyTypeResponse> ObtainCurrencyTypeResponses(ICollection<AnalysedComponent> analysedComponents)
        {
            var res = new List<CurrencyTypeResponse>();
            
            if (analysedComponents != null && analysedComponents.Count > 0)
            {
                foreach (var ac in analysedComponents)
                {
                    // Safetynet
                    if (ac.CurrencyType != null && !string.IsNullOrEmpty(ac.Value))
                    {
                        // If the CTR does not exist yet
                        if (!res.Any(ctr => ctr.Name.Equals(ac.CurrencyType.Name)))
                        {
                            // Create
                            res.Add(new CurrencyTypeResponse
                            {
                                Name = ac.CurrencyType.Name,
                                Components = new List<AnalysedComponentResponse>()
                                {
                                    new AnalysedComponentResponse
                                    {
                                        ComponentType = ac.ComponentType,
                                        Historical = ac.AnalysedHistoricItems
                                            .OrderBy(ahi => ahi.HistoricDateTime)
                                            .Select(ahi => ahi.Value)
                                            .ToList(),
                                        Value = ac.Value
                                    }
                                }
                            });
                        }
                        else
                        {
                            // Already exists, populate
                            res.SingleOrDefault(ctr => ctr.Name.Equals(ac.CurrencyType.Name))?.Components
                                .Add(new AnalysedComponentResponse
                                {
                                    ComponentType = ac.ComponentType,
                                    Historical = ac.AnalysedHistoricItems
                                        .OrderBy(ahi => ahi.HistoricDateTime)
                                        .Select(ahi => ahi.Value)
                                        .ToList(),
                                    Value = ac.Value
                                });
                        }
                    }
                }
            }

            return res;
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