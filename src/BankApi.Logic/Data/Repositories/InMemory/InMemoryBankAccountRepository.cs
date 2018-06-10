using System;
using System.Linq;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories.InMemory
{
    public class InMemoryBankAccountRepository : IBankAccountRepository
    {
        public BankAccount CreateAccount(int userId, string bankId, string accountNumber)
        {
            throw new NotImplementedException();
        }

        public BankAccount GetAccountById(int bankAccountId)
        {
            throw new NotImplementedException();
        }

        public BankAccount GetAccountByBankIdAndAccountNumber(string bankId, string accountNumber)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BankAccount> GetAllAccountsByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}