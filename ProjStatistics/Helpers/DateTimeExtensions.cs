namespace ProjStatistics.Helpers
{
    /// <summary>
    /// Расширения для строкового представления DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Возвращает относительное время (с учетом количества прошедших дней).
        /// </summary>
        public static string DaysAgoString(this int daysAgo)
        {
            switch (daysAgo)
            {
                case 0: return "less than a day";

                case 1: return "yesterday";

                case 2: return "day before yesterday";

                case 7: return "a week ago";

                case 30: return "a month ago";

                default: break;
            };

            var daysAgoStr = daysAgo + " days ago";

            if (daysAgo > 30 && daysAgo < 60)
            {
                daysAgoStr += " - more than a month";
            }
            else
            if (daysAgo > 60 && daysAgo < 90)
            {
                daysAgoStr += " - more than two months";
            }
            else
            if (daysAgo > 90 && daysAgo < 120)
            {
                daysAgoStr += " - more than three month";
            }
            else
            if (daysAgo > 180 && daysAgo < 365)
            {
                daysAgoStr += " - more than six months";
            }
            if (daysAgo > 365 && daysAgo < 730)
            {
                daysAgoStr += " - more than a year";
            }
            else
            if (daysAgo > 730 && daysAgo < 1095)
            {
                daysAgoStr += " - more than two years";
            }
            else
            if (daysAgo > 1095 && daysAgo < 1460)
            {
                daysAgoStr += " - more than three years";
            }

            return daysAgoStr;
        }
    }
}