using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("Products")]
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [NotNull, MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [NotNull]
        public decimal Price { get; set; }

        [NotNull]
        public int StockQuantity { get; set; } = 0;

        public int LowStockThreshold { get; set; } = 10;

        [MaxLength(200)]
        public string? Supplier { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int? LastEditedBy { get; set; } // FK to Users

        [Ignore]
        public bool IsLowStock => StockQuantity <= LowStockThreshold && StockQuantity > 0;

        [Ignore]
        public bool IsOutOfStock => StockQuantity <= 0;
    }
}