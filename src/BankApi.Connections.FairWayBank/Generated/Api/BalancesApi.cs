/* 
 * FairWayBank
 *
 * FairWayBank Accounts + Transactions API
 *
 * OpenAPI spec version: v1
 * Contact: development@bizfitech.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.Client;
using IO.Swagger.Model;
using RestSharp;

namespace IO.Swagger.Api
{
    /// <summary>
    ///     Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IBalancesApi : IApiAccessor
    {
        #region Synchronous Operations

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>BalanceViewModel</returns>
        BalanceViewModel ApiV1AccountsByAccountNumberBalanceGet(string accountNumber);

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>ApiResponse of BalanceViewModel</returns>
        ApiResponse<BalanceViewModel> ApiV1AccountsByAccountNumberBalanceGetWithHttpInfo(string accountNumber);

        #endregion Synchronous Operations

        #region Asynchronous Operations

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>Task of BalanceViewModel</returns>
        Task<BalanceViewModel> ApiV1AccountsByAccountNumberBalanceGetAsync(string accountNumber);

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>Task of ApiResponse (BalanceViewModel)</returns>
        Task<ApiResponse<BalanceViewModel>> ApiV1AccountsByAccountNumberBalanceGetAsyncWithHttpInfo(
            string accountNumber);

        #endregion Asynchronous Operations
    }

    /// <summary>
    ///     Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class BalancesApi : IBalancesApi
    {
        private ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BalancesApi" /> class.
        /// </summary>
        /// <returns></returns>
        public BalancesApi(string basePath)
        {
            Configuration = new Configuration {BasePath = basePath};

            ExceptionFactory = Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BalancesApi" /> class
        ///     using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public BalancesApi(Configuration configuration = null)
        {
            if (configuration == null) // use the default one in Configuration
                Configuration = Configuration.Default;
            else
                Configuration = configuration;

            ExceptionFactory = Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        ///     Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        ///     Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public Configuration Configuration { get; set; }

        /// <summary>
        ///     Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                return _exceptionFactory;
            }
            set => _exceptionFactory = value;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>BalanceViewModel</returns>
        public BalanceViewModel ApiV1AccountsByAccountNumberBalanceGet(string accountNumber)
        {
            var localVarResponse = ApiV1AccountsByAccountNumberBalanceGetWithHttpInfo(accountNumber);
            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>ApiResponse of BalanceViewModel</returns>
        public ApiResponse<BalanceViewModel> ApiV1AccountsByAccountNumberBalanceGetWithHttpInfo(string accountNumber)
        {
            // verify the required parameter 'accountNumber' is set
            if (accountNumber == null)
                throw new ApiException(400,
                    "Missing required parameter 'accountNumber' when calling BalancesApi->ApiV1AccountsByAccountNumberBalanceGet");

            var localVarPath = "/api/v1/accounts/{account_number}/balance";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes =
            {
            };
            var localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts =
            {
                "application/json"
            };
            var localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accountNumber != null)
                localVarPathParams.Add("account_number",
                    Configuration.ApiClient.ParameterToString(accountNumber)); // path parameter


            // make the HTTP request
            var localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams,
                localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            var localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                var exception = ExceptionFactory("ApiV1AccountsByAccountNumberBalanceGet", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<BalanceViewModel>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (BalanceViewModel) Configuration.ApiClient.Deserialize(localVarResponse, typeof(BalanceViewModel)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>Task of BalanceViewModel</returns>
        public async Task<BalanceViewModel> ApiV1AccountsByAccountNumberBalanceGetAsync(string accountNumber)
        {
            var localVarResponse = await ApiV1AccountsByAccountNumberBalanceGetAsyncWithHttpInfo(accountNumber);
            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="IO.Swagger.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="accountNumber"></param>
        /// <returns>Task of ApiResponse (BalanceViewModel)</returns>
        public async Task<ApiResponse<BalanceViewModel>> ApiV1AccountsByAccountNumberBalanceGetAsyncWithHttpInfo(
            string accountNumber)
        {
            // verify the required parameter 'accountNumber' is set
            if (accountNumber == null)
                throw new ApiException(400,
                    "Missing required parameter 'accountNumber' when calling BalancesApi->ApiV1AccountsByAccountNumberBalanceGet");

            var localVarPath = "/api/v1/accounts/{account_number}/balance";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes =
            {
            };
            var localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts =
            {
                "application/json"
            };
            var localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (accountNumber != null)
                localVarPathParams.Add("account_number",
                    Configuration.ApiClient.ParameterToString(accountNumber)); // path parameter


            // make the HTTP request
            var localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams,
                localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            var localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                var exception = ExceptionFactory("ApiV1AccountsByAccountNumberBalanceGet", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<BalanceViewModel>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Name, x => x.Value.ToString()),
                (BalanceViewModel) Configuration.ApiClient.Deserialize(localVarResponse, typeof(BalanceViewModel)));
        }

        /// <summary>
        ///     Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete(
            "SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(string basePath)
        {
            // do nothing
        }

        /// <summary>
        ///     Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public IDictionary<string, string> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(Configuration.DefaultHeader);
        }

        /// <summary>
        ///     Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns>
        [Obsolete("AddDefaultHeader is deprecated, please use Configuration.AddDefaultHeader instead.")]
        public void AddDefaultHeader(string key, string value)
        {
            Configuration.AddDefaultHeader(key, value);
        }
    }
}