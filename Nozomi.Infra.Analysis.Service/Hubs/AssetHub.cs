using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Nozomi.Infra.Analysis.Service.Hubs.Interfaces;

namespace Nozomi.Infra.Analysis.Service.Hubs
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