namespace BankApi.Logic.BankConnections
{
    /// <summary>
    ///     Describes a type which can be used to create connections to a banking service
    /// </summary>
    public interface IBankConnectionProvider
    {
        /// <summary>
        ///     Gets the id of the banking service which provides account data
        /// </summary>
        string BankId { get; }

        /// <summary>
        ///     Creates a connection to the banking service
        /// </summary>
        /// <returns>IBankConnection instance providing banking services</returns>
        IBankConnection CreateConnection();
    }
}