using System.Threading.Tasks;
using Nozomi.Base.Auth.ViewModels.Account;

namespace Nozomi.Infra.Auth.Services.User
{
    public interface IUserService
    {
        bool HasStripe(string userId);
        
        Task LinkStripe(string stripeCustId, string userId);
        
        Task Update(UpdateUserInputModel vm, string userId);
        
        void AddPaymentMethod(string userId, string paymentMethodId);

        void RemovePaymentMethod(string userId, string paymentMethodId);

        void SetDefaultPaymentMethod(string userId, string paymentMethodId);

        void AddSubscription(string userId, string subscriptionId);

        void RemoveSubscription(string userId, string subscriptionId);
    }
}