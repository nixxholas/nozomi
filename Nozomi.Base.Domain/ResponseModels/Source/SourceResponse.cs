using System.Collections.Generic;
using Nozomi.Data.ResponseModels.Currency;

namespace Nozomi.Data.ResponseModels.Source
{
    public class SourceResponse
    {
        public string Abbreviation { get; set; }
        
        public string Name { get; set; }
        
        public string APIDocsURL { get; set; }
    }
}