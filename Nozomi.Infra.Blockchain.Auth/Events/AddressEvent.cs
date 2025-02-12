using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;

namespace Nozomi.Infra.Blockchain.Auth.Events
{
    public class AddressEvent : BaseEvent<AddressEvent, AuthDbContext>, IAddressEvent
    {
        private readonly IValidatingEvent _validatingEvent;
        
        public AddressEvent(ILogger<AddressEvent> logger, AuthDbContext context,
            IValidatingEvent validatingEvent) 
            : base(logger, context)
        {
            _validatingEvent = validatingEvent;
        }

        public bool IsBinded(string address)
        {
            return !string.IsNullOrWhiteSpace(address) &&
                   _context.Addresses.AsNoTracking().Any(addr => addr.Hash.Equals(address));
        }

        public Address Authenticate(string address, string signature, string message)
        {
            // Null check
            if (!string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(message)
                // Make sure the user is can proof that he owns the address
                && _validatingEvent.ValidateOwner(new ValidateOwnerQuery()
                {
                    ClaimerAddress = address,
                    Signature = signature,
                    RawMessage = message
                }))
            {
                // Db Layer check
                var addr = _context.Addresses.AsNoTracking()
                    .SingleOrDefault(a => a.Hash.Equals(address));

                return addr;
            }

            return null;
        }
    }
}