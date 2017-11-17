// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine.ConditionRunner;
using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.ConditionRunner
{
    [TestFixture]
    public class ActionStateConditionRunnerTests
    {
        [Test]
        public void Complete_NoConditions_False()
        {
            var conditionRunner = new ActionStateConditionRunner();

            Assert.IsFalse(conditionRunner.IsComplete());
        }

        [Test]
        public void Complete_IncompleteCondition_False()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);

            Assert.IsFalse(conditionRunner.IsComplete());
        }

        [Test]
        public void AddActionStateCondition_CallsStartIfOneTrack()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);

            Assert.IsTrue(condition.StartCalled);
        }

        [Test]
        public void AddActionStateCondition_DoesNotCallStartIfNotOnRunningTrack()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.PushNewTrack();
            conditionRunner.AddCondition(condition);

            Assert.IsFalse(condition.StartCalled);
        }

        [Test]
        public void Update_CallsUpdateWithFloat()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();
            const float deltaTime = 12.0f;

            conditionRunner.AddCondition(condition);
            conditionRunner.Update(deltaTime);

            Assert.IsTrue(condition.UpdateCalled);
            Assert.AreEqual(deltaTime, condition.UpdateDelta);
        }

        [Test]
        public void Update_ConditionDoesNotComplete_CompleteFalse()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);
            conditionRunner.Update(1.0f);

            Assert.IsFalse(conditionRunner.IsComplete());
        }

        [Test]
        public void Update_ConditionDoesComplete_CompleteTrue()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);
            condition.ForceComplete();
            conditionRunner.Update(1.0f);

            Assert.IsTrue(conditionRunner.IsComplete());
        }

        [Test]
        public void Update_ConditionDoesComplete_CallsEnd()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);
            condition.ForceComplete();
            conditionRunner.Update(1.0f);

            Assert.AreEqual(1, condition.EndCalls);
        }

        [Test]
        public void ConditionCompletes_NeedsUpdateToBeNotified()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);
            condition.ForceComplete();

            Assert.IsFalse(conditionRunner.IsComplete());
        }

        [Test]
        public void ConditionCompletes_NeedsUpdateToCallEnd()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var condition = new TestActionStateCondition();

            conditionRunner.AddCondition(condition);
            condition.ForceComplete();

            Assert.AreEqual(0, condition.EndCalls);
        }

        [Test]
        public void Update_NeedsAllConditionsToComplete()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var firstCondition = new TestActionStateCondition();
            var secondCondition = new TestActionStateCondition();

            conditionRunner.AddCondition(firstCondition);
            conditionRunner.AddCondition(secondCondition);
            firstCondition.ForceComplete();

            conditionRunner.Update(1.0f);

            Assert.IsFalse(conditionRunner.IsComplete());
        }

        [Test]
        public void Update_OnlyEndsConditionOnceOnCompletion()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var firstCondition = new TestActionStateCondition();
            var secondCondition = new TestActionStateCondition();

            conditionRunner.AddCondition(firstCondition);
            conditionRunner.AddCondition(secondCondition);
            firstCondition.ForceComplete();

            conditionRunner.Update(1.0f);
            conditionRunner.Update(1.0f);

            Assert.AreEqual(1, firstCondition.EndCalls);
        }

        [Test]
        public void PushNewTrack_DoesNotUpdateNewTrackUntilPriorComplete()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var firstCondition = new TestActionStateCondition();
            var secondCondition = new TestActionStateCondition();

            conditionRunner.AddCondition(firstCondition);
            conditionRunner.PushNewTrack();
            conditionRunner.AddCondition(secondCondition);
            firstCondition.ForceComplete();
            secondCondition.ForceComplete();

            conditionRunner.Update(1.0f);

            Assert.IsFalse(conditionRunner.IsComplete());
        }

        [Test]
        public void PushNewTrack_UpdateAfterPriorCompletesTrack()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var firstCondition = new TestActionStateCondition();
            var secondCondition = new TestActionStateCondition();

            conditionRunner.AddCondition(firstCondition);
            conditionRunner.PushNewTrack();
            conditionRunner.AddCondition(secondCondition);
            firstCondition.ForceComplete();
            secondCondition.ForceComplete();

            conditionRunner.Update(1.0f);
            conditionRunner.Update(1.0f);

            Assert.IsTrue(conditionRunner.IsComplete());
        }

        [Test]
        public void Update_CallsStartOnConditionsOnceTheirTrackIsRunning()
        {
            var conditionRunner = new ActionStateConditionRunner();

            var firstCondition = new TestActionStateCondition();
            var secondCondition = new TestActionStateCondition();

            conditionRunner.AddCondition(firstCondition);
            conditionRunner.PushNewTrack();
            conditionRunner.AddCondition(secondCondition);
            firstCondition.ForceComplete();
            conditionRunner.Update(1.0f);
            conditionRunner.Update(1.0f);

            Assert.IsTrue(secondCondition.StartCalled);
        }
    }
}

#endif
