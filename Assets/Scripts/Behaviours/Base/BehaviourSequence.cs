﻿// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Behaviours.Base
{
    public class BehaviourSequence : BehaviourBase
    {
        private readonly BehaviourBase[] _behaviours;

        public BehaviourSequence(BehaviourBase[] behaviours)
        {
            _behaviours = behaviours;
        }

        public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
        {
            var result = ReturnCode.Failure;
            foreach (var behaviour in _behaviours)
            {
                yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(theBase, val => result = val));

                switch (result)
                {
                    case ReturnCode.Failure:
                        returnCode(ReturnCode.Failure);
                        yield break;
                    case ReturnCode.Success:
                        continue;
                    case ReturnCode.Running:
                        returnCode(ReturnCode.Failure);
                        yield break;
                    default:
                        returnCode(ReturnCode.Success);
                        yield break;
                }
            }

            if (result != ReturnCode.Success)
            {
                returnCode(ReturnCode.Failure);
            }
            else
            {
                returnCode(result);
            }
        }
    }
}
