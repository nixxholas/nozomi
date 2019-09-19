using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models.Wallet;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Blockchain.Auth.Events
{
    public class AddressEvent : BaseEvent<AddressEvent, NozomiDbContext>, IAddressEvent
    {
        private readonly IValidatingEvent _validatingEvent;
        
        public AddressEvent(ILogger<AddressEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork,
            IValidatingEvent validatingEvent) 
            : base(logger, unitOfWork)
        {
            _validatingEvent = validatingEvent;
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
                var addr = _unitOfWork.GetRepository<Address>()
                    .GetQueryable()
                    .AsNoTracking()
                    .SingleOrDefault(a => a.Hash.Equals(address));

                return addr;
            }

            return null;
        }
    }
}