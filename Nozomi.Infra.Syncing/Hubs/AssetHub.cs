using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Syncing.Hubs.Interfaces;

namespace Nozomi.Infra.Syncing.Hubs
{
    public class AssetHub : Hub<IAssetHubClient>
    {
        private readonly ILogger<AssetHub> _logger;
        
        public AssetHub(ILogger<AssetHub> logger)
        {
            _logger = logger;
        }
    }
}