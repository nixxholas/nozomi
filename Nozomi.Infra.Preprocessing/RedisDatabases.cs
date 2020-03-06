namespace Nozomi.Preprocessing
{
    public enum RedisDatabases
    {
        Default = -1,
        /// <summary>
        /// Unrecorded API Key Events Database
        ///
        /// Keys are the Event's critical information
        /// Values is the weight of the event.
        /// </summary>
        UnrecordedApiKeyEvents = 0,
        /// <summary>
        /// ApiKeyUser Database
        ///
        /// Keys are APIs that map to its respective user.
        /// Values are the user's ID.
        /// </summary>
        ApiKeyUser = 2,
        /// <summary>
        /// BlockedUserApiKeys Database
        /// 
        /// Keys are UserIds
        /// Values are API Keys
        /// </summary>
        BlockedUserApiKeys = 3,
    }
}