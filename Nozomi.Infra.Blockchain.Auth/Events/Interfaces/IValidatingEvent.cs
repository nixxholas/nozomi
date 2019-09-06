namespace Nozomi.Infra.Blockchain.Auth.Events.Interfaces
{
    public interface IValidatingEvent
    {
        bool ValidateOwner(string claimerAddress, string signature, string rawMessage);
    }
}