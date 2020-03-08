using System.Collections.Generic;

namespace Nozomi.Data.ViewModels.Trello
{
    public class CardViewModel
    {
        public BadgeViewModel Badges { get; set; }

        public List<CheckListViewModel> CheckLists { get; set; }

        public string DateLastActivity { get; set; }

        public string Desc { get; set; }

        public string ID { get; set; }

        public List<LabelViewModel> Labels { get; set; }

        public string Name { get; set; }
    }
}
