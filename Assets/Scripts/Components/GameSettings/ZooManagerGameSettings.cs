// Sifaka Game Studios (C) 2017

using Assets.Scripts.Core.Attributes;

namespace Assets.Scripts.Components.GameSettings
{
    public class ZooManagerGameSettings
    {
        [UnityReflectionAttribute(UnityReflectionAttributeType.FloatType)]
        public float SecondsPerDay = 100f;
    }
}
