using System;
using System.Linq;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories.InMemory
{
    /// <summary>
    ///     Implements IUserRepository using an in-memory data set
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly InMemoryDbSet<AppUser> _dbSet = new InMemoryDbSet<AppUser>();

        /// <summary>
        ///     Creates an AppUser with the specified username
        /// </summary>
        /// <param name="username">Username for the user to be created</param>
        /// <returns>The created AppUser instance</returns>
        public AppUser CreateUser(string username)
        {
            var existingUser = GetUserByUsername(username);
            if (existingUser != null)
                throw new InvalidOperationException($"AppUser with username {username} already exists");

            var newUser = new AppUser
            {
                Username = username
            };

            return _dbSet.Add(newUser);
        }

        /// <summary>
        ///     Retrieves an AppUser with the specified user id
        /// </summary>
        /// <param name="userId">Id of the AppUser to be retrieved</param>
        /// <returns>The requested AppUser if it exists, otherwise null</returns>
        public AppUser GetUserById(int userId)
        {
            return _dbSet.Query().FirstOrDefault(o => o.Id == userId);
        }

        /// <summary>
        ///     Retrieves an AppUser with the specified username
        /// </summary>
        /// <param name="username">Username of the AppUser to be retrieved</param>
        /// <returns>The requested AppUser if it exists, otherwise null</returns>
        public AppUser GetUserByUsername(string username)
        {
            return _dbSet.Query().FirstOrDefault(o => o.Username == username);
        }

        /// <summary>
        ///     Gets all users from the underlying store
        /// </summary>
        /// <returns>IQueryable instance representing all users in the store</returns>
        public IQueryable<AppUser> GetAllUsers()
        {
            return _dbSet.Query();
        }
    }
}