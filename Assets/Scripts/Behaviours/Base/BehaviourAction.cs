// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Behaviours.Base
{
    public delegate IEnumerator ActionDelegate(AIBase theBase, Action<ReturnCode> returnCode);

    public class BehaviourAction : BehaviourBase
    {
        private readonly ActionDelegate _action;

        public BehaviourAction(ActionDelegate action)
        {
            _action = action;
        }

        public override IEnumerator Behave(AIBase theBase, Action<ReturnCode> returnCode)
        {
            var result = ReturnCode.Failure;
            yield return CoroutineSys.Instance.StartCoroutine(_action(theBase, val => result = val));
            returnCode(result);
        }
    }
}
