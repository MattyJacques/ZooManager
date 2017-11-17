// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.UnityEvent;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.UnityEvent
{
    [TestFixture]
    public class UnityMessageEventFunctionsTestFixture {

        [Test]
        public void RegisterMessageEvent_NullDispatcher_ExceptionThrown()
        {
            TestUnityMessageEventDispatcherInterface dispatcherInterface = null;
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            Assert.Throws<UnityMessageHandleException>(() =>
                UnityMessageEventFunctions.RegisterActionWithDispatcher<UnityTestMessagePayload>(dispatcherInterface,
                    responseObject.OnResponse));
        }

        [Test]
        public void RegisterMessageEvent_RegistersMessageEvent()
        {
            var dispatcherInterface = new TestUnityMessageEventDispatcherInterface();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();

            var handle = UnityMessageEventFunctions.RegisterActionWithDispatcher<UnityTestMessagePayload>(dispatcherInterface, responseObject.OnResponse);

            Assert.IsTrue(handle.IsRegistered());

            dispatcherInterface.Dispatcher.UnregisterForMessageEvent(handle);
        }

        [Test]
        public void UnregisterMessageEvent_NullDispatcher_ExceptionThrown()
        {
            TestUnityMessageEventDispatcherInterface dispatcherInterface = null;

            var handle = new UnityMessageEventHandle<UnityTestMessagePayload>();

            Assert.Throws<UnityMessageHandleException>(() =>
                UnityMessageEventFunctions.UnregisterActionWithDispatcher(dispatcherInterface,
                    handle));
        }

        [Test]
        public void UnregisterMessageEvent_UnregistersEvent()
        {
            var dispatcherInterface = new TestUnityMessageEventDispatcherInterface();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();
            var handle = dispatcherInterface.Dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);

            UnityMessageEventFunctions.UnregisterActionWithDispatcher(dispatcherInterface, handle);

            Assert.IsFalse(handle.IsRegistered());
        }

        [Test]
        public void InvokeMessageEvent_NullDispatcher_ExceptionThrown()
        {
            TestUnityMessageEventDispatcherInterface dispatcherInterface = null;

            Assert.Throws<UnityMessageHandleException>(() =>
                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(dispatcherInterface,
                    new UnityTestMessagePayload()));
        }

        [Test]
        public void InvokeMessageEventWithDispatcher_InvokesEvent()
        {
            var dispatcherInterface = new TestUnityMessageEventDispatcherInterface();
            var responseObject = new UnityTestMessageHandleResponseObject<UnityTestMessagePayload>();
            var handle = dispatcherInterface.Dispatcher.RegisterForMessageEvent<UnityTestMessagePayload>(responseObject.OnResponse);
            
            UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(dispatcherInterface, new UnityTestMessagePayload());

            Assert.IsTrue(responseObject.ActionCalled);

            dispatcherInterface.Dispatcher.UnregisterForMessageEvent(handle);
        }
    }
}

#endif
