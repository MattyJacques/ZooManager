// Sifaka Game Studios (C) 2017

using Assets.Scripts.Instance;
using Assets.Scripts.Tests.Instance;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Instance
{
    [TestFixture]
    public class GameInstanceTestFixture
    {
        [TearDown]
        public void AfterTest()
        {
            GameInstance.ClearGameInstance();
        }

        [Test]
        public void Awake_SetAsGameInstance()
        {
            var testInstance = new GameObject().AddComponent<TestGameInstance>();
            testInstance.TestAwake();

            Assert.AreSame(GameInstance.CurrentInstance, testInstance);
        }

        [Test]
        public void Awake_InstanceAlreadySet_Error()
        {
            LogAssert.Expect(LogType.Error, "Found existing GameInstance!");

            var testInstance = new GameObject().AddComponent<TestGameInstance>();
            testInstance.TestAwake();

            var otherTestInstance = new GameObject().AddComponent<TestGameInstance>();
            otherTestInstance.TestAwake();

            Assert.AreSame(GameInstance.CurrentInstance, testInstance);
        }

        [Test]
        public void Awake_CanBeRemovedWithClear()
        {
            var testInstance = new GameObject().AddComponent<TestGameInstance>();
            testInstance.TestAwake();

            GameInstance.ClearGameInstance();

            Assert.IsNull(GameInstance.CurrentInstance);
        }

        [Test]
        public void GetUIDispatcher_ReturnsValidDispatcher()
        {
            var testInstance = new GameObject().AddComponent<TestGameInstance>();
            testInstance.TestAwake();

            Assert.IsNotNull(GameInstance.CurrentInstance.GetUIMessageDispatcher());
        }
    }
}
