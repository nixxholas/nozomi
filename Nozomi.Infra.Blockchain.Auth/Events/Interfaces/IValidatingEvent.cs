using Nozomi.Base.Blockchain.Auth.Query.Validating;

namespace Nozomi.Infra.Blockchain.Auth.Events.Interfaces
{
    public interface IValidatingEvent
    {
        bool ValidateOwner(ValidateOwnerQuery request);
    }
}