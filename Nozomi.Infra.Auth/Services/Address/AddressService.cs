using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Auth.Services.Address
{
    public class AddressService : BaseService<AddressService, AuthDbContext>, IAddressService
    {
        public AddressService(ILogger<AddressService> logger, AuthDbContext context) 
            : base(logger, context)
        {
        }

        public string Create(string userId, string address, AddressType type)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(address))
                return string.Empty;

            var user = _context.Users
                .AsNoTracking()
                .SingleOrDefault(u => u.Id.Equals(userId));

            if (user == null)
                return string.Empty;

            var addr = new Base.Auth.Models.Wallet.Address()
            {
                UserId = userId,
                Hash = address,
                Type = type
            };
            
            _context.Addresses.Add(addr);
            _context.SaveChanges();

            return addr.Id;
        }
    }
}