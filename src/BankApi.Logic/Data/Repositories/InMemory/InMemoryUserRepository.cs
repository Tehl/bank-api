using System;
using System.Linq;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories.InMemory
{
    public class InMemoryUserRepository : IUserRepository
    {
        public AppUser CreateUser(string username)
        {
            throw new NotImplementedException();
        }

        public AppUser GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public AppUser GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AppUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}