using System;
using System.Globalization;

namespace FishShopPOS.Helpers
{
    public static class CurrencyHelper
    {
        private static readonly CultureInfo IndonesianCulture = new CultureInfo("id-ID");

        /// <summary>
        /// Format decimal amount to Indonesian Rupiah string
        /// </summary>
        public static string FormatIDR(decimal amount)
        {
            return $"Rp {amount:N0}";
        }

        /// <summary>
        /// Format decimal amount to Indonesian Rupiah string with detailed formatting
        /// </summary>
        public static string FormatIDRDetailed(decimal amount)
        {
            return amount.ToString("C0", IndonesianCulture);
        }

        /// <summary>
        /// Parse IDR string to decimal
        /// </summary>
        public static decimal ParseIDR(string idrString)
        {
            // Remove "Rp" and any whitespace
            var cleaned = idrString.Replace("Rp", "").Replace(" ", "").Replace(".", "").Replace(",", "");
            
            if (decimal.TryParse(cleaned, out decimal result))
                return result;
            
            return 0;
        }

        /// <summary>
        /// Format number input for currency display (as user types)
        /// </summary>
        public static string FormatCurrencyInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "0";

            // Remove non-numeric characters
            var cleaned = new string(Array.FindAll(input.ToCharArray(), char.IsDigit));
            
            if (string.IsNullOrEmpty(cleaned))
                return "0";

            if (decimal.TryParse(cleaned, out decimal amount))
                return amount.ToString("N0");

            return "0";
        }

        /// <summary>
        /// Calculate discount amount based on percentage or fixed value
        /// </summary>
        public static decimal CalculateDiscount(decimal totalAmount, decimal discountValue, bool isPercentage)
        {
            if (isPercentage)
            {
                // Ensure percentage is between 0-100
                discountValue = Math.Max(0, Math.Min(100, discountValue));
                return totalAmount * (discountValue / 100);
            }
            else
            {
                // Ensure discount doesn't exceed total
                return Math.Min(discountValue, totalAmount);
            }
        }

        /// <summary>
        /// Round to nearest Rupiah (no decimals)
        /// </summary>
        public static decimal RoundIDR(decimal amount)
        {
            return Math.Round(amount, 0, MidpointRounding.AwayFromZero);
        }
    }
}
