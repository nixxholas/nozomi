using IdentityServer4.Events;
using Nozomi.Base.Auth.Global;

namespace Nozomi.Base.Auth.Events
{
    public class UserCreateClaimFailureEvent : Event
    {
        public UserCreateClaimFailureEvent(string username, string error, string endpoint, string clientId = null) 
            : base(NozomiAuthEventCategories.AccountCreation,
                "User Login Failure",
                EventTypes.Failure, 
                EventIds.UserLoginFailure,
                error)
        {
            Username = username;
            ClientId = clientId;
            Endpoint = endpoint;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; set; }
    }
}