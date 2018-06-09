namespace BankApi.Logic.BankConnections
{
    public interface IBankConnectionProvider
    {
        string BankId { get; set; }
        IBankConnection CreateConnection();
    }
}