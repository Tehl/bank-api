using BankApi.Connections.BizfiBank.Generated.Api;
using BankApi.Logic.BankConnections;

namespace BankApi.Connections.BizfiBank
{
    /// <summary>
    ///     Implements IBankConnectionProvider for the BizfiBank api service
    /// </summary>
    public class BizfiBankConnectionProvider : IBankConnectionProvider
    {
        /// <summary>
        ///     Gets the id of the BizfiBank banking service
        /// </summary>
        public string BankId => "BizfiBank";

        /// <summary>
        ///     Creates a connection to the BizfiBank banking service
        /// </summary>
        /// <returns>IBankConnection instance providing banking services</returns>
        public IBankConnection CreateConnection()
        {
            const string remoteServiceUri = "http://bizfibank-bizfitech.azurewebsites.net";

            return new BizfiBankConnection(
                new AccountsApi(remoteServiceUri),
                new TransactionsApi(remoteServiceUri)
            );
        }
    }
}