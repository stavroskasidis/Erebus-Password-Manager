using Erebus.Mobile.Views.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Erebus.Mobile.ViewModels;
using System.Threading;

namespace Erebus.Mobile.Views.Implementations
{
    public class VaultExplorerView : ContentPage, IVaultExplorerView
    {
        private ListView ListView = new ListView();
        private SearchBar SearchBar = new SearchBar();

        public event Action<string> Search;
        public event Action<EntryListItem> EntrySelected;
        private CancellationTokenSource SearchCancellationTokenSource;

        public VaultExplorerView()
        {
            this.SearchBar.SearchButtonPressed += (object sender, EventArgs e) =>
            {
                this.Search?.Invoke(this.SearchBar.Text);
            };

            this.SearchBar.TextChanged += async (object sender, TextChangedEventArgs e) =>
            {
                if (this.SearchCancellationTokenSource != null)
                {
                    this.SearchCancellationTokenSource.Cancel();
                }
                this.SearchCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = this.SearchCancellationTokenSource.Token;
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return;

                    await Task.Run(() =>
                    {
                        this.Search?.Invoke(this.SearchBar.Text);
                    }, cancellationToken);

                }
                catch (OperationCanceledException)
                {
                    //Expected
                }
                finally
                {

                }
            };

            this.ListView.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                if (e.Item != null)
                {
                    this.EntrySelected?.Invoke(e.Item as EntryListItem);
                }
            };

            this.ListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                this.ListView.SelectedItem = null;
            };

            this.ListView.ItemTemplate = new DataTemplate(() =>
            {
                var textCell = new TextCell();
                textCell.SetBinding(TextCell.TextProperty, "ItemText");
                textCell.SetBinding(TextCell.DetailProperty, "ItemDetail");
                return textCell;
            });
            this.ListView.VerticalOptions = LayoutOptions.FillAndExpand;

            Content = new StackLayout
            {
                Children =
                {
                    SearchBar,
                    new ScrollView
                    {
                        Content = new StackLayout
                        {
                            Padding = 10,
                            Children =
                            {
                                ListView
                            }
                        }
                    }
                }
            };
        }

        public List<EntryListItem> EntryListItems
        {
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.ListView.BeginRefresh();
                    this.ListView.ItemsSource = value;
                    this.ListView.EndRefresh();
                });
            }
        }
    }
}
