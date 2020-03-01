using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Nozomi.Preprocessing.Attributes
{
    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time.
    ///
    /// https://stackoverflow.com/questions/33969/best-way-to-implement-request-throttling-in-asp-net-mvc/1318059#1318059
    /// </summary>
    /// <remarks>
    /// Uses the current System.Web.Caching.Cache to store each client request to the decorated route.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// A unique name for this Throttle.
        /// </summary>
        /// <remarks>
        /// We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1"
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// The number of milliseconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Milliseconds { get; set; }

        /// <summary>
        /// A text message that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
        /// </summary>
        public string Message { get; set; }

        public override void OnActionExecuting(ActionExecutingContext c)
        {
            var memoryCache = c.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            
            var key = string.Concat(Name, "-",
                // https://stackoverflow.com/questions/28664686/how-do-i-get-client-ip-address-in-asp-net-core
                c.HttpContext.Connection.RemoteIpAddress);
            var allowExecute = false;

            var cacheItem = memoryCache.Get(key);
            // Ensure that the accessor didn't access recently
            if (cacheItem == null)
            {
                // Setup the expiration timing
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    Priority = CacheItemPriority.Normal,
                    AbsoluteExpiration = DateTime.Now.AddMilliseconds(Milliseconds),
                };
                // Ensure expiry in 3 seconds
                // .SetSlidingExpiration(TimeSpan.FromSeconds(Seconds));

                // Record down this request
                memoryCache.Set(key, DateTime.UtcNow.AddMilliseconds(Milliseconds), cacheEntryOptions);

                allowExecute = true;
            }

            if (!allowExecute)
            {
                if (String.IsNullOrEmpty(Message))
                    Message = "You may only perform this action every {n} seconds.";

                c.Result = new ContentResult {Content = Message.Replace("{n}", 
                    (Milliseconds/1000).ToString())};
                // see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                c.HttpContext.Response.StatusCode = (int) HttpStatusCode.Conflict;
            }
        }
    }
}