using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;

namespace Nozomi.Web2.Controllers.v1.Auth
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
            var idtok = await HttpContext.GetTokenAsync("id_token");
            var cookies = HttpContext.Request.Cookies;

            var client = new HttpClient();
            //client.SetToken(CookieAuthenticationDefaults.AuthenticationScheme, accessToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var res = await client.GetStringAsync("https://localhost:6001/connect/userinfo");
            //var content = await res.RequestMessage.Content.ReadAsStringAsync();

            ViewBag.Json = JToken.Parse(res).ToString();
            return Ok(JToken.Parse(res).ToString());
        }

        [Authorize]
        [HttpGet]
        public async Task<object> SignOut()
        {
            await HttpContext.SignOutAsync();
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
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
