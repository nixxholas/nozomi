namespace Nozomi.Infra.Auth.Services.Stripe
{
    public interface IStripeService
    {
        void addCard(string stripeCardId, Base.Auth.Models.User user);

        void removeCard(string stripeCardId, Base.Auth.Models.User user);
    }
}