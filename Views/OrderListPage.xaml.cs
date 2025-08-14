using BMB.DatabaseHelper.Interfaces;
using BMB.Utilities;
using BMB.ViewModels;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
#if ANDROID
using Android;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Android.Content.PM;
#endif

namespace BMB.Views
{
    public partial class OrderListPage : ContentPage
    {
        private readonly OrderListViewModel _viewModel;

        public OrderListPage(IOrderRepository orderRepo, IClientRepository clientRepo, UserUtility userUtility)
        {
            InitializeComponent();
            _viewModel = new OrderListViewModel(orderRepo, clientRepo, userUtility);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

        #if ANDROID
            await RequestNotificationPermissionAsync();
        #endif

            await _viewModel.LoadOrdersAsync();
        }

#if ANDROID
        private Task RequestNotificationPermissionAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            var context = Android.App.Application.Context;

            if (ContextCompat.CheckSelfPermission(context, Manifest.Permission.PostNotifications) == Permission.Granted)
            {
                tcs.SetResult(true);
            }
            else
            {
                ActivityCompat.RequestPermissions(Platform.CurrentActivity,
                    new[] { Manifest.Permission.PostNotifications }, 1001);

                // NOTE: For a real app, you would need to handle the callback from permission request and set the result here.
                // For simplicity, we just complete immediately.
                tcs.SetResult(true);
            }

            return tcs.Task;
        }
#endif


        private void OnFabTapped(object sender, EventArgs e)
        {
            PopupOverlay.IsVisible = true;
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            PopupOverlay.IsVisible = false;
        }

        private async void OnNextClicked(object sender, EventArgs e)
        {
            if (_viewModel.SelectedClient != null)
            {
                string dateString = Uri.EscapeDataString(_viewModel.SelectedDate.ToString("O"));
                await Shell.Current.GoToAsync($"{nameof(ProductSelectionPage)}?ClientId={_viewModel.SelectedClient.Id}&SelectedDate={dateString}");
            }

            PopupOverlay.IsVisible = false;
        }

    }
}
