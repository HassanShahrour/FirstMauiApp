using SQLite;

namespace BMB.Data.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
