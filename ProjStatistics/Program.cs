using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ProjStatistics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var hostBuilder = CreateHostBuilder(args).Build();
                hostBuilder.Run();
            }
            catch (Exception e)
            {
                throw new Exception("Can't run web application: ", e);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Определение стартового класса
                    webBuilder.UseStartup<Startup>();
                });
    }
}