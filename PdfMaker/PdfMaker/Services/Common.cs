using Android.Widget;
using PdfMaker.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamFormsImageResize;

namespace PdfMaker.Services
{
    public class Common
    {
        private static Common instance;

        public static Common Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new();
                }
                return instance;
            }
            set { instance = value; }
        }

        public Common()
        {
        }
        public async Task<IEnumerable<FilesResult>> GetResizedFileNames(IEnumerable<FileResult> files)
        {
            List<FilesResult> filesResult = new();
            string TempPath = Directories.GetTempPath();
            foreach (var item in files.OrderBy(s => s.FileName))
            {
                using var openedFile = await item.OpenReadAsync();
                using MemoryStream stream = new();
                await openedFile.CopyToAsync(stream);
                var resizedImage = await ImageResizer.ResizeImage(stream.ToArray());
                string newFullPath = Path.Combine(TempPath, item.FileName);
                using (FileStream fs = new(newFullPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using MemoryStream ms = new(resizedImage);
                    await ms.CopyToAsync(fs);
                }
                filesResult.Add(new(item.FileName, newFullPath, item.ContentType));
            }
            Directories.RefreshRoots(new[] { TempPath });
            return await Task.FromResult(filesResult);
        }
        public void MakeToast(string message, ToastLength toastLength)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Android.App.Application.Context, message, toastLength).Show();
            });
        }
        public async Task DisplayAlert(string message)
        {
            await Application.Current.MainPage.DisplayAlert("Are you sure?", message, "No");
        }
        public async Task<bool> DisplayAlert(string message, bool hasYes)
        {
            return await Application.Current.MainPage.DisplayAlert("Are you sure?", message, "Yes", "No");
        }
    }
}
