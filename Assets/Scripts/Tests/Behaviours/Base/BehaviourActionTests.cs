// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Characters;
using Assets.Scripts.Tests.Characters;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours.Base
{
    [TestFixture]
    public class BehaviourActionTestFixture
    {
        private AIBase _aiBase;
        private static ReturnCode ExpectedReturnCode = ReturnCode.Running;

        [SetUp]
        public void BeforeTest()
        {
            _aiBase = new AIBaseBuilder().Build();
        }

        [TearDown]
        public void AfterTest()
        {
            _aiBase = null;
        }

        [UnityTest]
        public IEnumerator ActionDelegateDoesNotAlterResult_ReturnsFailure()
        {
            var behaviour = new BehaviourAction(EmptyAction);

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Failure, returnCode);
        }

        [UnityTest]
        public IEnumerator ActionDelegateAltersResult_ReturnsAlteredResult()
        {
            var behaviour = new BehaviourAction(AlteringAction);

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, val => { returnCode = val; }));

            Assert.AreEqual(ExpectedReturnCode, returnCode);
        }

        private static IEnumerator EmptyAction(AIBase inAiBase, Action<ReturnCode> returnCode)
        {
            yield return null;
        }

        private static IEnumerator AlteringAction(AIBase inAiBase, Action<ReturnCode> returnCode)
        {
            returnCode(ExpectedReturnCode);
            yield return null;
        }
    }
}
