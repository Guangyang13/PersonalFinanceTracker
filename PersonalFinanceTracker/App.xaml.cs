using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Interfaces.Auth;
using PersonalFinanceTracker.Interfaces.Infrastructure;
using PersonalFinanceTracker.Interfaces.Navigation;
using PersonalFinanceTracker.Interfaces.Repository;
using PersonalFinanceTracker.Interfaces.Transactions;
using PersonalFinanceTracker.Repositories;
using PersonalFinanceTracker.Services.Auth;
using PersonalFinanceTracker.Services.Infrastructure;
using PersonalFinanceTracker.Services.Navigation;
using PersonalFinanceTracker.Services.Transactions;
using PersonalFinanceTracker.ViewModels;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using static System.Formats.Asn1.AsnWriter;

namespace PersonalFinanceTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _services = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceCollection serviceProvider = new ServiceCollection();
            ConfigureServices(serviceProvider);
            _services = serviceProvider.BuildServiceProvider();

            MainWindow mainWindow = _services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowVM>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserSessionService, UserSessionService>();

            services.AddTransient<ITransactionRepository, TransactionRepository>();

            services.AddHttpClient<IAuthService, AuthService>(options => options.BaseAddress = new Uri("http://localhost:5263/"));
            services.AddHttpClient<INetworkService, NetworkService>(options => options.BaseAddress = new Uri("http://localhost:5263/"));
            services.AddHttpClient<ITransactionService, TransactionService>(options => options.BaseAddress = new Uri("http://localhost:5263/"));

            services.AddTransient<LoginVM>();
            services.AddTransient<RegistrationVM>();
            services.AddTransient<TransactionFormVM>();


            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //services.AddDbContextFactory<AppDbContext>
            //    (options => options.UseSqlServer(config.GetConnectionString("Default")));
        }
    }

}
