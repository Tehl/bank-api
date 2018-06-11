using System;
using System.Collections.Generic;
using System.Linq;

namespace BankApi.Logic.BankConnections
{
    /// <summary>
    ///     Creates connections to remote banking services using a list of registered connection providers
    /// </summary>
    public class BankConnectionManager : IBankConnectionManager
    {
        private readonly Dictionary<string, IBankConnectionProvider> _connectionProviders;

        /// <summary>
        ///     Initializes the BankConnectionManager
        /// </summary>
        public BankConnectionManager()
        {
            _connectionProviders = new Dictionary<string, IBankConnectionProvider>();
        }

        /// <summary>
        ///     Gets a list of all registered connection providers by bank id
        /// </summary>
        /// <returns>List of bank ids exposed by the registered connection providers</returns>
        public List<string> GetRegisteredBankIds()
        {
            return _connectionProviders.Keys.ToList();
        }

        /// <summary>
        ///     Creates a connection to the specified banking service
        /// </summary>
        /// <param name="bankId">Id of the banking service to connect to</param>
        /// <returns>An IBankConnection implementation providing access to the specified banking service</returns>
        public IBankConnection CreateConnection(string bankId)
        {
            if (!_connectionProviders.ContainsKey(bankId))
                throw new InvalidOperationException($"Connection provider for BankId {bankId} does not exist");

            return _connectionProviders[bankId].CreateConnection();
        }

        /// <summary>
        ///     Adds the specified connection provider to the list of available connections
        /// </summary>
        /// <param name="provider">Bank connection provider to be added to the list of available connections</param>
        public void RegisterConnectionProvider(IBankConnectionProvider provider)
        {
            var bankId = provider.BankId;

            if (_connectionProviders.ContainsKey(bankId))
                throw new ArgumentException(
                    $"Connection provider for BankId {bankId} already exists",
                    nameof(provider)
                );

            _connectionProviders[bankId] = provider;
        }
    }
}