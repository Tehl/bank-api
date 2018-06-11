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
    }
}