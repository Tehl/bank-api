using System;
using IO.Swagger.Client;
using Newtonsoft.Json;

namespace BankApi.Tests
{
    /// <summary>
    ///     Utility methods to support unit testing
    /// </summary>
    public static class TestUtility
    {
        /// <summary>
        ///     Generates a Swagger ApiException for the specified error state
        /// </summary>
        public static Exception SwaggerApiException(int errorCode, string errorMessage, object errorDetails)
        {
            string errorContent = null;
            if (errorDetails != null)
                errorContent = JsonConvert.SerializeObject(errorDetails);

            return new ApiException(errorCode, errorMessage, errorContent);
        }
    }
}