// Sifaka Game Studios (C) 2017

using Assets.Scripts.Services;
using Assets.Scripts.Tests.Services;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Services
{
    [TestFixture]
    public class GameServiceProviderTests
    {
        [TearDown]
        public void AfterTest()
        {
            GameServiceProvider.ClearGameServiceProvider();
        }

        [Test]
        public void Awake_BecomesActiveInstance()
        {
            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            serviceProvider.TestAwake();

            Assert.AreSame(serviceProvider, GameServiceProvider.CurrentInstance);
        }

        [Test]
        public void AwakeSecondInstance_LogsErrorAndDoesNotAssign()
        {
            LogAssert.Expect(LogType.Error, "Found existing instance of GameServiceProvider!");

            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            serviceProvider.TestAwake();

            var otherServiceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            otherServiceProvider.TestAwake();

            Assert.AreSame(serviceProvider, GameServiceProvider.CurrentInstance);
        }

        [Test]
        public void ClearGameServiceProvider_RemovesCurrentServiceProvider()
        {
            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();
            serviceProvider.TestAwake();

            GameServiceProvider.ClearGameServiceProvider();

            Assert.IsNull(GameServiceProvider.CurrentInstance);
        }

        [Test]
        public void AddService_CanBeRetrievedByCorrectType()
        {
            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();

            const string exampleService = "Service";
            serviceProvider.AddService(exampleService);

            Assert.AreSame(serviceProvider.GetService<string>(), exampleService);
        }

        [Test]
        public void GetService_LogsErrorIfDoesNotExist()
        {
            var serviceProvider = new GameObject().AddComponent<TestGameServiceProvider>();

            var exampleServiceType = typeof(string);

            LogAssert.Expect(LogType.Error, "Could not find service of type" + exampleServiceType);
            serviceProvider.GetService<string>();
        }
    }
}
