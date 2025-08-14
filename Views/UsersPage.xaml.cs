using BMB.ViewModels;

namespace BMB.Views;

[QueryProperty(nameof(UserId), "userId")]
public partial class UsersPage : ContentPage
{
    public string? UserId { get; set; }

    private readonly UsersViewModel _viewModel;
    public UsersPage(UsersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadUsersAsync();
    }

}