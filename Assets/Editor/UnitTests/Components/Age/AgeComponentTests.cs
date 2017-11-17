// Sifaka Game Studios (C) 2017

using Assets.Editor.UnitTests.Components.UnityEvent;
using Assets.Scripts.Components.Age;
using Assets.Scripts.Components.UnityEvent;
using Assets.Scripts.GameSettings;
using Assets.Scripts.Tests.Components.Age;
using Assets.Scripts.Tests.Components.UnityEvent;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Age
{
    [TestFixture]
    public class AgeComponentTestFixture
    {
        public static string TestSettings = "Tests/TestZooManagerGameSettings";

        private float _expectedSecondsPerDay;
        private TestAgeComponent _ageComponent;

        [SetUp]
        public void BeforeTest()
        {
            var settingsAsset = Resources.Load<TextAsset>(TestSettings);

            Assert.NotNull(settingsAsset);
            var gameSettings = JsonUtility.FromJson<ZooManagerGameSettings>(settingsAsset.text);
            Assert.NotNull(gameSettings);

            _expectedSecondsPerDay = gameSettings.SecondsPerDay;

            _ageComponent = new GameObject().AddComponent<TestAgeComponent>();

            _ageComponent.SettingsPath = TestSettings;
            _ageComponent.TestAwake();

            _ageComponent.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>().TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _ageComponent = null;
        }

        [Test]
        public void GetCurrentAge_NoTime_Zero()
        {
            Assert.AreEqual(0, _ageComponent.GetCurrentAge());
        }

        [Test]
        public void GetCurrentAge_NumberOfSecondsPerDayCycles()
        {
            const int expectedAge = 3;

            for (var i = 0; i < expectedAge; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.AreEqual(expectedAge, _ageComponent.GetCurrentAge());
        }

        [Test]
        public void GetCurrentAge_PassedMaxAge_StopsIncrementingAge()
        {
            for (var i = 0; i < _ageComponent.MaxAge + 20; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.AreEqual(_ageComponent.MaxAge + 1, _ageComponent.GetCurrentAge());
        }

        [Test]
        public void NotPassedMaxAge_FiresAgingCompleteMessage()
        {
            var messageCapture = new UnityTestMessageHandleResponseObject<AgingCompleteMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<AgingCompleteMessage>(_ageComponent.gameObject,
                    messageCapture.OnResponse);

            for (var i = 0; i < _ageComponent.MaxAge; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.IsFalse(messageCapture.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_ageComponent.gameObject, handle);
        }

        [Test]
        public void PassedMaxAge_FiresAgingCompleteMessage()
        {
            var messageCapture = new UnityTestMessageHandleResponseObject<AgingCompleteMessage>();

            var handle =
                UnityMessageEventFunctions.RegisterActionWithDispatcher<AgingCompleteMessage>(_ageComponent.gameObject,
                    messageCapture.OnResponse);

            for (var i = 0; i < _ageComponent.MaxAge + 1; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.IsTrue(messageCapture.ActionCalled);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(_ageComponent.gameObject, handle);
        }

        [Test]
        public void HasPassedMaxAge_NotPassed_False()
        {
            for (var i = 0; i < _ageComponent.MaxAge - 1; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.IsFalse(_ageComponent.HasPassedMaxAge());
        }

        [Test]
        public void HasPassedMaxAge_Equal_False()
        {
            for (var i = 0; i < _ageComponent.MaxAge; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.IsFalse(_ageComponent.HasPassedMaxAge());
        }

        [Test]
        public void HasPassedMaxAge_Greater_True()
        {
            for (var i = 0; i < _ageComponent.MaxAge + 1; i++)
            {
                _ageComponent.TestUpdate(_expectedSecondsPerDay + 0.1f);
            }

            Assert.IsTrue(_ageComponent.HasPassedMaxAge());
        }
    }
}
