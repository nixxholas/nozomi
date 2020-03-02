using System;
using System.Collections.Generic;

namespace Nozomi.Data.ViewModels.Trello
{
    public class ListViewModel
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public IEnumerable<CardViewModel> Cards { get;set; }
    }
}
