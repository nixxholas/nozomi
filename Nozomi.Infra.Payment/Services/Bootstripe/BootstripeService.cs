using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Auth.Models;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Bootstripe
{
    public class BootstripeService : BaseService<BootstripeService>, IBootstripeService
    {
        private readonly IUserService _userService;
        private readonly IStripeEvent _stripeEvent;
        private readonly IUserEvent _userEvent;
        
        private readonly CustomerService _customerService;
        private readonly PaymentMethodService _paymentMethodService;
        
        public BootstripeService(ILogger<BootstripeService> logger, IUserService userService, IUserEvent userEvent, IStripeEvent stripeEvent) : base(logger)
        {
            _userService = userService;
            _userEvent = userEvent;
            _stripeEvent = stripeEvent;
            _customerService = new CustomerService();
            _paymentMethodService = new PaymentMethodService();
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

            var createOptions = new CustomerCreateOptions
            {
                Email = user.Email
            };

            Customer stripeCustomer = await _customerService.CreateAsync(createOptions);

            if (stripeCustomer.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogInformation($"{_serviceName} {methodName}: There was an issue " +
                                       $"propagating the stripe id for user {user.Id}.");
                throw new StripeException($"{_serviceName} {methodName}: There was an issue " +
                                          $"propagating the stripe id for user {user.Id}.");
            }

            await _userService.LinkStripe(stripeCustomer.Id, user.Id);
            return;
        }

        public async Task AddPaymentMethod(string paymentMethodId, User user)
        {
            const string methodName = "AddPaymentMethod";
            PerformUserPrecheck(user, methodName);
            
            if(string.IsNullOrEmpty(paymentMethodId))
                throw new NullReferenceException($"{_serviceName} {methodName}: Payment method id is null");
            
            if(!_userService.HasStripe(user.Id))
                throw new InvalidOperationException($"{_serviceName} {methodName}: User is not registered for stripe.");

            var paymentMethod = await _paymentMethodService.GetAsync(paymentMethodId);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);
            
            if(string.IsNullOrEmpty(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: An error occured while trying to retrieve stripe customer id.");
            
            if(!paymentMethod.CustomerId.Equals(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Payment method does not belong to customer.");
            
            _userService.AddPaymentMethod(user.Id, paymentMethodId);

            if (_userEvent.HasDefaultPaymentMethod(user.Id))
                return;
            //TODO: SET DEFAULT PAYMENT
            return;
        }

        public async Task RemovePaymentMethod(string paymentMethodId, User user)
        {
            const string methodName = "RemovePaymentMethod";
            PerformUserPrecheck(user, methodName);

            if (string.IsNullOrEmpty(paymentMethodId))
                throw new NullReferenceException($"{_serviceName} {methodName}: Payment method id is null");

            if (!_userService.HasStripe(user.Id))
                throw new InvalidOperationException($"{_serviceName} {methodName}: User is not registered for stripe.");

            var paymentMethod = await _paymentMethodService.GetAsync(paymentMethodId);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);

            if (string.IsNullOrEmpty(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: An error occured while trying to retrieve stripe customer id.");

            if (!paymentMethod.CustomerId.Equals(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Payment method does not belong to customer.");

            _userService.RemovePaymentMethod(user.Id, paymentMethodId);

            throw new System.NotImplementedException();
        }

        public Task SetDefaultPaymentMethod(string paymentMethodId, User user)
        {
            //TODO: IMPLEMENTATION
            throw new NotImplementedException();
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