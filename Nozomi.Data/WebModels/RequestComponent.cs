using Counter.SDK.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Data.WebModels
{
    public class RequestComponent : BaseEntityModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the query component of the JSON data retrieved from the APIUrl.
        /// </summary>
        /// <value>The query component.</value>
        /// 
        /// Example: MainNest.Nest2.property3
        /// 
        /// If the component is actually an array, just toss a number. We'll attempt
        /// to pass thing parameter to an integer first anyway, so as to be able
        /// to distinguish if we're offloading data from an array or object.
        public string QueryComponent { get; set; }

        public long RequestId { get; set; }
        public Request Request { get; set; }
        
        public ICollection<RequestComponentDatum> RequestComponentData { get; set; }
    }
}
