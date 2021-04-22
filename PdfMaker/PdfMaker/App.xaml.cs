using Android.Widget;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PdfMaker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.UserAppTheme = RequestedTheme;
            MainPage = new NavigationPage(new TabbedMainPage());
            AppActions.OnAppAction += AppActions_OnAppAction;
            
            RequestedThemeChanged += (s, e) =>
            {
                Current.UserAppTheme = e.RequestedTheme;
            };
        }

        private async void AppActions_OnAppAction(object sender, AppActionEventArgs e)
        {
            switch (e.AppAction.Id)
            {
                case "pdf_history":
                    await MainPage.Navigation.PushAsync(new HistoryPage());
                    break;
                case "clear_cache":
                    await AppServices.ClearApplicationCache();
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Toast.MakeText(Android.App.Application.Context, "Application cache cleared.", ToastLength.Short).Show();
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
