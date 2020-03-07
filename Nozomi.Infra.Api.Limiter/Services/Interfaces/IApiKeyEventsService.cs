using System;

namespace Nozomi.Infra.Api.Limiter.Services.Interfaces
{
    public interface IApiKeyEventsService
    {
        /// <summary>
        /// Informs Redis about how much the user has filled up for the request he has made.
        /// </summary>
        /// <param name="apiKey">The API Key the user is currently using</param>
        /// <param name="fillAmount">The amount of tokens to fill the bucket</param>
        /// <param name="customDescription">Additional text added past the api key to indicate this row's specific
        /// subject.</param>
        void Fill(string apiKey, long fillAmount = 1, string customDescription = null);

        /// <summary>
        /// Clears all actions made by this API Key.
        /// </summary>
        /// <param name="apiKey">The API Key for KVP evaluation</param>
        [Obsolete]
        void Clear(string apiKey);

        /// <summary>
        /// Creates a new redis key entry for ApiKeyEvents.
        /// </summary>
        /// <param name="key">The API Key to set as a key for the redis entry.</param>
        void Create(string key);
    }
}