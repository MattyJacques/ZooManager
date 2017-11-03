// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.Needs
{
    public class NeedParams
    {
        public NeedType AssignedNeedType { get; set; }
        public int MaxValue { get; set; }
        public float UpdateFrequency { get; set; }
        public int ValueDecay { get; set; }
        public AnimationCurve HealthAdjustmentCurve { get; set; }
    }

    public class Need
    {
        private readonly NeedParams _needParams;

        private int _currentValue;

        public Need(NeedParams inNeedParams)
        {
            _needParams = inNeedParams;
        }

        public void AdjustNeed(int inAdjustment)
        {
            _currentValue = Mathf.Clamp(_currentValue + inAdjustment, 0, _needParams.MaxValue);
        }

        public int GetHealthAdjustment()
        {
            return (int)_needParams.HealthAdjustmentCurve.Evaluate((_currentValue / _needParams.MaxValue) * 100);
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
