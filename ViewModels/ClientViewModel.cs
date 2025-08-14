using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BMB.ViewModels
{
    public partial class ClientViewModel : BaseViewModel
    {
        private readonly IClientRepository _clientRepo;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddClientCommand))]
        private string? name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddClientCommand))]
        private string? phone;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddClientCommand))]
        private string? address;

        [ObservableProperty]
        private string? errorMessage;


        public ObservableCollection<Client> Clients { get; set;  } = new();

        public ClientViewModel(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        public async Task LoadClientsAsync()
        {
            try
            {
                ErrorMessage = null;

                var clients = await _clientRepo.GetAllClients();

                Clients = new ObservableCollection<Client>(clients);

                OnPropertyChanged(nameof(Clients));
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load clients. Please try again.";
                Debug.WriteLine($"[LoadClientsAsync] {ex}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanAddClient))]
        public async Task AddClientAsync()
        {
            try
            {
                ErrorMessage = null;

                var newClient = new Client
                {
                    FullName = Name!.Trim(),
                    PhoneNumber = Phone?.Trim(),
                    Address = Address?.Trim()
                };

                await _clientRepo.AddClient(newClient);

                Clients.Add(newClient);

                Name = string.Empty;
                Phone = string.Empty;
                Address = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to add client. Please try again.";
                Debug.WriteLine($"[AddClientAsync] {ex}");
            }
        }

        private bool CanAddClient()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
