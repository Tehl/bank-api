using System.Linq;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories
{
    /// <summary>
    ///     Describes a type which can store and query BankAccount instances
    /// </summary>
    public interface IBankAccountRepository
    {
        /// <summary>
        ///     Creates a BankAccount for the specified user with the specified bank id and account number
        /// </summary>
        /// <param name="userId">Id of the user who owns the bank account</param>
        /// <param name="bankId">Bank id of the bank which holds the account</param>
        /// <param name="accountNumber">Account number of the account</param>
        /// <returns>The created BankAccount instance</returns>
        BankAccount CreateAccount(int userId, string bankId, string accountNumber);

        /// <summary>
        ///     Retrieves a BankAccount with the specified bank account id
        /// </summary>
        /// <param name="bankAccountId">Id of the BankAccount to be retrieved</param>
        /// <returns>The requested BankAccount if it exists, otherwise null</returns>
        BankAccount GetAccountById(int bankAccountId);

        /// <summary>
        ///     Retrieves a BankAccount with the specified bank id and account number
        /// </summary>
        /// <param name="bankId">Bank id of the bank which holds the account</param>
        /// <param name="accountNumber">Account number of the account</param>
        /// <returns>The requested BankAccount if it exists, otherwise null</returns>
        BankAccount GetAccountByBankIdAndAccountNumber(string bankId, string accountNumber);

        /// <summary>
        ///     Retrieves all BankAccount instances which are owned by the specified user
        /// </summary>
        /// <param name="userId">Id of the user who owns the bank accounts to be retrieved</param>
        /// <returns>IQueryable instance representing all BankAccount which belong the specified user</returns>
        IQueryable<BankAccount> GetAllAccountsByUserId(int userId);
    }
}