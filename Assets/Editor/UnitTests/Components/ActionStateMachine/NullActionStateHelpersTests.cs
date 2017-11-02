// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Tests.Components.ActonStateMachine;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    [TestFixture]
    public class NullActionStateHelpersTestFixture
    {
        [Test]
        public void TransitionIntoNullActionState_SetsNullActionStateActive()
        {
            var actionStateMachineComponent = new GameObject().AddComponent<MockActionStateMachineComponent>();
            actionStateMachineComponent.Setup();

            NullActionStateHelpers.TransitionIntoNullActionState(actionStateMachineComponent.gameObject);

            Assert.AreEqual(EActionStateMachineTrack.Locomotion, actionStateMachineComponent.RequestedTrack);
            Assert.AreEqual(EActionStateId.Null, actionStateMachineComponent.RequestedState.ActionStateId);
            Assert.NotNull((NullActionState)actionStateMachineComponent.RequestedState);
        }
    }
}

#endif
