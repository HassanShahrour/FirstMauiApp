using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using SQLite;
using System.Diagnostics;

namespace BMB.Data
{
    public class AppDatabase : IAppDatabase
    {
        private const string DatabaseName = "bmbDb.db3";
        private readonly SQLiteAsyncConnection _database;

        public AppDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);
            Debug.WriteLine($"SQLite DB Path: {dbPath}");

            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitAsync()
        {
            try
            {
                await _database.CreateTableAsync<User>();
                await _database.CreateTableAsync<Client>();
                await _database.CreateTableAsync<Order>();
                await _database.CreateTableAsync<OrderItem>();
                await _database.CreateTableAsync<Product>();
                await _database.CreateTableAsync<EventItem>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[InitAsync Error] {ex.Message}");
                throw;
            }
        }

        public async Task<int> AddAsync<T>(T entity) where T : class
        {
            try
            {
                return await _database.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddAsync Error] {ex.Message}");
                throw;
            }
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                return await _database.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UpdateAsync Error] {ex.Message}");
                throw;
            }
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            try
            {
                return await _database.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DeleteAsync Error] {ex.Message}");
                throw;
            }
        }

        public async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            try
            {
                return await _database.Table<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetAllAsync Error] {ex.Message}");
                return new List<T>();
            }
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class, new()
        {
            try
            {
                return await _database.FindAsync<T>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetByIdAsync Error] {ex.Message}");
                return new T();
            }
        }

        public async Task<T> GetByConditionAsync<T>(Func<T, bool> predicate) where T : class, new()
        {
            try
            {
                var all = await _database.Table<T>().ToListAsync();
                return all.FirstOrDefault(predicate) ?? new T();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetByConditionAsync Error] {ex.Message}");
                return new T();
            }
        }

        public async Task<List<T>> GetManyByConditionAsync<T>(Func<T, bool> predicate) where T : class, new()
        {
            try
            {
                var all = await _database.Table<T>().ToListAsync();
                return all.Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetManyByConditionAsync Error] {ex.Message}");
                return new List<T>();
            }
        }


    }
}
