using BMB.Data;
using BMB.DatabaseHelper.Interfaces;
using BMB.DatabaseHelper.Repositories;
using BMB.Resources.Themes;
using BMB.Services;
using BMB.Utilities;
using BMB.ViewModels;
using BMB.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Syncfusion.Maui.Core.Hosting;
using System.Globalization;

namespace BMB
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureSyncfusionCore();

            CultureInfo.DefaultThreadCurrentCulture = new("en");
            CultureInfo.DefaultThreadCurrentUICulture = new("en");

            builder.Services.AddSingleton<TranslateUtility>();
            builder.Services.AddSingleton<ThemeUtility>();
            builder.Services.AddSingleton<LightTheme>();
            builder.Services.AddSingleton<DarkTheme>();

            builder.Services.AddSingleton<IAppDatabase, AppDatabase>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
            builder.Services.AddSingleton<IClientRepository, ClientRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();
            builder.Services.AddSingleton<IEventRepository, EventRepository>();
            builder.Services.AddSingleton<UserUtility>();
            builder.Services.AddSingleton<EventReminderService>();

            builder.Services.AddSingleton<MainPage>();

            builder.ConfigureSyncfusionCore();

            RegisterView(builder);

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterView(MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<RegisterViewModel>();
           
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<UsersViewModel>();

            builder.Services.AddSingleton<OrderListViewModel>();

            builder.Services.AddSingleton<ClientViewModel>();

            builder.Services.AddSingleton<ProductViewModel>();

            builder.Services.AddSingleton<ProductSelectionViewModel>();

            builder.Services.AddSingleton<OrdersCalendarViewModel>();

            builder.Services.AddSingleton<EventsCalendarViewModel>();

            builder.Services.AddSingleton<CountrySelectionViewModel>();

            builder.Services.AddSingleton<PickerViewModel>();
        }


    }
}
