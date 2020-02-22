﻿using System.Threading.Tasks;
using Nozomi.Base.Auth.Models;

namespace Nozomi.Infra.Payment.Services.Customer
{
    public interface ICustomerService
    {
        Task RegisterCustomer(User user);
        
        Task AddPaymentMethod(string paymentMethodId, User user);
        
        Task RemovePaymentMethod(string paymentMethodId, User user);
        
        Task Subscribe(string planId, User user);
        
        Task Unsubscribe(User user);
        
        Task ChangeSubscription(string planId, Base.Auth.Models.User user);
    }
}