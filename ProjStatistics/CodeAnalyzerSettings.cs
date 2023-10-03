namespace ProjStatistics
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public class CodeAnalyzerSettings
    {
        public static readonly string SectionName = "SourceCodeAnalyzer";

        /// <summary>
        /// Стартовый каталог для выполнения анализа файлов с исходным кодом.
        /// </summary>
        public string TargetDirectory { get; set; }

        /// <summary>
        /// Подкаталоги, которые необходимо игнорировать в ходе анализа.
        /// </summary>
        public string IgnoredSubdirs { get; set; }

        /// <summary>
        /// Расширения файлов с исходным кодом.
        /// </summary>
        public string SourceFileExtensions { get; set; }

        /// <summary>
        /// Расширения минифицированных версий файлов.
        /// </summary>
        public string MinifiedVersions { get; set; }
    }
}