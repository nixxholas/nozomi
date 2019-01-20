using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nozomi.Base.Core;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Base.Identity.ViewModels.Manage
{
    public class IndexViewModel : BaseViewModel
    {
        public ICollection<Source> Sources { get; set; } = new List<Source>();
    }
}