using System;
using System.Threading.Tasks;
using BankApi.Connections.BizfiBank;
using BankApi.Connections.BizfiBank.Generated.Api;
using BankApi.Connections.BizfiBank.Generated.Model;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace BankApi.Tests.Connections.BizfiBank
{
    /// <summary>
    ///     Contains tests for <see cref="BizfiBankConnection" />
    /// </summary>
    [TestFixture]
    public class BizfiBankConnectionTests
    {
        /// <summary>
        ///     Generates a Swagger ApiException for the specified error state
        /// </summary>
        private static Exception SwaggerApiException(int statusCode, long errorCode, string errorMessage)
        {
            return TestUtility.SwaggerApiException(
                statusCode,
                "",
                new ErrorViewModel(
                    errorMessage,
                    statusCode,
                    errorCode
                )
            );
        }

        /// <summary>
        ///     Tests that the GetAccountDetails correctly handles a status 404 response from the server
        /// </summary>
        /// <remarks>
        ///     The OperationResult should
        ///     * return Success == false
        ///     * return a null Result
        ///     * return an Error containing the error code from the server
        /// </remarks>
        [Test]
        public async Task GetAccountDetailsHandlesAccountNotFound()
        {
            const int statusCodeNotFound = 404;
            const long errorCodeNotFound = 1001123;
            const string errorMessageNotFound = "Unable to find account with account number \'{account_number}\'";

            var accountsApi = Substitute.For<IAccountsApi>();
            accountsApi.ApiV1AccountsByAccountNumberGetAsync("")
                .ThrowsForAnyArgs(
                    SwaggerApiException(
                        statusCodeNotFound,
                        errorCodeNotFound,
                        errorMessageNotFound
                    )
                );

            var connection = new BizfiBankConnection(accountsApi, null);

            var result = await connection.GetAccountDetails("12345678");

            Assert.That(result.Success, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.Error, Is.Not.Null);

            Assert.That(result.StatusCode, Is.EqualTo(statusCodeNotFound));
            Assert.That(result.Error.ErrorCode, Is.EqualTo(errorCodeNotFound));
        }

        /// <summary>
        ///     Tests that the GetAccountDetails correctly handles a status 400 response from the server
        /// </summary>
        /// <remarks>
        ///     The OperationResult should
        ///     * return Success == false
        ///     * return a null Result
        ///     * return an Error containing the error code from the server
        /// </remarks>
        [Test]
        public async Task GetAccountDetailsHandlesInvalidAccountNumber()
        {
            const int statusCodeInvalidAccountNumber = 400;
            const long errorCodeInvalidAccountNumber = 1001124;
            const string errorMessageInvalidAccountNumber =
                "Account number \'{account_number}\' does not fit format ^[\\\\d]{8}$";

            var accountsApi = Substitute.For<IAccountsApi>();
            accountsApi.ApiV1AccountsByAccountNumberGetAsync("")
                .ThrowsForAnyArgs(
                    SwaggerApiException(
                        statusCodeInvalidAccountNumber,
                        errorCodeInvalidAccountNumber,
                        errorMessageInvalidAccountNumber
                    )
                );

            var connection = new BizfiBankConnection(accountsApi, null);

            var result = await connection.GetAccountDetails("0");

            Assert.That(result.Success, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.Error, Is.Not.Null);

            Assert.That(result.StatusCode, Is.EqualTo(statusCodeInvalidAccountNumber));
            Assert.That(result.Error.ErrorCode, Is.EqualTo(errorCodeInvalidAccountNumber));
        }

        /// <summary>
        ///     Tests that the GetAccountDetails correctly handles an unexpected error
        /// </summary>
        /// <remarks>
        ///     The OperationResult should
        ///     * return Success == false
        ///     * return a null Result
        ///     * return an Error containing a null ErrorCode
        /// </remarks>
        [Test]
        public async Task GetAccountDetailsHandlesServerError()
        {
            const int statusCodeServerError = 500;

            var accountsApi = Substitute.For<IAccountsApi>();
            accountsApi.ApiV1AccountsByAccountNumberGetAsync("")
                .ThrowsForAnyArgs(
                    new Exception()
                );

            var connection = new BizfiBankConnection(accountsApi, null);

            var result = await connection.GetAccountDetails("12345678");

            Assert.That(result.Success, Is.False);
            Assert.That(result.Result, Is.Null);
            Assert.That(result.Error, Is.Not.Null);

            Assert.That(result.StatusCode, Is.EqualTo(statusCodeServerError));
            Assert.That(result.Error.ErrorCode, Is.Null);
        }

        /// <summary>
        ///     Tests that the GetAccountDetails returns the correct account data from the server
        /// </summary>
        /// <remarks>
        ///     The OperationResult should
        ///     * return Success == true
        ///     * return a Result containing the correct data for the requested account number
        ///     * return a null Error
        /// </remarks>
        [Test]
        public async Task GetAccountDetailsReturnsData()
        {
            const string accountNumber = "00112233";

            var remoteAccountData = new AccountViewModel
            {
                AccountNumber = accountNumber,
                AccountName = "Current Account",
                SortCode = "079046",
                Balance = 350,
                AvailableBalance = 400,
                Overdraft = 50
            };

            var accountsApi = Substitute.For<IAccountsApi>();
            accountsApi.ApiV1AccountsByAccountNumberGetAsync(accountNumber)
                .Returns(remoteAccountData);

            var connection = new BizfiBankConnection(accountsApi, null);

            var result = await connection.GetAccountDetails(accountNumber);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Result, Is.Not.Null);
            Assert.That(result.Error, Is.Null);

            Assert.That(result.Result.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(result.Result.AccountName, Is.EqualTo(remoteAccountData.AccountName));
            Assert.That(result.Result.SortCode, Is.EqualTo(remoteAccountData.SortCode));
            Assert.That(result.Result.CurrentBalance, Is.EqualTo(remoteAccountData.Balance));
            Assert.That(result.Result.OverdraftLimit, Is.EqualTo(remoteAccountData.Overdraft));
        }
    }
}