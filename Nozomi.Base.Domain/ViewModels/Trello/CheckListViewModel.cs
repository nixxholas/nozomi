using System.Collections.Generic;

namespace Nozomi.Data.ViewModels.Trello
{
    public class CheckListViewModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public List<CheckItemViewModel> CheckItems { get; set; }
    }
}
