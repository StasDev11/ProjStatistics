using ProjStatistics.Models;
using System.IO;

namespace ProjStatistics.Helpers
{
    /// <summary>
    /// Внешний вида отчета.
    /// </summary>
    public static class ReportAppearance
    {
        public static string GetHighlightText(this string text) => $"<strong class='text-primary'>{text}</strong>";

        public static string GetHighlightedText(this int numeric) => GetHighlightText(numeric.ToString());

        public static string GetReportRow(this ReadSourceFile readSourceFile) =>
            $"<td>{Path.GetDirectoryName(readSourceFile.Filename)}\\{Path.GetFileName(readSourceFile.Filename).GetHighlightText()}</td>" +
            $"<td>{readSourceFile.GetCodeLinesCount()}</td><td>{readSourceFile.Filename.GetFileSizeString()}</td>";
    }
}