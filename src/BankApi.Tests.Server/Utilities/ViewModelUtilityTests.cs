using BankApi.Logic.Data.Models;
using BankApi.Server.Utilities;
using NUnit.Framework;

namespace BankApi.Tests.Server.Utilities
{
    /// <summary>
    ///     Contains tests for <see cref="ViewModelUtility" />
    /// </summary>
    [TestFixture]
    public class ViewModelUtilityTests
    {
        /// <summary>
        ///     Tests that CreateUserViewModel generates a UserViewModel containing the specified user details
        /// </summary>
        [Test]
        public void CreateUserViewModelReturnsUserData()
        {
            var user = new AppUser {Id = 1, Username = "User1"};

            var model = ViewModelUtility.CreateUserViewModel(user);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.UserId, Is.EqualTo(user.Id));
            Assert.That(model.Username, Is.EqualTo(user.Username));
        }
    }
}