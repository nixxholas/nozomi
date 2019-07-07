using System.Collections.Generic;

namespace Nozomi.Base.Core.Responses
{
    public class NozomiPaginatedResult<T> where T : class
    {
        /// <summary>
        /// Count of how many pages/100 to go. Starting from 0 as the first.
        /// </summary>
        public long Pages { get; set; }
        
        public ICollection<T> Data { get; set; }
    }
}