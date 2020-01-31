using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Data;
using Inlmämningsuppgift2.Models.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inlmämningsuppgift2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await EnsureAdminInDatabase(host);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task EnsureAdminInDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();

                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await context.Database.EnsureCreatedAsync();

                if (!context.Users.Any())
                {
                    await userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = configuration.GetSection("Admin").GetSection("Username").Value
                    }, configuration.GetSection("Admin").GetSection("Password").Value);
                }
            }
        }
    }
}
