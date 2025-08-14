using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BMB.ViewModels
{
    public partial class OrdersCalendarViewModel : BaseViewModel
    {
        private readonly IOrderRepository _orderRepo;

        public ObservableCollection<Order> Orders { get; } = new();

        [ObservableProperty]
        ObservableCollection<Order> ordersForSelectedDate = new();

        [ObservableProperty]
        DateTime selectedDate = DateTime.Today;

        public ObservableCollection<CalendarEvent> Events { get; } = new();

        public OrdersCalendarViewModel(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            FilterOrdersForDate(value);
        }

        public async Task LoadOrders()
        {
            var orders = await _orderRepo.GetAllOrders();
            Orders.Clear();

            foreach (var order in orders)
                Orders.Add(order);

            FilterOrdersForDate(SelectedDate);
            await PopulateEvents();
        }

        public async Task PopulateEvents()
        {
            Events.Clear();

            var grouped = await Task.Run(() =>
                Orders
                    .GroupBy(o => o.Date.Date)
                    .ToDictionary(g => g.Key, g => g.Count())
            );

            DateTime firstOfMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
            int startOffset = (int)firstOfMonth.DayOfWeek;

            DateTime startDate = firstOfMonth.AddDays(-startOffset);

            for (int i = 0; i < 42; i++)
            {
                DateTime date = startDate.AddDays(i);

                var ev = new CalendarEvent
                {
                    Date = date,
                    Count = grouped.TryGetValue(date, out var count) ? count : 0,
                    IsCurrentMonth = date.Month == SelectedDate.Month
                };

                Events.Add(ev);
            }
        }

        void FilterOrdersForDate(DateTime date)
        {
            var filtered = Orders.Where(o => o.Date.Date == date.Date).ToList();
            OrdersForSelectedDate = new ObservableCollection<Order>(filtered);
        }
    }

    public class CalendarEvent
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public bool IsCurrentMonth { get; set; }
    }

}
