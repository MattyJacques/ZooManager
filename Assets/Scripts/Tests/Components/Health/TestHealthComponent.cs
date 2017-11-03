// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.Health;

namespace Assets.Scripts.Tests.Components.Health
{
    public class TestHealthComponent 
        : HealthComponent
    {
        public void TestStart()
        {
            Start();
        }
    }
}

#endif
