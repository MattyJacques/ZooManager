// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.BehaviourTree.Base;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Tests.BehaviourTree.Base
{
    public class TestBehaviour
        : BehaviourBase
    {
        public ReturnCode ReturnCodeResult { get; set; }

        public TestBehaviour(ReturnCode inReturnCodeResult)
        {
            ReturnCodeResult = inReturnCodeResult;
        }

        public override IEnumerator Behave(AIBase theBase, Action<ReturnCode> returnCode)
        {
            returnCode(ReturnCodeResult);
            yield break;
        }
    }
}
