using System.Collections.Generic;
using Nozomi.Data.ViewModels.ApiKey;

namespace Nozomi.Infra.Auth.Events.ApiKey
{
    public interface IApiKeyEvent
    {
        bool Exists(string apiKey);

        IEnumerable<ApiKeyViewModel> ViewAll(string userId);
    }
}