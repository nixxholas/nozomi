using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Base.Identity.Models.Areas
{
    public class BaseViewModel
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}