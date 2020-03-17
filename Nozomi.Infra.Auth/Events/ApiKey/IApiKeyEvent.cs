using System.Linq;

namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public interface IApiKeyEvent
    {
        bool Exists(string apiKey);

        IQueryable<string> ViewAll(string userId);
    }
}