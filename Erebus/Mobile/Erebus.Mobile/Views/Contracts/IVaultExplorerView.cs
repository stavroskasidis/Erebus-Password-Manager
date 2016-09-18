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
        IVaultExplorerMasterView MasterView { get; set; }
        IVaultExplorerDetailView DetailView { get; set; }
    }
}
