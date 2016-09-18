using Erebus.Mobile.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Mobile.Views.Implementations
{
    public class VaultExplorerView : MasterDetailPage, IVaultExplorerView
    {
        private IVaultExplorerMasterView VaultExplorerMasterView;
        private IVaultExplorerDetailView VaultExplorerDetailView;
        

        public VaultExplorerView()
        {
        }


        public IVaultExplorerMasterView MasterView
        {
            get
            {
                return this.VaultExplorerMasterView;
            }
            set
            {
                VaultExplorerMasterView = value;
                Master = value as Page;
            }
        }

        public IVaultExplorerDetailView DetailView
        {
            get
            {
                return this.VaultExplorerDetailView;
            }
            set
            {
                this.VaultExplorerDetailView = value;
                Detail = new NavigationPage(value as Page);
            }
        }
    }
}
