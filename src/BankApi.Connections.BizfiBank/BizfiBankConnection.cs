using System;
using System.Threading.Tasks;
using BankApi.Connections.BizfiBank.Generated.Api;
using BankApi.Logic.BankConnections;
using BankApi.Logic.BankConnections.Data;

namespace BankApi.Connections.BizfiBank
{
    public class BizfiBankConnection : IBankConnection
    {
        private readonly IAccountsApi _accountsApi;
        private readonly ITransactionsApi _transactionsApi;

        public BizfiBankConnection(IAccountsApi accountsApi, ITransactionsApi transactionsApi)
        {
            _accountsApi = accountsApi;
            _transactionsApi = transactionsApi;
        }

        public Task<OperationResult<AccountDetails>> GetAccountDetails(string accountNumber)
        {
            throw new NotImplementedException();
        }
    }
}