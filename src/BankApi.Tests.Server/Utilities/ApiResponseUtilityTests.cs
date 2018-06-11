using System.Net;
using BankApi.Server.Models;
using BankApi.Server.Utilities;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BankApi.Tests.Server.Utilities
{
    /// <summary>
    ///     Contains tests for <see cref="ApiResponseUtility" />
    /// </summary>
    [TestFixture]
    public class ApiResponseUtilityTests
    {
        /// <summary>
        ///     Tests that ApiError generates an ErrorViewModel containing the specified error details
        /// </summary>
        [Test]
        public void ApiErrorCreatesErrorViewModel()
        {
            const int statusCode = (int) HttpStatusCode.InternalServerError;
            const string errorMessage = "An error has occurred";

            var actionResult = ApiResponseUtility.ApiError((HttpStatusCode) statusCode, errorMessage);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo(statusCode));

            var errorResult = contentResult.Value as ErrorViewModel;

            Assert.That(errorResult, Is.Not.Null);
            Assert.That(errorResult.Status, Is.EqualTo(statusCode));
            Assert.That(errorResult.Message, Is.EqualTo(errorMessage));
        }
    }
}