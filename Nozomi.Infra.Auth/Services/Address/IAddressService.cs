using Nozomi.Base.Auth.Models.Wallet;

namespace Nozomi.Infra.Auth.Services.Address
{
    public interface IAddressService
    {
        string Create(string userId, string address, AddressType type);
    }
}