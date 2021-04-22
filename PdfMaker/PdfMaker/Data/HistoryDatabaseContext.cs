using PdfMaker.Models;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PdfMaker.Data
{
    public class HistoryDatabaseContext
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<HistoryDatabaseContext> Instance = new(async () =>
        {
            var instance = new HistoryDatabaseContext();
            CreateTableResult result = await Database.CreateTableAsync<TblHistory>();
            return instance;
        });

        public HistoryDatabaseContext()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }
        public Task<List<TblHistory>> GetItemsAsync()
        {
            return Database.Table<TblHistory>().ToListAsync();
        }

        public Task<List<TblHistory>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<TblHistory>("SELECT * FROM [TblHistory]");
        }

        public Task<TblHistory> GetItemAsync(int id)
        {
            return Database.Table<TblHistory>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(TblHistory item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TblHistory item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
