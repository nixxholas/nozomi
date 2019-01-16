using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Base.Identity.ViewModels.AreaModels.v1.ApiToken;
using Nozomi.Data;

namespace Nozomi.Ticker.Areas.v1.ApiToken
{
    public interface IApiTokenController
    {
        Task<NozomiResult<ICollection<ApiTokenResult>>> ApiTokens();

        Task<NozomiResult<ApiTokenResult>> GenerateToken(string label = null);

        Task<NozomiResult<ApiTokenRevocationResult>> RevokeToken(string tokenGuid);
    }
}