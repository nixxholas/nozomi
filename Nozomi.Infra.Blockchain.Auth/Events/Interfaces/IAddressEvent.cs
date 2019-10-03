using Nozomi.Base.Auth.Models.Wallet;

namespace Nozomi.Infra.Blockchain.Auth.Events.Interfaces
{
    public interface IAddressEvent
    {
        bool IsBinded(string address);
        
        Address Authenticate(string address, string signature, string message);
    }
}