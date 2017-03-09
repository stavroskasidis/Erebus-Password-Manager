using Erebus.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Mobile.Views.Contracts
{
    public interface IVaultExplorerView
    {
        List<EntryListItem> EntryListItems { set; }
        event Action<string> Search;
        event Action<EntryListItem> EntrySelected;
    }
}
