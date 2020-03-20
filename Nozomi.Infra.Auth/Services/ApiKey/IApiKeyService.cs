namespace Nozomi.Infra.Auth.Services.ApiKey
{
    public interface IApiKeyService
    {
        void GenerateApiKey(string userId, string label = null);

        void RevokeApiKey(string apiKey, string userId = null);
    }
}