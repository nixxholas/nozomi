namespace Nozomi.Infra.Auth.Services.ApiKey
{
    public interface IApiKeyService
    {
        void GenerateApiKey(string userId);
    }
}