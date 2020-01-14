using System.Collections.Generic;

namespace Nozomi.Base.BCL.Responses
{
    public class NozomiPaginatedResult<T> where T : class
    {
        /// <summary>
        /// Count of how many pages to go. Starting from 0 as the first.
        /// </summary>
        public long Pages { get; set; }
        
        public long ElementsPerPage { get; set; }
        
        public ICollection<T> Data { get; set; }
    }
}