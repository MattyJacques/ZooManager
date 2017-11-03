// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Editor.UnitTests.Components.UnityEvent;
using Assets.Scripts.Components.Health;
using Assets.Scripts.Tests.Components.Health;
using Assets.Scripts.Tests.Components.UnityEvent;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Health
{
    [TestFixture]
    public class HealthComponentTestFixture
    {
        [SetUp]
        public void BeforeTest()
        {
            _healthComponent = new GameObject().AddComponent<TestHealthComponent>();
            _healthComponent.TestStart();

            _dispatcherComponent = _healthComponent.gameObject.AddComponent<TestUnityMessageEventDispatcherComponent>();
            _dispatcherComponent.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _dispatcherComponent = null;
            _healthComponent = null;
        }

        [Test]
        public void Start_DefaultMaxHealthGreaterThanZero()
        {
            Assert.Greater(_healthComponent.GetMaxHealth(), 0);
        }

        [Test]
        public void Start_HasMaxHealth()
        {
            Assert.AreEqual(_healthComponent.GetCurrentHealth(), _healthComponent.GetMaxHealth());
        }

        [Test]
        public void AdjustHealth_ClampedToMax()
        {
            _healthComponent.AdjustHealth(100);
            Assert.AreEqual(_healthComponent.GetCurrentHealth(), _healthComponent.GetMaxHealth());
        }

        [Test]
        public void AdjustHealth_ChangesHealth()
        {
            var expectedHealthChange = _healthComponent.GetCurrentHealth() / 2;

            _healthComponent.AdjustHealth(-expectedHealthChange);

            Assert.AreEqual(_healthComponent.GetMaxHealth() - expectedHealthChange, _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void AdjustHealth_FiresHealthChangedMessage()
        {
            var expectedHealthChange = _healthComponent.GetCurrentHealth() / 2;

            var eventCapture = new UnityTestMessageHandleResponseObject<HealthChangedMessage>();

            var handle = _dispatcherComponent.GetUnityMessageEventDispatcher().RegisterForMessageEvent<HealthChangedMessage>
            (
                eventCapture.OnResponse
            );

            _healthComponent.AdjustHealth(-expectedHealthChange);

            Assert.AreEqual(_healthComponent.GetMaxHealth() - expectedHealthChange, eventCapture.MessagePayload.NewHealth);
            Assert.AreEqual(-expectedHealthChange, eventCapture.MessagePayload.HealthChange);

            _dispatcherComponent.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(handle);
        }

        [Test]
        public void AdjustHealth_HealthChangeDisabled_NoEffect()
        {
            _healthComponent.SetHealthChangedEnabled(false);

            _healthComponent.AdjustHealth(-1 *(_healthComponent.GetCurrentHealth() / 2));

            Assert.AreEqual(_healthComponent.GetMaxHealth(), _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void AdjustHealth_HealthChangeDisabled_NoEventFired()
        {
            var eventCapture = new UnityTestMessageHandleResponseObject<HealthChangedMessage>();

            var handle = _dispatcherComponent.GetUnityMessageEventDispatcher().RegisterForMessageEvent<HealthChangedMessage>
            (
                eventCapture.OnResponse
            );

            _healthComponent.SetHealthChangedEnabled(false);
            _healthComponent.AdjustHealth(-1 * (_healthComponent.GetCurrentHealth() / 2));

            Assert.IsFalse(eventCapture.ActionCalled);

            _dispatcherComponent.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(handle);
        }

        [Test]
        public void ReplenishHealth_SetsToMax()
        {
            _healthComponent.AdjustHealth(-1 * (_healthComponent.GetCurrentHealth() / 2));
            _healthComponent.ReplenishHealth();

            Assert.AreEqual(_healthComponent.GetMaxHealth(), _healthComponent.GetCurrentHealth());
        }

        [Test]
        public void ReplenishHealth_HealthChangeDisabled_NoEffect()
        {
            _healthComponent.SetHealthChangedEnabled(false);
            _healthComponent.AdjustHealth(-1 * (_healthComponent.GetCurrentHealth() / 2));
            _healthComponent.ReplenishHealth();

            Assert.AreEqual(_healthComponent.GetMaxHealth(), _healthComponent.GetCurrentHealth());
        }

        private TestHealthComponent _healthComponent;
        private TestUnityMessageEventDispatcherComponent _dispatcherComponent;
    }
}

#endif
