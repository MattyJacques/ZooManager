// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Components.ActionStateMachine.States.Dead;
using Assets.Scripts.Tests.Components.ActonStateMachine;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.States.Dead
{
    [TestFixture]
    public class DeadActionStateHelpersTestFixture
    {
        [Test]
        public void TransitionIntoDeadActionState_SetsDeadActionStateActive()
        {
            var actionStateMachineComponent = new GameObject().AddComponent<MockActionStateMachineComponent>();

            DeadActionStateHelpers.TransitionIntoDeadActionState(actionStateMachineComponent.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, actionStateMachineComponent.RequestedTrack);
            Assert.AreEqual(EActionStateId.Dead, actionStateMachineComponent.RequestedState.ActionStateId);
            Assert.NotNull((DeadActionState)actionStateMachineComponent.RequestedState);
        }
    }
}

#endif
