// Sifaka Game Studios (C) 2017

namespace Assets.Scripts.Components.ActionStateMachine
{
    public abstract class ActionState
    {
        protected ActionState(EActionStateId inActionStateId, ActionStateInfo inInfo)
        {
            ActionStateId = inActionStateId;
            Info = inInfo;
        }

        public void Start()
        {
            OnStart();
        }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public void End()
        {
            OnEnd();
        }

        protected abstract void OnStart();
        protected abstract void OnUpdate(float deltaTime);
        protected abstract void OnEnd();

        public EActionStateId ActionStateId { get; private set; }
        protected ActionStateInfo Info { get; set; }
    }
}
