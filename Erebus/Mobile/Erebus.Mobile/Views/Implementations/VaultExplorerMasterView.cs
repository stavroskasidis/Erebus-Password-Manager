using Erebus.Mobile.ViewModels;
using Erebus.Mobile.Views.Contracts;
using Erebus.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Erebus.Mobile.Views.Implementations
{
    public class VaultExplorerMasterView : ContentPage, IVaultExplorerMasterView
    {
        private ListView ListView;
        public event Action<GroupListItem> GroupListItemSelected;

        public VaultExplorerMasterView()
        {
            ListView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    var textCell = new TextCell();
                    textCell.SetBinding(TextCell.TextProperty, "GroupDisplayName");
                    return textCell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None
            };

            Padding = new Thickness(0, 40, 0, 0);
            Icon = "menu.png";
            Title = StringResources.Groups;
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    ListView
                }
            };

            ListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => GroupListItemSelected.Invoke(e.SelectedItem as GroupListItem);
        }

        public IEnumerable<GroupListItem> GroupList
        {
            set
            {
                this.ListView.ItemsSource = value;
            }
        }

    }
}
