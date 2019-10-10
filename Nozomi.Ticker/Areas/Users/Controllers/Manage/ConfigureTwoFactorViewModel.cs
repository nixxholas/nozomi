using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nozomi.Base.Core;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class ConfigureTwoFactorViewModel : BaseViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }
    }
}