using System;
using System.Net;
using System.Threading.Tasks;
using BankApi.Connections.BizfiBank.Generated.Api;
using BankApi.Connections.BizfiBank.Generated.Model;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;
using IO.Swagger.Client;
using Newtonsoft.Json;

namespace BankApi.Connections.BizfiBank
{
    /// <summary>
    ///     Implements IBankConnection for the BizfiBank api service
    /// </summary>
    public class BizfiBankConnection : IBankConnection
    {
        private readonly IAccountsApi _accountsApi;
        private readonly ITransactionsApi _transactionsApi;

        /// <summary>
        ///     Initializes the BizfiBankConnection
        /// </summary>
        /// <param name="accountsApi">Service implementation providing access to the Accounts api</param>
        /// <param name="transactionsApi">Service implementation providing access to the Transactions api</param>
        public BizfiBankConnection(IAccountsApi accountsApi, ITransactionsApi transactionsApi)
        {
            _accountsApi = accountsApi;
            _transactionsApi = transactionsApi;
        }

        /// <summary>
        ///     Gets account details for the specified account number
        /// </summary>
        /// <param name="accountNumber">Account number to retrieve account details for</param>
        /// <returns>OperationResult instance describing the outcome of the remote query</returns>
        public async Task<OperationResult<AccountDetails>> GetAccountDetails(string accountNumber)
        {
            try
            {
                var response = await _accountsApi.ApiV1AccountsByAccountNumberGetAsync(accountNumber);

                var accountDetails = new AccountDetails
                {
                    AccountName = response.AccountName,
                    AccountNumber = response.AccountNumber,
                    SortCode = response.SortCode,
                    CurrentBalance = response.Balance ?? 0,
                    OverdraftLimit = response.Overdraft ?? 0
                };

                return new OperationResult<AccountDetails>(accountDetails);
            }
            catch (ApiException ex)
            {
                return CatchApiException<AccountDetails>(ex);
            }
            catch (Exception ex)
            {
                return CatchGenericException<AccountDetails>(ex);
            }
        }

        /// <summary>
        ///     Handles an ApiException raised by the remote service
        /// </summary>
        /// <typeparam name="TResult">Return type for the calling operation</typeparam>
        /// <param name="ex">ApiException instance describing the error which has occurred</param>
        /// <returns>OperationResult instance describing the error which has occurred</returns>
        private static OperationResult<TResult> CatchApiException<TResult>(ApiException ex)
        {
            try
            {
                var errorJson = ex.ErrorContent.ToString();
                var errorDetails = JsonConvert.DeserializeObject<ErrorViewModel>(errorJson);

                return new OperationResult<TResult>(
                    errorDetails.Status,
                    new OperationError(errorDetails.ErrorCode, errorDetails.Message)
                );
            }
            catch (Exception innerException)
            {
                return CatchGenericException<TResult>(innerException);
            }
        }

        /// <summary>
        ///     Handles an Exception raised by the remote service
        /// </summary>
        /// <typeparam name="TResult">Return type for the calling operation</typeparam>
        /// <param name="ex">Exception instance describing the error which has occurred</param>
        /// <returns>OperationResult instance describing the error which has occurred</returns>
        private static OperationResult<TResult> CatchGenericException<TResult>(Exception ex)
        {
            return new OperationResult<TResult>(
                (int) HttpStatusCode.InternalServerError,
                new OperationError(null, "An unknown error has occurred")
            );
        }
    }
}