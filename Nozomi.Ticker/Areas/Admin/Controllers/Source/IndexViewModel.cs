using System.Collections.Generic;

namespace Nozomi.Ticker.Areas.Admin.Controllers.Source
{
    public class IndexViewModel
    {
        public ICollection<Data.Models.Currency.Source> Sources { get; set; }
    }
}