using System.Collections.Generic;

namespace ProjStatistics.Models.ViewModels
{
    public class StatisticsViewModel
    {
        public string TargetDirectory { get; private set; }

        public List<string> TableRows { get; private set; }

        public StatisticsViewModel(string targetDirectory) => TargetDirectory = targetDirectory;

        public StatisticsViewModel(string targetDirectory, List<string> tableRows) : this(targetDirectory) => TableRows = tableRows;
    }
}