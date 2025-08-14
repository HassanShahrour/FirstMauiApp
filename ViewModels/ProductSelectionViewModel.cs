using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BMB.ViewModels
{
    public partial class ProductSelectionViewModel : BaseViewModel
    {
        private readonly IProductRepository _productRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IClientRepository _clientRepo;
        private readonly UserUtility _userUtility;

        public ObservableCollection<ProductQuantityModel> Products { get; set; } = new();
        public ObservableCollection<ProductQuantityModel> SelectedProducts { get; set; } = new();

      

        [ObservableProperty]
        private Client _selectedClient = new Client();

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Now;

        private Order? _editingOrder;
        public bool IsEditing => _editingOrder != null;

        [ObservableProperty]
        private string? errorMessage;

        public decimal TotalPrice => Products.Sum(p => p.Price * p.Quantity);
        public bool CanCreateOrder => Products.Any(p => p.Quantity > 0 && p.Price > 0);

        public ProductSelectionViewModel(IProductRepository productRepo, IOrderRepository orderRepo, IClientRepository clientRepo, UserUtility userUtility)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _clientRepo = clientRepo;
            _userUtility = userUtility;

            SubscribeToProductEvents();
        }

       
        private void SubscribeToProductEvents()
        {
            Products.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (ProductQuantityModel item in e.NewItems)
                        item.PropertyChanged += Product_PropertyChanged;
                }
                if (e.OldItems != null)
                {
                    foreach (ProductQuantityModel item in e.OldItems)
                        item.PropertyChanged -= Product_PropertyChanged;
                }
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(CanCreateOrder));
                UpdateSelectedProducts();
            };

            foreach (var product in Products)
                product.PropertyChanged += Product_PropertyChanged;
        }

        private void Product_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProductQuantityModel.Quantity) ||
                e.PropertyName == nameof(ProductQuantityModel.Price))
            {
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(CanCreateOrder));
                UpdateSelectedProducts();
            }
        }

        private void UpdateSelectedProducts()
        {
            SelectedProducts.Clear();
            foreach (var product in Products.Where(p => p.Quantity > 0))
            {
                SelectedProducts.Add(product);
            }
        }

        public async Task LoadProductsAsync()
        {
            try
            {
                ErrorMessage = null;

                var products = await _productRepo.GetAllProducts() ?? new List<Product>();

                var productModels = products.Select(product => new ProductQuantityModel
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 0,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                }).ToList();

                Products = new ObservableCollection<ProductQuantityModel>(productModels);
                SubscribeToProductEvents();

                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(CanCreateOrder));
                UpdateSelectedProducts();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load products.";
                Debug.WriteLine($"[LoadProductsAsync] {ex}");
            }
        }

        public async Task LoadOrderForEditAsync(int orderId)
        {
            try
            {
                ErrorMessage = null;

                _editingOrder = await _orderRepo.GetOrderById(orderId);
                if (_editingOrder == null) return;

                SelectedClient = await _clientRepo.GetClientById(_editingOrder.ClientId);
                SelectedDate = _editingOrder.Date;

                var productsTask = _productRepo.GetAllProducts();
                var orderItemsTask = _orderRepo.GetOrderItemsByOrderId(orderId);

                await Task.WhenAll(productsTask, orderItemsTask);

                var products = productsTask.Result ?? new List<Product>();
                var orderItems = orderItemsTask.Result ?? new List<OrderItem>();

                var productModels = products.Select(product =>
                {
                    var orderItem = orderItems.FirstOrDefault(oi => oi.ProductId == product.Id);
                    return new ProductQuantityModel
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Description = product.Description,
                        ImageUrl = product.ImageUrl,
                        Quantity = orderItem?.Quantity ?? 0
                    };
                }).ToList();

                Products = new ObservableCollection<ProductQuantityModel>(productModels);
                SubscribeToProductEvents();

                OnPropertyChanged(nameof(SelectedClient));
                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(CanCreateOrder));
                UpdateSelectedProducts();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load order for edit.";
                Debug.WriteLine($"[LoadOrderForEditAsync] {ex}");
            }
        }

      
        [RelayCommand(CanExecute = nameof(CanCreateOrder))]
        private async Task CreateOrderAsync()
        {
            try
            {
                ErrorMessage = null;

                var userId = await _userUtility.GetLoggedInUserIdAsync();
                var selectedProducts = Products.Where(p => p.Quantity > 0).ToList();
                var totalPrice = selectedProducts.Sum(p => p.Price * p.Quantity);

                var message = "";

                if (IsEditing && _editingOrder != null)
                {
                    _editingOrder.TotalPrice = totalPrice;
                    _editingOrder.Date = SelectedDate;

                    await _orderRepo.UpdateOrder(_editingOrder);
                    await _orderRepo.DeleteOrderItemsByOrderId(_editingOrder.Id);

                    foreach (var product in selectedProducts)
                    {
                        await _orderRepo.AddOrderItem(new OrderItem
                        {
                            OrderId = _editingOrder.Id,
                            ProductId = product.ProductId,
                            Quantity = product.Quantity,
                            Price = product.Price
                        });
                    }

                    message = "Order Updated Successfully!";
                }
                else
                {
                    var newOrder = new Order
                    {
                        UserId = userId,
                        ClientId = SelectedClient.Id,
                        Date = SelectedDate,
                        TotalPrice = totalPrice,
                        Notes = "New Order"
                    };

                    await _orderRepo.AddOrder(newOrder);

                    foreach (var product in selectedProducts)
                    {
                        await _orderRepo.AddOrderItem(new OrderItem
                        {
                            OrderId = newOrder.Id,
                            ProductId = product.ProductId,
                            Quantity = product.Quantity,
                            Price = product.Price
                        });
                    }

                    message = "Order Created Successfully!";
                }

                await Shell.Current.GoToAsync("..");
                await Shell.Current.DisplayAlert($"{message}", $"Items: {selectedProducts.Count}, Total: ${totalPrice:F2}", "OK");

                foreach (var product in Products)
                    product.Quantity = 0;

                OnPropertyChanged(nameof(TotalPrice));
                OnPropertyChanged(nameof(CanCreateOrder));
                UpdateSelectedProducts();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to create/update order.";
                Debug.WriteLine($"[CreateOrderAsync] {ex}");
                await Shell.Current.DisplayAlert("Error", "An error occurred while saving the order. Please try again.", "OK");
            }
        }
    }

    public partial class ProductQuantityModel : ObservableObject
    {
        public int ProductId { get; set; }

        [ObservableProperty]
        public string? name;

        [ObservableProperty]
        public decimal price;

        [ObservableProperty]
        public int quantity;

        [ObservableProperty]
        public string? imageUrl;

        [ObservableProperty]
        public string? description;

        public ProductQuantityModel()
        {
            IncreaseCommand = new RelayCommand(() => Quantity++);

            DecreaseCommand = new RelayCommand(() =>
            {
                if (Quantity > 0) Quantity--;
            });
        }

        public IRelayCommand IncreaseCommand { get; }
        public IRelayCommand DecreaseCommand { get; }
    }
}
