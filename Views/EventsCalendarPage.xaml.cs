using BMB.DatabaseHelper.Interfaces;
using BMB.ViewModels;

namespace BMB.Views;

public partial class EventsCalendarPage : ContentPage
{
    private readonly EventsCalendarViewModel _viewModel;

    public EventsCalendarPage(IEventRepository eventRepository)
    {
        InitializeComponent();
        _viewModel = new EventsCalendarViewModel(eventRepository);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEventsAsync();
    }

    private void OnFabTapped(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = true;

        DescriptionEntry.Text = string.Empty;
        DateEntry.Date = DateTime.Today;

        DescriptionEntry.Focus();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = false;
    }

    private void OnAddEventClicked(object sender, EventArgs e)
    {
        PopupOverlay.IsVisible = false;
    }

}