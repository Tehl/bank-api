using System.Threading.Tasks;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;

namespace BankApi.Logic.AccountData
{
    /// <summary>
    ///     Implements IAccountDataProvider by passing queries directly to the remote connection manager
    /// </summary>
    /// <remarks>
    ///     This implementation passes all queries directly to the remote service, via IBankConnectionManager.
    ///     Future implementations could query a local cache or SQL database, or provide transaction categorisation
    ///     as part of the fetch operation.
    /// </remarks>
    public class PassThroughAccountDataProvider : IAccountDataProvider
    {
        private readonly IBankConnectionManager _connectionManager;

        /// <summary>
        ///     Initializes the PassThroughAccountDataProvider
        /// </summary>
        /// <param name="connectionManager">BankConnectionManager used to connect to remote banking services</param>
        public PassThroughAccountDataProvider(IBankConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        /// <summary>
        ///     Gets account details for the specified account number
        /// </summary>
        /// <param name="bankId">Id of the bank which operates the specified account</param>
        /// <param name="accountNumber">Account number to retrieve account details for</param>
        /// <returns>OperationResult instance describing the outcome of the query</returns>
        public Task<OperationResult<AccountDetails>> GetAccountDetails(string bankId, string accountNumber)
        {
            var connection = _connectionManager.CreateConnection(bankId);

            return connection.GetAccountDetails(accountNumber);
        }
    }
}