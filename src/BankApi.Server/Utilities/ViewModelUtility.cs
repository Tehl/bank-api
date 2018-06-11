using BankApi.Logic.Data.Models;
using BankApi.Server.Models;

namespace BankApi.Server.Utilities
{
    /// <summary>
    ///     Contains utility methods for working with API view models
    /// </summary>
    public static class ViewModelUtility
    {
        /// <summary>
        ///     Creates a UserViewModel from an AppUser instance
        /// </summary>
        /// <param name="appUser">AppUser to create a UserViewModel from</param>
        /// <returns>UserViewModel instance representing the specified AppUser</returns>
        public static UserViewModel CreateUserViewModel(AppUser appUser)
        {
            return new UserViewModel
            {
                UserId = appUser.Id,
                Username = appUser.Username
            };
        }

        /// <summary>
        ///     Creates a AccountOverviewViewModel from an BankAccount instance
        /// </summary>
        /// <param name="bankAccount">BankAccount to create a AccountOverviewViewModel from</param>
        /// <returns>AccountOverviewViewModel instance representing the specified BankAccount</returns>
        public static AccountOverviewViewModel CreateAccountOverviewViewModel(BankAccount bankAccount)
        {
            return new AccountOverviewViewModel
            {
                AccountId = bankAccount.Id,
                BankId = bankAccount.BankId,
                AccountNumber = bankAccount.AccountNumber
            };
        }
    }
}