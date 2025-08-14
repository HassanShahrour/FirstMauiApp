using SQLite;

namespace BMB.Data.Models
{
    public class EventItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Description { get; set; }

        public string? Type { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string ColorCode { get; set; } = "#007AFF";

        [Ignore]
        public bool Notified { get; set; } = false;
    }
}
