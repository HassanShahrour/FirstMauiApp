using BMB.DatabaseHelper.Interfaces;
using BMB.ViewModels;

namespace BMB.Views
{
    public partial class ClientsPage : ContentPage
    {
        private readonly ClientViewModel _viewModel;

        public ClientsPage(IClientRepository clientRepo)
        {
            InitializeComponent();
            _viewModel = new ClientViewModel(clientRepo);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await _viewModel.LoadClientsAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OnAppearing] Failed to load clients: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Could not load clients.", "OK");
            }
        }

        private void OnFabTapped(object sender, EventArgs e)
        {
            ResetForm();
            PopupOverlay.IsVisible = true;
            NameEntry.Focus();
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            ResetForm();
            PopupOverlay.IsVisible = false;
        }

        private void OnAddClientClicked(object sender, EventArgs e)
        {
            PopupOverlay.IsVisible = false;
            ResetForm();
        }

        private void ResetForm()
        {
            NameEntry.Text = string.Empty;
            PhoneEntry.Text = string.Empty;
            AddressEntry.Text = string.Empty;
        }
    }
}