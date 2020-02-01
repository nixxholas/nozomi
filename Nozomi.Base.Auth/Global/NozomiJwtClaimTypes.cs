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
        /// The Id of the user's current subscription
        /// </summary>
        public const string StripeSubscriptionId = "stripe_sub_id";
        
    }
}