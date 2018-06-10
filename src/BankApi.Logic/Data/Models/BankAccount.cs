namespace BankApi.Logic.Data.Models
{
    public class BankAccount : DbModel
    {
        public int UserId { get; set; }
        public string BankId { get; set; }
        public string AccountNumber { get; set; }
    }
}