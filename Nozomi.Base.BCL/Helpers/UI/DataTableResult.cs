using System.Collections.Generic;

namespace Nozomi.Base.BCL.Helpers.UI
{
    public class DataTableResult<T> where T : class
    {
        // Page number
        public long Draw { get; set; }
        
        public long RecordsTotal { get; set; }
        
        public long RecordFiltered { get; set; }

        public ICollection<T> Data { get; set; }
    }
}