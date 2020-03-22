using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Helpers.Crypto;
using Nozomi.Infra.Auth.Events.ApiKey;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Services.ApiKey
{
    public class ApiKeyService : BaseService<ApiKeyService, AuthDbContext>, IApiKeyService
    {
        private readonly IApiKeyEvent _apiKeyEvent;
        private readonly IUserEvent _userEvent;

        public ApiKeyService(ILogger<ApiKeyService> logger, AuthDbContext context,
            IApiKeyEvent apiKeyEvent, IUserEvent userEvent) : base(logger, context)
        {
            _apiKeyEvent = apiKeyEvent;
            _userEvent = userEvent;
        }

        public ApiKeyService(IHttpContextAccessor contextAccessor, ILogger<ApiKeyService> logger,
            AuthDbContext context, IApiKeyEvent apiKeyEvent, IUserEvent userEvent)
            : base(contextAccessor, logger, context)
        {
            _apiKeyEvent = apiKeyEvent;
            _userEvent = userEvent;
        }

        public void GenerateApiKey(string userId, string label = null)
        {
            var newKey = Randomizer.GenerateRandomCryptographicKey(32);
            while (_apiKeyEvent.Exists(newKey)) // Ensure truly random
            {
                newKey = Randomizer.GenerateRandomCryptographicKey(32);
            }

            if (_userEvent.Exists(userId))
            {
                _context.ApiKeys.Add(new Base.Auth.Models.ApiKey
                    {
                        Label = label,
                        CreatedAt = DateTime.UtcNow,
                        Value = newKey,
                        UserId = userId
                    });
                
                _context.SaveChanges();
                _logger.LogInformation($"{_serviceName} GenerateApiKey: Api key {newKey} generated for user" +
                                       $" {userId}");
                return;
            }

            _logger.LogWarning($"{_serviceName} GenerateApiKey: Invalid user {userId}.");
            throw new KeyNotFoundException("User not found.");
        }

        public void RevokeApiKey(string apiKeyGuid, string userId = null)
        {
            if (Guid.TryParse(apiKeyGuid, out var parsedGuid))
            {
                var revokingKey = _context.ApiKeys.AsTracking()
                    .SingleOrDefault(e => e.Guid.Equals(parsedGuid));

                if (revokingKey != null)
                {
                    // If we're filtering via userId and if the userId doesn't match
                    if (!string.IsNullOrEmpty(userId) && !revokingKey.UserId.Equals(userId))
                        throw new InvalidConstraintException("Invalid user for this api key.");

                    // Else all is good, revoke it
                    _context.ApiKeys.Remove(revokingKey);
                    _context.SaveChanges();

                    _logger.LogInformation($"{_serviceName} RevokeApiKey: Api key {parsedGuid} revoked " +
                                           "successfully.");
                    return;
                }

                _logger.LogWarning($"{_serviceName} RevokeApiKey: Api key {parsedGuid} is not found..");
            }

            throw new InvalidOperationException("Invalid api key.");
        }
    }
}