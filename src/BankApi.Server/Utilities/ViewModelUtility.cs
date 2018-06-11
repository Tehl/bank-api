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
    }
}