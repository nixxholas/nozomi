using Nozomi.Base.Auth.Models.Wallet;

namespace Nozomi.Infra.Blockchain.Auth.Events.Interfaces
{
    public interface IAddressEvent
    {
        Address Authenticate(string address, string signature, string message);
    }
}