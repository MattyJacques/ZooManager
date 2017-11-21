// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Services;

namespace Assets.Scripts.Tests.Services
{
    public class TestGameServiceProvider 
        : GameServiceProvider
    {
        public void TestAwake()
        {
            Awake();
        }
    }
}

#endif // UNITY_EDITOR
