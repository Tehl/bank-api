using System.Net;
using System.Threading.Tasks;
using BankApi.Logic.AccountData;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;
using BankApi.Logic.Data.Models;
using BankApi.Logic.Data.Repositories;
using BankApi.Server.Controllers;
using BankApi.Server.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BankApi.Tests.Server.Controllers
{
    /// <summary>
    ///     Contains tests for <see cref="AccountsApiController" />
    /// </summary>
    [TestFixture]
    public class AccountsApiTests
    {
        /// <summary>
        ///     Tests that ApiV1AccountsGetById will pass forward any error which is returned by the account data provider
        /// </summary>
        [Test]
        public async Task ApiV1AccountsGetByIdForwardsErrorFromAccountProvider()
        {
            const int accountId = 1;
            const string bankId = "TestBank";
            const string accountNumber = "00112233";

            const long errorCode = 1001123;

            var accountRepository = Substitute.For<IBankAccountRepository>();
            accountRepository.GetAccountById(accountId)
                .Returns(new BankAccount {BankId = bankId, AccountNumber = accountNumber});

            // GetAccountDetails should return a not found result even though we know this account should exist
            var accountDataProvider = Substitute.For<IAccountDataProvider>();
            accountDataProvider.GetAccountDetails(bankId, accountNumber).Returns(x =>
                Task.FromResult(
                    new OperationResult<AccountDetails>(
                        (int) HttpStatusCode.NotFound,
                        new OperationError(
                            errorCode,
                            $"Account number '{accountNumber}' does not exist"
                        )
                    )
                )
            );

            var controller = new AccountsApiController(accountRepository, accountDataProvider);

            var actionResult = await controller.ApiV1AccountsGetById(accountId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));

            var errorViewModel = contentResult.Value as ErrorViewModel;

            Assert.That(errorViewModel, Is.Not.Null);
            Assert.That(errorViewModel.ErrorCode, Is.EqualTo(errorCode));
        }

        /// <summary>
        ///     Tests that ApiV1AccountsGetById returns 400 when an account id is not supplied
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 400
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public async Task ApiV1AccountsGetByIdHandlesAccountIdNotSupplied()
        {
            var controller = new AccountsApiController(null, null);

            var actionResult = await controller.ApiV1AccountsGetById(null);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var accountResult = contentResult.Value as ErrorViewModel;

            Assert.That(accountResult, Is.Not.Null);
            Assert.That(accountResult.Status, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        /// <summary>
        ///     Tests that ApiV1AccountsGetById returns 404 when the requested account does not exist
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 404
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public async Task ApiV1AccountsGetByIdHandlesAccountNotFound()
        {
            const int requestedAccountId = 1;

            var accountRepository = Substitute.For<IBankAccountRepository>();
            accountRepository.GetAccountById(0).ReturnsForAnyArgs((BankAccount) null);

            var controller = new AccountsApiController(accountRepository, null);

            var actionResult = await controller.ApiV1AccountsGetById(requestedAccountId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));

            var accountResult = contentResult.Value as ErrorViewModel;

            Assert.That(accountResult, Is.Not.Null);
            Assert.That(accountResult.Status, Is.EqualTo((int) HttpStatusCode.NotFound));
        }

        /// <summary>
        ///     Tests that ApiV1AccountsGetById can locate a known bank account by id
        /// </summary>
        [Test]
        public async Task ApiV1AccountsGetByIdReturnsAccountDetailsById()
        {
            const int accountId = 1;
            const string bankId = "TestBank";
            const string accountNumber = "00112233";

            var remoteAccountDetails = new AccountDetails
            {
                AccountNumber = accountNumber,
                AccountName = "Current Account",
                SortCode = "079046",
                CurrentBalance = 350,
                OverdraftLimit = 50
            };

            var accountRepository = Substitute.For<IBankAccountRepository>();
            accountRepository.GetAccountById(accountId)
                .Returns(new BankAccount {BankId = bankId, AccountNumber = accountNumber});

            var accountDataProvider = Substitute.For<IAccountDataProvider>();
            accountDataProvider.GetAccountDetails(bankId, accountNumber).Returns(x =>
                Task.FromResult(
                    new OperationResult<AccountDetails>(remoteAccountDetails)
                )
            );

            var controller = new AccountsApiController(accountRepository, accountDataProvider);

            var actionResult = await controller.ApiV1AccountsGetById(accountId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            var accountResult = contentResult.Value as AccountDetailsViewModel;

            Assert.That(accountResult, Is.Not.Null);
            Assert.That(accountResult.BankId, Is.EqualTo(bankId));
            Assert.That(accountResult.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(accountResult.AccountName, Is.EqualTo(remoteAccountDetails.AccountName));
            Assert.That(accountResult.SortCode, Is.EqualTo(remoteAccountDetails.SortCode));
            Assert.That(accountResult.CurrentBalance, Is.EqualTo(remoteAccountDetails.CurrentBalance));
            Assert.That(accountResult.OverdraftLimit, Is.EqualTo(remoteAccountDetails.OverdraftLimit));
        }
    }
}