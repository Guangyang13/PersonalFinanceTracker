using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using System.Net;
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

            // This line bypasses SSL certificate validation for development purposes
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            // Ensure TLS 1.2 is being used
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServiceCollection serviceProvider = new ServiceCollection();
            ConfigureServices(serviceProvider);
            _services = serviceProvider.BuildServiceProvider();

            MainWindow mainWindow = _services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowVM>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IUserSessionService, UserSessionService>();

            services.AddTransient<ITransactionRepository, TransactionRepository>();

            services.AddHttpClient<IAuthService, AuthService>(options => options.BaseAddress = new Uri(config["ApiSettings:BaseUrl"] ?? "http://localhost:5263/"));
            services.AddHttpClient<INetworkService, NetworkService>(options => options.BaseAddress = new Uri(config["ApiSettings:BaseUrl"] ?? "http://localhost:5263/"));
            services.AddHttpClient<ITransactionService, TransactionService>(options => options.BaseAddress = new Uri(config["ApiSettings:BaseUrl"] ?? "http://localhost:5263/"));

            services.AddTransient<LoginVM>();
            services.AddTransient<RegistrationVM>();
            services.AddTransient<TransactionFormVM>();

        }
    }

}
