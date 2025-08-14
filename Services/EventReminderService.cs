using BMB.DatabaseHelper.Interfaces;
using Plugin.LocalNotification;

namespace BMB.Services
{
    public class EventReminderService
    {
        private readonly IEventRepository _eventRepo;

        public EventReminderService(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task ScheduleAllRemindersAsync()
        {
            try
            {
                var events = await _eventRepo.GetAllEvents();

                if (events == null || !events.Any())
                    return;

                var now = DateTime.Now;

                foreach (var ev in events)
                {
                    try
                    {
                        var reminderTime = ev.Date.AddSeconds(-10);

                        if (reminderTime <= now)
                            continue;

                        var notification = new NotificationRequest
                        {
                            NotificationId = ev.Id,
                            Title = "Event Reminder",
                            Description = $"'{ev.Description}' starts soon.",
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = reminderTime
                            }
                        };

                        await LocalNotificationCenter.Current.Show(notification);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to schedule notification for event {ev.Id}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while scheduling reminders: {ex.Message}");
            }
        }



    }
}
