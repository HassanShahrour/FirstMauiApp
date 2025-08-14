using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BMB.ViewModels
{
    public partial class EventsCalendarViewModel : BaseViewModel
    {
        private readonly IEventRepository _eventRepo;

        public List<string> Types { get; } = new()
        {
            "Meeting",
            "Call",
            "Deadline",
            "Holiday",
            "Training",
            "Travel",
            "Other"
        };

        public Dictionary<string, string> TypeColorMap { get; } = new()
        {
            { "Meeting", "#007AFF" },
            { "Call", "#27AE60" },
            { "Deadline", "#E74C3C" },
            { "Holiday", "#F39C12" },
            { "Training", "#9B59B6" },
            { "Travel", "#16A085" },
            { "Other", "#95A5A6" }
        };

        public ObservableCollection<SchedulerAppointment> Appointments { get; } = new();

        public ObservableCollection<EventItem> AllEvents { get; set; } = new();
        public ObservableCollection<CalendarEventItem> Events { get; set; } = new();

        [ObservableProperty]
        ObservableCollection<EventItem> eventsForSelectedDate = new();

        [ObservableProperty]
        DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? selectedType;

        [ObservableProperty]
        private DateTime date = DateTime.Today;

        [ObservableProperty]
        private TimeSpan time = DateTime.Now.TimeOfDay;

        [ObservableProperty]
        private string? errorMessage;

        public EventsCalendarViewModel(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            FilterEventsForDate(value);
        }

        public async Task LoadEventsAsync()
        {
            try
            {
                ErrorMessage = null;

                var events = await _eventRepo.GetAllEvents();

                AllEvents = new ObservableCollection<EventItem>(events);

                FilterEventsForDate(SelectedDate);

                //Maybe we can remove this line if we don't need to filter again (review later)
                await PopulateEvents();

                await LoadAppointmentsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load events. Please try again.";
                Debug.WriteLine($"[LoadEventsAsync] {ex}");
            }
        }

        public async Task PopulateEvents()
        {
            Events.Clear();

            var grouped = await Task.Run(() =>
                AllEvents
                    .GroupBy(e => e.Date.Date)
                    .ToDictionary(g => g.Key, g => g.Count())
            );

            DateTime firstOfMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
            int startOffset = (int)firstOfMonth.DayOfWeek;
            DateTime startDate = firstOfMonth.AddDays(-startOffset);

            for (int i = 0; i < 42; i++)
            {
                DateTime date = startDate.AddDays(i);

                Events.Add(new CalendarEventItem
                {
                    Date = date,
                    Count = grouped.TryGetValue(date, out var count) ? count : 0,
                    IsCurrentMonth = date.Month == SelectedDate.Month
                });
            }
        }

        public async Task LoadAppointmentsAsync()
        {
            Appointments.Clear();

            foreach (var ev in AllEvents)
            {
                Appointments.Add(new SchedulerAppointment
                {
                    StartTime = ev.Date,
                    EndTime = ev.Date.AddHours(1),
                    Subject = ev.Description ?? "",
                    Background = Color.FromArgb(ev.ColorCode ?? "#BDC3C7"),
                });
            }
        }

        void FilterEventsForDate(DateTime date)
        {
            var filtered = AllEvents.Where(e => e.Date.Date == date.Date).ToList();
            EventsForSelectedDate = new ObservableCollection<EventItem>(filtered);
        }

        [RelayCommand]
        public async Task AddEventAsync()
        {
            try
            {
                ErrorMessage = null;

                if (string.IsNullOrWhiteSpace(Description))
                {
                    ErrorMessage = "Description is required.";
                    return;
                }

                var newEvent = new EventItem
                {
                    Description = Description.Trim(),
                    Type = SelectedType,
                    Date = Date.Date + Time,
                    ColorCode = TypeColorMap.TryGetValue(SelectedType ?? "", out var color) ? color : "#BDC3C7"
                };

                await _eventRepo.AddEvent(newEvent);

                AllEvents.Add(newEvent);

                if (newEvent.Date.Date == SelectedDate.Date)
                    EventsForSelectedDate.Add(newEvent);

                await LoadEventsAsync();

                var notifyTime = newEvent.Date.AddSeconds(-10);
                Debug.WriteLine($"NotifyTime: {notifyTime}, Now: {DateTime.Now}");

#if ANDROID
                if (notifyTime > DateTime.Now)
                {
                    var alarmService = new BMB.Platforms.Android.AndroidAlarmService();
                    alarmService.ScheduleAlarm("Upcoming Event", $"{newEvent.Type}: {newEvent.Description}", notifyTime);
                }
                else
                {
                    Debug.WriteLine("NotifyTime is in the past. Alarm not scheduled.");
                }
#else
        if (notifyTime > DateTime.Now)
        {
            var notification = new NotificationRequest
            {
                NotificationId = int.Parse(DateTime.Now.ToString("MMddHHmmss")),
                Title = "Upcoming Event",
                Description = $"{newEvent.Type}: {newEvent.Description}",
                Android = new AndroidOptions
                {
                    IconSmallName = new AndroidIcon(),
                    ChannelId = "default_channel"
                },
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                }
            };

            await LocalNotificationCenter.Current.Show(notification);
        }
        else
        {
            Debug.WriteLine("notifyTime is in the past. Notification not scheduled.");
        }
#endif

                Description = string.Empty;
                SelectedType = string.Empty;
                Date = DateTime.Today;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to add event. Please try again.";
                Debug.WriteLine($"[AddEventAsync] {ex}");
            }
        }




    }

    public class CalendarEventItem
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public bool IsCurrentMonth { get; set; }
    }
}
