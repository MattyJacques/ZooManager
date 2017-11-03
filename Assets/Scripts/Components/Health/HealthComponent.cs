// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;
using UnityEngine;

namespace Assets.Scripts.Components.Health
{
    public class HealthComponent 
        : MonoBehaviour
        , IHealthInterface
    {
        public int MaxHealth = 100;

        private bool _healthChangeEnabled;
        private int _currentHealth;
        private int CurrentHealth { get { return _currentHealth; } set { OnHealthChanged(value);} }

        protected void Start ()
        {
            _currentHealth = MaxHealth;
            _healthChangeEnabled = true;
        }

        public void AdjustHealth(int inChange)
        {
            CurrentHealth += inChange;
        }

        public void SetHealthChangedEnabled(bool isEnabled)
        {
            _healthChangeEnabled = isEnabled;
        }

        public void ReplenishHealth()
        {
            CurrentHealth = MaxHealth;
        }

        public int GetCurrentHealth()
        {
            return CurrentHealth;
        }

        public int GetMaxHealth()
        {
            return MaxHealth;
        }

        private bool CanAdjustHealth()
        {
            return _healthChangeEnabled;
        }

        private void OnHealthChanged(int newHealth)
        {
            if (CanAdjustHealth())
            {
                var healthChange = newHealth - _currentHealth;
                _currentHealth = newHealth;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, GetMaxHealth());

                UnityMessageEventFunctions.InvokeMessageEventWithDispatcher(gameObject, new HealthChangedMessage(healthChange, _currentHealth));
            }
        }
    }
}
