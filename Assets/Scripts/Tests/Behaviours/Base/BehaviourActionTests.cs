// Sifaka Game Studios (C) 2017

using System;
using System.Collections;
using Assets.Scripts.Behaviours.Base;
using Assets.Scripts.Blackboards;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Assets.Scripts.Tests.Behaviours.Base
{
    [TestFixture]
    public class BehaviourActionTestFixture
    {
        private Blackboard _blackboard;
        private static ReturnCode ExpectedReturnCode = ReturnCode.Running;

        [SetUp]
        public void BeforeTest()
        {
            _blackboard = new Blackboard();
        }

        [TearDown]
        public void AfterTest()
        {
            _blackboard = null;
        }

        [UnityTest]
        public IEnumerator ActionDelegateDoesNotAlterResult_ReturnsFailure()
        {
            var behaviour = new BehaviourAction(EmptyAction);

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_blackboard, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Failure, returnCode);
        }

        [UnityTest]
        public IEnumerator ActionDelegateAltersResult_ReturnsAlteredResult()
        {
            var behaviour = new BehaviourAction(AlteringAction);

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_blackboard, val => { returnCode = val; }));

            Assert.AreEqual(ExpectedReturnCode, returnCode);
        }

        private static IEnumerator EmptyAction(Blackboard inBlackboard, Action<ReturnCode> returnCode)
        {
            yield return null;
        }

        private static IEnumerator AlteringAction(Blackboard inBlackboard, Action<ReturnCode> returnCode)
        {
            returnCode(ExpectedReturnCode);
            yield return null;
        }
    }
}
