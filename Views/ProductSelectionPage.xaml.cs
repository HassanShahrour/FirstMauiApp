using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;
using BMB.ViewModels;

namespace BMB.Views;

public partial class ProductSelectionPage : ContentPage, IQueryAttributable
{
    private int? _orderId;
    private int _clientId;
    private DateTime _selectedDate;

    private readonly IClientRepository _clientRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly UserUtility _userUtility;

    private readonly ProductSelectionViewModel _viewModel;

    public ProductSelectionPage(
        IClientRepository clientRepository,
        IProductRepository productRepository,
        IOrderRepository orderRepository,
        UserUtility userUtility)
    {
        InitializeComponent();

        _clientRepository = clientRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _userUtility = userUtility;

        // Initialize ViewModel once
        _viewModel = new ProductSelectionViewModel(_productRepository, _orderRepository, _clientRepository, _userUtility);
        BindingContext = _viewModel;

        //_ = _viewModel.LoadProductsAsync();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            if (query.TryGetValue("OrderId", out var orderIdObj) &&
                int.TryParse(orderIdObj?.ToString(), out var orderId))
            {
                _orderId = orderId;
            }

            if (query.TryGetValue("ClientId", out var clientIdObj) &&
                int.TryParse(clientIdObj?.ToString(), out var clientId))
            {
                _clientId = clientId;
            }

            _selectedDate = query.TryGetValue("SelectedDate", out var dateObj) && dateObj is string encodedDate &&
                            DateTime.TryParse(Uri.UnescapeDataString(encodedDate), out var parsedDate)
                ? parsedDate
                : DateTime.Now;
        }
        catch (Exception ex)
        {
            _orderId = null;
            _clientId = 0;
            _selectedDate = DateTime.Now;
            Console.WriteLine($"[ApplyQueryAttributes] Error parsing query: {ex.Message}");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            if (_orderId.HasValue)
            {
                await _viewModel.LoadOrderForEditAsync(_orderId.Value);
            }
            else
            {
                var client = await _clientRepository.GetClientById(_clientId);
                if (client != null)
                {
                    await _viewModel.LoadProductsAsync();
                    _viewModel.SelectedClient = client;
                    _viewModel.SelectedDate = _selectedDate;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid client selected.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OnAppearing] Error initializing page: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Failed to load order data.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
