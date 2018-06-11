using System.Net;
using BankApi.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Server.Utilities
{
    /// <summary>
    ///     Contains utility methods for working with API responses
    /// </summary>
    public static class ApiResponseUtility
    {
        /// <summary>
        ///     Creates a response containing an ErrorViewModel which describes the error
        /// </summary>
        /// <param name="statusCode">Status code for the API response</param>
        /// <param name="errorMessage">Error message describing the error which has occured</param>
        /// <returns></returns>
        public static IActionResult ApiError(HttpStatusCode statusCode, string errorMessage)
        {
            var error = new ErrorViewModel
            {
                Status = (int) statusCode,
                Message = errorMessage
            };

            return new ObjectResult(error)
            {
                StatusCode = (int) statusCode
            };
        }
    }
}