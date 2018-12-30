using System;
using System.IO;
using FluentMigrator.Runner;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tuszcom.chat.Data;
using tuszcom.chat.Models;
using tuszcom.migrations.Migrations;
using tuszcom.models;

namespace tuszcom.chat
{
    public class Program
    {
        protected static IConfigurationRoot config;

        public static void Main(string[] args)
        {
            config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false)
                 .AddEnvironmentVariables()
                 .AddCommandLine(args)
                 .Build();

            var host = CreateWebHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    InitializeDb initializeDb = new InitializeDb();
                    initializeDb.Initialize(context, userManager, roleManager).Wait();

                    var serviceProvider = CreateService();

                    using (var migrationScope = serviceProvider.CreateScope())
                    {
                        UpdateDatabase(migrationScope.ServiceProvider);
                    }

                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex);
                }
            }
           
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                  .UseIISIntegration()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .CaptureStartupErrors(false)
            .UseKestrel()
                .UseStartup<Startup>();

        public static IServiceProvider CreateService()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer2016()                
                    .WithGlobalConnectionString(config.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(_20181227_221500_InitDatabase).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole())
                    .BuildServiceProvider(false);
        }
        public static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runnerMigrations = serviceProvider.GetRequiredService<IMigrationRunner>();
            runnerMigrations.MigrateUp();
        }
    }
    
}
