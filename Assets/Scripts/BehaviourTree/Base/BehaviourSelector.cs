// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public class BehaviourSelector : BehaviourBase
    {
        private readonly BehaviourBase[] _behaviours;

        public BehaviourSelector(BehaviourBase[] behaviours)
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
                        continue;
                    case ReturnCode.Success:
                        returnCode(ReturnCode.Success);
                        yield break;
                    case ReturnCode.Running:
                        returnCode(ReturnCode.Failure);
                        yield break;
                    default:
                        continue;
                }
            }

            // Final Failure should return a failure
            if (result == ReturnCode.Failure)
            {
                returnCode(result);
            }
        }
    }
}
