using System.Threading.Tasks;
using BankApi.Logic.BankConnections.Data;

namespace BankApi.Logic.BankConnections
{
    public interface IBankConnection
    {
        Task<OperationResult<AccountDetails>> GetAccountDetails(string accountNumber);
    }
}