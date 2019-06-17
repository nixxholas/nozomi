using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Nozomi.Data.AreaModels.v1.Source
{
    public class CreateSource
    {
        [Required]
        [DisplayName("The name of the source. (i.e. Bitfinex)")]
        public string Name { get; set; }
        
        [Required]
        [DisplayName("The short form of the source. (i.e. BFX)")]
        public string Abbreviation { get; set; }
        
        [Required]
        [DisplayName("The URL to the documentation of the source's API.")]
        public string ApiDocsUrl { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Abbreviation) && !string.IsNullOrEmpty(ApiDocsUrl);
        }
    }
}