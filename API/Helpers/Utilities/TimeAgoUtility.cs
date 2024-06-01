namespace API.Helpers.Utilities
{
    public static class TimeAgoUtility
    {
        public static string TimeAgo(this DateTime dateTime, DateTime? from = null)
        {
            var now = from ?? DateTime.Now;
            var timeSpan = now.Subtract(dateTime);

            string result;
            if (timeSpan <= TimeSpan.FromSeconds(3))
            {
                result = "bây giờ";
            }
            else if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} giây trước", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = string.Format("{0} phút trước", timeSpan.Minutes);
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = string.Format("{0} giờ trước", timeSpan.Hours);
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = string.Format("{0} ngày trước", timeSpan.Days);
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = string.Format("{0} tháng trước", timeSpan.Days / 30);
            }
            else
            {
                result = string.Format("{0} năm trước", timeSpan.Days / 365);
            }

            return result;
        }
    }
}