// Sifaka Game Studios (C) 2017

using Assets.Scripts.Components.UnityEvent;

namespace Assets.Scripts.Components.ActionStateMachine.States.Dead
{
    public class DeadActionState
        : ActionState
    {
        private IUnityMessageEventInterface DispatcherInterface { get; set; }

        public DeadActionState(ActionStateInfo inInfo) : base(EActionStateId.Dead, inInfo)
        {
        }

        protected override void OnStart()
        {
            DispatcherInterface = Info.Owner.GetComponent<IUnityMessageEventInterface>();
            DispatcherInterface.GetUnityMessageEventDispatcher().InvokeMessageEvent(new EnteredDeadActionStateMessage());
        }

        protected override void OnUpdate(float deltaTime)
        {
        }

        protected override void OnEnd()
        {
            DispatcherInterface.GetUnityMessageEventDispatcher().InvokeMessageEvent(new LeftDeadActionStateMessage());
        }
    }
}
