using Erebus.Core;
using Erebus.Model;
using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.ViewModels
{
    public class EntryListItem
    {
        public string GroupPath { get; set; }
        public Entry Entry { get; set; }

        public string ItemText
        {
            get
            {
                return $"{this.Entry.Title}";
            }
        }

        public string ItemDetail
        {
            get
            {
                return $"{this.GroupPath}";
            }
        }

        public EntryListItem(Entry entry, string groupPath)
        {
            GuardClauses.ArgumentIsNotNull(nameof(entry), entry);
            GuardClauses.ArgumentIsNotNull(nameof(groupPath), groupPath);

            this.Entry = entry;
            this.GroupPath = groupPath;
        }
    }
}
