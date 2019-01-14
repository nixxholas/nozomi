using System.Collections.Generic;
using Stripe;

namespace Nozomi.Base.Identity.ViewModels.Manage.PaymentMethods
{
    public class PaymentMethodsViewModel : AddNewCardInputModel
    {   
        public ICollection<Card> Cards { get; set; }
        
        public Customer Customer { get; set; }
    }
}