using System;
using System.IO;
using System.Linq;
using ProjStatistics.Models;
using ProjStatistics.Helpers;
using System.Collections.Generic;

namespace ProjStatistics.Services
{
    /// <summary>
    /// Сервис анализа файлов с исходным кодом.
    /// </summary>
    public class SourceCodeAnalyzer
    {
        /// <summary>
        /// Стартовый каталог для анализа.
        /// </summary>
        public string TargetDirectory { get; private set; }

        /// <summary>
        /// Все имена файлов.
        /// </summary>
        public List<string> AllFiles { get; private set; }

        /// <summary>
        /// Список файлов с исходным кодом.
        /// </summary>
        public List<ReadSourceFile> SourceFiles { get; private set; }

        /// <summary>
        /// Количество файлов по типам.
        /// </summary>

        private readonly Dictionary<string, int> filesGroups = new();

        /// <summary>
        /// Список игнорируемых при анализе каталогов.
        /// </summary>
        private List<string> ignoredSubdirs;

        /// <summary>
        /// Список расширений файлов с исходным кодом.
        /// </summary>
        private List<string> sourceFileExtensions;

        /// <summary>
        /// Расширения минифицированных файлов.
        /// </summary>
        private List<string> minifiedVersions;

        private static List<string> ExtrudePaths(string pathsList) => pathsList.Split(";").Where(x => x.Trim() != string.Empty).ToList();

        /// <summary>
        /// Получает все имена файлов из целевого каталога.
        /// </summary>
        private List<string> GetFilenamesList(string targetDir)
        {
            List<string> filenamesList = new();
            filenamesList.AddRange(Directory.GetFiles(targetDir).Select(x => x));
            var subdirs = Directory.GetDirectories(targetDir);

            foreach (string subdir in subdirs)
            {
                filenamesList.AddRange(GetFilenamesList(subdir));
            }

            return filenamesList;
        }

        /// <summary>
        /// Тест на минифицированную версию файла.
        /// </summary>
        private static bool IsMinifiedVersion(string filename, List<string> minifiedVersions)
        {
            foreach (var minifiedVer in minifiedVersions)
            {
                if (filename.Contains(minifiedVer))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Отбор файлов с исходным кодом.
        /// </summary>
        private List<ReadSourceFile> GetSourceFilesList(List<string> allFilenames, List<string> sourceFileExts, List<string> minifiedVersions)
        {
            List<ReadSourceFile> sourcesFiles = new();

            foreach (var filename in allFilenames)
            {
                var extension = Path.GetExtension(filename);
                if (sourceFileExts.Contains(extension) && !IgnorePath(Path.GetFullPath(filename)) && !IsMinifiedVersion(Path.GetFileName(filename), minifiedVersions))
                {
                    try
                    {
                        var content = File.ReadAllLines(filename);
                        sourcesFiles.Add(new(filename, content, filename.GetLastWriteTime()));
                        filesGroups[extension]++;
                    }
                    catch (IOException e)
                    {
                        throw new Exception($"Can't read from source file \"{filename}\". The analysis of source code has been interrupted.", e);
                    }
                }
            }

            return sourcesFiles;
        }

        /// <summary>
        /// Если подкаталог следует игнорировать, то возвращает true.
        /// </summary>
        private bool IgnorePath(string path)
        {
            foreach (var subdir in ignoredSubdirs)
            {
                if (path.Contains(subdir))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Все расширения файлов проекта, содержащие код.
        /// </summary>
        private string GetSourceFileExts()
        {
            var exts = "";
            foreach (var extension in sourceFileExtensions)
            {
                exts += (!string.IsNullOrEmpty(exts) ? "," : "") + extension;
            }
            return exts;
        }

        /// <summary>
        /// Возвращает общее количество строк в файлах с исходным кодом.
        /// </summary>
        private int GetTotalCodeLinesCount() => SourceFiles.Sum(x => x.GetCodeLinesCount());

        /// <summary>
        /// Возвращает подробную информацию о файлах с исходным кодом.
        /// </summary>
        private List<string> GetSourceFilesDetails() => SourceFiles.Select(x => x.GetReportRow()).ToList();

        private static SourceFileStatistics GetFileStatisticsByLinesCount(List<SourceFileStatistics> sourceFilesStatistics, int linesCount) =>
            sourceFilesStatistics.FirstOrDefault(x => x.AllLinesCount == linesCount);

        private static SourceFileStatistics GetFileStatisticsByAge(List<SourceFileStatistics> sourcesFileStatistics, int ageDays) =>
            sourcesFileStatistics.FirstOrDefault(x => x.AgeInDays == ageDays);

        /// <summary>
        /// Возвращает общую статистику по просмотренным каталогам.
        /// </summary>
        /// <returns></returns>
        private static FoldersStatistics GetFolderStatistics(List<SourceFileStatistics> sourcesFileStatistics)
        {
            var maxLinesCount = sourcesFileStatistics.Max(x => x.AllLinesCount);
            var minLinesCount = sourcesFileStatistics.Min(x => x.AllLinesCount);
            var maxCodeLinesCount = sourcesFileStatistics.Max(x => x.CodeLinesCount);
            var maxFileAge = sourcesFileStatistics.Max(x => x.AgeInDays);
            var minFileAge = sourcesFileStatistics.Min(x => x.AgeInDays);

            return new()
            {
                MaxLinesCount = maxCodeLinesCount,
                VerbosedFile = GetFileStatisticsByLinesCount(sourcesFileStatistics, maxLinesCount),
                MinLinesCount = minLinesCount,
                LaconicFile = GetFileStatisticsByLinesCount(sourcesFileStatistics, minLinesCount),
                PlumpestFile = sourcesFileStatistics.FirstOrDefault(x => x.CodeLinesCount == maxCodeLinesCount),
                MaxFileAge = maxFileAge,
                OldestFile = GetFileStatisticsByAge(sourcesFileStatistics, maxFileAge),
                MinFileAge = minFileAge,
                YoungestFile = GetFileStatisticsByAge(sourcesFileStatistics, minFileAge)
            };
        }

        /// <summary>
        /// Выполняет анализ исходный файлов.
        /// </summary>
        public List<string> Analyze(CodeAnalyzerSettings codeAnalyzerSettings, bool details)
        {
            if (!Directory.Exists(codeAnalyzerSettings.TargetDirectory))
            {
                throw new Exception($"Directory \"{codeAnalyzerSettings.TargetDirectory}\" doesn't exist. " +
                    $"Please, check application settings (parameter {nameof(codeAnalyzerSettings.TargetDirectory)}).");
            }

            TargetDirectory = codeAnalyzerSettings.TargetDirectory;
            ignoredSubdirs = ExtrudePaths(codeAnalyzerSettings.IgnoredSubdirs);
            sourceFileExtensions = ExtrudePaths(codeAnalyzerSettings.SourceFileExtensions);
            minifiedVersions = ExtrudePaths(codeAnalyzerSettings.MinifiedVersions);

            foreach (var ext in sourceFileExtensions)
            {
                filesGroups.Add(ext, 0);
            };

            AllFiles = GetFilenamesList(TargetDirectory);
            SourceFiles = GetSourceFilesList(AllFiles, sourceFileExtensions, minifiedVersions);
            var sourceFilesStatistics = SourceFiles.Select(x => new SourceFileStatistics(x)).ToList();
            var foldersStatistics = GetFolderStatistics(sourceFilesStatistics);

            List<string> reportRows = new();

            if (!details)
            {
                reportRows.AddRange(new List<string>()
                   {
                        $"Total count of code lines in {SourceFiles.Count.GetHighlightedText()} files (without comments and empty strings): {GetTotalCodeLinesCount().GetHighlightedText()}",
                        $"Analyzed target folder: \"{TargetDirectory.GetHighlightText()}\"",
                        $"Folder contains files: {AllFiles.Count.GetHighlightedText()}",
                        $"Folder contains files with source code ({GetSourceFileExts()}): {SourceFiles.Count.GetHighlightedText()}"
                    }
                    .Union(SourceFileStatistics.GetReportRows(sourceFilesStatistics))
                    .Union(filesGroups.Select(x => $"Files count with the extension \"{x.Key}\": {x.Value.GetHighlightedText()}")
                    .Union(foldersStatistics.GetReportRows()))
                );
            }
            else
            {
                reportRows.AddRange(GetSourceFilesDetails());
            }

            return reportRows;
        }
    }
}