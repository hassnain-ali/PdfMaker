using Xamarin.Essentials;

namespace PdfMaker.Models
{
    public class FilesResult : FileBase
    {
        public FilesResult(string fileName, string path, string contantType) : base(new FileResult(path, contantType))
        {
            FileName = fileName;
        }
    }
}
