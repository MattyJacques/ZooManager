// Sifaka Game Studios (C) 2017

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Assets.Scripts.Components.UnityEvent
{
    public class UnityMessageEventDispatcher
    {
        public UnityMessageEventDispatcher()
        {
            _registeredEvents = new Dictionary<Type, object>();
        }

        public UnityMessageEventHandle<TMessageType> RegisterForMessageEvent<TMessageType>(UnityAction<TMessageType> inAction)
            where TMessageType : UnityMessagePayload
        {
            var messageType = typeof(TMessageType);
            if (!_registeredEvents.ContainsKey(messageType))
            {
                _registeredEvents.Add(typeof(TMessageType), new UnityMessageEvent<TMessageType>());
            }

            var messageEvent = (UnityMessageEvent<TMessageType>)_registeredEvents[messageType];
            var messageHandle = new UnityMessageEventHandle<TMessageType>();

            messageHandle.RegisterHandle(messageEvent, inAction);

            return messageHandle;
        }

        public void UnregisterForMessageEvent<TMessageType>(UnityMessageEventHandle<TMessageType> inHandle)
            where TMessageType : UnityMessagePayload
        {
            if (inHandle == null)
            {
                throw new UnityMessageHandleException("Null handle on unregistration!");
            }
            if (!inHandle.IsRegistered())
            {
                throw new UnityMessageHandleException("handle was not registered!");
            }

            var messageType = typeof(TMessageType);
            if (!_registeredEvents.ContainsKey(messageType))
            {
                throw new UnityMessageHandleException("Handle was registered to other dispatcher!");
            }

            var messageEvent = (UnityMessageEvent<TMessageType>)_registeredEvents[typeof(TMessageType)];
            inHandle.UnregisterHandle(messageEvent);
        }

        public void InvokeMessageEvent<TMessageType>(TMessageType inMessage)
            where TMessageType : UnityMessagePayload
        {
            var messageType = typeof(TMessageType);
            if (_registeredEvents.ContainsKey(messageType))
            {
                var messageEvent = (UnityMessageEvent<TMessageType>) _registeredEvents[messageType];
                messageEvent.Invoke(inMessage);
            }
        }

        private readonly IDictionary<Type, object> _registeredEvents;
    }
}
