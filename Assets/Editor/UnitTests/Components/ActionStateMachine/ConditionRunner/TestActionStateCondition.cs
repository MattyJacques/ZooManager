// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine.ConditionRunner;

namespace Assets.Editor.UnitTests.Components.ActionStateMachine.ConditionRunner
{
    public class TestActionStateCondition
        : ActionStateCondition
    {
        public bool StartCalled { get; private set; }
        public bool UpdateCalled { get; private set; }
        public float UpdateDelta { get; private set; }
        public int EndCalls { get; private set; }

        public TestActionStateCondition()
            : base()
        {
            StartCalled = false;
            UpdateCalled = false;
            UpdateDelta = -1.0f;
            EndCalls = 0;
        }

        public override void Start()
        {
            StartCalled = true;
        }

        public override void Update(float deltaTime)
        {
            UpdateCalled = true;
            UpdateDelta = deltaTime;
        }

        public override void End()
        {
            EndCalls++;
        }

        public void ForceComplete()
        {
            Complete = true;
        }
    }
}

#endif
