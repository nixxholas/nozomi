using System.Collections.Generic;
using Nozomi.Base.Identity.AreaModels.v1.ApiToken;
using Nozomi.Base.Identity.ViewModels.Identity;

namespace Nozomi.Base.Identity.ViewModels.Areas.Manage.ApiTokens
{
    public class ApiTokensViewModel
    {
        public ICollection<ApiTokenResult> ApiTokens { get; set; }
    }
}