using PdfSharpCore;
using Xamarin.Essentials;

namespace PdfMaker.Services
{
    public static class Settings
    {
        public static string DefaultFileName
        {
            get { return Preferences.Get(Keys.DefaultFileName, "FileName"); }
            set { Preferences.Set(Keys.DefaultFileName, value); }
        }
        public static bool DefaultOpenFileAfterMake
        {
            get { return Preferences.Get(Keys.DefaultFileOpenAfterComplete, true); }
            set { Preferences.Set(Keys.DefaultFileOpenAfterComplete, value); }
        }

        public static int ImageResizeHeight
        {
            get { return Preferences.Get(Keys.DefaultImageResizeHeight, 1051); }
            set { Preferences.Set(Keys.DefaultImageResizeHeight, value); }
        }

        /// <summary>
        /// page size in <seealso cref="PageSize"/>
        /// </summary>
        public static string PdfPageSize
        {
            get { return Preferences.Get(Keys.DefaultPdfPageSize, PageSize.A4.ToString()); }
            set { Preferences.Set(Keys.DefaultPdfPageSize, value); }
        }

        public static int ImagePreRotate
        {
            get { return Preferences.Get(Keys.DefaultImagePreRotate, 90); }
            set { Preferences.Set(Keys.DefaultImagePreRotate, value); }
        }

        public static int ImageCompressionQuality
        {
            get { return Preferences.Get(Keys.DefaultCompressionQuality, 100); }
            set { Preferences.Set(Keys.DefaultCompressionQuality, value); }
        }


        public static string CancelShakeSpeed
        {
            get { return Preferences.Get(Keys.DefaultShakeSpeed, SensorSpeed.Default.ToString()); }
            set { Preferences.Set(Keys.DefaultShakeSpeed, value); }
        }
    }
}
