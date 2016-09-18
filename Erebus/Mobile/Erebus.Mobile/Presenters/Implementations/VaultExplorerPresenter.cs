using Autofac;
using Erebus.Core.Contracts;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Mobile.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.Mobile.Presenters.Implementations
{
    public class VaultExplorerPresenter : IVaultExplorerPresenter
    {
        private IVaultExplorerView View;
        private IVaultRepositoryFactory VaultRepositoryFactory;
        //private IVaultManipulatorFactory VaultManipulatorFactory;
        private IContainer Container;
        private IApplicationContext ApplicationContext;

        public VaultExplorerPresenter(IVaultExplorerView view, IVaultRepositoryFactory vaultRepositoryFactory, IContainer container, IApplicationContext applicationContext)
        //IVaultManipulatorFactory vaultManipulatorFactory)
        {
            this.View = view;
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.Container = container;
            this.ApplicationContext = applicationContext;
            //this.VaultManipulatorFactory = vaultManipulatorFactory;

            var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
            using (var password = ApplicationContext.GetMasterPassword())
            {
                var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
                //var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);

                this.View.MasterView = this.Container.Resolve<IVaultExplorerMasterView>();
                this.View.MasterView.GroupListItemSelected += OnGroupSelected;
                this.View.MasterView.GroupList = vault.Groups.Select(group => new GroupListItem(group, null, false));

                this.View.DetailView = this.Container.Resolve<IVaultExplorerDetailView>();
            }
        }

        public object GetView()
        {
            return this.View;
        }

        public void OnGroupSelected(GroupListItem selected)
        {
            if (selected.IsNavigateBackItem)
            {
                var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
                using (var password = ApplicationContext.GetMasterPassword())
                {
                    var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
                    //var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);

                    if (selected.ParentItem == null)
                    {
                        var groupList = vault.Groups.Select(group => new GroupListItem(group, null, false)).ToList();
                        this.View.MasterView.GroupList = groupList;
                    }
                    else
                    {
                        var groupList = selected.ParentItem.Group.Groups.Select(group => new GroupListItem(selected.ParentItem.Group, selected.ParentItem.ParentItem, false)).ToList();
                        groupList.Insert(0, new GroupListItem(selected.Group, selected.ParentItem, true));
                        this.View.MasterView.GroupList = groupList;
                    }
                }
            }
            else
            {
                if (selected.Group.Groups.Count > 0)
                {
                    var groupList = selected.Group.Groups.Select(group => new GroupListItem(group, selected, false)).ToList();
                    groupList.Insert(0, new GroupListItem(selected.Group, selected.ParentItem, true));
                    this.View.MasterView.GroupList = groupList;
                }
            }

            if (selected.Group.Entries.Count > 0)
            {
                this.View.DetailView.EntryListItem = selected.Group.Entries.Select(entry => new EntryListItem(entry, selected.Group));
            }

            //var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
            //using (var password = ApplicationContext.GetMasterPassword())
            //{
            //    var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
            //    //var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);
            //    //vaultManipulator.GetGroupById(selected.Group.)

            //}
        }
    }
}
