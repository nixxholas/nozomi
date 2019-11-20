using Nozomi.Base.Blockchain.Auth.Query.Validating;

namespace Nozomi.Web2.Controllers.APIs.v1.Auth
{
    public interface IAuthController
    {
        object EthAuth(ValidateOwnerQuery request);
    }
}
