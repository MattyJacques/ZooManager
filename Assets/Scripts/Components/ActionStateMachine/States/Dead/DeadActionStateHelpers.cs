// Sifaka Game Studios (C) 2017

using UnityEngine;

namespace Assets.Scripts.Components.ActionStateMachine.States.Dead
{
    public static class DeadActionStateHelpers
    {
        public static void TransitionIntoDeadActionState(GameObject inGameObject)
        {
            if (inGameObject != null)
            {
                var actionStateMachineInterface = inGameObject.GetComponent<IActionStateMachineInterface>();
                if (actionStateMachineInterface != null)
                {
                    actionStateMachineInterface.RequestActionState
                    (
                        EActionStateMachineTrack.Locomotion,
                        new DeadActionState(new ActionStateInfo(inGameObject))
                    );
                }
            }
        }
    }
}
