using System;
using System.Collections.Generic;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    public class SubCompute : Entity
    {
        public Guid ParentComputeGuid { get; set; }
        
        public Compute ParentCompute { get; set; }
        
        public Guid ChildComputeGuid { get; set; }
        
        public Compute ChildCompute { get; set; }
    }
}