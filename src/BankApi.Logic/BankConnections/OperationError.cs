namespace BankApi.Logic.BankConnections
{
    /// <summary>
    ///     Describes an error which occured while performing a service operation
    /// </summary>
    public class OperationError
    {
        /// <summary>
        ///     Initializes the OperationError
        /// </summary>
        /// <param name="errorCode">Error code identifying the error which has occurred</param>
        /// <param name="errorMessage">Error message describing the error which has occurred</param>
        public OperationError(long? errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        ///     Gets the error code for the error which has occurred
        /// </summary>
        public long? ErrorCode { get; }

        /// <summary>
        ///     Gets the error message describing the error which has occurred
        /// </summary>
        public string ErrorMessage { get; }
    }
}