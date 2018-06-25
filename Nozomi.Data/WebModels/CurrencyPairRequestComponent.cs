using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class CurrencyPairRequestComponent : RequestComponent
    {
        public new CurrencyPairRequest Request { get; set; }

        public ComponentType ComponentType { get; set; } = ComponentType.Unknown;
    }
}
