// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Behaviours.Base
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
