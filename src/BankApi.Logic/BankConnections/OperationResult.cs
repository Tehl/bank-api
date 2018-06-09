using System;

namespace BankApi.Logic.BankConnections
{
    public class OperationResult<TResult>
    {
        public const int StatusCode_Success = 200;

        public OperationResult(TResult result)
        {
            StatusCode = StatusCode_Success;
            Result = result;
        }

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

        public int StatusCode { get; }
        public TResult Result { get; }
        public OperationError Error { get; set; }
        public bool Success => StatusCode == StatusCode_Success;
    }
}