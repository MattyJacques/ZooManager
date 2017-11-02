// Sifaka Game Studios (C) 2017

#if UNITY_EDITOR

using Assets.Scripts.Components.ActionStateMachine;

namespace Assets.Scripts.Tests.Components.ActonStateMachine
{
    public class TestActionStateMachineComponent 
        : ActionStateMachineComponent
    {
        public void TestAwake()
        {
            Awake();
        }

        public void TestUpdate()
        {
            Update();
        }

        public void TestDestroy()
        {
            OnDestroy();
        }
    }
}

#endif
