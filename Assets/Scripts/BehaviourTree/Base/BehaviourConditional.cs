// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Characters;

namespace Assets.Scripts.BehaviourTree.Base
{
    public delegate IEnumerator ConditionalDelegate(AIBase theBase, System.Action<bool> conditionResult);

    public class BehaviourConditional : BehaviourBase
    {
        private readonly ConditionalDelegate _testCondition;

        public BehaviourConditional(ConditionalDelegate testCondition)
        {
            _testCondition = testCondition;
        }

        public override IEnumerator Behave(AIBase theBase, System.Action<ReturnCode> returnCode)
        {
            var result = false;
            yield return CoroutineSys.Instance.StartCoroutine(_testCondition(theBase, val => result = val)); // Run the test condition (also coroutine for future proofing)

            if (result)
            {
                returnCode(ReturnCode.Success);
            }
            else
            {
                returnCode(ReturnCode.Failure);
            }
        }
    }
}
