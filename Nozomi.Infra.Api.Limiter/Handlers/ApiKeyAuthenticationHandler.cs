using System;
using System.Linq;
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
    /// <summary>
    /// An Auth handler to handle authentication for a .NET Core project via Api keys.
    ///
    /// This helps to resolve dependency issues when utilises a non-conventional method.
    /// https://stackoverflow.com/questions/47324129/no-authenticationscheme-was-specified-and-there-was-no-defaultchallengescheme-f
    /// </summary>
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
            var token = Request.Headers[ApiKeyAuthenticationOptions.HeaderKey].FirstOrDefault();

            if (string.IsNullOrEmpty(token)) {
                return Task.FromResult (AuthenticateResult.Fail ("Token is null"));
            }

            var nozomiRedisEvent = _serviceProvider.GetRequiredService<INozomiRedisEvent>();
            if (!nozomiRedisEvent.Exists(token, RedisDatabases.ApiKeyUser)) {
                return Task.FromResult (AuthenticateResult.Fail ($"Invalid token {token}."));
            }

            var claims = new [] { new Claim ("token", token) };
            var identity = new ClaimsIdentity (claims, nameof (ApiKeyAuthenticationHandler));
            var ticket = new AuthenticationTicket (new ClaimsPrincipal (identity), Scheme.Name);
            Logger.LogInformation($"ApiKeyAuthenticationHandler: token ending with " +
                                  $"{token.Substring(token.Length - 8)} is authorised.");
            return Task.FromResult (AuthenticateResult.Success(ticket));
        }
    }
}