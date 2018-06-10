using System;
using System.Linq;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories.InMemory
{
    /// <summary>
    ///     Implements IBankAccountRepository using an in-memory data set
    /// </summary>
    public class InMemoryBankAccountRepository : IBankAccountRepository
    {
        private readonly InMemoryDbSet<BankAccount> _dbSet = new InMemoryDbSet<BankAccount>();

        /// <summary>
        ///     Creates a BankAccount for the specified user with the specified bank id and account number
        /// </summary>
        /// <param name="userId">Id of the user who owns the bank account</param>
        /// <param name="bankId">Bank id of the bank which holds the account</param>
        /// <param name="accountNumber">Account number of the account</param>
        /// <returns>The created BankAccount instance</returns>
        public BankAccount CreateAccount(int userId, string bankId, string accountNumber)
        {
            var existingAccount = GetAccountByBankIdAndAccountNumber(bankId, accountNumber);
            if (existingAccount != null)
                throw new InvalidOperationException(
                    $"BankAccount with bankId {bankId} and accountNumber {accountNumber} already exists"
                );

            var newAccount = new BankAccount
            {
                UserId = userId,
                BankId = bankId,
                AccountNumber = accountNumber
            };

            return _dbSet.Add(newAccount);
        }

        /// <summary>
        ///     Retrieves a BankAccount with the specified bank account id
        /// </summary>
        /// <param name="bankAccountId">Id of the BankAccount to be retrieved</param>
        /// <returns>The requested BankAccount if it exists, otherwise null</returns>
        public BankAccount GetAccountById(int bankAccountId)
        {
            return _dbSet.Query().FirstOrDefault(o => o.Id == bankAccountId);
        }

        /// <summary>
        ///     Retrieves a BankAccount with the specified bank id and account number
        /// </summary>
        /// <param name="bankId">Bank id of the bank which holds the account</param>
        /// <param name="accountNumber">Account number of the account</param>
        /// <returns>The requested BankAccount if it exists, otherwise null</returns>
        public BankAccount GetAccountByBankIdAndAccountNumber(string bankId, string accountNumber)
        {
            return _dbSet.Query().FirstOrDefault(o => o.BankId == bankId && o.AccountNumber == accountNumber);
        }

        /// <summary>
        ///     Retrieves all BankAccount instances which are owned by the specified user
        /// </summary>
        /// <param name="userId">Id of the user who owns the bank accounts to be retrieved</param>
        /// <returns>IQueryable instance representing all BankAccount which belong the specified user</returns>
        public IQueryable<BankAccount> GetAllAccountsByUserId(int userId)
        {
            return _dbSet.Query().Where(o => o.UserId == userId);
        }
    }
}