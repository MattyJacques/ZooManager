// Sifaka Game Studios (C) 2017

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components.UnityEvent
{
    public static class UnityMessageEventFunctions
    {
        public static UnityMessageEventHandle<TMessageType> RegisterActionWithDispatcher<TMessageType>
        (
            GameObject inDispatcherObject, UnityAction<TMessageType> inAction
        )
            where TMessageType : UnityMessagePayload
        {
            return RegisterActionWithDispatcher(inDispatcherObject.GetComponent<IUnityMessageEventInterface>(),
                inAction);
        }

        public static UnityMessageEventHandle<TMessageType> RegisterActionWithDispatcher<TMessageType>
        (
            IUnityMessageEventInterface inDispatcherInterface, UnityAction<TMessageType> inAction
        )
            where TMessageType : UnityMessagePayload
        {
            if (inDispatcherInterface == null)
            {
                throw new UnityMessageHandleException("Invalid dispatcher interface passed in for registration!");    
            }

            return inDispatcherInterface.GetUnityMessageEventDispatcher().RegisterForMessageEvent(inAction);
        }

        public static void UnregisterActionWithDispatcher<TMessageType>
        (
            GameObject inDispatcherObject, UnityMessageEventHandle<TMessageType> inHandle
        )
            where TMessageType : UnityMessagePayload
        {
            UnregisterActionWithDispatcher(inDispatcherObject.GetComponent<IUnityMessageEventInterface>(), inHandle);
        }

        public static void UnregisterActionWithDispatcher<TMessageType>
        (
            IUnityMessageEventInterface inDispatcherInterface, UnityMessageEventHandle<TMessageType> inHandle
        )
            where TMessageType : UnityMessagePayload
        {
            if (inDispatcherInterface == null)
            {
                throw new UnityMessageHandleException("Invalid dispatcher interface passed in for unregistration!");
            }

            inDispatcherInterface.GetUnityMessageEventDispatcher().UnregisterForMessageEvent(inHandle);
        }

        public static void InvokeMessageEventWithDispatcher<TMessageType>
        (
            IUnityMessageEventInterface inDispatcherInterface,
            TMessageType inMessage
        )
            where TMessageType : UnityMessagePayload
        {
            if (inDispatcherInterface == null)
            {
                throw new UnityMessageHandleException("Invalid dispatcher interface passed in for invoking event!");
            }

            inDispatcherInterface.GetUnityMessageEventDispatcher().InvokeMessageEvent(inMessage);
        }

        public static void InvokeMessageEventWithDispatcher<TMessageType>
        (
            GameObject inDispatcherObject,
            TMessageType inMessage
        )
            where TMessageType : UnityMessagePayload
        {
            InvokeMessageEventWithDispatcher(inDispatcherObject.GetComponent<IUnityMessageEventInterface>(), inMessage);
        }
    }
}
