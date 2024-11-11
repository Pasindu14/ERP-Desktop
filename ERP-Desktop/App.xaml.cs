using ERP_Desktop.DBModels;
using ERP_Desktop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ERP_Desktop
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ERPDesktopContext>(options =>
                options.UseSqlServer("Server=DESKTOP-8A8HHP7;Database=ERP-Desktop;Trusted_Connection=True;TrustServerCertificate=True;"));
            services.AddSingleton<CategoryService>();
        }


        public ServiceProvider ServiceProvider => _serviceProvider;
    }
}
