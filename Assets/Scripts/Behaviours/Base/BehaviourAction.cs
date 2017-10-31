// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Blackboards;

namespace Assets.Scripts.Behaviours.Base
{
    public delegate IEnumerator ActionDelegate(Blackboard inBlackboard, Action<ReturnCode> returnCode);

    public class BehaviourAction : BehaviourBase
    {
        private readonly ActionDelegate _action;

        public BehaviourAction(ActionDelegate action)
        {
            _action = action;
        }

        public override IEnumerator Behave(Blackboard inBlackboard, Action<ReturnCode> returnCode)
        {
            var result = ReturnCode.Failure;
            yield return CoroutineSys.Instance.StartCoroutine(_action(inBlackboard, val => result = val));
            returnCode(result);
        }
    }
}
