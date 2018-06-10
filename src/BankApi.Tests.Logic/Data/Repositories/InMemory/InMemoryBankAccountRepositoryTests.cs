using System;
using System.Linq;
using BankApi.Logic.Data.Repositories.InMemory;
using NUnit.Framework;

namespace BankApi.Tests.Logic.Data.Repositories.InMemory
{
    /// <summary>
    ///     Contains tests for <see cref="InMemoryBankAccountRepository" />
    /// </summary>
    [TestFixture]
    public class InMemoryBankAccountRepositoryTests
    {
        /// <summary>
        ///     Tests that CreateAccount can add an account to the store
        /// </summary>
        [Test]
        public void CreateAccountCanAddAccount()
        {
            const int userId = 1;
            const string bankId = "TestBank";
            const string accountNumber = "12346578";

            var repository = new InMemoryBankAccountRepository();

            var createdAccount = repository.CreateAccount(userId, bankId, accountNumber);

            Assert.That(createdAccount, Is.Not.Null);
            Assert.That(createdAccount.Id, Is.GreaterThan(0));
            Assert.That(createdAccount.UserId, Is.EqualTo(userId));
            Assert.That(createdAccount.BankId, Is.EqualTo(bankId));
            Assert.That(createdAccount.AccountNumber, Is.EqualTo(accountNumber));
        }

        /// <summary>
        ///     Tests that accounts added via CreateAccount are assigned unique database ids
        /// </summary>
        [Test]
        public void CreateAccountGeneratesUniqueIds()
        {
            const int userId = 1;
            const string bankId = "TestBank";
            const string accountNumber1 = "12346578";
            const string accountNumber2 = "12346579";

            var repository = new InMemoryBankAccountRepository();

            var createdAccount1 = repository.CreateAccount(userId, bankId, accountNumber1);
            var createdAccount2 = repository.CreateAccount(userId, bankId, accountNumber2);

            Assert.That(createdAccount1.Id, Is.Not.EqualTo(createdAccount2.Id));
        }

        /// <summary>
        ///     Tests that CreateAccount throws an InvalidOperationException if an account with the same bankId and accountNumber
        ///     already exists
        /// </summary>
        /// <remarks>
        ///     The repository should accept
        ///     * the same account number from different banks
        ///     * different accounts at the same bank
        ///     It should not accept
        ///     * the same account number at the same bank
        /// </remarks>
        [Test]
        public void CreateAccountRejectsDuplicateAccountDetails()
        {
            const int userId = 1;
            const string bankId1 = "TestBank1";
            const string bankId2 = "TestBank2";
            const string accountNumber1 = "12346578";
            const string accountNumber2 = "12346579";

            var repository = new InMemoryBankAccountRepository();

            repository.CreateAccount(userId, bankId1, accountNumber1);

            Assert.DoesNotThrow(
                () => repository.CreateAccount(userId, bankId1, accountNumber2)
            );

            Assert.DoesNotThrow(
                () => repository.CreateAccount(userId, bankId2, accountNumber1)
            );

            Assert.Throws<InvalidOperationException>(
                () => repository.CreateAccount(userId, bankId1, accountNumber1)
            );
        }

        /// <summary>
        ///     Tests that accounts can be located by bank id and account number
        /// </summary>
        [Test]
        public void GetAccountByBankIdAndAccountNumberCanLocateAccount()
        {
            const int userId = 1;
            const string bankId = "TestBank";
            const string accountNumber = "12346578";

            var repository = new InMemoryBankAccountRepository();

            var createdAccount = repository.CreateAccount(userId, bankId, accountNumber);
            var locatedAccount = repository.GetAccountByBankIdAndAccountNumber(bankId, accountNumber);

            Assert.That(createdAccount, Is.EqualTo(locatedAccount));
        }

        /// <summary>
        ///     Tests that requesting an account with an unknown bank id or account number returns null
        /// </summary>
        [Test]
        public void GetAccountByBankIdAndAccountNumberHandleAccountNotFound()
        {
            const string requestedBankId = "TestBank";
            const string requestedAccountNumber = "12346578";

            var repository = new InMemoryBankAccountRepository();

            var locatedAccount = repository.GetAccountByBankIdAndAccountNumber(requestedBankId, requestedAccountNumber);

            Assert.That(locatedAccount, Is.Null);
        }

        /// <summary>
        ///     Tests that accounts can be located by id
        /// </summary>
        [Test]
        public void GetAccountByIdCanLocateAccount()
        {
            const int userId = 1;
            const string bankId = "TestBank";
            const string accountNumber = "12346578";

            var repository = new InMemoryBankAccountRepository();

            var createdAccount = repository.CreateAccount(userId, bankId, accountNumber);
            var locatedAccount = repository.GetAccountById(createdAccount.Id);

            Assert.That(createdAccount, Is.EqualTo(locatedAccount));
        }

        /// <summary>
        ///     Tests that requesting an account with an unknown id returns null
        /// </summary>
        [Test]
        public void GetAccountByIdHandlesAccountNotFound()
        {
            const int requestedAccountId = 1;

            var repository = new InMemoryBankAccountRepository();

            var locatedAccount = repository.GetAccountById(requestedAccountId);

            Assert.That(locatedAccount, Is.Null);
        }

        /// <summary>
        ///     Tests that GetAllAccountsByUserId returns all accounts contained in the store which belong to the specified user,
        ///     but not any other accounts
        /// </summary>
        [Test]
        public void GetAllAccountsByUserIdReturnsAllCreatedAccounts()
        {
            const int userId1 = 1;
            const int userId2 = 2;
            const string bankId = "TestBank";
            const string accountNumber1 = "12346578";
            const string accountNumber2 = "12346579";
            const string accountNumber3 = "12346570";

            var repository = new InMemoryBankAccountRepository();

            var createdAccount1 = repository.CreateAccount(userId1, bankId, accountNumber1);
            var createdAccount2 = repository.CreateAccount(userId1, bankId, accountNumber2);
            var createdAccount3 = repository.CreateAccount(userId2, bankId, accountNumber3);

            var allAccounts = repository.GetAllAccountsByUserId(userId1).ToList();

            Assert.That(allAccounts.Count, Is.EqualTo(2));
            Assert.That(allAccounts, Contains.Item(createdAccount1));
            Assert.That(allAccounts, Contains.Item(createdAccount2));
            Assert.That(allAccounts, Does.Not.Contain(createdAccount3));
        }
    }
}