using System.Collections.Generic;
using Nozomi.Base.Identity.Models.Identity;

namespace Nozomi.Base.Identity.Models.Areas.Manage.ApiTokens
{
    public class ApiTokensViewModel
    {
        public ICollection<ApiToken> ApiTokens { get; set; }
    }
}