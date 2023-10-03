using System;
using System.Linq;
using ProjStatistics.Helpers;
using System.Collections.Generic;

namespace ProjStatistics.Models
{
    /// <summary>
    /// Статистика файла с исходным кодом.
    /// </summary>
    public class SourceFileStatistics : SourceFile
    {
        /// <summary>
        /// Количество всех строк в файле.
        /// </summary>
        public int AllLinesCount { get; set; }

        /// <summary>
        /// Количество строк кода.
        /// </summary>
        public int CodeLinesCount { get; set; }

        /// <summary>
        /// Возраст в днях с момента изменения.
        /// </summary>
        public int AgeInDays { get; set; }

        /// <summary>
        /// Размер файла в строковом представлении.
        /// </summary>
        public string SizeInWords { get; set; }

        public SourceFileStatistics(ReadSourceFile sourceFile) :
            base(sourceFile.Filename)
        {
            AllLinesCount = sourceFile.Content.Length;
            CodeLinesCount = sourceFile.GetCodeLinesCount();
            AgeInDays = (int)(DateTime.Now - sourceFile.Modified).TotalDays;
            SizeInWords = sourceFile.Filename.GetFileSizeString();
        }

        public static List<string> GetReportRows(List<SourceFileStatistics> sourceFilesStatistics) => new()
        {
            $"Average count of lines in a file: {sourceFilesStatistics.Average(x => x.AllLinesCount).GetFormattedString().GetHighlightText()}",
            $"Average count of code lines in a file: {sourceFilesStatistics.Average(x => x.CodeLinesCount).GetFormattedString().GetHighlightText()}"
        };
    }
}