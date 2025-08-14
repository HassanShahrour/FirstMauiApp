using SQLite;

namespace BMB.Data.Models
{
    public class OrderItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
