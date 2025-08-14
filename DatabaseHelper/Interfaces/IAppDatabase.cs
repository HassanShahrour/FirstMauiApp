namespace BMB.DatabaseHelper.Interfaces
{
    public interface IAppDatabase
    {
        Task InitAsync();
        Task<int> AddAsync<T>(T entity) where T : class;
        Task<int> UpdateAsync<T>(T entity) where T : class;
        Task<int> DeleteAsync<T>(T entity) where T : class;

        Task<List<T>> GetAllAsync<T>() where T : class, new();
        Task<T> GetByIdAsync<T>(int id) where T : class, new();

        Task<T> GetByConditionAsync<T>(Func<T, bool> predicate) where T : class, new();
        Task<List<T>> GetManyByConditionAsync<T>(Func<T, bool> predicate) where T : class, new();
    }
}
