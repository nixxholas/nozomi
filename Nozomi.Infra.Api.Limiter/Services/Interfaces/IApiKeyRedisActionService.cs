namespace Nozomi.Infra.Api.Limiter.Services.Interfaces
{
    public interface IApiKeyRedisActionService
    {
        /// <summary>
        /// Informs Redis about how much the user has filled up for the request he has made.
        /// </summary>
        /// <param name="apiKey">The API Key the user is currently using</param>
        /// <param name="fillAmount">The amount of tokens to fill the bucket</param>
        void Fill(string apiKey, long fillAmount = 1, string customDescription = null);

        /// <summary>
        /// Clears all actions made by this API Key.
        /// </summary>
        /// <param name="apiKey">The API Key for KVP evaluation</param>
        void Clear(string apiKey);
    }
}