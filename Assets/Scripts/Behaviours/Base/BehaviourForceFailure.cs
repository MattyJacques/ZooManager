// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Blackboards;

namespace Assets.Scripts.Behaviours.Base
{
    public class BehaviourForceFailure : BehaviourBase
    {
        public override IEnumerator Behave(Blackboard inBlackboard, Action<ReturnCode> returnCode)
        {
            returnCode(ReturnCode.Failure);
            yield break;
        }
    }
}
