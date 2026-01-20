using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("MoneyTransactions")]
    public class MoneyTransaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserId { get; set; } // FK to Users

        [NotNull, MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;
        // "CashSale", "TransferSale", "Expense", "OpeningBalance", "Return"

        [NotNull]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; } // For expenses

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ReceiptImagePath { get; set; }

        public int? RelatedTransactionId { get; set; } // FK to Transactions

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool SyncedToCloud { get; set; } = false;
    }
}   