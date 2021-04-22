using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PdfMaker.Droid
{
    [Activity(Label = "Pdf Maker", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    [IntentFilter(
        new[] { Platform.Intent.ActionAppAction },
        Categories = new[] { Intent.CategoryDefault })]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
         
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        protected override void OnResume()
        {
            base.OnResume();

            Platform.OnResume(this);
        }

        protected async override void OnStart()
        {
            base.OnStart();
            try
            {
                await AppActions.SetAsync(
                    new AppAction("clear_cache", "Clear Cache", subtitle: "Clear Application's cache"),
                    new AppAction("pdf_history", "History", subtitle: "View your Pdf History"),
                    new AppAction("new_pdf", "New Pdf", subtitle: "Created New Pdf")
                    );
            }
            catch (FeatureNotSupportedException ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            Platform.OnNewIntent(intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }
    }
}