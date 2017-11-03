// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.Needs;
using Assets.Scripts.Tests.Components.Health;
using Assets.Scripts.Tests.Components.Needs;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Editor.UnitTests.Components.Needs
{
    [TestFixture]
    public class NeedsComponentTestFixture
    {
        private MockHealthComponent _healthComponent;
        private TestNeedsComponent _needsComponent;

        private AnimationCurve _animationCurve;
        private Need _need;

        private const int MaxValue = 100;
        private const int DecayValue = 20;

        private const int HealthAdjustmentBeforeDecay = 50;
        private const int HealthAdjustmentAfterDecay = 20;

        [SetUp]
        public void BeforeTest()
        {
            _healthComponent = new GameObject().AddComponent<MockHealthComponent>();

            _needsComponent = _healthComponent.gameObject.AddComponent<TestNeedsComponent>();
            _needsComponent.TestStart();

            _animationCurve = new AnimationCurve();
            _animationCurve.AddKey(100, HealthAdjustmentBeforeDecay);
            _animationCurve.AddKey((((MaxValue - DecayValue) / MaxValue) * 100), HealthAdjustmentAfterDecay);

            _need = new Need
            ( 
                new NeedParams{AssignedNeedType = NeedType.Hunger, MaxValue = MaxValue, ValueDecay = DecayValue, UpdateFrequency = 1.0f, HealthAdjustmentCurve = _animationCurve}
            );
        }

        [TearDown]
        public void AfterTest()
        {
            _need = null;
            _animationCurve = null;

            _needsComponent = null;
            _healthComponent = null;
        }
 
        [Test]
        public void AddNeed_UpdateForLessThanDecay_DoesNotAdjustHealthByDecay()
        {
            _needsComponent.AddNeed(_need);

            _needsComponent.TestUpdate(_need.GetUpdateFrequency() * 0.5f);

            Assert.IsNull(_healthComponent.AdjustHealthResult);
        }

        [Test]
        public void AddNeed_UpdateForDecay_AdjustsHealthByResult()
        {
            _needsComponent.AddNeed(_need);

            _needsComponent.TestUpdate(_need.GetUpdateFrequency() + 0.1f);

            Assert.AreEqual(_healthComponent.AdjustHealthResult, _need.GetHealthAdjustment());
        }

        [Test]
        public void AddNeed_UpdateForDecay_DecaysNeed()
        {
            _needsComponent.AddNeed(_need);

            _needsComponent.TestUpdate(_need.GetUpdateFrequency() + 0.1f);

            Assert.AreEqual(_healthComponent.AdjustHealthResult, HealthAdjustmentAfterDecay);
        }

        [Test]
        public void AddNeed_UpdateForNTimesDecay_AdjustsHealthByNTimesDecay()
        {
            _needsComponent.AddNeed(_need);

            const int updateNum = 3;

            for (int i = 0; i < updateNum; ++i)
            {
                _needsComponent.TestUpdate(_need.GetUpdateFrequency() + 0.1f);
                Assert.AreEqual(_healthComponent.AdjustHealthResult, HealthAdjustmentAfterDecay);

                // Reset to make sure it updates the next time again
                _healthComponent.AdjustHealthResult = HealthAdjustmentBeforeDecay;
            }
        }

        [Test]
        public void AddNeed_MultipleNeedsExist_AdjustsHealthAsExpected()
        {
            _needsComponent.AddNeed(_need);

            var otherNeed = new Need
            (
                new NeedParams { AssignedNeedType = NeedType.Thirst, MaxValue = MaxValue, ValueDecay = DecayValue, UpdateFrequency = _need.GetUpdateFrequency() * 2, HealthAdjustmentCurve  = _animationCurve}
            );

            _needsComponent.AddNeed(otherNeed);

            _needsComponent.TestUpdate(_need.GetUpdateFrequency() + 0.1f);

            Assert.AreEqual(_healthComponent.AdjustHealthResult, HealthAdjustmentAfterDecay);
        }

        [Test]
        public void AddNeed_NeedAlreadyExists_ThrowsError()
        {
            LogAssert.Expect(LogType.Error, "Tried to add NeedType that already existed!");

            _needsComponent.AddNeed(_need);
            _needsComponent.AddNeed(_need);
        }
    }
}
