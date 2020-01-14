using System.Collections.Generic;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Ticker.Areas.Users.Controllers.Manage
{
    public class IndexViewModel : BaseViewModel
    {
        public ICollection<Source> Sources { get; set; } = new List<Source>();
    }
}