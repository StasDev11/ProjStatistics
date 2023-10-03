using System;
using System.Linq;
using ProjStatistics.Helpers;

namespace ProjStatistics.Models
{
    /// <summary>
    /// Прочтенный файл с исходным кодом.
    /// </summary>
    public class ReadSourceFile: SourceFile
    {
        /// <summary>
        /// Строки содержимого.
        /// </summary>
        public string[] Content { get; set; }

        /// <summary>
        /// Дата и время изменения.
        /// </summary>
        public DateTime Modified { get; set; }

        public static readonly string SingleLineComments = "//";

        public static readonly string MultiLineCommentBegins = "/*";

        public static readonly string MultiLineCommentEnds = "*/";

        public ReadSourceFile(string filename) : base(filename) { }

        public ReadSourceFile(string filename, string[] content, DateTime modified) : this(filename)
        {
            Content = content;
            Modified = modified;
        }

        /// <summary>
        /// Возвращает количество строк кода.
        /// </summary>
        public int GetCodeLinesCount()
        {
            for (int i = 0; i < Content.Length; i++)
            {
                var line = Content[i].Trim();
                if (line.Length > 1)
                {
                    // Исключение комментариев.
                    var firstChar = line.Substring(0, 1);
                    if (firstChar == "*")
                    {
                        line = "";
                    }
                    else
                    {
                        var subStr = line.Substring(0, 2);
                        if (subStr == SingleLineComments || subStr == MultiLineCommentBegins || subStr == MultiLineCommentEnds)
                        {
                            line = "";
                        }
                    }
                }
                Content[i] = line;
            }

            return Content.Count(x => !string.IsNullOrEmpty(x));
        }
    }
}