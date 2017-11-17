// Sifaka Game Studios (C) 2017

using System;

namespace Assets.Scripts.Components.UnityEvent
{
    public class UnityMessageHandleException 
        : Exception
    {
        public UnityMessageHandleException(string message)
            : base(message)
        {
        }
    }
}
