using System;
using System.Collections.Generic;

namespace BankApi.Logic.BankConnections
{
    public class BankConnectionManager
    {
        public void RegisterConnectionProvider(IBankConnectionProvider provider)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRegisteredBankIds()
        {
            throw new NotImplementedException();
        }

        public IBankConnection CreateConnection(string bankId)
        {
            throw new NotImplementedException();
        }
    }
}