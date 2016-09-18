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
    public class VaultExplorerDetailView : ContentPage, IVaultExplorerDetailView
    {
        private ListView ListView;

        public VaultExplorerDetailView()
        {
            ListView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    var textCell = new TextCell();
                    textCell.SetBinding(TextCell.TextProperty, "EntryDisplayName");
                    return textCell;
                }),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None
            };

            Padding = new Thickness(0, 40, 0, 0);
            //Icon = "hamburger.png";
            //Title = StringResources.Entries;
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    ListView
                }
            };
        }

        public IEnumerable<EntryListItem> EntryListItem
        {
            set
            {
                this.ListView.ItemsSource = value;
            }
        }
    }
}
