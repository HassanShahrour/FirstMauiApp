using BMB.ViewModels;

namespace BMB.Views;

public partial class CountrySelectionPage : ContentPage
{
	public CountrySelectionPage()
	{
		InitializeComponent();
        BindingContext = new CountrySelectionViewModel();
    }
}