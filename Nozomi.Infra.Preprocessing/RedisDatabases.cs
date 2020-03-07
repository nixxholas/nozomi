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
        ///
        /// Keys are always earmarked for expiry in 3 months to prevent stacking.
        /// </summary>
        /// <remarks>
        /// If the value is null, the user is banned for the billing month for exceeding his quota.
        /// </remarks>
        ApiKeyUser = 2
    }
}