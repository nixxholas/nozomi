using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Nozomi.Repo.Identity.Data;

namespace Nozomi.Service.Identity.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly NozomiAuthContext _context;
        private readonly ILogger _logger;
 
        public ResourceStore(NozomiAuthContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("ResourceStore");
        }
 
        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apiResource = _context.ApiResources.First(t => t.ApiResourceName == name);
            apiResource.MapDataFromEntity();
            return Task.FromResult(apiResource.ApiResource);
        }
 
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
 
 
            var apiResources = new List<ApiResource>();
            var apiResourcesEntities = _context.ApiResources.Where(i => scopeNames.Contains(i.ApiResourceName));
 
            foreach (var apiResourceEntity in apiResourcesEntities)
            {
                apiResourceEntity.MapDataFromEntity();
 
                apiResources.Add(apiResourceEntity.ApiResource);
            }
 
            return Task.FromResult(apiResources.AsEnumerable());
        }
 
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));
 
            var identityResources = new List<IdentityResource>();
            var identityResourcesEntities =
                _context.IdentityResources.Where(i => scopeNames.Contains(i.IdentityResourceName));
 
            foreach (var identityResourceEntity in identityResourcesEntities)
            {
                identityResourceEntity.MapDataFromEntity();
 
                identityResources.Add(identityResourceEntity.IdentityResource);
            }
 
            return Task.FromResult(identityResources.AsEnumerable());
        }
 
        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResourcesEntities = _context.ApiResources.ToList();
            var identityResourcesEntities = _context.IdentityResources.ToList();
 
            var apiResources = new List<ApiResource>();
            var identityResources= new List<IdentityResource>();
 
            foreach (var apiResourceEntity in apiResourcesEntities)
            {
                apiResourceEntity.MapDataFromEntity();
 
                apiResources.Add(apiResourceEntity.ApiResource);
            }
 
            foreach (var identityResourceEntity in identityResourcesEntities)
            {
                identityResourceEntity.MapDataFromEntity();
 
                identityResources.Add(identityResourceEntity.IdentityResource);
            }
 
            var result = new Resources(identityResources, apiResources);
            return Task.FromResult(result);
        }
    }
}