// Sifaka Game Studios (C) 2017

namespace Assets.Scripts.Components.Health
{
    public interface IHealthInterface
    {
        void AdjustHealth(int inChange);
        void SetHealthChangedEnabled(bool isEnabled);
        void ReplenishHealth();
        int GetCurrentHealth();
        int GetMaxHealth();
    }
}
