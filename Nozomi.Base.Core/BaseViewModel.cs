using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Base.Core
{
    public class BaseViewModel
    {
        [TempData]
        public string StatusMessage { get; set; }
    }
}