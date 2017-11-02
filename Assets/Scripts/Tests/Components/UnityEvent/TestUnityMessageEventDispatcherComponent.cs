// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.UnityEvent;

namespace Assets.Scripts.Tests.Components.UnityEvent
{
    public class TestUnityMessageEventDispatcherComponent
        : UnityMessageEventDispatcherComponent
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif
