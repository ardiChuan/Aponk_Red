using System;
using System.IO;
using System.Threading.Tasks;
using FishShopPOS.Helpers;

namespace FishShopPOS.Services
{
    public class FileService
    {
        private readonly string _documentsPath;

        public FileService()
        {
            _documentsPath = Path.Combine(FileSystem.AppDataDirectory, "Documents");
            EnsureDirectoriesExist();
        }

        private void EnsureDirectoriesExist()
        {
            Directory.CreateDirectory(Path.Combine(_documentsPath, Constants.ReceiptsFolder));
            Directory.CreateDirectory(Path.Combine(_documentsPath, Constants.TransferProofsFolder));
            Directory.CreateDirectory(Path.Combine(_documentsPath, Constants.ExpenseReceiptsFolder));
            Directory.CreateDirectory(Path.Combine(_documentsPath, Constants.BackupsFolder));
        }

        public async Task<string> SaveReceiptAsync(byte[] pdfData, string transactionNumber)
        {
            var fileName = $"Receipt_{transactionNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(_documentsPath, Constants.ReceiptsFolder, fileName);
            
            await File.WriteAllBytesAsync(filePath, pdfData);
            return filePath;
        }

        public async Task<string> SaveImageAsync(byte[] imageData, string folder, string prefix)
        {
            var fileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            var filePath = Path.Combine(_documentsPath, folder, fileName);
            
            await File.WriteAllBytesAsync(filePath, imageData);
            return filePath;
        }

        public async Task<string> SaveTransferProofAsync(byte[] imageData, string transactionNumber)
        {
            return await SaveImageAsync(imageData, Constants.TransferProofsFolder, $"Transfer_{transactionNumber}");
        }

        public async Task<string> SaveExpenseReceiptAsync(byte[] imageData)
        {
            return await SaveImageAsync(imageData, Constants.ExpenseReceiptsFolder, "Expense");
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public async Task<byte[]?> ReadFileAsync(string filePath)
        {
            if (!FileExists(filePath))
                return null;

            return await File.ReadAllBytesAsync(filePath);
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                if (FileExists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public long GetFileSize(string filePath)
        {
            if (!FileExists(filePath))
                return 0;

            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }
    }
}
