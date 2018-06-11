using System.Collections.Generic;
using System.Linq;
using System.Net;
using BankApi.Logic.Data.Models;
using BankApi.Logic.Data.Repositories;
using BankApi.Server.Controllers;
using BankApi.Server.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace BankApi.Tests.Server.Controllers
{
    /// <summary>
    ///     Contains tests for <see cref="UsersApiController" />
    /// </summary>
    [TestFixture]
    public class UsersApiTests
    {
        /// <summary>
        ///     Tests that ApiV1UsersGetAll returns all users from the underlying repository
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 200
        ///     Value should be a List of UserViewModels
        /// </remarks>
        [Test]
        public void ApiV1UsersGetAllReturnsAllUsers()
        {
            var user1 = new AppUser {Id = 1, Username = "User1"};
            var user2 = new AppUser {Id = 2, Username = "User2"};
            var allUsers = new[] {user1, user2};

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetAllUsers().Returns(allUsers.AsQueryable());

            var controller = new UsersApiController(userRepository);

            var actionResult = controller.ApiV1UsersGetAll();
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            var usersResult = contentResult.Value as List<UserViewModel>;

            Assert.That(usersResult, Is.Not.Null);
            Assert.That(usersResult.Count, Is.EqualTo(allUsers.Length));

            for (var i = 0; i < allUsers.Length; i++)
            {
                Assert.That(usersResult[i].UserId, Is.EqualTo(allUsers[i].Id));
                Assert.That(usersResult[i].Username, Is.EqualTo(allUsers[i].Username));
            }
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetById returns 404 when the requested user does not exist
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 404
        ///     Value should be an ErrorViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetByIdHandlesUserNotFound()
        {
            const int requestedUserId = 1;

            var allUsers = new AppUser[0];

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetAllUsers().Returns(allUsers.AsQueryable());

            var controller = new UsersApiController(userRepository);

            var actionResult = controller.ApiV1UsersGetById(requestedUserId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.NotFound));

            var userResult = contentResult.Value as ErrorViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.Status, Is.EqualTo((int) HttpStatusCode.NotFound));
        }

        /// <summary>
        ///     Tests that ApiV1UsersGetById can locate a known user by id
        /// </summary>
        /// <remarks>
        ///     StatusCode should be 200
        ///     Value should be the requested UserViewModel
        /// </remarks>
        [Test]
        public void ApiV1UsersGetByIdReturnsUserById()
        {
            const int userId = 1;

            var user1 = new AppUser {Id = userId, Username = "User1"};
            var allUsers = new[] {user1};

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetAllUsers().Returns(allUsers.AsQueryable());

            var controller = new UsersApiController(userRepository);

            var actionResult = controller.ApiV1UsersGetById(userId);
            var contentResult = actionResult as ObjectResult;

            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));

            var userResult = contentResult.Value as UserViewModel;

            Assert.That(userResult, Is.Not.Null);
            Assert.That(userResult.UserId, Is.EqualTo(userId));
            Assert.That(userResult.Username, Is.EqualTo(user1.Username));
        }
    }
}