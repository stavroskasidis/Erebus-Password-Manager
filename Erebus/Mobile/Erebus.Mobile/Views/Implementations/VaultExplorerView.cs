using Erebus.Mobile.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Erebus.Mobile.ViewModels;

namespace Erebus.Mobile.Views.Implementations
{
    public class VaultExplorerView : ContentPage, IVaultExplorerView
    {
        private ListView ListView = new ListView();
        private SearchBar SearchBar = new SearchBar();

        public event Action<string> Search;
        public event Action<EntryListItem> EntrySelected;

        public VaultExplorerView()
        {
            this.SearchBar.SearchButtonPressed += (object sender, EventArgs e) =>
            {
                this.ListView.BeginRefresh();
                this.Search?.Invoke(this.SearchBar.Text);
                this.ListView.EndRefresh();
            };
            this.SearchBar.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                this.ListView.BeginRefresh();
                this.Search?.Invoke(this.SearchBar.Text);
                this.ListView.EndRefresh();
            };

            this.ListView.ItemTapped += (object sender, ItemTappedEventArgs e) => this.EntrySelected?.Invoke(e.Item as EntryListItem);

            this.ListView.ItemTemplate = new DataTemplate(() =>
            {
                var textCell = new TextCell();
                textCell.SetBinding(TextCell.TextProperty, "ItemText");
                textCell.SetBinding(TextCell.DetailProperty, "ItemDetail");
                return textCell;
            });
            this.ListView.VerticalOptions = LayoutOptions.FillAndExpand;

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 10,
                    Children =
                    {
                       SearchBar,
                       ListView
                    }
                }
            };
        }

        public List<EntryListItem> EntryListItems
        {
            set
            {
                this.ListView.ItemsSource = value;
            }
        }
    }
}
