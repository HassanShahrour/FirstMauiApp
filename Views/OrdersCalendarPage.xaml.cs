using BMB.DatabaseHelper.Interfaces;
using BMB.ViewModels;

namespace BMB.Views;

public partial class OrdersCalendarPage : ContentPage
{
    private readonly OrdersCalendarViewModel _viewModel;

    public OrdersCalendarPage(IOrderRepository orderRepo)
    {
        InitializeComponent();
        _viewModel = new OrdersCalendarViewModel(orderRepo);
        BindingContext = _viewModel;
        Loaded += OrdersCalendarPage_Loaded;
    }

    private async void OrdersCalendarPage_Loaded(object? sender, EventArgs e)
    {
        await _viewModel.LoadOrders();
    }
}

