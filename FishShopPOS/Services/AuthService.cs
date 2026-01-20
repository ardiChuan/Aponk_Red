using FishShopPOS.Models;
using System.Threading.Tasks;

namespace FishShopPOS.Services
{
    public class AuthService
    {
        private readonly DatabaseService _database;
        private User? _currentUser;

        public AuthService(DatabaseService database)
        {
            _database = database;
        }

        public User? CurrentUser => _currentUser;

        public bool IsAuthenticated => _currentUser != null;

        public bool IsOwner => _currentUser?.Role == Helpers.Constants.RoleOwner;

        public bool IsAttendant => _currentUser?.Role == Helpers.Constants.RoleAttendant;

        public async Task<(bool Success, string Message)> LoginAsync(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
                return (false, "Please enter your PIN");

            if (!Helpers.ValidationHelper.IsValidPIN(pin))
                return (false, "Invalid PIN format");

            var user = await _database.GetUserByPINAsync(pin);
            
            if (user == null)
                return (false, "Invalid PIN");

            if (!user.IsActive)
                return (false, "This account is inactive");

            _currentUser = user;
            return (true, $"Welcome, {user.FullName}!");
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public async Task<bool> ChangePINAsync(string oldPIN, string newPIN)
        {
            if (_currentUser == null)
                return false;

            if (_currentUser.PIN != oldPIN)
                return false;

            if (!Helpers.ValidationHelper.IsValidPIN(newPIN))
                return false;

            _currentUser.PIN = newPIN;
            _currentUser.UpdatedAt = System.DateTime.Now;
            await _database.UpdateAsync(_currentUser);
            return true;
        }
    }
}
