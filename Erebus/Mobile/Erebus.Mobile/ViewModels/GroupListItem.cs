using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.ViewModels
{
    public class GroupListItem
    {
        public Group Group { get; set; }
        public List<GroupListItem> ParentItems { get; set; }
        public List<GroupListItem> ChildItems { get; set; }
        public bool IsNavigateBackItem { get; set; }

        public string GroupDisplayName
        {
            get
            {
                if (IsNavigateBackItem)
                {
                    return $" < {this.Group.Name} ({this.Group.Entries.Count})";
                }
                else
                {
                    return $"   {this.Group.Name} ({this.Group.Entries.Count})";
                }

            }
        }

        public GroupListItem()
        {
            ChildItems = new List<GroupListItem>();
        }
    }
}
