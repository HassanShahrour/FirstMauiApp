using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;

namespace BMB.DatabaseHelper.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IAppDatabase _db;

        public UserRepository(IAppDatabase db)
        {
            _db = db;
        }

        public async Task<int> AddUser(User user)
        {
            try
            {
                return await _db.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding user", ex);
            }
        }

        public async Task<int> UpdateUser(User user)
        {
            try
            {
                return await _db.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating user", ex);
            }
        }

        public async Task<int> DeleteUser(User user)
        {
            try
            {
                return await _db.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting user", ex);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _db.GetAllAsync<User>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving users", ex);
            }
        }

        public async Task<User> GetUserById(int id)
        {
            try
            {
                return await _db.GetByIdAsync<User>(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving user with ID {id}", ex);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return await _db.GetByConditionAsync<User>(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving user with email {email}", ex);
            }
        }
    }
}
