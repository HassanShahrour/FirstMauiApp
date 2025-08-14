using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BMB.ViewModels
{
    public partial class OrderListViewModel : BaseViewModel
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IClientRepository _clientRepo;
        private readonly UserUtility _userUtility;

        public ObservableCollection<OrderDisplayModel> Orders { get; } = new();
        public ObservableCollection<Client> Clients { get; } = new();

        public Client SelectedClient { get; set; } = new();
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        [ObservableProperty]
        private string? errorMessage;

        public OrderListViewModel(IOrderRepository orderRepo, IClientRepository clientRepo, UserUtility userUtility)
        {
            _orderRepo = orderRepo;
            _clientRepo = clientRepo;
            _userUtility = userUtility;
        }

        public async Task LoadOrdersAsync()
        {
            try
            {
                ErrorMessage = null;

                var userId = await _userUtility.GetLoggedInUserIdAsync();
                var orders = await _orderRepo.GetOrdersByUserId(userId);
                var clients = await _clientRepo.GetAllClients();

                Orders.Clear();
                Clients.Clear();

                var clientDict = clients.ToDictionary(c => c.Id);

                foreach (var client in clients)
                    Clients.Add(client);

                var sortedOrders = orders.OrderByDescending(o => o.Date);

                foreach (var order in sortedOrders)
                {
                    if (!clientDict.TryGetValue(order.ClientId, out var client))
                    {
                        try
                        {
                            client = await _clientRepo.GetClientById(order.ClientId);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Failed to load client {order.ClientId}: {ex}");
                            client = null;
                        }
                    }

                    Orders.Add(new OrderDisplayModel
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Amount = order.TotalPrice,
                        ClientId = order.ClientId,
                        ClientName = client?.FullName ?? "N/A",
                        ClientPhone = client?.PhoneNumber ?? "N/A",
                        ClientAddress = client?.Address ?? "N/A"
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load orders. Please try again.";
                Debug.WriteLine($"[LoadOrdersAsync] {ex}");
            }
        }

        [RelayCommand]
        private async Task DeleteOrderAsync(int orderId)
        {
            try
            {
                bool confirm = await Shell.Current.DisplayAlert("Confirm Delete", "Are you sure you want to delete this order?", "Yes", "No");
                if (!confirm) return;

                int result = await _orderRepo.DeleteOrder(orderId);
                if (result > 0)
                {
                    var orderToRemove = Orders.FirstOrDefault(o => o.Id == orderId);
                    if (orderToRemove != null)
                        Orders.Remove(orderToRemove);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to delete the order.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Exception", ex.Message, "OK");
                Debug.WriteLine($"[DeleteOrderAsync] {ex}");
            }
        }

        [RelayCommand]
        private async Task EditOrderAsync(int orderId)
        {
            try
            {
                await Shell.Current.GoToAsync($"ProductSelectionPage?OrderId={orderId}");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to navigate to order editing page.";
                Debug.WriteLine($"[EditOrderAsync] {ex}");
            }
        }
    }

    public class OrderDisplayModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? ClientPhone { get; set; }
        public string? ClientAddress { get; set; }
    }
}
