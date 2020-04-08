using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Nozomi.Base.BCL;

namespace Nozomi.Data.Models.Categorisation
{
    /// <summary>
    /// The best way to peg currencies to sources.
    /// </summary>
    [DataContract]
    public class ItemSource : Entity
    {
        public ItemSource() {}

        /// <summary>
        /// Constructor for Currency-based seeding
        /// </summary>
        /// <param name="sourceId"></param>
        public ItemSource(long sourceId)
        {
            SourceId = sourceId;
        }

        public ItemSource(long sourceId, long itemId)
        {
            SourceId = sourceId;
            ItemId = itemId;
        }
        
        [Key]
        public long Id { get; set; }
        
        public long ItemId { get; set; }
        
        public Item Item { get; set; }
        
        public long SourceId { get; set; }
        
        public Source Source { get; set; }
    }
}