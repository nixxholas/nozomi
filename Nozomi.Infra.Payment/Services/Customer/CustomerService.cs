using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Customer
{
    public class CustomerService : BaseService<CustomerService>, ICustomerService
    {
        private readonly IUserService _userService;
        private readonly IStripeEvent _stripeEvent;
        
        public CustomerService(ILogger<CustomerService> logger, IUserService userService, IStripeEvent stripeEvent) : base(logger)
        {
            _userService = userService;
            _stripeEvent = stripeEvent;
        }

        public Task RegisterCustomer(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task AddPaymentMethod(string paymentMethodId, User user)
        {
            throw new System.NotImplementedException();
        }

        public Task RemovePaymentMethod(string paymentMethodId, User user)
        {
            throw new System.NotImplementedException();
        }

        public Task Subscribe(string planId, User user)
        {
            throw new System.NotImplementedException();
        }

        public Task Unsubscribe(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task ChangeSubscription(string planId, User user)
        {
            throw new System.NotImplementedException();
        }
    }
}