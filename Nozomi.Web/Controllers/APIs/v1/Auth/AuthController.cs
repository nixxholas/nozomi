using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;

namespace Nozomi.Web.Controllers.APIs.v1.Auth
{
    public class AuthController : BaseApiController<AuthController>, IAuthController
    {
        private readonly IValidatingEvent _validatingEvent;

        public AuthController(ILogger<AuthController> logger, UserManager<User> userManager,
            IValidatingEvent validatingEvent)
            : base(logger, userManager)
        {
            _validatingEvent = validatingEvent;
        }

        [HttpPost]
        public dynamic EthAuth([FromBody]ValidateOwnerQuery vm)
        {
            return _validatingEvent.ValidateOwner(vm);
        }
    }
}
