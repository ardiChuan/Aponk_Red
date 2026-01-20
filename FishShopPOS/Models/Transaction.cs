using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique, MaxLength(50)]
        public string TransactionNumber { get; set; } = string.Empty;

        [NotNull]
        public int SellerId { get; set; } // FK to Users

        [MaxLength(200)]
        public string? BuyerName { get; set; }

        [NotNull]
        public decimal TotalAmount { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        [NotNull]
        public decimal FinalAmount { get; set; }

        [NotNull, MaxLength(20)]
        public string PaymentMethod { get; set; } = "Cash"; // "Cash" or "Transfer"

        [MaxLength(500)]
        public string? TransferProofPath { get; set; }

        [MaxLength(500)]
        public string? ReceiptFilePath { get; set; }

        [NotNull, MaxLength(20)]
        public string TransactionType { get; set; } = "Sale"; // "Sale" or "Return"

        public int? OriginalTransactionId { get; set; } // FK to Transactions if return

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool SyncedToCloud { get; set; } = false;
    }
}   