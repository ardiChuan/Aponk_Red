using System;

namespace FishShopPOS.Helpers
{
    public static class DateTimeHelper
    {
        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd MMM yyyy HH:mm");
        }

        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("dd MMM yyyy");
        }

        public static string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("HH:mm");
        }

        public static string FormatRelativeTime(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} hours ago";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} days ago";
            
            return FormatDate(dateTime);
        }

        public static (DateTime Start, DateTime End) GetDateRange(string range)
        {
            var now = DateTime.Now;
            return range.ToLower() switch
            {
                "today" => (now.Date, now.Date.AddDays(1).AddSeconds(-1)),
                "yesterday" => (now.Date.AddDays(-1), now.Date.AddSeconds(-1)),
                "week" => (now.Date.AddDays(-7), now),
                "month" => (now.Date.AddMonths(-1), now),
                "year" => (now.Date.AddYears(-1), now),
                _ => (now.Date, now.Date.AddDays(1).AddSeconds(-1))
            };
        }
    }
}
