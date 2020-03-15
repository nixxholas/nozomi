namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public interface IApiKeyEvent
    {
        bool Exists(string apiKey);
    }
}