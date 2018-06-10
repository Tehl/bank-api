using System.Threading.Tasks;
using BankApi.Logic.BankConnections.Data;

namespace BankApi.Logic.BankConnections
{
    /// <summary>
    ///     Describes a type which can retrieve account information from a remote banking system
    /// </summary>
    public interface IBankConnection
    {
        /// <summary>
        ///     Gets account details for the specified account number
        /// </summary>
        /// <param name="accountNumber">Account number to retrieve account details for</param>
        /// <returns>OperationResult instance describing the outcome of the remote query</returns>
        Task<OperationResult<AccountDetails>> GetAccountDetails(string accountNumber);
    }
}