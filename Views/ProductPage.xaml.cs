using BMB.DatabaseHelper.Interfaces;
using BMB.ViewModels;

namespace BMB.Views;

public partial class ProductPage : ContentPage
{

    private readonly ProductViewModel _viewModel;
    public ProductPage(IProductRepository productRepository)
    {
        InitializeComponent();
        _viewModel = new ProductViewModel(productRepository);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadProductsAsync();
    }

    private void OnFabTapped(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;

        NameEntry.Text = string.Empty;
        PriceEntry.Text = string.Empty;
        QuantityEntry.Text = string.Empty;

        NameEntry.Focus();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = false;
    }

    private void OnAddProductClicked(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = false;
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select a product image"
            });

            if (result != null)
            {
                // Get the file path or copy it to your app folder if needed
                string filePath = result.FullPath;

                // Option 1: Just use the file path (works on some platforms)
                // Update the ViewModel property for ImageUrl to the local file path
                BindingContext.GetType().GetProperty("ImageUrl")?.SetValue(BindingContext, filePath);

                // Option 2: Copy the file to local app folder, then save relative path
                // var newPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                // using var sourceStream = await result.OpenReadAsync();
                // using var destStream = File.OpenWrite(newPath);
                // await sourceStream.CopyToAsync(destStream);
                // BindingContext.GetType().GetProperty("ImageUrl")?.SetValue(BindingContext, newPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
        }
    }


}