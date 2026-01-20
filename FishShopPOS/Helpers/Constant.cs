namespace FishShopPOS.Helpers
{
    public static class Constants
    {
        // App Info
        public const string AppName = "FishShop POS";
        public const string AppVersion = "1.0.0";

        // Roles
        public const string RoleOwner = "Owner";
        public const string RoleAttendant = "Attendant";

        // Transaction Types
        public const string TransactionTypeSale = "Sale";
        public const string TransactionTypeReturn = "Return";

        // Payment Methods
        public const string PaymentMethodCash = "Cash";
        public const string PaymentMethodTransfer = "Transfer";

        // Stock Adjustment Types
        public const string AdjustmentNewProduct = "NewProduct";
        public const string AdjustmentRestock = "Restock";
        public const string AdjustmentSale = "Sale";
        public const string AdjustmentReturn = "Return";
        public const string AdjustmentManual = "ManualAdjustment";
        public const string AdjustmentEdit = "Edit";

        // Money Transaction Types
        public const string MoneyCashSale = "CashSale";
        public const string MoneyTransferSale = "TransferSale";
        public const string MoneyExpense = "Expense";
        public const string MoneyOpeningBalance = "OpeningBalance";
        public const string MoneyReturn = "Return";

        // Expense Categories
        public static readonly string[] ExpenseCategories = new[]
        {
            "Stock Purchase",
            "Supplies",
            "Utilities",
            "Rent",
            "Salaries",
            "Maintenance",
            "Marketing",
            "Other"
        };

        // File Paths
        public const string ReceiptsFolder = "Receipts";
        public const string TransferProofsFolder = "TransferProofs";
        public const string ExpenseReceiptsFolder = "ExpenseReceipts";
        public const string BackupsFolder = "Backups";

        // Defaults
        public const int DefaultLowStockThreshold = 10;
        public const int DefaultPINLength = 4;
        public const int MaxPINLength = 6;

        // Sync Settings
        public const int AutoSyncIntervalMinutes = 60; // 1 hour
    }
}