// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Assets.Scripts.Components.ActionStateMachine;
using Assets.Scripts.Tests.Components.ActonStateMachine;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    [TestFixture]
    public class ActionStateMachineComponentTestFixture
    {
        [SetUp]
        public void BeforeTest()
        {

            _actionStateMachineComponent = new GameObject().AddComponent<TestActionStateMachineComponent>();
            _actionStateMachineComponent.TestAwake();
        }

        [TearDown]
        public void AfterTest()
        {
            _actionStateMachineComponent = null;
        }

        [Test]
        public void WhenCreated_AllTracksHaveNullActionStateId()
        {
            var tracks = Enum.GetValues(typeof(EActionStateMachineTrack));

            foreach(EActionStateMachineTrack track in tracks)
            {
                Assert.IsTrue(_actionStateMachineComponent.IsActionStateActiveOnTrack(track, EActionStateId.Null));
            }
        }

        [Test]
        public void IsActionStateActiveOnTrack_WrongTrackCorrectId_False()
        {
            const EActionStateId expectedStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack wrongTrack = EActionStateMachineTrack.None;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            var actionState = new TestActionState(expectedStateId, new ActionStateInfo());

            _actionStateMachineComponent.RequestActionState(changedTrack, actionState);

            Assert.IsFalse(_actionStateMachineComponent.IsActionStateActiveOnTrack(wrongTrack, expectedStateId));
        }

        [Test]
        public void IsActionStateActiveOnTrack_CorrectTrackWrongId_False()
        {
            const EActionStateId wrongId = EActionStateId.Null;
            const EActionStateId expectedStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            var actionState = new TestActionState(expectedStateId, new ActionStateInfo());

            _actionStateMachineComponent.RequestActionState(changedTrack, actionState);

            Assert.IsFalse(_actionStateMachineComponent.IsActionStateActiveOnTrack(changedTrack, wrongId));
        }

        [Test]
        public void RequestActionState_SetsTrackToNewIdAndIsActionStateActiveReturnsTrue()
        {
            const EActionStateId expectedStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            var actionState = new TestActionState(expectedStateId, new ActionStateInfo());

            _actionStateMachineComponent.RequestActionState(changedTrack, actionState);

            Assert.IsTrue(_actionStateMachineComponent.IsActionStateActiveOnTrack(changedTrack, expectedStateId));
        }

        [Test]
        public void RequestActionState_SetsCallsEndOnPriorActionStateAndStartOnNewOne()
        {
            const EActionStateId newStateId = EActionStateId.Null;
            const EActionStateId oldStateId = EActionStateId.Locomotion;
            const EActionStateMachineTrack changedTrack = EActionStateMachineTrack.Locomotion;

            var oldActionState = new TestActionState(oldStateId, new ActionStateInfo());
            var newActionState = new TestActionState(newStateId, new ActionStateInfo());

            _actionStateMachineComponent.RequestActionState(changedTrack, oldActionState);
            _actionStateMachineComponent.RequestActionState(changedTrack, newActionState);

            Assert.IsTrue(oldActionState.OnEndCalled);
            Assert.IsTrue(newActionState.OnStartCalled);
        }

        [Test]
        public void Update_CallsUpdateOnActionState()
        {
            var actionState = new TestActionState(EActionStateId.Locomotion, new ActionStateInfo());

            _actionStateMachineComponent.RequestActionState(EActionStateMachineTrack.Locomotion, actionState);
            _actionStateMachineComponent.TestUpdate();

            Assert.IsTrue(actionState.OnUpdateCalled);
            Assert.AreEqual(actionState.OnUpdateValue, Time.deltaTime);
        }

        [Test]
        public void Destroyed_CallsEndOnAllStates()
        {
            var actionStates = new List<TestActionState>();

            var tracks = Enum.GetValues(typeof(EActionStateMachineTrack));
            foreach (EActionStateMachineTrack track in tracks)
            {
                var actionState = new TestActionState(EActionStateId.Locomotion, new ActionStateInfo());
                _actionStateMachineComponent.RequestActionState(track, actionState);
                actionStates.Add(actionState);
            }

            _actionStateMachineComponent.TestDestroy();

            foreach (var actionState in actionStates)
            {
                Assert.IsTrue(actionState.OnEndCalled);
            }
        }

        TestActionStateMachineComponent _actionStateMachineComponent;
    }
}

#endif
