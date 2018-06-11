using System.Threading.Tasks;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;

namespace BankApi.Logic.AccountData
{
    /// <summary>
    ///     Describes a type which can fetch data for bank accounts
    /// </summary>
    public interface IAccountDataProvider
    {
        /// <summary>
        ///     Gets account details for the specified account number
        /// </summary>
        /// <param name="bankId">Id of the bank which operates the specified account</param>
        /// <param name="accountNumber">Account number to retrieve account details for</param>
        /// <returns>OperationResult instance describing the outcome of the query</returns>
        Task<OperationResult<AccountDetails>> GetAccountDetails(string bankId, string accountNumber);
    }
}