using BMB.ViewModels;

namespace BMB.Views;

public partial class PickerPage : ContentPage
{

    private readonly PickerViewModel _viewModel;
    public PickerPage(PickerViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    
}