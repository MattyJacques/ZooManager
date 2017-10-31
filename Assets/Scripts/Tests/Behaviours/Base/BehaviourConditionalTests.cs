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
    public class BehaviourConditionalTests
    {
        private Blackboard _blackboard;
        private bool _expectedSuccess = true;

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
        public IEnumerator ConditionalDelegateDoesNotAlterResult_ReturnsFailure()
        {
            var behaviour = new BehaviourConditional(EmptyConditional);

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_blackboard, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Failure, returnCode);
        }

        [UnityTest]
        public IEnumerator ConditionalDelegateAltersResult_ReturnsAlteredResult()
        {
            var behaviour = new BehaviourConditional(AlteringConditional);

            _expectedSuccess = true;
            var returnCode = ReturnCode.Failure;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_blackboard, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Success, returnCode);
        }

        private static IEnumerator EmptyConditional(Blackboard inBlackboard, Action<bool> success)
        {
            yield return null;
        }

        private IEnumerator AlteringConditional(Blackboard inBlackboard, Action<bool> success)
        {
            success(_expectedSuccess);
            yield return null;
        }
    }
}

