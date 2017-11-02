// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.UnityEvent
{
    public class UnityMessageEventDispatcherComponent 
        : MonoBehaviour
        , IUnityMessageEventInterface
    {

        private UnityMessageEventDispatcher Dispatcher { get; set; }

        protected void Awake ()
        {
            Dispatcher = new UnityMessageEventDispatcher();
        }

        // IUnityMessageEventInterface
        public UnityMessageEventDispatcher GetUnityMessageEventDispatcher()
        {
            return Dispatcher;
        }
        // ~IUnityMessageEventInterface
    }
}
