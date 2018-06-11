using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    ///     Contains tests for <see cref="UserAccountsApiController" />
    /// </summary>
    [TestFixture]
    public class UserAccountsApiTests
    {
        /// <summary>
        ///     Tests that ApiV1UsersGetAccountsByUserId returns 404 when the specified user does not exist
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 404
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetAccountsByUserIdHandlesUserNotFound()
        {
            const int requestedUserId = 1;

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetUserById(0).ReturnsForAnyArgs((AppUser) null);

            var controller = new UserAccountsApiController(userRepository, null);

            var actionResult = controller.ApiV1UsersGetAccountsByUserId(requestedUserId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));

            var userResult = contentResult.Value as ErrorViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.Status, Is.EqualTo((int) HttpStatusCode.NotFound));
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetAccountsByUserId returns all bank accounts for the specified user
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 200
        ///     Value should be a List of AccountOverviewViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetAccountsByUserIdReturnsUserBankAccounts()
        {
            const int userId = 1;
            const string bankId = "TestBank";
            const string accountNumber1 = "112233";
            const string accountNumber2 = "122334";

            var bankAccount1 =
                new BankAccount {Id = 1, UserId = userId, BankId = bankId, AccountNumber = accountNumber1};
            var bankAccount2 =
                new BankAccount {Id = 2, UserId = userId, BankId = bankId, AccountNumber = accountNumber2};
            var allAccounts = new[] {bankAccount1, bankAccount2};

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetUserById(userId)
                .Returns(new AppUser {Id = userId});

            var accountRepository = Substitute.For<IBankAccountRepository>();
            accountRepository.GetAllAccountsByUserId(userId)
                .Returns(allAccounts.AsQueryable());

            var controller = new UserAccountsApiController(userRepository, accountRepository);

            var actionResult = controller.ApiV1UsersGetAccountsByUserId(userId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            var accountsResult = contentResult.Value as List<AccountOverviewViewModel>;

            Assert.That(accountsResult, Is.Not.Null);
            Assert.That(accountsResult.Count, Is.EqualTo(allAccounts.Length));

            for (var i = 0; i < allAccounts.Length; i++)
            {
                Assert.That(accountsResult[i].AccountId, Is.EqualTo(allAccounts[i].Id));
                Assert.That(accountsResult[i].BankId, Is.EqualTo(allAccounts[i].BankId));
                Assert.That(accountsResult[i].AccountNumber, Is.EqualTo(allAccounts[i].AccountNumber));
            }
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetAccountsByUserId returns 400 when a user id is not supplied
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 400
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetByIdHandlesUserIdNotSupplied()
        {
            var controller = new UserAccountsApiController(null, null);

            var actionResult = controller.ApiV1UsersGetAccountsByUserId(null);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var userResult = contentResult.Value as ErrorViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.Status, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }
    }
}