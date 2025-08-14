using BMB.Utilities;
using BMB.ViewModels;
using System.Globalization;

namespace BMB.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage(LoginViewModel viewModel)  
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;

            FlowDirection = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "ar"
                ? FlowDirection.RightToLeft
                : FlowDirection.LeftToRight;
        }

        private void OnLanguageToggleClicked(object sender, EventArgs e)
        {
            var current = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var next = current == "en" ? "ar" : "en";

            LocalizationUtility.Instance.ChangeCulture(next);
            FlowDirection = next == "ar" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }



    }
}
