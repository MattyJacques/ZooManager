// Sifaka Game Studios (C) 2017

using System.Collections;
using Assets.Scripts.Blackboards;

namespace Assets.Scripts.Behaviours.Base
{
    public delegate IEnumerator ConditionalDelegate(Blackboard inBlackboard, System.Action<bool> conditionResult);

    public class BehaviourConditional : BehaviourBase
    {
        private readonly ConditionalDelegate _testCondition;

        public BehaviourConditional(ConditionalDelegate testCondition)
        {
            _testCondition = testCondition;
        }

        public override IEnumerator Behave(Blackboard inBlackboard, System.Action<ReturnCode> returnCode)
        {
            var result = false;
            yield return CoroutineSys.Instance.StartCoroutine(_testCondition(inBlackboard, val => result = val)); // Run the test condition (also coroutine for future proofing)

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
