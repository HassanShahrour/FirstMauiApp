using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BMB.ViewModels
{
    public partial class ProductViewModel : ObservableObject
    {
        private readonly IProductRepository _productRepo;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddProductCommand))]
        private string? name;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddProductCommand))]
        private decimal price;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddProductCommand))]
        private int quantity;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string imageUrl;

        [ObservableProperty]
        private string? errorMessage;

        public ObservableCollection<Product> Products { get; set;  } = new();

        public ProductViewModel(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task LoadProductsAsync()
        {
            try
            {
                ErrorMessage = null;

                var products = await _productRepo.GetAllProducts();

                Products = new ObservableCollection<Product>(products);

                OnPropertyChanged(nameof(Products));

            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load products. Please try again.";
                Debug.WriteLine($"[LoadProductsAsync] {ex}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanAddProduct))]
        public async Task AddProductAsync()
        {
            try
            {
                ErrorMessage = null;

                var newProduct = new Product
                {
                    Name = Name!.Trim(),
                    Price = Price,
                    Quantity = Quantity,
                    Description = Description?.Trim(),
                    ImageUrl = ImageUrl?.Trim()
                };

                await _productRepo.AddProduct(newProduct);

                Products.Add(newProduct);

                Name = string.Empty;
                Price = 0;
                Quantity = 0;
                Description = string.Empty;
                ImageUrl = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to add product. Please try again.";
                Debug.WriteLine($"[AddProductAsync] {ex}");
            }
        }

        [RelayCommand]
        public async void DeleteProduct(Product product)
        {
            if (product != null && Products.Contains(product))
            {
                Products.Remove(product);
                await _productRepo.DeleteProduct(product);
            }
        }

        private bool CanAddProduct()
        {
            return true;
            //return !string.IsNullOrWhiteSpace(Name)
            //    && Price > 0
            //    && Quantity >= 0
            //    && !string.IsNullOrWhiteSpace(Description)
            //    && !string.IsNullOrWhiteSpace(ImageUrl);
        }
    }
}
