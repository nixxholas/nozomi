namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        void addCard(string stripeCardTokenId, string userId);
    }
}