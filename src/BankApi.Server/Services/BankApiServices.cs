using BankApi.Connections.BizfiBank;
using BankApi.Logic.AccountData;
using BankApi.Logic.BankConnections;
using BankApi.Logic.Data.Repositories;
using BankApi.Logic.Data.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace BankApi.Server.Services
{
    /// <summary>
    ///     Sets up dependency injection for the application
    /// </summary>
    public static class BankApiServices
    {
        /// <summary>
        ///     Adds services which fulfil the various data repository contracts for the application
        /// </summary>
        public static IServiceCollection AddAppDataRepositories(this IServiceCollection services)
        {
            var userRepository = new InMemoryUserRepository();
            var accountRepository = new InMemoryBankAccountRepository();

            return services
                .AddSingleton<IUserRepository>(userRepository)
                .AddSingleton<IBankAccountRepository>(accountRepository);
        }

        /// <summary>
        ///     Adds the IBankConnectionManager service with a list of registered connection providers for the application
        /// </summary>
        public static IServiceCollection AddBankConnections(this IServiceCollection services)
        {
            var connectionManager = new BankConnectionManager();

            connectionManager.RegisterConnectionProvider(new BizfiBankConnectionProvider());

            return services
                .AddSingleton<IBankConnectionManager>(connectionManager);
        }

        /// <summary>
        ///     Adds the IAccountDataProvider for the application
        /// </summary>
        public static IServiceCollection AddAccountDataProvider(this IServiceCollection services)
        {
            return services
                .AddTransient<IAccountDataProvider, PassThroughAccountDataProvider>();
        }
    }
}