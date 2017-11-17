// Sifaka Game Studios (C) 2017

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine
{
    public class ActionStateMachineComponent : MonoBehaviour
      , IActionStateMachineInterface
    {
        protected void Awake()
        {
            // Initialise all tracks to null
            _activeActionStates = new Dictionary<EActionStateMachineTrack, ActionState>();

            foreach (EActionStateMachineTrack track in Enum.GetValues(typeof(EActionStateMachineTrack)))
            {
                _activeActionStates.Add(track, new NullActionState());
            }
        }

        protected void Update()
        {
            foreach (var activeActionState in _activeActionStates)
            {
                activeActionState.Value.Update(Time.deltaTime);
            }
        }

        protected void OnDestroy()
        {
            foreach (var actionState in _activeActionStates)
            {
                actionState.Value.End();
            }
        }

        // IActionStateMachineInterface
        public virtual void RequestActionState(EActionStateMachineTrack selectedTrack, ActionState newState)
        {
            _activeActionStates[selectedTrack].End();
            _activeActionStates[selectedTrack] = newState;
            newState.Start();
        }

        public virtual bool IsActionStateActiveOnTrack(EActionStateMachineTrack selectedTrack, EActionStateId expectedId)
        {
            return _activeActionStates[selectedTrack].ActionStateId == expectedId;
        }
        // ~IActionStateMachineInterface

        private Dictionary<EActionStateMachineTrack, ActionState> _activeActionStates;
    }
}
