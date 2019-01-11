using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.ApiToken;

namespace Nozomi.Ticker.Areas.v1.ApiToken
{
    public interface IApiTokenController
    {
        Task<NozomiResult<ICollection<ApiTokenResult>>> ApiTokens();

        Task<NozomiResult<ApiTokenResult>> CreateToken();

        Task<NozomiResult<ApiTokenRevocationResult>> RevokeToken();
    }
}