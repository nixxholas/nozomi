using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.Auth
{
    public class AuthController : BaseApiController<AuthController>, IAuthController
    {
        private readonly IValidatingEvent _validatingEvent;

        public AuthController(ILogger<AuthController> logger, IValidatingEvent validatingEvent)
            : base(logger)
        {
            _validatingEvent = validatingEvent;
        }

        [Authorize]
        [HttpGet]
        public async Task<object> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("http://localhost:6001/identity");

            ViewBag.Json = JArray.Parse(content).ToString();
            return Ok(JArray.Parse(content).ToString());
        }

        /// <summary>
        /// Web3 Authentication API
        ///
        /// Assist the user in obtaining a JWT token from Nozomi.Auth
        /// </summary>
        /// <param name="vm">ViewModel payload.</param>
        /// <returns>JWT object.</returns>
        [HttpPost]
        public object EthAuth([FromBody]ValidateOwnerQuery vm)
        {
            // Make sure the user owns this address with the correct proof.
            if (_validatingEvent.ValidateOwner(vm))
            {

            }

            // Invalid validation request, user is not an owner of this address.
            return BadRequest(_validatingEvent.ValidateOwner(vm));
        }
    }
}
