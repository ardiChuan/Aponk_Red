using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("CashBalance")]
    public class CashBalance
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, Unique]
        public DateTime Date { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal CashSales { get; set; }

        public decimal Expenses { get; set; }

        public decimal Returns { get; set; }

        public decimal ClosingBalance { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}