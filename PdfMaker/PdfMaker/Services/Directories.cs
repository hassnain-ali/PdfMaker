using Android.Media;
using System;
using System.IO;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace PdfMaker
{
    public class Directories
    {
        /// <summary>
        /// creates directory with folder name
        /// </summary>
        /// <param name="directoryName">folder name to create</param>
        /// <param name="root">application root directory</param>
        /// <returns>combined path to created folder</returns>
        public static string GetOrCreateRoot()
        {
            string root;
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {

                var dirs = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                if (!dirs.Exists())
                {
                    dirs.Mkdirs();
                }
                root = dirs.AbsolutePath;
            }
            else
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            RefreshRoots(new[] { root });
            return root;
        }
        public static void RefreshRoots(string[] root)
        {
            MediaScannerConnection.ScanFile(Android.App.Application.Context, root, default, default);
        }
        public static async void AskPermissions<T>() where T : BasePermission, new()
        {
            var status = await CheckStatusAsync<T>();
            if (status != PermissionStatus.Granted)
            {
                status = await RequestAsync<T>();
                if (status != PermissionStatus.Granted)
                {
                    throw new UnauthorizedAccessException("You need to grant write permissions");
                }
            }
        }
        public static string PdfFileName(string name)
        {
            if (Path.GetExtension(name).EndsWith(".pdf"))
            {
                return name;
            }
            else
            {
                return string.Concat(name.Trim(), ".pdf");
            }
        }
        public static string SafeFileName(string path)
        {
            if (!File.Exists(path))
            {
                return path;
            }
            string fPath = Directory.GetParent(path).FullName;
            string fName = ValidFilename(Path.GetFileNameWithoutExtension(path));
            string ext = Path.GetExtension(path);
            return Path.Combine(fPath, fName + DateTime.Now.Ticks + ext);
        }
        private static string ValidFilename(string testName)
        {
            return string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
        }
        public static string GetTempPath()
        {
            var dirs = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);
            if (!dirs.Exists())
            {
                dirs.Mkdirs();
            }

            RefreshRoots(new[] { dirs.AbsolutePath });
            return dirs.AbsolutePath;
        }
    }
}
