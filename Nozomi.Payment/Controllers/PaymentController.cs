using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nozomi.Base.BCL.Configurations;
using Nozomi.Infra.Payment.Services.DisputesHandling;
using Nozomi.Infra.Payment.Services.InvoicesHandling;
using Nozomi.Infra.Payment.Services.SubscriptionHandling;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.ActionResults;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nozomi.Payment.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseApiController<PaymentController>
    {
        private readonly IOptions<StripeOptions> _stripeOptions;
        private readonly IInvoicesHandlingService _invoicesHandlingService;
        private readonly IDisputesHandlingService _disputesHandlingService;
        private readonly ISubscriptionsHandlingService _subscriptionsHandlingService;

        public PaymentController(ILogger<PaymentController> logger, IOptions<StripeOptions> stripeOptions, 
            IInvoicesHandlingService invoicesHandlingService, IDisputesHandlingService disputesHandlingService, 
            ISubscriptionsHandlingService subscriptionsHandlingService) : base(logger)
        {
            _stripeOptions = stripeOptions;
            _invoicesHandlingService = invoicesHandlingService;
            _disputesHandlingService = disputesHandlingService;
            _subscriptionsHandlingService = subscriptionsHandlingService;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(_stripeOptions.Value.WebhookSecret))
            {
                _logger.LogCritical($"{_controllerName} Index: Stripe Webhook Secret is invalid!");
                return new InternalServerErrorObjectResult("Please contact Nozomi's staff immediately! Something is " +
                                                           "wrong on their end..");
            }
            
            var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(payload, 
                    HttpContext.Request.Headers["Stripe-Signature"], 
                    _stripeOptions.Value.WebhookSecret);

                // Handle the event
                switch (stripeEvent.Type) {
                    case Events.InvoiceFinalized:
                        await _invoicesHandlingService.InvoiceFinalized(ParseEventToInvoice(stripeEvent));
                        return Ok();

                    case Events.CustomerSubscriptionUpdated:
                        var subscription = ParseEventToSubscription(stripeEvent);
                        if (subscription.Status.ToLower().Equals("canceled"))
                            await _subscriptionsHandlingService.SubscriptionCancelled(subscription);
                        return Ok();    
                    
                    case Events.ChargeDisputeClosed:
                        var dispute = ParseEventToDispute(stripeEvent);
                        await _disputesHandlingService.DisputeClosed(dispute);
                        return Ok();
                    default:
                        return BadRequest();
                }
            }
            catch (StripeException e)
            {
                _logger.LogWarning($"{_controllerName} Index: Stripe exception -> {e}");
                return BadRequest();
            }
        }

        private Charge ParseEventToCharge(Event stripeEvent) {
            return stripeEvent.Data.Object as Charge;
        }

        private Dispute ParseEventToDispute(Event stripeEvent) {
            return stripeEvent.Data.Object as Dispute;
        }

        private Invoice ParseEventToInvoice(Event stripeEvent) {
            return stripeEvent.Data.Object as Invoice;
        }
        
        private Subscription ParseEventToSubscription(Event stripeEvent) {
            return stripeEvent.Data.Object as Subscription;
        }
    }
}
