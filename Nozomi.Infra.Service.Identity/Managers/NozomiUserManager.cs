using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Core.Helpers.Enumerator;
using Nozomi.Base.Identity.Models.Identity;
using Nozomi.Service.Identity.Managers.Interfaces;
using Nozomi.Service.Identity.Stores;
using Nozomi.Service.Identity.Stores.Interfaces;
using Stripe;

namespace Nozomi.Service.Identity.Managers
{
    public class NozomiUserManager : UserManager<User>, INozomiUserManager
    {
        private new readonly INozomiUserStore Store;
        
        public NozomiUserManager(INozomiUserStore store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, 
                errors, services, logger)
        {
            Store = store;
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var options = new CustomerCreateOptions {
                Description = "Customer for jenny.rosen@example.com",
                SourceToken = "tok_mastercard"
            };

            var service = new CustomerService();
            var customer = service.Create(options);
            
            if (customer != null)
            {
                user.StripeCustomerId = customer.Id;
                return await base.CreateAsync(user);
            }
            
            return IdentityResult.Failed(new IdentityError()
            {
                Code = IdentityErrorType.CreateAccountStripeIssue.ToString(),
                Description = IdentityErrorType.CreateAccountStripeIssue.GetDescription()
            });
        }

        public async Task<User> FindAsync(string id, string password)
        {
            var user = await FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return await CheckPasswordAsync(user, password) ? user : null;
        }

        public Task ForceConfirmEmail(long userId)
        {
            return Store.ForceConfirmEmailAsync(userId);
        }

        public Task ForceConfirmEmail(User user)
        {
            return Store.ForceConfirmEmailAsync(user);
        }
        
        /// <summary>
        /// Gets the user identifier for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose identifier should be retrieved.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user" />.</returns>
        public override async Task<string> GetUserIdAsync(User user)
        {
            ThrowIfDisposed();
            return await Store.GetUserIdAsync(user, CancellationToken);
        }
    }
}