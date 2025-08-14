using SQLite;

namespace BMB.Data.Models
{
    public class Order
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ClientId { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal TotalPrice { get; set; }

        public string? Notes { get; set; }

    }
}
