using System;
using System.ComponentModel.DataAnnotations;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Data.Models.Category
{
    public class ItemSource : Entity
    {
        public ItemSource() {}

        /// <summary>
        /// Constructor for Currency-based seeding
        /// </summary>
        /// <param name="sourceId"></param>
        public ItemSource(Guid sourceGuid)
        {
            SourceGuid = sourceGuid;
        }

        public ItemSource(Guid sourceGuid, Guid itemGuid)
        {
            SourceGuid = sourceGuid;
            ItemGuid = itemGuid;
        }
        
        [Key]
        public long Id { get; set; }
        
        public Guid ItemGuid { get; set; }
        
        public Item Item { get; set; }
        
        public Guid SourceGuid { get; set; }
        
        public Source Source { get; set; }
    }
}