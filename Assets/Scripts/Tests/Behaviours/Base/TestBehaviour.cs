// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;

namespace Assets.Scripts.Tests.Behaviours.Base
{
    public class TestBehaviour
        : BehaviourBase
    {
        public ReturnCode ReturnCodeResult { get; set; }
        public bool BehaveCalled { get; private set; }

        public TestBehaviour(ReturnCode inReturnCodeResult)
        {
            BehaveCalled = false;
            ReturnCodeResult = inReturnCodeResult;
        }

        public override IEnumerator Behave(Blackboard inBlackboard, Action<ReturnCode> returnCode)
        {
            BehaveCalled = true;
            returnCode(ReturnCodeResult);
            yield break;
        }
    }
}
