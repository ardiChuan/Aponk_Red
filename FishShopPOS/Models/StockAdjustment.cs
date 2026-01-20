using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("StockAdjustments")]
    public class StockAdjustment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int ProductId { get; set; } // FK to Products

        [NotNull]
        public int UserId { get; set; } // FK to Users

        [NotNull, MaxLength(50)]
        public string AdjustmentType { get; set; } = string.Empty;
        // "NewProduct", "Restock", "Sale", "Return", "ManualAdjustment", "Edit"

        public int QuantityChange { get; set; }

        public int PreviousQuantity { get; set; }

        public int NewQuantity { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool SyncedToCloud { get; set; } = false;
    }
}