using BankApi.Logic.BankConnections.Data;
using BankApi.Logic.Data.Models;
using BankApi.Server.Utilities;
using NUnit.Framework;

namespace BankApi.Tests.Server.Utilities
{
    /// <summary>
    ///     Contains tests for <see cref="ViewModelUtility" />
    /// </summary>
    [TestFixture]
    public class ViewModelUtilityTests
    {
        /// <summary>
        ///     Tests that CreateAccountDetailsViewModel generates a AccountDetailsViewModel containing the specified account
        ///     details
        /// </summary>
        [Test]
        public void CreateAccountDetailsViewModelReturnsAccountData()
        {
            const string accountNumber = "1234678";

            var bankAccount = new BankAccount {Id = 1, BankId = "TestBank", AccountNumber = accountNumber};
            var accountDetails = new AccountDetails
            {
                AccountNumber = accountNumber,
                AccountName = "Current Account",
                SortCode = "079046",
                CurrentBalance = 350,
                OverdraftLimit = 50
            };

            var model = ViewModelUtility.CreateAccountDetailsViewModel(bankAccount, accountDetails);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.AccountId, Is.EqualTo(bankAccount.Id));
            Assert.That(model.BankId, Is.EqualTo(bankAccount.BankId));
            Assert.That(model.AccountNumber, Is.EqualTo(bankAccount.AccountNumber));
            Assert.That(model.AccountName, Is.EqualTo(accountDetails.AccountName));
            Assert.That(model.SortCode, Is.EqualTo(accountDetails.SortCode));
            Assert.That(model.CurrentBalance, Is.EqualTo(accountDetails.CurrentBalance));
            Assert.That(model.OverdraftLimit, Is.EqualTo(accountDetails.OverdraftLimit));
        }

        /// <summary>
        ///     Tests that CreateAccountOverviewViewModel generates a AccountOverviewViewModel containing the specified account
        ///     details
        /// </summary>
        [Test]
        public void CreateAccountOverviewViewModelReturnsAccountData()
        {
            var bankAccount = new BankAccount {Id = 1, BankId = "TestBank", AccountNumber = "12345678"};

            var model = ViewModelUtility.CreateAccountOverviewViewModel(bankAccount);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.AccountId, Is.EqualTo(bankAccount.Id));
            Assert.That(model.BankId, Is.EqualTo(bankAccount.BankId));
            Assert.That(model.AccountNumber, Is.EqualTo(bankAccount.AccountNumber));
        }

        /// <summary>
        ///     Tests that CreateUserViewModel generates a UserViewModel containing the specified user details
        /// </summary>
        [Test]
        public void CreateUserViewModelReturnsUserData()
        {
            var user = new AppUser {Id = 1, Username = "User1"};

            var model = ViewModelUtility.CreateUserViewModel(user);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.UserId, Is.EqualTo(user.Id));
            Assert.That(model.Username, Is.EqualTo(user.Username));
        }
    }
}