using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;

namespace BMB.DatabaseHelper.Repositories
{
    class EventRepository : IEventRepository
    {
        private readonly IAppDatabase _db;

        public EventRepository(IAppDatabase db)
        {
            _db = db;
        }

        public async Task<int> AddEvent(EventItem eventItem)
        {
            try
            {
                return await _db.AddAsync(eventItem);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding event", ex);
            }
        }

        public async Task<int> DeleteEvent(EventItem eventItem)
        {
            try
            {
                return await _db.DeleteAsync(eventItem);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting event", ex);
            }
        }

        public async Task<List<EventItem>> GetAllEvents()
        {
            try
            {
                return await _db.GetAllAsync<EventItem>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving events", ex);
            }
        }

        public async Task<EventItem> GetEventById(int id)
        {
            try
            {
                return await _db.GetByIdAsync<EventItem>(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving event with ID {id}", ex);
            }
        }

        public async Task<int> UpdateEvent(EventItem eventItem)
        {
            try
            {
                return await _db.UpdateAsync(eventItem);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating event", ex);
            }
        }


    }
}
