namespace ProjStatistics.Models
{
    /// <summary>
    /// Файл с исходным кодом.
    /// </summary>
    public class SourceFile
    {
        public string Filename { get; set; }

        public SourceFile(string filename) => Filename = filename;
    }
}