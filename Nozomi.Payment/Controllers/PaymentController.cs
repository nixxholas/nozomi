using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nozomi.Payment.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                switch (stripeEvent.Type) {
                    case Events.ChargeSucceeded:
                        //Charges Service: Handle Charge Event
                        return Ok();

                    case Events.ChargeFailed:
                        //Charges Service: Handle Charge Event
                        return Ok();

                    case Events.ChargeRefunded:
                        //Charges Service: Handle Charge Event
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
    }
}
