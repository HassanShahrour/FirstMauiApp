using BMB.Views;

namespace BMB;

public partial class AuthShell : Shell
{
    public AuthShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
    }
}