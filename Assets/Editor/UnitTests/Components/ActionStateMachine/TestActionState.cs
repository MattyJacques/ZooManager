// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine
{
    public class TestActionState : ActionState
    {
        public TestActionState()
            : this(EActionStateId.Null, new ActionStateInfo())
        {
        }

        public TestActionState(EActionStateId inActionStateId, ActionStateInfo inActionStateInfo)
            : base(inActionStateId, inActionStateInfo)
        {
            OnStartCalled = false;
            OnUpdateCalled = false;
            OnEndCalled = false;
        }

        protected override void OnStart()
        {
            OnStartCalled = true;
        }

        protected override void OnUpdate(float deltaTime)
        {
            OnUpdateCalled = true;
            OnUpdateValue = deltaTime;
        }

        protected override void OnEnd()
        {
            OnEndCalled = true;
        }

        public bool OnStartCalled { get; set; }
        public bool OnUpdateCalled { get; set; }
        public float ? OnUpdateValue { get; set; }
        public bool OnEndCalled { get; set; }
    }
}

#endif
