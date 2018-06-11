using System.Collections.Generic;

namespace BankApi.Logic.BankConnections
{
    /// <summary>
    ///     Describes a type which creates connections to remote banking services using a list of registered connection
    ///     providers
    /// </summary>
    public interface IBankConnectionManager
    {
        /// <summary>
        ///     Gets a list of all registered connection providers by bank id
        /// </summary>
        /// <returns>List of bank ids exposed by the registered connection providers</returns>
        List<string> GetRegisteredBankIds();

        /// <summary>
        ///     Creates a connection to the specified banking service
        /// </summary>
        /// <param name="bankId">Id of the banking service to connect to</param>
        /// <returns>An IBankConnection implementation providing access to the specified banking service</returns>
        IBankConnection CreateConnection(string bankId);
    }
}