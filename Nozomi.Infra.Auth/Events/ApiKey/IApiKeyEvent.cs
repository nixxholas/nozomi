using System.Collections.Generic;
using Nozomi.Base.Auth.ViewModels.ApiKey;

namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public interface IApiKeyEvent
    {
        bool Exists(string apiKey);

        string Reveal(string userId);

        IEnumerable<ApiKeyViewModel> ViewAll(string userId);
    }
}