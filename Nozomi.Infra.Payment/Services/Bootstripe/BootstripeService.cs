using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Bootstripe
{
    public class BootstripeService : BaseService<BootstripeService>, IBootstripeService
    {
        private readonly IUserService _userService;
        private readonly IStripeEvent _stripeEvent;
        private readonly CustomerService _customerService;
        
        public BootstripeService(ILogger<BootstripeService> logger, IUserService userService, IStripeEvent stripeEvent) : base(logger)
        {
            _userService = userService;
            _stripeEvent = stripeEvent;
            _customerService = new CustomerService();
        }

        public async Task RegisterCustomer(User user)
        {
            const string methodName = "RegisterCustomer";
            PerformUserPrecheck(user, methodName);

            if (_userService.HasStripe(user.Id))
            {
                _logger.LogInformation($"{_serviceName} PropagateCustomer: User {user.Id} already has " +
                                       $"stripe binded.");
                return;
            }

            var customer = new CustomerCreateOptions
            {
                Email = user.Email
            };
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

        private void PerformUserPrecheck(User user, string methodName)
        {
            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: User is null.");
        }
    }
}