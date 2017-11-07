// Sifaka Game Studios (C) 2017

using Assets.Scripts.Core.Attributes;

namespace Assets.Scripts.GameSettings
{
    public static class GameSettingsConstants
    {
        public const string GameSettingsPath = "GameSettings/ZooManagerGameSettings";
    }

    public class ZooManagerGameSettings
    {
        [UnityReflection(UnityReflectionAttributeType.FloatType)]
        public float SecondsPerDay = 100f;
    }
}
