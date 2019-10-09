using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nozomi.Ticker.Areas.Users.Controllers.Account
{
    public class SendCodeInputModel
    {
        public string SelectedProvider { get; set; }
        
        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}