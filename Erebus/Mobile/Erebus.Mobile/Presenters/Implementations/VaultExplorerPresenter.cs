using Autofac;
using Erebus.Core.Contracts;
using Erebus.Core.Mobile.Contracts;
using Erebus.Mobile.Presenters.Contracts;
using Erebus.Mobile.ViewModels;
using Erebus.Mobile.Views.Contracts;
using Erebus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Mobile.Presenters.Implementations
{
    public class VaultExplorerPresenter : IVaultExplorerPresenter
    {

        private IVaultExplorerView View;
        private IVaultRepositoryFactory VaultRepositoryFactory;
        private IApplicationContext ApplicationContext;
        private INavigationManager NavigationManager;


        public VaultExplorerPresenter(IVaultExplorerView view, IVaultRepositoryFactory vaultRepositoryFactory, IApplicationContext applicationContext,
                                      INavigationManager navigationManager)
        {
            this.View = view;
            this.VaultRepositoryFactory = vaultRepositoryFactory;
            this.ApplicationContext = applicationContext;
            this.NavigationManager = navigationManager;

            var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
            using (var password = ApplicationContext.GetMasterPassword())
            {
                var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
                this.View.EntryListItems = CreateListItems(vault.Groups, null);
            }

            this.View.EntrySelected += OnEntryItemSelected;
            this.View.Search += OnSearch;
        }

        private List<EntryListItem> CreateListItems(List<Group> groups, string groupPath)
        {
            List<EntryListItem> result = new List<EntryListItem>();
            foreach (var group in groups)
            {
                var currentGroupPath = groupPath == null ? group.Name : groupPath + " > " + group.Name;
                result.AddRange(group.Entries.Select(entry => new EntryListItem(entry, currentGroupPath)));
                result.AddRange(CreateListItems(group.Groups, currentGroupPath));
            }

            return result;
        }

        public object GetView()
        {
            return this.View;
        }

        public void OnEntryItemSelected(EntryListItem entryListItem)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.NavigationManager.NavigateAsync<IEntryDetailsPresenter>(new PresenterParameter[]
                {
                    new PresenterParameter("entry", entryListItem.Entry)
                });
            });
        }

        public void OnSearch(string searchText)
        {
            var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
            using (var password = ApplicationContext.GetMasterPassword())
            {
                var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
                var listItems = CreateListItems(vault.Groups, null);

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    this.View.EntryListItems = listItems;
                }
                else
                {
                    var searchTextUpper = searchText.ToUpper();
                    this.View.EntryListItems = listItems.Where(item => (item.Entry.Title != null ? item.Entry.Title.ToUpper().Contains(searchTextUpper) : false) ||
                                                                      (item.Entry.UserName != null ? item.Entry.UserName.ToUpper().Contains(searchTextUpper) : false) ||
                                                                      (item.Entry.Description != null ? item.Entry.Description.ToUpper().Contains(searchTextUpper) : false) ||
                                                                      (item.GroupPath != null ? item.GroupPath.ToUpper().Contains(searchTextUpper) : false)).ToList();
                }
            }
        }
    }
}



//private IVaultExplorerView View;
//private IVaultRepositoryFactory VaultRepositoryFactory;
////private IVaultManipulatorFactory VaultManipulatorFactory;
//private IContainer Container;
//private IApplicationContext ApplicationContext;

//public VaultExplorerPresenter(IVaultExplorerView view, IVaultRepositoryFactory vaultRepositoryFactory, IContainer container, IApplicationContext applicationContext)
////IVaultManipulatorFactory vaultManipulatorFactory)
//{
//    this.View = view;
//    this.VaultRepositoryFactory = vaultRepositoryFactory;
//    this.Container = container;
//    this.ApplicationContext = applicationContext;
//    //this.VaultManipulatorFactory = vaultManipulatorFactory;

//    var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
//    using (var password = ApplicationContext.GetMasterPassword())
//    {
//        var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
//        //var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);

//        this.View.MasterView = this.Container.Resolve<IVaultExplorerMasterView>();
//        this.View.MasterView.GroupListItemSelected += OnGroupSelected;
//        this.View.MasterView.GroupList = GetGroupsModel(vault.Groups,null , null); //vault.Groups.Select(group => new GroupListItem(group, null, false));

//        this.View.DetailView = this.Container.Resolve<IVaultExplorerDetailView>();
//    }
//}

//public object GetView()
//{
//    return this.View;
//}

//private List<GroupListItem> GetGroupsModel(List<Group> groups, Group parentGroup, List<GroupListItem> parentList)
//{
//    List<GroupListItem> result = new List<GroupListItem>();
//    foreach (var group in groups)
//    {
//        var item = new GroupListItem();
//        item.Group = group;
//        item.IsNavigateBackItem = false;
//        item.ChildItems = GetGroupsModel(group.Groups, group, result);
//        result.Add(item);
//    }

//    if (groups.Count > 0 && parentList != null)
//    {
//        result.Insert(0, new GroupListItem()
//        {
//            IsNavigateBackItem = true,
//            Group = parentGroup,
//            //ChildItems = parent.ChildItems,
//            ParentItems = parentList
//        });
//        //selected.Group, selected.ParentItem, true));
//    }

//    return result;
//}

//public void OnGroupSelected(GroupListItem selected)
//{
//    //if (selected.IsNavigateBackItem)
//    //{
//    if (selected.IsNavigateBackItem)
//    {
//        this.View.MasterView.GroupList = selected.ParentItems;
//    }
//    else if(selected.ChildItems.Count > 0)
//    {
//        this.View.MasterView.GroupList = selected.ChildItems;
//    }

//    //}

//    //if (selected.IsNavigateBackItem)
//    //{
//    //    var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
//    //    using (var password = ApplicationContext.GetMasterPassword())
//    //    {
//    //        var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
//    //        //var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);

//    //        if (selected.ParentItem == null)
//    //        {
//    //            var groupList = vault.Groups.Select(group => new GroupListItem(group, null, false)).ToList();
//    //            this.View.MasterView.GroupList = groupList;
//    //        }
//    //        else
//    //        {
//    //            var groupList = selected.ParentItem.Group.Groups.Select(group => new GroupListItem(selected.ParentItem.Group, selected.ParentItem.ParentItem, false)).ToList();
//    //            groupList.Insert(0, new GroupListItem(selected.Group, selected.ParentItem, true));
//    //            this.View.MasterView.GroupList = groupList;
//    //        }
//    //    }
//    //}
//    //else
//    //{
//    //    if (selected.Group.Groups.Count > 0)
//    //    {
//    //        var groupList = selected.Group.Groups.Select(group => new GroupListItem(group, selected, false)).ToList();
//    //        groupList.Insert(0, new GroupListItem(selected.Group, selected.ParentItem, true));
//    //        this.View.MasterView.GroupList = groupList;
//    //    }
//    //}

//    if (selected.Group.Entries.Count > 0)
//    {
//        this.View.DetailView.EntryListItem = selected.Group.Entries.Select(entry => new EntryListItem(entry, selected.Group));
//    }

//    //var vaultRepository = this.VaultRepositoryFactory.CreateInstance();
//    //using (var password = ApplicationContext.GetMasterPassword())
//    //{
//    //    var vault = vaultRepository.GetVault(this.ApplicationContext.GetCurrentVaultName(), password);
//    //    //var vaultManipulator = VaultManipulatorFactory.CreateInstance(vault);
//    //    //vaultManipulator.GetGroupById(selected.Group.)

//    //}
//}