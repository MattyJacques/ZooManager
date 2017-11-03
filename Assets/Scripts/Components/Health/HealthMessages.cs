// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;

namespace Assets.Scripts.Components.Health
{
    [System.Serializable]
    public class HealthChangedMessage
        : UnityMessagePayload
    {
        public HealthChangedMessage(int inHealthChange, int inNewHealth)
        {
            HealthChange = inHealthChange;
            NewHealth = inNewHealth;
        }

        public readonly int HealthChange;
        public readonly int NewHealth;
    }
}
