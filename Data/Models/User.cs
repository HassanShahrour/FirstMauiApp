using SQLite;

namespace BMB.Data.Models
{

    [Table("Users")]
    public class User
    {

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("FullName")]
        public string? FullName { get; set; }

        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        [Column("Password")]
        public string? Password { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
