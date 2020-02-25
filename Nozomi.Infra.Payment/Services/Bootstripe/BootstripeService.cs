using System;
using System.Linq;
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

            CheckPaymentMethodOwnership(user.Id, paymentMethodId, methodName);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);

            var options = new PaymentMethodAttachOptions
            {
                Customer = stripeCustomerId,
            };

            var result = _paymentMethodService.Attach(paymentMethodId, options);

            if (result != null && result.StripeResponse != null && result.StripeResponse.StatusCode.Equals(HttpStatusCode.OK))
            {
                _logger.LogInformation($"{_serviceName} AddPaymentMethod: {user.Id} successfully added " +
                                           $"a new payment method tokenized as {paymentMethodId}");
                _userService.AddPaymentMethod(user.Id, paymentMethodId);
            }
            else
            {
                _logger.LogInformation($"{_serviceName} AddPaymentMethod: There was an issue related to binding " +
                                       $"paymentMethodId {paymentMethodId} to user {user.Id}.");
                throw new StripeException($"{_serviceName} AddPaymentMethod: There was a problem binding " +
                                          $"the newly created payment method of {paymentMethodId} to {user.Id}");
            }

            if (_userEvent.HasDefaultPaymentMethod(user.Id))
                return;

            await SetDefaultPaymentMethod(paymentMethodId, user);
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

            CheckPaymentMethodOwnership(user.Id, paymentMethodId, methodName);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);

            var paymentMethodOptions = new PaymentMethodListOptions
            {
                Customer = stripeCustomerId
            };

            var paymentMethods = _paymentMethodService.List(paymentMethodOptions);

            if (paymentMethods.Count() <= 1)
            {
                _logger.LogInformation($"{_serviceName} {methodName}: {user.Id} only has one payment method, detachment prevented.");
                throw new StripeException($"{_serviceName} {methodName}: {user.Id} only has one payment method, detachment prevented.");
            }

            var paymentMethod = _paymentMethodService.Detach(paymentMethodId);

            if (paymentMethod == null || paymentMethod.Customer != null)
            {
                _logger.LogInformation($"{_serviceName} {methodName}: {user.Id} detachment of payment method {paymentMethodId} failed.");
                throw new StripeException($"{_serviceName} {methodName}: {user.Id} detachment of payment method {paymentMethodId} failed.");
            }
                
            _userService.RemovePaymentMethod(user.Id, paymentMethodId);
            _logger.LogInformation($"{_serviceName} RemovePaymentMethod: {user.Id} successfully " +
                               $"removed a payment method that was tokenized as {paymentMethodId}");

            var customer = await _customerService.GetAsync(stripeCustomerId);

            if(customer == null)
                throw new StripeException($"{_serviceName} {methodName}: An error occured while trying to retrieve stripe customer {stripeCustomerId}");

            _userService.SetDefaultPaymentMethod(user.Id, customer.InvoiceSettings.DefaultPaymentMethodId);

            return;
        }

        public async Task SetDefaultPaymentMethod(string paymentMethodId, User user)
        {
            const string methodName = "SetDefaultPaymentMethod";
            PerformUserPrecheck(user, methodName);

            if (string.IsNullOrEmpty(paymentMethodId))
                throw new ArgumentNullException($"{_serviceName} {methodName}: Payment method id is null");

            CheckPaymentMethodOwnership(user.Id, paymentMethodId, methodName);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);

            var customerUpdateOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethodId
                }
            };

            var updatedCustomer = await _customerService.UpdateAsync(stripeCustomerId, customerUpdateOptions);
            if (updatedCustomer.InvoiceSettings.DefaultPaymentMethodId != paymentMethodId)
                throw new StripeException($"{_serviceName} {methodName}: An error occured while trying to update user's default payment method.");

            _userService.SetDefaultPaymentMethod(user.Id, paymentMethodId);
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

        private async void CheckPaymentMethodOwnership(string userId, string paymentMethodId, string methodName) {
            var paymentMethod = await _paymentMethodService.GetAsync(paymentMethodId);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(userId);

            if (string.IsNullOrEmpty(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: An error occured while trying to retrieve stripe customer id.");

            if (!paymentMethod.CustomerId.Equals(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Payment method does not belong to customer.");
        }
    }
}