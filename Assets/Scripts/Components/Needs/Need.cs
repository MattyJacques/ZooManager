// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.Needs
{
    public class Need
    {
        private readonly int _maxValue;
        private readonly float _updateFrequency;
        private readonly AnimationCurve _healthAdjustmentCurve;

        private int _currentValue;

        public Need(int inMaxValue, float inUpdateFrequency, AnimationCurve inHealthAdjustmentCurve)
        {
            _maxValue = inMaxValue;
            _currentValue = inMaxValue;

            _updateFrequency = inUpdateFrequency;

            _healthAdjustmentCurve = inHealthAdjustmentCurve;
        }

        public void AdjustNeed(int inAdjustment)
        {
            _currentValue = Mathf.Clamp(_currentValue + inAdjustment, 0, _maxValue);
        }

        public int GetHealthAdjustment()
        {
            return (int)_healthAdjustmentCurve.Evaluate((_currentValue / _maxValue) * 100);
        }

        public float GetUpdateFrequency()
        {
            return _updateFrequency;
        }
    }
}
