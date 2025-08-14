using System.Diagnostics;

namespace BMB.Utilities
{
    public class UserUtility
    {
        private const string UserIdKey = "LoggedInUserId";


        //public Task SetLoggedInUserIdAsync(int userId)
        //{
        //    Preferences.Set(UserIdKey, userId);
        //    return Task.CompletedTask;
        //}

        //public Task<int> GetLoggedInUserIdAsync()
        //{
        //    int userId = Preferences.Get(UserIdKey, -1);
        //    return Task.FromResult(userId);
        //}

        //public Task ClearLoggedInUserAsync()
        //{
        //    Preferences.Remove(UserIdKey);
        //    return Task.CompletedTask;
        //}

        public async Task SetLoggedInUserIdAsync(int userId)
        {
            try 
            { 
                await SecureStorage.SetAsync(UserIdKey, userId.ToString());
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"User Utility error: {ex.Message}");
            }
        }

        public async Task<int> GetLoggedInUserIdAsync()
        {
            try
            {
                var userIdString = await SecureStorage.GetAsync(UserIdKey);
                if (int.TryParse(userIdString, out int userId))
                {
                    return userId;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"User Utility error: {ex.Message}");
                return -1;
            }
        }

        public void ClearLoggedInUser()
        {
            SecureStorage.Remove(UserIdKey);
        }

    }
}
