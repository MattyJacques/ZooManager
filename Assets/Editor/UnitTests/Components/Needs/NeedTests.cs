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
        public void AdjustNeed_UpdatesCurveValueCorrectly()
        {
            const int maxHealth = 100;
            const int adjustmentValue = -50;
            const int expectedHealthChange = -10;

            // Create curve with differing points to make sure it's not a simple straight line graph
            var curve = new AnimationCurve();
            curve.AddKey((((maxHealth + adjustmentValue)/maxHealth) * 100 ), expectedHealthChange);
            curve.AddKey(100, expectedHealthChange + 100);
            curve.AddKey(0, expectedHealthChange - 100);

            var need = new Need(maxHealth, 11f, curve);

            need.AdjustNeed(adjustmentValue);
            Assert.AreEqual(expectedHealthChange, need.GetHealthAdjustment());
        }

        [Test]
        public void GetUpdateFrequency_ReturnsAssignedUpdateFrequency()
        {
            const float expectedUpdateFrequency = 100f;
            
            var need = new Need(1, expectedUpdateFrequency, new AnimationCurve());

            Assert.AreEqual(expectedUpdateFrequency, need.GetUpdateFrequency());
        }
    }
}
