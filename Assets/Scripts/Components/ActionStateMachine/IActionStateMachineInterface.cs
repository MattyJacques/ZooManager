// Sifaka Game Studios (C) 2017

namespace Assets.Scripts.Components.ActionStateMachine
{
    public interface IActionStateMachineInterface
    {
        void RequestActionState(EActionStateMachineTrack selectedTrack, ActionState newState);

        bool IsActionStateActiveOnTrack(EActionStateMachineTrack selectedTrack, EActionStateId expectedId);
    }
}
