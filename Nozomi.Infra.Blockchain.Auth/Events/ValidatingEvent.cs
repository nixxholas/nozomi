using System.Text;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using Nethereum.Util;
using Nozomi.Base.Blockchain.Auth.Query.Validating;
using Nozomi.Data.Interfaces;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Blockchain.Auth.Events
{
    public class ValidatingEvent : BaseEvent<ValidatingEvent, NozomiDbContext>, IValidatingEvent
    {
        public ValidatingEvent(ILogger<ValidatingEvent> logger, IUnitOfWork<NozomiDbContext> unitOfWork) 
            : base(logger, unitOfWork)
        {
        }

        /// <summary>
        /// ValidateOwner method
        ///
        /// Enables the caller to validate the signature in accordance with the claimer's address and the
        /// signed message.
        ///
        /// Note that the signedMessage must match expectedMessage
        /// </summary>
        /// <param name="claimerAddress"></param>
        /// <param name="signature"></param>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public bool ValidateOwner(ValidateOwnerQuery request)
        {
            // Null Checks
            var addrValidator = new AddressUtil();
            if (request == null || string.IsNullOrEmpty(request.RawMessage) || string.IsNullOrEmpty(request.Signature) 
                || addrValidator.IsAnEmptyAddress(request.ClaimerAddress)
                || !addrValidator.IsValidEthereumAddressHexFormat(request.ClaimerAddress))
                return false;
            
            // Check for validity
            var signer = new EthereumMessageSigner();
            
            var resultantAddr = signer.EncodeUTF8AndEcRecover(request.RawMessage, request.Signature);
            return !addrValidator.IsAnEmptyAddress(resultantAddr) && resultantAddr.Equals(request.ClaimerAddress);
        }
    }
}