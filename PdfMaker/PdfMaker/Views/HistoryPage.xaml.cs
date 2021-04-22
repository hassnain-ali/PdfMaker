using Android.Widget;
using PdfMaker.Data;
using PdfMaker.Models;
using PdfMaker.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PdfMaker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        HistoryDatabaseContext context;
        ObservableCollection<TblHistory> items;
        public HistoryPage()
        {
            InitializeComponent();
            ListView_history.IsRefreshing = true;

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Run(async () =>
            {
                context = await HistoryDatabaseContext.Instance;
                await Fetch();
            });
        }

        private async void MenuItem_Clicked(object sender, System.EventArgs e)
        {
            var menuItem = sender as MenuItem;
            TblHistory history = menuItem.CommandParameter as TblHistory;
            await Share.RequestAsync(new ShareFileRequest()
            {
                Title = "Share " + history.FileName,
                File = new ShareFile(history.FilePath)
            });
        }

        private async void MenuItem_Clicked_1(object sender, System.EventArgs e)
        {
            var menuItem = sender as MenuItem;
            TblHistory history = menuItem.CommandParameter as TblHistory;
            try
            {
                if (await DisplayAlert("Are you sure?", "You want to delete " + history.FileName + "?", "Yes", "No"))
                {
                    await Task.Run(() =>
                    {
                        Java.IO.File file = new(history.FilePath);
                        if (file.Exists())
                        {
                            file.Delete();
                            context.DeleteItemAsync(history);
                        }
                        else
                        {
                            context.DeleteItemAsync(history);
                        }
                        ListView_history_Refreshing(default, default);
                        Common.Instance.MakeToast("Removed successfully.", ToastLength.Long);
                    });
                }
            }
            catch (System.Exception ex)
            {
                await Common.Instance.DisplayAlert(ex.Message);
            }

        }

        private async void ListView_history_Refreshing(object sender, System.EventArgs e)
        {
            await Fetch();
            ListView_history.IsRefreshing = false;
        }
        private async Task Fetch()
        {
            await Task.Run(async () =>
        {
            items = new ObservableCollection<TblHistory>(await context.GetItemsAsync());
            MainThread.BeginInvokeOnMainThread(async () =>
                   {
                       ListView_history.ItemsSource = items;
                       await Task.Delay(1000);
                       ListView_history.IsRefreshing = false;
                   });
        });
        }

        private async void ListView_history_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            TblHistory history = ListView_history.SelectedItem as TblHistory;
            await Launcher.OpenAsync(new OpenFileRequest() { File = new ReadOnlyFile(history.FilePath) });
        }
    }
}