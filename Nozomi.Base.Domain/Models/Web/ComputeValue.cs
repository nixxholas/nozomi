using System;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    public class ComputeValue : Entity
    {
        public Guid Guid { get; set; }

        public string Value { get; set; }
        
        public Guid ComputeGuid { get; set; }
        
        public Compute Compute { get; set; }
    }
}