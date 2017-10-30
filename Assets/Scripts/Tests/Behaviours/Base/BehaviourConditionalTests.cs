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
    public class BehaviourConditionalTests
    {
        private AIBase _aiBase;
        private bool ExpectedSuccess = true;

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
        public IEnumerator ConditionalDelegateDoesNotAlterResult_ReturnsFailure()
        {
            var behaviour = new BehaviourConditional(EmptyConditional);

            var returnCode = ReturnCode.Success;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Failure, returnCode);
        }

        [UnityTest]
        public IEnumerator ConditionalDelegateAltersResult_ReturnsAlteredResult()
        {
            var behaviour = new BehaviourConditional(AlteringConditional);

            ExpectedSuccess = true;
            var returnCode = ReturnCode.Failure;

            yield return CoroutineSys.Instance.StartCoroutine(behaviour.Behave(_aiBase, val => { returnCode = val; }));

            Assert.AreEqual(ReturnCode.Success, returnCode);
        }

        private static IEnumerator EmptyConditional(AIBase inAiBase, Action<bool> success)
        {
            yield return null;
        }

        private IEnumerator AlteringConditional(AIBase inAiBase, Action<bool> success)
        {
            success(ExpectedSuccess);
            yield return null;
        }
    }
}

