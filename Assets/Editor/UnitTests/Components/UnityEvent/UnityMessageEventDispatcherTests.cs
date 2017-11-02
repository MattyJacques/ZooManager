// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.UnityEvent;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.UnityEvent
{
    [TestFixture]
    public class UnityMessageEventDispatcherTestFixture {

        [Test]
        public void RegisterForMessageEventOfType_InvokeReceivesMessageWithExpectedArgs()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            var handle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);
            dispatcher.InvokeMessageEvent(new UnityTestMessagePayload(false));

            Assert.IsTrue(responseObject.ActionCalled);
            Assert.IsFalse(responseObject.MessagePayload.Flag);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void RegisterForMessageEventOfType_InvokeReceivesMessageWithExpectedArgsForAllRegisteredListeners()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var firstResponseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();
            var secondResponseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();
            var thirdResponseObject = new UnityTestMessageHandleResponseObject<OtherUnityTestMessagePayload>();

            var firstHandle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(firstResponseObject.OnResponse);
            var secondHandle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(secondResponseObject.OnResponse);
            var thirdHandle = dispatcher.RegisterForMessageEvent<OtherUnityTestMessagePayload>(thirdResponseObject.OnResponse);

            dispatcher.InvokeMessageEvent(new UnityTestMessagePayload());

            Assert.IsTrue(firstResponseObject.ActionCalled);
            Assert.IsTrue(secondResponseObject.ActionCalled);
            Assert.IsFalse(thirdResponseObject.ActionCalled);

            dispatcher.UnregisterForMessageEvent(thirdHandle);
            dispatcher.UnregisterForMessageEvent(secondHandle);
            dispatcher.UnregisterForMessageEvent(firstHandle);
        }

        [Test]
        public void RegisterForMessageEventOfType_InvokeDoesNotReceiveMessagesOfOtherTypes()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            var handle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);
            dispatcher.InvokeMessageEvent(new OtherUnityTestMessagePayload(false));

            Assert.IsFalse(responseObject.ActionCalled);

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void RegisterForMessageEventOfType_RegistersHandle()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            var handle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);

            Assert.IsTrue(handle.IsRegistered());

            dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void UnregisterForMessageEvent_UnregistersHandle()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            var handle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);

            dispatcher.UnregisterForMessageEvent(handle);
            Assert.IsFalse(handle.IsRegistered());
        }

        [Test]
        public void UnregisterForMessageEvent_NoLongerReceivesEvents()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            var handle = dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);

            dispatcher.UnregisterForMessageEvent(handle);

            dispatcher.InvokeMessageEvent(new UnityTestMessagePayload());
            Assert.IsFalse(responseObject.ActionCalled);
        }

        [Test]
        public void UnregisterNullHandle_ThrowsException()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            Assert.Throws<UnityMessageHandleException>(() => dispatcher.UnregisterForMessageEvent<UnityTestMessagePayload>(null));
        }

        [Test]
        public void UnregisterHandleNotRegistered_ThrowsException()
        {
            var dispatcher = new UnityMessageEventDispatcher();
            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();
            Assert.Throws<UnityMessageHandleException>(() => dispatcher.UnregisterForMessageEvent<UnityTestMessagePayload>(handle));
        }
    }
}

#endif
