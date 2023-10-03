using ProjStatistics.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjStatistics.Models
{
    /// <summary>
    /// Регистрация конфигураций и сервисов в качестве служб.
    /// </summary>
    public static class ServicesExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.Configure<CodeAnalyzerSettings>(configuration.GetSection(CodeAnalyzerSettings.SectionName));
            _ = services.AddTransient<SourceCodeAnalyzer>();
        }
    }
}