using System;
using System.Net;

namespace BankApi.Logic.BankConnections
{
    /// <summary>
    ///     Describes the result of a service operation
    /// </summary>
    /// <typeparam name="TResult">Type of the result data expected from the operation</typeparam>
    public class OperationResult<TResult>
    {
        /// <summary>
        ///     Status code indicating that the operation was successful
        /// </summary>
        public const int StatusCode_Success = (int) HttpStatusCode.OK;

        /// <summary>
        ///     Initializes the OperationResult with the outcome of a successful operation
        /// </summary>
        /// <param name="result">Result data from the completed operation</param>
        public OperationResult(TResult result)
        {
            StatusCode = StatusCode_Success;
            Result = result;
        }

        /// <summary>
        ///     Initializes the OperationResult with the outcome of a failed operation
        /// </summary>
        /// <param name="statusCode">Status code of the error which has occurred</param>
        /// <param name="error">OperationError describing the error which has occurred</param>
        public OperationResult(int statusCode, OperationError error)
        {
            if (statusCode == StatusCode_Success)
                throw new ArgumentException(
                    $"Cannot initialize an OperationResult with error details when the status code is {StatusCode_Success}",
                    nameof(statusCode)
                );

            StatusCode = statusCode;
            Error = error;
        }

        /// <summary>
        ///     Gets the status code for the result of the operation
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        ///     Gets the result data from the operation
        /// </summary>
        public TResult Result { get; }

        /// <summary>
        ///     Gets a description of the error which occured while performing the operation
        /// </summary>
        public OperationError Error { get; set; }

        /// <summary>
        ///     Gets a flag indicating if the operation was successful
        /// </summary>
        public bool Success => StatusCode == StatusCode_Success;
    }
}