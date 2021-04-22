using Android.App;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PdfMaker
{

    public static class AppServices
    {
        /// <summary>
        /// same as python 'join'
        /// </summary>
        /// <typeparam name="T">list type</typeparam>
        /// <param name="separator">string separator </param>
        /// <param name="list">list of objects to be ToString'd</param>
        /// <returns>a concatenated list interleaved with separators</returns>
        static public string Join<T>(this IEnumerable<T> list, string separator)
        {
            var sb = new StringBuilder();
            bool first = true;

            foreach (T v in list)
            {
                if (!first)
                    sb.Append(separator);
                first = false;

                if (v != null)
                    sb.Append(v.ToString());
            }

            return sb.ToString();
        }
        public async static Task ClearApplicationCache()
        {
            Java.IO.File dir = Application.Context.CacheDir;
            await DeleteDir(dir);
            if (System.IO.Directory.Exists(Directories.GetTempPath()))
            {
                System.IO.Directory.Delete(Directories.GetTempPath(), true);
            }
            Directories.RefreshRoots(new[] { dir.AbsolutePath });
        }

        public async static Task<bool> DeleteDir(Java.IO.File dir)
        {
            if (dir != null && dir.IsDirectory)
            {
                string[] children = await dir.ListAsync();
                for (int i = 0; i < children.Length; i++)
                {
                    bool success = await DeleteDir(new Java.IO.File(dir, children[i]));
                    if (!success)
                    {
                        return false;
                    }
                }
                return dir.Delete();
            }
            else if (dir != null && dir.IsFile)
            {
                return dir.Delete();
            }
            else
            {
                return false;
            }
        }
    }
}
