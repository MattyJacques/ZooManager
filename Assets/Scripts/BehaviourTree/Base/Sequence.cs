// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public class Sequence : BehaviourBase
    {
        private readonly BehaviourBase[] _behaviours;

        public Sequence(BehaviourBase[] behaviours)
        {
            _behaviours = behaviours;
        }

        public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
        {
            for (int i = 0; i < _behaviours.Length; i++)
            { 
                ReturnCode result = ReturnCode.Failure;
                yield return CoroutineSys.Instance.StartCoroutine(_behaviours[i].Behave(theBase, val => result = val));

                switch (result)
                {
                    case ReturnCode.Failure:
                        returnCode(ReturnCode.Failure);
                        yield break;
                    case ReturnCode.Success:
                        continue;
                    case ReturnCode.Running:
                        continue;
                    default:
                        returnCode(ReturnCode.Success);
                        yield break;
                }
            }
            returnCode(ReturnCode.Success);
        }
    }
}
