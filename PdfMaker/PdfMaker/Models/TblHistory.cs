using SQLite;
using System;

namespace PdfMaker.Models
{
    public class TblHistory
    {
        [PrimaryKey]
        [AutoIncrement]
        [NotNull]
        [Unique]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
