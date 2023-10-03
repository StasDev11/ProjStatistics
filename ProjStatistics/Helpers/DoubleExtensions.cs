namespace ProjStatistics.Helpers
{
    /// <summary>
    /// Расширения для строкового представления вещественного числа.
    /// </summary>
    public static class DoubleExtensions
    {
        public const string DoubleFormatString = "0.##";

        /// <summary>
        /// Возвращает строковое представление double.
        /// </summary>
        public static string GetFormattedString(this double d) => d.ToString(DoubleFormatString);
    }
}