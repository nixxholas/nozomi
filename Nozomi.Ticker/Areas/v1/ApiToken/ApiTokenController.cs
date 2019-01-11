using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Data;
using Nozomi.Data.AreaModels.v1.ApiToken;

namespace Nozomi.Ticker.Areas.v1.ApiToken
{
    public class ApiTokenController : BaseController<ApiTokenController>, IApiTokenController
    {
        public ApiTokenController(ILogger<ApiTokenController> logger) : base(logger)
        {
        }

        public Task<NozomiResult<ICollection<ApiTokenResult>>> ApiTokens()
        {
            throw new System.NotImplementedException();
        }

        public Task<NozomiResult<ApiTokenResult>> CreateToken()
        {
            throw new System.NotImplementedException();
        }

        public Task<NozomiResult<ApiTokenRevocationResult>> RevokeToken()
        {
            throw new System.NotImplementedException();
        }
    }
}