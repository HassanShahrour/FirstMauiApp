using BMB.Data.Models;

namespace BMB.DatabaseHelper.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<int> UpdateUser(User user);
        Task<int> DeleteUser(User user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
    }
}
