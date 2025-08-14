using BMB.Data.Models;

namespace BMB.DatabaseHelper.Interfaces
{
    public interface IEventRepository
    {
        Task<int> AddEvent(EventItem eventItem);
        Task<int> UpdateEvent(EventItem eventItem);
        Task<int> DeleteEvent(EventItem eventItem);
        Task<List<EventItem>> GetAllEvents();
        Task<EventItem> GetEventById(int id);
    }
}
