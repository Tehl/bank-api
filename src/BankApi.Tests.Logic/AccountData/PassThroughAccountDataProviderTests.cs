using System.Threading.Tasks;
using BankApi.Logic.AccountData;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;
using NSubstitute;
using NUnit.Framework;

namespace BankApi.Tests.Logic.AccountData
{
    /// <summary>
    ///     Contains tests for <see cref="PassThroughAccountDataProvider" />
    /// </summary>
    [TestFixture]
    public class PassThroughAccountDataProviderTests
    {
        /// <summary>
        ///     Tests that GetAccountDetails uses the underlying IBankConnection directly
        /// </summary>
        [Test]
        public async Task GetAccountDetailsQueriesUnderlyingConnection()
        {
            const string bankId = "TestBank";
            const string accountNumber = "12345678";

            var accountDetails = new AccountDetails
            {
                AccountName = "Current Account",
                AccountNumber = accountNumber,
                SortCode = "01-02-03",
                CurrentBalance = 100,
                OverdraftLimit = 50
            };

            var connection = Substitute.For<IBankConnection>();
            connection.GetAccountDetails(accountNumber)
                .Returns(x =>
                    Task.FromResult(
                        new OperationResult<AccountDetails>(accountDetails)
                    )
                );

            var connectionManager = Substitute.For<IBankConnectionManager>();
            connectionManager.CreateConnection(bankId).Returns(connection);

            var accountDataProvider = new PassThroughAccountDataProvider(connectionManager);

            var accountResult = await accountDataProvider.GetAccountDetails(bankId, accountNumber);

            Assert.DoesNotThrow(() => connectionManager.Received().CreateConnection(bankId));
            Assert.DoesNotThrow(() => connection.Received().GetAccountDetails(accountNumber));
            Assert.That(accountResult.Success, Is.True);
            Assert.That(accountResult.Result, Is.EqualTo(accountDetails));
        }
    }
}