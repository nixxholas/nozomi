using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Infra.Api.Limiter.Events.Interfaces;
using Nozomi.Preprocessing;
using Nozomi.Preprocessing.Options;

namespace Nozomi.Infra.Api.Limiter.Handlers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IServiceProvider _serviceProvider;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, 
            UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider) 
            : base (options, logger, encoder, clock) 
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync() 
        {
            var token = Request.Headers[ApiKeyAuthenticationOptions.HeaderKey];

            if (string.IsNullOrEmpty(token)) {
                return Task.FromResult (AuthenticateResult.Fail ("Token is null"));
            }

            var nozomiRedisEvent = _serviceProvider.GetRequiredService<INozomiRedisEvent>();
            var isValidToken = nozomiRedisEvent.Exists(token, RedisDatabases.ApiKeyUser);

            if (!isValidToken) {
                return Task.FromResult (AuthenticateResult.Fail ($"Invalid token {token}."));
            }

            var claims = new [] { new Claim ("token", token) };
            var identity = new ClaimsIdentity (claims, nameof (ApiKeyAuthenticationHandler));
            var ticket = new AuthenticationTicket (new ClaimsPrincipal (identity), Scheme.Name);
            return Task.FromResult (AuthenticateResult.Success (ticket));
        }
    }
}