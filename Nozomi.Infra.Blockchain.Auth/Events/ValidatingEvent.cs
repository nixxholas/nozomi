using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using Nethereum.Util;
using Nozomi.Infra.Blockchain.Auth.Events.Interfaces;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.BCL.Repository;
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
        /// <param name="signedMessage"></param>
        /// <returns></returns>
        public bool ValidateOwner(string claimerAddress, string signature, string rawMessage)
        {
            // Null Checks
            var addrValidator = new AddressUtil();
            if (string.IsNullOrEmpty(rawMessage) || addrValidator.IsAnEmptyAddress(claimerAddress)
                || !addrValidator.IsValidEthereumAddressHexFormat(claimerAddress))
                return false;
            
            // Check for validity
            var signer = new EthereumMessageSigner();
            var resultantAddr = signer.EncodeUTF8AndEcRecover(rawMessage, signature);
            return addrValidator.IsAnEmptyAddress(resultantAddr) && resultantAddr.Equals(claimerAddress);
        }
    }
}