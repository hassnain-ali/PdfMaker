using Android.Widget;
using PdfMaker.Services;
using PdfSharpCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PdfMaker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            TxtDefaultFileName.Text = Settings.DefaultFileName;
            SwitchAfterComplete.IsToggled = Settings.DefaultOpenFileAfterMake;
            SelectList.ItemsSource = Enum.GetValues(typeof(PageSize)).Cast<PageSize>().ToArray();
            SelectList_ShakeSpeed.ItemsSource = Enum.GetValues(typeof(SensorSpeed)).Cast<SensorSpeed>().ToArray();
            TxtCompression.Text = Settings.ImageCompressionQuality.ToString();
            TxtRotation.Text = Settings.ImagePreRotate.ToString();
            TxtImageHeight.Text = Settings.ImageResizeHeight.ToString();
            BindingContext = new MainPageViewModel();
        }

        private async void BtnSaveChanges_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Are you sure?", "Do You want to update settings?", "Yes", "No"))
            {
                Settings.DefaultFileName = TxtDefaultFileName.Text;
                Settings.DefaultOpenFileAfterMake = SwitchAfterComplete.IsToggled;
                Settings.ImageCompressionQuality = Convert.ToInt32(TxtCompression.Text);
                Settings.ImagePreRotate = Convert.ToInt32(TxtRotation.Text);
                Settings.ImageResizeHeight = Convert.ToInt32(TxtImageHeight.Text);
                Settings.PdfPageSize = SelectList.SelectedItem?.ToString();
                Settings.CancelShakeSpeed = SelectList_ShakeSpeed.SelectedItem?.ToString();
                Toast.MakeText(Android.App.Application.Context, "New application settings loaded", ToastLength.Long).Show();
            }
        }
    }
}