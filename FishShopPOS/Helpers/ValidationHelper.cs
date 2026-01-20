using  System.Text.RegularExpressions;

namespace FishShopPOS.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidPIN(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin))
                return false;

            return pin.Length >= Constants.DefaultPINLength && 
                   pin.Length <= Constants.MaxPINLength && 
                   Regex.IsMatch(pin, @"^\d+$");
        }

        public static bool IsValidProductName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length <= 200;
        }

        public static bool IsValidPrice(decimal price)
        {
            return price > 0;
        }

        public static bool IsValidQuantity(int quantity)
        {
            return quantity > 0;
        }

        public static bool IsValidDiscount(decimal discount, bool isPercentage, decimal totalAmount)
        {
            if (discount < 0)
                return false;

            if (isPercentage)
                return discount <= 100;
            else
                return discount <= totalAmount;
        }
    }
}