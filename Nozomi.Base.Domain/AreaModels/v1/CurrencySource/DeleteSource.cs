using System;
using System.Linq.Expressions;
using Nozomi.Data.CurrencyModels;

namespace Nozomi.Data.AreaModels.v1.CurrencySource
{
    public class DeleteSource
    {
        public long Id { get; set; }

        public bool HardDelete { get; set; } = false;
    }
}