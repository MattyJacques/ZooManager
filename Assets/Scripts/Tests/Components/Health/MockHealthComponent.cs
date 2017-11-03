// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.Health;
using UnityEngine;

namespace Assets.Scripts.Tests.Components.Health
{
    public class MockHealthComponent 
        : MonoBehaviour
        , IHealthInterface
    {
        public int ? AdjustHealthResult { get; private set; }
        public bool ? SetHealthChangeEnabledResult { get; private set; }

        // IHealthInterface
        public void AdjustHealth(int inChange)
        {
            AdjustHealthResult = inChange;
        }

        public void SetHealthChangedEnabled(bool isEnabled)
        {
            SetHealthChangeEnabledResult = isEnabled;
        }

        public void ReplenishHealth()
        {
        }

        public int GetCurrentHealth()
        {
            return 1;
        }

        public int GetMaxHealth()
        {
            return 1;
        }
        // ~IHealthInterface
    }
}

#endif
