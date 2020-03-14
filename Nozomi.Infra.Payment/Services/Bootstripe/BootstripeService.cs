﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.CompilerServices;
using Nozomi.Base.Auth.Models;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.UserEvent;
using Nozomi.Infra.Auth.Services.QuotaClaims;
using Nozomi.Infra.Auth.Services.User;
using Nozomi.Preprocessing.Abstracts;
using Stripe;

namespace Nozomi.Infra.Payment.Services.Bootstripe
{
    public class BootstripeService : BaseService<BootstripeService>, IBootstripeService
    {
        private readonly IUserService _userService;
        // private readonly IQuotaClaimsService _quotaClaimsService;
        private readonly IUserEvent _userEvent;

        public BootstripeService(ILogger<BootstripeService> logger, IUserService userService, IUserEvent userEvent,
            IOptions<StripeOptions> stripeOptions// , IQuotaClaimsService quotaClaimsService
            ) : base(logger)
        {
            StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;

            _userService = userService;
            // _quotaClaimsService = quotaClaimsService;
            _userEvent = userEvent;
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

            var customerService = new CustomerService();
            Customer stripeCustomer = await customerService.CreateAsync(createOptions);

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

            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);

            var options = new PaymentMethodAttachOptions
            {
                Customer = stripeCustomerId,
            };

            var paymentMethodService = new PaymentMethodService();
            var result = paymentMethodService.Attach(paymentMethodId, options);

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
            
            var stripeCustomerId = _userEvent.GetStripeCustomerId(user.Id);
            
            CheckPaymentMethodOwnership(stripeCustomerId, paymentMethodId, methodName);

            var paymentMethodOptions = new PaymentMethodListOptions
            {
                Customer = stripeCustomerId,
                Type = "card"
            };

            var paymentMethodService = new PaymentMethodService();
            var paymentMethods = paymentMethodService.List(paymentMethodOptions);

            if (paymentMethods.Count() <= 1)
            {
                _logger.LogInformation($"{_serviceName} {methodName}: {user.Id} only has one payment method, detachment prevented.");
                throw new StripeException($"{_serviceName} {methodName}: {user.Id} only has one payment method, detachment prevented.");
            }
            
            var paymentMethod = paymentMethodService.Detach(paymentMethodId);

            if (paymentMethod == null || paymentMethod.Customer != null)
            {
                _logger.LogInformation($"{_serviceName} {methodName}: {user.Id} detachment of payment method {paymentMethodId} failed.");
                throw new StripeException($"{_serviceName} {methodName}: {user.Id} detachment of payment method {paymentMethodId} failed.");
            }
                
            _userService.RemovePaymentMethod(user.Id, paymentMethodId);
            _logger.LogInformation($"{_serviceName} RemovePaymentMethod: {user.Id} successfully " +
                               $"removed a payment method that was tokenized as {paymentMethodId}");

            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(stripeCustomerId);

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

            var customerService = new CustomerService();
            var updatedCustomer = await customerService.UpdateAsync(stripeCustomerId, customerUpdateOptions);
            if (updatedCustomer.InvoiceSettings.DefaultPaymentMethodId != paymentMethodId)
                throw new StripeException($"{_serviceName} {methodName}: An error occured while trying to update user's default payment method.");

            _userService.SetDefaultPaymentMethod(user.Id, paymentMethodId);
        }

        private void PerformUserPrecheck(User user, string methodName)
        {
            if(user == null)
                throw new NullReferenceException($"{_serviceName} {methodName}: User is null.");
        }

        private async void CheckPaymentMethodOwnership(string userId, string paymentMethodId, string methodName) {
            var paymentMethodService = new PaymentMethodService();
            var paymentMethod = await paymentMethodService.GetAsync(paymentMethodId);

            var stripeCustomerId = _userEvent.GetStripeCustomerId(userId);

            if (string.IsNullOrEmpty(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: An error occured while trying to retrieve stripe customer id.");

            if (!paymentMethod.CustomerId.Equals(stripeCustomerId))
                throw new InvalidOperationException($"{_serviceName} {methodName}: Payment method does not belong to customer.");
        }

        private async Task<int> GetPlanQuota(string planid, string methodName)
        {
            var planService = new PlanService();
            var stripePlan = await planService.GetAsync(planid);

            if (stripePlan == null)
                throw new StripeException($"{_serviceName} {methodName}: Unable to find plan based on ID");

            var quotaString = stripePlan.Metadata["Quota"];

            var quotaValue = 0;
            
            if (!int.TryParse(quotaString, out quotaValue))
                throw new FormatException($"{_serviceName} {methodName}: Failed to parse plan quota to int");
            
            return quotaValue;
        }
    }
}