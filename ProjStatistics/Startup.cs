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
    /// ��������� ������ �������� ��������.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ��������� �������� ����������.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // ���������� �������� MVC
            services.AddMvc();
            // ��������� MVC ��� �������
            services.AddRazorPages();
            // ���������� ����� ��� ������������ � �������
            services.AddControllersWithViews();
            services.RegisterServices(Configuration);
        }

        /// <summary>
        /// ��������� ��������� ��������� ��������.
        /// </summary>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ���������� ����������� �������������� ����������� ������
            app.UseStaticFiles();
            // ���������� ����������� �������������
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
