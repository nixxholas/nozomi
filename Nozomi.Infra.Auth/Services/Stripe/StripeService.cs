using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.Auth.Global;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Auth.Events.Stripe;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Repo.Auth.Data;
using Nozomi.Repo.BCL.Repository;
using Stripe;

namespace Nozomi.Infra.Auth.Services.Stripe
{
    public class StripeService : BaseService<StripeService, AuthDbContext>, IStripeService
    {
        private readonly IOptions<StripeOptions> _stripeConfiguration;
        private readonly UserManager<Base.Auth.Models.User> _userManager;
        private readonly IStripeEvent _stripeEvent;
        private readonly SubscriptionService _subscriptionService;
        private readonly PlanService _planService;
        private readonly CustomerService _customerService;
        private readonly PaymentMethodService _paymentMethodService;

        public StripeService(ILogger<StripeService> logger, IUnitOfWork<AuthDbContext> unitOfWork, 
            IStripeEvent stripeEvent, UserManager<Base.Auth.Models.User> userManager, 
            IOptions<StripeOptions> stripeConfiguration)
            : base(logger, unitOfWork)
        {
            _stripeConfiguration = stripeConfiguration;
            _stripeEvent = stripeEvent;
            _userManager = userManager;
            _subscriptionService = new SubscriptionService();
            _planService = new PlanService();
            _customerService = new CustomerService();
            _paymentMethodService = new PaymentMethodService();
        }

        public StripeService(IHttpContextAccessor contextAccessor, ILogger<StripeService> logger,
            UserManager<Base.Auth.Models.User> userManager, IUnitOfWork<AuthDbContext> unitOfWork, 
            IStripeEvent stripeEvent, IOptions<StripeOptions> stripeConfiguration)
            : base(contextAccessor, logger, unitOfWork)
        {
            _stripeConfiguration = stripeConfiguration;
            _stripeEvent = stripeEvent;
            _userManager = userManager;
        }
    }
}