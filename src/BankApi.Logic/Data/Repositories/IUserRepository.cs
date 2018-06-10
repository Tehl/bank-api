using System.Linq;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories
{
    /// <summary>
    ///     Describes a type which can store and query AppUser instances
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        ///     Creates an AppUser with the specified username
        /// </summary>
        /// <param name="username">Username for the user to be created</param>
        /// <returns>The created AppUser instance</returns>
        AppUser CreateUser(string username);

        /// <summary>
        ///     Retrieves an AppUser with the specified user id
        /// </summary>
        /// <param name="userId">Id of the AppUser to be retrieved</param>
        /// <returns>The requested AppUser if it exists, otherwise null</returns>
        AppUser GetUserById(int userId);

        /// <summary>
        ///     Retrieves an AppUser with the specified username
        /// </summary>
        /// <param name="username">Username of the AppUser to be retrieved</param>
        /// <returns>The requested AppUser if it exists, otherwise null</returns>
        AppUser GetUserByUsername(string username);

        /// <summary>
        ///     Gets all users from the underlying store
        /// </summary>
        /// <returns>IQueryable instance representing all users in the store</returns>
        IQueryable<AppUser> GetAllUsers();
    }
}