// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using NUnit.Framework;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    [TestFixture]
    public class ActionStateTestFixture
    {
        [Test]
        public void ActionState_Start_OnStartCalled()
        {
            var actionState = new TestActionState();

            actionState.Start();

            Assert.IsTrue(actionState.OnStartCalled);
        }

        [Test]
        public void ActionState_Update_OnUpdateCalledWithDeltaTime()
        {
            var actionState = new TestActionState();
            const float expectedDelta = 12.1f;

            actionState.Update(expectedDelta);

            Assert.IsTrue(actionState.OnUpdateCalled);
            Assert.AreEqual(actionState.OnUpdateValue, expectedDelta);
        }

        [Test]
        public void ActionState_End_OnEndCalled()
        {
            var actionState = new TestActionState();

            actionState.End();

            Assert.IsTrue(actionState.OnEndCalled);
        }
    }
}

#endif
