namespace Nozomi.Base.Auth.Global
{
    public static class NozomiJwtClaimTypes
    {
        public const string DefaultWallet = "default_wallet_hash";
        /// <summary>
        /// The unique identifier of the user on stripe.
        /// </summary>
        public const string StripeCustomerId = "stripe_cust_id";
        /// <summary>
        /// A set of array for the payment methods the user has.
        /// </summary>
        public const string StripeCustomerPaymentMethodId = "stripe_cust_payment_method_id";
        /// <summary>
        /// The Id of the user's default payment method
        /// </summary>
        public const string StripeCustomerDefaultPaymentId = "stripe_cust_default_payment_method_id";
        /// <summary>
        /// The Id of the user's current subscription
        /// </summary>
        public const string StripeSubscriptionId = "stripe_sub_id";
        /// <summary>
        /// The Ids of the user's previous subscriptions
        /// </summary>
        public const string PreviousStripeSubscriptionId = "ex_stripe_sud_id";
        /// <summary>
        /// The user's quota
        /// </summary>
        public const string UserQuota = "user_quota";
        /// <summary>
        /// The user's current usage
        /// </summary>
        public const string UserUsage = "user_usage";

    }
}