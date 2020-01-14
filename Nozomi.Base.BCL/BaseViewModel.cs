using Microsoft.AspNetCore.Mvc;

namespace Nozomi.Base.BCL
{
    public class BaseViewModel
    {
        [TempData] public string StatusMessage { get; set; } = null;
    }
}