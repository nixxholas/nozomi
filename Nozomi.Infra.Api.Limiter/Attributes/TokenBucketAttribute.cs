using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Nozomi.Base.Auth.Models;
using Nozomi.Data.Models.Web;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Infra.Api.Limiter.Services;
using Nozomi.Infra.Api.Limiter.Services.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Options;

namespace Nozomi.Infra.Api.Limiter.Attributes
{
    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by API quotas.
    /// </summary>
    /// <remarks>
    /// Uses the StackExchange.Redis to store each client quota usage to the decorated route.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TokenBucketAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// A unique name for this route.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Weight of the API;
        ///
        /// How much it costs.
        /// </summary>
        /// <remarks>
        /// Reverts to 1 by default
        /// </remarks>
        public long Weight { get; set; } = 1;
        
        /// <summary>
        /// A text message that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
        /// </summary>
        public string Message { get; set; }

        // Some helpers
        // https://ignas.me/tech/custom-authentication-asp-net-core-20/
        public override void OnActionExecuting(ActionExecutingContext c)
        {
            if (c.HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationOptions.HeaderKey, 
                out var apiKey))
            {
                var serviceProvider = c.HttpContext.RequestServices;
                var redisEvent = serviceProvider.GetService<INozomiRedisEvent>();
                
                // Filtration checks
                if (redisEvent.Exists(apiKey, RedisDatabases.ApiKeyUser) // Ensure key exists in mapping 
                    // And if the ApiKeyUser list does contain this key and it has a value
                    && redisEvent.ContainsValue(apiKey, RedisDatabases.ApiKeyUser))
                {
                    var apiKeyRedisActionService = serviceProvider.GetService<IApiKeyEventsService>();
                    
                    // Push the usage and move on
                    apiKeyRedisActionService.Fill(apiKey, Weight);
                }
                else
                {
                    c.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotAcceptable;
                }
            }
            else
            {
                // Return 404 always.
                c.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
            }
            
            base.OnActionExecuting(c);
        }
    }
}