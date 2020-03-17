using System.Linq;

namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public interface IApiKeyEvent
    {
        bool Exists(string apiKey);

        string View(string apiKey, string userId = null);

        IQueryable<string> ViewAll(string userId);
    }
}