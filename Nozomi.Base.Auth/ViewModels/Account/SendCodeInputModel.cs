using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nozomi.Base.Auth.ViewModels.Account
{
    public class SendCodeInputModel
    {
        public string SelectedProvider { get; set; }
        
        public ICollection<SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}