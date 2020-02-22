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
        
        /// <summary>
        /// Time of the delay between each compute.
        /// </summary>
        public int Delay { get; set; }
        
        /// <summary>
        /// Tells us if the compute is failing its computation or not.
        /// </summary>
        public bool IsFailing { get; set; }
        
        public ICollection<ComputeExpression> Expressions { get; set; }
        
        public ICollection<SubCompute> ChildComputes { get; set; }
        

        public ICollection<SubCompute> ParentComputes { get; set; }
        
        public ICollection<ComputeValue> Values { get; set; }
    }
}