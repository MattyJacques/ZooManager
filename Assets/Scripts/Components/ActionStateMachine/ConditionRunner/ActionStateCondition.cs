// Sifaka Game Studios (C) 2017

namespace Assets.Scripts.Components.ActionStateMachine.ConditionRunner
{
    public abstract class ActionStateCondition
    {
        protected ActionStateCondition()
        {
            Complete = false;
        }

        public abstract void Start();
        public abstract void Update(float deltaTime);
        public abstract void End();

        public bool Complete { get; protected set; }
    }
}
