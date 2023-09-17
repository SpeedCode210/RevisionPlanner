using RevisionPlanner.Models;
using SQLite;

namespace RevisionPlanner
{
    public class DatabaseConnector
    {
        private bool _lock;
        public const string DatabaseFilename = "revisionData.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        SQLiteAsyncConnection Database;

        public DatabaseConnector()
        {
            _ = Init();
        }

        async Task Init()
        {
            while (_lock)
                await Task.Delay(100);
            if (Database is not null)
                return;
            _lock = true;
            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
            await Database.CreateTableAsync<Lesson>();
            await Database.CreateTableAsync<Category>();
            _lock = false;
        }


        public async Task<List<Lesson>> GetLessonsAsync()
        {
            await Init();
            return await Database.Table<Lesson>().ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            await Init();
            return await Database.Table<Category>().ToListAsync();
        }

        public async Task<int> AddLessonAsync(Lesson item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }

        public async Task<int> UpdateLessonAsync(Lesson item)
        {
            await Init();
            return await Database.UpdateAsync(item);
        }

        public async Task<int> DeleteLessonAsync(Lesson item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> AddCategoryAsync(Category item)
        {
            await Init();
            return await Database.InsertAsync(item);
        }

        public async Task<int> UpdateCategoryAsync(Category item)
        {
            await Init();
            return await Database.UpdateAsync(item);
        }

        public async Task<int> DeleteCategoryAsync(Category item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

    }
}
