namespace Nozomi.Base.BCL.Configurations
{
    public class StripeOptions
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
        /// <summary>
        /// The product id that is stashed in Stripe, an entity that stores all pricing plans
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// The id of the default plan 
        /// </summary>
        public string DefaultPlanId { get; set; }
    }
}