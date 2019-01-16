using System.Collections.Generic;
using Nozomi.Base.Identity.ViewModels.AreaModels.v1.ApiToken;

namespace Nozomi.Base.Identity.ViewModels.Manage.ApiTokens
{
    public class ApiTokensViewModel
    {
        public ICollection<ApiTokenResult> ApiTokens { get; set; }
    }
}