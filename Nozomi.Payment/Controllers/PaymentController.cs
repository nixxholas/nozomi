using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nozomi.Infra.Payment.Services.DisputesHandling;
using Nozomi.Infra.Payment.Services.InvoicesHandling;
using Nozomi.Infra.Payment.Services.SubscriptionHandling;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nozomi.Payment.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IInvoicesHandlingService _invoicesHandlingService;
        private readonly IDisputesHandlingService _disputesHandlingService;
        private readonly ISubscriptionsHandlingService _subscriptionsHandlingService;

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

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
