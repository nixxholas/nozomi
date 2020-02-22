using System;

namespace Nozomi.Data.Models.Web
{
    public class ComputeExpression
    {
        public Guid Guid { get; set; }
        
        /// <summary>
        /// The type of this expression.
        ///
        /// Allows us to distinguish what type of entity its value comes from.
        /// </summary>
        public ComputeExpressionType Type { get; set; }
        
        /// <summary>
        /// The expression of this entity.
        ///
        /// Could be [123129389] or [asdjkfn-dsafja]
        /// Basically the key of the value.
        /// </summary>
        public string Expression { get; set; }
        
        /// <summary>
        /// The hardcoded value of the expression.
        /// Only works when Type == Generic.
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Foreign key binding.
        ///
        /// Always unique along with Expression.
        /// </summary>
        public Guid ComputeGuid { get; set; }
        
        public Compute Compute { get; set; }
    }
}