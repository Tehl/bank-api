namespace BankApi.Logic.BankConnections
{
    public class OperationError
    {
        public OperationError(long? errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public long? ErrorCode { get; }
        public string ErrorMessage { get; }
    }
}