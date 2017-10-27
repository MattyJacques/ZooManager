// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public class Selector : BehaviourBase
    {
        private readonly BehaviourBase[] _behaviours;

        public Selector(BehaviourBase[] behaviours)
        {
            _behaviours = behaviours;
        }

        public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
        {
            foreach (var behaviour in _behaviours)
            {
                var result = ReturnCode.Failure;
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
        }
    }
}
