using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Nozomi.Repo.Identity.Data;

namespace Nozomi.Repo.Identity.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly NozomiAuthContext _context;
        private readonly ILogger _logger;
 
        public ClientStore(NozomiAuthContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("ClientStore");
        }
 
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _context.Clients.First(t => t.ClientId == clientId);
            client.MapDataFromEntity();
            return Task.FromResult(client.Client);
        }
    }
}