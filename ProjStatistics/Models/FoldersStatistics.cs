using ProjStatistics.Helpers;
using System.Collections.Generic;

namespace ProjStatistics.Models
{
    /// <summary>
    /// Общая статистика по всем каталогам, где имеются файлы с исходным кодом.
    /// </summary>
    public struct FoldersStatistics
    {
        /// <summary>
        /// Максимальное количество строк кода.
        /// </summary>
        public long MaxLinesCount { get; set; }

        /// <summary>
        /// Минимальное количество строк кода.
        /// </summary>
        public long MinLinesCount { get; set; }

        /// <summary>
        /// Максимальный возраст файла.
        /// </summary>
        public long MaxFileAge { get; set; }

        /// <summary>
        /// Минимальный возраст файла.
        /// </summary>
        public long MinFileAge { get; set; }

        /// <summary>
        /// Файл с самым большим количеством строк.
        /// </summary>
        public SourceFileStatistics VerbosedFile { get; set; }

        /// <summary>
        /// Файл с минимальным количеством строка кода.
        /// </summary>
        public SourceFileStatistics LaconicFile { get; set; }

        /// <summary>
        /// Файл с самым большим количеством строк кода.
        /// </summary>
        public SourceFileStatistics PlumpestFile { get; set; }

        /// <summary>
        /// Файл, который давно не изменялся.
        /// </summary>
        public SourceFileStatistics OldestFile { get; set; }

        /// <summary>
        /// Файл, который был изменен самым последним.
        /// </summary>
        public SourceFileStatistics YoungestFile { get; set; }

        public List<string> GetReportRows() => new()
        {
            $"File with min lines count: \"{VerbosedFile.Filename.GetHighlightText()} ({VerbosedFile.AllLinesCount.GetHighlightedText()})",
            $"File with maximum code lines count: \"{PlumpestFile.Filename.GetHighlightText()})\" ({PlumpestFile.CodeLinesCount.GetHighlightedText()})",
            $"File with minimum code lines count: \"{LaconicFile.Filename.GetHighlightText()}\" ({LaconicFile.AllLinesCount.GetHighlightedText()})",
            $"File hans't changed for a long time (age: {OldestFile.AgeInDays.DaysAgoString().GetHighlightText()}): " + OldestFile.Filename.GetHighlightText(),
            $"Recently modified file (age: {YoungestFile.AgeInDays.DaysAgoString().GetHighlightText()}): {YoungestFile.Filename.GetHighlightText()}"
        };
    }
}