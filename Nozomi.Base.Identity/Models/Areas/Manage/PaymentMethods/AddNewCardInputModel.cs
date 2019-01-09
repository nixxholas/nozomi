using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Manage.PaymentMethods
{
    public class AddNewCardInputModel
    {
        [Required]
        public string CardholderName { get; set; }
        
        [Required]
        public string CardToken { get; set; }
    }
}