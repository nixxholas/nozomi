using System.Collections.Generic;
using Stripe;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage.PaymentMethods
{
    public class PaymentMethodsViewModel : AddNewCardInputModel
    {   
        public ICollection<Card> Cards { get; set; }
        
        public Customer Customer { get; set; }
    }
}