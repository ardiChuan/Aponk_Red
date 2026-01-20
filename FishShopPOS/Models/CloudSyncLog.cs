using SQLite;
using System;

namespace FishShopPOS.Models
{
    [Table("CloudSyncLog")]
    public class CloudSyncLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string TableName { get; set; } = string.Empty;

        [NotNull]
        public int RecordId { get; set; }

        [NotNull, MaxLength(20)]
        public string SyncStatus { get; set; } = "Pending"; // "Pending", "Success", "Failed"

        public DateTime LastSyncAttempt { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? ErrorMessage { get; set; }
    }
}