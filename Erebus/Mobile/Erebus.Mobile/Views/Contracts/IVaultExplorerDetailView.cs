using Erebus.Core;
using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{

    public class EntryListItem
    {
        public Group ParentGroup { get; set; }
        public Entry Entry { get; set; }

        public string EntryDisplayName
        {
            get
            {
                return $"{this.Entry.Title} - ({this.Entry.UserName})";
            }
        }

        public EntryListItem(Entry entry, Group parentGroup)
        {
            GuardClauses.ArgumentIsNotNull(nameof(entry), entry);
            GuardClauses.ArgumentIsNotNull(nameof(parentGroup), parentGroup);

            this.Entry = entry;
            this.ParentGroup = parentGroup;
        }
    }

    public interface IVaultExplorerDetailView
    {
        IEnumerable<EntryListItem> EntryListItem { set; }
    }
}
