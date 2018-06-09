namespace BankApi.Logic.BankConnections.Data
{
    public class AccountDetails
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string SortCode { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal OverdraftLimit { get; set; }
    }
}