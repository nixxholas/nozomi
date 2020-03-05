namespace Nozomi.Infra.Api.Limiter.Events.Interfaces
{
    public interface IApiKeyRedisActionEvent
    {
        /// <summary>
        /// Can this API Key spill into the leaky bucket of the user?
        /// </summary>
        /// <param name="key">The API Key in question</param>
        /// <returns>True if we can 'pour' more into the user's bucket</returns>
        bool CanPour(string key);
    }
}