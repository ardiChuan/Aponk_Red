using SQLite;

namespace FishShopPOS.Models
{
    [Table("TransactionItems")]
    public class TransactionItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int TransactionId { get; set; } // FK to Transactions

        [NotNull]
        public int ProductId { get; set; } // FK to Products

        [NotNull, MaxLength(200)]
        public string ProductName { get; set; } = string.Empty; // Snapshot at sale

        [NotNull]
        public int Quantity { get; set; }

        [NotNull]
        public decimal PriceAtSale { get; set; }

        [NotNull]
        public decimal Subtotal { get; set; }
    }
}