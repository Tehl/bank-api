using System;
using System.Linq;
using BankApi.Logic.Data.Repositories.InMemory;
using NUnit.Framework;

namespace BankApi.Tests.Logic.Data.Repositories.InMemory
{
    /// <summary>
    ///     Contains tests for <see cref="InMemoryUserRepository" />
    /// </summary>
    [TestFixture]
    public class InMemoryUserRepositoryTests
    {
        /// <summary>
        ///     Tests that CreateUser can add a user to the store
        /// </summary>
        [Test]
        public void CreateUserCanAddUser()
        {
            const string username = "TestUser";

            var repository = new InMemoryUserRepository();

            var createdUser = repository.CreateUser(username);

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.Id, Is.GreaterThan(0));
            Assert.That(createdUser.Username, Is.EqualTo(username));
        }

        /// <summary>
        ///     Tests that users added via CreateUser are assigned unique database ids
        /// </summary>
        [Test]
        public void CreateUserGeneratesUniqueIds()
        {
            const string username1 = "TestUser1";
            const string username2 = "TestUser1";

            var repository = new InMemoryUserRepository();

            var createdUser1 = repository.CreateUser(username1);
            var createdUser2 = repository.CreateUser(username2);

            Assert.That(createdUser1.Id, Is.Not.EqualTo(createdUser2.Id));
        }

        /// <summary>
        ///     Tests that CreateUser throws an InvalidOperationException if a user with the specified username already exists
        /// </summary>
        [Test]
        public void CreateUserRejectsDuplicateUsername()
        {
            const string username = "TestUser";

            var repository = new InMemoryUserRepository();

            repository.CreateUser(username);

            Assert.Throws<InvalidOperationException>(
                () => repository.CreateUser(username)
            );
        }

        /// <summary>
        ///     Tests that GetAllUsers returns all users contained in the store
        /// </summary>
        [Test]
        public void GetAllUsersReturnsAllCreatedUsers()
        {
            const string username1 = "TestUser1";
            const string username2 = "TestUser2";

            var repository = new InMemoryUserRepository();

            var createdUser1 = repository.CreateUser(username1);
            var createdUser2 = repository.CreateUser(username2);

            var allUsers = repository.GetAllUsers().ToList();

            Assert.That(allUsers.Count, Is.EqualTo(2));
            Assert.That(allUsers, Contains.Item(createdUser1));
            Assert.That(allUsers, Contains.Item(createdUser2));
        }

        /// <summary>
        ///     Tests that users can be located by id
        /// </summary>
        [Test]
        public void GetUserByIdCanLocateUser()
        {
            const string username = "TestUser";

            var repository = new InMemoryUserRepository();

            var createdUser = repository.CreateUser(username);
            var locatedUser = repository.GetUserById(createdUser.Id);

            Assert.That(createdUser, Is.EqualTo(locatedUser));
        }

        /// <summary>
        ///     Tests that requesting a user with an unknown id returns null
        /// </summary>
        [Test]
        public void GetUserByIdHandlesUserNotFound()
        {
            const int requestedUserId = 1;

            var repository = new InMemoryUserRepository();

            var locatedUser = repository.GetUserById(requestedUserId);

            Assert.That(locatedUser, Is.Null);
        }

        /// <summary>
        ///     Tests that users can be located by username
        /// </summary>
        [Test]
        public void GetUserByUsernameCanLocateUser()
        {
            const string username = "TestUser";

            var repository = new InMemoryUserRepository();

            var createdUser = repository.CreateUser(username);
            var locatedUser = repository.GetUserByUsername(username);

            Assert.That(createdUser, Is.EqualTo(locatedUser));
        }

        /// <summary>
        ///     Tests that requesting a user with an unknown username returns null
        /// </summary>
        [Test]
        public void GetUserByUsernameHandlesUserNotFound()
        {
            const string requestedUsername = "TestUser";

            var repository = new InMemoryUserRepository();

            var locatedUser = repository.GetUserByUsername(requestedUsername);

            Assert.That(locatedUser, Is.Null);
        }
    }
}