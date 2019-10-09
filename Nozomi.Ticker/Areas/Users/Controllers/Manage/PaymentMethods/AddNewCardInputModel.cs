using System.ComponentModel.DataAnnotations;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage.PaymentMethods
{
    public class AddNewCardInputModel
    {
        [Required]
        public string CardholderName { get; set; }
        
        [Required]
        public string CardToken { get; set; }
    }
}