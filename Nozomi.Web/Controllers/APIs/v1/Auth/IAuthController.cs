using Nozomi.Base.Blockchain.Auth.Query.Validating;

namespace Nozomi.Web.Controllers.APIs.v1.Auth
{
    public interface IAuthController
    {
        dynamic EthAuth(ValidateOwnerQuery request);
    }
}
