using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(6)]
        public string PIN { get; set; } = string.Empty;

        [NotNull, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [NotNull, MaxLength(20)]
        public string Role { get; set; } = "Attendant"; // "Owner" or "Attendant"

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}