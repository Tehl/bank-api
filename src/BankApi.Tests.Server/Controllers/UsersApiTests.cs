using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    ///     Contains tests for <see cref="UsersApiController" />
    /// </summary>
    [TestFixture]
    public class UsersApiTests
    {
        /// <summary>
        ///     Tests that ApiV1UsersCreate can successfully create a user
        /// </summary>
        [Test]
        public async Task ApiV1UsersCreateCanCreateUser()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string accountNumber = "11223344";

            const int internalUserId = 1;
            const int internalBankAccountId = 1;

            // CreateUser must return an AppUser
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.CreateUser(username).Returns(new AppUser
            {
                Id = internalUserId,
                Username = username
            });

            // CreateAccount must return a BankAccount
            var accountRepository = Substitute.For<IBankAccountRepository>();
            accountRepository.CreateAccount(internalUserId, bankId, accountNumber).Returns(new BankAccount
            {
                Id = internalBankAccountId,
                UserId = internalUserId,
                BankId = bankId,
                AccountNumber = accountNumber
            });

            // ApiV1UsersCreate must be able to resolve the bank account directly with the server
            var bankConnection = Substitute.For<IBankConnection>();
            bankConnection.GetAccountDetails(accountNumber).Returns(x =>
                Task.FromResult(
                    new OperationResult<AccountDetails>(new AccountDetails {AccountNumber = accountNumber})
                )
            );

            var connectionManager = Substitute.For<IBankConnectionManager>();
            connectionManager.CreateConnection(bankId).Returns(bankConnection);

            // execute controller action
            var controller = new UsersApiController(userRepository, accountRepository, connectionManager);

            var actionResult = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = accountNumber
            });

            // CreateUser and CreateAccount must be called
            Assert.DoesNotThrow(() => userRepository.Received().CreateUser(username));
            Assert.DoesNotThrow(() =>
                accountRepository.Received().CreateAccount(internalUserId, bankId, accountNumber));

            // the controller must return HTTP 200
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            // the controller must return the newly created user
            var userResult = contentResult.Value as UserViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.Username, Is.EqualTo(username));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate will pass forward any error which is returned by the remote service
        /// </summary>
        [Test]
        public async Task ApiV1UsersCreateForwardsErrorFromRemoteService()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string accountNumber = "11223344";

            const long errorCode = 1001124;

            var userRepository = Substitute.For<IUserRepository>();
            var accountRepository = Substitute.For<IBankAccountRepository>();

            // GetAccountDetails should return a bad request result even though the data is valid
            var bankConnection = Substitute.For<IBankConnection>();
            bankConnection.GetAccountDetails(accountNumber).Returns(x =>
                Task.FromResult(
                    new OperationResult<AccountDetails>(
                        (int) HttpStatusCode.BadRequest,
                        new OperationError(
                            errorCode,
                            $"Account number '{accountNumber}' does not fit format ^[\\d]{{8}}"
                        )
                    )
                )
            );

            var connectionManager = Substitute.For<IBankConnectionManager>();
            connectionManager.CreateConnection(bankId).Returns(bankConnection);

            var controller = new UsersApiController(userRepository, accountRepository, connectionManager);

            var actionResult = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = accountNumber
            });
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var errorViewModel = contentResult.Value as ErrorViewModel;

            Assert.That(errorViewModel, Is.Not.Null);
            Assert.That(errorViewModel.ErrorCode, Is.EqualTo(errorCode));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate validates the supplied account number
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 400
        /// </remarks>
        [Test]
        public async Task ApiV1UsersCreateValidatesAccountNumber()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string invalidAccountNumberByLength = "1234567";
            const string invalidAccountNumberByContent = "x1234567";
            const string invalidAccountNumberByLeadingZero = "01234567";

            var controller = new UsersApiController(null, null, null);

            var actionResultBadLength = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = invalidAccountNumberByLength
            });
            var contentResultBadLength = actionResultBadLength as ObjectResult;

            Assert.That(contentResultBadLength, Is.Not.Null);
            Assert.That(contentResultBadLength.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var actionResultBadContent = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = invalidAccountNumberByContent
            });
            var contentResultBadContent = actionResultBadContent as ObjectResult;

            Assert.That(contentResultBadContent, Is.Not.Null);
            Assert.That(contentResultBadContent.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var actionResultBadLeadingZero = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = invalidAccountNumberByLeadingZero
            });
            var contentResultBadLeadingZero = actionResultBadLeadingZero as ObjectResult;

            Assert.That(contentResultBadLeadingZero, Is.Not.Null);
            Assert.That(contentResultBadLeadingZero.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate validates the supplied bank id
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 400
        /// </remarks>
        [Test]
        public async Task ApiV1UsersCreateValidatesBankId()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank1";
            const string accountNumber = "11223344";

            const string invalidBankId = "TestBank2";

            var connectionManager = Substitute.For<IBankConnectionManager>();
            connectionManager.GetRegisteredBankIds().Returns(new List<string> {bankId});

            var controller = new UsersApiController(null, null, connectionManager);

            var actionResultBadBankId = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = invalidBankId,
                AccountNumber = accountNumber
            });
            var contentResultBadBankId = actionResultBadBankId as ObjectResult;

            Assert.That(contentResultBadBankId, Is.Not.Null);
            Assert.That(contentResultBadBankId.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate validates that the requested bank account exists on the remote banking service
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 404
        /// </remarks>
        [Test]
        public async Task ApiV1UsersCreateValidatesRemoteBankAccount()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string accountNumber = "11223344";

            const long errorCode = 1001123;

            var userRepository = Substitute.For<IUserRepository>();

            var accountRepository = Substitute.For<IBankAccountRepository>();

            var bankConnection = Substitute.For<IBankConnection>();
            bankConnection.GetAccountDetails(accountNumber).Returns(x =>
                Task.FromResult(
                    new OperationResult<AccountDetails>(
                        (int) HttpStatusCode.NotFound,
                        new OperationError(errorCode, $"Unable to find account with account number '{accountNumber}'")
                    )
                )
            );

            var connectionManager = Substitute.For<IBankConnectionManager>();
            connectionManager.CreateConnection(bankId).Returns(bankConnection);

            var controller = new UsersApiController(userRepository, accountRepository, null);

            var actionResultBankAccountNotFound = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = accountNumber
            });
            var contentResultBankAccountNotFound = actionResultBankAccountNotFound as ObjectResult;

            Assert.That(contentResultBankAccountNotFound, Is.Not.Null);
            Assert.That(contentResultBankAccountNotFound.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));

            var errorViewModel = contentResultBankAccountNotFound.Value as ErrorViewModel;

            Assert.That(errorViewModel, Is.Not.Null);
            Assert.That(errorViewModel.ErrorCode, Is.EqualTo(errorCode));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate validates all required input parameters
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ApiV1UsersCreateValidatesRequiredData()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string accountNumber = "11223344";

            var controller = new UsersApiController(null, null, null);

            var actionResultNoUserData = await controller.ApiV1UsersCreate(null);
            var contentResultNoUserData = actionResultNoUserData as ObjectResult;

            Assert.That(contentResultNoUserData, Is.Not.Null);
            Assert.That(contentResultNoUserData.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var actionResultNoUsername = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = null,
                BankId = bankId,
                AccountNumber = accountNumber
            });
            var contentResultNoUsername = actionResultNoUsername as ObjectResult;

            Assert.That(contentResultNoUsername, Is.Not.Null);
            Assert.That(contentResultNoUsername.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var actionResultNoBankId = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = null,
                AccountNumber = accountNumber
            });
            var contentResultNoBankId = actionResultNoBankId as ObjectResult;

            Assert.That(contentResultNoBankId, Is.Not.Null);
            Assert.That(contentResultNoBankId.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var actionResultNoAccountNumber = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = null
            });
            var contentResultNoAccountNumber = actionResultNoAccountNumber as ObjectResult;

            Assert.That(contentResultNoAccountNumber, Is.Not.Null);
            Assert.That(contentResultNoAccountNumber.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate validates that the bank account does not already exist
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 409
        /// </remarks>
        [Test]
        public async Task ApiV1UsersCreateValidatesUniqueBankAccount()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string accountNumber = "11223344";

            var userRepository = Substitute.For<IUserRepository>();

            var accountRepository = Substitute.For<IBankAccountRepository>();
            accountRepository.GetAccountByBankIdAndAccountNumber(bankId, accountNumber)
                .Returns(new BankAccount {BankId = bankId, AccountNumber = accountNumber});

            var controller = new UsersApiController(userRepository, accountRepository, null);

            var actionResultDuplicateBankAccount = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = accountNumber
            });
            var contentResultDuplicateBankAccount = actionResultDuplicateBankAccount as ObjectResult;

            Assert.That(contentResultDuplicateBankAccount, Is.Not.Null);
            Assert.That(contentResultDuplicateBankAccount.StatusCode, Is.EqualTo((int) HttpStatusCode.Conflict));
        }

        /// <summary>
        ///     Tests that ApiV1UsersCreate validates that the user does not already exist
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 409
        /// </remarks>
        [Test]
        public async Task ApiV1UsersCreateValidatesUniqueUser()
        {
            const string username = "TestUser1";
            const string bankId = "TestBank";
            const string accountNumber = "11223344";

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetUserByUsername(username).Returns(new AppUser {Username = username});

            var controller = new UsersApiController(userRepository, null, null);

            var actionResultDuplicateUser = await controller.ApiV1UsersCreate(new CreateUserViewModel
            {
                Username = username,
                BankId = bankId,
                AccountNumber = accountNumber
            });
            var contentResultDuplicateUser = actionResultDuplicateUser as ObjectResult;

            Assert.That(contentResultDuplicateUser, Is.Not.Null);
            Assert.That(contentResultDuplicateUser.StatusCode, Is.EqualTo((int) HttpStatusCode.Conflict));
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetAll returns all users from the underlying repository
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 200
        ///     Value should be a List of UserViewModels
        /// </remarks>
        [Test]
        public void ApiV1UsersGetAllReturnsAllUsers()
        {
            var user1 = new AppUser {Id = 1, Username = "User1"};
            var user2 = new AppUser {Id = 2, Username = "User2"};
            var allUsers = new[] {user1, user2};

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetAllUsers().Returns(allUsers.AsQueryable());

            var controller = new UsersApiController(userRepository, null, null);

            var actionResult = controller.ApiV1UsersGetAll();
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            var usersResult = contentResult.Value as List<UserViewModel>;

            Assert.That(usersResult, Is.Not.Null);
            Assert.That(usersResult.Count, Is.EqualTo(allUsers.Length));

            for (var i = 0; i < allUsers.Length; i++)
            {
                Assert.That(usersResult[i].UserId, Is.EqualTo(allUsers[i].Id));
                Assert.That(usersResult[i].Username, Is.EqualTo(allUsers[i].Username));
            }
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetById returns 400 when a user id is not supplied
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 400
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetByIdHandlesUserIdNotSupplied()
        {
            var controller = new UsersApiController(null, null, null);

            var actionResult = controller.ApiV1UsersGetById(null);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));

            var userResult = contentResult.Value as ErrorViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.Status, Is.EqualTo((int) HttpStatusCode.BadRequest));
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetById returns 404 when the requested user does not exist
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 404
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetByIdHandlesUserNotFound()
        {
            const int requestedUserId = 1;

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetUserById(0).ReturnsForAnyArgs((AppUser) null);

            var controller = new UsersApiController(userRepository, null, null);

            var actionResult = controller.ApiV1UsersGetById(requestedUserId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));

            var userResult = contentResult.Value as ErrorViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.Status, Is.EqualTo((int) HttpStatusCode.NotFound));
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetById can locate a known user by id
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 200
        ///     Value should be the requested UserViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetByIdReturnsUserById()
        {
            const int userId = 1;

            var user1 = new AppUser {Id = userId, Username = "User1"};

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetUserById(userId).Returns(user1);

            var controller = new UsersApiController(userRepository, null, null);

            var actionResult = controller.ApiV1UsersGetById(userId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            var userResult = contentResult.Value as UserViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.UserId, Is.EqualTo(userId));
            Assert.That(userResult.Username, Is.EqualTo(user1.Username));
        }
    }
}