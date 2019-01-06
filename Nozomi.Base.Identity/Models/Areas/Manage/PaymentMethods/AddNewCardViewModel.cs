using System.ComponentModel.DataAnnotations;

namespace Nozomi.Base.Identity.Models.Areas.Manage.PaymentMethods
{
    public class AddNewCardViewModel
    {
        [Required]
        public string CardholderName { get; set; }
        
        [Required]
        public string CardToken { get; set; }
    }
}