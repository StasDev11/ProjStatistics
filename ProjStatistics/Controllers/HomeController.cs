using System;
using ProjStatistics.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using ProjStatistics.Models.ViewModels;

namespace ProjStatistics.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(SourceCodeAnalyzer sourceCodeAnalyzer, IOptions<CodeAnalyzerSettings> appOptions) :
            base()
        {
            this.appOptions = appOptions;
            this.sourceCodeAnalyzer = sourceCodeAnalyzer;
        }

        private readonly IOptions<CodeAnalyzerSettings> appOptions;

        private readonly SourceCodeAnalyzer sourceCodeAnalyzer;

        public CodeAnalyzerSettings AppSettings => appOptions?.Value;

        /// <summary>
        /// Страница с общей статистикой по файлам с исходным кодом.
        /// </summary>
        [HttpGet]
        public ActionResult Statistics() => View(new StatisticsViewModel(AppSettings.TargetDirectory));

        private List<string> AnalyzeSourceCode(bool details) => sourceCodeAnalyzer.Analyze(AppSettings, details);

        /// <summary>
        /// Возвращает журнал анализа файлов с исходным кодом.
        /// </summary>
        [HttpPost]
        public ActionResult<List<string>> GetSourcesStatistics()
        {
            try
            {
                return Ok(AnalyzeSourceCode(false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Страница с детальной статистикой по файлам с исходным кодом.
        /// </summary>
        [HttpPost]
        public ActionResult DetailedStatistics()
        {
            try
            {
                var fileDetails = AnalyzeSourceCode(true);
                return View(new StatisticsViewModel(AppSettings.TargetDirectory, fileDetails));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}