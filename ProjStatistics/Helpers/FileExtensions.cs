using System;
using System.IO;

namespace ProjStatistics.Helpers
{
    public static class FileExtensions
    {
        /// <summary>
        /// Возвращает строку размера файла.
        /// </summary>
        public static string GetFileSizeString(this string filename)
        {
            var fileSize = new FileInfo(filename).Length;
            long sizeInKbs = fileSize / 1024;
            return sizeInKbs == 0 ? $"{fileSize} bytes" : $"{sizeInKbs} Kb";
        }

        /// <summary>
        /// Возвращает дату и время последнего изменения файла.
        /// </summary>
        public static DateTime GetLastWriteTime(this string filename) => File.GetLastWriteTime(filename);
    }
}