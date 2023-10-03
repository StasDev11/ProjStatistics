using ProjStatistics.Models;
using ProjStatistics.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjStatistics
{
    /// <summary>
    /// Обработка логики входящих запросов.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Настройка сервисов приложения.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавление сервисов MVC
            services.AddMvc();
            // Настройка MVC для страниц
            services.AddRazorPages();
            // Добавление служб для контроллеров с вызовом
            services.AddControllersWithViews();
            services.RegisterServices(Configuration);
        }

        /// <summary>
        /// Настройка конвейера обработки запросов.
        /// </summary>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Добавление возможности предоставления статических файлов
            app.UseStaticFiles();
            // Добавление возможности маршрутизации
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Home}/{action=" + nameof(HomeController.Statistics) + "}");
                endpoints.MapRazorPages();
            });
        }
    }
}
