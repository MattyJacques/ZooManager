// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.Needs
{
    [System.Serializable]
    public struct NeedParams
    {
        public NeedType AssignedNeedType;
        public int MaxValue;
        public float UpdateFrequency;
        public int ValueDecay;
        public AnimationCurve HealthAdjustmentCurve;
    }

    public class Need
    {
        private readonly NeedParams _needParams;

        public int CurrentValue { get; private set; }

        public Need(NeedParams inNeedParams)
        {
            _needParams = inNeedParams;
            CurrentValue = inNeedParams.MaxValue;
        }

        public void AdjustNeed(int inAdjustment)
        {
            CurrentValue = Mathf.Clamp(CurrentValue + inAdjustment, 0, _needParams.MaxValue);
        }

        public int GetHealthAdjustment()
        {
            return (int)_needParams.HealthAdjustmentCurve.Evaluate((CurrentValue / _needParams.MaxValue) * 100);
        }

        public float GetUpdateFrequency()
        {
            return _needParams.UpdateFrequency;
        }

        public void Decay()
        {
            AdjustNeed(-_needParams.ValueDecay);
        }

        public NeedType GetNeedType()
        {
            return _needParams.AssignedNeedType;
        }
    }
}
