using System.Collections.Generic;

namespace Nozomi.Data.ViewModels.Trello
{
    public class BoardViewModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DateLastActivity { get; set; }

        public IEnumerable<ListViewModel> Lists { get; set; }
    }
}
