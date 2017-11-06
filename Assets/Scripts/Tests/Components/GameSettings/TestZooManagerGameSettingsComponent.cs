// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.GameSettings;

namespace Assets.Scripts.Tests.Components.GameSettings
{
    public class TestZooManagerGameSettingsComponent : ZooManagerGameSettingsComponent
    {
        public void TestAwake()
        {
            SettingsPath = "Tests/TestZooManagerGameSettings";
            Awake();
        }
    }
}

#endif
