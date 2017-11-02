// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.UnityEvent;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.UnityEvent
{
    [TestFixture]
    public class UnityEventMessageHandleTestFixture
    {
        [Test]
        public void RegisterHandle_NullEvent_ThrowsException()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();
            Assert.Throws<UnityMessageHandleException>(() => handle.RegisterHandle(null, responseObject.OnResponse)); 
        }

        [Test]
        public void RegisterHandle_NullAction_ThrowsException()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            Assert.Throws<UnityMessageHandleException>(() => handle.RegisterHandle(new UnityMessageEvent<UnityTestMessagePayload>(), null));
        }

        [Test]
        public void RegisterHandle_ActionIsAddedAsListener()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            var unityEvent = new UnityMessageEvent<UnityTestMessagePayload>();

            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            handle.RegisterHandle(unityEvent, responseObject.OnResponse);
            unityEvent.Invoke(new UnityTestMessagePayload());
            Assert.IsTrue(responseObject.ActionCalled);
            handle.UnregisterHandle(unityEvent);
        }

        [Test]
        public void NewHandle_IsRegistered_False()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            Assert.IsFalse(handle.IsRegistered());
        }

        [Test]
        public void RegisterHandle_IsRegistered_True()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            var unityEvent = new UnityMessageEvent<UnityTestMessagePayload>();

            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            handle.RegisterHandle(unityEvent, responseObject.OnResponse);
            Assert.IsTrue(handle.IsRegistered());
            handle.UnregisterHandle(unityEvent);
        }

        [Test]
        public void UnregisterHandle_IsRegistered_False()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            var unityEvent = new UnityMessageEvent<UnityTestMessagePayload>();

            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            handle.RegisterHandle(unityEvent, responseObject.OnResponse);
            handle.UnregisterHandle(unityEvent);
            Assert.IsFalse(handle.IsRegistered());
        }

        [Test]
        public void UnregisterHandle_NullEvent_ThrowsException()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            var unityEvent = new UnityMessageEvent<UnityTestMessagePayload>();

            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            handle.RegisterHandle(unityEvent, responseObject.OnResponse);
            
            Assert.Throws<UnityMessageHandleException>(() => handle.UnregisterHandle(null));
            handle.UnregisterHandle(unityEvent);
        }

        [Test]
        public void UnregisterHandle_ActionIsRemovedAsListener()
        {
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            var unityEvent = new UnityMessageEvent<UnityTestMessagePayload>();

            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            handle.RegisterHandle(unityEvent, responseObject.OnResponse);
            handle.UnregisterHandle(unityEvent);

            unityEvent.Invoke(new UnityTestMessagePayload());
            Assert.IsFalse(responseObject.ActionCalled);
        }
    }
}

#endif
