using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BMB.ViewModels
{
    public partial class UsersViewModel : ObservableObject
    {
        private readonly IUserRepository _repository;

        public ObservableCollection<User> Users { get; } = new();

        public UsersViewModel(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task LoadUsersAsync()
        {
            var users = await _repository.GetAllUsers();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
    }
}
