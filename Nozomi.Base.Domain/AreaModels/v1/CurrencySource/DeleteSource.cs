using System;
using System.Linq.Expressions;

namespace Nozomi.Data.AreaModels.v1.CurrencySource
{
    public class DeleteSource
    {
        public long Id { get; set; }

        public bool HardDelete { get; set; } = false;
    }
}