using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using System.Threading.Tasks;

namespace PdfMaker.Droid
{
    [Activity(Label = "Pdf Maker", NoHistory = true, Icon = "@mipmap/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.SetTheme(Resource.Style.Theme_AppCompat_DayNight_NoActionBar);
            base.OnCreate(savedInstanceState, persistentState);
            //SetContentView(Resource.Animation.fragment_open_enter);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { StartActivity(new Intent(Application.Context, typeof(MainActivity))); });
            startupWork.Start();
        }
    }
}