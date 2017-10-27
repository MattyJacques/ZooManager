// Sifaka Game Studios (C) 2017

using System.Collections;
using System;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public delegate bool ForceFailureDelegate(AIBase theBase);

    public class BehaviourForceFailure : BehaviourBase
    {
        public override IEnumerator Behave(AIBase theBase, Action<ReturnCode> returnCode)
        {
            returnCode(ReturnCode.Failure);
            yield break;
        }
    }
}
