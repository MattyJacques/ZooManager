// Sifaka Game Studios (C) 2017

using UnityEngine.Events;

namespace Assets.Scripts.Components.UnityEvent
{
    [System.Serializable]
    public abstract class UnityMessagePayload
    {
    }

    [System.Serializable]
    public class UnityMessageEvent<TMessageType>
        : UnityEvent<TMessageType>
        where TMessageType : UnityMessagePayload
    {     
    }
}
