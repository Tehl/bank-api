using System;
using System.Threading.Tasks;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;

namespace BankApi.Logic.AccountData
{
    public class PassThroughAccountDataProvider : IAccountDataProvider
    {
        private readonly IBankConnectionManager _connectionManager;

        public PassThroughAccountDataProvider(IBankConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public Task<OperationResult<AccountDetails>> GetAccountDetails(string bankId, string accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}