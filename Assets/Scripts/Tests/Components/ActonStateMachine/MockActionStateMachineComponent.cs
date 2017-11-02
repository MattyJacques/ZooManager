// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;
using UnityEngine;

namespace Assets.Scripts.Tests.Components.ActonStateMachine
{
    public class MockActionStateMachineComponent 
        : MonoBehaviour
        , IActionStateMachineInterface
    {
        public EActionStateMachineTrack RequestedTrack { get; private set; }
        public ActionState RequestedState { get; private set; }
        public bool IsActionStateActiveResult { get; set; }

        public void Setup()
        {
            RequestedTrack = EActionStateMachineTrack.None;
            RequestedState = null;
            IsActionStateActiveResult = false;
        }

        public void RequestActionState(EActionStateMachineTrack selectedTrack, ActionState newState)
        {
            RequestedTrack = selectedTrack;
            RequestedState = newState;
        }

        public bool IsActionStateActiveOnTrack(EActionStateMachineTrack selectedTrack, EActionStateId expectedId)
        {
            return IsActionStateActiveResult;
        }
    }
}

#endif
