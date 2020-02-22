using System;
using System.Collections.Generic;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Web
{
    public class Compute : Entity
    {
        public Guid Guid { get; set; }
        
        /// <summary>
        /// Optional
        ///
        /// If this compute is a child compute, this will be used to identify its
        /// presence in the parent compute 
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// The complete formula of this compute.
        ///
        /// Could be [x] * [s] % [t]
        /// </summary>
        public string Formula { get; set; }
        
        public ICollection<ComputeExpression> Expressions { get; set; }
        
        public ICollection<SubCompute> ChildComputes { get; set; }
        
        public SubCompute ParentCompute { get; set; }
    }
}