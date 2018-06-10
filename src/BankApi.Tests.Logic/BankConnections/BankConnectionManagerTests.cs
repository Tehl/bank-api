using System;
using BankApi.Logic.BankConnections;
using NSubstitute;
using NUnit.Framework;

namespace BankApi.Tests.Logic.BankConnections
{
    /// <summary>
    ///     Contains tests for <see cref="BankConnectionManager" />
    /// </summary>
    [TestFixture]
    public class BankConnectionManagerTests
    {
        /// <summary>
        ///     Tests that CreateConnection throws an InvalidOperationException if the specified bank id is not registered
        /// </summary>
        [Test]
        public void CreateConnectionFailsWithNoRegisteredProvider()
        {
            const string bankId1 = "TestBank1";
            const string bankId2 = "TestBank2";

            var testConnection = Substitute.For<IBankConnection>();

            var testProvider1 = Substitute.For<IBankConnectionProvider>();
            testProvider1.BankId.Returns(bankId1);
            testProvider1.CreateConnection().Returns(testConnection);

            var connectionManager = new BankConnectionManager();
            connectionManager.RegisterConnectionProvider(testProvider1);

            Assert.Throws<InvalidOperationException>(
                () => connectionManager.CreateConnection(bankId2)
            );
        }

        /// <summary>
        ///     Tests that CreateConnection selects the correct provider for the specified bank id
        /// </summary>
        [Test]
        public void CreateConnectionUsesRegisteredProvider()
        {
            const string bankId1 = "TestBank1";
            const string bankId2 = "TestBank2";

            var testConnection1 = Substitute.For<IBankConnection>();
            var testConnection2 = Substitute.For<IBankConnection>();

            var testProvider1 = Substitute.For<IBankConnectionProvider>();
            testProvider1.BankId.Returns(bankId1);
            testProvider1.CreateConnection().Returns(testConnection1);

            var testProvider2 = Substitute.For<IBankConnectionProvider>();
            testProvider2.BankId.Returns(bankId2);
            testProvider2.CreateConnection().Returns(testConnection2);

            var connectionManager = new BankConnectionManager();
            connectionManager.RegisterConnectionProvider(testProvider1);
            connectionManager.RegisterConnectionProvider(testProvider2);

            var newConnection = connectionManager.CreateConnection(bankId1);

            Assert.That(newConnection, Is.EqualTo(testConnection1));
            Assert.That(newConnection, Is.Not.EqualTo(testConnection2));
        }

        /// <summary>
        ///     Tests that RegisterConnectionProvider adds the specified provider to the connection manager
        /// </summary>
        [Test]
        public void RegisterConnectionProviderCanRegisterProvider()
        {
            const string bankId = "TestBank";

            var testProvider = Substitute.For<IBankConnectionProvider>();
            testProvider.BankId.Returns(bankId);

            var connectionManager = new BankConnectionManager();
            connectionManager.RegisterConnectionProvider(testProvider);

            var registeredConnections = connectionManager.GetRegisteredBankIds();

            Assert.That(registeredConnections.Count, Is.EqualTo(1));
            Assert.That(registeredConnections[0], Is.EqualTo(bankId));
        }

        /// <summary>
        ///     Tests that RegisterConnectionProvider throws an ArgumentException if another connection provider with the same bank
        ///     id has already been registered
        /// </summary>
        [Test]
        public void RegisterConnectionProviderPreventsDuplicateRegistration()
        {
            const string bankId = "TestBank";

            var testProvider1 = Substitute.For<IBankConnectionProvider>();
            testProvider1.BankId.Returns(bankId);

            var testProvider2 = Substitute.For<IBankConnectionProvider>();
            testProvider2.BankId.Returns(bankId);

            var connectionManager = new BankConnectionManager();
            connectionManager.RegisterConnectionProvider(testProvider1);

            Assert.Throws<ArgumentException>(
                () => connectionManager.RegisterConnectionProvider(testProvider2)
            );
        }
    }
}