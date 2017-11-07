// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Needs;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.Needs
{
    [TestFixture]
    public class NeedTestFixture
    {
        [Test]
        public void Created_NeedValueIsMax()
        {
            const int maxValue = 100;
            const int adjustmentValue = -50;
            const int expectedValueChange = -10;

            // Create curve with differing points to make sure it's not a simple straight line graph
            var curve = new AnimationCurve();
            curve.AddKey((((maxValue + adjustmentValue) / maxValue) * 100), expectedValueChange);
            curve.AddKey(100, expectedValueChange + 100);
            curve.AddKey(0, expectedValueChange - 100);

            var need = new Need
            (
                new NeedParams { MaxValue = maxValue, HealthAdjustmentCurve = curve }
            );

            Assert.AreEqual(maxValue, need.CurrentValue);
        }

        [Test]
        public void AdjustNeed_UpdatesCurveValueCorrectly()
        {
            const int maxValue = 100;
            const int adjustmentValue = -50;
            const int expectedValueChange = -10;

            // Create curve with differing points to make sure it's not a simple straight line graph
            var curve = new AnimationCurve();
            curve.AddKey((((maxValue + adjustmentValue)/maxValue) * 100 ), expectedValueChange);
            curve.AddKey(100, expectedValueChange + 100);
            curve.AddKey(0, expectedValueChange - 100);

            var need = new Need
            (
                new NeedParams { MaxValue = maxValue, HealthAdjustmentCurve = curve }
            );

            need.AdjustNeed(adjustmentValue);
            Assert.AreEqual(expectedValueChange, need.GetHealthAdjustment());
        }

        [Test]
        public void GetUpdateFrequency_ReturnsAssignedUpdateFrequency()
        {
            const float expectedUpdateFrequency = 100f;
            
            var need = new Need
            (
                new NeedParams { UpdateFrequency =  expectedUpdateFrequency }
            );

            Assert.AreEqual(expectedUpdateFrequency, need.GetUpdateFrequency());
        }

        [Test]
        public void GetCurrentValue_ReflectsExpectedResult()
        {
            const int maxValue = 100;
            const int valueDecay = 2;

            var need = new Need
            (
                new NeedParams { MaxValue = maxValue, ValueDecay = valueDecay}
            );

            need.Decay();

            Assert.AreEqual(maxValue - valueDecay, need.CurrentValue);
        }

        [Test]
        public void GetNeedType_ReturnsNeedType()
        {
            const NeedType expectedNeedType = NeedType.Hunger;

            var need = new Need
            (
                new NeedParams { AssignedNeedType =  expectedNeedType }
            );

            Assert.AreEqual(expectedNeedType, need.GetNeedType());
        }

        [Test]
        public void Decay_UpdatesCurveValueCorrectly()
        {
            const int maxValue = 100;
            const int adjustmentValue = 50;
            const int expectedValueChange = -10;

            // Create curve with differing points to make sure it's not a simple straight line graph
            var curve = new AnimationCurve();
            curve.AddKey((((maxValue - adjustmentValue) / maxValue) * 100), expectedValueChange);
            curve.AddKey(100, expectedValueChange + 100);
            curve.AddKey(0, expectedValueChange - 100);

            var need = new Need
            (
                new NeedParams { MaxValue = maxValue, ValueDecay = adjustmentValue, HealthAdjustmentCurve = curve }
            );

            need.Decay();
            Assert.AreEqual(expectedValueChange, need.GetHealthAdjustment());
        }
    }
}
