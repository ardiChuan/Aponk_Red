using SQLite;
using FishShopPOS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FishShopPOS.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        private const string DatabaseFilename = "FishShopPOS.db3";

        private const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;

        private static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public async Task InitializeAsync()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(DatabasePath, Flags);

            // Create all tables
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Product>();
            await _database.CreateTableAsync<Transaction>();
            await _database.CreateTableAsync<TransactionItem>();
            await _database.CreateTableAsync<StockAdjustment>();
            await _database.CreateTableAsync<MoneyTransaction>();
            await _database.CreateTableAsync<CashBalance>();
            await _database.CreateTableAsync<CloudSyncLog>();

            // Create indexes for performance
            await CreateIndexesAsync();

            // Seed initial data if needed
            await SeedInitialDataAsync();
        }

        private async Task CreateIndexesAsync()
        {
            if (_database == null) return;

            await _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_products_category ON Products(Category)");
            await _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_products_isdeleted ON Products(IsDeleted)");
            await _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_transactions_date ON Transactions(CreatedAt)");
            await _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_transactions_seller ON Transactions(SellerId)");
            await _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_stockadjustments_product ON StockAdjustments(ProductId)");
            await _database.ExecuteAsync("CREATE INDEX IF NOT EXISTS idx_moneytransactions_date ON MoneyTransactions(CreatedAt)");
        }

        private async Task SeedInitialDataAsync()
        {
            if (_database == null) return;

            // Check if any users exist
            var userCount = await _database.Table<User>().CountAsync();
            if (userCount == 0)
            {
                // Create default owner account
                var owner = new User
                {
                    PIN = "1234", // TODO: Hash this in production
                    FullName = "Owner",
                    Role = "Owner",
                    IsActive = true
                };
                await _database.InsertAsync(owner);
            }

            // Initialize today's cash balance if not exists
            var today = DateTime.Today;
            var todayBalance = await _database.Table<CashBalance>()
                .Where(cb => cb.Date == today)
                .FirstOrDefaultAsync();

            if (todayBalance == null)
            {
                var cashBalance = new CashBalance
                {
                    Date = today,
                    OpeningBalance = 0,
                    CashSales = 0,
                    Expenses = 0,
                    Returns = 0,
                    ClosingBalance = 0
                };
                await _database.InsertAsync(cashBalance);
            }
        }

        // Generic CRUD operations
        public async Task<int> InsertAsync<T>(T entity) where T : class, new()
        {
            await InitializeAsync();
            return await _database!.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : class, new()
        {
            await InitializeAsync();
            return await _database!.UpdateAsync(entity);
        }

        public async Task<int> DeleteAsync<T>(T entity) where T : class, new()
        {
            await InitializeAsync();
            return await _database!.DeleteAsync(entity);
        }

        public async Task<T?> GetAsync<T>(int id) where T : class, new()
        {
            await InitializeAsync();
            return await _database!.FindAsync<T>(id);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            await InitializeAsync();
            return await _database!.Table<T>().ToListAsync();
        }

        public AsyncTableQuery<T> Table<T>() where T : class, new()
        {
            InitializeAsync().Wait();
            return _database!.Table<T>();
        }

        // User-specific methods
        public async Task<User?> GetUserByPINAsync(string pin)
        {
            await InitializeAsync();
            return await _database!.Table<User>()
                .Where(u => u.PIN == pin && u.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            await InitializeAsync();
            return await _database!.Table<User>()
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        // Product-specific methods
        public async Task<List<Product>> GetActiveProductsAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Product>()
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            await InitializeAsync();
            return await _database!.Table<Product>()
                .Where(p => !p.IsDeleted && p.StockQuantity <= p.LowStockThreshold)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            await InitializeAsync();
            return await _database!.Table<Product>()
                .Where(p => !p.IsDeleted && p.Category == category)
                .ToListAsync();
        }

        // Transaction-specific methods
        public async Task<string> GenerateTransactionNumberAsync()
        {
            var today = DateTime.Today.ToString("yyyyMMdd");
            var count = await _database!.Table<Transaction>()
                .Where(t => t.TransactionNumber.StartsWith($"TXN-{today}"))
                .CountAsync();
            return $"TXN-{today}-{(count + 1):D3}";
        }

        public async Task<List<Transaction>> GetTransactionsByDateRangeAsync(DateTime start, DateTime end)
        {
            await InitializeAsync();
            return await _database!.Table<Transaction>()
                .Where(t => t.CreatedAt >= start && t.CreatedAt <= end)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TransactionItem>> GetTransactionItemsAsync(int transactionId)
        {
            await InitializeAsync();
            return await _database!.Table<TransactionItem>()
                .Where(ti => ti.TransactionId == transactionId)
                .ToListAsync();
        }

        // Cash Balance methods
        public async Task<CashBalance?> GetTodayCashBalanceAsync()
        {
            await InitializeAsync();
            var today = DateTime.Today;
            return await _database!.Table<CashBalance>()
                .Where(cb => cb.Date == today)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateCashBalanceAsync(decimal openingBalance, decimal cashSales, decimal expenses, decimal returns)
        {
            await InitializeAsync();
            var today = DateTime.Today;
            var balance = await GetTodayCashBalanceAsync();

            if (balance == null)
            {
                balance = new CashBalance
                {
                    Date = today,
                    OpeningBalance = openingBalance,
                    CashSales = cashSales,
                    Expenses = expenses,
                    Returns = returns,
                    ClosingBalance = openingBalance + cashSales - expenses + returns
                };
                await _database!.InsertAsync(balance);
            }
            else
            {
                balance.OpeningBalance = openingBalance;
                balance.CashSales = cashSales;
                balance.Expenses = expenses;
                balance.Returns = returns;
                balance.ClosingBalance = openingBalance + cashSales - expenses + returns;
                balance.UpdatedAt = DateTime.Now;
                await _database!.UpdateAsync(balance);
            }
        }

        // Backup and restore
        public async Task<string> BackupDatabaseAsync()
        {
            var backupFileName = $"FishShopPOS_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.db3";
            var backupPath = Path.Combine(FileSystem.AppDataDirectory, "Backups", backupFileName);

            var backupDir = Path.GetDirectoryName(backupPath);
            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir!);

            File.Copy(DatabasePath, backupPath, true);
            return backupPath;
        }

        public async Task<bool> RestoreDatabaseAsync(string backupPath)
        {
            try
            {
                if (_database != null)
                    await _database.CloseAsync();

                File.Copy(backupPath, DatabasePath, true);
                _database = null;
                await InitializeAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}