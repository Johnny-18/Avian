using System;
using System.IO;
using System.Text;
using System.Windows;
using Application.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Avian
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private IServiceProvider ServiceProvider { get; set; }

        private IConfigurationRoot Configuration { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Configuration = configuration;

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            
            using (var scope = ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AvianContext>();
                db.Database.Migrate();
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<AvianContext>(builder =>
            {
                var connectionString = Configuration.GetConnectionString("Avian");
                builder.UseNpgsql(connectionString);
            });

            serviceCollection.AddSingleton<MainWindow>();
        }
    }
}