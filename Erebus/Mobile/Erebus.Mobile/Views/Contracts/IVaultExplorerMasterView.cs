using Erebus.Core;
using Erebus.Mobile.ViewModels;
using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Views.Contracts
{
    public interface IVaultExplorerMasterView
    {
        IEnumerable<GroupListItem> GroupList { set; }
        event Action<GroupListItem> GroupListItemSelected;
    }
}
