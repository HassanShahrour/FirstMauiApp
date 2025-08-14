using BMB.ViewModels;

namespace BMB.Views;


public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
