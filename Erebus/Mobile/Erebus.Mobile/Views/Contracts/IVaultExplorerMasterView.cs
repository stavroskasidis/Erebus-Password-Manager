using Erebus.Core;
using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{
    public class GroupListItem
    {
        public Group Group { get; set; }
        public GroupListItem ParentItem { get; set; }
        public bool IsNavigateBackItem { get; set; }

        public string GroupDisplayName
        {
            get
            {
                if (IsNavigateBackItem)
                {
                    return $"< {this.Group.Name} ({this.Group.Entries.Count})";
                }
                else
                {
                    return $"  {this.Group.Name} ({this.Group.Entries.Count})";
                }
                
            }
        }

        public GroupListItem(Group group, GroupListItem parentItem, bool isParentItem)
        {
            GuardClauses.ArgumentIsNotNull(nameof(group), group);

            this.Group = group;
            this.ParentItem = parentItem;
            this.IsNavigateBackItem = isParentItem;
        }
    }

    public interface IVaultExplorerMasterView
    {
        IEnumerable<GroupListItem> GroupList { set; }
        event Action<GroupListItem> GroupListItemSelected;
    }
}
