using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Infra.Preprocessing.SignalR.Hubs.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Service.Events.Interfaces;

namespace Nozomi.Service.Hubs
{
    public class NozomiSourceStreamHub : Hub<ISourceHubClient>
    {
        public const string NozomiSourceStreamHubStr = "NSSHub_"; 
        private readonly ILogger<NozomiSourceStreamHub> _logger;
        private readonly ITickerEvent _tickerEvent;

        public static IDictionary<string, ICollection<string>> _subscriptions = 
            new Dictionary<string, ICollection<string>>();
        
        public NozomiSourceStreamHub(ILogger<NozomiSourceStreamHub> logger,
            ITickerEvent tickerEvent)
        {
            _logger = logger;
            _tickerEvent = tickerEvent;
        }

        public async Task<NozomiResult<string>> Subscribe(string sourceAbbrv)
        {
            if (_subscriptions.ContainsKey(Context.ConnectionId))
            {
                _subscriptions[Context.ConnectionId].Add(NozomiSourceStreamHubStr + sourceAbbrv);
                await Groups.AddToGroupAsync(Context.ConnectionId, NozomiSourceStreamHubStr + sourceAbbrv);
                
                return new NozomiResult<string>(NozomiResultType.Success,
                    $"Successfully subscribed to the source stream of {sourceAbbrv}!");
            }
            else if (_subscriptions.TryAdd(Context.ConnectionId, new List<string>
            {
                NozomiSourceStreamHubStr + sourceAbbrv
            }))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, NozomiSourceStreamHubStr + sourceAbbrv);
                
                return new NozomiResult<string>(NozomiResultType.Success,
                    $"Successfully subscribed to the source stream of {sourceAbbrv}!");
            }
            
            return new NozomiResult<string>(NozomiResultType.Failed,
                "Invalid Source.");
        }
        
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connected to Nozomi's source stream! " +
                                   $"ConnectionId: {Context.ConnectionId}");
            
            return base.OnConnectedAsync();
        }
        
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_subscriptions.ContainsKey(Context.ConnectionId))
            {
                // Clear subs
                foreach (var sub in _subscriptions[Context.ConnectionId])
                {
                    Groups.RemoveFromGroupAsync(Context.ConnectionId, NozomiSourceStreamHubStr + sub)
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
    }
}